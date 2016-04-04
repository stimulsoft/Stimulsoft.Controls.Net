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
{	TRADE SECRETS OF Stimulsoft										}
{																	}
{	CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON		}
{	ADDITIONAL RESTRICTIONS.										}
{																	}
{*******************************************************************}
*/
#endregion Copyright (C) 2003-2016 Stimulsoft

using System;
using System.Collections.Generic;

namespace Stimulsoft.Base
{
    public sealed partial class StiDbType
    {
        #region Db2
        public enum Db2
        {
            Invalid = 0,
            SmallInt = 1,
            Integer = 2,
            BigInt = 3,
            Real = 4,
            Double = 5,
            Float = 6,
            Decimal = 7,
            Numeric = 8,
            Date = 9,
            Time = 10,
            Timestamp = 11,
            Char = 12,
            VarChar = 13,
            LongVarChar = 14,
            Binary = 15,
            VarBinary = 16,
            LongVarBinary = 17,
            Graphic = 18,
            VarGraphic = 19,
            LongVarGraphic = 20,
            Clob = 21,
            Blob = 22,
            DbClob = 23,
            Datalink = 24,
            RowId = 25,
            Xml = 26,
            Real370 = 27,
            DecimalFloat = 28,
            DynArray = 29,
            BigSerial = 30,
            BinaryXml = 31,
            TimeStampWithTimeZone = 32,
            Cursor = 33,
            Serial = 34,
            Int8 = 35,
            Serial8 = 36,
            Money = 37,
            DateTime = 38,
            Text = 39,
            Byte = 40,
            SmallFloat = 1002,
            Null = 1003,
            NChar = 1006,
            NVarChar = 1007,
            Boolean = 1015,
            Other = 1016,
        }
        #endregion

        #region Devart
        public class Devart
        {
            #region MySql
            public enum MySql
            {
                BigInt = 1,
                Binary = 2,
                Bit = 3,
                Blob = 4,
                Char = 5,
                Date = 6,
                DateTime = 7,
                Decimal = 8,
                Double = 9,
                Float = 10,
                Int = 11,
                SmallInt = 12,
                Text = 13,
                Time = 14,
                TimeStamp = 15,
                TinyInt = 16,
                VarBinary = 17,
                VarChar = 18,
                Year = 19,
                Guid = 20,
                Geometry = 21,
            }
            #endregion

            #region Oracle
            public enum Oracle
            {
                Array = 1,
                BFile = 2,
                Blob = 3,
                Boolean = 4,
                Char = 5,
                Clob = 6,
                Cursor = 7,
                Date = 8,
                Double = 9,
                Float = 10,
                Integer = 11,
                IntervalDS = 12,
                IntervalYM = 13,
                Long = 14,
                LongRaw = 15,
                NChar = 16,
                NClob = 17,
                NVarChar = 18,
                Number = 19,
                Object = 20,
                Ref = 21,
                Raw = 22,
                RowId = 23,
                Table = 24,
                TimeStamp = 25,
                TimeStampLTZ = 26,
                TimeStampTZ = 27,
                VarChar = 28,
                Xml = 29,
                AnyData = 30,
                Byte = 31,
                Int16 = 32,
                Int64 = 33,
            }
            #endregion

            #region PostgreSql
            public enum PostgreSql
            {
                Row = 1,
                Array = 2,
                LargeObject = 3,
                Boolean = 16,
                ByteA = 17,
                BigInt = 20,
                SmallInt = 21,
                Int = 23,
                Text = 25,
                Json = 114,
                Xml = 142,
                Point = 600,
                LSeg = 601,
                Path = 602,
                Box = 603,
                Polygon = 604,
                Line = 628,
                CIdr = 650,
                Real = 700,
                Double = 701,
                Circle = 718,
                Money = 790,
                MacAddr = 829,
                Inet = 869,
                Char = 1042,
                VarChar = 1043,
                Date = 1082,
                Time = 1083,
                TimeStamp = 1114,
                TimeStampTZ = 1184,
                Interval = 1186,
                TimeTZ = 1266,
                Bit = 1560,
                VarBit = 1562,
                Numeric = 1700,
                Uuid = 2950,
                IntRange = 3904,
                NumericRange = 3906,
                TimeStampRange = 3908,
                TimeStampTZRange = 3910,
                DateRange = 3912,
                BigIntRange = 3926,
            }
            #endregion
        }
        #endregion

        #region Firebird
        public enum Firebird
        {
            Array = 0,
            BigInt = 1,
            Binary = 2,
            Boolean = 3,
            Char = 4,
            Date = 5,
            Decimal = 6,
            Double = 7,
            Float = 8,
            Guid = 9,
            Integer = 10,
            Numeric = 11,
            SmallInt = 12,
            Text = 13,
            Time = 14,
            TimeStamp = 15,
            VarChar = 16,
        }
        #endregion

        #region Informix
        public enum Informix
        {
            Char = 0,
            SmallInt = 1,
            Integer = 2,
            Float = 3,
            SmallFloat = 4,
            Real = 4,
            Decimal = 5,
            Serial = 6,
            Date = 7,
            Money = 8,
            Null = 9,
            DateTime = 10,
            Byte = 11,
            Text = 12,
            VarChar = 13,
            NChar = 15,
            NVarChar = 16,
            Int8 = 17,
            Serial8 = 18,
            Other = 99,
            LVarChar = 101,
            LongVarChar = 101,
            Blob = 110,
            Clob = 111,
            Boolean = 126,
            Invalid = 200,
            BigInt = 203,
            Double = 205,
            Numeric = 208,
            Time = 210,
            Timestamp = 211,
            Binary = 215,
            VarBinary = 216,
            LongVarBinary = 217,
            BigSerial = 230,
        }
        #endregion

        #region MySql
        public enum MySql
        {
            Decimal = 0,
            Byte = 1,
            Int16 = 2,
            Int32 = 3,
            Float = 4,
            Double = 5,
            Timestamp = 7,
            Int64 = 8,
            Int24 = 9,
            Date = 10,
            Time = 11,
            DateTime = 12,
            Year = 13,
            Newdate = 14,
            VarString = 15,
            Bit = 16,
            NewDecimal = 246,
            Enum = 247,
            Set = 248,
            TinyBlob = 249,
            MediumBlob = 250,
            LongBlob = 251,
            Blob = 252,
            VarChar = 253,
            String = 254,
            Geometry = 255,
            UByte = 501,
            UInt16 = 502,
            UInt32 = 503,
            UInt64 = 508,
            UInt24 = 509,
            Binary = 600,
            VarBinary = 601,
            TinyText = 749,
            MediumText = 750,
            LongText = 751,
            Text = 752,
            Guid = 800,
        }
        #endregion

        #region MsSql
        public enum MsSql
        {
            // Summary:
            //     System.Int64. A 64-bit signed integer.
            BigInt = 0,
            //
            // Summary:
            //     System.Array of type System.Byte. A fixed-length stream of binary data ranging
            //     between 1 and 8,000 bytes.
            Binary = 1,
            //
            // Summary:
            //     System.Boolean. An unsigned numeric value that can be 0, 1, or null.
            Bit = 2,
            //
            // Summary:
            //     System.String. A fixed-length stream of non-Unicode characters ranging between
            //     1 and 8,000 characters.
            Char = 3,
            //
            // Summary:
            //     System.DateTime. Date and time data ranging in value from January 1, 1753
            //     to December 31, 9999 to an accuracy of 3.33 milliseconds.
            DateTime = 4,
            //
            // Summary:
            //     System.Decimal. A fixed precision and scale numeric value between -10 38
            //     -1 and 10 38 -1.
            Decimal = 5,
            //
            // Summary:
            //     System.Double. A floating point number within the range of -1.79E +308 through
            //     1.79E +308.
            Float = 6,
            //
            // Summary:
            //     System.Array of type System.Byte. A variable-length stream of binary data
            //     ranging from 0 to 2 31 -1 (or 2,147,483,647) bytes.
            Image = 7,
            //
            // Summary:
            //     System.Int32. A 32-bit signed integer.
            Int = 8,
            //
            // Summary:
            //     System.Decimal. A currency value ranging from -2 63 (or -9,223,372,036,854,775,808)
            //     to 2 63 -1 (or +9,223,372,036,854,775,807) with an accuracy to a ten-thousandth
            //     of a currency unit.
            Money = 9,
            //
            // Summary:
            //     System.String. A fixed-length stream of Unicode characters ranging between
            //     1 and 4,000 characters.
            NChar = 10,
            //
            // Summary:
            //     System.String. A variable-length stream of Unicode data with a maximum length
            //     of 2 30 - 1 (or 1,073,741,823) characters.
            NText = 11,
            //
            // Summary:
            //     System.String. A variable-length stream of Unicode characters ranging between
            //     1 and 4,000 characters. Implicit conversion fails if the string is greater
            //     than 4,000 characters. Explicitly set the object when working with strings
            //     longer than 4,000 characters. Use System.Data.MsSql.NVarChar when the
            //     database column is nvarchar(max).
            NVarChar = 12,
            //
            // Summary:
            //     System.Single. A floating point number within the range of -3.40E +38 through
            //     3.40E +38.
            Real = 13,
            //
            // Summary:
            //     System.Guid. A globally unique identifier (or GUID).
            UniqueIdentifier = 14,
            //
            // Summary:
            //     System.DateTime. Date and time data ranging in value from January 1, 1900
            //     to June 6, 2079 to an accuracy of one minute.
            SmallDateTime = 15,
            //
            // Summary:
            //     System.Int16. A 16-bit signed integer.
            SmallInt = 16,
            //
            // Summary:
            //     System.Decimal. A currency value ranging from -214,748.3648 to +214,748.3647
            //     with an accuracy to a ten-thousandth of a currency unit.
            SmallMoney = 17,
            //
            // Summary:
            //     System.String. A variable-length stream of non-Unicode data with a maximum
            //     length of 2 31 -1 (or 2,147,483,647) characters.
            Text = 18,
            //
            // Summary:
            //     System.Array of type System.Byte. Automatically generated binary numbers,
            //     which are guaranteed to be unique within a database. timestamp is used typically
            //     as a mechanism for version-stamping table rows. The storage size is 8 bytes.
            Timestamp = 19,
            //
            // Summary:
            //     System.Byte. An 8-bit unsigned integer.
            TinyInt = 20,
            //
            // Summary:
            //     System.Array of type System.Byte. A variable-length stream of binary data
            //     ranging between 1 and 8,000 bytes. Implicit conversion fails if the byte
            //     array is greater than 8,000 bytes. Explicitly set the object when working
            //     with byte arrays larger than 8,000 bytes.
            VarBinary = 21,
            //
            // Summary:
            //     System.String. A variable-length stream of non-Unicode characters ranging
            //     between 1 and 8,000 characters. Use System.Data.MsSql.VarChar when the
            //     database column is varchar(max).
            VarChar = 22,
            //
            // Summary:
            //     System.Object. A special data type that can contain numeric, string, binary,
            //     or date data as well as the SQL Server values Empty and Null, which is assumed
            //     if no other type is declared.
            Variant = 23,
            //
            // Summary:
            //     An XML value. Obtain the XML as a string using the System.Data.SqlClient.SqlDataReader.GetValue(System.Int32)
            //     method or System.Data.SqlTypes.SqlXml.Value property, or as an System.Xml.XmlReader
            //     by calling the System.Data.SqlTypes.SqlXml.CreateReader() method.
            Xml = 25,
            //
            // Summary:
            //     A SQL Server 2005 user-defined type (UDT).
            Udt = 29,
            //
            // Summary:
            //     A special data type for specifying structured data contained in table-valued
            //     parameters.
            Structured = 30,
            //
            // Summary:
            //     Date data ranging in value from January 1,1 AD through December 31, 9999
            //     AD.
            Date = 31,
            //
            // Summary:
            //     Time data based on a 24-hour clock. Time value range is 00:00:00 through
            //     23:59:59.9999999 with an accuracy of 100 nanoseconds. Corresponds to a SQL
            //     Server time value.
            Time = 32,
            //
            // Summary:
            //     Date and time data. Date value range is from January 1,1 AD through December
            //     31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999 with an
            //     accuracy of 100 nanoseconds.
            DateTime2 = 33,
            //
            // Summary:
            //     Date and time data with time zone awareness. Date value range is from January
            //     1,1 AD through December 31, 9999 AD. Time value range is 00:00:00 through
            //     23:59:59.9999999 with an accuracy of 100 nanoseconds. Time zone value range
            //     is -14:00 through +14:00.
            DateTimeOffset = 34,
        }
        #endregion

        #region Odbc
        public enum Odbc
        {
            // Summary:
            //     Exact numeric value with precision 19 (if signed) or 20 (if unsigned) and
            //     scale 0 (signed: –2[63] <= n <= 2[63] – 1, unsigned:0 <= n <= 2[64] – 1)
            //     (SQL_BIGINT). This maps to System.Int64.
            BigInt = 1,
            //
            // Summary:
            //     A stream of binary data (SQL_BINARY). This maps to an System.Array of type
            //     System.Byte.
            Binary = 2,
            //
            // Summary:
            //     Single bit binary data (SQL_BIT). This maps to System.Boolean.
            Bit = 3,
            //
            // Summary:
            //     A fixed-length character string (SQL_CHAR). This maps to System.String.
            Char = 4,
            //
            // Summary:
            //     Date data in the format yyyymmddhhmmss (SQL_TYPE_TIMESTAMP). This maps to
            //     System.DateTime.
            DateTime = 5,
            //
            // Summary:
            //     Signed, exact, numeric value with a precision of at least p and scale s,
            //     where 1 <= p <= 15 and s <= p. The maximum precision is driver-specific (SQL_DECIMAL).
            //     This maps to System.Decimal.
            Decimal = 6,
            //
            // Summary:
            //     Signed, exact, numeric value with a precision p and scale s, where 1 <= p
            //     <= 15, and s <= p (SQL_NUMERIC). This maps to System.Decimal.
            Numeric = 7,
            //
            // Summary:
            //     Signed, approximate, numeric value with a binary precision 53 (zero or absolute
            //     value 10[–308] to 10[308]) (SQL_DOUBLE). This maps to System.Double.
            Double = 8,
            //
            // Summary:
            //     Variable length binary data. Maximum length is data source–dependent (SQL_LONGVARBINARY).
            //     This maps to an System.Array of type System.Byte.
            Image = 9,
            //
            // Summary:
            //     Exact numeric value with precision 10 and scale 0 (signed: –2[31] <= n <=
            //     2[31] – 1, unsigned:0 <= n <= 2[32] – 1) (SQL_INTEGER). This maps to System.Int32.
            Int = 10,
            //
            // Summary:
            //     Unicode character string of fixed string length (SQL_WCHAR). This maps to
            //     System.String.
            NChar = 11,
            //
            // Summary:
            //     Unicode variable-length character data. Maximum length is data source–dependent.
            //     (SQL_WLONGVARCHAR). This maps to System.String.
            NText = 12,
            //
            // Summary:
            //     A variable-length stream of Unicode characters (SQL_WVARCHAR). This maps
            //     to System.String.
            NVarChar = 13,
            //
            // Summary:
            //     Signed, approximate, numeric value with a binary precision 24 (zero or absolute
            //     value 10[–38] to 10[38]).(SQL_REAL). This maps to System.Single.
            Real = 14,
            //
            // Summary:
            //     A fixed-length GUID (SQL_GUID). This maps to System.Guid.
            UniqueIdentifier = 15,
            //
            // Summary:
            //     Data and time data in the format yyyymmddhhmmss (SQL_TYPE_TIMESTAMP). This
            //     maps to System.DateTime.
            SmallDateTime = 16,
            //
            // Summary:
            //     Exact numeric value with precision 5 and scale 0 (signed: –32,768 <= n <=
            //     32,767, unsigned: 0 <= n <= 65,535) (SQL_SMALLINT). This maps to System.Int16.
            SmallInt = 17,
            //
            // Summary:
            //     Variable length character data. Maximum length is data source–dependent (SQL_LONGVARCHAR).
            //     This maps to System.String.
            Text = 18,
            //
            // Summary:
            //     A stream of binary data (SQL_BINARY). This maps to an System.Array of type
            //     System.Byte.
            Timestamp = 19,
            //
            // Summary:
            //     Exact numeric value with precision 3 and scale 0 (signed: –128 <= n <= 127,
            //     unsigned:0 <= n <= 255)(SQL_TINYINT). This maps to System.Byte.
            TinyInt = 20,
            //
            // Summary:
            //     Variable length binary. The maximum is set by the user (SQL_VARBINARY). This
            //     maps to an System.Array of type System.Byte.
            VarBinary = 21,
            //
            // Summary:
            //     A variable-length stream character string (SQL_CHAR). This maps to System.String.
            VarChar = 22,
            //
            // Summary:
            //     Date data in the format yyyymmdd (SQL_TYPE_DATE). This maps to System.DateTime.
            Date = 23,
            //
            // Summary:
            //     Date data in the format hhmmss (SQL_TYPE_TIMES). This maps to System.DateTime.
            Time = 24,
        }
        #endregion
        
        #region OleDb
        // Summary:
        //     Specifies the data type of a field, a property, for use in an System.Data.OleDb.OleDbParameter.
        public enum OleDb
        {
            // Summary:
            //     No value (DBTYPE_EMPTY).
            Empty = 0,
            //
            // Summary:
            //     A 16-bit signed integer (DBTYPE_I2). This maps to System.Int16.
            SmallInt = 2,
            //
            // Summary:
            //     A 32-bit signed integer (DBTYPE_I4). This maps to System.Int32.
            Integer = 3,
            //
            // Summary:
            //     A floating-point number within the range of -3.40E +38 through 3.40E +38
            //     (DBTYPE_R4). This maps to System.Single.
            Single = 4,
            //
            // Summary:
            //     A floating-point number within the range of -1.79E +308 through 1.79E +308
            //     (DBTYPE_R8). This maps to System.Double.
            Double = 5,
            //
            // Summary:
            //     A currency value ranging from -2 63 (or -922,337,203,685,477.5808) to 2 63
            //     -1 (or +922,337,203,685,477.5807) with an accuracy to a ten-thousandth of
            //     a currency unit (DBTYPE_CY). This maps to System.Decimal.
            Currency = 6,
            //
            // Summary:
            //     Date data, stored as a double (DBTYPE_DATE). The whole portion is the number
            //     of days since December 30, 1899, and the fractional portion is a fraction
            //     of a day. This maps to System.DateTime.
            Date = 7,
            //
            // Summary:
            //     A null-terminated character string of Unicode characters (DBTYPE_BSTR). This
            //     maps to System.String.
            BSTR = 8,
            //
            // Summary:
            //     A pointer to an IDispatch interface (DBTYPE_IDISPATCH). This maps to System.Object.
            IDispatch = 9,
            //
            // Summary:
            //     A 32-bit error code (DBTYPE_ERROR). This maps to System.Exception.
            Error = 10,
            //
            // Summary:
            //     A Boolean value (DBTYPE_BOOL). This maps to System.Boolean.
            Boolean = 11,
            //
            // Summary:
            //     A special data type that can contain numeric, string, binary, or date data,
            //     and also the special values Empty and Null (DBTYPE_VARIANT). This type is
            //     assumed if no other is specified. This maps to System.Object.
            Variant = 12,
            //
            // Summary:
            //     A pointer to an IUnknown interface (DBTYPE_UNKNOWN). This maps to System.Object.
            IUnknown = 13,
            //
            // Summary:
            //     A fixed precision and scale numeric value between -10 38 -1 and 10 38 -1
            //     (DBTYPE_DECIMAL). This maps to System.Decimal.
            Decimal = 14,
            //
            // Summary:
            //     A 8-bit signed integer (DBTYPE_I1). This maps to System.SByte.
            TinyInt = 16,
            //
            // Summary:
            //     A 8-bit unsigned integer (DBTYPE_UI1). This maps to System.Byte.
            UnsignedTinyInt = 17,
            //
            // Summary:
            //     A 16-bit unsigned integer (DBTYPE_UI2). This maps to System.UInt16.
            UnsignedSmallInt = 18,
            //
            // Summary:
            //     A 32-bit unsigned integer (DBTYPE_UI4). This maps to System.UInt32.
            UnsignedInt = 19,
            //
            // Summary:
            //     A 64-bit signed integer (DBTYPE_I8). This maps to System.Int64.
            BigInt = 20,
            //
            // Summary:
            //     A 64-bit unsigned integer (DBTYPE_UI8). This maps to System.UInt64.
            UnsignedBigInt = 21,
            //
            // Summary:
            //     A 64-bit unsigned integer representing the number of 100-nanosecond intervals
            //     since January 1, 1601 (DBTYPE_FILETIME). This maps to System.DateTime.
            Filetime = 64,
            //
            // Summary:
            //     A globally unique identifier (or GUID) (DBTYPE_GUID). This maps to System.Guid.
            Guid = 72,
            //
            // Summary:
            //     A stream of binary data (DBTYPE_BYTES). This maps to an System.Array of type
            //     System.Byte.
            Binary = 128,
            //
            // Summary:
            //     A character string (DBTYPE_STR). This maps to System.String.
            Char = 129,
            //
            // Summary:
            //     A null-terminated stream of Unicode characters (DBTYPE_WSTR). This maps to
            //     System.String.
            WChar = 130,
            //
            // Summary:
            //     An exact numeric value with a fixed precision and scale (DBTYPE_NUMERIC).
            //     This maps to System.Decimal.
            Numeric = 131,
            //
            // Summary:
            //     Date data in the format yyyymmdd (DBTYPE_DBDATE). This maps to System.DateTime.
            DBDate = 133,
            //
            // Summary:
            //     Time data in the format hhmmss (DBTYPE_DBTIME). This maps to System.TimeSpan.
            DBTime = 134,
            //
            // Summary:
            //     Data and time data in the format yyyymmddhhmmss (DBTYPE_DBTIMESTAMP). This
            //     maps to System.DateTime.
            DBTimeStamp = 135,
            //
            // Summary:
            //     An automation PROPVARIANT (DBTYPE_PROP_VARIANT). This maps to System.Object.
            PropVariant = 138,
            //
            // Summary:
            //     A variable-length numeric value (System.Data.OleDb.OleDbParameter only).
            //     This maps to System.Decimal.
            VarNumeric = 139,
            //
            // Summary:
            //     A variable-length stream of non-Unicode characters (System.Data.OleDb.OleDbParameter
            //     only). This maps to System.String.
            VarChar = 200,
            //
            // Summary:
            //     A long string value (System.Data.OleDb.OleDbParameter only). This maps to
            //     System.String.
            LongVarChar = 201,
            //
            // Summary:
            //     A variable-length, null-terminated stream of Unicode characters (System.Data.OleDb.OleDbParameter
            //     only). This maps to System.String.
            VarWChar = 202,
            //
            // Summary:
            //     A long null-terminated Unicode string value (System.Data.OleDb.OleDbParameter
            //     only). This maps to System.String.
            LongVarWChar = 203,
            //
            // Summary:
            //     A variable-length stream of binary data (System.Data.OleDb.OleDbParameter
            //     only). This maps to an System.Array of type System.Byte.
            VarBinary = 204,
            //
            // Summary:
            //     A long binary value (System.Data.OleDb.OleDbParameter only). This maps to
            //     an System.Array of type System.Byte.
            LongVarBinary = 205,
        }
        #endregion

        #region Oracle
        public enum Oracle
        {
            BFile = 101,
            Blob = 102,
            Byte = 103,
            Char = 104,
            Clob = 105,
            Date = 106,
            Decimal = 107,
            Double = 108,
            Long = 109,
            LongRaw = 110,
            Int16 = 111,
            Int32 = 112,
            Int64 = 113,
            IntervalDS = 114,
            IntervalYM = 115,
            NClob = 116,
            NChar = 117,
            NVarchar2 = 119,
            Raw = 120,
            RefCursor = 121,
            Single = 122,
            TimeStamp = 123,
            TimeStampLTZ = 124,
            TimeStampTZ = 125,
            Varchar2 = 126,
            XmlType = 127,
            BinaryDouble = 132,
            BinaryFloat = 133,
        }
        #endregion

        #region OracleClient
        public enum OracleClient
        {
            // Summary:
            //     An Oracle BFILE data type that contains a reference to binary data with a
            //     maximum size of 4 gigabytes that is stored in an external file. Use the OracleClient
            //     System.Data.OracleClient.OracleBFile data type with the System.Data.OracleClient.OracleParameter.Value
            //     property.
            BFile = 1,
            //
            // Summary:
            //     An Oracle BLOB data type that contains binary data with a maximum size of
            //     4 gigabytes. Use the OracleClient System.Data.OracleClient.OracleLob data
            //     type in System.Data.OracleClient.OracleParameter.Value.
            Blob = 2,
            //
            // Summary:
            //     An Oracle CHAR data type that contains a fixed-length character string with
            //     a maximum size of 2,000 bytes. Use the .NET Framework System.String or OracleClient
            //     System.Data.OracleClient.OracleString data type in System.Data.OracleClient.OracleParameter.Value.
            Char = 3,
            //
            // Summary:
            //     An Oracle CLOB data type that contains character data, based on the default
            //     character set on the server, with a maximum size of 4 gigabytes. Use the
            //     OracleClient System.Data.OracleClient.OracleLob data type in System.Data.OracleClient.OracleParameter.Value.
            Clob = 4,
            //
            // Summary:
            //     An Oracle REF CURSOR. The System.Data.OracleClient.OracleDataReader object
            //     is not available.
            Cursor = 5,
            //
            // Summary:
            //     An Oracle DATE data type that contains a fixed-length representation of a
            //     date and time, ranging from January 1, 4712 B.C. to December 31, A.D. 4712,
            //     with the default format dd-mmm-yy. For A.D. dates, DATE maps to System.DateTime.
            //     To bind B.C. dates, use a String parameter and the Oracle TO_DATE or TO_CHAR
            //     conversion functions for input and output parameters respectively. Use the
            //     .NET Framework System.DateTime or OracleClient System.Data.OracleClient.OracleDateTime
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            DateTime = 6,
            //
            // Summary:
            //     An Oracle INTERVAL DAY TO SECOND data type (Oracle 9i or later) that contains
            //     an interval of time in days, hours, minutes, and seconds, and has a fixed
            //     size of 11 bytes. Use the .NET Framework System.TimeSpan or OracleClient
            //     System.Data.OracleClient.OracleTimeSpan data type in System.Data.OracleClient.OracleParameter.Value.
            IntervalDayToSecond = 7,
            //
            // Summary:
            //     An Oracle INTERVAL YEAR TO MONTH data type (Oracle 9i or later) that contains
            //     an interval of time in years and months, and has a fixed size of 5 bytes.
            //     Use the .NET Framework System.Int32 or OracleClient System.Data.OracleClient.OracleMonthSpan
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            IntervalYearToMonth = 8,
            //
            // Summary:
            //     An Oracle LONGRAW data type that contains variable-length binary data with
            //     a maximum size of 2 gigabytes. Use the .NET Framework Byte[] or OracleClient
            //     System.Data.OracleClient.OracleBinary data type in System.Data.OracleClient.OracleParameter.Value.
            LongRaw = 9,
            //
            // Summary:
            //     An Oracle LONG data type that contains a variable-length character string
            //     with a maximum size of 2 gigabytes. Use the .NET Framework System.String
            //     or OracleClient System.Data.OracleClient.OracleString data type in System.Data.OracleClient.OracleParameter.Value.
            LongVarChar = 10,
            //
            // Summary:
            //     An Oracle NCHAR data type that contains fixed-length character string to
            //     be stored in the national character set of the database, with a maximum size
            //     of 2,000 bytes (not characters) when stored in the database. The size of
            //     the value depends on the national character set of the database. See your
            //     Oracle documentation for more information. Use the .NET Framework System.String
            //     or OracleClient System.Data.OracleClient.OracleString data type in System.Data.OracleClient.OracleParameter.Value.
            NChar = 11,
            //
            // Summary:
            //     An Oracle NCLOB data type that contains character data to be stored in the
            //     national character set of the database, with a maximum size of 4 gigabytes
            //     (not characters) when stored in the database. The size of the value depends
            //     on the national character set of the database. See your Oracle documentation
            //     for more information. Use the .NET Framework System.String or OracleClient
            //     System.Data.OracleClient.OracleString data type in System.Data.OracleClient.OracleParameter.Value.
            NClob = 12,
            //
            // Summary:
            //     An Oracle NUMBER data type that contains variable-length numeric data with
            //     a maximum precision and scale of 38. This maps to System.Decimal. To bind
            //     an Oracle NUMBER that exceeds what System.Decimal.MaxValue can contain, either
            //     use an System.Data.OracleClient.OracleNumber data type, or use a String parameter
            //     and the Oracle TO_NUMBER or TO_CHAR conversion functions for input and output
            //     parameters respectively. Use the .NET Framework System.Decimal or OracleClient
            //     System.Data.OracleClient.OracleNumber data type in System.Data.OracleClient.OracleParameter.Value.
            Number = 13,
            //
            // Summary:
            //     An Oracle NVARCHAR2 data type that contains a variable-length character string
            //     stored in the national character set of the database, with a maximum size
            //     of 4,000 bytes (not characters) when stored in the database. The size of
            //     the value depends on the national character set of the database. See your
            //     Oracle documentation for more information. Use the .NET Framework System.String
            //     or OracleClient System.Data.OracleClient.OracleString data type in System.Data.OracleClient.OracleParameter.Value.
            NVarChar = 14,
            //
            // Summary:
            //     An Oracle RAW data type that contains variable-length binary data with a
            //     maximum size of 2,000 bytes. Use the .NET Framework Byte[] or OracleClient
            //     System.Data.OracleClient.OracleBinary data type in System.Data.OracleClient.OracleParameter.Value.
            Raw = 15,
            //
            // Summary:
            //     The base64 string representation of an Oracle ROWID data type. Use the .NET
            //     Framework System.String or OracleClient System.Data.OracleClient.OracleString
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            RowId = 16,
            //
            // Summary:
            //     An Oracle TIMESTAMP (Oracle 9i or later) that contains date and time (including
            //     seconds), and ranges in size from 7 to 11 bytes. Use the .NET Framework System.DateTime
            //     or OracleClient System.Data.OracleClient.OracleDateTime data type in System.Data.OracleClient.OracleParameter.Value.
            Timestamp = 18,
            //
            // Summary:
            //     An Oracle TIMESTAMP WITH LOCAL TIMEZONE (Oracle 9i or later) that contains
            //     date, time, and a reference to the original time zone, and ranges in size
            //     from 7 to 11 bytes. Use the .NET Framework System.DateTime or OracleClient
            //     System.Data.OracleClient.OracleDateTime data type in System.Data.OracleClient.OracleParameter.Value.
            TimestampLocal = 19,
            //
            // Summary:
            //     An Oracle TIMESTAMP WITH TIMEZONE (Oracle 9i or later) that contains date,
            //     time, and a specified time zone, and has a fixed size of 13 bytes. Use the
            //     .NET Framework System.DateTime or OracleClient System.Data.OracleClient.OracleDateTime
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            TimestampWithTZ = 20,
            //
            // Summary:
            //     An Oracle VARCHAR2 data type that contains a variable-length character string
            //     with a maximum size of 4,000 bytes. Use the .NET Framework System.String
            //     or OracleClient System.Data.OracleClient.OracleString data type in System.Data.OracleClient.OracleParameter.Value.
            VarChar = 22,
            //
            // Summary:
            //     An integral type representing unsigned 8-bit integers with values between
            //     0 and 255. This is not a native Oracle data type, but is provided to improve
            //     performance when binding input parameters. Use the .NET Framework System.Byte
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            Byte = 23,
            //
            // Summary:
            //     An integral type representing unsigned 16-bit integers with values between
            //     0 and 65535. This is not a native Oracle data type, but is provided to improve
            //     performance when binding input parameters. For information about conversion
            //     of Oracle numeric values to common language runtime (CLR) data types, see
            //     System.Data.OracleClient.OracleNumber. Use the .NET Framework System.UInt16
            //     or OracleClient System.Data.OracleClient.OracleNumber data type in System.Data.OracleClient.OracleParameter.Value.
            UInt16 = 24,
            //
            // Summary:
            //     An integral type representing unsigned 32-bit integers with values between
            //     0 and 4294967295. This is not a native Oracle data type, but is provided
            //     to improve performance when binding input parameters. For information about
            //     conversion of Oracle numeric values to common language runtime (CLR) data
            //     types, see System.Data.OracleClient.OracleNumber. Use the .NET Framework
            //     System.UInt32 or OracleClient System.Data.OracleClient.OracleNumber data
            //     type in System.Data.OracleClient.OracleParameter.Value.
            UInt32 = 25,
            //
            // Summary:
            //     An integral type representing signed 8 bit integers with values between -128
            //     and 127. This is not a native Oracle data type, but is provided to improve
            //     performance when binding input parameters. Use the .NET Framework System.SByte
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            SByte = 26,
            //
            // Summary:
            //     An integral type representing signed 16-bit integers with values between
            //     -32768 and 32767. This is not a native Oracle data type, but is provided
            //     to improve performance when binding input parameters. For information about
            //     conversion of Oracle numeric values to common language runtime (CLR) data
            //     types, see System.Data.OracleClient.OracleNumber. Use the .NET Framework
            //     System.Int16 or OracleClient System.Data.OracleClient.OracleNumber data type
            //     in System.Data.OracleClient.OracleParameter.Value.
            Int16 = 27,
            //
            // Summary:
            //     An integral type representing signed 32-bit integers with values between
            //     -2147483648 and 2147483647. This is not a native Oracle data type, but is
            //     provided for performance when binding input parameters. For information about
            //     conversion of Oracle numeric values to common language runtime data types,
            //     see System.Data.OracleClient.OracleNumber. Use the .NET Framework System.Int32
            //     or OracleClient System.Data.OracleClient.OracleNumber data type in System.Data.OracleClient.OracleParameter.Value.
            Int32 = 28,
            //
            // Summary:
            //     A single-precision floating-point value. This is not a native Oracle data
            //     type, but is provided to improve performance when binding input parameters.
            //     For information about conversion of Oracle numeric values to common language
            //     runtime data types, see System.Data.OracleClient.OracleNumber. Use the .NET
            //     Framework System.Single or OracleClient System.Data.OracleClient.OracleNumber
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            Float = 29,
            //
            // Summary:
            //     A double-precision floating-point value. This is not a native Oracle data
            //     type, but is provided to improve performance when binding input parameters.
            //     For information about conversion of Oracle numeric values to common language
            //     runtime (CLR) data types, see System.Data.OracleClient.OracleNumber. Use
            //     the .NET Framework System.Double or OracleClient System.Data.OracleClient.OracleNumber
            //     data type in System.Data.OracleClient.OracleParameter.Value.
            Double = 30,
        }
        #endregion

        #region PostgreSql
        public enum PostgreSql
        {
            Array = -2147483648,
            Bigint = 1,
            Boolean = 2,
            Box = 3,
            Bytea = 4,
            Circle = 5,
            Char = 6,
            Date = 7,
            Double = 8,
            Integer = 9,
            Line = 10,
            LSeg = 11,
            Money = 12,
            Numeric = 13,
            Path = 14,
            Point = 15,
            Polygon = 16,
            Real = 17,
            Smallint = 18,
            Text = 19,
            Time = 20,
            Timestamp = 21,
            Varchar = 22,
            Refcursor = 23,
            Inet = 24,
            Bit = 25,
            TimestampTZ = 26,
            Uuid = 27,
            Xml = 28,
            Oidvector = 29,
            Interval = 30,
            TimeTZ = 31,
            Name = 32,
            Abstime = 33,
            MacAddr = 34,
            Json = 35,
            Jsonb = 36,
            Hstore = 37,
        }
        #endregion

        #region SqlCe
        public enum SqlCe
        {
            // Summary:
            //     System.Int64. A 64-bit signed integer.
            BigInt = 0,
            //
            // Summary:
            //     System.Array of type System.Byte. A fixed-length stream of binary data ranging
            //     between 1 and 8,000 bytes.
            Binary = 1,
            //
            // Summary:
            //     System.Boolean. An unsigned numeric value that can be 0, 1, or null.
            Bit = 2,
            //
            // Summary:
            //     System.String. A fixed-length stream of non-Unicode characters ranging between
            //     1 and 8,000 characters.
            Char = 3,
            //
            // Summary:
            //     System.DateTime. Date and time data ranging in value from January 1, 1753
            //     to December 31, 9999 to an accuracy of 3.33 milliseconds.
            DateTime = 4,
            //
            // Summary:
            //     System.Decimal. A fixed precision and scale numeric value between -10 38
            //     -1 and 10 38 -1.
            Decimal = 5,
            //
            // Summary:
            //     System.Double. A floating point number within the range of -1.79E +308 through
            //     1.79E +308.
            Float = 6,
            //
            // Summary:
            //     System.Array of type System.Byte. A variable-length stream of binary data
            //     ranging from 0 to 2 31 -1 (or 2,147,483,647) bytes.
            Image = 7,
            //
            // Summary:
            //     System.Int32. A 32-bit signed integer.
            Int = 8,
            //
            // Summary:
            //     System.Decimal. A currency value ranging from -2 63 (or -9,223,372,036,854,775,808)
            //     to 2 63 -1 (or +9,223,372,036,854,775,807) with an accuracy to a ten-thousandth
            //     of a currency unit.
            Money = 9,
            //
            // Summary:
            //     System.String. A fixed-length stream of Unicode characters ranging between
            //     1 and 4,000 characters.
            NChar = 10,
            //
            // Summary:
            //     System.String. A variable-length stream of Unicode data with a maximum length
            //     of 2 30 - 1 (or 1,073,741,823) characters.
            NText = 11,
            //
            // Summary:
            //     System.String. A variable-length stream of Unicode characters ranging between
            //     1 and 4,000 characters. Implicit conversion fails if the string is greater
            //     than 4,000 characters. Explicitly set the object when working with strings
            //     longer than 4,000 characters. Use System.Data.MsSql.NVarChar when the
            //     database column is nvarchar(max).
            NVarChar = 12,
            //
            // Summary:
            //     System.Single. A floating point number within the range of -3.40E +38 through
            //     3.40E +38.
            Real = 13,
            //
            // Summary:
            //     System.Guid. A globally unique identifier (or GUID).
            UniqueIdentifier = 14,
            //
            // Summary:
            //     System.DateTime. Date and time data ranging in value from January 1, 1900
            //     to June 6, 2079 to an accuracy of one minute.
            SmallDateTime = 15,
            //
            // Summary:
            //     System.Int16. A 16-bit signed integer.
            SmallInt = 16,
            //
            // Summary:
            //     System.Decimal. A currency value ranging from -214,748.3648 to +214,748.3647
            //     with an accuracy to a ten-thousandth of a currency unit.
            SmallMoney = 17,
            //
            // Summary:
            //     System.String. A variable-length stream of non-Unicode data with a maximum
            //     length of 2 31 -1 (or 2,147,483,647) characters.
            Text = 18,
            //
            // Summary:
            //     System.Array of type System.Byte. Automatically generated binary numbers,
            //     which are guaranteed to be unique within a database. timestamp is used typically
            //     as a mechanism for version-stamping table rows. The storage size is 8 bytes.
            Timestamp = 19,
            //
            // Summary:
            //     System.Byte. An 8-bit unsigned integer.
            TinyInt = 20,
            //
            // Summary:
            //     System.Array of type System.Byte. A variable-length stream of binary data
            //     ranging between 1 and 8,000 bytes. Implicit conversion fails if the byte
            //     array is greater than 8,000 bytes. Explicitly set the object when working
            //     with byte arrays larger than 8,000 bytes.
            VarBinary = 21,
            //
            // Summary:
            //     System.String. A variable-length stream of non-Unicode characters ranging
            //     between 1 and 8,000 characters. Use System.Data.MsSql.VarChar when the
            //     database column is varchar(max).
            VarChar = 22,
            //
            // Summary:
            //     System.Object. A special data type that can contain numeric, string, binary,
            //     or date data as well as the SQL Server values Empty and Null, which is assumed
            //     if no other type is declared.
            Variant = 23,
            //
            // Summary:
            //     An XML value. Obtain the XML as a string using the System.Data.SqlClient.SqlDataReader.GetValue(System.Int32)
            //     method or System.Data.SqlTypes.SqlXml.Value property, or as an System.Xml.XmlReader
            //     by calling the System.Data.SqlTypes.SqlXml.CreateReader() method.
            Xml = 25,
            //
            // Summary:
            //     A SQL Server 2005 user-defined type (UDT).
            Udt = 29,
            //
            // Summary:
            //     A special data type for specifying structured data contained in table-valued
            //     parameters.
            Structured = 30,
            //
            // Summary:
            //     Date data ranging in value from January 1,1 AD through December 31, 9999
            //     AD.
            Date = 31,
            //
            // Summary:
            //     Time data based on a 24-hour clock. Time value range is 00:00:00 through
            //     23:59:59.9999999 with an accuracy of 100 nanoseconds. Corresponds to a SQL
            //     Server time value.
            Time = 32,
            //
            // Summary:
            //     Date and time data. Date value range is from January 1,1 AD through December
            //     31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999 with an
            //     accuracy of 100 nanoseconds.
            DateTime2 = 33,
            //
            // Summary:
            //     Date and time data with time zone awareness. Date value range is from January
            //     1,1 AD through December 31, 9999 AD. Time value range is 00:00:00 through
            //     23:59:59.9999999 with an accuracy of 100 nanoseconds. Time zone value range
            //     is -14:00 through +14:00.
            DateTimeOffset = 34,
        }
        #endregion

        #region SqLite
        public enum SqLite
        {
            // Summary:
            //     Not used
            Uninitialized = 0,
            //
            // Summary:
            //     All integers in SQLite default to Int64
            Int64 = 1,
            //
            // Summary:
            //     All floating point numbers in SQLite default to double
            Double = 2,
            //
            // Summary:
            //     The default data type of SQLite is text
            Text = 3,
            //
            // Summary:
            //     Typically blob types are only seen when returned from a function
            Blob = 4,
            //
            // Summary:
            //     Null types can be returned from functions
            Null = 5,
            //
            // Summary:
            //     Used internally by this provider
            DateTime = 10,
            //
            // Summary:
            //     Used internally by this provider
            None = 11,
        }
        #endregion

        #region Sybase
        public enum Sybase
        {
            UnsignedBigInt = -208,
            UnsignedInt = -207,
            UnsignedSmallInt = -206,
            NVarChar = -205,
            NChar = -204,
            TimeStamp = -203,
            SmallDateTime = -202,
            SmallMoney = -201,
            Money = -200,
            Unitext = -10,
            UniVarChar = -9,
            UniChar = -8,
            Bit = -7,
            TinyInt = -6,
            BigInt = -5,
            Image = -4,
            VarBinary = -3,
            Binary = -2,
            Text = -1,
            Unsupported = 0,
            Char = 1,
            Numeric = 2,
            Decimal = 3,
            Integer = 4,
            SmallInt = 5,
            Real = 7,
            Double = 8,
            VarChar = 12,
            Date = 91,
            Time = 92,
            BigDateTime = 93,
            DateTime = 93,
        }
        #endregion

        #region Teradata
        public enum Teradata
        {
            BigInt = 90,
            Blob = 100,
            Byte = 110,
            ByteInt = 120,
            Char = 130,
            Clob = 140,
            Date = 150,
            Decimal = 160,
            Double = 170,
            Graphic = 180,
            Integer = 190,
            IntervalDay = 200,
            IntervalDayToHour = 210,
            IntervalDayToMinute = 220,
            IntervalDayToSecond = 230,
            IntervalHour = 240,
            IntervalHourToMinute = 250,
            IntervalHourToSecond = 260,
            IntervalMinute = 270,
            IntervalMinuteToSecond = 280,
            IntervalSecond = 290,
            IntervalYear = 300,
            IntervalYearToMonth = 310,
            IntervalMonth = 320,
            SmallInt = 330,
            Time = 340,
            TimeWithZone = 350,
            Timestamp = 360,
            TimestampWithZone = 370,
            VarByte = 380,
            VarChar = 390,
            VarGraphic = 400,
            PeriodDate = 410,
            PeriodTime = 420,
            PeriodTimeWithTimeZone = 430,
            PeriodTimestamp = 440,
            PeriodTimestampWithTimeZone = 450,
            Number = 460,
            Xml = 480,
            Json = 500,
            AnyType = 65535,
        }
        #endregion

        #region Universal
        public enum Universal
        {
            Array = 0,
            BigInt = 1,
            Binary = 2,
            Bit = 3,
            Blob = 4,
            Boolean = 5,
            Byte = 6,
            Char = 7,
            Clob = 8,
            Currency = 9,
            Cursor = 10,
            Date = 11,
            DateTime = 12,
            Decimal = 13,
            Double = 14,
            Guid = 15,
            Int = 16,
            IntervalDS = 17,
            IntervalYM = 18,
            NChar = 19,
            NClob = 20,
            NVarChar = 21,
            Object = 22,
            Single = 23,
            SmallInt = 24,
            TinyInt = 25,
            Time = 26,
            TimeStamp = 27,
            VarChar = 28,
            Xml = 29,
            TimeStampTZ = 30,
            DateTime2 = 31,
        }
        #endregion

        #region VistaDb
        public enum VistaDb
        {
            Uninitialized = -1,
            Char = 1,
            NChar = 2,
            VarChar = 3,
            NVarChar = 4,
            Text = 5,
            NText = 6,
            TinyInt = 8,
            SmallInt = 9,
            Int = 10,
            BigInt = 11,
            Real = 12,
            Float = 13,
            Decimal = 14,
            Money = 15,
            SmallMoney = 16,
            Bit = 17,
            DateTime = 19,
            Image = 20,
            UniqueIdentifier = 22,
            SmallDateTime = 23,
            Timestamp = 24,
            Binary = 25,
            VarBinary = 26,
            Time = 27,
            Date = 28,
            DateTime2 = 29,
            DateTimeOffset = 30,
            Unknown = 31,
        }
        #endregion
    }
}