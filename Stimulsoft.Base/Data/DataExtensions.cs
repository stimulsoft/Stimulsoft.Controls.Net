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
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base
{
    public static class DataExtensions
    {
        #region Methods
        public static byte[] WriteToArray(this DataSet dataSet)
        {
            if (dataSet == null) return null;

            using (var stream = new MemoryStream())
            {
                dataSet.WriteXml(stream, XmlWriteMode.WriteSchema);
                return stream.ToArray();
            }
        }

        public static byte[] WriteToArray(this DataTable table)
        {
            if (table == null) return null;
            
            using (var stream = new MemoryStream())
            {
                table.WriteXml(stream, XmlWriteMode.WriteSchema);
                return stream.ToArray();
            }
        }

        public static string WriteToString(this DataTable table)
        {
            if (table == null) return null;

            using (var writer = new StringWriter())
            {
                table.WriteXml(writer, XmlWriteMode.WriteSchema);
                return writer.ToString();
            }
        }

        public static string WriteToJson(this DataTable table)
        {
            if (table == null) return null;

            return JsonConvert.SerializeObject(table, Formatting.Indented, StiJsonHelper.DefaultSerializerSettings);
        }

        public static string WriteToString(this DataTable table, StiDataFormatType type)
        {
            if (type == StiDataFormatType.Xml)
                return table.WriteToString();

            if (type == StiDataFormatType.Json && table.Rows.Count == 0)
            {
                var columns = new List<StiDataColumnSchema>();
                foreach (DataColumn dataColumn in table.Columns)
                {
                    columns.Add(new StiDataColumnSchema(dataColumn.ColumnName, dataColumn.DataType));
                }

                return JsonConvert.SerializeObject(columns);
            }
            return table.WriteToJson();
        }

        public static void Read(this DataTable table, byte[] content)
        {
            if (content == null || content.Length == 0) return;

            using (var stream = new MemoryStream(content))
            {
                table.ReadXml(stream);
            }
        }

        public static void Read(this DataTable table, string content)
        {
            if (content == null) return;

            using (var reader = new StringReader(content))
            {
                table.ReadXml(reader);
            }
        }

        public static DataTable ReadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;

            return JsonConvert.DeserializeObject<DataTable>(json);
        }

        public static DataTable ReadFromString(string content)
        {
            if (string.IsNullOrWhiteSpace(content)) return null;

            var table = new DataTable();
            table.Read(content);
            return table;
        }

        public static DataTable ReadFrom(StiDataFormatType type, string str)
        {
            return type == StiDataFormatType.Json ? ReadFromJson(str) : ReadFromString(str);
        }

        public static void Read(this DataSet dataSet, byte[] schema, byte[] data = null)
        {
            if (schema != null)
            {
                using (var stream = new MemoryStream(schema))
                {
                    dataSet.ReadXmlSchema(stream);
                }
            }

            if (data != null)
            {
                using (var stream = new MemoryStream(data))
                {
                    dataSet.ReadXml(stream);
                }
            }
        }
        #endregion
    }
}
