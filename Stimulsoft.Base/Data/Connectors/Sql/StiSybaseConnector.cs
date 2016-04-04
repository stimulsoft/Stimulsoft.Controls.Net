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
    public class StiSybaseConnector : StiSqlDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.SybaseDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.SybaseDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "Sybase";
            }
        }
        

        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                //return typeof(StiDbType.Sybase);
                return null;
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return 0;
                //return (int)StiDbType.Sybase.VarChar;
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
                    var tables = new DataTable("Tables");
                    
                    try
                    {
                        using (var adapter = this.CreateAdapter("SELECT id, name FROM sysobjects where type = 'U'", connection))
                        {
                            adapter.Fill(tables);

                            foreach (DataRow row in tables.Rows)
                            {
                                var table = StiDataTableSchema.NewTable(row["name"] as string, this);

                                var columns = new DataTable("Columns");
                                using (var columnAdapter = this.CreateAdapter("SELECT name, type FROM syscolumns where id = " + row["id"], connection))
                                {
                                    columnAdapter.Fill(columns);
                                    foreach (DataRow rowColumns in columns.Rows)
                                    {
                                        var typeColumn = this.GetNetType((byte)rowColumns["type"]);
                                        table.Columns.Add(new StiDataColumnSchema(rowColumns["name"].ToString(), typeColumn));
                                    }
                                }

                                schema.Tables.Add(table);

                            }
                        }
                    }
                    catch
                    {
                    }
                    #endregion

                    #region Views
                    var views = new DataTable("Views");
                    try
                    {
                        using (var adapter = this.CreateAdapter("SELECT name FROM sysobjects where type = 'V'", connection))
                        {
                            adapter.Fill(views);

                            foreach (DataRow row in views.Rows)
                            {
                                var view = StiDataTableSchema.NewView(row[0] as string, this);

                                var columns = new DataTable("Columns");
                                using (var columnAdapter = this.CreateAdapter("SELECT name, type FROM syscolumns where id = " + row["id"], connection))
                                {
                                    columnAdapter.Fill(columns);
                                    foreach (DataRow rowColumns in columns.Rows)
                                    {
                                        var typeColumn = this.GetNetType((byte)rowColumns["type"]);
                                        view.Columns.Add(new StiDataColumnSchema(rowColumns["name"].ToString(), typeColumn));
                                    }
                                }

                                schema.Views.Add(view);
                            }
                        }
                    }
                    catch
                    {
                    }
                    #endregion

                    #region Procedures
                    var procedures = new DataTable("Procedures");
                    try
                    {
                        using (var adapter = CreateAdapter("SELECT name FROM sysobjects where type = 'P'", connection))
                        {
                            adapter.Fill(procedures);

                            foreach (DataRow row in procedures.Rows)
                            {
                                var procedure = StiDataTableSchema.NewProcedure(row[0] as string, this);
                                schema.StoredProcedures.Add(procedure);
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
            if (type == typeof(DateTime)) return (int)StiDbType.Sybase.Date;
            if (type == typeof(TimeSpan)) return (int)StiDbType.Sybase.TimeStamp;

            if (type == typeof(Int64)) return (int)StiDbType.Sybase.BigInt;
            if (type == typeof(Int32)) return (int)StiDbType.Sybase.Integer;
            if (type == typeof(Int16)) return (int)StiDbType.Sybase.SmallInt;
            if (type == typeof(SByte)) return (int)StiDbType.Sybase.SmallInt;

            if (type == typeof(UInt64)) return (int)StiDbType.Sybase.BigInt;
            if (type == typeof(UInt32)) return (int)StiDbType.Sybase.Integer;
            if (type == typeof(UInt16)) return (int)StiDbType.Sybase.SmallInt;
            if (type == typeof(Byte)) return (int)StiDbType.Sybase.TinyInt;

            if (type == typeof(Single)) return (int)StiDbType.Sybase.Real;
            if (type == typeof(Double)) return (int)StiDbType.Sybase.Double;
            if (type == typeof(Decimal)) return (int)StiDbType.Sybase.Decimal;

            if (type == typeof(String)) return (int)StiDbType.Sybase.VarChar;

            if (type == typeof(Boolean)) return (int)StiDbType.Sybase.Bit;
            if (type == typeof(Char)) return (int)StiDbType.Sybase.VarChar;
            if (type == typeof(Byte[])) return (int)StiDbType.Sybase.Binary;

            return (int)StiDbType.Sybase.VarChar;
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
            switch ((StiDbType.Sybase)dbType)
            {
                case StiDbType.Sybase.BigInt:
                    return typeof(Int64);

                case StiDbType.Sybase.Integer:
                    return typeof(Int32);

                case StiDbType.Sybase.SmallInt:
                    return typeof(Int16);

                case StiDbType.Sybase.TinyInt:
                    return typeof(Int16);

                case StiDbType.Sybase.UnsignedBigInt:
                    return typeof(UInt64);

                case StiDbType.Sybase.UnsignedInt:
                    return typeof(UInt32);

                case StiDbType.Sybase.UnsignedSmallInt:
                    return typeof(UInt16);

                case StiDbType.Sybase.Bit:
                    return typeof(bool);

                case StiDbType.Sybase.Decimal:
                case StiDbType.Sybase.Money:
                case StiDbType.Sybase.SmallMoney:
                    return typeof(decimal);

                case StiDbType.Sybase.Double:
                    return typeof(double);

                case StiDbType.Sybase.Numeric:
                case StiDbType.Sybase.Real:
                    return typeof(float);

                case StiDbType.Sybase.BigDateTime:
                case StiDbType.Sybase.Date:
                case StiDbType.Sybase.SmallDateTime:
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
            return @"Data Source=myASEserver;Port=5000;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";
        }
        #endregion

        #region Methods.Static
        public static StiSybaseConnector Get(string connectionString = null)
        {
            return new StiSybaseConnector(connectionString);
        }
        #endregion

        public StiSybaseConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "Sybase.Data.AseClient.dll";
            this.TypeConnection = "Sybase.Data.AseClient.AseConnection";
            this.TypeDataAdapter = "Sybase.Data.AseClient.AseDataAdapter";
            this.TypeCommand = "Sybase.Data.AseClient.AseCommand";
            this.TypeParameter = "Sybase.Data.AseClient.AseParameter";
            this.TypeDbType = "Sybase.Data.AseClient.AseDbType";
            this.TypeCommandBuilder = "Sybase.Data.AseClient.AseCommandBuilder";
        }
    }
}