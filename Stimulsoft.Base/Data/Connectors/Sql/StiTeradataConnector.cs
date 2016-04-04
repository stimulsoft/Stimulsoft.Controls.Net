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
    public class StiTeradataConnector : StiSqlDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.TeradataDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.TeradataDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "Teradata";
            }
        }
        

        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof(StiDbType.Teradata);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.Teradata.VarChar;
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

                    var restrictionsArray = new[] { connection.Database };
                    var tables = connection.GetSchema("Tables", restrictionsArray);
                    var tableHash = new Hashtable();

                    try
                    {
                        foreach (var row in StiSchemaRow.All(tables))
                        {
                            var tableName = string.Format("{0}.{1}", row.TABLE_SCHEMA, row.TABLE_NAME);
                            var table = StiDataTableSchema.NewTableOrView(tableName);
                            tableHash[tableName] = table;

                            if (row.TABLE_TYPE == "TABLE") schema.Tables.Add(table);
                            if (row.TABLE_TYPE == "VIEW") schema.Views.Add(table);
                        }
                    }
                    catch
                    {
                    }

                    #region Columns
                    try
                    {
                        var colRestrictionsArray = new[] { connection.Database };
                        var columns = connection.GetSchema("Columns", colRestrictionsArray);

                        foreach (var row in StiSchemaRow.All(columns))
                        {
                            var tableName = string.Format("{0}.{1}", row.TABLE_SCHEMA, row.TABLE_NAME);
                            if (tableHash[tableName] == null) continue;

                            var column = new DataColumn(row.COLUMN_NAME, GetNetType(row.COLUMN_TYPE));
                            
                            var table = tableHash[tableName] as DataTable;
                            if (table != null)table.Columns.Add(column);
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
            if (type == typeof(DateTime)) return (int)StiDbType.Teradata.Date;
            if (type == typeof(TimeSpan)) return (int)StiDbType.Teradata.Timestamp;

            if (type == typeof(Int64)) return (int)StiDbType.Teradata.BigInt;
            if (type == typeof(Int32)) return (int)StiDbType.Teradata.Integer;
            if (type == typeof(Int16)) return (int)StiDbType.Teradata.SmallInt;
            if (type == typeof(SByte)) return (int)StiDbType.Teradata.Byte;

            if (type == typeof(UInt64)) return (int)StiDbType.Teradata.BigInt;
            if (type == typeof(UInt32)) return (int)StiDbType.Teradata.Integer;
            if (type == typeof(UInt16)) return (int)StiDbType.Teradata.SmallInt;
            if (type == typeof(Byte)) return (int)StiDbType.Teradata.Byte;

            if (type == typeof(Single)) return (int)StiDbType.Teradata.Double;
            if (type == typeof(Double)) return (int)StiDbType.Teradata.Double;
            if (type == typeof(Decimal)) return (int)StiDbType.Teradata.Decimal;

            if (type == typeof(String)) return (int)StiDbType.Teradata.VarChar;

            if (type == typeof(Boolean)) return (int)StiDbType.Teradata.Byte;
            if (type == typeof(Char)) return (int)StiDbType.Teradata.VarChar;
            if (type == typeof(Byte[])) return (int)StiDbType.Teradata.Blob;

            return (int)StiDbType.Teradata.VarChar;
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(string dbType)
        {
            dbType = dbType.Trim().ToUpper();

            if (dbType.StartsWith("PERIOD"))
                return typeof(string);

            switch (dbType)
            {
                case "SMALLINT":
                case "BYTEINT":
                    return typeof(Int16);

                case "INTEGER":
                    return typeof(Int32);

                case "BIGINT":
                    return typeof(Int64);

                case "BLOB":
                case "BYTE":
                case "VARBYTE":
                    return typeof(byte[]);

                case "DECIMAL":
                    return typeof(decimal);

                case "NUMBER":
                case "DOUBLE":
                    return typeof(double);

                case "INTERVALDAY":
                case "INTERVALDAYTOHOUR":
                case "INTERVALDAYTOMINUTE":
                case "INTERVALDAYTOSECOND":
                case "INTERVALHOUR":
                case "INTERVALHOURTOMINUTE":
                case "INTERVALHOURTOSECOND":
                case "INTERVALMINUTE":
                case "INTERVALMINUTETOSECOND":
                case "INTERVALSECOND":
                case "TIME":
                    return typeof(TimeSpan);

                case "DATE":
                case "TIMESTAMP":
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
            switch ((StiDbType.Teradata)dbType)
            {
                case StiDbType.Teradata.AnyType:
                    return typeof(Object);

                case StiDbType.Teradata.BigInt:
                    return typeof(Int64);

                case StiDbType.Teradata.Integer:
                    return typeof(Int32);

                case StiDbType.Teradata.SmallInt:
                    return typeof(Int16);

                case StiDbType.Teradata.ByteInt:
                    return typeof(SByte);

                case StiDbType.Teradata.Blob:
                case StiDbType.Teradata.Byte:
                case StiDbType.Teradata.VarByte:
                    return typeof(Byte[]);
                
                case StiDbType.Teradata.Char:
                case StiDbType.Teradata.Clob:
                case StiDbType.Teradata.Graphic:
                case StiDbType.Teradata.IntervalMonth:
                case StiDbType.Teradata.IntervalYear:
                case StiDbType.Teradata.IntervalYearToMonth:
                case StiDbType.Teradata.TimeWithZone:
                case StiDbType.Teradata.TimestampWithZone:
                case StiDbType.Teradata.PeriodTimeWithTimeZone:
                case StiDbType.Teradata.PeriodTimestampWithTimeZone:
                case StiDbType.Teradata.VarChar:
                case StiDbType.Teradata.VarGraphic:
                case StiDbType.Teradata.PeriodDate:
                case StiDbType.Teradata.PeriodTime:
                case StiDbType.Teradata.PeriodTimestamp:
                    return typeof(string);

                case StiDbType.Teradata.Date:
                case StiDbType.Teradata.Timestamp:
                    return typeof(DateTime);

                case StiDbType.Teradata.Decimal:
                    return typeof(Decimal);

                case StiDbType.Teradata.Double:
                case StiDbType.Teradata.Number:
                    return typeof(Double);

                case StiDbType.Teradata.IntervalDay:
                case StiDbType.Teradata.IntervalDayToHour:
                case StiDbType.Teradata.IntervalDayToMinute:
                case StiDbType.Teradata.IntervalDayToSecond:
                case StiDbType.Teradata.IntervalHour:
                case StiDbType.Teradata.IntervalHourToMinute:
                case StiDbType.Teradata.IntervalHourToSecond:
                case StiDbType.Teradata.IntervalMinute:
                case StiDbType.Teradata.IntervalMinuteToSecond:
                case StiDbType.Teradata.IntervalSecond:
                case StiDbType.Teradata.Time:
                    return typeof(TimeSpan);

                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// Returns sample of the connection string to this connector.
        /// </summary>
        public override string GetSampleConnectionString()
        {
            return @"Data Source=myServerAddress;User ID=myUsername;Password=myPassword;";
        }
        #endregion

        #region Methods.Static
        public static StiTeradataConnector Get(string connectionString = null)
        {
            return new StiTeradataConnector(connectionString);
        }
        #endregion

        public StiTeradataConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "Teradata.Client.Provider.dll";
            this.TypeConnection = "Teradata.Client.Provider.TdConnection";
            this.TypeDataAdapter = "Teradata.Client.Provider.TdDataAdapter";
            this.TypeCommand = "Teradata.Client.Provider.TdCommand";
            this.TypeParameter = "Teradata.Client.Provider.TdParameter";
            this.TypeDbType = "Teradata.Client.Provider.TdType";
            this.TypeCommandBuilder = "Teradata.Client.Provider.TdCommandBuilder";
        }
    }
}