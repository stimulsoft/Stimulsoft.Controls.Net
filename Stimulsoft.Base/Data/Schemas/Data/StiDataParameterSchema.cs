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

namespace Stimulsoft.Base
{
    /// <summary>
    /// This class describes a sql parameter in the stored proc.
    /// </summary>
    public class StiDataParameterSchema : StiObjectSchema
    {
        #region Properties
        /// <summary>
        /// Type of this parameter.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Value of this parameter.
        /// </summary>
        public object Value { get; set; }
        #endregion

        public StiDataParameterSchema()
        {
            
        }

        public StiDataParameterSchema(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
