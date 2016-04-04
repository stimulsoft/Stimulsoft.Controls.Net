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
using System.Text;
using System.Globalization;
using System.Drawing.Design;
using System.Drawing.Printing;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Runtime.InteropServices;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Services;
using Stimulsoft.Report;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Base.Drawing
{
    public static class StiShadow2013Painter
	{
        public static void DrawShadow(Graphics g, Rectangle rect, int size, Color color)
        {
            Rectangle shadowRect = new Rectangle(rect.Left - size, rect.Top - size, rect.Width + size * 2, rect.Height + size * 2);

            using (GraphicsPath path = GetRoundedPath(shadowRect))
            using (PathGradientBrush shadowBrush = new PathGradientBrush(path))
            {
                shadowBrush.WrapMode = WrapMode.Clamp;

                ColorBlend colorBlend = new ColorBlend(3);
                colorBlend.Colors = new Color[] { Color.Transparent, color, color };
                colorBlend.Positions = new float[] { 0f, .01f, 1f };

                shadowBrush.InterpolationColors = colorBlend;

                rect.Inflate(size + 2, size + 2);
                g.FillRectangle(shadowBrush, rect);
            }
        }

        private static GraphicsPath GetRoundedPath(Rectangle rect)
        {
            GraphicsPath path = new GraphicsPath();

            int space = 5;
            path.StartFigure();
            path.AddLine(rect.X + space, rect.Y, rect.Right - space, rect.Y);
            path.AddArc(rect.Right - 2 * space, rect.Y, 2 * space, 2 * space, -90, 90);
            path.AddLine(rect.Right, rect.Y + space, rect.Right, rect.Bottom - space);
            path.AddArc(rect.Right - 2 * space, rect.Bottom - 2 * space, 2 * space, 2 * space, 0, 90);
            path.AddLine(rect.Right - space, rect.Bottom, rect.X + space, rect.Bottom);
            path.AddArc(rect.X, rect.Bottom - 2 * space, 2 * space, 2 * space, 90, 90);
            path.AddLine(rect.X, rect.Bottom - space, rect.X, rect.Y + space);
            path.AddArc(rect.X, rect.Y, 2 * space, 2 * space, 180, 90);
            path.CloseFigure();

            return path;
        }
	}
}
