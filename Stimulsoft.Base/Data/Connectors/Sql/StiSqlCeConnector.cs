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
using System.Data.Common;
using System.Linq;

namespace Stimulsoft.Base
{
    public class StiSqlCeConnector : StiSqlDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.SqlCeDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.SqlCeDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "SQL CE";
            }
        }
        
        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof(StiDbType.SqlCe);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.SqlCe.VarChar;
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
        /// Sets timeout to the specified command.
        /// </summary>
        public override void SetTimeout(DbCommand command, int timeOut)
        {
            //SQL CE does not support this operation
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
                    var tableList = new List<StiDataTableSchema>();
                    try
                    {
                        var tables = connection.GetSchema("Tables");

                        foreach (var row in StiSchemaRow.All(tables))
                        {
                            var tableSchema = StiDataTableSchema.NewTable(row.TABLE_NAME);

                            tableList.Add(tableSchema);
                            schema.Tables.Add(tableSchema);
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
                        foreach (var table in tableList)
                        {
                            foreach (var row in StiSchemaRow.All(columns))
                            {
                                if (row.TABLE_NAME == null || row.TABLE_NAME != table.Name) continue;

                                var column = new StiDataColumnSchema(row.COLUMN_NAME, GetNetType(row.DATA_TYPE));
                                table.Columns.Add(column);
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
            if (type == typeof(DateTime)) return (int)StiDbType.SqlCe.DateTime;
            if (type == typeof(DateTimeOffset)) return (int)StiDbType.SqlCe.DateTimeOffset;
            if (type == typeof(TimeSpan)) return (int)StiDbType.SqlCe.BigInt;

            if (type == typeof(Int64)) return (int)StiDbType.SqlCe.BigInt;
            if (type == typeof(Int32)) return (int)StiDbType.SqlCe.Int;
            if (type == typeof(Int16)) return (int)StiDbType.SqlCe.SmallInt;
            if (type == typeof(Byte)) return (int)StiDbType.SqlCe.SmallInt;

            if (type == typeof(UInt64)) return (int)StiDbType.SqlCe.BigInt;
            if (type == typeof(UInt32)) return (int)StiDbType.SqlCe.Int;
            if (type == typeof(UInt16)) return (int)StiDbType.SqlCe.SmallInt;
            if (type == typeof(SByte)) return (int)StiDbType.SqlCe.SmallInt;

            if (type == typeof(Single)) return (int)StiDbType.SqlCe.Float;
            if (type == typeof(Double)) return (int)StiDbType.SqlCe.Real;
            if (type == typeof(Decimal)) return (int)StiDbType.SqlCe.Decimal;

            if (type == typeof(String)) return (int)StiDbType.SqlCe.VarChar;

            if (type == typeof(Boolean)) return (int)StiDbType.SqlCe.SmallInt;
            if (type == typeof(Char)) return (int)StiDbType.SqlCe.Char;
            if (type == typeof(Byte[])) return (int)StiDbType.SqlCe.Binary;
            if (type == typeof(Guid)) return (int)StiDbType.SqlCe.UniqueIdentifier;

            return (int)StiDbType.SqlCe.Int;
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(int dbType)
        {
            switch ((StiDbType.SqlCe)dbType)
            {
                case StiDbType.SqlCe.UniqueIdentifier:
                case StiDbType.SqlCe.BigInt:
                case StiDbType.SqlCe.Timestamp:
                    return typeof(Int64);

                case StiDbType.SqlCe.Int:
                    return typeof(Int32);

                case StiDbType.SqlCe.SmallInt:
                    return typeof(Int16);

                case StiDbType.SqlCe.TinyInt:
                    return typeof(Byte);

                case StiDbType.SqlCe.Decimal:
                case StiDbType.SqlCe.Money:
                case StiDbType.SqlCe.SmallMoney:
                    return typeof(decimal);

                case StiDbType.SqlCe.Float:
                    return typeof(float);

                case StiDbType.SqlCe.Real:
                    return typeof(double);

                case StiDbType.SqlCe.DateTime:
                case StiDbType.SqlCe.Date:
                case StiDbType.SqlCe.Time:
                case StiDbType.SqlCe.DateTime2:
                case StiDbType.SqlCe.SmallDateTime:
                    return typeof(DateTime);

                case StiDbType.SqlCe.DateTimeOffset:
                    return typeof(DateTimeOffset);

                case StiDbType.SqlCe.Bit:
                    return typeof(Boolean);

                default:
                    return typeof(string);
            }
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
                
                case "smallint":
                    return typeof(SByte);

                case "tinyint":
                case "int16":
                    return typeof(Int16);

                case "int32":
                case "int":
                    return typeof(Int32);

                case "int24":
                case "int64":

                case "byte":
                    return typeof(Byte);

                case "ubyte":
                
                case "uint16":
                    return typeof(UInt16);

                case "uint32":
                case "uint24":
                    return typeof(UInt32);
                
                case "year":
                    return typeof(Int64);

                case "uint64":
                    return typeof(UInt64);

                case "decimal":
                case "newdecimal":
                case "money":
                case "smallmoney":
                    return typeof(Decimal);

                case "float":
                case "real":
                    return typeof(float);
                
                case "double":
                    return typeof(double);

                case "bit":
                    return typeof(Boolean);

                case "newdatetime":
                case "smalldatetime":
                case "timestamp":
                case "datetime":
                case "date":
                case "time":
                    return typeof(DateTime);

                case "datetimeoffset":
                    return typeof(DateTimeOffset);

                case "image":
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
            return @"Data Source=c:\MyData.sdf; Persist Security Info=False;";
        }
        #endregion

        #region Methods.Static
        public static StiSqlCeConnector Get(string connectionString = null)
        {
            return new StiSqlCeConnector(connectionString);
        }
        #endregion

        public StiSqlCeConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "System.Data.SqlServerCe.dll";
            this.TypeConnection = "System.Data.SqlServerCe.SqlCeConnection";
            this.TypeDataAdapter = "System.Data.SqlServerCe.SqlCeDataAdapter";
            this.TypeCommand = "System.Data.SqlServerCe.SqlCeCommand";
            this.TypeParameter = "System.Data.SqlServerCe.SqlCeParameter";
            this.TypeDbType = "System.Data.SqlServerCe.SqlCeType";
        }
    }
}