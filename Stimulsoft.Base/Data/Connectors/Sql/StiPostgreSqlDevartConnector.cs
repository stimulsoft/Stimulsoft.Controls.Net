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
    public class StiPostgreSqlDevartConnector : StiPostgreSqlConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.PostgreSqlDataSource;
            }
        }

        /// <summary>
        /// Gets the type of an enumeration which describes data types.
        /// </summary>
        public override Type SqlType
        {
            get
            {
                return typeof(StiDbType.Devart.PostgreSql);
            }
        }

        /// <summary>
        /// Gets the default value of the data parameter.
        /// </summary>
        public override int DefaultSqlType
        {
            get
            {
                return (int)StiDbType.Devart.PostgreSql.Text;
            }
        }

        /// <summary>
        /// Gets the package identificator for this connector.
        /// </summary>
        public override string[] NuGetPackages
        {
            get
            {
                return new string[] { "dotConnect.Express.for.PostgreSQL" };
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a SQL based type from the .NET type.
        /// </summary>
        public override int GetSqlType(Type type)
        {
            if (type == typeof(DateTime)) return (int)StiDbType.Devart.PostgreSql.Date;
            if (type == typeof(TimeSpan)) return (int)StiDbType.Devart.PostgreSql.Interval;

            if (type == typeof(Int64)) return (int)StiDbType.Devart.PostgreSql.BigInt;
            if (type == typeof(Int32)) return (int)StiDbType.Devart.PostgreSql.Int;
            if (type == typeof(Int16)) return (int)StiDbType.Devart.PostgreSql.SmallInt;
            if (type == typeof(Byte)) return (int)StiDbType.Devart.PostgreSql.SmallInt;

            if (type == typeof(UInt64)) return (int)StiDbType.Devart.PostgreSql.BigInt;
            if (type == typeof(UInt32)) return (int)StiDbType.Devart.PostgreSql.Int;
            if (type == typeof(UInt16)) return (int)StiDbType.Devart.PostgreSql.SmallInt;
            if (type == typeof(SByte)) return (int)StiDbType.Devart.PostgreSql.SmallInt;

            if (type == typeof(Single)) return (int)StiDbType.Devart.PostgreSql.Real;
            if (type == typeof(Double)) return (int)StiDbType.Devart.PostgreSql.Double;
            if (type == typeof(Decimal)) return (int)StiDbType.Devart.PostgreSql.Numeric;

            if (type == typeof(String)) return (int)StiDbType.Devart.PostgreSql.Text;

            if (type == typeof(Boolean)) return (int)StiDbType.Devart.PostgreSql.Boolean;
            if (type == typeof(Char)) return (int)StiDbType.Devart.PostgreSql.Char;
            if (type == typeof(Byte[])) return (int)StiDbType.Devart.PostgreSql.ByteA;
            if (type == typeof(Guid)) return (int)StiDbType.Devart.PostgreSql.Uuid;

            return (int)StiDbType.PostgreSql.Integer;
        }

        /// <summary>
        /// Returns a .NET type from the specified string representaion of the database type.
        /// </summary>
        public override Type GetNetType(int dbType)
        {
            switch ((StiDbType.Devart.PostgreSql)dbType)
            {
                case StiDbType.Devart.PostgreSql.BigInt:
                case StiDbType.Devart.PostgreSql.Int:
                case StiDbType.Devart.PostgreSql.Numeric:
                    return typeof(Int64);

                case StiDbType.Devart.PostgreSql.SmallInt:
                    return typeof(Int16);

                case StiDbType.Devart.PostgreSql.Money:
                    return typeof(decimal);

                case StiDbType.Devart.PostgreSql.Real:
                    return typeof(float);

                case StiDbType.Devart.PostgreSql.Double:
                    return typeof(double);

                case StiDbType.Devart.PostgreSql.Date:
                case StiDbType.Devart.PostgreSql.TimeStamp:
                case StiDbType.Devart.PostgreSql.Time:
                case StiDbType.Devart.PostgreSql.TimeTZ:
                case StiDbType.Devart.PostgreSql.TimeStampTZ:
                    return typeof(DateTime);

                case StiDbType.Devart.PostgreSql.Boolean:
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
            return @"User ID=root;Password=myPassword;Host=localhost;Port=5432;Database=myDataBase;" + Environment.NewLine +
                   @"Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Lifetime=0;";
        }
        #endregion

        public StiPostgreSqlDevartConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "Devart.Data.PostgreSql.dll";
            this.TypeConnection = "Devart.Data.PostgreSql.PgSqlConnection";
            this.TypeDataAdapter = "Devart.Data.PostgreSql.PgSqlDataAdapter";
            this.TypeCommand = "Devart.Data.PostgreSql.PgSqlCommand";
            this.TypeParameter = "Devart.Data.PostgreSql.PgSqlParameter";
            this.TypeDbType = "Devart.Data.PostgreSql.PgSqlType";
            this.TypeCommandBuilder = "Devart.Data.PostgreSql.PgSqlCommandBuilder";
        }
    }
}