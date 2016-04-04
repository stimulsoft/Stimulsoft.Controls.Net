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
using System.ComponentModel;
using System.Net;

namespace Stimulsoft.Base
{
    [ToolboxItem(false)]
    public class StiWebClientEx : WebClient
    {
        #region Properties
        private CookieContainer container = null;
        public CookieContainer CookieContainer
        {
            get
            {
                return container; 
                
            }
            set
            {
                container = value; 
            }
        }
        #endregion

        #region WebClient override
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest r = base.GetWebRequest(address);
            if (container != null)
            {
                var request = r as HttpWebRequest;
                if (request != null)
                {
                    request.CookieContainer = container;
                }
            }
            return r;
        }

        //protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        //{
        //    WebResponse response = base.GetWebResponse(request, result);
        //    ReadCookies(response);
        //    return response;
        //}

        //protected override WebResponse GetWebResponse(WebRequest request)
        //{
        //    WebResponse response = base.GetWebResponse(request);
        //    ReadCookies(response);
        //    return response;
        //}

        //private void ReadCookies(WebResponse r)
        //{
        //    var response = r as HttpWebResponse;
        //    if (response != null)
        //    {
        //        CookieCollection cookies = response.Cookies;
        //        container.Add(cookies);
        //    }
        //}
        #endregion

        public StiWebClientEx(CookieContainer container)
        {
            this.container = container;
            this.Encoding = StiBaseOptions.WebClientEncoding;
        }
    }
}
