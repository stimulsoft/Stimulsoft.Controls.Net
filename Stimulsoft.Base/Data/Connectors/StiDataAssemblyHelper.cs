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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Stimulsoft.Base;

namespace Stimulsoft.Base
{
    public class StiDataAssemblyHelper
    {
        #region Methods
        internal DbConnection CreateConnection(string typeName, string connectionString)
        {
            return CreateObject(typeName, connectionString) as DbConnection;
        }

        internal DbDataAdapter CreateAdapter(string typeName, string query, DbConnection connection)
        {
            return CreateObject(typeName, query, connection) as DbDataAdapter;
        }

        internal DbCommand CreateCommand(string typeName, string query, DbConnection connection)
        {
            return CreateObject(typeName, query, connection) as DbCommand;
        }

        internal DbParameter CreateParameterWithValueAndSize(string typeName, string parameterName, object value, int size)
        {
            return CreateObject(typeName, parameterName, value, size) as DbParameter;
        }

        internal DbParameter CreateParameterWithValue(string typeName, string parameterName, object value)
        {
            return CreateObject(typeName, parameterName, value) as DbParameter;
        }

        internal DbParameter CreateParameterWithType(string typeName, string parameterName, int type, Type dbType)
        {
            return CreateObject(typeName, parameterName, StiConvert.ChangeType(type, dbType)) as DbParameter;
        }

        internal DbParameter CreateParameterWithTypeAndSize(string typeName, string parameterName, int type, int size, Type dbType)
        {
            return CreateObject(typeName, parameterName, StiConvert.ChangeType(type, dbType), size) as DbParameter;
        }

        internal DbCommandBuilder CreateCommandBuilder(string typeName)
        {
            return CreateObject(typeName) as DbCommandBuilder;
        }

        internal DbConnectionStringBuilder CreateConnectionStringBuilder(string typeName, string connectionString = null)
        {
            return CreateObject(typeName, connectionString) as DbConnectionStringBuilder;
        }

        internal DbDataSourceEnumerator CreateDataSourceEnumerator(string typeName)
        {
            return CreateObject(typeName) as DbDataSourceEnumerator;
        }

        /// <summary>
        /// Retrieves SQL parameters for the specified command.
        /// </summary>
        internal void DeriveParameters(string typeName, string methodName, DbCommand command)
        {
            try
            {
                var type = GetType(typeName);
                var method = type.GetMethod(methodName);
                if (method != null)
                    method.Invoke(null, new object[] { command });
            }
            catch (Exception e)
            {
                if (e.InnerException != null) throw e.InnerException;
                throw;
            }
        }

        internal object CreateObject(string name, params object[] args)
        {
            try
            {
                var type = GetType(name);

                var argTypes = args != null ? args.ToList().Select(a => a != null ? a.GetType() : typeof(object)).ToArray() : null;

                var constructor = type.GetConstructor(argTypes);
                return constructor.Invoke(args);
            }
            catch (Exception e)
            {
                if (e.InnerException != null) throw e.InnerException;
                throw;
            }
        }

        internal Type GetType(string typeName)
        {
            var asm = Assembly;
            if (asm == null)
                throw new Exception(string.Format("\"{0}\" assembly is not found!", assemblyName));

            var type = asm.GetType(typeName);

            if (type == null)
                throw new Exception(string.Format("\"{0}\" type is not found!", typeName));

            return type;
        }
        #endregion

        #region Fields
        private string assemblyName;
        #endregion

        #region Properties
        public bool IsAllowed
        {
            get
            {
                return Assembly != null;
            }
        }

        private Assembly assembly;
        private Assembly Assembly
        {
            get
            {
                try
                {
                    if (assembly == null)
                        assembly = StiAssemblyFinder.GetAssembly(assemblyName);

                    if (assembly == null)
                        throw new Exception(string.Format("\"{0}\" assembly is not found!", assemblyName));

                    return assembly;
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion

        internal StiDataAssemblyHelper(string assemblyName)
        {
            this.assemblyName = assemblyName;
        }
    }
}