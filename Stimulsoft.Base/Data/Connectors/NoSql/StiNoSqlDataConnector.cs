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
    public abstract class StiNoSqlDataConnector : StiDataConnector
    {
        #region Fields
        public string NameAssembly;
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

        public virtual bool AllowTestConnection
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns StiTestConnectionResult that is the information of whether the connection string specified in this class is correct.
        /// </summary>
        /// <returns>The result of testing the connection string.</returns>
        public abstract StiTestConnectionResult TestConnection();

        /// <summary>
        /// Returns schema object which contains information about structure of the database. Schema returned start at specified root element (if it applicable).
        /// </summary>
        public abstract StiDataSchema RetrieveSchema();

        /// <summary>
        /// Returns sample of the connection string to this connector.
        /// </summary>
        public abstract string GetSampleConnectionString();

        public abstract List<StiDataColumnSchema> GetColumns(string collectionName);

        public abstract DataTable GetDataTable(string collectionName, string query, int? index = null, int? count = null);
        #endregion

        #region Methods.Static
        public static StiNoSqlDataConnector Get(StiConnectionIdent ident, string connectionString)
        {
            switch (ident)
            {
                case StiConnectionIdent.MongoDbDataSource:
                    return StiMongoDbConnector.Get(connectionString);

                default:
                    return null;
            }
        }
        #endregion

        protected StiNoSqlDataConnector(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}
