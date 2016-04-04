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
using System.Text.RegularExpressions;
using Stimulsoft.Base;

namespace Stimulsoft.Base
{
    public class StiOracleClientConnector : StiOracleConnector
    {
        #region Properties
        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof(StiDbType.OracleClient);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.OracleClient.NVarChar;
            }
        }


        /// <summary>
        /// Gets the package identificator for this connector.
        /// </summary>
        public override string[] NuGetPackages
        {
            get
            {
                return null;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a SQL based type from the .NET type.
        /// </summary>
        public override int GetSqlType(Type type)
        {
            if (type == typeof(DateTime)) return (int)StiDbType.OracleClient.DateTime;
            if (type == typeof(TimeSpan)) return (int)StiDbType.OracleClient.Timestamp;

            if (type == typeof(Int64)) return (int)StiDbType.OracleClient.Number;
            if (type == typeof(Int32)) return (int)StiDbType.OracleClient.Int32;
            if (type == typeof(Int16)) return (int)StiDbType.OracleClient.Int16;
            if (type == typeof(SByte)) return (int)StiDbType.OracleClient.SByte;

            if (type == typeof(UInt64)) return (int)StiDbType.OracleClient.Number;
            if (type == typeof(UInt32)) return (int)StiDbType.OracleClient.Int32;
            if (type == typeof(UInt16)) return (int)StiDbType.OracleClient.Int16;
            if (type == typeof(Byte)) return (int)StiDbType.OracleClient.Byte;

            if (type == typeof(Single)) return (int)StiDbType.OracleClient.Float;
            if (type == typeof(Double)) return (int)StiDbType.OracleClient.Double;
            if (type == typeof(Decimal)) return (int)StiDbType.OracleClient.Number;

            if (type == typeof(String)) return (int)StiDbType.OracleClient.VarChar;

            if (type == typeof(Boolean)) return (int)StiDbType.OracleClient.Byte;
            if (type == typeof(Char)) return (int)StiDbType.OracleClient.Char;
            if (type == typeof(Byte[])) return (int)StiDbType.OracleClient.Blob;
            if (type == typeof(Guid)) return (int)StiDbType.OracleClient.Raw;

            return (int)StiDbType.OracleClient.Int32;
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(int dbType)
        {
            switch ((StiDbType.OracleClient)dbType)
            {
                case StiDbType.OracleClient.DateTime:
                    return typeof(byte[]);

                case StiDbType.OracleClient.BFile:
                case StiDbType.OracleClient.Blob:
                case StiDbType.OracleClient.LongRaw:
                case StiDbType.OracleClient.Raw:
                    return typeof(byte[]);

                case StiDbType.OracleClient.Float:
                    return typeof(Single);

                case StiDbType.OracleClient.Double:
                    return typeof(Double);

                case StiDbType.OracleClient.Number:
                    return typeof(Decimal);

                case StiDbType.OracleClient.SByte:
                    return typeof(SByte);

                case StiDbType.OracleClient.Int16:
                    return typeof(Int16);

                case StiDbType.OracleClient.Int32:
                    return typeof(Int32);

                case StiDbType.OracleClient.Byte:
                    return typeof(Byte);

                case StiDbType.OracleClient.UInt16:
                    return typeof(UInt16);

                case StiDbType.OracleClient.UInt32:
                    return typeof(UInt32);

                default:
                    return typeof(string);
            }
        }
        #endregion

        public StiOracleClientConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "System.Data.OracleClient.dll";
            this.TypeConnection = "System.Data.OracleClient.OracleConnection";
            this.TypeDataAdapter = "System.Data.OracleClient.OracleDataAdapter";
            this.TypeCommand = "System.Data.OracleClient.OracleCommand";
            this.TypeParameter = "System.Data.OracleClient.OracleParameter";
            this.TypeDbType = "System.Data.OracleClient.OracleDbType";
            this.TypeCommandBuilder = "System.Data.OracleClient.OracleCommandBuilder";
            this.TypeConnectionStringBuilder = "Oracle.DataAccess.Client.OracleConnectionStringBuilder";
        }
    }
}