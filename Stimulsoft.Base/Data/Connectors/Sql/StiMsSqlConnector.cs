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
using System.Linq;
using System.Data.SqlClient;
using Stimulsoft.Base;

namespace Stimulsoft.Base
{
    public class StiMsSqlConnector : StiSqlDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.MsSqlDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.MsSqlDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "MS SQL";
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
                return typeof(SqlConnection);
            }
        }

        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof(StiDbType.MsSql);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.MsSql.Text;
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
            return new SqlConnection(this.ConnectionString);
        }

        /// <summary>
        /// Returns new data adapter to this type of the database.
        /// </summary>
        /// <param name="query">A SQL query.</param>
        /// <param name="connection">A connection to database.</param>
        /// <returns>Created adapter.</returns>
        public override DbDataAdapter CreateAdapter(string query, DbConnection connection, CommandType commandType = CommandType.Text)
        {
            var adapter = new SqlDataAdapter(query, connection as SqlConnection);
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
            return new SqlCommand(query, connection as SqlConnection) { CommandType = commandType };
        }

        /// <summary>
        /// Returns new SQL parameter with specified parameter.
        /// </summary>
        public override DbParameter CreateParameter(string parameterName, object value, int size)
        {
            return new SqlParameter(parameterName, value) { Size = size };
        }

        /// <summary>
        /// Returns new SQL parameter with specified parameter.
        /// </summary>
        public override DbParameter CreateParameter(string parameterName, int type, int size)
        {
            return new SqlParameter(parameterName, (SqlDbType)type) { Size = size };
        }

        /// <summary>
        /// Retrieves SQL parameters for the specified command.
        /// </summary>
        public override void DeriveParameters(DbCommand command)
        {
            SqlCommandBuilder.DeriveParameters(command as SqlCommand);
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

                    #region Tables & Views
                    var tableHash = new Hashtable();

                    try
                    {
                        var tables = connection.GetSchema("Tables");

                        foreach (var row in StiSchemaRow.All(tables))
                        {
                            if (row.TABLE_SCHEMA == "sys") continue;

                            var tableName = StiTableName.GetName(row.TABLE_SCHEMA != "dbo" ? row.TABLE_SCHEMA : null, row.TABLE_NAME);
                            var query = StiTableQuery.Get(this).GetSelectQuery(row.TABLE_SCHEMA != "dbo" ? row.TABLE_SCHEMA : null, row.TABLE_NAME);

                            var tableSchema = StiDataTableSchema.NewTableOrView(tableName, this, query);
                            tableHash[tableName] = tableSchema;

                            if (row.TABLE_TYPE == "BASE TABLE") schema.Tables.Add(tableSchema);
                            if (row.TABLE_TYPE == "VIEW") schema.Views.Add(tableSchema);

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

                        foreach (var row in StiSchemaRow.All(columns))
                        {
                            if (row.TABLE_SCHEMA == "sys") continue;

                            var tableName = StiTableName.GetName(row.TABLE_SCHEMA != "dbo" ? row.TABLE_SCHEMA : null, row.TABLE_NAME);

                            var column = new StiDataColumnSchema(row.COLUMN_NAME, GetNetType(row.DATA_TYPE));
                            var table = tableHash[tableName] as StiDataTableSchema;
                            if (table != null) table.Columns.Add(column);
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
                        var connectionDatabase = connection.Database != null ? connection.Database.ToUpperInvariant() : null;

                        foreach (var row in StiSchemaRow.All(procedures))
                        {
                            var rowROUTINE_CATALOG = row.ROUTINE_CATALOG != null ? row.ROUTINE_CATALOG.ToUpperInvariant() : null;
                            if (row.SPECIFIC_SCHEMA == "sys" || row.ROUTINE_TYPE != "PROCEDURE" || rowROUTINE_CATALOG != connectionDatabase) continue;

                            var procName = StiTableName.GetName(row.SPECIFIC_SCHEMA != "dbo" ? row.SPECIFIC_SCHEMA : null, row.SPECIFIC_NAME);
                            var query = StiTableQuery.Get(this).GetProcQuery(row.SPECIFIC_SCHEMA != "dbo" ? row.SPECIFIC_SCHEMA : null, row.SPECIFIC_NAME);

                            var procedure = StiDataTableSchema.NewProcedure(procName, this, query);
                            schema.StoredProcedures.Add(procedure);
                        }
                    }
                    catch
                    {
                    }
                    #endregion

                    #region Procedures Parameters and Columns
                    try
                    {
                        var queryGetParams = "select obj.name as procName, params.*, type_name(system_type_id) as type_name from sys.parameters params, sys.objects obj" +
                                    " where params.object_id = obj.object_id";
                        using (var commandGetParams = CreateCommand(queryGetParams, connection, CommandType.Text))
                        {
                            using (var reader = commandGetParams.ExecuteReader())
                            {
                                using (var table = new DataTable("Parameters"))
                                {
                                    table.Load(reader);
                                    foreach (var procedure in schema.StoredProcedures)
                                    {
                                        #region Fill Parameters
                                        foreach (DataRow row in table.Rows)
                                        {
                                            var typeConverter = new StiMsSqlConnector();
                                            if (procedure.Name == row["procName"].ToString())
                                            {
                                                procedure.Parameters.Add(new StiDataParameterSchema
                                                {
                                                    Name = row["name"].ToString(),
                                                    Type = GetNetType(row["type_name"].ToString())
                                                });
                                            }
                                        }
                                        #endregion
                                    }
                                }
                            }
                        }
                        #region Fill Columns
                        if (StiDataOptions.WizardStoredProcRetriveMode == StiWizardStoredProcRetriveMode.All)
                        {
                            try
                            {
                                foreach (var procedure in schema.StoredProcedures)
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
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                        #endregion
                    }
                    catch
                    {
                    }
                    #endregion

                    #region Relations
                    try
                    {
                        const string commandText = @"
                            SELECT KCU1.CONSTRAINT_NAME AS 'FK_CONSTRAINT_NAME' 
                                , KCU1.TABLE_NAME AS 'FK_TABLE_NAME'
                                , KCU1.COLUMN_NAME AS 'FK_COLUMN_NAME'
                                , KCU1.ORDINAL_POSITION AS 'FK_ORDINAL_POSITION'
                                , KCU2.CONSTRAINT_NAME AS 'UQ_CONSTRAINT_NAME'
                                , KCU2.TABLE_NAME AS 'UQ_TABLE_NAME'
                                , KCU2.COLUMN_NAME AS 'UQ_COLUMN_NAME'
                                , KCU2.ORDINAL_POSITION AS 'UQ_ORDINAL_POSITION'
                            FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC
                            JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU1
                            ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG 
                                AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA
                                AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
                            JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU2
                            ON KCU2.CONSTRAINT_CATALOG = 
                            RC.UNIQUE_CONSTRAINT_CATALOG 
                                AND KCU2.CONSTRAINT_SCHEMA = 
                            RC.UNIQUE_CONSTRAINT_SCHEMA
                                AND KCU2.CONSTRAINT_NAME = 
                            RC.UNIQUE_CONSTRAINT_NAME
                                AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION";

                        using (var dataSet = new DataSet())
                        using (var adapter = CreateAdapter(commandText, connection))
                        {
                            adapter.Fill(dataSet);
                            var dataTable = (dataSet.Tables.Count > 0) ? dataSet.Tables[0] : null;
                            if (dataTable != null)
                            {
                                foreach (var row in StiSchemaRow.All(dataTable))
                                {
                                    schema.Relations.Add(new StiDataRelationSchema
                                    {
                                        Name = row.FK_CONSTRAINT_NAME,
                                        ParentSourceName = row.UQ_TABLE_NAME,
                                        ChildSourceName = row.FK_TABLE_NAME,
                                        ParentColumns = new List<string> { row.UQ_COLUMN_NAME },
                                        ChildColumns = new List<string> { row.FK_COLUMN_NAME }
                                    });
                                }
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
            if (type == typeof(DateTime)) return (int)StiDbType.MsSql.DateTime;
            if (type == typeof(DateTimeOffset)) return (int)StiDbType.MsSql.DateTimeOffset;
            if (type == typeof(TimeSpan)) return (int)StiDbType.MsSql.Timestamp;

            if (type == typeof(Int64)) return (int)StiDbType.MsSql.BigInt;
            if (type == typeof(Int32)) return (int)StiDbType.MsSql.Int;
            if (type == typeof(Int16)) return (int)StiDbType.MsSql.SmallInt;
            if (type == typeof(Byte)) return (int)StiDbType.MsSql.TinyInt;

            if (type == typeof(UInt64)) return (int)StiDbType.MsSql.BigInt;
            if (type == typeof(UInt32)) return (int)StiDbType.MsSql.Int;
            if (type == typeof(UInt16)) return (int)StiDbType.MsSql.SmallInt;
            if (type == typeof(SByte)) return (int)StiDbType.MsSql.TinyInt;

            if (type == typeof(Single)) return (int)StiDbType.MsSql.Float;
            if (type == typeof(Double)) return (int)StiDbType.MsSql.Real;
            if (type == typeof(Decimal)) return (int)StiDbType.MsSql.Decimal;

            if (type == typeof(String)) return (int)StiDbType.MsSql.VarChar;

            if (type == typeof(Boolean)) return (int)StiDbType.MsSql.Bit;
            if (type == typeof(Char)) return (int)StiDbType.MsSql.Char;
            if (type == typeof(Byte[])) return (int)StiDbType.MsSql.Binary;
            if (type == typeof(Guid)) return (int)StiDbType.MsSql.UniqueIdentifier;

            return (int)StiDbType.MsSql.VarChar;
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(string dbType)
        {
            switch (dbType.ToLowerInvariant())
            {
                case "uniqueidentifier":
                case "bigint":
                case "timestamp":
                    return typeof(Int64);

                case "int":
                    return typeof(Int32);

                case "smallint":
                    return typeof(Int16);

                case "tinyint":
                    return typeof(Byte);

                case "decimal":
                case "money":
                case "smallmoney":
                    return typeof(decimal);

                case "float":
                    return typeof(float);

                case "real":
                    return typeof(double);

                case "datetime":
                case "date":
                case "time":
                case "datetime2":
                case "smalldatetime":
                    return typeof(DateTime);

                case "datetimeoffset":
                    return typeof(DateTimeOffset);

                case "bit":
                    return typeof(Boolean);

                case "binary":
                case "image":
                    return typeof(byte[]);

                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(int dbType)
        {
            switch ((StiDbType.MsSql)dbType)
            {
                case StiDbType.MsSql.UniqueIdentifier:
                    return typeof(Guid);

                case StiDbType.MsSql.BigInt:
                case StiDbType.MsSql.Timestamp:
                    return typeof(Int64);

                case StiDbType.MsSql.Int:
                    return typeof(Int32);

                case StiDbType.MsSql.SmallInt:
                    return typeof(Int16);

                case StiDbType.MsSql.TinyInt:
                    return typeof(Byte);

                case StiDbType.MsSql.Decimal:
                case StiDbType.MsSql.Money:
                case StiDbType.MsSql.SmallMoney:
                    return typeof(decimal);

                case StiDbType.MsSql.Float:
                    return typeof(float);

                case StiDbType.MsSql.Real:
                    return typeof(double);

                case StiDbType.MsSql.DateTime:
                case StiDbType.MsSql.Date:
                case StiDbType.MsSql.Time:
                case StiDbType.MsSql.DateTime2:
                case StiDbType.MsSql.SmallDateTime:
                    return typeof(DateTime);

                case StiDbType.MsSql.DateTimeOffset:
                    return typeof(DateTimeOffset);

                case StiDbType.MsSql.Bit:
                    return typeof(Boolean);

                case StiDbType.MsSql.Binary:
                    return typeof(byte[]);

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

        /// <summary>
        /// Returns sample of the connection string to this connector.
        /// </summary>
        public override string GetSampleConnectionString()
        {
            return @"Integrated Security=False; Data Source=myServerAddress;" + Environment.NewLine +
                   @"Initial Catalog=myDataBase; User ID=myUsername; Password=myPassword;";
        }

        /// <summary>
        /// Returns the type of the DBType.
        /// </summary>
        public override Type GetDbType()
        {
            return typeof(SqlDbType);
        }
        #endregion

        #region Methods.Static
        public static StiMsSqlConnector Get(string connectionString = null)
        {
            return new StiMsSqlConnector(connectionString);
        }
        #endregion

        public StiMsSqlConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "System.Data.SqlClient.SqlConnection.dll";
            this.TypeConnection = typeof(SqlConnection).ToString();
            this.TypeDataAdapter = typeof(SqlDataAdapter).ToString();
            this.TypeCommand = typeof(SqlCommand).ToString();
            this.TypeParameter = typeof(SqlParameter).ToString();
            this.TypeCommandBuilder = typeof(SqlCommandBuilder).ToString();
        }
    }
}
