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

namespace Stimulsoft.Base.Context
{
    public class StiContextOptions
    {
        public bool IsPrinting
        {
            get
            {
                return false;
            }
        }

        private bool isWpf = false;
        public bool IsWpf
        {
            get
            {
                return isWpf;
            }
        }

        private bool isGdi = false;
        public bool IsGdi
        {
            get
            {
                return isGdi;
            }
        }

        private float zoom = 1f;
        public float Zoom
        {
            get
            {
                return zoom;
            }
        }
        /*
        public bool OldChartPercentMode
        {
            get
            {
                return StiOptions.Engine.OldChartPercentMode;
            }
        }*/

        internal StiContextOptions(bool isGdi, bool isWpf, bool isPrinting, float zoom)
        {
            this.isGdi = isGdi;
            this.isWpf = isWpf;
            this.zoom = zoom;
        }
    }
}
