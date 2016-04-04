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

namespace Stimulsoft.Base
{
    public class StiFirebirdConnector : StiSqlDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.FirebirdDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.FirebirdDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "Firebird";
            }
        }

        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof (StiDbType.Firebird);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.Firebird.Text;
            }
        }

        /// <summary>
        /// Gets the package identificator for this connector.
        /// </summary>
        public override string[] NuGetPackages
        {
            get
            {
                return new string[] { "FirebirdSql.Data.FirebirdClient" };
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
                    var tableHash = new Hashtable();
                    try
                    {
                        var tables = connection.GetSchema("Tables", new[] { null, null, null, "TABLE" });
                        foreach (var row in StiSchemaRow.All(tables))
                        {
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
                            var view = StiDataTableSchema.NewView(row.VIEW_NAME);

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
                            var column = new StiDataColumnSchema(row.COLUMN_NAME, GetNetType(row.COLUMN_DATA_TYPE));

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
                            var procedure = StiDataTableSchema.NewProcedure(row.PROCEDURE_NAME);

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
                            var parameters = connection.GetSchema("procedureparameters", new[] { null, null, procedure.Name });

                            foreach (var row in StiSchemaRow.All(parameters))
                            {
                                if (row.PARAMETER_DIRECTION != "1") continue;

                                procedure.Parameters.Add(new StiDataParameterSchema(row.PARAMETER_NAME, GetNetType(row.PARAMETER_DATA_TYPE)));
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
                        const string commandText = @"
                            SELECT rc.RDB$CONSTRAINT_NAME AS constraint_name,
                            i.RDB$RELATION_NAME AS table_name,
                            s.RDB$FIELD_NAME AS field_name,
                            i2.RDB$RELATION_NAME AS references_table,
                            s2.RDB$FIELD_NAME AS references_field,
                            (s.RDB$FIELD_POSITION + 1) AS field_position
                            FROM RDB$INDEX_SEGMENTS s
                            LEFT JOIN RDB$INDICES i ON i.RDB$INDEX_NAME = s.RDB$INDEX_NAME
                            LEFT JOIN RDB$RELATION_CONSTRAINTS rc ON rc.RDB$INDEX_NAME = s.RDB$INDEX_NAME
                            LEFT JOIN RDB$REF_CONSTRAINTS refc ON rc.RDB$CONSTRAINT_NAME = refc.RDB$CONSTRAINT_NAME
                            LEFT JOIN RDB$RELATION_CONSTRAINTS rc2 ON rc2.RDB$CONSTRAINT_NAME = refc.RDB$CONST_NAME_UQ
                            LEFT JOIN RDB$INDICES i2 ON i2.RDB$INDEX_NAME = rc2.RDB$INDEX_NAME
                            LEFT JOIN RDB$INDEX_SEGMENTS s2 ON i2.RDB$INDEX_NAME = s2.RDB$INDEX_NAME
                            WHERE rc.RDB$CONSTRAINT_TYPE = 'FOREIGN KEY'
                            ORDER BY i.RDB$RELATION_NAME";

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
                                        Name = row.CONSTRAINT_NAME.Trim(),
                                        ParentSourceName = row.TABLE_NAME.Trim(),
                                        ChildSourceName = row.REFERENCES_TABLE.Trim(),
                                        ParentColumns = new List<string> { row.FIELD_NAME.Trim() },
                                        ChildColumns = new List<string> { row.REFERENCES_FIELD.Trim() }
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

                return schema;
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
            if (type == typeof(DateTime)) return (int)StiDbType.Firebird.Date;
            if (type == typeof(TimeSpan)) return (int)StiDbType.Firebird.TimeStamp;

            if (type == typeof(Int64)) return (int)StiDbType.Firebird.BigInt;
            if (type == typeof(Int32)) return (int)StiDbType.Firebird.Integer;
            if (type == typeof(Int16)) return (int)StiDbType.Firebird.SmallInt;
            if (type == typeof(Byte)) return (int)StiDbType.Firebird.SmallInt;

            if (type == typeof(UInt64)) return (int)StiDbType.Firebird.BigInt;
            if (type == typeof(UInt32)) return (int)StiDbType.Firebird.Integer;
            if (type == typeof(UInt16)) return (int)StiDbType.Firebird.SmallInt;
            if (type == typeof(SByte)) return (int)StiDbType.Firebird.SmallInt;

            if (type == typeof(Single)) return (int)StiDbType.Firebird.Float;
            if (type == typeof(Double)) return (int)StiDbType.Firebird.Double;
            if (type == typeof(Decimal)) return (int)StiDbType.Firebird.Decimal;

            if (type == typeof(String)) return (int)StiDbType.Firebird.VarChar;

            if (type == typeof(Boolean)) return (int)StiDbType.Firebird.SmallInt;
            if (type == typeof(Char)) return (int)StiDbType.Firebird.Char;
            if (type == typeof(Byte[])) return (int)StiDbType.Firebird.Binary;
            if (type == typeof(Guid)) return (int)StiDbType.Firebird.Guid;

            return (int)StiDbType.Firebird.Integer;
        }
        
        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(string dbType)
        {
            switch (dbType.ToLowerInvariant())
            {
                case "bigint":
				case "numeric":
                case "uniqueidentifier":
                case "timestamp":
                    return typeof(Int64);

                case "int":
                case "integer":
                    return typeof(Int32);

                case "smallint":
                    return typeof(Int16);

                case "decimal":
				case "money":
                case "smallmoney":
                    return typeof(Decimal);

                case "float":
				case "real":
                    return typeof(Single);

                case "double":
                    return typeof(Double);

                case "datetime":
                case "time":
                case "date":
                case "smalldatetime":
                    return typeof(DateTime);

                case "boolean":
                    return typeof(Boolean);

                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(int dbType)
        {
            switch ((StiDbType.Firebird)dbType)
            {
                case StiDbType.Firebird.BigInt:
                case StiDbType.Firebird.Numeric:
                case StiDbType.Firebird.TimeStamp:
                    return typeof(Int64);

                case StiDbType.Firebird.Integer:
                    return typeof(Int32);

                case StiDbType.Firebird.SmallInt:
                    return typeof(Int16);

                case StiDbType.Firebird.Decimal:
                    return typeof(Decimal);

                case StiDbType.Firebird.Float:
                    return typeof(Single);

                case StiDbType.Firebird.Double:
                    return typeof(Double);

                case StiDbType.Firebird.Date:
                case StiDbType.Firebird.Time:
                    return typeof(DateTime);

                case StiDbType.Firebird.Boolean:
                    return typeof(Boolean);

                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// Returns sample of the connection string to this connector.
        /// </summary>
        public override string GetSampleConnectionString()
        {
            return @"User=SYSDBA; Password=masterkey; Database=SampleDatabase.fdb;" + Environment.NewLine +
                   @"DataSource=myServerAddress; Port=3050; Dialect=3; Charset=NONE;" + Environment.NewLine +
                   @"Role=; Connection lifetime=15; Pooling=true; MinPoolSize=0;" + Environment.NewLine +
                   @"MaxPoolSize=50; Packet Size=8192; ServerType=0;";
        }
        #endregion

        #region Methods.Static
        public static StiFirebirdConnector Get(string connectionString = null)
        {
            return new StiFirebirdConnector(connectionString);
        }
        #endregion

        public StiFirebirdConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "FirebirdSql.Data.FirebirdClient.dll";
            this.TypeConnection = "FirebirdSql.Data.FirebirdClient.FbConnection";
            this.TypeDataAdapter = "FirebirdSql.Data.FirebirdClient.FbDataAdapter";
            this.TypeCommand = "FirebirdSql.Data.FirebirdClient.FbCommand";
            this.TypeParameter = "FirebirdSql.Data.FirebirdClient.FbParameter";
            this.TypeDbType = "FirebirdSql.Data.FirebirdClient.FbDbType";
            this.TypeCommandBuilder = "FirebirdSql.Data.FirebirdClient.FbCommandBuilder";
        }
    }
}