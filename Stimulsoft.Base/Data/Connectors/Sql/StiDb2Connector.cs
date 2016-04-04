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
    public class StiDb2Connector : StiSqlDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.Db2DataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.Db2DataSource;
            }
        }
        
        public override string Name
        {
            get
            {
                return "DB2";
            }
        }

        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof(StiDbType.Db2);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.Db2.VarChar;
            }
        }

        /// <summary>
        /// Gets the package identificator for this connector.
        /// </summary>
        public override string[] NuGetPackages
        {
            get
            {
                return new string[] { "IBM.Data.DB2" };
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
            if (type == typeof(DateTime)) return (int)StiDbType.Db2.Date;
            if (type == typeof(TimeSpan)) return (int)StiDbType.Db2.Timestamp;

            if (type == typeof(Int64)) return (int)StiDbType.Db2.BigInt;
            if (type == typeof(Int32)) return (int)StiDbType.Db2.Integer;
            if (type == typeof(Int16)) return (int)StiDbType.Db2.SmallInt;
            if (type == typeof(SByte)) return (int)StiDbType.Db2.Byte;

            if (type == typeof(UInt64)) return (int)StiDbType.Db2.BigInt;
            if (type == typeof(UInt32)) return (int)StiDbType.Db2.Integer;
            if (type == typeof(UInt16)) return (int)StiDbType.Db2.SmallInt;
            if (type == typeof(Byte)) return (int)StiDbType.Db2.Byte;

            if (type == typeof(Single)) return (int)StiDbType.Db2.Double;
            if (type == typeof(Double)) return (int)StiDbType.Db2.Double;
            if (type == typeof(Decimal)) return (int)StiDbType.Db2.Decimal;

            if (type == typeof(String)) return (int)StiDbType.Db2.VarChar;

            if (type == typeof(Boolean)) return (int)StiDbType.Db2.Byte;
            if (type == typeof(Char)) return (int)StiDbType.Db2.VarChar;
            if (type == typeof(Byte[])) return (int)StiDbType.Db2.Blob;

            return (int)StiDbType.Db2.VarChar;
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
            switch ((StiDbType.Db2)dbType)
            {
                case StiDbType.Db2.BigInt:
                    return typeof(Int64);

                case StiDbType.Db2.Integer:
                    return typeof(Int32);

                case StiDbType.Db2.SmallInt:
                    return typeof(Int16);

                case StiDbType.Db2.Byte:
                case StiDbType.Db2.Int8:
                    return typeof(Byte);

                case StiDbType.Db2.Decimal:
                case StiDbType.Db2.Numeric:
                case StiDbType.Db2.Money:
                    return typeof(Decimal);

                case StiDbType.Db2.Double:
                    return typeof(Double);

                case StiDbType.Db2.Float:
                case StiDbType.Db2.SmallFloat:
                    return typeof(Single);

                case StiDbType.Db2.Date:
                case StiDbType.Db2.DateTime:
                case StiDbType.Db2.Timestamp:
                case StiDbType.Db2.TimeStampWithTimeZone:
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
            return @"Server=myAddress:myPortNumber;Database=myDataBase;UID=myUsername;PWD=myPassword;" + Environment.NewLine +
                   @"Max Pool Size=100;Min Pool Size=10;";
        }
        #endregion

        #region Methods.Static
        public static StiDb2Connector Get(string connectionString = null)
        {
            return new StiDb2Connector(connectionString);
        }
        #endregion

        public StiDb2Connector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "IBM.Data.DB2.dll";
            this.TypeConnection = "IBM.Data.DB2.DB2Connection";
            this.TypeDataAdapter = "IBM.Data.DB2.DB2DataAdapter";
            this.TypeCommand = "IBM.Data.DB2.DB2Command";
            this.TypeParameter = "IBM.Data.DB2.DB2Parameter";
            this.TypeDbType = "IBM.Data.DB2.DB2Type";
            this.TypeCommandBuilder = "IBM.Data.DB2.DB2CommandBuilder";
        }
    }
}