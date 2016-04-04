#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports												}
{	                         										}
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
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base
{
    #region StiDataFormatType
    public enum StiDataFormatType
    {
        Xml,
        Json
    }
    #endregion

    #region StiRetrieveColumnsMode
    public enum StiRetrieveColumnsMode
    {
        KeyInfo,
        SchemaOnly,
        FillSchema
    }
    #endregion

    #region StiWizardStoredProcRetriveMode
    public enum StiWizardStoredProcRetriveMode
    {
        All,
        ParametersOnly
    }
    #endregion

    #region StiConnectionIdent
    /// <summary>
    /// An enumeration which contains a types of the data sources.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StiConnectionIdent
    {
        Db2DataSource = 1,
        InformixDataSource,
        MsAccessDataSource,
        MsSqlDataSource,
        MySqlDataSource,
        OdbcDataSource,
        OleDbDataSource,
        FirebirdDataSource,
        PostgreSqlDataSource,
        OracleDataSource,
        SqlCeDataSource,
        SqLiteDataSource,
        SybaseDataSource,
        TeradataDataSource,
        VistaDbDataSource,
        UniversalDevartDataSource,

        ODataDataSource,

        CsvDataSource,
        DBaseDataSource,
        DynamicsNavDataSource,
        ExcelDataSource,
        JsonDataSource,
        XmlDataSource,

        MongoDbDataSource,

        DropboxCloudStorage,
        GoogleDriveCloudStorage,
        OneDriveCloudStorage,
        SharePointCloudStorage,

        Unspecified
    }
    #endregion

    #region StiConnectionOrder
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StiConnectionOrder
    {
        MsSqlDataSource = 10,
        MySqlDataSource = 20,
        OdbcDataSource = 30,
        OleDbDataSource = 40,
        OracleDataSource = 50,

        MsAccessDataSource = 60,
        PostgreSqlDataSource = 70,
        FirebirdDataSource = 80,
        SqlCeDataSource = 90,
        SqLiteDataSource = 100,

        Db2DataSource = 110,
        InformixDataSource = 120,
        SybaseDataSource = 130,
        TeradataDataSource = 140,
        VistaDbDataSource = 150,
        UniversalDevartDataSource = 160,

        ODataDataSource = 170,

        ExcelDataSource = 180,
        JsonDataSource = 190,
        XmlDataSource = 200,
        CsvDataSource = 210,
        DBaseDataSource = 220,

        MongoDbDataSource = 400,
        
        DynamicsNavDataSource = 230,
        

        DropboxCloudStorage = 240,
        GoogleDriveCloudStorage = 250,
        OneDriveCloudStorage = 260,
        SharePointCloudStorage = 270,

        Unspecified = 0
    }
    #endregion

    #region StiFileType
    /// <summary>
    /// Enum contains list of file types which can be present in FileImem.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StiFileType
    {
        Unknown = 1,
        ReportSnapshot,
        Pdf,
        Xps,
        PowerPoint,
        Html,
        Text,
        RichText,
        Word,
        OpenDocumentWriter,
        Excel,
        OpenDocumentCalc,
        Data,
        Image,
        Xml,
        Xsd,
        Csv,
        Dbf,
        Sylk,
        Dif,
        Json
    }
    #endregion
}
