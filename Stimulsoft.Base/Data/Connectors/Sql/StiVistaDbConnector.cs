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
using System.Drawing;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Base
{
    public class StiVistaDbConnector : StiSqlDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.VistaDbDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.VistaDbDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "VistaDB";
            }
        }
        

        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof(StiDbType.VistaDb);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.VistaDb.VarChar;
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

                    #region Tables
                    var tables = connection.GetSchema("Tables");

                    var tableHash = new Hashtable();
                    try
                    {

                        foreach (var row in StiSchemaRow.All(tables))
                        {
                            if (row.TABLE_TYPE == null || row.TABLE_TYPE == "System") continue;

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
                    var views = connection.GetSchema("Views");
                    var viewHash = new Hashtable();

                    try
                    {
                        foreach (var row in StiSchemaRow.All(views))
                        {
                            var table = StiDataTableSchema.NewView(row.TABLE_NAME);

                            viewHash[table.Name] = table;
                            schema.Views.Add(table);
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
                            if (tableHash[row.TABLE_NAME] == null) continue;

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
                    var procedures = connection.GetSchema("Procedures");

                    var procedureHash = new Hashtable();
                    try
                    {

                        foreach (var row in StiSchemaRow.All(procedures))
                        {
                            var table = StiDataTableSchema.NewProcedure(row.SPECIFIC_NAME);

                            procedureHash[table.Name] = table;
                            schema.StoredProcedures.Add(table);
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
            if (type == typeof(DateTime)) return (int)StiDbType.VistaDb.Date;
            if (type == typeof(TimeSpan)) return (int)StiDbType.VistaDb.Timestamp;

            if (type == typeof(Int64)) return (int)StiDbType.VistaDb.BigInt;
            if (type == typeof(Int32)) return (int)StiDbType.VistaDb.Int;
            if (type == typeof(Int16)) return (int)StiDbType.VistaDb.SmallInt;
            if (type == typeof(SByte)) return (int)StiDbType.VistaDb.TinyInt;

            if (type == typeof(UInt64)) return (int)StiDbType.VistaDb.BigInt;
            if (type == typeof(UInt32)) return (int)StiDbType.VistaDb.Int;
            if (type == typeof(UInt16)) return (int)StiDbType.VistaDb.SmallInt;
            if (type == typeof(Byte)) return (int)StiDbType.VistaDb.TinyInt;

            if (type == typeof(Single)) return (int)StiDbType.VistaDb.Float;
            if (type == typeof(Double)) return (int)StiDbType.VistaDb.Real;
            if (type == typeof(Decimal)) return (int)StiDbType.VistaDb.Decimal;

            if (type == typeof(String)) return (int)StiDbType.VistaDb.VarChar;

            if (type == typeof(Boolean)) return (int)StiDbType.VistaDb.Bit;
            if (type == typeof(Char)) return (int)StiDbType.VistaDb.VarChar;
            if (type == typeof(Byte[])) return (int)StiDbType.VistaDb.Binary;

            return (int)StiDbType.VistaDb.VarChar;
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(string dbType)
        {
            switch (dbType)
            {
                case "BigInt":
                case "UniqueIdentifier":
                case "Timestamp":
                    return typeof(Int64);

                case "Int":
                    return typeof(Int32);

                case "TinyInt":
                    return typeof(Int16);

                case "SmallInt":
                    return typeof(SByte);

                case "Decimal":
                case "Money":
                case "SmallMoney":
                    return typeof(Decimal);

                case "Float":
                    return typeof(Single);

                case "Real":
                    return typeof(Double);

                case "DateTime":
                case "DateTime2":
                case "SmallDateTime":
                case "Date":
                    return typeof(DateTime);

                case "DateTimeOffset":
                    return typeof(DateTimeOffset);

                case "Timespan":
                    return typeof(TimeSpan);

                case "Char":
                    return typeof(Char);

                case "Image":
                    return typeof(Image);

                case "Bit":
                    return typeof(Boolean);

                case "Binary":
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
            switch ((StiDbType.VistaDb)dbType)
            {
                case StiDbType.VistaDb.BigInt:
                case StiDbType.VistaDb.UniqueIdentifier:
                    return typeof(Int64);

                case StiDbType.VistaDb.Int:
                    return typeof(Int32);

                case StiDbType.VistaDb.TinyInt:
                    return typeof(Int16);

                case StiDbType.VistaDb.SmallInt:
                    return typeof(SByte);

                case StiDbType.VistaDb.Decimal:
                case StiDbType.VistaDb.Money:
                case StiDbType.VistaDb.SmallMoney:
                    return typeof(Decimal);

                case StiDbType.VistaDb.Float:
                    return typeof(Single);

                case StiDbType.VistaDb.Real:
                    return typeof(Double);

                case StiDbType.VistaDb.DateTime:
                case StiDbType.VistaDb.DateTime2:
                case StiDbType.VistaDb.SmallDateTime:
                case StiDbType.VistaDb.Date:
                    return typeof(DateTime);

                case StiDbType.VistaDb.DateTimeOffset:
                    return typeof(DateTime);

                case StiDbType.VistaDb.Timestamp:
                    return typeof(TimeSpan);

                case StiDbType.VistaDb.Char:
                    return typeof(Char);

                case StiDbType.VistaDb.Image:
                    return typeof(Image);

                case StiDbType.VistaDb.Bit:
                    return typeof(Boolean);

                case StiDbType.VistaDb.Binary:
                    return typeof(byte[]);

                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// Returns sample of the connection string to this connector.
        /// </summary>
        public override string GetSampleConnectionString()
        {
            return @"Data Source=D:\folder\myVistaDatabaseFile.vdb4;Open Mode=ExclusiveReadWrite;";
        }
        #endregion

        #region Methods.Static
        public static StiVistaDbConnector Get(string connectionString = null)
        {
            return new StiVistaDbConnector(connectionString);
        }
        #endregion

        public StiVistaDbConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "VistaDB.5.NET40.dll";
            this.TypeConnection = "VistaDB.Provider.VistaDBConnection";
            this.TypeDataAdapter = "VistaDB.Provider.VistaDBDataAdapter";
            this.TypeCommand = "VistaDB.Provider.VistaDBCommand";
            this.TypeParameter = "VistaDB.Provider.VistaDBParameter";
            this.TypeDbType = "VistaDB.VistaDBType";
            this.TypeCommandBuilder = "VistaDB.Provider.VistaDBCommandBuilder";
        }
    }
}