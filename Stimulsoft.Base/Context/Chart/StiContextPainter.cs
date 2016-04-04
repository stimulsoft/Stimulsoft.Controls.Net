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

using System.Drawing;
using System.Collections.Generic;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Context
{
    public abstract class StiContextPainter
    {
        #region Methods.String Format
        public abstract StiStringFormatGeom GetDefaultStringFormat();

        public abstract StiStringFormatGeom GetGenericStringFormat();
        #endregion

        #region Methods.Shadow
        public abstract StiContext CreateShadowGraphics(bool isPrinting, float zoom);
        #endregion

        #region Methods.Path
        public abstract RectangleF GetPathBounds(List<StiSegmentGeom> geoms);
        #endregion

        #region Methods.Measure String
        public abstract SizeF MeasureString(string text, StiFontGeom font);

        public abstract SizeF MeasureString(string text, StiFontGeom font, int width, StiStringFormatGeom sf);

        public abstract RectangleF MeasureRotatedString(string text, StiFontGeom font, RectangleF rect, StiStringFormatGeom sf, float angle);

        public abstract RectangleF MeasureRotatedString(string text, StiFontGeom font, RectangleF rect, StiStringFormatGeom sf, StiRotationMode mode, float angle);

        public abstract RectangleF MeasureRotatedString(string text, StiFontGeom font, PointF point, StiStringFormatGeom sf, StiRotationMode mode, float angle, int maximalWidth);

        public abstract RectangleF MeasureRotatedString(string text, StiFontGeom font, PointF point, StiStringFormatGeom sf, StiRotationMode mode, float angle);
        #endregion

        #region Methods.Render
        public abstract void Render(RectangleF rect, List<StiGeom> geoms);
        #endregion
    }
}
