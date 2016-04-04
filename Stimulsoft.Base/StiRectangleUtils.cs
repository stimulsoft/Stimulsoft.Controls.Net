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

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Helps works with Rectangle.
	/// </summary>
	sealed public class StiRectangleUtils
	{
		/// <summary>
		/// Normalizes (converts all negative importances).
		/// </summary>
		/// <param name="rect">Rectangle to normalize.</param>
		/// <returns>Normalized rectangle.</returns>
		public static Rectangle Normalize(Rectangle rect)
		{
			if (rect.Width < 0)
			{
				rect.X += rect.Width;
				rect.Width = - rect.Width;
			}

			if (rect.Height < 0)
			{
				rect.Y += rect.Height;
				rect.Height = - rect.Height;
			}
			return rect;
		}

		/// <summary>
		/// Fits rectangles rect1 in rect2.
		/// </summary>
		/// <param name="rect1">Rectangle rect1.</param>
		/// <param name="rect2">Rectangle rect2.</param>
		/// <returns>Resulted Rectangle.</returns>
		public static Rectangle FitToRectangle(Rectangle rect1, Rectangle rect2)
		{
			if (rect1.IsEmpty)return rect2;
			if (rect2.IsEmpty)return rect1;

			Rectangle rectangle2 = rect1;
			if (rectangle2.Left > rect2.Left)
			{
				rectangle2.Width += rectangle2.Left - rect2.Left;
				rectangle2.X = rect2.Left;
			}
			if (rectangle2.Top > rect2.Top)
			{
				rectangle2.Height += rectangle2.Top - rect2.Top;
				rectangle2.Y = rect2.Top;
			}
			if (rectangle2.Right < rect2.Right)
			{
				rectangle2.Width += rect2.Right - rectangle2.Right;
			}
			
			if (rectangle2.Bottom < rect2.Bottom)
			{
				rectangle2.Height += rect2.Bottom - rectangle2.Bottom;
			}
			
			return rectangle2;
		}

		
		/// <summary>
		/// Converts Rectangle to RectangleF.
		/// </summary>
		public static RectangleF Convert(Rectangle rect)
		{
			return new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
		}


		public static RectangleD AlignSizeInRect(RectangleD rect, SizeD size, ContentAlignment alignment)
		{
			double x = 0;
			double y = 0;

			if (alignment == ContentAlignment.BottomLeft || 
				alignment == ContentAlignment.MiddleLeft ||
				alignment == ContentAlignment.TopLeft)
			{
				x = rect.X;
			}

			if (alignment == ContentAlignment.BottomCenter || 
				alignment == ContentAlignment.MiddleCenter ||
				alignment == ContentAlignment.TopCenter)
			{
				x = rect.X + (rect.Width - size.Width) / 2;
			}

			if (alignment == ContentAlignment.BottomRight || 
				alignment == ContentAlignment.MiddleRight ||
				alignment == ContentAlignment.TopRight)
			{
				x = rect.Right - size.Width;
			}

			if (alignment == ContentAlignment.TopLeft || 
				alignment == ContentAlignment.TopCenter ||
				alignment == ContentAlignment.TopRight)
			{
				y = rect.Y;
			}

			if (alignment == ContentAlignment.MiddleLeft || 
				alignment == ContentAlignment.MiddleCenter ||
				alignment == ContentAlignment.MiddleRight)
			{
				y = rect.Y + (rect.Height - size.Height) / 2;
			}

			if (alignment == ContentAlignment.BottomLeft || 
				alignment == ContentAlignment.BottomCenter ||
				alignment == ContentAlignment.BottomRight)
			{
				y = rect.Bottom - size.Height;
			}

			return new RectangleD(x, y, size.Width, size.Height);
		}

		public static RectangleD AlignSizeInRect(RectangleD rect, SizeD size, StiHorAlignment horAlignment, StiVertAlignment vertAlignment)
		{
			double x = 0;
			double y = 0;

			if (horAlignment == StiHorAlignment.Left)x = rect.X;
			if (horAlignment == StiHorAlignment.Center)x = rect.X + (rect.Width - size.Width) / 2;
			if (horAlignment == StiHorAlignment.Right)x = rect.Right - size.Width;
			
			if (vertAlignment == StiVertAlignment.Top)y = rect.Y;
			if (vertAlignment == StiVertAlignment.Center)y = rect.Y + (rect.Height - size.Height) / 2;
			if (vertAlignment == StiVertAlignment.Bottom)y = rect.Bottom - size.Height;

			return new RectangleD(x, y, size.Width, size.Height);
		}
	}
}
