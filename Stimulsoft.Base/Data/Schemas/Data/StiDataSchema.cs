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
using System.Linq;
using System.Collections.Generic;
using System.Data;

namespace Stimulsoft.Base
{
    /// <summary>
    /// This class describes a data schema in data source.
    /// </summary>
    public class StiDataSchema : StiObjectSchema
    {
        #region Properties
        /// <summary>
        /// A list of tables in the schema.
        /// </summary>
        public List<StiDataTableSchema> Tables { get; set; }

        /// <summary>
        /// A list of views in the schema.
        /// </summary>
        public List<StiDataTableSchema> Views { get; set; }

        /// <summary>
        /// A list of stored procedures in the schema.
        /// </summary>
        public List<StiDataTableSchema> StoredProcedures { get; set; }

        /// <summary>
        /// A list of sql queries in the schema.
        /// </summary>
        public List<StiDataTableSchema> Queries { get; set; }

        /// <summary>
        /// A list of relations for this schema.
        /// </summary>
        public List<StiDataRelationSchema> Relations { get; set; }


        /// <summary>
        /// A type of the connection from which this schema is created.
        /// </summary>
        public StiConnectionIdent ConnectionIdent { get; set; }
        #endregion

        #region Methods
        public bool IsEmpty()
        {
            return (this.Tables.Count <= 0 &&
                    this.Views.Count <= 0 &&
                    this.StoredProcedures.Count <= 0 &&
                    this.Relations.Count <= 0 &&
                    this.Queries.Count <= 0);
        }

        public DataSet GetDataSet()
        {
            var dataSet = new DataSet();

            #region Tables
            if (this.Tables != null)
            {
                this.Tables.ForEach(table =>
                {
                    var dataTable = new DataTable(table.Name);

                    table.Columns.ForEach(column => dataTable.Columns.Add(column.Name, column.Type));

                    dataSet.Tables.Add(dataTable);
                });
            }
            #endregion

            #region Views
            if (this.Views != null)
            {
                this.Views.ForEach(view =>
                {
                    var dataTable = new DataTable(view.Name);

                    view.Columns.ForEach(column => dataTable.Columns.Add(column.Name, column.Type));

                    dataSet.Tables.Add(dataTable);
                });
            }
            #endregion

            #region StoredProcs
            if (this.StoredProcedures != null)
            {
                this.StoredProcedures.ForEach(proc =>
                {
                    var dataTable = new DataTable(proc.Name);

                    proc.Columns.ForEach(column => dataTable.Columns.Add(column.Name, column.Type));

                    dataSet.Tables.Add(dataTable);
                });
            }
            #endregion

            #region Queries
            if (this.Queries != null)
            {
                this.Queries.ForEach(query =>
                {
                    var dataTable = new DataTable(query.Name);

                    query.Columns.ForEach(column => dataTable.Columns.Add(column.Name, column.Type));

                    dataSet.Tables.Add(dataTable);
                });
            }
            #endregion

            #region Relations
            if (this.Relations != null)
            {
                this.Relations.ForEach(relation =>
                {
                    try
                    {
                        var parentTable = dataSet.Tables[relation.ParentSourceName];
                        var childTable = dataSet.Tables[relation.ChildSourceName];

                        if (parentTable == null || childTable == null) return;

                        var parentColumns = new List<DataColumn>();
                        var childColumns = new List<DataColumn>();

                        foreach (var columnName in relation.ParentColumns)
                        {
                            var column = parentTable.Columns[columnName];
                            if (column == null) return;
                            parentColumns.Add(column);
                        }

                        foreach (var columnName in relation.ChildColumns)
                        {
                            var column = childTable.Columns[columnName];
                            if (column == null) return;
                            childColumns.Add(column);
                        }

                        dataSet.Relations.Add(new DataRelation(relation.Name, parentColumns.ToArray(), childColumns.ToArray(), false));
                    }
                    catch
                    {

                    }
                });
            }
            #endregion

            return dataSet;
        }

        public StiDataSchema Sort()
        {
            this.Tables = this.Tables.OrderBy(t => t.Name).ToList();
            this.Views = this.Views.OrderBy(v => v.Name).ToList();
            this.Queries = this.Queries.OrderBy(q => q.Name).ToList();
            this.StoredProcedures = this.StoredProcedures.OrderBy(s => s.Name).ToList();
            this.Relations = this.Relations.OrderBy(r => r.Name).ToList();

            return this;
        }
        #endregion

        public StiDataSchema(): this(StiConnectionIdent.Unspecified)
        {
            
        }

        public StiDataSchema(StiConnectionIdent ident)
        {
            this.ConnectionIdent = ident;
            this.Tables = new List<StiDataTableSchema>();
            this.Views = new List<StiDataTableSchema>();
            this.Queries = new List<StiDataTableSchema>();
            this.StoredProcedures = new List<StiDataTableSchema>();
            this.Relations = new List<StiDataRelationSchema>();
        }
    }
}