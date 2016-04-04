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
    public class StiRadialRangeGaugeGeom : StiGaugeGeom
    {
        #region Fields
        public readonly RectangleF rect;
        public readonly StiBrush background;
        public readonly StiBrush borderBrush;
        public readonly float borderWidth;
        public readonly PointF centerPoint;
        public readonly float startAngle;
        public readonly float sweepAngle;
        public readonly float radius1;
        public readonly float radius2;
        public readonly float radius3;
        public readonly float radius4;
        #endregion

        #region Properties
        public override StiGaugeGeomType Type
        {
            get 
            {
                return StiGaugeGeomType.RadialRange;
            }
        }
        #endregion

        public StiRadialRangeGaugeGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, float borderWidth, PointF centerPoint, 
            float startAngle, float sweepAngle, float radius1, float radius2, float radius3, float radius4)
        {
            this.rect = rect;
            this.background = background;
            this.borderBrush = borderBrush;
            this.borderWidth = borderWidth;
            this.centerPoint = centerPoint;
            this.startAngle = startAngle;
            this.sweepAngle = sweepAngle;
            this.radius1 = radius1;
            this.radius2 = radius2;
            this.radius3 = radius3;
            this.radius4 = radius4;
        }
    }
}