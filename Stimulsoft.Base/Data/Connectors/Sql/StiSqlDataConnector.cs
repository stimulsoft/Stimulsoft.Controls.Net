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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Stimulsoft.Base;

namespace Stimulsoft.Base
{
    public abstract class StiSqlDataConnector : StiDataConnector
    {
        #region Fields
        public string NameAssembly;
        public string TypeConnection;
        public string TypeDataAdapter;
        public string TypeCommand;
        public string TypeParameter;
        public string TypeDbType;
        public string TypeCommandBuilder;
        public string TypeConnectionStringBuilder;
        public string TypeDataSourceEnumerator;
        public string MethodDeriveParameters = "DeriveParameters";
        #endregion

        #region Properties
        /// <summary>
        /// Gets connection string to the database.
        /// </summary>
        public string ConnectionString { get; set; }

        
        private StiDataAssemblyHelper assemblyHelper;
        /// <summary>
        /// Gets AssemblyHelper object which helps in interaction with data provider assembly.
        /// </summary>
        protected StiDataAssemblyHelper AssemblyHelper
        {
            get
            {
                if (NameAssembly == null)
                    throw new NotSupportedException("NameAssembly is not specified!");

                return assemblyHelper ?? (assemblyHelper = new StiDataAssemblyHelper(NameAssembly));
            }
        }

        /// <summary>
        /// Get a value which indicates that this data connector can be used now.
        /// </summary>
        public override bool IsAvailable
        {
            get
            {
                return AssemblyHelper != null && AssemblyHelper.IsAllowed;
            }
        }

        /// <summary>
        /// Gets a type of the connection which is used for this connector.
        /// </summary>
        public virtual Type ConnectionType
        {
            get
            {
                return AssemblyHelper.GetType(TypeConnection);
            }
        }

        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public abstract Type SqlType
        {
            get;
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public abstract int DefaultSqlType
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets timeout to the specified command.
        /// </summary>
        public virtual void SetTimeout(DbCommand command, int timeOut)
        {
            command.CommandTimeout = timeOut;
        }

        /// <summary>
        /// Sets timeout to the specified command.
        /// </summary>
        public virtual void SetTimeout(IDbCommand command, int timeOut)
        {
            SetTimeout(command as DbCommand, timeOut);
        }

        /// <summary>
        /// Returns StiTestConnectionResult that is the information of whether the connection string specified in this class is correct.
        /// </summary>
        /// <returns>The result of testing the connection string.</returns>
        public virtual StiTestConnectionResult TestConnection()
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    connection.Close();
                }
            }
            catch (Exception exception)
            {
                return StiTestConnectionResult.MakeWrong(exception.Message);
            }

            return StiTestConnectionResult.MakeFine();
        }

        /// <summary>
        /// Returns new connection to this type of the database.
        /// </summary>
        /// <returns>Created connection.</returns>
        public virtual DbConnection CreateConnection()
        {
            return AssemblyHelper.CreateConnection(TypeConnection, this.ConnectionString);
        }

        /// <summary>
        /// Returns new data adapter to this type of the database.
        /// </summary>
        /// <param name="query">A SQL query.</param>
        /// <param name="connection">A connection to database.</param>
        /// <param name="commandType">A type of the query.</param>
        /// <returns>Created adapter.</returns>
        public virtual DbDataAdapter CreateAdapter(string query, DbConnection connection, CommandType commandType = CommandType.Text)
        {
            var adapter = AssemblyHelper.CreateAdapter(TypeDataAdapter, query, connection);
            adapter.SelectCommand.CommandType = commandType;
            return adapter;
        }

        /// <summary>
        /// Returns new data command for this type of the database.
        /// </summary>
        /// <param name="query">A SQL query.</param>
        /// <param name="connection">A connection to database.</param>
        /// <param name="commandType">A type of the command.</param>
        /// <returns>Created command.</returns>
        public virtual DbCommand CreateCommand(string query, DbConnection connection, CommandType commandType = CommandType.Text)
        {
            var command = AssemblyHelper.CreateCommand(TypeCommand, query, connection);
            command.CommandType = commandType;
            return command;
        }

        /// <summary>
        /// Returns new SQL parameter with specified parameter.
        /// </summary>
        public virtual DbParameter CreateParameter(string parameterName, object value, int size = 0)
        {
            return AssemblyHelper.CreateParameterWithValueAndSize(TypeParameter, parameterName, value, size);
        }

        /// <summary>
        /// Returns new SQL parameter with specified parameter.
        /// </summary>
        public virtual DbParameter CreateParameter(string parameterName, int type, int size = 0)
        {
            var dbType = GetDbType();
            return AssemblyHelper.CreateParameterWithTypeAndSize(TypeParameter, parameterName, type, size, dbType ?? typeof(int));
        }

        /// <summary>
        /// Returns new CommandBuilder.
        /// </summary>
        public virtual DbCommandBuilder CreateCommandBuilder()
        {
            return AssemblyHelper.CreateCommandBuilder(TypeCommandBuilder);
        }

        /// <summary>
        /// Returns new ConnectionStringBuilder.
        /// </summary>
        public virtual DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return AssemblyHelper.CreateConnectionStringBuilder(TypeConnectionStringBuilder, this.ConnectionString);
        }

        /// <summary>
        /// Returns new DataSourceEnumerator.
        /// </summary>
        public virtual DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            return AssemblyHelper.CreateDataSourceEnumerator(TypeDataSourceEnumerator);
        }

        /// <summary>
        /// Retrieves SQL parameters for the specified command.
        /// </summary>
        public virtual void DeriveParameters(DbCommand command)
        {
            try
            {
                if (TypeCommandBuilder == null || MethodDeriveParameters == null)
                    throw new NotSupportedException();

                AssemblyHelper.DeriveParameters(TypeCommandBuilder, MethodDeriveParameters, command);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Returns schema object which contains information about structure of the database. Schema returned start at specified root element (if it applicable).
        /// </summary>
        public abstract StiDataSchema RetrieveSchema();

        /// <summary>
        /// Returns a SQL based type from the .NET type.
        /// </summary>
        public abstract int GetSqlType(Type type);

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public abstract Type GetNetType(string dbType);

        /// <summary>
        /// Returns a .NET type from the specified int representaion of the database type.
        /// </summary>
        public abstract Type GetNetType(int dbType);

        /// <summary>
        /// Bracketing string with specials characters
        /// </summary>
        /// <param name="name">unput string</param>
        /// <returns>Bracketed string</returns>
        public virtual string GetDatabaseSpecificName(string name)
        {
            return name;
        }
        
        public DataTable GetSchemaTable(DbCommand command, bool isStoredProc = false)
        {
            var dataTable = new DataTable("Schema");

            using (var reader = command.ExecuteReader(StiDataOptions.RetriveColumnsMode == StiRetrieveColumnsMode.KeyInfo ? CommandBehavior.KeyInfo : CommandBehavior.SchemaOnly))
            {
                dataTable.Load(reader);
            }

            return dataTable;
        }

        public DataTable GetDataTable(DbCommand command, int index, int count)
        {
            var dataTable = new DataTable("Data");

            using (var reader = command.ExecuteReader())
            {
                #region Columns
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);
                    dataTable.Columns.Add(columnName);
                }
                #endregion

                #region Seek to the first row by index
                for (var i = 0; i < index && reader.Read(); i++)
                {
                }
                #endregion

                #region Read rows limited by count
                for (var i = 0; i < count && reader.Read(); i++)
                {
                    var values = new Object[reader.FieldCount];
                    int fieldCount = reader.GetValues(values);
                    if (fieldCount > 0)
                        dataTable.Rows.Add(values);
                }
                #endregion
            }
            return dataTable;
        }

        public DataTable GetParametersTable(DbCommand command)
        {
            var paramTable = new DataTable("Parameters");
            paramTable.Columns.Add("Name");
            paramTable.Columns.Add("Type");
            paramTable.Columns.Add("Direction");

            foreach (DbParameter param in command.Parameters)
            {
                if (param.Direction == ParameterDirection.Input)
                    paramTable.Rows.Add(param.ParameterName, StiDbTypeConversion.GetNetType(param.DbType).ToString(), param.Direction.ToString());
            }

            return paramTable;
        }

        protected virtual DataTable GetRelationsTable(DbConnection connection, string tableName)
        {
            return null;
        }


        /// <summary>
        /// Returns sample of the connection string to this connector.
        /// </summary>
        public abstract string GetSampleConnectionString();

        /// <summary>
        /// Returns the type of the DBType.
        /// </summary>
        public virtual Type GetDbType()
        {
            return AssemblyHelper.GetType(TypeDbType);
        }
        #endregion

        #region Methods.Static
        public static StiSqlDataConnector Get(StiConnectionIdent ident, string connectionString)
        {
            switch (ident)
            {
                case StiConnectionIdent.Db2DataSource:
                    return StiDb2Connector.Get(connectionString);

                case StiConnectionIdent.FirebirdDataSource:
                    return StiFirebirdConnector.Get(connectionString);

                case StiConnectionIdent.InformixDataSource:
                    return StiInformixConnector.Get(connectionString);

                case StiConnectionIdent.MsAccessDataSource:
                    return StiMsAccessConnector.Get(connectionString);

                case StiConnectionIdent.MsSqlDataSource:
                    return StiMsSqlConnector.Get(connectionString);

                case StiConnectionIdent.MySqlDataSource:
                    return StiMySqlConnector.Get(connectionString);

                case StiConnectionIdent.OdbcDataSource:
                    return StiOdbcConnector.Get(connectionString);

                case StiConnectionIdent.OleDbDataSource:
                    return StiOleDbConnector.Get(connectionString);

                case StiConnectionIdent.OracleDataSource:
                    return StiOracleConnector.Get(connectionString);

                case StiConnectionIdent.PostgreSqlDataSource:
                    return StiPostgreSqlConnector.Get(connectionString);

                case StiConnectionIdent.SqlCeDataSource:
                    return StiSqlCeConnector.Get(connectionString);

                case StiConnectionIdent.SqLiteDataSource:
                    return StiSqLiteConnector.Get(connectionString);

                case StiConnectionIdent.SybaseDataSource:
                    return StiSybaseConnector.Get(connectionString);

                case StiConnectionIdent.TeradataDataSource:
                    return StiTeradataConnector.Get(connectionString);

                case StiConnectionIdent.VistaDbDataSource:
                    return StiVistaDbConnector.Get(connectionString);

                case StiConnectionIdent.UniversalDevartDataSource:
                    return StiUniversalDevartConnector.Get(connectionString);


                case StiConnectionIdent.ODataDataSource:
                    return StiODataConnector.Get(connectionString);

                default:
                    return null;
            }
        }
        #endregion

        protected StiSqlDataConnector(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}
