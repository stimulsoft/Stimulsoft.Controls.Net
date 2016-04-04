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

using System.Text;

namespace Stimulsoft.Base
{
    /// <summary>
    /// Class for adjustment aspects of Stimulsoft Reports.
    /// </summary>
    public sealed class StiBaseOptions
    {

        private static bool fullTrust = true;
        public static bool FullTrust
        {
            get
            {
                return fullTrust;
            }
            set
            {
                fullTrust = value;
            }
        }

        private static bool fipsCompliance = false;
        public static bool FIPSCompliance
        {
            get
            {
                return fipsCompliance;
            }
            set
            {
                fipsCompliance = value;
            }
        }

        private static Encoding webClientEncoding = Encoding.UTF8;
        public static Encoding WebClientEncoding
        {
            get
            {
                return webClientEncoding;
            }
            set
            {
                webClientEncoding = value;
            }
        }
    }
}
