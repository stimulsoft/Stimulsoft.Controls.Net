#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports.Net											}
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
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Gauge;

namespace Stimulsoft.Base.Gauge.GaugeGeoms
{
    public class StiRoundedRectangleGaugeGeom : StiGaugeGeom
    {
        #region Fields
        public readonly RectangleF Rect;
        public readonly StiBrush Background;
        public readonly StiBrush BorderBrush;
        public readonly float BorderWidth;
        public readonly int LeftTop;
        public readonly int RightTop;
        public readonly int RightBottom;
        public readonly int LeftBottom;
        #endregion

        #region Properties
        public override StiGaugeGeomType Type
        {
            get 
            {
                return StiGaugeGeomType.RoundedRectangle;
            }
        }
        #endregion

        public StiRoundedRectangleGaugeGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth, int leftTop, int rightTop, int rightBottom, int leftBottom)
        {
            this.Rect = rect;
            this.Background = background;
            this.BorderBrush = borderBrush;
            this.BorderWidth = borderWidth;
            this.LeftTop = leftTop;
            this.RightTop = rightTop;
            this.RightBottom = rightBottom;
            this.LeftBottom = leftBottom;
        }
    }
}