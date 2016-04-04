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
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Base.Drawing
{	
	public class StiShadowGraphics : IDisposable
	{
        #region IDisposable
		public void Dispose()
		{
			graphics.Dispose();
			ShadowBitmap.Dispose();
		}
        #endregion

        #region Fields
		private Graphics graphics;
		public Graphics Graphics
		{
			get
			{
				return graphics;
			}
		}

		private Bitmap ShadowBitmap = null;
		private float factor = 3f;
        #endregion

        #region Methods
		public void DrawShadow(Graphics g, Rectangle rect, int shadowSize)
		{
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.HighQualityBilinear;

			g.DrawImage(ShadowBitmap, 
				rect.X - 5 * factor + shadowSize,
				rect.Y - 5 * factor + shadowSize,
				rect.Width + 10 * factor, 
				rect.Height + 10 * factor);
		}

		public void DrawShadow(Graphics g, RectangleF rect, float shadowSize)
		{
			g.CompositingQuality = CompositingQuality.HighQuality;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.HighQualityBilinear;

			g.DrawImage(ShadowBitmap, 
				rect.X - 5 * factor + shadowSize,
				rect.Y - 5 * factor + shadowSize,
				rect.Width + 10 * factor, 
				rect.Height + 10 * factor);
		}
        #endregion

		public StiShadowGraphics(RectangleF rect)
		{                
			ShadowBitmap = new Bitmap(
				(int)Math.Round(rect.Width / factor) + 10, 
				(int)Math.Round(rect.Height / factor) + 10, 
				PixelFormat.Format32bppArgb);

			graphics = Graphics.FromImage(ShadowBitmap);
			graphics.ScaleTransform(1 / factor, 1 / factor);
			graphics.TranslateTransform(5 * factor, 5 * factor);
                             
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
		}
	}
}
