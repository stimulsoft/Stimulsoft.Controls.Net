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

using System;
using System.Drawing;
using System.Collections.Generic;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Context
{
    public class StiContext
    {
        #region Methods.Render
        public void Render(RectangleF rect)
        {
            contextPainter.Render(rect, geoms);
        }
        #endregion

        #region Methods.StringFormat
        public StiStringFormatGeom GetDefaultStringFormat()
        {
            return contextPainter.GetDefaultStringFormat();
        }

        public StiStringFormatGeom GetGenericStringFormat()
        {
            return contextPainter.GetGenericStringFormat();
        }
        #endregion

        #region Methods.Text
        public StiTextGeom DrawString(string text, StiFontGeom font, object brush, Rectangle rect, StiStringFormatGeom sf)
        {
            StiTextGeom textGeom = new StiTextGeom(text, font, brush, rect, sf, false);
            geoms.Add(textGeom);
            return textGeom;
        }

        public StiTextGeom DrawString(string text, StiFontGeom font, object brush, RectangleF rect, StiStringFormatGeom sf)
        {
            StiTextGeom textGeom = new StiTextGeom(text, font, brush, rect, sf, false);
            geoms.Add(textGeom);
            return textGeom;
        }

        public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, Rectangle rect, StiStringFormatGeom sf, float angle, bool antialiasing)
        {
            StiTextGeom textGeom = new StiTextGeom(text, font, brush, new RectangleF(rect.X, rect.Y, rect.Width, rect.Height), sf, angle, antialiasing, true);
            geoms.Add(textGeom);
            return textGeom;
        }

        public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, RectangleF rect, StiStringFormatGeom sf, float angle, bool antialiasing)
        {
            StiTextGeom textGeom = new StiTextGeom(text, font, brush, rect, sf, angle, antialiasing, true);
            geoms.Add(textGeom);
            return textGeom;
        }

        public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, PointF pos, StiStringFormatGeom sf, StiRotationMode mode,
            float angle, bool antialiasing)
        {
            StiTextGeom textGeom = new StiTextGeom(text, font, brush, pos, sf, angle, antialiasing, mode, true);
            geoms.Add(textGeom);
            return textGeom;
        }

        public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, Rectangle rect, StiStringFormatGeom sf, StiRotationMode mode,
            float angle, bool antialiasing)
        {
            StiTextGeom textGeom = new StiTextGeom(text, font, brush, rect, sf, angle, antialiasing, mode, true);
            geoms.Add(textGeom);
            return textGeom;
        }

        public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, Rectangle rect, StiStringFormatGeom sf, StiRotationMode mode,
            float angle, bool antialiasing, int maximalWidth)
        {
            StiTextGeom textGeom = new StiTextGeom(text, font, brush, rect, sf, angle, antialiasing, maximalWidth, mode, true);
            geoms.Add(textGeom);
            return textGeom;
        }

        public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, RectangleF rect, StiStringFormatGeom sf, StiRotationMode mode,
            float angle, bool antialiasing, int maximalWidth)
        {
            StiTextGeom textGeom = new StiTextGeom(text, font, brush, rect, sf, angle, antialiasing, maximalWidth, mode, true);
            geoms.Add(textGeom);
            return textGeom;
        }

        public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, RectangleF rect, StiStringFormatGeom sf, StiRotationMode mode,
            float angle, bool antialiasing)
        {
            StiTextGeom textGeom = new StiTextGeom(text, font, brush, rect, sf, angle, antialiasing, mode, true);
            geoms.Add(textGeom);
            return textGeom;
        }

        public StiTextGeom DrawRotatedString(string text, StiFontGeom font, object brush, PointF pos, StiStringFormatGeom sf, StiRotationMode mode,
            float angle, bool antialiasing, int maximalWidth)
        {
            StiTextGeom textGeom = new StiTextGeom(text, font, brush, pos, sf, angle, antialiasing, maximalWidth, mode, true);
            geoms.Add(textGeom);
            return textGeom;
        }
        #endregion

        #region Methods.Text.Measure
        public SizeF MeasureString(string text, StiFontGeom font)
        {
            return contextPainter.MeasureString(text, font);
        }

        public SizeF MeasureString(string text, StiFontGeom font, int width, StiStringFormatGeom sf)
        {
            return contextPainter.MeasureString(text, font, width, sf);
        }

        public RectangleF MeasureRotatedString(string text, StiFontGeom font, RectangleF rect, StiStringFormatGeom sf, float angle)
        {
            return contextPainter.MeasureRotatedString(text, font, rect, sf, angle);
        }

        public RectangleF MeasureRotatedString(string text, StiFontGeom font, RectangleF rect, StiStringFormatGeom sf, StiRotationMode mode, float angle)
        {
            return contextPainter.MeasureRotatedString(text, font, rect, sf, mode, angle);
        }

        public RectangleF MeasureRotatedString(string text, StiFontGeom font, PointF point, StiStringFormatGeom sf, StiRotationMode mode, float angle, int maximalWidth)
        {
            return contextPainter.MeasureRotatedString(text, font, point, sf, mode, angle, maximalWidth);
        }

        public RectangleF MeasureRotatedString(string text, StiFontGeom font, PointF point, StiStringFormatGeom sf, StiRotationMode mode, float angle)
        {
            return contextPainter.MeasureRotatedString(text, font, point, sf, mode, angle);
        }            
        #endregion

        #region Methods.Shadow
        public void DrawShadow(StiContext sg, RectangleF rect, float radius)
        {
            geoms.Add(new StiShadowGeom(sg, rect, radius));
        }

        public void DrawCachedShadow(RectangleF rect, StiShadowSides sides, bool isPrinting)
        {
            geoms.Add(new StiCachedShadowGeom(rect, sides, isPrinting));
        }

        public StiContext CreateShadowGraphics()
        {
            return contextPainter.CreateShadowGraphics(this.Options.IsPrinting, this.Options.Zoom);
        }
        #endregion  

        #region Methods.Transform
        public void PushTranslateTransform(float x, float y)
        {
            geoms.Add(new StiPushTranslateTransformGeom(x, y));
        }

        public void PushRotateTransform(float angle)
        {
            geoms.Add(new StiPushRotateTransformGeom(angle));
        }

        public void PopTransform()
        {
            geoms.Add(new StiPopTransformGeom());
        }
        #endregion

        #region Methods.Clip
        public void PushClip(RectangleF clipRect)
        {
            geoms.Add(new StiPushClipGeom(clipRect));
        }

        public void PopClip()
        {
            geoms.Add(new StiPopClipGeom());
        }
        #endregion

        #region Methods.Animation
        public void DrawAnimationColumn(object brush, StiPenGeom borderPen, object rect, TimeSpan duration, TimeSpan? beginTime, bool upMove, string toolTip, object tag)
        {
            geoms.Add(new StiClusteredColumnSeriesAnimationGeom(brush, borderPen, rect, duration, beginTime, upMove, toolTip, tag));
        }

        public void DrawAnimationBar(object brush, StiPenGeom borderPen, object rect, TimeSpan duration, TimeSpan? beginTime, bool upMove, string toolTip, object tag)
        {
            geoms.Add(new StiClusteredBarSeriesAnimationGeom(brush, borderPen, rect, duration, beginTime, upMove, toolTip, tag));
        }

        public void DrawAnimationRectangle(object brush, StiPenGeom pen, Rectangle rect, TimeSpan duration, TimeSpan? beginTime)
        {
            geoms.Add(new StiBorderAnimationGeom(brush, pen, rect, duration, beginTime, StiAnimationType.Opacity));
        }

        public void DrawAnimationPathElement(object brush, StiPenGeom borderPen, List<StiSegmentGeom> path, object rect, TimeSpan duration, TimeSpan? beginTime, string toolTip, object tag)
        {
            geoms.Add(new StiPathElementAnimationGeom(brush, borderPen, path, rect, duration, beginTime, toolTip, tag));
        }

        public void DrawAnimationLabel(string text, StiFontGeom font, object textBrush, object labelBrush, StiPenGeom penBorder, Rectangle rect, StiStringFormatGeom sf, StiRotationMode mode,
            float angle, bool drawBorder, TimeSpan duration, TimeSpan? beginTime)
        {
            geoms.Add(new StiLabelAnimationGeom(text, font, textBrush, labelBrush, penBorder, rect, sf, mode, angle, drawBorder, duration, beginTime));
        }

        public void DrawAnimationLines(StiPenGeom pen, PointF[] points, TimeSpan duration, TimeSpan? beginTime, StiAnimationType animationType)
        {
            geoms.Add(new StiLinesAnimationGeom(pen, points, duration, beginTime, animationType));
        }

        public void DrawAnimationCurve(StiPenGeom pen, PointF[] points, float tension, TimeSpan duration, TimeSpan? beginTime)
        {
            geoms.Add(new StiCurveAnimationGeom(pen, points, tension, duration, beginTime));
        }

        public void FillDrawAnimationPath(object brush, StiPenGeom pen, List<StiSegmentGeom> path, object rect, TimeSpan durationOpacity, TimeSpan? beginTimeOpacity, object tag)
        {
            geoms.Add(new StiPathAnimationGeom(brush, pen, path, rect, durationOpacity, beginTimeOpacity, tag));
        }

        public void FillDrawAnimationEllipse(object brush, StiPenGeom pen, float x, float y, float width, float height, TimeSpan durationOpacity, TimeSpan? beginTimeOpacity, StiAnimationType animationType, string toolTip, object tag)
        {
            geoms.Add(new StiEllipseAnimationGeom(brush, pen, new RectangleF(x, y, width, height), durationOpacity, beginTimeOpacity, animationType, toolTip, tag));
        }
        #endregion

        #region Methods.Primitives
        public void DrawLine(StiPenGeom pen, float x1, float y1, float x2, float y2)
        {
            geoms.Add(new StiLineGeom(pen, x1, y1, x2, y2));
        }

        public void DrawLines(StiPenGeom pen, PointF[] points)
        {
            #region Propection from NaN values
            for (int index = 0; index < points.Length; index++)
            {
                PointF pos = points[index];

                if (double.IsNaN(pos.X))
                    pos.X = 0;
                if (double.IsNaN(pos.Y))
                    pos.Y = 0;

                points[index] = pos;
            }
            #endregion

            geoms.Add(new StiLinesGeom(pen, points));
        }

        public void DrawRectangle(StiPenGeom pen, Rectangle rect)
        {
            geoms.Add(new StiBorderGeom(null, pen, rect));
        }

        public void DrawRectangle(StiPenGeom pen, float x, float y, float width, float height)
        {
            geoms.Add(new StiBorderGeom(null, pen, new RectangleF(x, y, width, height)));
        }

        public void DrawEllipse(StiPenGeom pen, float x, float y, float width, float height)
        {
            geoms.Add(new StiEllipseGeom(null, pen, new RectangleF(x, y, width, height)));
        }

        public void DrawEllipse(StiPenGeom pen, RectangleF rect)
        {
            geoms.Add(new StiEllipseGeom(null, pen, rect));
        }

        public void FillEllipse(object brush, float x, float y, float width, float height)
        {
            geoms.Add(new StiEllipseGeom(brush, null, new RectangleF(x, y, width, height)));
        }

        public void FillEllipse(object brush, RectangleF rect)
        {
            geoms.Add(new StiEllipseGeom(brush, null, rect));
        }

        public void DrawPath(StiPenGeom pen, List<StiSegmentGeom> path, object rect)
        {
            geoms.Add(new StiPathGeom(null, pen, path, rect));
        }

        public void FillPath(object brush, List<StiSegmentGeom> path, object rect)
        {
            geoms.Add(new StiPathGeom(brush, null, path, rect));            
        }

        public void DrawCurve(StiPenGeom pen, PointF[] points, float tension)
        {
            geoms.Add(new StiCurveGeom(pen, points, tension));
        }

        public void FillRectangle(object brush, Rectangle rect)
        {
            geoms.Add(new StiBorderGeom(brush, null, rect));
        }

        public void FillRectangle(object brush, float x, float y, float width, float height)
        {
            geoms.Add(new StiBorderGeom(brush, null, new RectangleF(x, y, width, height)));
        }
        #endregion

        #region Methods.Smothing
        public void PushSmoothingModeToAntiAlias()
        {
            geoms.Add(new StiPushSmothingModeToAntiAliasGeom());
        }

        public void PopSmoothingMode()
        {
            geoms.Add(new StiPopSmothingModeGeom());
        }
        #endregion

        #region Methods.Text Hint
        public void PushTextRenderingHintToAntiAlias()
        {
            geoms.Add(new StiPushTextRenderingHintToAntiAliasGeom());
        }

        public void PopTextRenderingHint() 
        {
            geoms.Add(new StiPopTextRenderingHintGeom());
        }
        #endregion

        #region Methods.GetPathBounds
        public RectangleF GetPathBounds(List<StiSegmentGeom> geoms)
        {
            return contextPainter.GetPathBounds(geoms);
        }
        #endregion

        #region Fields
        public List<StiGeom> geoms = new List<StiGeom>();
        #endregion        

        #region Properties
        private StiContextPainter contextPainter;
        public StiContextPainter ContextPainter
        {
            get
            {
                return contextPainter;
            }
        }

        private StiContextOptions options;
        public StiContextOptions Options
        {
            get
            {
                return options;
            }
        }
        #endregion

        #region Methods.Shadow
        public void DrawShadowRect(RectangleF rect, int shadowWidth, TimeSpan duration, TimeSpan? beginTime)
        {
            geoms.Add(new StiShadowAnimationGeom(rect, shadowWidth, duration, beginTime));
        }

        public void DrawShadowRect(RectangleF rect, double radiusX, double radiusY, int shadowWidth, TimeSpan duration, TimeSpan? beginTime)
        {
            geoms.Add(new StiShadowAnimationGeom(rect, radiusX, radiusY, shadowWidth, duration, beginTime));
        }
        #endregion 

        public StiContext(StiContextPainter contextPainter, bool isGdi, bool isWpf, bool isPrinting, float zoom)
        {
            this.contextPainter = contextPainter;

            this.options = new StiContextOptions(isGdi, isWpf, isPrinting, zoom);
        }
    }
}