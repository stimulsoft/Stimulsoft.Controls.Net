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
    public class StiXmlConnector : StiFileDataConnector
    {
        #region Properties
        /// <summary>
        /// Gets a type of the connection helper.
        /// </summary>
        public override StiConnectionIdent ConnectionIdent
        {
            get
            {
                return StiConnectionIdent.XmlDataSource;
            }
        }

        /// <summary>
        /// Gets an order of the connector.
        /// </summary>
        public override StiConnectionOrder ConnectionOrder
        {
            get
            {
                return StiConnectionOrder.XmlDataSource;
            }
        }

        public override string Name
        {
            get
            {
                return "XML";
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
                return StiFileType.Xml;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns DataSet based on specified options.
        /// </summary>
        public override DataSet GetDataSet(StiFileDataOptions options)
        {
            var xmlOptions = options as StiXmlOptions;
            if (xmlOptions == null) throw new NotSupportedException("Only StiXmlOptions accepted!");

            if (xmlOptions.Content == null || xmlOptions.Content.Length == 0) return null;

            //ADO.NET XML
            if (xmlOptions.IsAdoNet)
            {
                var dataSet = new DataSet { EnforceConstraints = false };
                dataSet.Read(xmlOptions.Schema, xmlOptions.Content);
                return dataSet;
            }
            //XML
            return StiJsonToDataSetConverter.GetDataSetFromXml(xmlOptions.Content);
        }
        #endregion

        #region Methods.Static
        public static StiXmlConnector Get()
        {
            return new StiXmlConnector();
        }
        #endregion

        private StiXmlConnector()
        {
        }
    }
}