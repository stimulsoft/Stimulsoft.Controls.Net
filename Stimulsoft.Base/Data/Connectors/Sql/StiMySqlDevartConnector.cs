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
    public class StiMySqlDevartConnector : StiMySqlConnector
    {
        #region Properties
        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof(StiDbType.Devart.MySql);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.Devart.MySql.Text;
            }
        }

        /// <summary>
        /// Gets the package identificator for this connector.
        /// </summary>
        public override string[] NuGetPackages
        {
            get
            {
                return new string[] { "dotConnect.Express.for.MySQL" };
            }
        }
        #endregion

        #region Methods
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
                        var tables = connection.GetSchema("Tables", new[] { connection.Database, null });

                        foreach (var row in StiSchemaRow.All(tables))
                        {
                            var tableSchema = StiDataTableSchema.NewTable(row.NAME, this);

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
                        var views = connection.GetSchema("Views", new[] { connection.Database, null });

                        foreach (var row in StiSchemaRow.All(views))
                        {
                            var viewSchema = StiDataTableSchema.NewView(row.NAME, this);

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
                        foreach (var table in tableList)
                        {
                            var columns = connection.GetSchema("Columns", new[] { connection.Database, table.Name, null });
                            foreach (var row in StiSchemaRow.All(columns))
                            {
                                var column = new StiDataColumnSchema(row.NAME, GetNetType(row.DATATYPE));
                                table.Columns.Add(column);
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
                        var procedures = connection.GetSchema("Procedures", new[] { connection.Database, null });

                        foreach (var row in StiSchemaRow.All(procedures))
                        {
                            var procedure = StiDataTableSchema.NewProcedure(row.NAME);

                            procedureHash[procedure.Name] = procedure;
                            schema.StoredProcedures.Add(procedure);
                        }
                    }
                    catch
                    {
                    }

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

                                        if (paramStr.Length > 0)
                                            procedure.Query = string.Format("{0} ({1})", procedure.Query, paramStr);
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
                            using (var dataSet = new DataSet())
                            {
                                var commandText = "SELECT * FROM information_schema.TABLE_CONSTRAINTS i " +
                                                  "LEFT JOIN information_schema.KEY_COLUMN_USAGE k " +
                                                  "ON i.CONSTRAINT_NAME = k.CONSTRAINT_NAME " +
                                                  "WHERE i.CONSTRAINT_TYPE = 'FOREIGN KEY' and i.TABLE_NAME = '{0}'";

                                using (var adapter = CreateAdapter(string.Format(commandText, schemaTable.Name), connection))
                                {
                                    adapter.Fill(dataSet, schemaTable.Name);

                                    var dataTable = (dataSet.Tables.Count > 0) ? dataSet.Tables[0] : null;
                                    if (dataTable != null)
                                    {
                                        foreach (var row in StiSchemaRow.All(dataTable))
                                        {
                                            schema.Relations.Add(new StiDataRelationSchema
                                            {
                                                Name = row.CONSTRAINT_NAME,
                                                ParentSourceName = row.TABLE_NAME,
                                                ChildSourceName = row.REFERENCED_TABLE_NAME,
                                                ParentColumns = new List<string> { row.COLUMN_NAME },
                                                ChildColumns = new List<string> { row.REFERENCED_COLUMN_NAME }
                                            });
                                        }
                                    }
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
        /// Returns new connection to this type of the database.
        /// </summary>
        /// <returns>Created connection.</returns>
        public override DbConnection CreateConnection()
        {
            var connection = base.CreateConnection();
            var prop = connection.GetType().GetProperty("Charset");
            prop.SetValue(connection, "auto", null);
            return connection;
        }

        /// <summary>
        /// Retrieves SQL parameters for the specified command.
        /// </summary>
        public override void DeriveParameters(DbCommand command)
        {
            var prop = command.GetType().GetProperty("ParameterCheck");
            prop.SetValue(command, true, null);
        }

        /// <summary>
        /// Returns a SQL based type from the .NET type.
        /// </summary>
        public override int GetSqlType(Type type)
        {
            if (type == typeof(DateTime)) return (int)StiDbType.Devart.MySql.DateTime;
            if (type == typeof(TimeSpan)) return (int)StiDbType.Devart.MySql.TimeStamp;

            if (type == typeof(Int64)) return (int)StiDbType.Devart.MySql.BigInt;
            if (type == typeof(Int32)) return (int)StiDbType.Devart.MySql.Int;
            if (type == typeof(Int16)) return (int)StiDbType.Devart.MySql.SmallInt;
            if (type == typeof(SByte)) return (int)StiDbType.Devart.MySql.TinyInt;

            if (type == typeof(UInt64)) return (int)StiDbType.Devart.MySql.BigInt;
            if (type == typeof(UInt32)) return (int)StiDbType.Devart.MySql.Int;
            if (type == typeof(UInt16)) return (int)StiDbType.Devart.MySql.SmallInt;
            if (type == typeof(Byte)) return (int)StiDbType.Devart.MySql.TinyInt;
            

            if (type == typeof(Single)) return (int)StiDbType.Devart.MySql.Float;
            if (type == typeof(Double)) return (int)StiDbType.Devart.MySql.Double;
            if (type == typeof(Decimal)) return (int)StiDbType.Devart.MySql.Decimal;

            if (type == typeof(String)) return (int)StiDbType.Devart.MySql.VarChar;

            if (type == typeof(Boolean)) return (int)StiDbType.Devart.MySql.SmallInt;
            if (type == typeof(Char)) return (int)StiDbType.Devart.MySql.Char;
            if (type == typeof(Byte[])) return (int)StiDbType.Devart.MySql.Binary;
            if (type == typeof(Guid)) return (int)StiDbType.Devart.MySql.Guid;

            return (int)StiDbType.Devart.MySql.Int;
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(int dbType)
        {
            switch ((StiDbType.Devart.MySql)dbType)
            {
                case StiDbType.Devart.MySql.BigInt:
                case StiDbType.Devart.MySql.Year:                
                    return typeof(Int64);

                case StiDbType.Devart.MySql.Int:
                    return typeof(Int32);

                case StiDbType.Devart.MySql.SmallInt:
                    return typeof(Int16);

                case StiDbType.Devart.MySql.TinyInt:
                    return typeof(SByte);

                case StiDbType.Devart.MySql.Decimal:
                    return typeof(decimal);

                case StiDbType.Devart.MySql.Float:
                    return typeof(float);

                case StiDbType.Devart.MySql.Double:
                    return typeof(double);

                case StiDbType.Devart.MySql.Bit:
                    return typeof(Boolean);

                case StiDbType.Devart.MySql.DateTime:
                case StiDbType.Devart.MySql.Date:
                case StiDbType.Devart.MySql.Time:
                case StiDbType.Devart.MySql.TimeStamp:
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
            return @"User ID=root;Password=myPassword;Host=localhost;Port=3306;Database=myDataBase;" + Environment.NewLine +
                   @"Direct=true;Protocol=TCP;Compress=false;Pooling=true;Min Pool Size=0;" + Environment.NewLine +
                   @"Max Pool Size=100;Connection Lifetime=0;";
        }
        #endregion

        public StiMySqlDevartConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "Devart.Data.MySql.dll";
            this.TypeConnection = "Devart.Data.MySql.MySqlConnection";
            this.TypeDataAdapter = "Devart.Data.MySql.MySqlDataAdapter";
            this.TypeCommand = "Devart.Data.MySql.MySqlCommand";
            this.TypeParameter = "Devart.Data.MySql.MySqlParameter";
            this.TypeDbType = "Devart.Data.MySql.MySqlType";
            this.TypeCommandBuilder = "Devart.Data.MySql.MySqlCommandBuilder";            
        }
    }
}