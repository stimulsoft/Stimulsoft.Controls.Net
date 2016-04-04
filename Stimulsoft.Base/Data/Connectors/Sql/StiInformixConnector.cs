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
    public class StiInformixConnector : StiSqlDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.InformixDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.InformixDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "Informix";
            }
        }

        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof(StiDbType.Informix);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.Informix.Text;
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
            return null;
        }

        /// <summary>
        /// Returns a SQL based type from the .NET type.
        /// </summary>
        public override int GetSqlType(Type type)
        {
            if (type == typeof(DateTime)) return (int)StiDbType.Informix.Date;

            if (type == typeof(Int64)) return (int)StiDbType.Informix.BigInt;
            if (type == typeof(Int32)) return (int)StiDbType.Informix.Integer;
            if (type == typeof(Int16)) return (int)StiDbType.Informix.SmallInt;
            if (type == typeof(Byte)) return (int)StiDbType.Informix.SmallInt;

            if (type == typeof(UInt64)) return (int)StiDbType.Informix.BigInt;
            if (type == typeof(UInt32)) return (int)StiDbType.Informix.Integer;
            if (type == typeof(UInt16)) return (int)StiDbType.Informix.SmallInt;
            if (type == typeof(SByte)) return (int)StiDbType.Informix.SmallInt;

            if (type == typeof(Single)) return (int)StiDbType.Informix.Float;
            if (type == typeof(Double)) return (int)StiDbType.Informix.Double;
            if (type == typeof(Decimal)) return (int)StiDbType.Informix.Decimal;

            if (type == typeof(String)) return (int)StiDbType.Informix.Text;

            if (type == typeof(Boolean)) return (int)StiDbType.Informix.SmallInt;
            if (type == typeof(Char)) return (int)StiDbType.Informix.Char;
            if (type == typeof(Byte[])) return (int)StiDbType.Informix.Binary;
            if (type == typeof(Guid)) return (int)StiDbType.Informix.Text;

            return (int)StiDbType.Informix.Text;
        }
        
        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(string dbType)
        {
           switch (dbType.ToLowerInvariant())
            {
                case "BigInt":
                    return typeof(Int64);

                case "Integer":
                    return typeof(Int32);

                case "SmallInt":
                    return typeof(Int16);

                case "Decimal":
                    return typeof(Decimal);

                case "Float":
                    return typeof(Single);

                case "Double":
                    return typeof(Double);

                case "Date":
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
            switch ((StiDbType.Informix)dbType)
            {
                case StiDbType.Informix.BigInt:
                case StiDbType.Informix.Numeric:
                    return typeof(Int64);

                case StiDbType.Informix.Integer:
                    return typeof(Int32);

                case StiDbType.Informix.SmallInt:
                    return typeof(Int16);

                case StiDbType.Informix.Decimal:
                    return typeof(Decimal);

                case StiDbType.Informix.Float:
                    return typeof(Single);

                case StiDbType.Informix.Double:
                    return typeof(Double);

                case StiDbType.Informix.Date:
                case StiDbType.Informix.Time:
                    return typeof(DateTime);

                case StiDbType.Informix.Boolean:
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
            return @"Database=myDataBase;Host=192.168.10.10;Server=db_engine_tcp;Service=1492;" + Environment.NewLine +
                   @"Protocol=onsoctcp;UID=myUsername;Password=myPassword;";
        }
        #endregion

        #region Methods.Static
        public static StiInformixConnector Get(string connectionString = null)
        {
            return new StiInformixConnector(connectionString);
        }
        #endregion

        protected internal StiInformixConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "IBM.Data.Informix.dll";
            this.TypeConnection = "IBM.Data.Informix.IfxConnection";
            this.TypeDataAdapter = "IBM.Data.Informix.IfxDataAdapter";
            this.TypeCommand = "IBM.Data.Informix.IfxCommand";
            this.TypeParameter = "IBM.Data.Informix.IfxParameter";
            this.TypeDbType = "IBM.Data.Informix.IfxType";
            this.TypeCommandBuilder = "IBM.Data.Informix.IfxCommandBuilder";
        }
    }
}