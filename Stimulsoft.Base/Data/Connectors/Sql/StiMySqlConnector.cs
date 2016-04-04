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
using System.Text;
using Stimulsoft.Base;

namespace Stimulsoft.Base
{
    public class StiMySqlConnector : StiSqlDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.MySqlDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.MySqlDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "MySQL";
            }
        }
        
        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof(StiDbType.MySql);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.MySql.Text;
            }
        }

        /// <summary>
        /// Gets the package identificator for this connector.
        /// </summary>
        public override string[] NuGetPackages
        {
            get
            {
                return new string[] { "MySql.Data" };
            }
        }
        #endregion

        #region Methods
        public override void ResetSettings()
        {
            isGeneric = null;
            isDevart = null;
        }

        /// <summary>
        /// Return an array of the data connectors which can be used also to access data for this type of the connector.
        /// </summary>
        public override StiDataConnector[] GetFamilyConnectors()
        {
            return new StiDataConnector[] 
            {
                new StiMySqlConnector(),
                new StiMySqlDevartConnector()
            };
        }

        /// <summary>
        /// Returns new SQL parameter with specified parameter.
        /// </summary>
        public override DbParameter CreateParameter(string parameterName, object value, int size)
        {
            var parameter = AssemblyHelper.CreateParameterWithValue(TypeParameter, parameterName, value);
            parameter.Size = size;
            return parameter;
        }

        /// <summary>
        /// Returns new SQL parameter with specified parameter.
        /// </summary>
        public override DbParameter CreateParameter(string parameterName, int type, int size)
        {
            var dbType = GetDbType();
            var parameter = AssemblyHelper.CreateParameterWithType(TypeParameter, parameterName, type, dbType ?? typeof(int));
            parameter.Size = size;
            return parameter;
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

                    var connectionDatabase = connection.Database != null ? connection.Database.ToLowerInvariant() : null;

                    #region Tables
                    var tableHash = new Hashtable();
                    try
                    {
                        var tables = connection.GetSchema("Tables");

                        foreach (var row in StiSchemaRow.All(tables))
                        {
                            if (row.TABLE_SCHEMA == null || row.TABLE_SCHEMA.ToLowerInvariant() != connectionDatabase || row.TABLE_TYPE != "BASE TABLE") continue;

                            var table = StiDataTableSchema.NewTable(row.TABLE_NAME);
                            tableHash[table.Name] = table;
                            schema.Tables.Add(table);
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
                            if (row.TABLE_SCHEMA == null || row.TABLE_SCHEMA.ToLowerInvariant() != connectionDatabase) continue;

                            var view = StiDataTableSchema.NewView(row.TABLE_NAME);
                            tableHash[view.Name] = view;
                            schema.Views.Add(view);
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
                            if (row.TABLE_SCHEMA == null || row.TABLE_SCHEMA.ToLowerInvariant() != connectionDatabase) continue;

                            var column = new StiDataColumnSchema(row.COLUMN_NAME, GetNetType(row.DATA_TYPE));

                            var table = tableHash[row.TABLE_NAME] as StiDataTableSchema;
                            if (table != null)table.Columns.Add(column);
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
                            var procName = row.ROUTINE_NAME;

                            if (row.ROUTINE_SCHEMA == null || row.ROUTINE_SCHEMA.ToLowerInvariant() != connectionDatabase || procName == null) continue;

                            if (procName.IndexOf(";", StringComparison.InvariantCulture) != -1)
                                procName = procName.Substring(0, procName.IndexOf(";", StringComparison.InvariantCulture));

                            var procedure = StiDataTableSchema.NewProcedure(procName);
                            procedure.Query = StiTableQuery.Get(this).GetProcQuery(procName);

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

                                    if (command.Parameters.Count > 0)
                                    {
                                        var paramStr = new StringBuilder();
                                        foreach (DbParameter param in command.Parameters)
                                        {
                                            if (param.Direction == ParameterDirection.Input)
                                            {
                                                procedure.Parameters.Add(new StiDataParameterSchema
                                                {
                                                    Name = param.ParameterName,
                                                    Type = StiDbTypeConversion.GetNetType(param.DbType)
                                                });
                                                paramStr = paramStr.Length == 0 ? paramStr.Append(param.ParameterName) : paramStr.AppendFormat("{0},", param.ParameterName);
                                            }
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
            if (type == typeof(DateTime)) return (int)StiDbType.MySql.DateTime;
            if (type == typeof(TimeSpan)) return (int)StiDbType.MySql.Timestamp;

            if (type == typeof(Int64)) return (int)StiDbType.MySql.Int64;
            if (type == typeof(Int32)) return (int)StiDbType.MySql.Int32;
            if (type == typeof(Int16)) return (int)StiDbType.MySql.Int16;
            if (type == typeof(SByte)) return (int)StiDbType.MySql.Byte;

            if (type == typeof(UInt64)) return (int)StiDbType.MySql.UInt64;
            if (type == typeof(UInt32)) return (int)StiDbType.MySql.UInt32;
            if (type == typeof(UInt16)) return (int)StiDbType.MySql.UInt16;
            if (type == typeof(Byte)) return (int)StiDbType.MySql.UByte;

            if (type == typeof(Single)) return (int)StiDbType.MySql.Float;
            if (type == typeof(Double)) return (int)StiDbType.MySql.Double;
            if (type == typeof(Decimal)) return (int)StiDbType.MySql.Decimal;

            if (type == typeof(String)) return (int)StiDbType.MySql.VarChar;

            if (type == typeof(Boolean)) return (int)StiDbType.MySql.Bit;
            if (type == typeof(Char)) return (int)StiDbType.MySql.VarChar;
            if (type == typeof(Byte[])) return (int)StiDbType.MySql.Binary;
            if (type == typeof(Guid)) return (int)StiDbType.MySql.Guid;

            return (int)StiDbType.MySql.Int32;
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
                case "int64":
                case "year":                
                    return typeof(Int64);

                case "int32":
                case "int24":
                case "int":
                    return typeof(Int32);

                case "int16":
                case "smallint":
                    return typeof(Int16);

                case "byte":
                case "ubyte":
                    return typeof(Byte);

                case "uint32":
                case "uint24":
                    return typeof(UInt32);

                case "uint16":
                    return typeof(UInt16);

                case "tinyint":
                    return typeof(SByte);
                
                case "uint64":
                    return typeof(UInt64);

                case "decimal":
                case "newdecimal":
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

                case "newdatetime":
                case "smalldatetime":
                case "datetime":
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
            switch ((StiDbType.MySql)dbType)
            {
                case StiDbType.MySql.Int64:
                case StiDbType.MySql.Year:                
                    return typeof(Int64);

                case StiDbType.MySql.Int32:
                    return typeof(Int32);

                case StiDbType.MySql.Int16:
                    return typeof(Int16);

                case StiDbType.MySql.Byte:
                    return typeof(SByte);

                case StiDbType.MySql.UInt64:
                    return typeof(UInt64);

                case StiDbType.MySql.UInt32:
                    return typeof(UInt32);

                case StiDbType.MySql.UInt16:
                    return typeof(UInt16);

                case StiDbType.MySql.UByte:
                    return typeof(Byte);

                case StiDbType.MySql.Decimal:
                    return typeof(decimal);

                case StiDbType.MySql.Float:
                    return typeof(float);

                case StiDbType.MySql.Double:
                    return typeof(double);

                case StiDbType.MySql.Bit:
                    return typeof(Boolean);

                case StiDbType.MySql.DateTime:
                case StiDbType.MySql.Date:
                case StiDbType.MySql.Time:
                case StiDbType.MySql.Timestamp:
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
        #endregion

        #region Fields.Static
        private static object lockObject = new object();
        private static bool? isGeneric = null;
        private static bool? isDevart = null;
        #endregion

        #region Methods.Static
        public static StiMySqlConnector Get(string connectionString = null)
        {
            lock (lockObject)
            {
                if (isGeneric == true) return new StiMySqlConnector(connectionString);
                if (isDevart == true) return new StiMySqlDevartConnector(connectionString);
                if (connectionString == null)return new StiMySqlConnector();

                if (isGeneric != true && isDevart != true)
                {
                    isGeneric = null;
                    isDevart = null;
                }

                if (isGeneric == null)
                {
                    var connector = new StiMySqlConnector(connectionString);
                    isGeneric = connector.AssemblyHelper.IsAllowed;
                    if (isGeneric == true) return connector;
                }

                if (isDevart == null)
                {
                    var connector = new StiMySqlDevartConnector(connectionString);
                    isDevart = connector.AssemblyHelper.IsAllowed;
                    if (isDevart == true) return connector;
                }

                isGeneric = true;
                return new StiMySqlConnector(connectionString);
            }
        }

        public static void SetDevart()
        {
            isDevart = true;
        }

        /// <summary>
        /// Returns sample of the connection string to this connector.
        /// </summary>
        public override string GetSampleConnectionString()
        {
            return @"Server=myServerAddress; Database=myDataBase;" + Environment.NewLine +
                   @"UserId=myUsername; Pwd=myPassword;";
        }
        #endregion

        public StiMySqlConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "MySql.Data.dll";
            this.TypeConnection = "MySql.Data.MySqlClient.MySqlConnection";
            this.TypeDataAdapter = "MySql.Data.MySqlClient.MySqlDataAdapter";
            this.TypeCommand = "MySql.Data.MySqlClient.MySqlCommand";
            this.TypeParameter = "MySql.Data.MySqlClient.MySqlParameter";
            this.TypeDbType = "MySql.Data.MySqlClient.MySqlDbType";
            this.TypeCommandBuilder = "MySql.Data.MySqlClient.MySqlCommandBuilder";
        }
    }
}