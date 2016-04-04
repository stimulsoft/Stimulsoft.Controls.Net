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
using System.Linq;
using System.Text.RegularExpressions;
using Stimulsoft.Base;

namespace Stimulsoft.Base
{
    public class StiOracleManagedConnector : StiOracleConnector
    {
        #region Properties
        /// <summary>
        /// Gets the package identificator for this connector.
        /// </summary>
        public override string[] NuGetPackages
        {
            get
            {
                return new string[] { "Oracle.ManagedDataAccess" };
            }
        }
        #endregion

        public StiOracleManagedConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "Oracle.ManagedDataAccess.dll";
            this.TypeConnection = "Oracle.ManagedDataAccess.Client.OracleConnection";
            this.TypeDataAdapter = "Oracle.ManagedDataAccess.Client.OracleDataAdapter";
            this.TypeCommand = "Oracle.ManagedDataAccess.Client.OracleCommand";
            this.TypeParameter = "Oracle.ManagedDataAccess.Client.OracleParameter";
            this.TypeDbType = "Oracle.ManagedDataAccess.Client.OracleDbType";
            this.TypeCommandBuilder = "Oracle.ManagedDataAccess.Client.OracleCommandBuilder";
            this.TypeConnectionStringBuilder = "Oracle.ManagedDataAccess.Client.OracleConnectionStringBuilder";
            this.TypeDataSourceEnumerator = "Oracle.ManagedDataAccess.Client.OracleDataSourceEnumerator";
        }
    }
}