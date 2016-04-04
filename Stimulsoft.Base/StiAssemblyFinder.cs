#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports												}
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
{	TRADE SECRETS OF Stimulsoft										}
{																	}
{	CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON		}
{	ADDITIONAL RESTRICTIONS.										}
{																	}
{*******************************************************************}
*/
#endregion Copyright (C) 2003-2016 Stimulsoft

using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Reflection;

namespace Stimulsoft.Base
{
	/// <summary>
	/// Class contains statistic methods to find assemblies.
	/// </summary>
	public sealed class StiAssemblyFinder
	{
		/// <summary>
		/// Gets assembly by it name.
		/// </summary>
		public static Assembly GetAssembly(string assemblyName)
		{
            var resCurrentDirectory = Directory.GetCurrentDirectory();

            try
            {
                var location = typeof(StiAssemblyFinder).Assembly.Location;
                location = Path.GetDirectoryName(location);
                Directory.SetCurrentDirectory(location);

                var asmName = assemblyName;
                if (asmName.ToLower().EndsWith(".dll", StringComparison.InvariantCulture)) asmName = Path.GetFileNameWithoutExtension(assemblyName);
                else if (asmName.ToLower().EndsWith(".exe", StringComparison.InvariantCulture)) asmName = Path.GetFileNameWithoutExtension(assemblyName);

                #region Search name in assemblies in current domain
                var assembly = string.IsNullOrEmpty(asmName) ? null : AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == asmName);
                if (assembly != null) return assembly;
                #endregion

                #region Try with Assembly.Load
                try
                {
                    return Assembly.Load(asmName);
                }
                catch (FileNotFoundException)
                {
                }
                #endregion

                #region Try with Assembly.Load (with specified version)
                try
                {
                    if (asmName.ToLowerInvariant().StartsWith("stimulsoft", StringComparison.InvariantCulture))
                    {
                        var version = typeof(StiAssemblyFinder).AssemblyQualifiedName;
                        var index = version.IndexOf(",", StringComparison.InvariantCulture);
                        version = version.Substring(index + 1);
                        version = version.Replace("Stimulsoft.Base", asmName);
                        return Assembly.Load(version);
                    }
                }
                catch (FileNotFoundException)
                {
                }
                #endregion

                #region Assembly.LoadFrom
                try
                {
                    if (File.Exists(assemblyName))
                    {
                        return Assembly.LoadFrom(assemblyName);
                    }
                }
                catch (FileNotFoundException)
                {
                }
                #endregion

                #region Assembly.Load
                try
                {
                    return Assembly.Load(assemblyName);
                }
                catch (FileNotFoundException)
                {
                }
                #endregion

#pragma warning disable 618,612
                return Assembly.LoadWithPartialName(asmName);
            }
            finally
            {
                Directory.SetCurrentDirectory(resCurrentDirectory);
            }
		}
	}
}
