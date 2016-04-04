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

using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Stimulsoft.Base
{
    public class StiODataHelper
    {
        #region Methods
        /// <summary>
        /// Returns schema object which contains information about structure of the database. Schema returned start at specified root element (if it applicable).
        /// </summary>
        public StiDataSchema RetrieveSchema()
        {
            if (string.IsNullOrEmpty(ConnectionString)) return null;
            var schema = new StiDataSchema(StiConnectionIdent.ODataDataSource);

            try
            {
                using (var client = GetDefaultWebClient())
                {
                    var metadata = client.DownloadString(StiUrl.Combine(Address, "$metadata"));

                    using (var reader = new StringReader(metadata))
                    using (var xmlReader = XmlReader.Create(reader))
                    {
                        var root = XElement.Load(xmlReader);
                        var edmx = root.GetNamespaceOfPrefix("edmx");
                        var edm = root.Element(edmx + "DataServices").Elements().First().GetDefaultNamespace();
                        var elementSchema = root.Element(edmx + "DataServices").Elements().FirstOrDefault();
                        var namespaceStr = elementSchema.Attribute("Namespace") != null ? (string)elementSchema.Attribute("Namespace") : null;

                        var types = new Hashtable();
                        var hash = new Hashtable();

                        #region Parse Types
                        foreach (var entityType in elementSchema.Elements().Where(e => e.Name.LocalName == "EntityType" || e.Name.LocalName == "ComplexType"))
                        {
                            try
                            {
                                var name = (string)entityType.Attribute("Name");
                                var baseType = entityType.Attribute("BaseType") != null ? (string)entityType.Attribute("BaseType") : null;

                                if (string.IsNullOrWhiteSpace(name)) continue;
                                var properties = entityType.Elements(edm + "Property");

                                var table = new StiDataTableSchema(name, name);

                                if (baseType != null)
                                {
                                    var str = baseType.Replace(namespaceStr + ".", "");
                                    hash[str] = table;
                                }

                                foreach (var property in properties)
                                {
                                    try
                                    {
                                        var propertyName = (string)property.Attribute("Name");
                                        if (string.IsNullOrWhiteSpace(propertyName)) continue;

                                        var propertyNullable = property.Attribute("Nullable") != null && property.Attribute("Nullable").Value == "true";
                                        var propertyType = (string)property.Attribute("Type");
                                        var columnType = GetNetType(propertyType);

                                        if (propertyNullable)
                                            columnType = typeof(Nullable<>).MakeGenericType(columnType);

                                        var column = new StiDataColumnSchema(propertyName, columnType);
                                        table.Columns.Add(column);
                                    }
                                    catch
                                    {
                                    }
                                }

                                types[namespaceStr + "." + table.Name] = table;
                            }
                            catch
                            {
                            }

                            foreach (string tableName in hash.Keys)
                            {
                                var table = hash[tableName] as StiDataTableSchema;
                                var baseTable = schema.Tables.FirstOrDefault(t => t.Name == tableName);
                                if (baseTable == null) continue;

                                foreach (var column in baseTable.Columns)
                                {
                                    if (table.Columns.Any(c => c.Name == column.Name)) continue;
                                    table.Columns.Add(column);
                                }
                            }
                        }
                        #endregion

                        #region Parse Containers
                        foreach (var entityCont in elementSchema.Elements().Where(e => e.Name.LocalName == "EntityContainer"))
                        {
                            foreach (var entitySet in entityCont.Elements().Where(e => e.Name.LocalName == "EntitySet"))
                            {
                                try
                                {
                                    var name = (string)entitySet.Attribute("Name");
                                    var type = (string)entitySet.Attribute("EntityType");

                                    if (string.IsNullOrWhiteSpace(name)) continue;

                                    var table = new StiDataTableSchema(name, name);
                                    var columnsTable = types[type] as StiDataTableSchema;
                                    if (columnsTable != null)
                                        table.Columns.AddRange(columnsTable.Columns);
                                    schema.Tables.Add(table);
                                }
                                catch
                                {
                                }

                                foreach (string tableName in hash.Keys)
                                {
                                    var table = hash[tableName] as StiDataTableSchema;
                                    var baseTable = schema.Tables.FirstOrDefault(t => t.Name == tableName);
                                    if (baseTable == null) continue;

                                    foreach (var column in baseTable.Columns)
                                    {
                                        if (table.Columns.Any(c => c.Name == column.Name)) continue;
                                        table.Columns.Add(column);
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                return schema;
            }
            catch
            {
                return null;
            }
        }

        public void FillDataTable(DataTable table, string query)
        {
            if (string.IsNullOrEmpty(this.ConnectionString)) return;

            var currentCulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);

                using (var client = GetDefaultWebClient())
                {
                    var url = StiUrl.Combine(this.Address, query);
                    var metadata = client.DownloadString(url);

                    #region JSON
                    object odata = null;
                    try
                    {
                        odata = JsonConvert.DeserializeObject(metadata);
                    }
                    catch
                    {
                    }

                    if (odata != null && odata is JObject)
                    {
                        JArray values = null;

                        var jObject = odata as JObject;
                        if (jObject != null)
                        {
                            foreach (var child in jObject.Children())
                            {
                                var jProperty = child as JProperty;
                                if (jProperty != null && jProperty.Name == "value" && jProperty.Value is JArray)
                                {
                                    values = jProperty.Value as JArray;
                                }
                            }
                        }

                        if (values != null)
                        {
                            foreach (JObject value in values.ChildrenTokens)
                            {
                                var row = table.NewRow();

                                foreach (JProperty columnObjValue in value.ChildrenTokens)
                                {
                                    try
                                    {
                                        var columnName = columnObjValue.Name;
                                        var columnValue = columnObjValue.Value;

                                        var currentColumn = table.Columns[columnName];
                                        if (currentColumn != null)
                                        {
                                            var currentValue = StiConvert.ChangeType(columnValue, currentColumn.DataType);
                                            row[columnName] = currentValue ?? DBNull.Value;
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }

                                table.Rows.Add(row);
                            }
                        }
                    }
                    #endregion

                    #region XML
                    else {
                        using (var reader = new StringReader(metadata))
                        using (var xmlReader = XmlReader.Create(reader))
                        {
                            var root = XElement.Load(xmlReader);
                            var title = root.Elements().FirstOrDefault(e => e.Name.LocalName == "title");
                            if (title != null) table.TableName = title.Value;

                            foreach (var entry in root.Elements().Where(e => e.Name.LocalName == "entry"))
                            {
                                var elementContent = entry.Elements().FirstOrDefault(e => e.Name.LocalName == "content");
                                if (elementContent == null) continue;

                                var elementProperties = elementContent.Elements().FirstOrDefault(e => e.Name.LocalName.EndsWith("properties"));
                                if (elementProperties == null) continue;

                                var row = table.NewRow();

                                #region Name
                                try
                                {
                                    var elementTitle = entry.Elements().FirstOrDefault(e => e.Name.LocalName == "title");
                                    if (elementTitle != null && table.Columns["Name"] != null) row["Name"] = elementTitle.Value;
                                }
                                catch
                                {
                                }
                                #endregion

                                #region Description
                                try
                                {
                                    var elementSummary = entry.Elements().FirstOrDefault(e => e.Name.LocalName == "summary");
                                    if (elementSummary != null && table.Columns["Description"] != null) row["Description"] = elementSummary.Value;
                                }
                                catch
                                {
                                }
                                #endregion


                                foreach (var elementProperty in elementProperties.Elements())
                                {
                                    try
                                    {
                                        var columnName = elementProperty.Name.LocalName.Replace("d:", "");
                                        var columnValue = elementProperty.Value;

                                        var currentColumn = table.Columns[columnName];
                                        if (currentColumn != null)
                                        {
                                            var value = StiConvert.ChangeType(columnValue, currentColumn.DataType);
                                            row[columnName] = value ?? DBNull.Value;
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }

                                table.Rows.Add(row);
                            }
                        }
                    }
                    #endregion
                }
            }
            catch
            {
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCulture;
            }
        }

        /// <summary>
        /// Returns StiTestConnectionResult that is the information of whether the connection string specified in this class is correct.
        /// </summary>
        /// <returns>The result of testing the connection string.</returns>
        public StiTestConnectionResult TestConnection()
        {
            try
            {
                using (var client = GetDefaultWebClient())
                {
                    client.DownloadString(Address);
                }
            }
            catch (Exception exception)
            {
                return StiTestConnectionResult.MakeWrong(exception.Message);
            }

            return StiTestConnectionResult.MakeFine();
        }
        #endregion

        #region Methods.Helpers
        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public static Type GetNetType(string dbType)
        {
            if (string.IsNullOrWhiteSpace(dbType))
                return null;

            dbType = dbType.ToLowerInvariant();
            if (dbType.StartsWith("edm.")) dbType = dbType.Replace("edm.", "");
            switch (dbType)
            {
                case "int64":
                    return typeof(Int64);

                case "int32":
                    return typeof(Int32);

                case "int16":
                    return typeof(Int16);

                case "byte":
                    return typeof(Byte);

                case "sbyte":
                    return typeof(SByte);

                case "int":
                    return typeof(Int32);

                case "boolean":
                    return typeof(Boolean);

                case "decimal":
                    return typeof(decimal);

                case "float":
                    return typeof(float);

                case "double":
                    return typeof(double);

                case "time":
                case "datetime":
                    return typeof(DateTime);

                case "datetimeoffset":
                    return typeof(DateTimeOffset);

                case "guid":
                    return typeof(Guid);

                case "binary":
                    return typeof(byte[]);

                default:
                    return typeof(string);
            }
        }

        private WebClient GetDefaultWebClient()
        {
            var client = new WebClient();
            client.Encoding = StiBaseOptions.WebClientEncoding; ;

            if (!string.IsNullOrWhiteSpace(this.UserName))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(this.UserName, this.Password);
            }

            return client;
        }

        private string GetConnectionStringKey(string key)
        {
            if (string.IsNullOrWhiteSpace(ConnectionString)) return null;
            var strs = ConnectionString.Split(';', ',');

            var address = strs.FirstOrDefault(s => s.ToLowerInvariant().StartsWith(key.ToLowerInvariant()));
            if (address == null) return null;

            var pairs = address.Split('=');
            if (pairs.Length != 2) return null;

            var value = pairs[1];
            if (value.StartsWith("\"") && value.EndsWith("\"")) value = value.Substring(0, value.Length - 2);

            return value;
        }

        private string GetConnectionStringKey()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString)) return null;
            var strs = ConnectionString.Split(';', ',');

            return strs.FirstOrDefault(s => !s.Contains("="));
        }
        #endregion

        #region Properties
        public string ConnectionString { get; private set; }

        public string Address
        {
            get
            {
                var address = GetConnectionStringKey("Address") ?? GetConnectionStringKey();
                return address ?? ConnectionString;
            }
        }

        

        public string UserName
        {
            get
            {
                return GetConnectionStringKey("UserName");
            }
        }

        public string Password
        {
            get
            {
                return GetConnectionStringKey("Password");
            }
        }
        #endregion

        public StiODataHelper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}
