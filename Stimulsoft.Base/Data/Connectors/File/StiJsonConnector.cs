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
    public class StiJsonConnector : StiFileDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.JsonDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.JsonDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "JSON";
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

        /// <summary>
        /// A type of the file which can be processed with this connection helper.
        /// </summary>
        public override StiFileType FileType
        {
            get
            {
                return StiFileType.Json;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns DataSet based on specified options.
        /// </summary>
        public override DataSet GetDataSet(StiFileDataOptions options)
        {
            var jsonOptions = options as StiJsonOptions;
            if (jsonOptions == null) throw new NotSupportedException("Only StiJsonOptions accepted!");

            if (jsonOptions.Content == null || jsonOptions.Content.Length == 0) return null;

            return StiJsonToDataSetConverter.GetDataSet(jsonOptions.Content);
        }
        #endregion

        #region Methods.Static
        public static StiJsonConnector Get()
        {
            return new StiJsonConnector();
        }
        #endregion

        private StiJsonConnector()
        {
        }
    }
}