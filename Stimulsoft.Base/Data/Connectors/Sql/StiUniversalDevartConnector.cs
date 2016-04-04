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
using Stimulsoft.Base;

namespace Stimulsoft.Base
{
    public class StiUniversalDevartConnector : StiSqlDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.UniversalDevartDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.UniversalDevartDataSource;
            }
        }
        
        public override string Name
        {
            get
            {
                return "Universal (dotConnect)";
            }
        }

        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof(StiDbType.Universal);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.Universal.VarChar;
            }
        }
        #endregion

        #region Methods
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

                    string[] restrictions = null;
                    var provider = connection.GetType().GetProperty("Provider").GetValue(connection, null) as string;
                    if (provider == "MySQL")
                    {
                        restrictions = new[] { connection.Database };
                    }
                    else if (provider == "Oracle" || provider == "OracleClient")
                    {
                        var userId = connection.GetType().GetProperty("UserId").GetValue(connection, null) as string;
                        if (userId != null)
                            restrictions = new[] { userId.ToUpper() };
                    }
                    else
                        restrictions = new string[] { };


                    #region Tables
                    var tableHash = new Hashtable();

                    try
                    {
                        var tables = connection.GetSchema("Tables", restrictions);

                        var columnName = "TABLE_NAME";

                        foreach (DataRow row in tables.Rows)
                        {
                            var table = StiDataTableSchema.NewTable(row[columnName] as string, this);

                            if (row["TABLE_SCHEMA"] != DBNull.Value && ((string)row["TABLE_SCHEMA"]) != "dbo")
                                table = StiDataTableSchema.NewTable(string.Format("{0} ({1})", row["TABLE_NAME"] as string, row["TABLE_SCHEMA"] as string), this);

                            tableHash[table.Name] = table;
                            schema.Tables.Add(table);
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

                        foreach (DataRow row in columns.Rows)
                        {
                            var columnName = row["COLUMN_NAME"] as string;

                            string tableName;
                            if (row["TABLE_SCHEMA"] != DBNull.Value && ((string)row["TABLE_SCHEMA"]) != "sys" && ((string)row["TABLE_SCHEMA"]) != "dbo")
                                tableName = string.Format("{0} ({1})", row["TABLE_NAME"] as string, row["TABLE_SCHEMA"] as string);
                            else
                                tableName = row["TABLE_NAME"] as string;

                            var columnType = GetNetType(row["DATA_TYPE"] as string);

                            var column = new StiDataColumnSchema(columnName, columnType);
                            var table = tableHash[tableName] as StiDataTableSchema;
                            if (table != null)
                                table.Columns.Add(column);
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
            if (type == typeof(DateTime)) return (int)StiDbType.Universal.Date;
            if (type == typeof(TimeSpan)) return (int)StiDbType.Universal.TimeStamp;

            if (type == typeof(Int64)) return (int)StiDbType.Universal.BigInt;
            if (type == typeof(Int32)) return (int)StiDbType.Universal.Int;
            if (type == typeof(Int16)) return (int)StiDbType.Universal.SmallInt;
            if (type == typeof(SByte)) return (int)StiDbType.Universal.Byte;

            if (type == typeof(UInt64)) return (int)StiDbType.Universal.BigInt;
            if (type == typeof(UInt32)) return (int)StiDbType.Universal.Int;
            if (type == typeof(UInt16)) return (int)StiDbType.Universal.SmallInt;
            if (type == typeof(Byte)) return (int)StiDbType.Universal.Byte;

            if (type == typeof(Single)) return (int)StiDbType.Universal.Double;
            if (type == typeof(Double)) return (int)StiDbType.Universal.Double;
            if (type == typeof(Decimal)) return (int)StiDbType.Universal.Decimal;

            if (type == typeof(String)) return (int)StiDbType.Universal.VarChar;

            if (type == typeof(Boolean)) return (int)StiDbType.Universal.Byte;
            if (type == typeof(Char)) return (int)StiDbType.Universal.VarChar;
            if (type == typeof(Byte[])) return (int)StiDbType.Universal.Blob;

            return (int)StiDbType.Teradata.VarChar;
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(string dbType)
        {
            switch (dbType.ToLowerInvariant())
            {
                case "bigint":
                case "uniqueidentifier":
                    return typeof(Int64);

                case "int":
                    return typeof(Int32);
                
                case "tinyint":
                case "smallint":
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
                case "smalldatetime":
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
            switch ((StiDbType.Universal)dbType)
            {
                case StiDbType.Universal.BigInt:
                    return typeof(Int64);

                case StiDbType.Universal.Int:
                    return typeof(Int32);

                case StiDbType.Universal.SmallInt:
                    return typeof(Int16);

                case StiDbType.Universal.Byte:
                case StiDbType.Universal.TinyInt:
                    return typeof(Byte);

                case StiDbType.Universal.Decimal:
                case StiDbType.Universal.Currency:
                    return typeof(Decimal);

                case StiDbType.Universal.Double:
                    return typeof(Double);

                case StiDbType.Universal.Single:
                    return typeof(Single);

                case StiDbType.Universal.Date:
                case StiDbType.Universal.DateTime:
                case StiDbType.Universal.TimeStamp:
                case StiDbType.Universal.TimeStampTZ:
                    return typeof(DateTime);

                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// Returns sample of the connection string to this connector.
        /// </summary>
        public override string GetSampleConnectionString()
        {
            return @"Provider=Oracle;direct=true;data source=192.168.0.1;port=1521;sid=sid;user=user;password=pass";
        }
        #endregion

        #region Methods.Static
        public static StiUniversalDevartConnector Get(string connectionString = null)
        {
            return new StiUniversalDevartConnector(connectionString);
        }
        #endregion

        public StiUniversalDevartConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "Devart.Data.Universal.dll";
            this.TypeConnection = "Devart.Data.Universal.UniConnection";
            this.TypeDataAdapter = "Devart.Data.Universal.UniDataAdapter";
            this.TypeCommand = "Devart.Data.Universal.UniCommand";
            this.TypeParameter = "Devart.Data.Universal.UniParameter";
            this.TypeDbType = "Devart.Data.Universal.UniDbType";
            this.TypeCommandBuilder = "Devart.Data.Universal.UniCommandBuilder";
            this.TypeConnectionStringBuilder = "Devart.Data.Universal.UniConnectionStringBuilder";
            this.TypeDataSourceEnumerator = "Devart.Data.Universal.UniDataSourceEnumerator";
        }
    }
}