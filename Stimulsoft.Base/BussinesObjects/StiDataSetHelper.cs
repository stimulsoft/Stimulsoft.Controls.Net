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
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Stimulsoft.Base.BusinessObjects
{
    public static class StiDataSetHelper
    {
        #region Methods.Helper
        private static string CheckName(string name)
        {
            StringBuilder builder = new StringBuilder(name);
            builder.Replace(" ", "_x0020_");
            builder.Replace("@", "_x0040_");
            builder.Replace("~", "_x007e_");
            builder.Replace("$", "_x0024_");
            builder.Replace("#", "_x0023_");
            builder.Replace("%", "_x0025_");
            builder.Replace("&", "_x0026_");
            builder.Replace("*", "_x002A_");
            builder.Replace("^", "_x005E_");
            builder.Replace("(", "_x0028_");
            builder.Replace(")", "_x0029_");
            builder.Replace("!", "_x0021_");

            return builder.ToString();
        }

        private static bool CheckIsFixedColumn(IEnumerable<StiFixedColumnInfo> fixedColumnsInfo, DataTable table, DataColumn column)
        {
            if (fixedColumnsInfo != null)
            {
                foreach (StiFixedColumnInfo info in fixedColumnsInfo)
                {
                    if (info.Table == table && info.FixedColumn == column)
                        return true;
                }
            }

            return false;
        }
        #endregion

        #region Method.Save
        private static byte[] Save(DataSet dataSet, IEnumerable<StiFixedColumnInfo> fixedColumnsInfo)
        {
            CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            Dictionary<string, string[]> tablesColumns = new Dictionary<string, string[]>();
            Dictionary<string, string> reduction = new Dictionary<string, string>();

            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;

            string dataSetName = string.IsNullOrEmpty(dataSet.DataSetName) ? "dataSet1" : dataSet.DataSetName;

            //bool containsChildRelations = false;
            writer.WriteStartElement("StiDataSet");
            writer.WriteAttributeString("name", CheckName(dataSetName));
            writer.WriteAttributeString("version", "1.1");

            #region Записываем информацию о сокращениях имен таблиц, полей, релейшенов. "Reduction"
            List<string> names = new List<string>();
            foreach (DataTable table in dataSet.Tables)
            {
                names.Add(table.TableName);

                foreach (DataColumn column in table.Columns)
                {
                    names.Add(column.ColumnName);
                }

                foreach (DataRelation relation in table.ChildRelations)
                {
                    names.Add(relation.RelationName);
                }

                foreach (DataRelation relation in table.ParentRelations)
                {
                    names.Add(relation.RelationName);
                }
            }

            writer.WriteStartElement("Reduction");
            foreach (string name in names)
            {
                if (!reduction.ContainsKey(name))
                {
                    string content = ("i" + reduction.Count.ToString());
                    reduction.Add(name, content);

                    writer.WriteStartElement(CheckName(name));
                    writer.WriteString(content);
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();

            names = null;
            #endregion

            #region Записываем информацию о таблицах "TablesInfo"
            writer.WriteStartElement("TablesInfo");

            foreach (DataTable table in dataSet.Tables)
            {
                //if (table.ChildRelations.Count > 0)
                    //containsChildRelations = true;

                int index = 0;
                string[] columnsStr = new string[table.Columns.Count];

                writer.WriteStartElement(reduction[table.TableName]);

                foreach (DataColumn column in table.Columns)
                {
                    writer.WriteStartElement(reduction[column.ColumnName]);

                    if (CheckIsFixedColumn(fixedColumnsInfo, table, column))
                        writer.WriteAttributeString("IsFixed", "true");

                    columnsStr[index++] = column.ColumnName;
                    writer.WriteString(column.DataType.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                tablesColumns.Add(table.TableName, columnsStr);
            }

            writer.WriteEndElement();
            #endregion

            #region Записываем сами поля каждой таблицы "Contents"
            writer.WriteStartElement("Contents");

            List<string> colsIndex = new List<string>();
            foreach (DataTable table in dataSet.Tables)
            {
                string[] columnsStr = tablesColumns[table.TableName];
                int countColumns = columnsStr.Length;

                foreach (DataRow row in table.Rows)
                {
                    colsIndex.Clear();

                    int index = 0;
                    while (index < countColumns)
                    {
                        string columnName = columnsStr[index++];
                        object value = row[columnName];

                        if (!(value is DBNull) && (value != null || (value is string && ((string)value).Length != 0)))
                            colsIndex.Add(columnName);
                    }

                    if (colsIndex.Count > 0)
                    {
                        writer.WriteStartElement(reduction[table.TableName]);

                        index = 0;
                        while (index < colsIndex.Count)
                        {
                            string columnName = colsIndex[index++];
                            object value = row[columnName];

                            writer.WriteStartElement(reduction[columnName]);
                            writer.WriteValue(value);
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }
                }
            }

            writer.WriteEndElement();
            #endregion

            #region Записываем информацию о всех релейшенах DataSet
            writer.WriteStartElement("RelationsInfo");

            foreach (DataRelation relation in dataSet.Relations)
            {
                writer.WriteStartElement("Relation");
                writer.WriteElementString("RelationName", reduction[relation.RelationName]);
                writer.WriteElementString("ParentTable", reduction[relation.ParentTable.TableName]);
                writer.WriteElementString("ChildTable", reduction[relation.ChildTable.TableName]);

                #region ParentColumns
                writer.WriteStartElement("ParentColumns");

                foreach (DataColumn parentColumn in relation.ParentColumns)
                {
                    writer.WriteStartElement(reduction[parentColumn.ColumnName]);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                #endregion

                #region ChildColumns
                writer.WriteStartElement("ChildColumns");

                foreach (DataColumn childColumn in relation.ChildColumns)
                {
                    writer.WriteStartElement(reduction[childColumn.ColumnName]);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                #endregion

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            #endregion

            writer.WriteEndElement();
            writer.Flush();

            byte[] buffer = stream.ToArray();

            stream.Close();
            stream.Dispose();
            writer.Close();
            stream = null;
            writer = null;
            tablesColumns = null;

            System.Threading.Thread.CurrentThread.CurrentCulture = culture;

            return buffer;
        }
        #endregion

        #region Methods
        public static void SaveDataSet(DataSet dateSet)
        {
            SaveDataSet(dateSet, null);
        }

        public static void SaveDataSet(DataSet dateSet, IEnumerable<StiFixedColumnInfo> fixedColumnsInfo)
        {
            if (dateSet != null || dateSet.Tables.Count > 0)
            {
                using (SaveFileDialog fileDialog = new SaveFileDialog())
                {
                    fileDialog.Filter = ".data|*.data";
                    fileDialog.FileName = "Demo";

                    if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        byte[] buffer = Save(dateSet, fixedColumnsInfo);
                        File.WriteAllBytes(fileDialog.FileName, buffer);
                    }
                }
            }
        }

        public static byte[] SaveDataSetToByteArray(DataSet dateSet)
        {
            return SaveDataSetToByteArray(dateSet, null);
        }

        public static byte[] SaveDataSetToByteArray(DataSet dateSet, IEnumerable<StiFixedColumnInfo> fixedColumnsInfo)
        {
            byte[] result = null;
            if (dateSet != null && dateSet.Tables.Count > 0)
            {
                result = Save(dateSet, fixedColumnsInfo);
            }

            return result;
        }

        public static string SaveDataSetToString(DataSet dateSet)
        {
            return SaveDataSetToString(dateSet, null);
        }

        public static string SaveDataSetToString(DataSet dateSet, IEnumerable<StiFixedColumnInfo> fixedColumnsInfo)
        {
            byte[] buffer = null;
            if (dateSet != null && dateSet.Tables.Count > 0)
            {
                buffer = Save(dateSet, fixedColumnsInfo);
            }

            return System.Text.UTF8Encoding.UTF8.GetString(buffer);
        }
        #endregion
    }
}