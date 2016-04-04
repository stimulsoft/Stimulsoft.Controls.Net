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
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Base.Drawing
{	
	/// <summary>
	/// Class contains methods for rotated text drawing.
	/// </summary>
	public class StiRotatedTextDrawing
	{
		private static PointF GetStartPoint(StiRotationMode rotationMode, RectangleF textRect)
		{
			PointF centerPoint = new PointF(textRect.X + (textRect.Width / 2), textRect.Y + (textRect.Height / 2));

			switch (rotationMode)
			{	
				case StiRotationMode.LeftCenter:
					return new PointF(textRect.X, centerPoint.Y);

				case StiRotationMode.LeftBottom:
					return new PointF(textRect.X, textRect.Bottom);

				case StiRotationMode.CenterTop:
					return new PointF(centerPoint.X, textRect.Top);

				case StiRotationMode.CenterCenter:
					return centerPoint;

				case StiRotationMode.CenterBottom:
					return new PointF(centerPoint.X, textRect.Bottom);

				case StiRotationMode.RightTop:
					return new PointF(textRect.Right, textRect.Top);

				case StiRotationMode.RightCenter:
					return new PointF(textRect.Right, centerPoint.Y);

				case StiRotationMode.RightBottom:
					return new PointF(textRect.Right, textRect.Bottom);

				default:
					return textRect.Location;
			}
		}


		private static RectangleF DrawString(Graphics g, string text, Font font, Brush brush, 
			PointF point, StringFormat stringFormat, StiRotationMode rotationMode, float angle, 
			bool antialiasing, bool draw, bool measure, int maximalWidth)
        {
			if (string.IsNullOrEmpty(text))return new RectangleF(point.X, point.Y, 0, 0);
			SizeF textSize = g.MeasureString(text, font, maximalWidth, stringFormat);
			if (textSize.Width == 0 && textSize.Height == 0)return new RectangleF(point.X, point.Y, 0, 0);
			GraphicsState state = g.Save();

			TextRenderingHint textHint = g.TextRenderingHint;

			if (antialiasing)g.TextRenderingHint = TextRenderingHint.AntiAlias;

			float px = point.X;
			float py = point.Y;

			
			RectangleF textRect = new RectangleF(0, 0, textSize.Width, textSize.Height);

			g.TranslateTransform(px, py);
			g.RotateTransform(angle);

			PointF startPoint = GetStartPoint(rotationMode, textRect);

			textRect.X -= startPoint.X;
			textRect.Y -= startPoint.Y;

			RectangleF rect = RectangleF.Empty;

			if (textRect.Width != 0 && textRect.Height != 0)
			{

				if (draw)g.DrawString(text, font, brush, textRect, stringFormat);

                //Red line
                //if (draw)
                    //g.DrawLine(Pens.Red, textRect.X, textRect.Y + textRect.Height / 2, textRect.Right, textRect.Y + textRect.Height / 2);

				if (measure)
				{
					using (GraphicsPath path = new GraphicsPath())
					{
						path.AddRectangle(textRect);
						path.Transform(g.Transform);
						rect = path.GetBounds();	
					}
				}

				if (antialiasing)g.TextRenderingHint = textHint;
			}

			g.RotateTransform(-angle);
			g.TranslateTransform(-px, -py);
			
			if (measure)
			{
				rect = new RectangleF(rect.X - g.Transform.OffsetX, rect.Y - g.Transform.OffsetY, rect.Width, rect.Height);
			}

			rect.Width++;
			rect.Height++;

			g.Restore(state);

			return rect;
		}


		public static void DrawString(Graphics g, string text, Font font, Brush brush, 
			PointF point, StringFormat stringFormat, StiRotationMode rotationMode, float angle, bool antialiasing)
		{
			DrawString(g, text, font, brush, 
				point, stringFormat, rotationMode, angle, antialiasing, 0);
		}

	
		public static void DrawString(Graphics g, string text, Font font, Brush brush, 
			PointF point, StringFormat stringFormat, StiRotationMode rotationMode, float angle, bool antialiasing,
			int maximalWidth)
		{
			DrawString(g, text, font, brush, point, stringFormat, rotationMode, angle, 
				antialiasing, true, false, maximalWidth);
		}


		public static RectangleF MeasureString(Graphics g, string text, Font font, 
			PointF point, StringFormat stringFormat, StiRotationMode rotationMode, float angle, int maximalWidth)
		{
			return DrawString(g, text, font, null, point, stringFormat, rotationMode, angle, 
				false, false, true, maximalWidth);
		}

	
		public static RectangleF MeasureString(Graphics g, string text, Font font, 
			PointF point, StringFormat stringFormat, StiRotationMode rotationMode, float angle)
		{
			return MeasureString(g, text, font, 
				point, stringFormat, rotationMode, angle, 0);
		}

		
		public static RectangleF MeasureString(Graphics g, string text, Font font, 
			RectangleF rect, StringFormat stringFormat, StiRotationMode rotationMode, float angle, int maximalWidth)
		{
			PointF point = new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
			return DrawString(g, text, font, null, point, stringFormat, rotationMode, angle, 
				false, false, true, maximalWidth);
		}

		public static RectangleF MeasureString(Graphics g, string text, Font font, 
			RectangleF rect, StringFormat stringFormat, StiRotationMode rotationMode, float angle)
		{
			return MeasureString(g, text, font, 
				rect, stringFormat, rotationMode, angle, 0);
		}


		public static void DrawString(Graphics g, string text, Font font, Brush brush, 
			RectangleF rect, StringFormat stringFormat, StiRotationMode rotationMode, float angle, bool antialiasing,
			int maximalWidth)
		{
			PointF point = new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
			DrawString(g, text, font, brush, point, stringFormat, rotationMode, angle, antialiasing, maximalWidth);
		}

		public static void DrawString(Graphics g, string text, Font font, Brush brush, 
			RectangleF rect, StringFormat stringFormat, StiRotationMode rotationMode, float angle, bool antialiasing)
		{
			DrawString(g, text, font, brush, 
				rect, stringFormat, rotationMode, angle, antialiasing, 0);
		}

		
		public static void DrawString(Graphics g, string text, Font font, Brush brush, 
			RectangleF rect, StringFormat stringFormat, float angle, bool antialiasing, int maximalWidth)
		{
			DrawString(g, text, font, brush, rect, stringFormat, StiRotationMode.CenterCenter, angle, antialiasing,
				maximalWidth);
		}

		public static void DrawString(Graphics g, string text, Font font, Brush brush, 
			RectangleF rect, StringFormat stringFormat, float angle, bool antialiasing)
		{
			DrawString(g, text, font, brush, rect, stringFormat, angle, antialiasing, 0);
		}
	

		/// <summary>
		/// Draws text at an angle.
		/// </summary>
		/// <param name="g">Graphics to draw on.</param>
		/// <param name="text">Text to draw on.</param>
		/// <param name="font">Font to draw on.</param>
		/// <param name="brush">Brush to draw.</param>
		/// <param name="rect">Rectangle to draw.</param>
		/// <param name="stringFormat">Text format.</param>
		/// <param name="angle">Show text at an angle.</param>
		public static void DrawString(Graphics g, string text, Font font, Brush brush, 
			RectangleF rect, StringFormat stringFormat, float angle, int maximalWidth)
		{
			DrawString(g, text, font, brush, rect, stringFormat,  angle, false, maximalWidth);
		}

		public static void DrawString(Graphics g, string text, Font font, Brush brush, 
			RectangleF rect, StringFormat stringFormat, float angle)
		{
			DrawString(g, text, font, brush, rect, stringFormat, angle, 0);
		}
	}
}
