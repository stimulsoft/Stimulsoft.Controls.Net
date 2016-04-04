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
using System.Net;
using System.ComponentModel;
using System.IO;


namespace Stimulsoft.Base.Drawing
{
    public sealed class StiRichTextFromURL
    {
        private static string baseAddress = null;
        public static string BaseAddress
        {
            get
            {                
                return baseAddress;
            }
            set
            {
                baseAddress = value;
            }
        }

        /// <summary>
        /// Load RichText from URL.
        /// </summary>
        public static string LoadRichText(string url, CookieContainer cookieContainer = null)
        {
            StiWebClientEx cl = new StiWebClientEx(cookieContainer);
            if (!string.IsNullOrEmpty(BaseAddress))
                cl.BaseAddress = BaseAddress;
            cl.Credentials = CredentialCache.DefaultCredentials;
            byte[] bytes = cl.DownloadData(url);
            cl.Dispose();

            MemoryStream stream = new MemoryStream(bytes);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.Default);
            string st = sr.ReadToEnd();
            sr.Close();
            return st;
        }

    }
}
