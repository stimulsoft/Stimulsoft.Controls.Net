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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base;

namespace Stimulsoft.Base.Drawing
{
    public sealed class StiCapStyleHelper
    {
        public static int GetMaxWidthOfCapStyles(Graphics g, Font font)
        {
            Array obj = Enum.GetValues(typeof(StiCapStyle));

            int maxWidth = 0;

            for (int k = 0; k < obj.Length; k++)
            {
                object capStyle = obj.GetValue(k);
                string locName = StiLocalization.Get("PropertyEnum", typeof(StiCapStyle).Name + Enum.GetName(typeof(StiCapStyle), capStyle), false);

                int strWidth = (int)g.MeasureString(locName, font).Width;

                maxWidth = Math.Max(maxWidth, strWidth);
            }
            return maxWidth + 60;
        }


        public static void DrawCapStyle(DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = e.Bounds;
            Rectangle borderRect = new Rectangle(rect.X + 2, rect.Y + 2, 52, 14);

            #region Fill
            rect.Width--;
            StiControlPaint.DrawItem(g, rect, e.State, SystemColors.Window, SystemColors.ControlText);
            #endregion

            #region Paint border style
            Array obj = Enum.GetValues(typeof(StiCapStyle));

            using (Pen pen = new Pen(Color.DimGray, 1))
            {
                StiCapStyle capStyle = StiCapStyle.None;
                if (e.Index != -1) capStyle = (StiCapStyle)obj.GetValue(e.Index);

                g.FillRectangle(Brushes.White, borderRect);

                int yCenter = borderRect.Top + borderRect.Height / 2;
                int xStep = borderRect.Width / 4;
                PointF[] points = null;
                SmoothingMode mode = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                switch (capStyle)
                {
                    #region None
                    case StiCapStyle.None:
                        g.DrawLine(pen, borderRect.X + xStep, yCenter, borderRect.Right - xStep, yCenter);
                        break;
                    #endregion

                    #region Arrow
                    case StiCapStyle.Arrow:
                        g.DrawLine(pen, borderRect.X + xStep, yCenter, borderRect.Right - xStep, yCenter);
                        Rectangle capArrowRECT = new Rectangle(borderRect.Right - xStep - 4, borderRect.Y + 2, 5, 11);
                        g.SmoothingMode = mode;
                        points = new PointF[] { new PointF(capArrowRECT.Right, capArrowRECT.Y + (float)(capArrowRECT.Height / 2)), 
                            new PointF(capArrowRECT.Left, capArrowRECT.Y),
                            new PointF(capArrowRECT.Left, capArrowRECT.Bottom)};
                        using (Brush capBrush = new SolidBrush(Color.DimGray))
                        {
                            g.FillPolygon(capBrush, points);
                        }
                        break;
                    #endregion

                    #region Open
                    case StiCapStyle.Open:
                        g.DrawLine(pen, borderRect.X + xStep, yCenter, borderRect.Right - xStep, yCenter);
                        Rectangle capOpenRECT = new Rectangle(borderRect.Right - xStep - 4, borderRect.Y + 4, 5, 7);
                        points = new PointF[] {new PointF(capOpenRECT.X, capOpenRECT.Y),
                            new PointF(capOpenRECT.Right, capOpenRECT.Y + (float)(capOpenRECT.Height / 2)),
                            new PointF(capOpenRECT.X, capOpenRECT.Bottom)};
                        using (Pen openPen = new Pen(Color.DimGray))
                        {
                            g.DrawLines(openPen, points);
                        }
                        break;
                    #endregion

                    #region Stealth
                    case StiCapStyle.Stealth:
                        g.DrawLine(pen, borderRect.X + xStep, yCenter, borderRect.Right - xStep, yCenter);
                        Rectangle capStealthRECT = new Rectangle(borderRect.Right - xStep - 9, borderRect.Y + 2, 10, 11);
                        points = new PointF[] { new PointF(capStealthRECT.X, capStealthRECT.Y), 
                                new PointF(capStealthRECT.Right, capStealthRECT.Y + (float)(capStealthRECT.Height / 2)),
                                new PointF(capStealthRECT.X, capStealthRECT.Bottom), 
                                new PointF(capStealthRECT.X  + (float)(capStealthRECT.Width / 3), capStealthRECT.Y + (float)(capStealthRECT.Height / 2)),
                                new PointF(capStealthRECT.X, capStealthRECT.Y)};
                        using (Brush capBrush = new SolidBrush(Color.DimGray))
                        {
                            g.FillPolygon(capBrush, points);
                        }
                        break;
                    #endregion

                    #region Diamond
                    case StiCapStyle.Diamond:
                        g.DrawLine(pen, borderRect.X + xStep, yCenter, borderRect.Right - xStep, yCenter);
                        Rectangle capDiamondRECT = new Rectangle(borderRect.Right - xStep - 6, borderRect.Y + 3, 8, 9);
                        points = new PointF[] { new PointF(capDiamondRECT.X, capDiamondRECT.Y + (float)(capDiamondRECT.Height / 2)), 
                                new PointF(capDiamondRECT.X + (float)(capDiamondRECT.Width / 2), capDiamondRECT.Y),
                                new PointF(capDiamondRECT.Right, capDiamondRECT.Y + (float)(capDiamondRECT.Height / 2)), 
                                new PointF(capDiamondRECT.X + (float)(capDiamondRECT.Width / 2), capDiamondRECT.Bottom)};
                        using (Brush diamondBrush = new SolidBrush(Color.DimGray))
                        {
                            g.FillPolygon(diamondBrush, points);
                        }
                        break;
                    #endregion

                    #region Oval
                    case StiCapStyle.Oval:
                        g.DrawLine(pen, borderRect.X + xStep, yCenter, borderRect.Right - xStep, yCenter);
                        Rectangle capOvalRECT = new Rectangle(borderRect.Right - xStep - 4, borderRect.Y + 4, 6, 6);
                        using (Brush ovalBrush = new SolidBrush(Color.DimGray))
                        {
                            g.FillEllipse(ovalBrush, capOvalRECT);
                        }
                        break;
                    #endregion

                    #region Square
                    case StiCapStyle.Square:
                        g.DrawLine(pen, borderRect.X + xStep, yCenter, borderRect.Right - xStep, yCenter);
                        Rectangle capSquareRECT = new Rectangle(borderRect.Right - xStep - 4, borderRect.Y + 4, 6, 6);
                        g.SmoothingMode = mode;
                        using (Brush squareBrush = new SolidBrush(Color.DimGray))
                        {
                            g.FillRectangle(squareBrush, capSquareRECT);
                        }
                        break;
                    #endregion
                }
                g.SmoothingMode = mode;
            }

            g.DrawRectangle(Pens.Black, borderRect);
            #endregion

            #region Paint name
            using (Font font = new Font("Arial", 8))
            {
                using (StringFormat stringFormat = new StringFormat())
                {
                    stringFormat.LineAlignment = StringAlignment.Center;

                    object capStyle = obj.GetValue(e.Index);
                    string locName = StiLocalization.Get("PropertyEnum", typeof(StiCapStyle).Name + Enum.GetName(typeof(StiCapStyle), capStyle), false);

                    e.Graphics.DrawString(locName, font, Brushes.Black,
                        new Rectangle(55, rect.Top + 4, rect.Width - 18, 10),
                        stringFormat);
                }
            }
            #endregion

        }
    }
}
