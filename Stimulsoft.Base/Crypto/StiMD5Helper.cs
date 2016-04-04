#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports												}
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
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Stimulsoft.Base
{
    public class StiMD5Helper
    {
        public static byte[] ComputeHash(Stream stream)
        {
            Crypto.MD5 md5 = new Crypto.MD5();
            byte[] buf = new byte[4096];
            int len = buf.Length;
            while (len == buf.Length)
            {
                len = stream.Read(buf, 0, buf.Length);
                if (len > 0) md5.BlockUpdate(buf, 0, len);
            }
            return md5.GetHash();
        }

        public static byte[] ComputeHash(byte[] buf)
        {
            return ComputeHash(buf, 0, buf.Length);
        }

        public static byte[] ComputeHash(byte[] buf, int offset, int count)
        {
            byte[] hash = null;
            if (StiBaseOptions.FIPSCompliance)
            {
                Crypto.MD5 md5 = new Crypto.MD5();
                md5.BlockUpdate(buf, offset, count);
                hash = md5.GetHash();
            }
            else
            {
                MD5CryptoServiceProvider hashMD5 = new MD5CryptoServiceProvider();
                hash = hashMD5.ComputeHash(buf, offset, count);
                hashMD5.Clear();
            }
            return hash;
        }
    }
}
