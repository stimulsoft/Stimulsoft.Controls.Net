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
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls
{	
	internal sealed class StiShadow
	{
		#region Methods
		internal static void DrawEdgeShadow(Graphics g, Rectangle rect)
		{
			g.DrawImageUnscaled(edgeShadow, rect.Right - ShadowSize, rect.Bottom - ShadowSize);
		}

		internal static void DrawLeftShadow(Graphics g, Rectangle rect)
		{
			g.DrawImageUnscaled(leftShadow, rect.Left + (ShadowSize >> 1), rect.Bottom - ShadowSize);
		}

		internal static void DrawTopShadow(Graphics g, Rectangle rect)
		{
			g.DrawImageUnscaled(topShadow, rect.Right - ShadowSize, rect.Top + (ShadowSize >> 1));
		}

		internal static void DrawRightShadow(Graphics g, Rectangle rect)
		{
			Color startColor = Color.FromArgb(ShadowPower, 0, 0, 0);
			Color endColor = Color.FromArgb(0, 0, 0, 0);

			Rectangle rectSideRight = new Rectangle(
				rect.Right - ShadowSize, 
				rect.Y + ShadowSize + (ShadowSize >> 1), 
				ShadowSize, 
				rect.Height - ShadowSize * 2 - (ShadowSize >> 1));

			using (var brush = new LinearGradientBrush(rectSideRight, startColor, endColor, 0f))
			{
				g.FillRectangle(brush, rectSideRight);
			}
		}

		internal static void DrawBottomShadow(Graphics g, Rectangle rect)
		{
			Color startColor = Color.FromArgb(ShadowPower, 0, 0, 0);
			Color endColor = Color.FromArgb(0, 0, 0, 0);

			Rectangle rectSideBottom = new Rectangle(
				rect.X + ShadowSize + (ShadowSize >> 1), 
				rect.Bottom - ShadowSize, 
				rect.Width - ShadowSize * 2 - (ShadowSize >> 1), 
				ShadowSize);

			using (var brush = new LinearGradientBrush(rectSideBottom, startColor, endColor, 90f))
			{
				g.FillRectangle(brush, rectSideBottom);
			}
		}

		internal static void DrawShadow(Graphics g, Rectangle rect)
		{
			DrawEdgeShadow(g, rect);
			DrawLeftShadow(g, rect);
			DrawTopShadow(g, rect);
			DrawRightShadow(g, rect);
			DrawBottomShadow(g, rect);
		}

		private static Bitmap GetEdgeShadow()
		{
			Bitmap bmp = new Bitmap(ShadowSize, ShadowSize, PixelFormat.Format32bppArgb);
            
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
		internal const int ShadowPower = 64;
		
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