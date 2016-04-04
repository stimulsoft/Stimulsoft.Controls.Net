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

using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Gauge.GaugeGeoms
{
    public class StiGraphicsPathGaugeGeom : StiGaugeGeom
    {
        #region Fields
        public readonly RectangleF Rect;
        public readonly StiBrush Background;
        public readonly StiBrush BorderBrush;
        public readonly float BorderWidth;
        public readonly PointF StartPoint;
        #endregion

        #region Properties
        public override StiGaugeGeomType Type
        {
            get 
            {
                return StiGaugeGeomType.GraphicsPath;
            }
        }

        private List<StiGaugeGeom> geoms = new List<StiGaugeGeom>();
        public List<StiGaugeGeom> Geoms
        {
            get
            {
                return this.geoms;
            }
        }
        #endregion

        #region Methods.Add
        public void AddGraphicsPathArcGaugeGeom(float x, float y, float width, float height, float startAngle, float sweepAngle)
        {
            if (width > 0 && height > 0)
                this.geoms.Add(new StiGraphicsPathArcGaugeGeom(x, y, width, height, startAngle, sweepAngle));
        }

        public void AddGraphicsPathCloseFigureGaugeGeom()
        {
            this.geoms.Add(new StiGraphicsPathCloseFigureGaugeGeom());
        }

        public void AddGraphicsPathLinesGaugeGeom(PointF[] points)
        {
            this.geoms.Add(new StiGraphicsPathLinesGaugeGeom(points));
        }

        public void AddGraphicsPathLineGaugeGeom(PointF p1, PointF p2)
        {
            this.geoms.Add(new StiGraphicsPathLineGaugeGeom(p1, p2));
        }
        #endregion

        public StiGraphicsPathGaugeGeom(RectangleF rect, PointF startPoint, StiBrush background, StiBrush borderBrush, float borderWidth)
        {
            this.Rect = rect;
            this.StartPoint = startPoint;
            this.Background = background;
            this.BorderBrush = borderBrush;
            this.BorderWidth = borderWidth;
            this.StartPoint = startPoint;
        }
    }
}