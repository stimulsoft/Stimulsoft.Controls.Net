#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports												}
{	                         										}
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
using System.Collections.Generic;

namespace Stimulsoft.Base
{
    public class StiNuGetPackageInfo
    {
        #region Properties
        public Uri IconUrl
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        // Author(s)
        public string Authors
        {
            get;
            set;
        }

        // Downloads
        public int DownloadCount
        {
            get;
            set;
        }

        // License
        public Uri LicenseUrl
        {
            get;
            set;
        }


        // Project URL
        public Uri ProjectUrl
        {
            get;
            set;
        }

        // Report Abuse
        public Uri ReportAbuseUrl
        {
            get;
            set;
        }

        // Tags
        public string Tags
        {
            get;
            set;
        }

        public string Version
        {
            get;
            set;
        }

        public bool IsLatestVersion
        {
            get;
            set;
        }


        public string Dependencies
        {
            get;
            set;
        }


        public List<StiPackageDependency> DependencySets = new List<StiPackageDependency>();

        #endregion

        #region Methods.override
        public override string ToString()
        {
            return string.Format("Version={0}, Title={1}, IsLatestVersion={2}", this.Version, this.Title, this.IsLatestVersion);
        }
        #endregion
    }
}