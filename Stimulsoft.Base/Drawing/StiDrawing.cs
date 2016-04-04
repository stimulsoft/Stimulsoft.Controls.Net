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
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Drawing
{	
	/// <summary>
	/// Class contains statistical methods for drawing.
	/// </summary>
	public sealed class StiDrawing
	{
		#region PaintRectangle
		/// <summary>
		/// Draws a rectangle specified by a coordinate pair, a width, and a height.
		/// </summary>
		/// <param name="g">The Graphics to draw on.</param>
		/// <param name="color">Color to draw Rectangle.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
		/// <param name="width">The width of the rectangle to draw.</param>
		/// <param name="height">The height of the rectangle to draw.</param>
		public static void DrawRectangle(Graphics g, Color color, double x, double y, double width, double height)
		{
			using (Pen pen = new Pen(color))
				DrawRectangle(g, pen, new RectangleD(x, y, width, height));
		}

		
		/// <summary>
		/// Draws a rectangle specified by a Rectangle structure.
		/// </summary>
		/// <param name="g">The Graphics to draw on.</param>
		/// <param name="color">Color to draw Rectangle.</param>
		/// <param name="rect">A Rectangle structure that represents the rectangle to draw.</param>
		public static void DrawRectangle(Graphics g, Color color, RectangleD rect)
		{
			using (Pen pen = new Pen(color))
				DrawRectangle(g, pen, rect);
		}

		
		/// <summary>
		/// Draws a rectangle specified by a coordinate pair, a width, and a height.
		/// </summary>
		/// <param name="g">The Graphics to draw on.</param>
		/// <param name="pen">A Pen object that determines the color, width, and style of the rectangle.</param>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
		/// <param name="width">The width of the rectangle to draw.</param>
		/// <param name="height">The height of the rectangle to draw.</param>
		public static void DrawRectangle(Graphics g, Pen pen, double x, double y, double width, double height)
		{
			DrawRectangle(g, pen, new RectangleD(x, y, width, height));
		}


		/// <summary>
		/// Draws a rectangle specified by a Rectangle structure.
		/// </summary>
		/// <param name="g">The Graphics to draw on.</param>
		/// <param name="pen">A Pen object that determines the color, width, and style of the rectangle.</param>
		/// <param name="rect">A Rectangle structure that represents the rectangle to draw.</param>
		public static void DrawRectangle(Graphics g, Pen pen, RectangleD rect)
		{
			g.DrawRectangle(pen, (float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
		}

		#endregion

		#region FillRectangle
		/// <summary>
		/// Fills the interior of a rectangle specified by a pair of coordinates, a width, and a height.
		/// </summary>
		/// <param name="g">The Graphics to draw on.</param>
		/// <param name="color">Color to fill.</param>
		/// <param name="x">x-coordinate of the upper-left corner of the rectangle to fill.</param>
		/// <param name="y">y-coordinate of the upper-left corner of the rectangle to fill.</param>
		/// <param name="width">Width of the rectangle to fill.</param>
		/// <param name="height">Height of the rectangle to fill.</param>
		public static void FillRectangle(Graphics g, Color color, double x, double y, double width, double height)
		{
			FillRectangle(g, color, new RectangleD(x, y, width, height));
		}

		
		/// <summary>
		/// Fills the interior of a rectangle specified by a RectangleD structure.
		/// </summary>
		/// <param name="g">The Graphics to draw on.</param>
		/// <param name="color">Color to fill.</param>
		/// <param name="rect">RectangleD structure that represents the rectangle to fill.</param>
		public static void FillRectangle(Graphics g, Color color, RectangleD rect)
		{
			if (color != Color.Transparent)
				using (Brush brush = new SolidBrush(color))FillRectangle(g, brush, rect);
		}

		
		/// <summary>
		/// Fills the interior of a rectangle specified by a pair of coordinates, a width, and a height.
		/// </summary>
		/// <param name="g">The Graphics to draw on.</param>
		/// <param name="brush">StiBrush object that determines the characteristics of the fill.</param>
		/// <param name="x">x-coordinate of the upper-left corner of the rectangle to fill.</param>
		/// <param name="y">y-coordinate of the upper-left corner of the rectangle to fill.</param>
		/// <param name="width">Width of the rectangle to fill.</param>
		/// <param name="height">Height of the rectangle to fill.</param>
		public static void FillRectangle(Graphics g, StiBrush brush, double x, double y, double width, double height)
		{
			FillRectangle(g, brush, new RectangleD(x, y, width, height));
		}

		
		/// <summary>
		/// Fills the interior of a rectangle specified by a RectangleD structure.
		/// </summary>
		/// <param name="g">The Graphics to draw on.</param>
		/// <param name="brush">StiBrush object that determines the characteristics of the fill.</param>
		/// <param name="rect">RectangleD structure that represents the rectangle to fill.</param>
		public static void FillRectangle(Graphics g, StiBrush brush, RectangleD rect)
		{
			using (Brush br = StiBrush.GetBrush(brush, rect))
			{
				FillRectangle(g, br, rect);
			}
		}


		/// <summary>
		/// Fills the interior of a rectangle specified by a RectangleF structure.
		/// </summary>
		/// <param name="g">The Graphics to draw on.</param>
		/// <param name="brush">StiBrush object that determines the characteristics of the fill.</param>
		/// <param name="rect">RectangleD structure that represents the rectangle to fill.</param>
		public static void FillRectangle(Graphics g, StiBrush brush, RectangleF rect)
		{
			using (Brush br = StiBrush.GetBrush(brush, rect))
			{
				FillRectangle(g, br, rect);
			}
		}

		
		/// <summary>
		/// Fills the interior of a rectangle specified by a pair of coordinates, a width, and a height.
		/// </summary>
		/// <param name="g">The Graphics to draw on.</param>
		/// <param name="brush">Brush object that determines the characteristics of the fill.</param>
		/// <param name="x">x-coordinate of the upper-left corner of the rectangle to fill.</param>
		/// <param name="y">y-coordinate of the upper-left corner of the rectangle to fill.</param>
		/// <param name="width">Width of the rectangle to fill.</param>
		/// <param name="height">Height of the rectangle to fill.</param>
		public static void FillRectangle(Graphics g, Brush brush, double x, double y, double width, double height)
		{
			FillRectangle(g, brush, new RectangleD(x, y, width, height));
		}

		
		/// <summary>
		/// Fills the interior of a rectangle specified by a RectangleD structure.
		/// </summary>
		/// <param name="g">The Graphics to draw on.</param>
		/// <param name="brush">Brush object that determines the characteristics of the fill.</param>
		/// <param name="rect">RectangleD structure that represents the rectangle to fill.</param>
		public static void FillRectangle(Graphics g, Brush brush, RectangleD rect)
		{
			if (brush is SolidBrush)
			{
				if (((SolidBrush)brush).Color == Color.Transparent)return;
			}
			
			g.FillRectangle(brush, rect.ToRectangleF());
		}


		/// <summary>
		/// Fills the interior of a rectangle specified by a RectangleF structure.
		/// </summary>
		/// <param name="g">The Graphics to draw on.</param>
		/// <param name="brush">Brush object that determines the characteristics of the fill.</param>
		/// <param name="rect">RectangleD structure that represents the rectangle to fill.</param>
		public static void FillRectangle(Graphics g, Brush brush, RectangleF rect)
		{
			if (brush is SolidBrush)
			{
				if (((SolidBrush)brush).Color == Color.Transparent)return;
			}
			
			g.FillRectangle(brush, rect);
		}


		#endregion

		#region PaintLine
		public static void DrawLine(Graphics g, Pen pen, double x1, double y1, double x2, double y2)
		{
			g.DrawLine(pen, (float)x1, (float)y1, (float)x2, (float)y2);
		}
		#endregion

		/// <summary>
		/// Draws a selected point specified by a coordinate pair.
		/// </summary>
		/// <param name="g">The Graphics to draw on.</param>
		/// <param name="size">The size of selected point.</param>
		/// <param name="brush">Brush to draw selected point.</param>
		/// <param name="x">x-coordinate of selected point.</param>
		/// <param name="y">y-coordinate of selected point</param>
		public static void DrawSelectedPoint(Graphics g, float size, Brush brush, double x, double y)
		{
			FillRectangle(g, brush, x - size, y - size, size * 2 + 1, size * 2 + 1);
		}


		/// <summary>
		/// Draws a selected rectangle specified by a Rectangle structure.
		/// </summary>
		/// <param name="g">The Graphics to draw on.</param>
		/// <param name="size">The size of selected point.</param>
		/// <param name="brush">Brush to draw selected point.</param>
		/// <param name="rect">RectangleD structure that represents the rectangle to draw selections.</param>
		public static void DrawSelectedRectangle(Graphics g, int size, Brush brush, RectangleD rect)
		{
			DrawSelectedPoint(g, size, brush, rect.Left, rect.Top);
			DrawSelectedPoint(g, size, brush, rect.Right, rect.Top);
			DrawSelectedPoint(g, size, brush, rect.Left, rect.Bottom);
			DrawSelectedPoint(g, size, brush, rect.Right, rect.Bottom);

			DrawSelectedPoint(g, size, brush, rect.Left + rect.Width / 2, rect.Bottom);
			DrawSelectedPoint(g, size, brush, rect.Left, rect.Top + rect.Height / 2);
			DrawSelectedPoint(g, size, brush, rect.Left + rect.Width / 2, rect.Top);
			DrawSelectedPoint(g, size, brush, rect.Right, rect.Top + rect.Height / 2);
		}		
	}
}
