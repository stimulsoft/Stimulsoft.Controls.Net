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
using System.Reflection;
using Stimulsoft.Base;

namespace Stimulsoft.Base
{
    public class StiTableQuery
    {
        #region Methods
        private string CorrectName(string name)
        {
            var specifiedName = connector != null ? connector.GetDatabaseSpecificName(name) : null;

            return name.Trim().IndexOfAny(new[] { ' ', '.', '-' }) != -1 ? specifiedName : name;
        }

        public string GetName(string schema, string table)
        {
            return string.IsNullOrWhiteSpace(schema) ? CorrectName(table) : string.Format("{0}.{1}", CorrectName(schema), CorrectName(table));
        }

        public string GetSelectQuery(string table)
        {
            return GetSelectQuery(null, table);
        }

        public string GetSelectQuery(string schema, string table)
        {
            return string.Format("select * from {0}", GetName(schema, table));
        }

        public string GetExecuteQuery(string table)
        {
            return GetExecuteQuery(null, table);
        }

        public string GetExecuteQuery(string schema, string table)
        {
            return string.Format("execute {0}", GetName(schema, table));
        }

        public string GetCallQuery(string table)
        {
            return GetCallQuery(null, table);
        }

        public string GetCallQuery(string schema, string table)
        {
            return string.Format("call {0}", GetName(schema, table));
        }

        public string GetProcQuery(string table)
        {
            return GetProcQuery(null, table);
        }

        public string GetProcQuery(string schema, string table)
        {
            return GetName(schema, table);
        }
        #endregion

        #region Methods.Get
        public static StiTableQuery Get(StiSqlDataConnector connector)
        {
            return new StiTableQuery(connector);
        }
        #endregion

        #region Fields
        private StiSqlDataConnector connector;
        #endregion

        public StiTableQuery(StiSqlDataConnector connector)
        {
            this.connector = connector;
        }
    }
}