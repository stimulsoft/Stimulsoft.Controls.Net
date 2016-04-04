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
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Provides methods used to paint controls and their elements.
	/// </summary>
	public sealed class StiControlPaint
	{
		#region Methods
		public static void DrawImageBackground(Graphics g, Image image, Rectangle rect)
		{
			Rectangle brushRect = rect;
			g.TranslateTransform(rect.X, rect.Y);
				
			brushRect.X = 0;
			brushRect.Y = 0;
			using (TextureBrush brush = new TextureBrush(image))
			{
				g.FillRectangle(brush, brushRect);
			}
			g.TranslateTransform(-rect.X, -rect.Y);
		}
		
				
		/// <summary>
		/// Draws the specified text string.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="text">String to draw.</param>
		/// <param name="font">Font object that defines the text format of the string.</param>
		/// <param name="brush">Brush object that determines the color and texture of the drawn text.</param>
		/// <param name="layoutRectangle">The RectangleF structure that specifies the location of the drawn text.</param>
		/// <param name="format">The StringFormat object that specifies formatting attributes, such as line spacing and alignment, that are applied to the drawn text.</param>
		/// <param name="angle">An angle of the text rotation.</param>
		public static void DrawString(Graphics graphics, string text, Font font, Brush brush, 
			Rectangle layoutRectangle, StringFormat format, float angle)
		{
			if (angle != 0)
			{
				Region svClip = graphics.Clip;
				graphics.SetClip(layoutRectangle, CombineMode.Intersect);
				GraphicsState gs = graphics.Save();
				graphics.TranslateTransform((float)(layoutRectangle.Left + layoutRectangle.Width / 2), 
					(float)(layoutRectangle.Top + layoutRectangle.Height / 2));
				graphics.RotateTransform((float)angle);
				layoutRectangle.X =  - layoutRectangle.Width / 2;
				layoutRectangle.Y =  - layoutRectangle.Height / 2;
				
				Rectangle drawRect = new Rectangle(layoutRectangle.X, layoutRectangle.Y, 
					layoutRectangle.Width, layoutRectangle.Height);
				
				if ((angle > 45 && angle < 135) || (angle > 225 && angle < 315))
					drawRect = new Rectangle(layoutRectangle.Y, layoutRectangle.X, 
						layoutRectangle.Height, layoutRectangle.Width);

				graphics.DrawString(text, font, brush, drawRect, format);
				graphics.Restore(gs);
				graphics.SetClip(svClip, CombineMode.Replace);
			}
			else graphics.DrawString(text, font, brush, layoutRectangle, format);
		}


		/// <summary>
		/// Draws a reversible frame on the screen within the specified bounds,
		/// with the specified background color, and in the specified state.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="x">The X coordinate of the left edge of the frame, in screen coordinates.</param>
		/// <param name="y">The Y coordinate of the top edge of the frame, in screen coordinates.</param>
		/// <param name="width">The width of this frame.</param>
		/// <param name="height">The height of this frame.</param>
		/// <param name="backColor">The Color of the background behind the frame.</param>
		/// <param name="style">The FrameStyle of the line.</param>
		public static void DrawReversibleFrame(Graphics graphics, int x, int y, int width, int height, 
			Color backColor, FrameStyle style)
		{
			DrawReversibleFrame(graphics, new Rectangle(x, y, width, height), backColor, style);
		}


		/// <summary>
		/// Draws a reversible frame on the screen within the specified bounds,
		/// with the specified background color, and in the specified state.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="rectangle">The Rectangle that represents the dimensions of the rectangle to draw, in screen coordinates.</param>
		/// <param name="backColor">The Color of the background behind the frame.</param>
		/// <param name="style">The FrameStyle of the line.</param>
		public static void DrawReversibleFrame(Graphics graphics, Rectangle rectangle, 
			Color backColor, FrameStyle style)
		{
			Win32.DrawMode drawMode;
			Color color;
			if (((double) backColor.GetBrightness()) < 0.5f)
			{
				drawMode = Win32.DrawMode.R2_NOTXORPEN;
				color = Color.White;
			}
			else
			{
				drawMode = Win32.DrawMode.R2_XORPEN;
				color = Color.Black;
			}
			IntPtr ptrPen = IntPtr.Zero;
			switch(style)
			{
				case FrameStyle.Dashed:
					ptrPen = Win32.CreatePen((int)Win32.PenStyle.PS_DOT, 1, ColorTranslator.ToWin32(backColor)); 
					break;
				
				case FrameStyle.Thick:
					ptrPen = Win32.CreatePen((int)Win32.PenStyle.PS_SOLID, 1, ColorTranslator.ToWin32(backColor));
					break;
			}

			IntPtr ptrGraphics = graphics.GetHdc();
			int oldDrawMode = Win32.SetROP2(ptrGraphics, (int)drawMode);
			IntPtr ptrOldBrush = Win32.SelectObject(ptrGraphics, Win32.GetStockObject((int)Win32.StockObject.NULL_BRUSH));
			IntPtr ptrOldPen = Win32.SelectObject(ptrGraphics, ptrPen);
			Win32.SetBkColor(ptrGraphics, ColorTranslator.ToWin32(color));
			Win32.Rectangle(ptrGraphics, rectangle.X, rectangle.Y, rectangle.Right, rectangle.Bottom);
			Win32.SetROP2(ptrGraphics, oldDrawMode);
			Win32.SelectObject(ptrGraphics, ptrOldBrush);
			Win32.SelectObject(ptrGraphics, ptrOldPen);
			Win32.DeleteObject(ptrPen);
			graphics.ReleaseHdc(ptrGraphics);		
            
        } 


		/// <summary>
		/// Draws a reversible line on the screen within the specified starting and ending points 
		/// and with the specified background color.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="startX">The starting X coordinate of the line, in screen coordinates.</param>
		/// <param name="startY">The starting Y coordinate of the line, in screen coordinates.</param>
		/// <param name="endX">The ending X coordinate of the line, in screen coordinates.</param>
		/// <param name="endY">The ending Y coordinate of the line, in screen coordinates.</param>
		/// <param name="backColor">The Color of the background behind the line.</param>
		/// <param name="style">The FrameStyle of the line.</param>
		public static void DrawReversibleLine(Graphics graphics, int startX, int startY, int endX, int endY, 
			Color backColor, FrameStyle style)
		{
			DrawReversibleLine(graphics, new Point(startX, startY), new Point(endX, endY), backColor, style);
		}


		/// <summary>
		/// Draws a reversible line on the screen within the specified starting and ending points 
		/// and with the specified background color.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="start">The starting Point of the line, in screen coordinates.</param>
		/// <param name="end">The ending Point of the line, in screen coordinates.</param>
		/// <param name="backColor">The Color of the background behind the line.</param>
		/// <param name="style">The FrameStyle of the line.</param>
		public static void DrawReversibleLine(Graphics graphics, Point start, Point end, 
			Color backColor, FrameStyle style)
		{
			Win32.DrawMode drawMode;
			Color color;
			if (((double) backColor.GetBrightness()) < 0.5f)
			{
				drawMode = Win32.DrawMode.R2_NOTXORPEN;
				color = Color.White;
			}
			else
			{
				drawMode = Win32.DrawMode.R2_XORPEN;
				color = Color.Black;
			}
			IntPtr ptrPen = IntPtr.Zero;
			switch(style)
			{
				case FrameStyle.Dashed:
					ptrPen = Win32.CreatePen((int)Win32.PenStyle.PS_DOT, 1, ColorTranslator.ToWin32(backColor)); 
					break;
				
				case FrameStyle.Thick:
					ptrPen = Win32.CreatePen((int)Win32.PenStyle.PS_SOLID, 1, ColorTranslator.ToWin32(backColor));
					break;
			}

			IntPtr ptrGraphics = graphics.GetHdc();
			int oldDrawMode = Win32.SetROP2(ptrGraphics, (int)drawMode);
			IntPtr ptrOldBrush = Win32.SelectObject(ptrGraphics, Win32.GetStockObject((int)Win32.StockObject.NULL_BRUSH));
			IntPtr ptrOldPen = Win32.SelectObject(ptrGraphics, ptrPen);
			Win32.SetBkColor(ptrGraphics, ColorTranslator.ToWin32(color));
			Win32.MoveToEx(ptrGraphics, start.X, start.Y, IntPtr.Zero);
			Win32.LineTo(ptrGraphics, end.X, end.Y);

			Win32.SetROP2(ptrGraphics, oldDrawMode);
			Win32.SelectObject(ptrGraphics, ptrOldBrush);
			Win32.SelectObject(ptrGraphics, ptrOldPen);
			Win32.DeleteObject(ptrPen);
			graphics.ReleaseHdc(ptrGraphics);	
		}


		/// <summary>
		/// Draws a filled, reversible rectangle on the screen.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="x">The X coordinate of the left edge of the rectangle, in screen coordinates.</param>
		/// <param name="y">The Y coordinate of the left edge of the rectangle, in screen coordinates.</param>
		/// <param name="width">The width of this rectangle.</param>
		/// <param name="height">The height of this rectangle.</param>
		/// <param name="backColor">The Color of the background behind the fill.</param>
		public static void FillReversibleRectangle(Graphics graphics, int x, int y, 
			int width, int height, Color backColor)
		{
			FillReversibleRectangle(graphics, new Rectangle(x, y, width, height), backColor);
		}


		/// <summary>
		/// Draws a filled, reversible rectangle on the screen.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="bounds">The Rectangle that represents the dimensions of the rectangle to fill, in screen coordinates.</param>
		/// <param name="backColor">The Color of the background behind the fill.</param>
		public static void FillReversibleRectangle(Graphics graphics, Rectangle bounds, Color backColor)
		{
			int color1 = StiColorUtils.GetColorRop(backColor, 10813541, 5898313);
			int color2 = StiColorUtils.GetColorRop(backColor, 6, 6);
			IntPtr ptrGraphics = graphics.GetHdc();
			IntPtr ptrBrush = Win32.CreateSolidBrush(ColorTranslator.ToWin32(backColor));
			int oldColor = Win32.SetROP2(ptrGraphics, color2);
			IntPtr ptrOldBrush = Win32.SelectObject(ptrGraphics, ptrBrush);
			Win32.PatBlt(ptrGraphics, bounds.X, bounds.Y, bounds.Width, bounds.Height, color1);
			Win32.SetROP2(ptrGraphics, oldColor);
			Win32.SelectObject(ptrGraphics, ptrOldBrush);
			Win32.DeleteObject(ptrBrush);
			graphics.ReleaseHdc(ptrGraphics);
		}
 

		/// <summary>
		/// Draws the specified image in a disabled state.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="image">The Image to draw.</param>
		/// <param name="x">The X coordinate of the top left of the border image.</param>
		/// <param name="y">The Y coordinate of the top left of the border image.</param>
		public static void DrawImageDisabled(Graphics graphics, Image image, int x, int y)
		{
			if (image is Bitmap)DrawImageDisabled(graphics, image as Bitmap, x, y);
			else ControlPaint.DrawImageDisabled(graphics, image, x, y, SystemColors.Control);
		}


		/// <summary>
		/// Draws the specified image in a disabled state.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="bmp">The Bitmap to draw.</param>
		/// <param name="x">The X coordinate of the top left of the border bitmap.</param>
		/// <param name="y">The Y coordinate of the top left of the border bitmap.</param>
		public static void DrawImageDisabled(Graphics graphics, Bitmap bmp, int x, int y)
		{
			ImageAttributes imageAttr = new ImageAttributes();
        
			ColorMatrix disableMatrix = new ColorMatrix(
				new float[][]{
								 new float[]{0.3f,0.3f,0.3f,0,0},
								 new float[]{0.59f,0.59f,0.59f,0,0},
								 new float[]{0.11f,0.11f,0.11f,0,0},
								 new float[]{0,0,0,0.4f,0,0},
								 new float[]{0,0,0,0,0.4f,0},
								 new float[]{0,0,0,0,0,0.4f}
							 });

			imageAttr.SetColorMatrix(disableMatrix);

			graphics.DrawImage(bmp, new Rectangle(x, y, bmp.Width, bmp.Height), 
				0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttr);
		}


		/// <summary>
		/// Draws a button control.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="bounds">The bounds that represents the dimensions of the button.</param>
		/// <param name="image">Image for draws on button.</param>
		/// <param name="isPressed">The Button is pressed.</param>
		/// <param name="isFocused">The Button is focused.</param>
		/// <param name="isMouseOverButton">Mouse pointer is over the button.</param>
		/// <param name="enabled">The Button is enabled.</param>
		public static void DrawButton(Graphics graphics, Rectangle bounds, Image image, 
			bool isPressed, bool isFocused, bool isMouseOverButton, bool enabled, bool flat)
		{
			#region Flat
			if (flat)
			{
				bounds.Width ++;
				bounds.Height ++;

				Color btColorStart = StiColors.ControlStart;
				Color btColorEnd = StiColors.ControlEnd;
			
				#region isMouseOverButton
				if (isMouseOverButton)
				{
					btColorStart = StiColors.ControlStartLight;
					btColorEnd = StiColors.ControlEndLight;
				}
				#endregion

				#region isPressed
				if (isPressed)
				{
					btColorStart = StiColors.ControlStartDark;
					btColorEnd = StiColors.ControlEndDark;
				}
				#endregion

				if (!enabled)btColorStart = btColorEnd;

				using (Brush brush = new LinearGradientBrush(bounds, btColorStart, btColorEnd, 90))
				{
					graphics.FillRectangle(brush, bounds);
				}

				Color color = SystemColors.ControlDark;

				if (isFocused)color = StiColors.SelectedText;

				using (Pen pen = new Pen(color))
				{
					bounds.X --;
					bounds.Y --;
					bounds.Width ++;
					bounds.Height ++;
					graphics.DrawRectangle(pen, bounds);
				}

			}
			#endregion

			#region 3D
			else
			{
				ButtonState state = ButtonState.Normal;
				if (isPressed)state = ButtonState.Pushed;
				if (!enabled)state = ButtonState.Inactive;

				bounds.Width ++;
				bounds.Height ++;

				if (bounds.Width > 0 && bounds.Height > 0)
				ControlPaint.DrawButton(graphics, bounds, state);
				
			}
			#endregion

			#region PaintImage
			if (isPressed)
			{
				bounds.X++;
				bounds.Y++;
			}

			if (image != null)
			{
				if (flat)
				{
					bounds.X ++;
					bounds.Y ++;
				}
				if (enabled)
				{
					graphics.DrawImage(image, new Rectangle(
						bounds.X + (bounds.Width - image.Width) / 2,
						bounds.Y + (bounds.Height - image.Height - 1) / 2, 
						16, 16));
				}
				else 
				{
					StiControlPaint.DrawImageDisabled(graphics, image, 
						bounds.X + (bounds.Width - image.Width) / 2,
						bounds.Y + (bounds.Height - image.Height - 1) / 2);
				}
			}
			#endregion
		}


		/// <summary>
		/// Draws a border.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="bounds">The bounds that represents the dimensions of the border rectangle.</param>
		/// <param name="isFocused">The Border is focused.</param>
		public static void DrawBorder(Graphics graphics, Rectangle bounds, bool isFocused, bool flat)
		{
			if (flat)
			{
				Color color = SystemColors.ControlDark;
				if (isFocused)color = StiColors.SelectedText;
			
				using (Pen pen = new Pen(color))
				{
					graphics.DrawRectangle(pen, bounds.X, bounds.Y,
						bounds.Width, bounds.Height);
				}
			}
			else 
			{
				bounds.Width ++;
				bounds.Height ++;
				ControlPaint.DrawBorder3D(graphics, bounds, Border3DStyle.Sunken);
			}
		}


	
		/// <summary>
		/// Draws a list item.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="bounds">The bounds that represents the dimensions of the item rectangle.</param>
		/// <param name="state">Specifies the state of an item that is being drawn.</param>
		public static void DrawItem(Graphics graphics, Rectangle bounds, DrawItemState state, Color backColor, Color foreColor)
		{
			if ((state & DrawItemState.Focus) != 0)
			{
				graphics.FillRectangle(StiBrushes.Focus, 
					bounds.X, bounds.Y, bounds.Width + 1, bounds.Height);
					
				graphics.DrawRectangle(StiPens.SelectedText, 
					bounds.X, bounds.Y, bounds.Width, bounds.Height - 1);
				
			}
			else
				if ((state & DrawItemState.Selected) != 0)
			{
				graphics.FillRectangle(StiBrushes.Selected, 
					bounds.X, bounds.Y, bounds.Width + 1, bounds.Height);
					
				graphics.DrawRectangle(StiPens.SelectedText, 
					bounds.X, bounds.Y, bounds.Width, bounds.Height - 1);
			}
			else
			{
				using (SolidBrush brush = new SolidBrush(backColor))
				{
					graphics.FillRectangle(brush,
						bounds.X, bounds.Y, bounds.Width + 1, bounds.Height);
				}
			}
		}

        /// <summary>
		/// Draws a list item.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="bounds">The bounds that represents the dimensions of the item rectangle.</param>
		/// <param name="state">Specifies the state of an item that is being drawn.</param>
		/// <param name="text">The Text of the item.</param>
		/// <param name="imageList">Specifies the ImageList of an item.</param>
		/// <param name="imageIndex">Specifies the ImageIndex of an item.</param>
		/// <param name="font">The Font to draw the item. </param>
        /// <param name="foreColor">The Color to draw the string with.</param>
		/// <param name="textStartPos">The Position for draws of the text.</param>
		/// <param name="rightToLeft">Specifies that text is right to left.</param>
        public static void DrawItem(Graphics graphics, Rectangle bounds, DrawItemState state, string text,
            ImageList imageList, int imageIndex, Font font, Color backColor, Color foreColor, int textStartPos,
            RightToLeft rightToLeft)
        {
            DrawItem(graphics, bounds, state, text, imageList, imageIndex, font, backColor, foreColor,
                textStartPos, rightToLeft, StringAlignment.Near);
        }
		
		/// <summary>
		/// Draws a list item.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="bounds">The bounds that represents the dimensions of the item rectangle.</param>
		/// <param name="state">Specifies the state of an item that is being drawn.</param>
		/// <param name="text">The Text of the item.</param>
		/// <param name="imageList">Specifies the ImageList of an item.</param>
		/// <param name="imageIndex">Specifies the ImageIndex of an item.</param>
		/// <param name="font">The Font to draw the item. </param>
        /// <param name="foreColor">The Color to draw the string with.</param>
		/// <param name="textStartPos">The Position for draws of the text.</param>
		/// <param name="rightToLeft">Specifies that text is right to left.</param>
		public static void DrawItem(Graphics graphics, Rectangle bounds, DrawItemState state, string text, 
			ImageList imageList, int imageIndex, Font font, Color backColor, Color foreColor, int textStartPos,
			RightToLeft rightToLeft, StringAlignment alignment)
		{
			DrawItem(graphics, bounds, state, backColor, foreColor);
			
			#region Paint image
			int imageWidth = 0;
			
            if (imageList != null && imageIndex >= 0 && imageIndex < imageList.Images.Count)
			{
				Rectangle imageRect = new Rectangle(
					bounds.X + 1, bounds.Y + (bounds.Height - imageList.ImageSize.Height) / 2,
					imageList.ImageSize.Width, imageList.ImageSize.Height);

				imageList.Draw(graphics, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height, imageIndex);

				imageWidth = imageList.ImageSize.Width + 2;
			}
			#endregion
				
			#region Paint text
			using (StringFormat sf = new StringFormat())
			{
                sf.Alignment = alignment;
				sf.LineAlignment = StringAlignment.Center;
				sf.FormatFlags = StringFormatFlags.NoWrap;
				sf.Trimming = StringTrimming.EllipsisCharacter;

				if (rightToLeft == RightToLeft.Yes)
					sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
						
				bounds.X += imageWidth + textStartPos;
				bounds.Width -= imageWidth + textStartPos;

				if (text != null)
					using (SolidBrush brush = new SolidBrush(foreColor))
						graphics.DrawString(text, font, brush, bounds, sf);
			}
			#endregion
		}


		/// <summary>
		/// Draws a list item.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="bounds">The bounds that represents the dimensions of the item rectangle.</param>
		/// <param name="state">Specifies the state of an item that is being drawn.</param>
		/// <param name="text">The Text of the item.</param>
		/// <param name="imageList">Specifies the ImageList of an item.</param>
		/// <param name="imageIndex">Specifies the ImageIndex of an item.</param>
		/// <param name="font">The Font to draw the item.</param>
        /// <param name="foreColor">The Color to draw the string with.</param>
		/// <param name="rightToLeft">Specifies that text is right to left.</param>
		public static void DrawItem(Graphics graphics, Rectangle bounds, DrawItemState state, string text, 
			ImageList imageList, int imageIndex, Font font, Color backColor, Color foreColor, RightToLeft rightToLeft)
		{
			DrawItem(graphics, bounds, state, text, imageList, imageIndex, font, backColor, foreColor, 0, rightToLeft);
		}


		/// <summary>
		/// Draws a check.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="x">The X coordinate of the top left of the check.</param>
		/// <param name="y">The Y coordinate of the top left of the check.</param>
		public static void DrawCheck(Graphics graphics, int x, int y, bool enabled)
		{
			x -= 3;
			y -= 3;
			Point p1 = new Point(x, y + 2);
			Point p2 = new Point(x + 2, y + 4);
			Point p3 = new Point(x + 6, y);

			Color color = Color.Black;
			if (!enabled)color = SystemColors.ControlDark;
			
			using (Pen pen = new Pen(color))
			{
				graphics.DrawLine(pen, p1, p2);
				graphics.DrawLine(pen, p2, p3);

				p1.Y++;
				p2.Y++;
				p3.Y++;

				graphics.DrawLine(pen, p1, p2);
				graphics.DrawLine(pen, p2, p3);

				p1.Y++;
				p2.Y++;
				p3.Y++;

				graphics.DrawLine(pen, p1, p2);
				graphics.DrawLine(pen, p2, p3);
			}
		}


		/// <summary>
		/// Draws a focus rectangle with button.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="bounds">The bounds that represents the dimensions of the focus rectangle.</param>
		/// <param name="buttonBounds">The bounds that represents the dimensions of the button rectangle.</param>
		public static void DrawFocus(Graphics graphics, Rectangle bounds, Rectangle buttonBounds)
		{
			Rectangle focusedRect = new Rectangle(
				bounds.X + 2, bounds.Y + 2, 
				bounds.Width - buttonBounds.Width - 4, bounds.Height - 3);

			ControlPaint.DrawFocusRectangle(graphics, focusedRect);
		}


		/// <summary>
		/// Draws a focus rectangle.
		/// </summary>
		/// <param name="graphics">The Graphics object to draw on.</param>
		/// <param name="bounds">The bounds that represents the dimensions of the focus rectangle.</param>
		public static void DrawFocus(Graphics graphics, Rectangle bounds)
		{
			Rectangle focusedRect = new Rectangle(
				bounds.X + 2, bounds.Y + 2, 
				bounds.Width - 3, bounds.Height - 3);

			ControlPaint.DrawFocusRectangle(graphics, focusedRect);
		}
		

		/// <summary>
		/// Scrolls the contents of the specified window's client area.
		/// </summary>
		/// <param name="hWnd">Handle to the window where the client area is to be scrolled.</param>
		/// <param name="xAmount">Specifies the amount, in device units, of horizontal scrolling.</param>
		/// <param name="yAmount">Specifies the amount, in device units, of vertical scrolling.</param>
		/// <returns>If the function succeeds, the return value is true.</returns>
		public static bool ScrollWindow(IntPtr hWnd, int xAmount, int yAmount)
		{
			return Win32.ScrollWindowEx(hWnd, xAmount, yAmount, IntPtr.Zero, IntPtr.Zero, 
				IntPtr.Zero, IntPtr.Zero, 10);
		}

		
		
		public static Rectangle GetButtonRect(Rectangle bounds, bool flat, RightToLeft rightToLeft)
		{
			return GetButtonRect(bounds, flat, SystemInformation.HorizontalScrollBarArrowWidth - 1, rightToLeft);
		}

		
		public static Rectangle GetButtonRect(Rectangle bounds, bool flat, int buttonWidth, RightToLeft rightToLeft)
		{
			int borderWidth = (int)SystemInformation.Border3DSize.Width;
			int borderHeight = (int)SystemInformation.Border3DSize.Height;
			if (flat)borderWidth = borderHeight = 1;

			if (rightToLeft == RightToLeft.Yes)
			{
				return new Rectangle(
					bounds.Left + borderWidth, 
					bounds.Top + borderHeight,
					buttonWidth + 1, 
					bounds.Height - borderHeight * 2);
			}
			else
			{
				return new Rectangle(
					bounds.Right - buttonWidth - borderWidth, 
					bounds.Top + borderHeight,
					buttonWidth, 
					bounds.Height - borderHeight * 2);
			}
		}

		
		public static Rectangle GetContentRect(Rectangle bounds, bool flat, RightToLeft rightToLeft)
		{
			Rectangle buttonRect = GetButtonRect(bounds, flat, rightToLeft);

			int borderWidth = (int)SystemInformation.Border3DSize.Width;
			int borderHeight = (int)SystemInformation.Border3DSize.Height;
			if (flat)borderWidth = borderHeight = 1;

			if (rightToLeft == RightToLeft.Yes)
			{
				return new Rectangle(bounds.Left + borderWidth + buttonRect.Width, bounds.Top + borderHeight, 
					bounds.Width - borderWidth * 2, bounds.Height - borderHeight * 2);
			}
			else 
			{
				return new Rectangle(bounds.Left + borderWidth, bounds.Top + borderHeight, 
					bounds.Width - borderWidth * 2 - buttonRect.Width, bounds.Height - borderHeight * 2);
			}
		}

		#endregion
		
		#region Constructors
		private StiControlPaint()
		{
		}
		#endregion
	}
}
