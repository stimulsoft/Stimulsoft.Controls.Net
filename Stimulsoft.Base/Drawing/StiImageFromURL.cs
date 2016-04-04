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
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.IO;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Base.Drawing
{
    public sealed class StiImageFromURL
    {
        /// <summary>
        /// Load Bitmap from URL.
        /// </summary>
        public static Image LoadBitmap(string url, CookieContainer cookieContainer = null)
        {
            var bytes = StiBytesFromURL.Load(url, cookieContainer);
            var stream = new MemoryStream(bytes);
            return new Bitmap(stream);
        }


        /// <summary>
        /// Load Metafile from URL.
        /// </summary>
        public static Image LoadMetafile(string url, CookieContainer cookieContainer = null)
        {
            var bytes = StiBytesFromURL.Load(url, cookieContainer);
            return StiMetafileConverter.BytesToMetafile(bytes);
        }
    }
}
