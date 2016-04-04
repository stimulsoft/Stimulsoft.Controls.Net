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
using System.Data.Common;
using System.Data.Odbc;
using System.Data;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Base
{
    public class StiOdbcConnector : StiSqlDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.OdbcDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.OdbcDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "ODBC";
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
                return typeof(OdbcConnection);
            }
        }
        
        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof (StiDbType.Odbc);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.Odbc.Text;
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
            return new OdbcConnection(this.ConnectionString);
        }

        /// <summary>
        /// Returns new data adapter to this type of the database.
        /// </summary>
        /// <param name="query">A SQL query.</param>
        /// <param name="connection">A connection to database.</param>
        /// <returns>Created adapter.</returns>
        public override DbDataAdapter CreateAdapter(string query, DbConnection connection, CommandType commandType = CommandType.Text)
        {
            var adapter = new OdbcDataAdapter(query, connection as OdbcConnection);
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
            return new OdbcCommand(query, connection as OdbcConnection) { CommandType = commandType };
        }

        /// <summary>
        /// Returns new SQL parameter with specified parameter.
        /// </summary>
        public override DbParameter CreateParameter(string parameterName, object value, int size)
        {
            return new OdbcParameter(parameterName, value) { Size = size };
        }

        /// <summary>
        /// Returns new SQL parameter with specified parameter.
        /// </summary>
        public override DbParameter CreateParameter(string parameterName, int type, int size)
        {
            return new OdbcParameter(parameterName, (OdbcType)type) { Size = size };
        }

        /// <summary>
        /// Retrieves SQL parameters for the specified command.
        /// </summary>
        public override void DeriveParameters(DbCommand command)
        {
            OdbcCommandBuilder.DeriveParameters(command as OdbcCommand);
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
                            if (row.TABLE_SCHEM == "sys" || row.TABLE_TYPE != "TABLE" || row.TABLE_SCHEM == "INFORMATION_SCHEMA") continue;

                            var tableName = StiTableName.GetName(row.TABLE_SCHEM != "dbo" ? row.TABLE_SCHEM : null, row.TABLE_NAME);
                            var query = StiTableQuery.Get(this).GetSelectQuery(row.TABLE_SCHEM != "dbo" ? row.TABLE_SCHEM : null, row.TABLE_NAME);

                            var tableSchema = StiDataTableSchema.NewTableOrView(tableName, this, query);

                            tableHash[tableSchema.Name] = tableSchema;
                            schema.Tables.Add(tableSchema);
                        }
                    }
                    catch
                    {
                    }
                    #endregion

                    #region Views
                    try
                    {
                        var views = connection.GetSchema("Views");

                        foreach (var row in StiSchemaRow.All(views))
                        {
                            if (row.TABLE_SCHEM == "sys" || row.TABLE_TYPE != "VIEW" || row.TABLE_SCHEM == "INFORMATION_SCHEMA") continue;

                            var tableName = StiTableName.GetName(row.TABLE_SCHEM != "dbo" ? row.TABLE_SCHEM : null, row.TABLE_NAME);
                            var query = StiTableQuery.Get(this).GetSelectQuery(row.TABLE_SCHEM != "dbo" ? row.TABLE_SCHEM : null, row.TABLE_NAME);

                            var tableSchema = StiDataTableSchema.NewTableOrView(tableName, this, query);

                            tableHash[tableSchema.Name] = tableSchema;
                            schema.Views.Add(tableSchema);
                        }
                    }
                    catch
                    {
                    }
                    #endregion

                    #region Columns
                    try
                    {
                        if (AdvancedRetrievalModeOfDatabaseSchema)
                        {
                            var tables = connection.GetSchema("Tables");
                            foreach (var row in StiSchemaRow.All(tables))
                            {
                                if (row.TABLE_SCHEM == "sys") continue;
                                if (row.TABLE_TYPE != "TABLE") continue;

                                var query = StiTableQuery.Get(this).GetSelectQuery(row.TABLE_SCHEM != "dbo" ? row.TABLE_SCHEM : null, row.TABLE_NAME);
                                using (var command = CreateCommand(query, connection))
                                using (var reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
                                using (var table = reader.GetSchemaTable())
                                {
                                    foreach (var rowTable in StiSchemaRow.All(table))
                                    {
                                        var columnSchema = new StiDataColumnSchema(rowTable.COLUMNNAME, Type.GetType(rowTable.DATATYPE));
                                        var tableName = StiTableName.GetName(row.TABLE_SCHEM != "dbo" ? row.TABLE_SCHEM : null, row.TABLE_NAME);

                                        var tableSchema = tableHash[tableName] as StiDataTableSchema;
                                        if (tableSchema != null && !tableSchema.Columns.Exists(t => t.Name == rowTable.COLUMNNAME))
                                            tableSchema.Columns.Add(columnSchema);
                                            
                                    }
                                }
                            }
                        }
                        else
                        {
                            var columns = connection.GetSchema("Columns");

                            foreach (var row in StiSchemaRow.All(columns))
                            {
                                if (row.TABLE_SCHEM == "sys") continue;

                                var column = new StiDataColumnSchema(row.COLUMN_NAME, GetNetType(row.TYPE_NAME));
                                var tableName = StiTableName.GetName(row.TABLE_SCHEM != "dbo" ? row.TABLE_SCHEM : null, row.TABLE_NAME);

                                var table = tableHash[tableName] as StiDataTableSchema;
                                if (table != null)table.Columns.Add(column);
                            }
                        }
                    }
                    catch
                    {
                    }
                    #endregion

                    #region Procedures
                    try
                    {
                        var procedures = connection.GetSchema("Procedures");

                        var procedureHash = new Hashtable();
                        foreach (var row in StiSchemaRow.All(procedures))
                        {
                            if (row.PROCEDURE_SCHEM == "sys") continue;

                            if (row.PROCEDURE_CAT == null || row.PROCEDURE_CAT != connection.Database) continue;

                            var baseName = row.PROCEDURE_NAME;
                            if (baseName.IndexOf(";", StringComparison.InvariantCulture) != -1)
                                baseName = baseName.Substring(0, baseName.IndexOf(";", StringComparison.InvariantCulture));

                            var procName = StiTableName.GetName(row.PROCEDURE_SCHEM != "dbo" ? row.PROCEDURE_SCHEM : null, baseName);
                            var query = StiTableQuery.Get(this).GetCallQuery(row.PROCEDURE_SCHEM != "dbo" ? row.PROCEDURE_SCHEM : null, baseName);

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
                            using (var command = CreateCommand(procedure.Name, connection, CommandType.StoredProcedure))
                            {
                                DeriveParameters(command);

                                if (command.Parameters != null && command.Parameters.Count > 0)
                                {
                                    if (command.Parameters[0].Direction == ParameterDirection.ReturnValue)
                                        command.Parameters.RemoveAt(0);

                                    var pars = new List<string>();
                                    var count = command.Parameters.Count;
                                    while (count > 0)
                                    {
                                        pars.Add("?");
                                        count--;
                                    }

                                    procedure.Query = string.Format("{{{0}({1})}}", procedure.Query, string.Join(",", pars));
                                }
                                else 
                                    procedure.Query = string.Format("{{{0}()}}", procedure.Query);

                                command.CommandText = procedure.Query;

                                using (var reader = command.ExecuteReader(CommandBehavior.SchemaOnly))
                                using (var table = new DataTable(procedure.Name))
                                {
                                    table.Load(reader);

                                    foreach (DataColumn column in table.Columns)
                                    {
                                        procedure.Columns.Add(new StiDataColumnSchema { Name = column.ColumnName, Type = column.DataType });
                                    }

                                    foreach (DbParameter param in command.Parameters)
                                    {
                                        if (param.Direction == ParameterDirection.Input)
                                        {
                                            procedure.Parameters.Add(new StiDataParameterSchema { Name = param.ParameterName, Type = StiDbTypeConversion.GetNetType(param.DbType) });
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
            if (type == typeof(DateTime)) return (int)StiDbType.Odbc.DateTime;
            if (type == typeof(TimeSpan)) return (int)StiDbType.Odbc.Timestamp;

            if (type == typeof(Int64)) return (int)StiDbType.Odbc.BigInt;
            if (type == typeof(Int32)) return (int)StiDbType.Odbc.Int;
            if (type == typeof(Int16)) return (int)StiDbType.Odbc.SmallInt;
            if (type == typeof(Byte)) return (int)StiDbType.Odbc.TinyInt;

            if (type == typeof(UInt64)) return (int)StiDbType.Odbc.BigInt;
            if (type == typeof(UInt32)) return (int)StiDbType.Odbc.Int;
            if (type == typeof(UInt16)) return (int)StiDbType.Odbc.SmallInt;
            if (type == typeof(SByte)) return (int)StiDbType.Odbc.TinyInt;

            if (type == typeof(Single)) return (int)StiDbType.Odbc.Decimal;
            if (type == typeof(Double)) return (int)StiDbType.Odbc.Double;
            if (type == typeof(Decimal)) return (int)StiDbType.Odbc.Decimal;

            if (type == typeof(String)) return (int)StiDbType.Odbc.VarChar;

            if (type == typeof(Boolean)) return (int)StiDbType.Odbc.Char;
            if (type == typeof(Char)) return (int)StiDbType.Odbc.Char;
            if (type == typeof(Byte[])) return (int)StiDbType.Odbc.Binary;
            if (type == typeof(Guid)) return (int)StiDbType.Odbc.UniqueIdentifier;

            return (int)StiDbType.Odbc.Int;
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(string dbType)
        {
            switch (dbType.ToLowerInvariant())
            {
                case "counter":
                case "bigint":
                case "longbinary":
                case "varbinary":
                case "int":
                case "uniqueidentifier":                
                    return typeof(Int64);

                case "smallint":
                    return typeof(Int16);

                case "tinyint":
                    return typeof(Byte);

                case "decimal":
                case "money":
                case "smallmoney":
                    return typeof(decimal);

                case "float":
                case "real":
                    return typeof(float);

                case "double":
                    return typeof(double);

                case "bit":
                    return typeof(Boolean);

                case "datetime":
                case "smalldatetime":
                case "date":
                case "time":
                case "timestamp":
                    return typeof(DateTime);

                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(int dbType)
        {
            switch ((StiDbType.Odbc)dbType)
            {
                case StiDbType.Odbc.BigInt:
                case StiDbType.Odbc.VarBinary:
                case StiDbType.Odbc.Int:
                case StiDbType.Odbc.UniqueIdentifier:                
                    return typeof(Int64);

                case StiDbType.Odbc.SmallInt:
                    return typeof(Int16);

                case StiDbType.Odbc.TinyInt:
                    return typeof(Byte);

                case StiDbType.Odbc.Decimal:
                    return typeof(decimal);

                case StiDbType.Odbc.Real:
                    return typeof(float);

                case StiDbType.Odbc.Double:
                    return typeof(double);

                case StiDbType.Odbc.Bit:
                    return typeof(Boolean);

                case StiDbType.Odbc.DateTime:
                case StiDbType.Odbc.SmallDateTime:
                case StiDbType.Odbc.Date:
                case StiDbType.Odbc.Time:
                case StiDbType.Odbc.Timestamp:
                    return typeof(DateTime);

                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// Bracketing string with specials characters
        /// </summary>
        /// <param name="name">A input string</param>
        /// <returns>Bracketed string</returns>
        public override string GetDatabaseSpecificName(string name)
        {
            return string.Format("`{0}`", name);
        }


        /// <summary>
        /// Returns sample of the connection string to this connector.
        /// </summary>
        public override string GetSampleConnectionString()
        {
            return @"Driver={SQL Server}; Server=myServerAddress;" + Environment.NewLine +
                   @"Database=myDataBase; Uid=myUsername; Pwd=myPassword;";
        }

        /// <summary>
        /// Returns the type of the DBType.
        /// </summary>
        public override Type GetDbType()
        {
            return typeof(OdbcType);
        }
        #endregion

        #region Methods.Static
        public static StiOdbcConnector Get(string connectionString = null)
        {
            return new StiOdbcConnector(connectionString);
        }
        #endregion

        public StiOdbcConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "System.Data.Odbc.OdbcConnection.dll";
            this.TypeConnection = typeof(System.Data.Odbc.OdbcConnection).ToString();
            this.TypeDataAdapter = typeof(System.Data.Odbc.OdbcDataAdapter).ToString();
            this.TypeCommand = typeof(System.Data.Odbc.OdbcCommand).ToString();
            this.TypeParameter = typeof(System.Data.Odbc.OdbcParameter).ToString();
            this.TypeCommandBuilder = typeof(System.Data.Odbc.OdbcCommandBuilder).ToString();
        }
    }
}
