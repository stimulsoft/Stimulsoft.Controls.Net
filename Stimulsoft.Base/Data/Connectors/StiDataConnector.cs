#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports  											}
{																	}
{	Copyright (C) 2003-2016 Stimulsoft     							}
{	ALL RIGHTS RESERVED												}
{																	}
{	The entire contents of this file is protected by U.S. and		}
{	International Copyright Laws. Unauthorized reproduction,		}
{	reverse-engineering, and distribution of all or any portion of	}
{	the code contained in this file is strictly prohibited and may	}
{	result in severe civil and criminal penalties and will be		}
{	prosecuted to the maximum extent possible under the law.		}
{																	}
{	RESTRICTIONS													}
{																	}
{	THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES			}
{	ARE CONFIDENTIAL AND PROPRIETARY								}
{	TRADE SECRETS OF STIMULSOFT										}
{																	}
{	CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON		}
{	ADDITIONAL RESTRICTIONS.										}
{																	}
{*******************************************************************}
*/
#endregion Copyright (C) 2003-2016 Stimulsoft

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using Stimulsoft.Base;

namespace Stimulsoft.Base
{
    public abstract class StiDataConnector
    {
        #region Properties.Static
        /// <summary>
        /// If true then database information will be required from database with help of DataAdapter (instead Connection.GetSchema).
        /// </summary>
        public static bool AdvancedRetrievalModeOfDatabaseSchema {get;set;}
        #endregion

        #region Properties
        /// <summary>
        /// Gets a type of the connector.
        /// </summary>
        public abstract StiConnectionIdent ConnectionIdent { get; }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public abstract StiConnectionOrder ConnectionOrder { get; }

        /// <summary>
        /// The name of this connector.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the package identificator for this connector.
        /// </summary>
        public virtual string[] NuGetPackages
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Get a value which indicates that this data connector can be used now.
        /// </summary>
        public abstract bool IsAvailable { get; }
        #endregion

        #region Methods
        public virtual void ResetSettings()
        {
            
        }

        /// <summary>
        /// Return an array of the data connectors which can be used also to access data for this type of the connector.
        /// </summary>
        public virtual StiDataConnector[] GetFamilyConnectors()
        {
            return new[]
            {
                this
            };
        }

        public static List<StiNuGetPackageInfo> GetPackageInfoFromNuGet(string packageId)
        {
            if (string.IsNullOrEmpty(packageId))
                throw new NotSupportedException("This connector does not support downloading package from NuGet!");

            var tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var assembly = StiAssemblyFinder.GetAssembly("NuGet.Core.dll");

            var typePackageRepositoryFactory = assembly.GetType("NuGet.PackageRepositoryFactory");
            var valueDefault = typePackageRepositoryFactory.GetProperty("Default").GetValue(null, null);
            var repository = Reflection.InvokeMethod(valueDefault, "CreateRepository", "https://packages.nuget.org/api/v2");


            var packagesList = Reflection.InvokeMethod(repository, "FindPackagesById", packageId) as IEnumerable;

            var packages = new List<object>();
            var typePackageExtensions = assembly.GetType("NuGet.PackageExtensions");

            var result = new List<StiNuGetPackageInfo>();

            foreach (var item in packagesList)
            {
                var type = item.GetType();
                string version = type.GetProperty("Version").GetValue(item, null) as string;

                if (packageId == "mongocsharpdriver" && version != "1.10.1")
                    continue;

                var info = new StiNuGetPackageInfo
                {
                    IconUrl = type.GetProperty("IconUrl").GetValue(item, null) as Uri,
                    Title = type.GetProperty("Title").GetValue(item, null) as string,
                    Description = type.GetProperty("Description").GetValue(item, null) as string,
                    Authors = type.GetProperty("Authors").GetValue(item, null) as string,
                    LicenseUrl = type.GetProperty("LicenseUrl").GetValue(item, null) as Uri,
                    DownloadCount = (int)type.GetProperty("DownloadCount").GetValue(item, null),
                    ProjectUrl = type.GetProperty("ProjectUrl").GetValue(item, null) as Uri,
                    ReportAbuseUrl = type.GetProperty("ReportAbuseUrl").GetValue(item, null) as Uri,
                    Tags = type.GetProperty("Tags").GetValue(item, null) as string,
                    Version = version,
                    IsLatestVersion = (bool)type.GetProperty("IsLatestVersion").GetValue(item, null),
                    Dependencies = type.GetProperty("Dependencies").GetValue(item, null) as string,
                };
                
                var dependencySets = type.GetProperty("DependencySets").GetValue(item, null) as IEnumerable;
                if (dependencySets != null)
                {
                    foreach (var dependencySet in dependencySets)
                    {
                        var dependencies = dependencySet.GetType().GetProperty("Dependencies").GetValue(dependencySet, null) as IEnumerable;
                        if (dependencies != null)
                        {
                            foreach (var dep in dependencies)
                            {
                                var packageDependencySet = new StiPackageDependency
                                {
                                    ID = dep.GetType().GetProperty("Id").GetValue(dep, null) as string,
                                    VersionSpec = dep.GetType().GetProperty("VersionSpec").GetValue(dep, null).ToString()
                                };

                                info.DependencySets.Add(packageDependencySet);
                            }
                        }
                    }
                }

                result.Add(info);
            }

            // Сортируем по версии, начиная с последней
            return result.OrderByDescending(x => x.Version).ToList();
        }

        public static void DownloadPackageFromNuGet(string assemblyPath, string nuGetPackageId)
        {
            if (string.IsNullOrEmpty(nuGetPackageId))
                throw new NotSupportedException("This connector does not support downloading package from NuGet!");

            var tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var assembly = StiAssemblyFinder.GetAssembly("NuGet.Core.dll");

            //var repository = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
            var typePackageRepositoryFactory = assembly.GetType("NuGet.PackageRepositoryFactory");
            var valueDefault = typePackageRepositoryFactory.GetProperty("Default").GetValue(null, null);
            var repository = Reflection.InvokeMethod(valueDefault, "CreateRepository", "https://packages.nuget.org/api/v2");


            //var packages = repository.FindPackagesById(NuGetPackageId).Where(item => item.IsReleaseVersion()).ToList();
            var packagesList = Reflection.InvokeMethod(repository, "FindPackagesById", nuGetPackageId) as IEnumerable;

            //var repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
            //var packages = repo.FindPackagesById(NuGetPackageId).Where(item => item.IsReleaseVersion()).ToList();

            var packages = new List<object>();
            var typePackageExtensions = assembly.GetType("NuGet.PackageExtensions");

            foreach (var item in packagesList)
            {
                var methodIsReleaseVersion = typePackageExtensions.GetMethod("IsReleaseVersion");

                var result = (bool)methodIsReleaseVersion.Invoke(null, new[] { item });
                if (result) packages.Add(item);
            }


            //var packageRequired = packages.Last();
            var packageRequired = packages.Last();
            if (packageRequired != null)
            {
                //var packageManager = new PackageManager(repository, tempFolder);
                var packageManager = Reflection.Create(assembly, "NuGet.PackageManager", repository, tempFolder);

                //packageManager.InstallPackage(packageRequired, true, false);
                Reflection.InvokeMethod(packageManager, "InstallPackage", packageRequired, true, false);

                //var allLibs = packageRequired2.GetLibFiles().ToList();
                var methodGetLibFiles = typePackageExtensions.GetMethod("GetLibFiles");
                var allLibs = methodGetLibFiles.Invoke(null, new[] { packageRequired }) as IEnumerable;
                var libs = new List<object>();

                foreach (var lib in allLibs)
                {
                    var propTargetFramework = Reflection.GetPropertyValue(lib, "TargetFramework");
                    if (propTargetFramework == null) continue;

                    var propVersion = Reflection.GetPropertyValue(propTargetFramework, "Version") as Version;
                    if (propVersion == null || propVersion != Version.Parse("4.0")) continue;

                    libs.Add(lib);
                }

                if (libs.Count == 0) libs.AddRange(allLibs.Cast<object>());

                foreach (var lib in libs)
                {
                    var packageId = Reflection.GetPropertyValue(packageRequired, "Id");//packageRequired.Id
                    var packageVersion = Reflection.GetPropertyValue(packageRequired, "Version");//packageRequired.Version
                    var libPath = Reflection.GetPropertyValue(lib, "Path");//lib.Path
                    var libEffectivePath = Reflection.GetPropertyValue(lib, "EffectivePath");//lib.EffectivePath

                    if (packageId == null || packageVersion == null || libPath == null || libEffectivePath == null)
                        continue;

                    var packageIdStr = packageId.ToString();
                    var packageVersionStr = packageVersion.ToString();
                    var libPathStr = libPath.ToString();
                    var libEffectivePathStr = libEffectivePath.ToString();

                    var filePath = Path.Combine(tempFolder, string.Format("{0}.{1}", packageIdStr, packageVersionStr), libPathStr);
                    var assemblyFileName = Path.Combine(assemblyPath, Path.GetFileName(libEffectivePathStr));

                    var excludedFiles = new List<string>
                    {
                        "db2dascmn64.dll",
                        "db2g11n64.dll",
                        "db2genreg64.dll",
                        "db2install64.dll",
                        "db2locale64.dll",
                        "db2osse64.dll",
                        "db2osse_db364.dll",
                        "db2sdbin64.dll",
                        "db2sys64.dll",
                        "db2trcapi64.dll",
                        "db2wint64.dll",
                        "ibm.data.db2.asp.dll",
                        "ibm.data.db2.entity.dll",
                        "npgsql.resources.dll"
                    };

                    if (Path.GetExtension(assemblyFileName).ToLowerInvariant() == ".dll" && !excludedFiles.Exists(item => item == Path.GetFileName(assemblyFileName).ToLowerInvariant()))
                    {
                        File.Copy(filePath, assemblyFileName, true);
                    }
                }
            }

            Directory.Delete(tempFolder, true);
        }

        public static void DownloadPackageFromNuGet(string assemblyPath, string nuGetPackageId, string version)
        {
            if (string.IsNullOrEmpty(nuGetPackageId))
                throw new NotSupportedException("This connector does not support downloading package from NuGet!");

            var tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var assembly = StiAssemblyFinder.GetAssembly("NuGet.Core.dll");

            //var repository = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
            var typePackageRepositoryFactory = assembly.GetType("NuGet.PackageRepositoryFactory");
            var valueDefault = typePackageRepositoryFactory.GetProperty("Default").GetValue(null, null);
            var repository = Reflection.InvokeMethod(valueDefault, "CreateRepository", "https://packages.nuget.org/api/v2");


            //var packages = repository.FindPackagesById(NuGetPackageId).Where(item => item.IsReleaseVersion()).ToList();
            var packagesList = Reflection.InvokeMethod(repository, "FindPackagesById", nuGetPackageId) as IEnumerable;

            //var repo = PackageRepositoryFactory.Default.CreateRepository("https://packages.nuget.org/api/v2");
            //var packages = repo.FindPackagesById(NuGetPackageId).Where(item => item.IsReleaseVersion()).ToList();

            object packageRequired = null;
            var typePackageExtensions = assembly.GetType("NuGet.PackageExtensions");

            foreach (var item in packagesList)
            {
                var propVersion = item.GetType().GetProperty("Version");

                var versionValue = (string)propVersion.GetValue(item, null);
                if (versionValue == version)
                {
                    packageRequired = item;
                    break;
                }
            }

            if (packageRequired != null)
            {
                //var packageManager = new PackageManager(repository, tempFolder);
                var packageManager = Reflection.Create(assembly, "NuGet.PackageManager", repository, tempFolder);

                //packageManager.InstallPackage(packageRequired, true, false);
                Reflection.InvokeMethod(packageManager, "InstallPackage", packageRequired, true, false);

                //var allLibs = packageRequired2.GetLibFiles().ToList();
                var methodGetLibFiles = typePackageExtensions.GetMethod("GetLibFiles");
                var allLibs = methodGetLibFiles.Invoke(null, new[] { packageRequired }) as IEnumerable;
                var libs = new List<object>();

                foreach (var lib in allLibs)
                {
                    var propTargetFramework = Reflection.GetPropertyValue(lib, "TargetFramework");
                    if (propTargetFramework == null) continue;

                    var propVersion = Reflection.GetPropertyValue(propTargetFramework, "Version") as Version;
                    if (propVersion == null || propVersion != Version.Parse("4.0")) continue;

                    libs.Add(lib);
                }

                if (libs.Count == 0) libs.AddRange(allLibs.Cast<object>());

                foreach (var lib in libs)
                {
                    var packageId = Reflection.GetPropertyValue(packageRequired, "Id");//packageRequired.Id
                    var packageVersion = Reflection.GetPropertyValue(packageRequired, "Version");//packageRequired.Version
                    var libPath = Reflection.GetPropertyValue(lib, "Path");//lib.Path
                    var libEffectivePath = Reflection.GetPropertyValue(lib, "EffectivePath");//lib.EffectivePath

                    if (packageId == null || packageVersion == null || libPath == null || libEffectivePath == null)
                        continue;

                    var packageIdStr = packageId.ToString();
                    var packageVersionStr = packageVersion.ToString();
                    var libPathStr = libPath.ToString();
                    var libEffectivePathStr = libEffectivePath.ToString();

                    var filePath = Path.Combine(tempFolder, string.Format("{0}.{1}", packageIdStr, packageVersionStr), libPathStr);
                    var assemblyFileName = Path.Combine(assemblyPath, Path.GetFileName(libEffectivePathStr));

                    var excludedFiles = new List<string>
                    {
                        "db2dascmn64.dll",
                        "db2g11n64.dll",
                        "db2genreg64.dll",
                        "db2install64.dll",
                        "db2locale64.dll",
                        "db2osse64.dll",
                        "db2osse_db364.dll",
                        "db2sdbin64.dll",
                        "db2sys64.dll",
                        "db2trcapi64.dll",
                        "db2wint64.dll",
                        "ibm.data.db2.asp.dll",
                        "ibm.data.db2.entity.dll",
                        "npgsql.resources.dll"
                    };

                    if (Path.GetExtension(assemblyFileName).ToLowerInvariant() == ".dll" && !excludedFiles.Exists(item => item == Path.GetFileName(assemblyFileName).ToLowerInvariant()))
                    {
                        File.Copy(filePath, assemblyFileName, true);
                    }
                }
            }

            Directory.Delete(tempFolder, true);
        }
        #endregion
    }
}
