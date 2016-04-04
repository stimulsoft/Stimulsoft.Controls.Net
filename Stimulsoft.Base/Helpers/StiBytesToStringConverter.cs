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
using System.IO;

namespace Stimulsoft.Base
{
    public static class StiBytesToStringConverter
    {
        public static string ConvertBytesToString(byte[] bytes)
        {
            if (bytes == null) return null;

            using (var memoryStream = new MemoryStream(bytes))
            using (var reader = new StreamReader(memoryStream))
            {
                var str = reader.ReadToEnd();
                reader.Close();
                memoryStream.Flush();
                memoryStream.Close();
                return str;
            }
        }

        public static byte[] ConvertStringToBytes(string str)
        {
            if (str == null) return null;

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write(str);
                writer.Close();
                memoryStream.Flush();
                memoryStream.Close();
                return memoryStream.ToArray();
            }
        }
    }
}
