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
using System.Data.SqlClient;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base
{
    public class StiMongoDbConnector : StiNoSqlDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.MongoDbDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.MongoDbDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "MongoDB";
            }
        }

        public override bool AllowTestConnection
        {
            get
            {
                try
                {
                    //var client = new MongoClient(this.ConnectionString);
                    var client = AssemblyHelper.CreateObject("MongoDB.Driver.MongoClient", this.ConnectionString);

                    //var server = client.GetServer();
                    var server = client.GetType().GetMethod("GetServer").Invoke(client, null);
                   return server != null;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the package identificator for this connector.
        /// </summary>
        public override string[] NuGetPackages
        {
            get
            {
                return new string[] { "mongocsharpdriver" };
            }
        }
        #endregion

        #region Methods
        private object GetDatabase()
        {
            //var client = new MongoClient(this.ConnectionString);
            var client = AssemblyHelper.CreateObject("MongoDB.Driver.MongoClient", this.ConnectionString);

            //var builder = new MongoUrlBuilder(this.ConnectionString);
            var builder = AssemblyHelper.CreateObject("MongoDB.Driver.MongoUrlBuilder", this.ConnectionString);

            //return client.GetServer().GetDatabase(builder.DatabaseName);
            var databaseName = builder.GetType().GetProperty("DatabaseName").GetValue(builder, null) as string;

            var method = client.GetType().GetMethod("GetServer");
            var server = method != null ? method.Invoke(client, null) : null;

            if (server != null)//Net4
            {
                return server.GetType().GetMethod("GetDatabase", new[] {typeof (string)}).Invoke(server, new object[] {databaseName});
            }
            /*else //Net45
            {
                var settingsType = AssemblyHelper.GetType("MongoDB.Driver.MongoDatabaseSettings");
                return client.GetType().GetMethod("GetDatabase", new[] {typeof (string), settingsType}).Invoke(client, new object[] {databaseName, null});
            }*/
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns StiTestConnectionResult that is the information of whether the connection string specified in this class is correct.
        /// </summary>
        /// <returns>The result of testing the connection string.</returns>
        public override StiTestConnectionResult TestConnection()
        {
            try
            {
                //var client = new MongoClient(this.ConnectionString);
                var client = AssemblyHelper.CreateObject("MongoDB.Driver.MongoClient", this.ConnectionString);

                //var server = client.GetServer();
                var server = client.GetType().GetMethod("GetServer").Invoke(client, null);
                if (server == null) throw new NotSupportedException();

                //server.Ping();
                server.GetType().GetMethod("Ping").Invoke(server, null);
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null )
                    return StiTestConnectionResult.MakeWrong(exception.InnerException.Message);
                else
                    return StiTestConnectionResult.MakeWrong(exception.Message);
            }

            return StiTestConnectionResult.MakeFine();
        }

        /// <summary>
        /// Returns schema object which contains information about structure of the database. Schema returned start at specified root element (if it applicable).
        /// </summary>
        public override StiDataSchema RetrieveSchema()
        {
            if (string.IsNullOrEmpty(this.ConnectionString)) return null;
            var schema = new StiDataSchema(this.ConnectionIdent);

            try
            {
                var database = GetDatabase();

                #region Collections

                try
                {
                    //var collectionNames = database.GetCollectionNames();
                    var collectionNames = database.GetType().GetMethod("GetCollectionNames").Invoke(database, null) as IEnumerable<string>;
                    
                    foreach (var collectionName in collectionNames)
                    {
                        if (collectionName == "system.indexes") continue;
                        if (collectionName == "system.users") continue;

                        var tableSchema = StiDataTableSchema.NewTable(collectionName);

                        try
                        {
                            var columns = GetColumns(collectionName);
                            if (columns != null)
                                tableSchema.Columns = columns;
                        }
                        catch
                        {
                            
                        }

                        schema.Tables.Add(tableSchema);
                    }
                }
                catch
                {
                }
                #endregion

                return schema.Sort();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns sample of the connection string to this connector.
        /// </summary>
        public override string GetSampleConnectionString()
        {
            return @"mongodb://<user>:<password>@localhost/test";
        }

        public override List<StiDataColumnSchema> GetColumns(string collectionName)
        {
            var dataTable = GetDataTable(collectionName, null, 0, 5);
            return dataTable != null ? dataTable.Columns.Cast<DataColumn>().ToList().Select(c => new StiDataColumnSchema(c.ColumnName, c.DataType)).ToList() : null;
        }

        public override DataTable GetDataTable(string collectionName, string query, int? index = null, int? count = null)
        {
            try
            {
                var database = GetDatabase();
                //var collection = database.GetCollection<BsonDocument>(collectionName);
                var method = database.GetType().GetMethods().FirstOrDefault(m => m.Name == "GetCollection" &&
                                                                                 !m.IsGenericMethodDefinition &&
                                                                                 m.GetParameters().Length == 1 &&
                                                                                 m.GetParameters()[0].ParameterType ==
                                                                                 typeof (string));

                object collection = null;
                if (method != null)//net40
                    collection = method.Invoke(database, new object[] {collectionName});
                /*else//net45
                {
                    var settingsType = AssemblyHelper.GetType("MongoDB.Driver.MongoCollectionSettings");
                    method = database.GetType().GetMethod("GetCollection", new[] { typeof (string), settingsType });
                    method = method.MakeGenericMethod(database.GetType());
                    collection = method.Invoke(database, new object[] { collectionName, null });
                }*/

                //var cursor = string.IsNullOrWhiteSpace(query) ? collection.FindAll() : collection.Find(new QueryDocument(BsonDocument.Parse(query)));
                var assembly = new StiDataAssemblyHelper("MongoDB.Bson.dll");
                IEnumerable cursor = null;
                if (string.IsNullOrWhiteSpace(query))
                {
                    method = collection.GetType().GetMethod("FindAll");
                    cursor = method.Invoke(collection, null) as IEnumerable;
                }
                else
                {

                    var bsonDocument = assembly.GetType("MongoDB.Bson.BsonDocument")
                        .GetMethod("Parse")
                        .Invoke(null, new object[] {query});
                    var queryDocument = AssemblyHelper.CreateObject("MongoDB.Driver.QueryDocument", bsonDocument);

                    cursor = collection.GetType().GetMethod("Find").Invoke(collection, new[] {queryDocument}) as IEnumerable;

                }

                var list = new List<JToken>();
                var i = 0;
                foreach (var row in cursor)
                {
                    if (count != null && i >= count) break;
                    if (index == null || i >= index)
                    {
                        try
                        {
                            //var jStr = row.ToJson(new JsonWriterSettings { OutputMode = JsonOutputMode.Strict });

                            //var jsonWriterSettings = assembly.CreateObject("MongoDB.Bson.IO.JsonWriterSettings");
                            //jsonWriterSettings.GetType().GetProperty("OutputMode").SetValue(jsonWriterSettings, 0, null);

                            //var method2 = assembly.GetType("MongoDB.Bson.BsonExtensionMethods").GetMethod("ToJson", new[] {typeof(object), typeof(Type), jsonWriterSettings.GetType() });
                            //var jStr = method2.Invoke(row, new[] { jsonWriterSettings }) as string;
                            var jStr = GetAsJson(row);

                            var jToken = JToken.Parse(jStr);
                            list.Add(jToken);
                        }
                        catch
                        {

                        }
                    }

                    i++;
                }

                var dataSet = StiJsonToDataSetConverter.GetDataSet(list, true);
                return dataSet != null && dataSet.Tables.Count > 0 ? dataSet.Tables[0] : null;
            }
            catch (Exception exception)
            {
                if (exception.InnerException != null)
                    throw exception.InnerException;
                else
                    throw;
            }
        }

        private string GetAsJson(object obj)
        {
            var assembly = new StiDataAssemblyHelper("MongoDB.Bson.dll");

            var settings = assembly.CreateObject("MongoDB.Bson.IO.JsonWriterSettings");
            settings.GetType().GetProperty("OutputMode").SetValue(settings, 0, null);

            using (var writer = new StringWriter())
            {
                //var writer2 = BsonWriter.Create(writer, settings as JsonWriterSettings);
                var method = assembly.GetType("MongoDB.Bson.IO.BsonWriter").GetMethod("Create", new[] { writer.GetType(), settings.GetType() });
                using (var writer2 = method.Invoke(null, new[] { writer, settings }) as IDisposable)
                {
                    var bsonWriterType = assembly.GetType("MongoDB.Bson.IO.BsonWriter");
                    //BsonSerializer.Serialize(writer2, typeof(object), obj);
                    method = assembly.GetType("MongoDB.Bson.Serialization.BsonSerializer").GetMethod("Serialize", new[] { bsonWriterType, typeof(Type), obj.GetType() });
                    method.Invoke(null, new[] { writer2, typeof (object), obj });

                    return writer.ToString();
                }
            }
        }

        public string ConvertDateTimeToJsonStr(DateTime date)
        {
            try
            {
                //var bsonDocument = new BsonDocument();
                var assembly = new StiDataAssemblyHelper("MongoDB.Bson.dll");

                //var bsonElement = new BsonElement("date", BsonValue.Create(date));
                var bsonValue = assembly.GetType("MongoDB.Bson.BsonValue").GetMethod("Create").Invoke(null, new object[] { date } );
                var bsonElement = assembly.CreateObject("MongoDB.Bson.BsonElement", "date", bsonValue);

                var str = bsonElement.ToString().Replace("date=", "");
                
                return string.Format("\"{0}\"", str);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Methods.Static
        public static StiMongoDbConnector Get(string connectionString = null)
        {
            return new StiMongoDbConnector(connectionString);
        }
        #endregion

        public StiMongoDbConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "MongoDB.Driver.dll";
        }
    }
}
