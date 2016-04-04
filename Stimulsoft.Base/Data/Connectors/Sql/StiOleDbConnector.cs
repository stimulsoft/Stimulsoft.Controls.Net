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
using System.Data.OleDb;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Base
{
    public class StiOleDbConnector : StiSqlDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.OleDbDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.OleDbDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "OLE DB";
            }
        }

        /// <summary>
        /// Get a value which indicates that this data connector can be used now.
        /// </summary>
        public override bool IsAvailable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a type of the connection which is used for this connector.
        /// </summary>
        public override Type ConnectionType
        {
            get
            {
                return typeof(OleDbConnection);
            }
        }
        
        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof(StiDbType.OleDb);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.OleDb.Variant;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns new connection to this type of the database.
        /// </summary>
        /// <returns>Created connection.</returns>
        public override DbConnection CreateConnection()
        {
            return new OleDbConnection(this.ConnectionString);
        }

        /// <summary>
        /// Returns new data adapter to this type of the database.
        /// </summary>
        /// <param name="query">A SQL query.</param>
        /// <param name="connection">A connection to database.</param>
        /// <returns>Created adapter.</returns>
        public override DbDataAdapter CreateAdapter(string query, DbConnection connection, CommandType commandType = CommandType.Text)
        {
            var adapter = new OleDbDataAdapter(query, connection as OleDbConnection);
            adapter.SelectCommand.CommandType = commandType;
            return adapter;
        }

        /// <summary>
        /// Returns new data command for this type of the database.
        /// </summary>
        /// <param name="query">A SQL query.</param>
        /// <param name="connection">A connection to database.</param>
        /// <returns>Created command.</returns>
        public override DbCommand CreateCommand(string query, DbConnection connection, CommandType commandType = CommandType.Text)
        {
            return new OleDbCommand(query, connection as OleDbConnection) { CommandType = commandType };
        }

        /// <summary>
        /// Returns new SQL parameter with specified parameter.
        /// </summary>
        public override DbParameter CreateParameter(string parameterName, object value, int size)
        {
            return new OleDbParameter(parameterName, value) { Size = size };
        }

        /// <summary>
        /// Returns new SQL parameter with specified parameter.
        /// </summary>
        public override DbParameter CreateParameter(string parameterName, int type, int size)
        {
            return new OleDbParameter(parameterName, (OleDbType)type) { Size = size };
        }

        /// <summary>
        /// Retrieves SQL parameters for the specified command.
        /// </summary>
        public override void DeriveParameters(DbCommand command)
        {
            OleDbCommandBuilder.DeriveParameters(command as OleDbCommand);
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
                using (var connection = CreateConnection())
                {

                    connection.Open();

                    #region Tables
                    var tableHash = new Hashtable();
                    try
                    {
                        var tables = connection.GetSchema("Tables");

                        foreach (var row in StiSchemaRow.All(tables))
                        {
                            if (row.TABLE_SCHEMA == "sys" || row.TABLE_SCHEMA == "INFORMATION_SCHEMA") continue;

                            var tableName = StiTableName.GetName(row.TABLE_SCHEMA != "dbo" ? row.TABLE_SCHEMA : null, row.TABLE_NAME);
                            var query = StiTableQuery.Get(this).GetSelectQuery(row.TABLE_SCHEMA != "dbo" ? row.TABLE_SCHEMA : null, row.TABLE_NAME);

                            var table = StiDataTableSchema.NewTableOrView(tableName, this, query);
                            tableHash[table.Name] = table;

                            if (row.TABLE_TYPE == "VIEW")schema.Views.Add(table);
                            if (row.TABLE_TYPE == "TABLE") schema.Tables.Add(table);
                        }
                    }
                    catch
                    {
                    }
                    #endregion

                    #region Columns
                    try
                    {
                        var columns = connection.GetSchema("Columns");

                        if (AdvancedRetrievalModeOfDatabaseSchema)
                        {
                            var tables = connection.GetSchema("Tables");
                            foreach (var row in StiSchemaRow.All(tables))
                            {
                                if (row.TABLE_SCHEMA == "sys") continue;
                                if (row.TABLE_TYPE == null || (row.TABLE_TYPE != "TABLE" && row.TABLE_TYPE != "VIEW")) continue;

                                var query = StiTableQuery.Get(this).GetSelectQuery(row.TABLE_SCHEMA != "dbo" ? row.TABLE_SCHEMA : null, row.TABLE_NAME);
                                
                                using (var command = CreateCommand(query, connection))
                                using (var reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
                                using (var table = reader.GetSchemaTable())
                                {
                                    foreach (DataRow rowTable in table.Rows)
                                    {
                                        var column = new StiDataColumnSchema(rowTable["COLUMNNAME"].ToString(), Type.GetType(rowTable["DATATYPE"].ToString()));
                                        var tableSchema = tableHash[row.TABLE_NAME] as StiDataTableSchema;

                                        if (table != null && !table.Columns.Contains(rowTable["ColumnName"].ToString()))
                                            tableSchema.Columns.Add(column);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var row in StiSchemaRow.All(columns))
                            {
                                if (row.TABLE_SCHEMA == "sys") continue;

                                var columnType = GetNetType(row.DATA_TYPE_INT);

                                var tableName = StiTableName.GetName(row.TABLE_SCHEMA != "dbo" ? row.TABLE_SCHEMA : null, row.TABLE_NAME);
                                var column = new StiDataColumnSchema(row.COLUMN_NAME, columnType);

                                var table = tableHash[tableName] as StiDataTableSchema;
                                if (table != null && table.Columns.All(c => c.Name != row.COLUMN_NAME))table.Columns.Add(column);
                            }
                        }
                    }
                    catch
                    {
                    }
                    #endregion

                    #region Procedures
                    var procedureHash = new Hashtable();

                    try
                    {
                        var procedures = connection.GetSchema("Procedures");

                        foreach (var row in StiSchemaRow.All(procedures))
                        {
                            if (row.PROCEDURE_SCHEMA == "sys") continue;

                            var baseName = row.PROCEDURE_NAME;
                            if (baseName.IndexOf(";", StringComparison.InvariantCulture) != -1)
                                baseName = baseName.Substring(0, baseName.IndexOf(";", StringComparison.InvariantCulture));

                            var procName = StiTableName.GetName(row.PROCEDURE_SCHEMA != "dbo" ? row.PROCEDURE_SCHEMA : null, baseName);
                            var query = StiTableQuery.Get(this).GetProcQuery(row.PROCEDURE_SCHEMA != "dbo" ? row.PROCEDURE_SCHEMA : null, baseName);

                            var procedure = StiDataTableSchema.NewProcedure(procName, this, query);

                            procedureHash[procedure.Name] = procedure;
                            schema.StoredProcedures.Add(procedure);
                        }
                    }
                    catch
                    {
                    }
                    #endregion

                    #region Procedures Parameters and Columns
                    foreach (var procedure in schema.StoredProcedures)
                    {
                        try
                        {
                            using (var command = CreateCommand(procedure.Query, connection, CommandType.StoredProcedure))
                            {
                                DeriveParameters(command);

                                using (var reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
                                using (var table = new DataTable(procedure.Name))
                                {
                                    table.Load(reader);

                                    foreach (DataColumn column in table.Columns)
                                    {
                                        procedure.Columns.Add(new StiDataColumnSchema
                                        {
                                            Name = column.ColumnName,
                                            Type = column.DataType
                                        });
                                    }

                                    foreach (DbParameter param in command.Parameters)
                                    {
                                        if (param.Direction == ParameterDirection.Input)
                                        {
                                            procedure.Parameters.Add(new StiDataParameterSchema
                                            {
                                                Name = param.ParameterName,
                                                Type = StiDbTypeConversion.GetNetType(param.DbType)
                                            });
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    #endregion

                    #region Relations
                    try
                    {
                        foreach (var schemaTable in schema.Tables)
                        {
                            var relations = GetRelationsTable(connection, schemaTable.Name);
                            if (relations == null) continue;

                            foreach (var row in StiSchemaRow.All(relations))
                            {
                                schema.Relations.Add(new StiDataRelationSchema
                                {
                                    Name = row.FK_NAME,
                                    ParentSourceName = row.PK_TABLE_NAME,
                                    ChildSourceName = row.FK_TABLE_NAME,
                                    ParentColumns = new List<string> { row.PK_COLUMN_NAME },
                                    ChildColumns = new List<string> { row.FK_COLUMN_NAME }
                                });
                            }
                        }
                    }
                    catch
                    {
                    }
                    #endregion

                    connection.Close();
                }

                return schema.Sort();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a SQL based type from the .NET type.
        /// </summary>
        public override int GetSqlType(Type type)
        {
            if (type == typeof(DateTime)) return (int)StiDbType.OleDb.DBDate;
            if (type == typeof(TimeSpan)) return (int)StiDbType.OleDb.DBTimeStamp;

            if (type == typeof(Int64)) return (int)StiDbType.OleDb.BigInt;
            if (type == typeof(Int32)) return (int)StiDbType.OleDb.Integer;
            if (type == typeof(Int16)) return (int)StiDbType.OleDb.SmallInt;
            if (type == typeof(Byte)) return (int)StiDbType.OleDb.TinyInt;

            if (type == typeof(UInt64)) return (int)StiDbType.OleDb.UnsignedBigInt;
            if (type == typeof(UInt32)) return (int)StiDbType.OleDb.UnsignedInt;
            if (type == typeof(UInt16)) return (int)StiDbType.OleDb.UnsignedSmallInt;
            if (type == typeof(SByte)) return (int)StiDbType.OleDb.UnsignedTinyInt;

            if (type == typeof(Single)) return (int)StiDbType.OleDb.Single;
            if (type == typeof(Double)) return (int)StiDbType.OleDb.Double;
            if (type == typeof(Decimal)) return (int)StiDbType.OleDb.Decimal;

            if (type == typeof(String)) return (int)StiDbType.OleDb.BSTR;

            if (type == typeof(Boolean)) return (int)StiDbType.OleDb.Boolean;
            if (type == typeof(Char)) return (int)StiDbType.OleDb.Char;
            if (type == typeof(Byte[])) return (int)StiDbType.OleDb.Binary;
            if (type == typeof(Guid)) return (int)StiDbType.OleDb.Guid;

            return (int)StiDbType.OleDb.Integer;
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(string dbType)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(int dbType)
        {
            switch ((StiDbType.OleDb)dbType)
            {
                case StiDbType.OleDb.Date:
                case StiDbType.OleDb.DBDate:
                case StiDbType.OleDb.DBTimeStamp:                
                    return typeof(DateTime);
                    
                case StiDbType.OleDb.DBTime:
                    return typeof(TimeSpan);

                case StiDbType.OleDb.BigInt:
                    return typeof(Int64);

                case StiDbType.OleDb.Integer:
                    return typeof(Int32);

                case StiDbType.OleDb.SmallInt:
                    return typeof(Int16);

                case StiDbType.OleDb.TinyInt:
                    return typeof(Byte);

                case StiDbType.OleDb.UnsignedBigInt:
                    return typeof(UInt64);

                case StiDbType.OleDb.UnsignedInt:
                    return typeof(UInt32);

                case StiDbType.OleDb.UnsignedSmallInt:
                    return typeof(UInt16);

                case StiDbType.OleDb.UnsignedTinyInt:
                    return typeof(SByte);

                case StiDbType.OleDb.Single:
                    return typeof(Single);

                case StiDbType.OleDb.Double:
                    return typeof(Double);

                case StiDbType.OleDb.Currency:
                case StiDbType.OleDb.Decimal:
                case StiDbType.OleDb.Numeric:
                    return typeof(Decimal);

                case StiDbType.OleDb.BSTR:
                case StiDbType.OleDb.VarChar:
                case StiDbType.OleDb.VarWChar:
                case StiDbType.OleDb.WChar:
                    return typeof(String);

                case StiDbType.OleDb.Boolean:
                    return typeof(Boolean);

                case StiDbType.OleDb.Char:
                    return typeof(Char);

                case StiDbType.OleDb.Binary: 
                    return typeof(Byte[]);

                case StiDbType.OleDb.Guid:
                    return typeof(Guid);

                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// Bracketing string with specials characters
        /// </summary>
        /// <param name="name">unput string</param>
        /// <returns>Bracketed string</returns>
        public override string GetDatabaseSpecificName(string name)
        {
            return string.Format("[{0}]", name);
        }

        protected override DataTable GetRelationsTable(DbConnection connection, string tableName)
        {
            return ((OleDbConnection)connection).GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, new object[] { null, null, tableName });
        }

        /// <summary>
        /// Returns sample of the connection string to this connector.
        /// </summary>
        public override string GetSampleConnectionString()
        {
            return @"Provider=SQLOLEDB.1; Integrated Security=SSPI;" + Environment.NewLine +
                   @"Persist Security Info=False; Initial Catalog=myDataBase;" + Environment.NewLine +
                   @"Data Source=myServerAddress";
        }

        /// <summary>
        /// Returns the type of the DBType.
        /// </summary>
        public override Type GetDbType()
        {
            return typeof(OleDbType);
        }
        #endregion

        #region Methods.Static
        public static StiOleDbConnector Get(string connectionString = null)
        {
            return new StiOleDbConnector(connectionString);
        }
        #endregion

        public StiOleDbConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "System.Data.OleDb.OleDbConnection.dll";
            this.TypeConnection = typeof(OleDbConnection).ToString();
            this.TypeDataAdapter = typeof(OleDbDataAdapter).ToString();
            this.TypeCommand = typeof(OleDbCommand).ToString();
            this.TypeParameter = typeof(OleDbParameter).ToString();
            this.TypeCommandBuilder = typeof(OleDbCommandBuilder).ToString();
        }
    }
}
