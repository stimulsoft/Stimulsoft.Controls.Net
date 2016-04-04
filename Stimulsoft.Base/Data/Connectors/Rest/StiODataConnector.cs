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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Stimulsoft.Base
{
    public class StiODataConnector : StiRestDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.ODataDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.ODataDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "OData";
            }
        }

        /// <summary>
        /// Get a value which indicates that this data connector can be used now.
        /// </summary>
        public override bool IsAvailable
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns schema object which contains information about structure of the database. Schema returned start at specified root element (if it applicable).
        /// </summary>
        public override StiDataSchema RetrieveSchema()
        {
            return new StiODataHelper(this.ConnectionString).RetrieveSchema();
        }

        public override void FillDataTable(DataTable table, string query)
        {
            new StiODataHelper(this.ConnectionString).FillDataTable(table, query);
        }

        /// <summary>
        /// Returns StiTestConnectionResult that is the information of whether the connection string specified in this class is correct.
        /// </summary>
        /// <returns>The result of testing the connection string.</returns>
        public override StiTestConnectionResult TestConnection()
        {
            return new StiODataHelper(this.ConnectionString).TestConnection();
        }

        /// <summary>
        /// Returns sample of the connection string to this connector.
        /// </summary>
        public override string GetSampleConnectionString()
        {
            return "http://services.odata.org/v3/odata/OData.svc/";
        }
        #endregion

        #region Methods.Static
        public static StiODataConnector Get(string connectionString = null)
        {
            return new StiODataConnector(connectionString);
        }
        #endregion

        public StiODataConnector(string connectionString = null)
            : base(connectionString)
        {
            this.NameAssembly = "System.Data.SqlServerCe.dll";
        }
    }
}