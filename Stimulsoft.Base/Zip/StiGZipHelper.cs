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
using System.IO;
using System.IO.Compression;

namespace Stimulsoft.Base
{
    public sealed class StiGZipHelper
    {
		#region Const
		//internal static int CompressionLevel = 6;
		#endregion

		#region Methods
        public static byte[] ConvertStringToByteArray(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return null;

            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamWriter writer = new StreamWriter(memoryStream))
            {
                writer.Write(str);
                writer.Flush();
                memoryStream.Flush();
                memoryStream.Close();
                return memoryStream.ToArray();
            }
        }


        public static string ConvertByteArrayToString(byte[] bytes)
        {
            if (bytes == null) return null;

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                string str = reader.ReadToEnd();
                reader.Close();
                memoryStream.Flush();
                memoryStream.Close();
                return str;
            }
        }


        public static string Pack(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return null;

            return Convert.ToBase64String(Pack(ConvertStringToByteArray(str)));
        }


        public static string Unpack(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return null;

            return ConvertByteArrayToString(Unpack(Convert.FromBase64String(str)));
        }


        public static byte[] Pack(byte[] bytes)
        {
            if (bytes == null) return null;

            using (MemoryStream memoryStream = new MemoryStream())
            using (GZipStream zipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                zipStream.Write(bytes, 0, bytes.Length);
                zipStream.Close();

                return memoryStream.ToArray();
            }
        }

        public static Stream Pack(Stream stream)
        {
            MemoryStream resultMemoryStream = new MemoryStream();
            using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Compress, true))
            {
                int size = 2048;
                byte[] data = new byte[2048];
                while (true)
                {
                    size = zipStream.Read(data, 0, data.Length);
                    if (size > 0)
                    {
                        resultMemoryStream.Write(data, 0, size);
                    }
                    else break;
                }
                return resultMemoryStream;
            }
        }


        public static byte[] Unpack(byte[] bytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            using (GZipStream zipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            using (MemoryStream resultMemoryStream = new MemoryStream())
            {
                int size = 2048;
                byte[] data = new byte[2048];
                while (true)
                {
                    size = zipStream.Read(data, 0, data.Length);
                    if (size > 0)
                    {
                        resultMemoryStream.Write(data, 0, size);
                    }
                    else break;
                }
                return resultMemoryStream.ToArray();
            }
        }

        public static Stream Unpack(Stream stream)
        {
            MemoryStream resultMemoryStream = new MemoryStream();
            using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Decompress))            
            {
                int size = 2048;
                byte[] data = new byte[2048];
                while (true)
                {
                    size = zipStream.Read(data, 0, data.Length);
                    if (size > 0)
                    {
                        resultMemoryStream.Write(data, 0, size);
                    }
                    else break;
                }                
            }
            return resultMemoryStream;
        }

        public static void Unpack(Stream stream, Stream resultStream)
        {
            using (GZipStream zipStream = new GZipStream(stream, CompressionMode.Decompress))
            {
                int size = 2048;
                byte[] data = new byte[2048];
                while (true)
                {
                    size = zipStream.Read(data, 0, data.Length);
                    if (size > 0)
                    {
                        resultStream.Write(data, 0, size);
                    }
                    else break;
                }
            }
            resultStream.Seek(0, SeekOrigin.Begin);
        }
		#endregion
    }
}
