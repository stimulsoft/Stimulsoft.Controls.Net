#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports 									            }
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
using System.Text;
using System.Data;
using System.IO;
using System.Collections.Generic;

namespace Stimulsoft.Base
{
    public class StiDBaseHelper
    {
        #region DBaseHeader
        private class DBaseHeader
        {
            public byte TableType;
            public int Year;
            public int Month;
            public int Day;
            public uint RecordsCount;
            public uint HeaderSize;
            public uint RecordSize;
            public byte CodePage;
        }
        #endregion

        #region DBaseFieldType
        private enum DBaseFieldType
        {
            Unknown = 0,
            Char,
            Date,
            Float,
            Numeric,
            Logical,
            Memo,
            Variable,
            Picture,
            Binary,
            General,
            ShortInt,
            LongInt,
            Double,
            Blob
        }
        #endregion

        #region DBaseFieldFlags
        [Flags]
        private enum DBaseFieldFlags
        {
            System = 0x01,
            MayBeNull = 0x02,
            Binary = 0x04,
            AutoIncrement = 0x08
        }
        #endregion

        #region DBaseFieldInfo
        private class DBaseFieldInfo
        {
            public string Name;
            public DBaseFieldType Type;
            public int Offset;
            public int Size;
            public int FractionSize;
            public DBaseFieldFlags Flags;
        }
        #endregion

        #region Constants CodePageCodes
        public static int[,] CodePageCodes = 
		{
			{0x00,  0},
			{0x01,  437},
			{0x69,  620}, 
			{0x6A,  737}, 
			{0x02,  850},  
			{0x64,  852},  
			{0x6B,  857},  
			{0x67,  861},  
			{0x66,  865},  
			{0x65,  866},  
			{0x7C,  874}, 
			{0x68,  895},  
			{0x7B,  932}, 
			{0x7A,  936}, 
			{0x79,  949}, 
			{0x78,  950}, 
			{0xC8,  1250}, 
			{0xC9,  1251}, 
			{0x03,  1252}, 
			{0xCB,  1253},
			{0xCA,  1254}, 
			{0x7D,  1255}, 
			{0x7E,  1256},
			{0x04,  10000},
			{0x98,  10006},
			{0x96,  10007},
			{0x97,  10029}
		};

        public static string[] CodePageNames =
		{
			"Default",
			"437  U.S. MS-DOS",
			"620  Mazovia (Polish) MS-DOS",
			"737  Greek MS-DOS (437G)",
			"850  International MS-DOS",
			"852  Eastern European MS-DOS",
			"857  Turkish MS-DOS",
			"861  Icelandic MS-DOS",
			"865  Nordic MS-DOS",
			"866  Russian MS-DOS",
			"874  Thai Windows",
			"895  Kamenicky (Czech) MS-DOS",
			"932  Japanese Windows",
			"936  Chinese Simplified (PRC, Singapore) Windows",
			"949  Korean Windows",
			"950  Traditional Chinese (Hong Kong SAR, Taiwan) Windows",
			"1250  EasternEuropean MS-DOS",
			"1251  Russian Windows",
			"1252  Windows ANSI",
			"1253  Greek Windows",
			"1254  Turkish Windows",
			"1255  Hebrew Windows",
			"1256  Arabic Windows",
			"10000  Standard Macintosh",
			"10006  Greek Macintosh",
			"10007  Russian Macintosh",
			"10029  Eastern European Macintosh"
		};
        #endregion

        #region Constants
        private const int DBaseHeaderLength = 32;
        private const int DBaseFieldInfoLength = 32;
        #endregion

        #region Methods
        /// <summary>
        /// Convert table from the DBase file.
		/// </summary>
        public static DataTable GetTable(string path)
		{
			return GetTable(path, 0);
		}


		/// <summary>
        /// Convert table from the DBase file.
		/// </summary>
        public static DataTable GetTable(string path, int codePage)
		{
		    return GetTable(path, codePage != 0, codePage);
		}


        /// <summary>
        /// Convert table from the DBase file.
        /// </summary>
        private static DataTable GetTable(string path, bool forceCodePage, int forcedCodePage)
        {
            DataTable dt = null;
            using (Stream data = new FileStream(path, FileMode.Open))
            {
                string memoPath = Path.ChangeExtension(path, "fpt");
                if (File.Exists(memoPath))
                {
                    using (Stream memo = new FileStream(memoPath, FileMode.Open))
                    {
                        dt = GetTable(data, memo, forceCodePage, forcedCodePage);
                    }
                }
                else
                {
                    dt = GetTable(data, null, forceCodePage, forcedCodePage);
                }
            }
            return dt;
        }

        /// <summary>
        /// Convert dataset from bytes stored in DBase format.
        /// </summary>
        public static DataSet GetDataSet(byte[] data, bool forceCodePage, int forcedCodePage)
        {
            DataTable dataTable = null;
            using (var sdata = new MemoryStream(data))
            {
                dataTable = GetTable(sdata, null, forceCodePage, forcedCodePage);
            }
            if (dataTable == null) return null;

            var dataSet = new DataSet { EnforceConstraints = false };
            dataSet.Tables.Add(dataTable);
            return dataSet;
        }

        /// <summary>
        /// Convert table from the bytes in which stored DBase.
        /// </summary>
        public static DataTable GetTable(byte[] data, int codePage)
        {
            using (var sdata = new MemoryStream(data))
            {
                return GetTable(sdata, null, codePage != 0, codePage);
            }
        }


        /// <summary>
        /// Convert table from the bytes in which stored DBase file.
        /// </summary>
        public static DataTable GetTable(Stream memoryStream, Stream memo, bool forceCodePage, int forcedCodePage)
        {
            memoryStream.Seek(0, SeekOrigin.Begin);

            //using (var memoryStream = new MemoryStream(data))
            {
                #region Read header
                var bufHeader = new byte[DBaseHeaderLength];
                memoryStream.Read(bufHeader, 0, bufHeader.Length);

                var dBaseHeader = new DBaseHeader
                {
                    TableType = bufHeader[0],
                    Year = 2000 + bufHeader[1],
                    Month = bufHeader[2],
                    Day = bufHeader[3],
                    RecordsCount = BitConverter.ToUInt32(bufHeader, 4),
                    HeaderSize = BitConverter.ToUInt16(bufHeader, 8),
                    RecordSize = BitConverter.ToUInt16(bufHeader, 10),
                    CodePage = bufHeader[29]
                };
                #endregion

                #region Prepare decoder
                var codePage = 0;
                for (var indexCP = 0; indexCP < CodePageCodes.Length; indexCP++)
                {
                    if (CodePageCodes[indexCP, 0] == dBaseHeader.CodePage)
                    {
                        codePage = CodePageCodes[indexCP, 1];
                        break;
                    }
                }
                if (forceCodePage) codePage = forcedCodePage;
                var decoder = codePage == 0 ? Encoding.Default.GetDecoder() : Encoding.GetEncoding(codePage).GetDecoder();
                #endregion

                uint fieldsNumber = (dBaseHeader.HeaderSize - DBaseHeaderLength - 1) / DBaseFieldInfoLength;

                #region Read fields info
                var bufFieldsInfo = new byte[dBaseHeader.HeaderSize - DBaseHeaderLength];
                memoryStream.Read(bufFieldsInfo, 0, bufFieldsInfo.Length);

                var fieldsInfo = new List<DBaseFieldInfo>();
                for (int fieldIndex = 0; fieldIndex < fieldsNumber; fieldIndex++)
                {
                    int fieldOffset = fieldIndex * DBaseFieldInfoLength;

                    if (bufFieldsInfo[fieldOffset] == 13)
                    {
                        fieldsNumber = (uint)fieldIndex;
                        break;
                    }

                    fieldsInfo.Add(new DBaseFieldInfo());

                    #region Get field name
                    var countChars = 0;
                    while (countChars < 10 && bufFieldsInfo[fieldOffset + countChars] != 0) countChars++;
                    var chars = new char[decoder.GetCharCount(bufFieldsInfo, fieldOffset, countChars)];
                    decoder.GetChars(bufFieldsInfo, fieldOffset, countChars, chars, 0);
                    fieldsInfo[fieldIndex].Name = new string(chars);
                    #endregion

                    #region Get field type
                    var fieldType = DBaseFieldType.Unknown;
                    switch ((char) bufFieldsInfo[fieldOffset + 11])
                    {
                        case 'C':
                            fieldType = DBaseFieldType.Char;
                            break;

                        case 'D':
                            fieldType = DBaseFieldType.Date;
                            break;

                        case 'F':
                            fieldType = DBaseFieldType.Float;
                            break;

                        case 'N':
                            fieldType = DBaseFieldType.Numeric;
                            break;

                        case 'L':
                            fieldType = DBaseFieldType.Logical;
                            break;

                        case 'M':
                            fieldType = DBaseFieldType.Memo;
                            break;

                        case 'V':
                            fieldType = DBaseFieldType.Variable;
                            break;

                        case 'P':
                            fieldType = DBaseFieldType.Picture;
                            break;

                        case 'B':
                            fieldType = DBaseFieldType.Binary;
                            break;

                        case 'W':
                            fieldType = DBaseFieldType.Blob;
                            break;

                        case 'G':
                            fieldType = DBaseFieldType.General;
                            break;

                        case '2':
                            fieldType = DBaseFieldType.ShortInt;
                            break;

                        case '4':
                        case 'I':
                            fieldType = DBaseFieldType.LongInt;
                            break;

                        case '8':
                            fieldType = DBaseFieldType.Double;
                            break;
                    }
                    fieldsInfo[fieldIndex].Type = fieldType;
                    #endregion

                    fieldsInfo[fieldIndex].Offset = BitConverter.ToInt32(bufFieldsInfo, fieldOffset + 12);
                    fieldsInfo[fieldIndex].Size = bufFieldsInfo[fieldOffset + 16];
                    fieldsInfo[fieldIndex].FractionSize = bufFieldsInfo[fieldOffset + 17];
                    fieldsInfo[fieldIndex].Flags = (DBaseFieldFlags) bufFieldsInfo[fieldOffset + 18];

                }
                #endregion

                //while (memoryStream.Position < dBaseHeader.HeaderSize) memoryStream.ReadByte();

                #region Prepare DataTable
                var dataTable = new DataTable(StiFileItemTable.DefaultDBaseTableName);
                foreach (var t in fieldsInfo)
                {
                    #region Column Types
                    Type columnType;
                    switch (t.Type)
                    {
                        case DBaseFieldType.Char:
                            columnType = typeof(string);
                            break;

                        case DBaseFieldType.Date:
                            columnType = typeof(DateTime);
                            break;

                        case DBaseFieldType.Float:
                            columnType = typeof(decimal);
                            break;

                        case DBaseFieldType.Numeric:
                            if ((t.FractionSize == 0) && (t.Size < 10))
                            {
                                columnType = typeof(int);
                            }
                            else
                            {
                                columnType = typeof(decimal);
                            }
                            break;

                        case DBaseFieldType.ShortInt:
                        case DBaseFieldType.LongInt:
                            columnType = typeof(int);
                            break;

                        case DBaseFieldType.Logical:
                            columnType = typeof(bool);
                            break;

                        case DBaseFieldType.Memo:
                            if ((t.Flags & DBaseFieldFlags.Binary) > 0)
                            {
                                columnType = typeof(object);
                            }
                            else
                            {
                                columnType = typeof(string);
                            }
                            break;

                        case DBaseFieldType.Binary:
                        case DBaseFieldType.Blob:
                        case DBaseFieldType.General:
                        case DBaseFieldType.Picture:
                            columnType = typeof(object);
                            break;

                        default:
                            columnType = typeof(object);
                            break;
                    }
                    #endregion

                    var column = new DataColumn(t.Name, columnType);
                    dataTable.Columns.Add(column);
                }
                dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
                #endregion

                #region Get memo info
                int memoRecordSize = 0;
                if (memo != null)
                {
                    memo.Seek(6, SeekOrigin.Begin);
                    memoRecordSize = memo.ReadByte() * 256 + memo.ReadByte();
                }
                byte[] memoRecordHeader = new byte[8];
                #endregion

                #region Read data
                var bufRecord = new byte[dBaseHeader.RecordSize];
                var decimalSeparator = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
                for (var rowIndex = 0; rowIndex < dBaseHeader.RecordsCount; rowIndex++)
                {
                    memoryStream.Read(bufRecord, 0, bufRecord.Length);
                    if (bufRecord[0] != (byte) '*') //not deleted
                    {
                        var dataRow = dataTable.NewRow();
                        var fieldOffset = 1;
                        for (var fieldIndex = 0; fieldIndex < fieldsInfo.Count; fieldIndex++)
                        {
                            #region Get dataString
                            string dataString = string.Empty;
                            switch (fieldsInfo[fieldIndex].Type)
                            {
                                case DBaseFieldType.Char:
                                case DBaseFieldType.Date:
                                case DBaseFieldType.Float:
                                case DBaseFieldType.Numeric:
                                    {
                                        var sb = new StringBuilder();
                                        for (int byteIndex = 0; byteIndex < fieldsInfo[fieldIndex].Size; byteIndex++)
                                        {
                                            sb.Append((char) bufRecord[fieldOffset + byteIndex]);
                                        }
                                        dataString = sb.ToString().TrimEnd();
                                    }
                                    break;
                            }
                            #endregion

                            #region Parse data
                            switch (fieldsInfo[fieldIndex].Type)
                            {
                                case DBaseFieldType.Char:
                                    #region parse string
                                    if ((fieldsInfo[fieldIndex].Flags & DBaseFieldFlags.Binary) > 0)
                                    {
                                        dataRow[fieldIndex] = dataString;
                                    }
                                    else
                                    {
                                        char[] chars = new char[fieldsInfo[fieldIndex].Size];
                                        decoder.GetChars(bufRecord, fieldOffset, fieldsInfo[fieldIndex].Size, chars, 0);
                                        dataRow[fieldIndex] = new string(chars);
                                    }
                                    #endregion
                                    break;

                                case DBaseFieldType.Date:
                                    #region parse date
                                    if (!string.IsNullOrWhiteSpace(dataString))
                                    {
                                        try
                                        {
                                            dataRow[fieldIndex] = new DateTime(
                                                int.Parse(dataString.Substring(0, 4)),
                                                int.Parse(dataString.Substring(4, 2)),
                                                int.Parse(dataString.Substring(6, 2)));
                                        }
                                        catch
                                        {
                                            //dataRow[fieldIndex] = DBNull.Value;
                                        }
                                    }
                                    #endregion
                                    break;

                                case DBaseFieldType.Float:
                                case DBaseFieldType.Numeric:
                                    #region parse decimal
                                    if (dataString.Length > 0)
                                    {
                                        if ((fieldsInfo[fieldIndex].FractionSize == 0) && (fieldsInfo[fieldIndex].Size < 10))
                                        {
                                            try
                                            {
                                                dataRow[fieldIndex] = int.Parse(dataString);
                                            }
                                            catch
                                            {
                                            }
                                        }
                                        else
                                        {
                                            try
                                            {
                                                dataRow[fieldIndex] = decimal.Parse(dataString.Replace(".", decimalSeparator));
                                            }
                                            catch
                                            {
                                            }
                                        }
                                    }
                                    #endregion
                                    break;

                                case DBaseFieldType.LongInt:
                                    dataRow[fieldIndex] = BitConverter.ToInt32(bufRecord, fieldOffset);
                                    break;

                                case DBaseFieldType.Memo:
                                case DBaseFieldType.Binary:
                                case DBaseFieldType.Blob:
                                case DBaseFieldType.General:
                                case DBaseFieldType.Picture:
                                    #region parse Memo
                                    int memoIndex = BitConverter.ToInt32(bufRecord, fieldOffset);
                                    if ((memoIndex > 0) && (memo != null))
                                    {
                                        try
                                        {
                                            memo.Seek(memoIndex * memoRecordSize, SeekOrigin.Begin);
                                            memo.Read(memoRecordHeader, 0, 8);
                                            int recordType = memoRecordHeader[3];
                                            int recordSize = memoRecordHeader[4] * 256 * 256 * 256 + memoRecordHeader[5] * 256 * 256 + memoRecordHeader[6] * 256 + memoRecordHeader[7];
                                            byte[] buf = new byte[recordSize];
                                            memo.Read(buf, 0, recordSize);
                                            if ((fieldsInfo[fieldIndex].Type == DBaseFieldType.Memo) && ((fieldsInfo[fieldIndex].Flags & DBaseFieldFlags.Binary) == 0))
                                            {
                                                char[] chars = new char[recordSize];
                                                decoder.GetChars(buf, 0, recordSize, chars, 0);
                                                dataRow[fieldIndex] = new string(chars);
                                            }
                                            else
                                            {
                                                dataRow[fieldIndex] = buf;
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    #endregion

                                    break;

                                case DBaseFieldType.Logical:
                                    #region parse bool
                                    var logical = (char) bufRecord[fieldOffset];
                                    if (logical == 'T' || logical == 't' || logical == 'Y' || logical == 'y')
                                    {
                                        dataRow[fieldIndex] = true;
                                    }
                                    if (logical == 'F' || logical == 'f' || logical == 'N' || logical == 'n')
                                    {
                                        dataRow[fieldIndex] = true;
                                    }
                                    #endregion
                                    break;

                                default:
                                    dataRow[fieldIndex] = DBNull.Value;
                                    break;
                            }
                            #endregion

                            fieldOffset += fieldsInfo[fieldIndex].Size;
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }
                #endregion

                return dataTable;
            }
        }
        #endregion
    }
}