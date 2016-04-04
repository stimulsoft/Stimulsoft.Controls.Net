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

namespace Stimulsoft.Base.Drawing
{	
	public sealed class StiShadow
	{
		#region Methods
		public static void DrawCachedShadow(Graphics g, RectangleF rect, StiShadowSides sides, bool isSimple)
		{
			if (isSimple)
			{
				using (var path = new GraphicsPath())
				using (var brush = new SolidBrush(Color.FromArgb(50, Color.Black)))
				{
					if ((sides & StiShadowSides.Top) > 0)	
					{
						path.AddRectangle(new RectangleF(rect.Right, rect.Y + 4, 4, 4));
					}
				
					if ((sides & StiShadowSides.Right) > 0)
					{
						path.AddRectangle(new RectangleF(rect.Right, rect.Y + 8, 4, rect.Height - 8));
					}

					if ((sides & StiShadowSides.Edge) > 0)
					{
						path.AddRectangle(new RectangleF(rect.Right, rect.Bottom, 4, 4));
					}

					if ((sides & StiShadowSides.Bottom) > 0)
					{
						path.AddRectangle(new RectangleF(rect.X + 8, rect.Bottom, rect.Width - 8, 4));
					}

					if ((sides & StiShadowSides.Left) > 0)
					{
						path.AddRectangle(new RectangleF(rect.X + 4, rect.Bottom, 4, 4));
					}
					g.FillPath(brush, path);
				}
			}
			else 
			{
				DrawCachedShadow(g, ConvertRect(rect), sides);
			}
		}

		public static void DrawCachedShadow(Graphics g, Rectangle rect, StiShadowSides sides)
		{
			var cachedRect = rect;

			if (rect.Width <= 0)rect.Width = 1;
			if (rect.Height <= 0)rect.Height = 1;			

			using (var bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb))
			{
                //Color shadowColor = Color.FromArgb(ShadowPower, 0, 0, 0);

				using (var gg = Graphics.FromImage(bmp))
                //using (Brush shadowBrush = new SolidBrush(shadowColor))
				{             
                    //Core drawing
                    //gg.FillRectangle(shadowBrush, new Rectangle(4, 0, cachedRect.Width - 8, cachedRect.Height - 4));
					cachedRect.X = -4;
					cachedRect.Y = -4;

					if ((sides & StiShadowSides.Top) > 0)	StiShadow.DrawTopShadow(gg, cachedRect);
					if ((sides & StiShadowSides.Right) > 0)	StiShadow.DrawRightShadow(gg, cachedRect);
					if ((sides & StiShadowSides.Edge) > 0)	StiShadow.DrawEdgeShadow(gg, cachedRect);
					if ((sides & StiShadowSides.Bottom) > 0)StiShadow.DrawBottomShadow(gg, cachedRect);
					if ((sides & StiShadowSides.Left) > 0)	StiShadow.DrawLeftShadow(gg, cachedRect);
				}
				g.DrawImageUnscaled(bmp, rect.X + 4, rect.Y + 4);
			}
		}

		public static void DrawEdgeShadow(Graphics g, Rectangle rect)
		{
            lock (edgeShadow)
            {
                g.DrawImageUnscaled(edgeShadow, rect.Right, rect.Bottom, rect.Width * 3, rect.Height * 3);
            }
		}

        public static void DrawLeftShadow(Graphics g, Rectangle rect)
		{
            lock (leftShadow)
            {
                g.DrawImageUnscaled(leftShadow, rect.Left + ShadowSize, rect.Bottom);
            }
		}

        public static void DrawTopShadow(Graphics g, Rectangle rect)
		{
            lock (topShadow)
            {
                g.DrawImageUnscaled(topShadow, rect.Right, rect.Top + ShadowSize);
            }
		}

        public static void DrawRightShadow(Graphics g, Rectangle rect)
		{
			var startColor = Color.FromArgb(ShadowPower, 0, 0, 0);
			var endColor = Color.FromArgb(0, 0, 0, 0);

			var rectSideRight = new Rectangle(
				rect.Right - 1, 
				rect.Y + ShadowSize * 2, 
				ShadowSize + 1, 
				rect.Height - ShadowSize * 2);

            if (rectSideRight.Width != 0 && rectSideRight.Height != 0)
            {
                using (var brush = new LinearGradientBrush(rectSideRight, startColor, endColor, 0f))
                {
                    g.FillRectangle(brush, rectSideRight);
                }
            }
		}

        public static void DrawBottomShadow(Graphics g, Rectangle rect)
		{
			Color startColor = Color.FromArgb(ShadowPower, 0, 0, 0);
			Color endColor = Color.FromArgb(0, 0, 0, 0);

			var rectSideBottom = new Rectangle(
				rect.X + ShadowSize * 2, 
				rect.Bottom, 
				rect.Width - ShadowSize * 2, 
				ShadowSize);

            if (rectSideBottom.Width != 0 && rectSideBottom.Height != 0)
            {
                using (var brush = new LinearGradientBrush(rectSideBottom, startColor, endColor, 90f))
                {
                    g.FillRectangle(brush, rectSideBottom);
                }
            }
		}

		public static void DrawShadow(Graphics g, Rectangle rect)
		{
			DrawEdgeShadow(g, rect);
			DrawLeftShadow(g, rect);
			DrawTopShadow(g, rect);
			DrawRightShadow(g, rect);
			DrawBottomShadow(g, rect);
		}

		private static Rectangle ConvertRect(RectangleF rect)
		{
			return new Rectangle(
				(int)Math.Round(rect.X), 
				(int)Math.Round(rect.Y), 
				(int)Math.Round(rect.Width), 
				(int)Math.Round(rect.Height));
		}

		private static Bitmap GetEdgeShadow()
		{
			var bmp = new Bitmap(ShadowSize, ShadowSize, PixelFormat.Format32bppArgb);
			for (int indexY = 0; indexY < ShadowSize; indexY++)
			{
				int alphay = ShadowPower * (ShadowSize + 1 - indexY) / (ShadowSize + 1);
						
				for (int indexX = 0; indexX < ShadowSize; indexX++)
				{
					bmp.SetPixel(indexX, indexY, Color.FromArgb(alphay * (ShadowSize - indexX) / (ShadowSize + 1), 0, 0, 0));
				}
			}
	
			return bmp;
		}

		#endregion
		
		#region Fields
		internal const int ShadowSize = 4;
		internal const int ShadowPower = 99;
		
		private static Bitmap edgeShadow = null;
		private static Bitmap leftShadow = null;
		private static Bitmap topShadow = null;
		#endregion

		static StiShadow()
		{
			edgeShadow = GetEdgeShadow();
			leftShadow = edgeShadow.Clone() as Bitmap;
			topShadow = edgeShadow.Clone() as Bitmap;

			leftShadow.RotateFlip(RotateFlipType.Rotate180FlipY);
			topShadow.RotateFlip(RotateFlipType.Rotate180FlipX);
		}
	}
}
