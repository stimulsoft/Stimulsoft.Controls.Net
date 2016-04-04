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
using System.Data;

namespace Stimulsoft.Base
{
    /// <summary>
    /// This class describes a table in data schema.
    /// </summary>
    public class StiDataTableSchema : StiObjectSchema
    {
        #region Properties
        /// <summary>
        /// A list of the columns.
        /// </summary>
        public List<StiDataColumnSchema> Columns { get; set; }

        /// <summary>
        /// A list of the parameters.
        /// </summary>
        public List<StiDataParameterSchema> Parameters { get; set; }

        /// <summary>
        /// A query string.
        /// </summary>
        public string Query { get; set; }
        #endregion

        #region Methods
        public static StiDataTableSchema NewTableOrView(string name, StiSqlDataConnector connector = null, string query = null)
        {
            return NewTable(name, connector, query);
        }

        public static StiDataTableSchema NewTable(string name, StiSqlDataConnector connector = null, string query = null)
        {
            return new StiDataTableSchema(name, query ?? StiTableQuery.Get(connector).GetSelectQuery(name));
        }

        public static StiDataTableSchema NewView(string name, StiSqlDataConnector connector = null, string query = null)
        {
            return new StiDataTableSchema(name, query ?? StiTableQuery.Get(connector).GetSelectQuery(name));
        }

        public static StiDataTableSchema NewProcedure(string name, StiSqlDataConnector connector = null, string query = null)
        {
            return new StiDataTableSchema(name, query ?? StiTableQuery.Get(connector).GetExecuteQuery(name));
        }
        #endregion

        public StiDataTableSchema()
        {
            this.Columns = new List<StiDataColumnSchema>();
            this.Parameters = new List<StiDataParameterSchema>();
        }

        public StiDataTableSchema(string name, string query = null)
            : this()
        {
            this.Name = name;
            this.Query = query;
        }

        public StiDataTableSchema(DataTable table)
        {
            this.Name = table.TableName;

            this.Columns = new List<StiDataColumnSchema>();
            foreach (DataColumn column in table.Columns)
            {
                this.Columns.Add(new StiDataColumnSchema
                {
                    Name = column.ColumnName, 
                    Type = column.DataType
                });
            }
            
            this.Parameters = new List<StiDataParameterSchema>();
        }
    }
}
