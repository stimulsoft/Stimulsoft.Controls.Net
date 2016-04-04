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
    public class StiSqLiteConnector : StiSqlDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.SqLiteDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.SqLiteDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "SQLite";
            }
        }
        
        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof(StiDbType.SqLite);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.SqLite.Text;
            }
        }

        /// <summary>
        /// Gets the package identificator for this connector.
        /// </summary>
        public override string[] NuGetPackages
        {
            get
            {
                return new string[] { "System.Data.SQLite.Core" };
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

                    #region Views
                    try
                    {
                        var views = connection.GetSchema("Views");

                        foreach (var row in StiSchemaRow.All(views))
                        {
                            var viewSchema = StiDataTableSchema.NewView(row.TABLE_NAME);

                            tableList.Add(viewSchema);
                            schema.Views.Add(viewSchema);
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
            if (type == typeof(DateTime)) return (int)StiDbType.SqLite.DateTime;
            if (type == typeof(TimeSpan)) return (int)StiDbType.SqLite.Int64;

            if (type == typeof(Int64)) return (int)StiDbType.SqLite.Int64;
            if (type == typeof(Int32)) return (int)StiDbType.SqLite.Int64;
            if (type == typeof(Int16)) return (int)StiDbType.SqLite.Int64;
            if (type == typeof(Byte)) return (int)StiDbType.SqLite.Int64;

            if (type == typeof(UInt64)) return (int)StiDbType.SqLite.Int64;
            if (type == typeof(UInt32)) return (int)StiDbType.SqLite.Int64;
            if (type == typeof(UInt16)) return (int)StiDbType.SqLite.Int64;
            if (type == typeof(SByte)) return (int)StiDbType.SqLite.Int64;

            if (type == typeof(Single)) return (int)StiDbType.SqLite.Double;
            if (type == typeof(Double)) return (int)StiDbType.SqLite.Double;
            if (type == typeof(Decimal)) return (int)StiDbType.SqLite.Double;

            if (type == typeof(String)) return (int)StiDbType.SqLite.Text;

            if (type == typeof(Boolean)) return (int)StiDbType.SqLite.Int64;
            if (type == typeof(Char)) return (int)StiDbType.SqLite.Text;
            if (type == typeof(Byte[])) return (int)StiDbType.SqLite.Blob;
            if (type == typeof(Guid)) return (int)StiDbType.SqLite.Text;

            return (int)StiDbType.SqLite.Int64;
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(int dbType)
        {
            switch ((StiDbType.SqLite)dbType)
            {
                case StiDbType.SqLite.Int64:
                    return typeof(Int64);

                case StiDbType.SqLite.Blob:
                    return typeof(byte[]);

                case StiDbType.SqLite.DateTime:
                    return typeof(DateTime);

                case StiDbType.SqLite.Double:
                    return typeof(Double);

                case StiDbType.SqLite.Text:
                    return typeof(Int64);

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
                case "int64":
                    return typeof(Int64);

                case "double":
                    return typeof(double);
                
                case "datetime":
                    return typeof(DateTime);

                case "blob":
                    return typeof(byte[]);

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
            return @"Data Source=c:\mydb.db; Version=3;";
        }
        #endregion

        #region Methods.Static
        public static StiSqLiteConnector Get(string connectionString = null)
        {
            return new StiSqLiteConnector(connectionString);
        }
        #endregion

        public StiSqLiteConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "System.Data.SQLite.dll";
            this.TypeConnection = "System.Data.SQLite.SQLiteConnection";
            this.TypeDataAdapter = "System.Data.SQLite.SQLiteDataAdapter";
            this.TypeCommand = "System.Data.SQLite.SQLiteCommand";
            this.TypeParameter = "System.Data.SQLite.SQLiteParameter";
            this.TypeDbType = "System.Data.SQLite.SQLiteType";
        }
    }
}