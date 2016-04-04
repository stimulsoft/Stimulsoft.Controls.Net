#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports 									            }
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
using System.IO;

namespace Stimulsoft.Base.Excel
{
    public static class StiExcelHelper
    {
        #region Methods
        public static DataSet GetDataSetFromExcelDocument(byte[] content, bool firstRowIsHeader = true)
        {
            if (content == null) return null;

            using (var stream = new MemoryStream(content))
            {
                return GetDataSetFromExcelDocument(stream, firstRowIsHeader);
            }
        }


        private static DataSet GetDataSetFromExcelDocument(Stream stream, bool firstRowIsHeader = true)
        {
            if (stream != null && stream.Length > 4)
            {
                //Get the document type
                var buf = new byte[4];
                stream.Read(buf, 0, 4);
                stream.Seek(0, SeekOrigin.Begin);
                
                if (buf[0] == 0x50 && buf[1] == 0x4B && buf[2] == 0x03 && buf[3] == 0x04)
                    return GetDataSetFromXlsx(stream, firstRowIsHeader);
                else
                    return GetDataSetFromXlsAlternate(stream, firstRowIsHeader);
            }

            return null;
        }
        
        /// <summary>
        /// Конвертация потока с XLS-файлом в DataSet.
        /// </summary>
        /// <param name="stream">Входной поток с XLS-документом.</param>
        /// <returns>DataSet с данными.</returns>
        private static DataSet GetDataSetFromXls(Stream stream, bool firstRowIsHeader = true)
        {
            return GetDataSetFromXlsAlternate(stream, firstRowIsHeader);
        }

        /// <summary>
        /// Конвертация потока с XLS-файлом в DataSet, используя другую библиотеку.
        /// Это позволяет решить проблему открытия файлов без MS BIFF индекса (созданые, например, нашим экспортом).
        /// </summary>
        /// <param name="stream">Входной поток с XLS-документом.</param>
        /// <returns>DataSet с данными.</returns>
        private static DataSet GetDataSetFromXlsAlternate(Stream stream, bool firstRowIsHeader = true, bool withoutTypes = false)
        {
            var dataSet = new DataSet();

            var assembly = StiAssemblyFinder.GetAssembly("LibExcel.dll");
            var type = assembly.GetType("ExcelLibrary.SpreadSheet.Workbook");
            var loadMethod = type.GetMethod("Load", new[] { typeof(Stream) });
            var book = loadMethod.Invoke(null, new[] { stream });
            var bookWorksheets = book.GetType().GetField("Worksheets").GetValue(book) as IList;

            foreach (var sheet in bookWorksheets)
            {
                var sheetName = sheet.GetType().GetField("Name").GetValue(sheet) as string;
                var dataTable = new DataTable(sheetName);
                var sheetCells = sheet.GetType().GetField("Cells").GetValue(sheet);
                var sheetCellsType = sheetCells.GetType();

                var firstColIndex = (int)sheetCellsType.GetField("FirstColIndex").GetValue(sheetCells);
                var lastColIndex = (int)sheetCellsType.GetField("LastColIndex").GetValue(sheetCells);
                var firstRowIndex = (int)sheetCellsType.GetField("FirstRowIndex").GetValue(sheetCells);
                var lastRowIndex = (int)sheetCellsType.GetField("LastRowIndex").GetValue(sheetCells);

                for (var colIndex = firstColIndex; colIndex <= lastColIndex; colIndex++)
                {
                    dataTable.Columns.Add();
                }

                for (var rowIndex = firstRowIndex; rowIndex <= lastRowIndex; rowIndex++)
                {
                    var getRowMethod = sheetCellsType.GetMethod("GetRow", new[] { typeof(int) });
                    var row = getRowMethod.Invoke(sheetCells, new object[] { rowIndex });
                    var rowType = row.GetType();

                    var rowFirstColIndex = (int)rowType.GetField("FirstColIndex").GetValue(row);
                    var rowLastColIndex = (int)rowType.GetField("LastColIndex").GetValue(row);

                    var dataRow = dataTable.NewRow();
                    for (var colIndex = rowFirstColIndex; colIndex <= rowLastColIndex; colIndex++)
                    {
                        if (colIndex < dataRow.ItemArray.Length)
                        {
                            var getCellMethod = row.GetType().GetMethod("GetCell", new[] { typeof(int) });
                            var cell = getCellMethod.Invoke(row, new object[] { colIndex });

                            if (withoutTypes)
                            {
                                dataRow[colIndex] = cell.GetType().GetProperty("StringValue").GetValue(cell, null);
                            }
                            else
                                dataRow[colIndex] = cell;
                        }

                    }
                    dataTable.Rows.Add(dataRow);
                }

                if (firstRowIsHeader && dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        try
                        {
                            var rowColumnName = row[column] as string;
                            if (string.IsNullOrWhiteSpace(rowColumnName)) continue;

                            var exists = dataTable.Columns[rowColumnName] != null;
                            if (exists) continue;

                            column.ColumnName = rowColumnName;
                        }
                        catch
                        {

                        }
                    }
                    dataTable.Rows.RemoveAt(0);
                }

                dataSet.Tables.Add(dataTable);
            }
            return dataSet;
        }

        /// <summary>
        /// Конвертация потока с XLSX-файлом в DataSet.
        /// </summary>
        /// <param name="stream">Входной поток с XLSX-документом.</param>
        /// <returns>DataSet с данными.</returns>
        private static DataSet GetDataSetFromXlsx(Stream stream, bool firstRowIsHeader = true)
        {
            var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = firstRowIsHeader;
            var ds = excelReader.AsDataSet();
            excelReader.Close();

            return ds;
        }

        /// <summary>
        /// Конвертация потока с XLS-файлом в DataSet без учёта типов (приводятся к String).
        /// </summary>
        /// <param name="stream">Входной поток с XLS-документом.</param>
        /// <returns>DataSet с данными.</returns>
        public static DataSet GetDataSetFromXlsWithoutTypes(Stream stream, bool firstRowIsHeader = true)
        {
            return GetDataSetFromXlsAlternate(stream, firstRowIsHeader, true);
        }

        /// <summary>
        /// Конвертация потока с XLSX-файлом в DataSet без учёта типов (приводятся к String).
        /// </summary>
        /// <param name="stream">Входной поток с XLSX-документом.</param>
        /// <returns>DataSet с данными.</returns>
        public static DataSet GetDataSetFromXlsxWithoutTypes(Stream stream, bool firstRowIsHeader = true)
        {
            var ds = GetDataSetFromXlsx(stream, firstRowIsHeader);

            return CloneDataSetWithoutTypes(ds);
        }

        /// <summary>
        /// Преобразует DataSet таким образом, что все колонки приводятся к строкам.
        /// </summary>
        /// <param name="inputDataSet">Входящий DataSet.</param>
        /// <returns>DataSet, все колонки которого имеют тип String.</returns>
        private static DataSet CloneDataSetWithoutTypes(DataSet inputDataSet)
        {
            var dsOut = new DataSet(inputDataSet.DataSetName);

            foreach (DataTable tab in inputDataSet.Tables)
            {
                var tabo = new DataTable(tab.TableName);
                foreach (DataColumn col in tab.Columns)
                {
                    tabo.Columns.Add(col.ColumnName, typeof(String));
                }
                foreach (DataRow row in tab.Rows)
                {
                    var rowo = tabo.NewRow();
                    rowo.ItemArray = (object[])row.ItemArray.Clone();
                    tabo.Rows.Add(rowo);
                }
                dsOut.Tables.Add(tabo);
            }

            return dsOut;
        }
        #endregion
    }
}
