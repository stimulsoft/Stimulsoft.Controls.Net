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
    public abstract class StiFileDataConnector : StiDataConnector
    {
        #region Properties
        /// <summary>
        /// A type of the file which can be processed with this connection helper.
        /// </summary>
        public abstract StiFileType FileType { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns schema object which contains information about structure of the database. Schema returned start at specified root element (if it applicable).
        /// </summary>
        public StiDataSchema RetrieveSchema(StiFileDataOptions options)
        {
            if (options == null) return null;

            var dataSet = GetDataSet(options);
            if (dataSet == null) return null;

            var schema = new StiDataSchema(this.ConnectionIdent);

            #region Tables
            foreach (DataTable table in dataSet.Tables)
            {
                var tableSchema = StiDataTableSchema.NewTable(table.TableName);

                foreach (DataColumn column in table.Columns)
                {
                    tableSchema.Columns.Add(new StiDataColumnSchema
                    {
                        Name = column.ColumnName,
                        Type = column.DataType
                    });
                }
                schema.Tables.Add(tableSchema);
            }
            #endregion

            #region Relations
            foreach (DataRelation relation in dataSet.Relations)
            {
                schema.Relations.Add(new StiDataRelationSchema
                {
                    Name = relation.RelationName,
                    ParentSourceName = relation.ParentTable.TableName,
                    ChildSourceName = relation.ChildTable.TableName,
                    ParentColumns = relation.ParentColumns.Select(col => col.ColumnName).ToList(),
                    ChildColumns = relation.ChildColumns.Select(col => col.ColumnName).ToList()
                });
            }
            #endregion

            schema.Relations = schema.Relations.OrderBy(e => e.Name).ToList();
            schema.Tables = schema.Tables.OrderBy(e => e.Name).ToList();

            return schema;
        }


        /// <summary>
        /// Returns StiTestConnectionResult that is the information of whether the connection string specified in this class is correct.
        /// </summary>
        /// <returns>The result of testing the connection string.</returns>
        public StiTestConnectionResult TestConnection(StiFileDataOptions options)
        {
            try
            {
                if (options == null || options.Content == null || options.Content.Length == 0)
                    return StiTestConnectionResult.MakeWrong(string.Format("{0} content is not found!", this.Name));

                var dataSet = GetDataSet(options);
                if (dataSet == null)
                    return StiTestConnectionResult.MakeWrong(string.Format("{0} content is not recognized!", this.Name));

                return StiTestConnectionResult.MakeFine();
            }
            catch (Exception e)
            {
                return StiTestConnectionResult.MakeWrong(e);
            }
        }



        /// <summary>
        /// Returns DataSet based on specified options.
        /// </summary>
        public abstract DataSet GetDataSet(StiFileDataOptions options);
        #endregion

        #region Methods.Static
        public static StiFileDataConnector Get(StiConnectionIdent ident)
        {
            switch (ident)
            {
                case StiConnectionIdent.CsvDataSource:
                    return StiCsvConnector.Get();

                case StiConnectionIdent.DBaseDataSource:
                    return StiDBaseConnector.Get();

                case StiConnectionIdent.ExcelDataSource:
                    return StiExcelConnector.Get();

                case StiConnectionIdent.JsonDataSource:
                    return StiJsonConnector.Get();

                case StiConnectionIdent.XmlDataSource:
                    return StiXmlConnector.Get();

                default:
                    return null;
            }
        }
        #endregion

        protected StiFileDataConnector()
        {
        }
    }
}
