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
using System.Collections.Generic;

namespace Stimulsoft.Base
{
    /// <summary>
    /// Describes the class that realizes relations between Data Sources.
    /// </summary>
    public class StiDataRelationSchema
    {
        #region Properties
        /// <summary>
        /// Gets or sets relation name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Parent data source name.
        /// </summary>
        public string ParentSourceName { get; set; }

        /// <summary>
        /// Gets or sets Child data source name.
        /// </summary>
        public string ChildSourceName { get; set; }


        /// <summary>
        /// Gets or sets collection of child column names.
        /// </summary>
        public List<string> ChildColumns { get; set; }


        /// <summary>
        /// Gets or sets collection of parent column names.
        /// </summary>
        public List<string> ParentColumns { get; set; }
        #endregion
    }
}
