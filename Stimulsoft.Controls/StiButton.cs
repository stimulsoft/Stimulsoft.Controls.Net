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
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Push button control.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiButton), "Toolbox.StiButton.bmp")]
	public class StiButton : Button
	{
		#region Browsable(false)
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new FlatStyle FlatStyle
		{
			get
			{
				return base.FlatStyle;
			}
			set
			{
				base.FlatStyle = value;
			}
		}


		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}
		#endregion

		#region Fields
		protected bool isMouseOver = false;
		protected bool isPressed = false;
		#endregion

		#region Methods
		public static void DrawButton(Graphics g, Rectangle rect, string text, Font font, 
			Color foreColor, Color backColor,
			ImageList imageList, int imageIndex, Image image, RightToLeft rightToLeft)
		{
			DrawButton(g, rect, text, font, foreColor, backColor, imageList, imageIndex, image, rightToLeft, true, 
				true, false, false, false, false, ContentAlignment.MiddleCenter, ContentAlignment.MiddleCenter);
		}


		public static void DrawButton(Graphics g, Rectangle rect, string text, Font font, 
			Color foreColor, Color backColor,
			ImageList imageList, int imageIndex, Image image, RightToLeft rightToLeft, bool wordWrap,
			bool isEnabled, bool isMouseOver, bool isPressed,
			bool isDefault, bool isFocused, ContentAlignment imageAlign, ContentAlignment textAlign)
		{
			Rectangle btRect = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
			
			Color controlStart =		StiColorUtils.Light(backColor, 30);
			Color controlEnd =			StiColorUtils.Dark(backColor, 10);

			Color controlStartLight =	StiColorUtils.Light(controlStart, 20);
			Color controlEndLight =		StiColorUtils.Light(controlEnd, 20);

			Color controlStartDark =	StiColorUtils.Dark(controlStart, 20);
			Color controlEndDark =		StiColorUtils.Dark(controlEnd, 20);

			Color clBorderStart =		StiColorUtils.Dark(controlStart, 20);
			Color clBorderEnd =			StiColorUtils.Dark(controlEnd, 20);

			Color clStart = controlStart;
			Color clEnd = controlEnd;

			if (isMouseOver)
			{
				clStart = controlStartLight;
				clEnd = controlEndLight;
			}
			
			if (isPressed)
			{
				clStart = controlStartDark;
				clEnd = controlEndDark;
			}			
            
			var grRect2 = new Rectangle(btRect.X + 1, btRect.Y + 1, btRect.Width - 1, btRect.Height - 1);
			using (var brush = new LinearGradientBrush(grRect2, clBorderStart, clBorderEnd, 90f))
			{
				g.FillRectangle(brush, grRect2);
			}

			var grRect = new Rectangle(btRect.X + 1, btRect.Y + 1, btRect.Width - 2, btRect.Height - 2);
			using (var brush = new LinearGradientBrush(grRect, clStart, clEnd, 90f))
			{
				g.FillRectangle(brush, grRect);
			}
			
			
			var oldSmoothingMode = g.SmoothingMode;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			
			#region Paint border
			using (var pen = new Pen(StiColorUtils.Dark(SystemColors.ControlDark, 30)))
			{
				DrawRoundedRectangle(g, pen, btRect, new Size(4, 4));
				if (isDefault)
				{
					g.DrawRectangle(pen, new Rectangle(btRect.X + 1, btRect.Y + 1, btRect.Width -2 , btRect.Height -2));
				}
			}
			#endregion

			g.SmoothingMode = oldSmoothingMode;
			
			if (isFocused)
			{
				var focusRect = new Rectangle(rect.X + 4, rect.Y + 4, rect.Width - 7, rect.Height - 7);
				ControlPaint.DrawFocusRectangle(g, focusRect, SystemColors.ControlDark, SystemColors.Control);
			}
			
			Rectangle imageRect;
			Rectangle textRect;
			
			CalcRectangles(out textRect, out imageRect, 
				new Rectangle(rect.X + 4, rect.Y + 4, rect.Width - 8, rect.Height - 8), 
				g, isEnabled, imageList, imageIndex, image, text, 
				wordWrap, rightToLeft, foreColor, backColor,
				font, imageAlign, textAlign, isPressed);

			try
			{
				DrawImage(g, isEnabled, imageRect, imageList, imageIndex, image);
			}
			catch
			{
			}

			DrawText(g, text, wordWrap, rightToLeft, isEnabled, foreColor, textRect, font, 
				textAlign, imageAlign);
		}


		private static void CalcRectangles(out Rectangle textRect, out Rectangle imageRect, Rectangle rect,  Graphics g, bool isEnabled, 
			ImageList imageList, int imageIndex, Image image,
			string text, bool wordWrap, RightToLeft rightToLeft, Color foreColor, Color backColor,
			Font font, ContentAlignment imageAlign, ContentAlignment textAlign, bool isPressed)
		{
			Size imageSize = Size.Empty;
			if (imageList != null && imageIndex >= 0 && imageIndex < imageList.Images.Count)
			{
				imageSize = imageList.ImageSize;
			}

			try
			{
				if (imageSize.IsEmpty && image != null)imageSize = new Size(image.Width, image.Height);
			}
			catch
			{
				imageSize = new Size(16, 16);
			}

			SizeF textSize;
			
			using (var sf = GetStringFormat(wordWrap, rightToLeft, textAlign, imageAlign))
			{
				textSize = g.MeasureString(text, font, new SizeF(rect.Width, rect.Height), sf);
			}

			textRect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
			imageRect = new Rectangle(rect.X, rect.Y, imageSize.Width, imageSize.Height);

			if (textAlign == ContentAlignment.MiddleCenter && imageAlign == ContentAlignment.MiddleCenter)
			{
				textRect.X += imageSize.Width;
				textRect.Width -= imageSize.Width;

				int pos = (int)((rect.Width - imageSize.Width - textSize.Width) / 2);
				imageRect.X = rect.X + pos;
				imageRect.Y = rect.Y + (rect.Height - imageSize.Height) / 2;
			}
			else
			{
				switch (imageAlign)
				{
					case ContentAlignment.TopLeft:
						imageRect.X = rect.X;
						imageRect.Y = rect.Y;
						break;

					case ContentAlignment.TopCenter:
						imageRect.X = rect.X + (rect.Width - imageSize.Width) / 2;
						imageRect.Y = rect.Y;
						break;

					case ContentAlignment.TopRight:
						imageRect.X = rect.Right - imageSize.Width;
						imageRect.Y = rect.Y;
						break;

					case ContentAlignment.MiddleLeft:
						imageRect.X = rect.X;
						imageRect.Y = rect.Y + (rect.Height - imageSize.Height) / 2;
						break;

					case ContentAlignment.MiddleCenter:
						imageRect.X = rect.X + (rect.Width - imageSize.Width) / 2;
						imageRect.Y = rect.Y + (rect.Height - imageSize.Height) / 2;
						break;

					case ContentAlignment.MiddleRight:
						imageRect.X = rect.Right - imageSize.Width;
						imageRect.Y = rect.Y + (rect.Height - imageSize.Height) / 2;
						break;

					case ContentAlignment.BottomLeft:
						imageRect.X = rect.X;
						imageRect.Y = rect.Bottom - imageSize.Height;
						break;

					case ContentAlignment.BottomCenter:
						imageRect.X = rect.X + (rect.Width - imageSize.Width) / 2;
						imageRect.Y = rect.Bottom - imageSize.Height;
						break;

					case ContentAlignment.BottomRight:
						imageRect.X = rect.Right - imageSize.Width;
						imageRect.Y = rect.Bottom - imageSize.Height;
						break;
				}
			}

			if (isPressed)
			{
				textRect.X ++;
				textRect.Y ++;

				imageRect.X ++;
				imageRect.Y ++;
			}
		}

	
		private static void DrawImage(Graphics g, bool isEnabled, Rectangle imageRect, ImageList imageList, int imageIndex, Image image)
		{
			if ((imageList != null && imageIndex >= 0 && imageIndex < imageList.Images.Count) || image != null)
			{
				if (imageList != null && imageIndex >= 0 && imageIndex < imageList.Images.Count)
				{
					if (isEnabled)imageList.Draw(g, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height, imageIndex);
					else StiControlPaint.DrawImageDisabled(g, imageList.Images[imageIndex], imageRect.Left, imageRect.Top);
				}
				else
				{
					if (isEnabled)g.DrawImage(image, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height);
					else StiControlPaint.DrawImageDisabled(g, image, imageRect.Left, imageRect.Top);
				}
			}
		}

	
		private static StringFormat GetStringFormat(bool wordWrap, RightToLeft rightToLeft, 
			ContentAlignment textAlign, ContentAlignment imageAlign)
		{
			var sf = new StringFormat
			{
			    Trimming = StringTrimming.EllipsisCharacter, 
                FormatFlags = 0,
                HotkeyPrefix = HotkeyPrefix.Show
			};
		    if (!wordWrap)
                sf.FormatFlags = StringFormatFlags.NoWrap;

			if (rightToLeft == RightToLeft.Yes)
				sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

			#region TextAlignment
			if (textAlign == ContentAlignment.MiddleCenter && imageAlign == ContentAlignment.MiddleCenter)
			{
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Center;
			}
			else
			{			
				switch (textAlign)
				{
					case ContentAlignment.TopLeft:
						sf.Alignment = StringAlignment.Near;
						sf.LineAlignment = StringAlignment.Near;
						break;

					case ContentAlignment.TopCenter:
						sf.Alignment = StringAlignment.Center;
						sf.LineAlignment = StringAlignment.Near;
						break;

					case ContentAlignment.TopRight:
						sf.Alignment = StringAlignment.Far;
						sf.LineAlignment = StringAlignment.Near;
						break;

					case ContentAlignment.MiddleLeft:
						sf.Alignment = StringAlignment.Near;
						sf.LineAlignment = StringAlignment.Center;
						break;

					case ContentAlignment.MiddleCenter:
						sf.Alignment = StringAlignment.Center;
						sf.LineAlignment = StringAlignment.Center;
						break;

					case ContentAlignment.MiddleRight:
						sf.Alignment = StringAlignment.Far;
						sf.LineAlignment = StringAlignment.Center;
						break;

					case ContentAlignment.BottomLeft:
						sf.Alignment = StringAlignment.Near;
						sf.LineAlignment = StringAlignment.Far;
						break;

					case ContentAlignment.BottomCenter:
						sf.Alignment = StringAlignment.Center;
						sf.LineAlignment = StringAlignment.Far;
						break;

					case ContentAlignment.BottomRight:
						sf.Alignment = StringAlignment.Far;
						sf.LineAlignment = StringAlignment.Far;
						break;
				}
			}
			#endregion

			return sf;
		}

		
		private static void DrawText(Graphics g, string text, bool wordWrap, RightToLeft rightToLeft, 
			bool isEnabled, Color foreColor, Rectangle textRect, Font font,
			ContentAlignment textAlign, ContentAlignment imageAlign)
		{
			if (!string.IsNullOrEmpty(text))
			{
				if (!isEnabled)foreColor = SystemColors.GrayText;
				using (var sf = GetStringFormat(wordWrap, rightToLeft, textAlign, imageAlign))
				{
					using (var brush = new SolidBrush(foreColor))
					{
						g.DrawString(text, font, brush, textRect, sf);
					}
				}
			}
		}


		private static void DrawRoundedRectangle(Graphics g, Pen p, Rectangle rc, Size size)
		{
			var oldSmoothingMode = g.SmoothingMode;
			g.SmoothingMode = SmoothingMode.AntiAlias;
		
			g.DrawLine(p, rc.Left  + size.Width / 2, rc.Top, rc.Right - size.Width / 2, rc.Top);
			g.DrawArc(p, rc.Right - size.Width, rc.Top, size.Width, size.Height, 270, 90);
			g.DrawLine(p, rc.Right, rc.Top + size.Height / 2, rc.Right, rc.Bottom - size.Height / 2);
			g.DrawArc(p, rc.Right - size.Width, rc.Bottom - size.Height, size.Width, size.Height, 0, 90);
			g.DrawLine(p, rc.Right - size.Width / 2, rc.Bottom, rc.Left  + size.Width / 2, rc.Bottom);
			g.DrawArc(p, rc.Left, rc.Bottom - size.Height, size.Width, size.Height, 90, 90);
			g.DrawLine(p, rc.Left, rc.Bottom - size.Height / 2,	rc.Left, rc.Top + size.Height / 2);
			g.DrawArc(p, rc.Left, rc.Top, size.Width, size.Height, 180, 90);

			g.SmoothingMode = oldSmoothingMode;
		}

		#endregion

		#region Properties
		private bool wordWrap = true;
		[Category("Behavior")]
		[DefaultValue(true)]
		public bool WordWrap
		{
			get
			{
				return wordWrap;
			}
			set
			{
				wordWrap = value;
				Invalidate();
			}
		}


		private Color buttonColor = SystemColors.Control;
		[Category("Appearance")]
		[DefaultValue(true)]
		public Color ButtonColor
		{
			get
			{
				return buttonColor;
			}
			set
			{
				buttonColor = value;
				Invalidate();
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never)]
		internal bool ShouldSerializeButtonColor()
		{
			return ButtonColor != SystemColors.Control;
		}
		#endregion

		#region Handlers
		protected override void OnPaint(PaintEventArgs p)
		{			
			Graphics g = p.Graphics;

			Rectangle rect = new Rectangle(0, 0, Width, Height);
			DrawButton(g, rect, Text, Font, ForeColor, ButtonColor, ImageList, ImageIndex, Image, 
				RightToLeft, WordWrap, Enabled, isMouseOver, isPressed, IsDefault, Focused, 
				ImageAlign, TextAlign);
		}

		protected override void OnGotFocus(System.EventArgs e)
		{
			base.OnGotFocus(e);
			Invalidate();            		
		}

		protected override void OnLostFocus(System.EventArgs e)
		{
			base.OnLostFocus(e);
			Invalidate();		
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			isMouseOver = true;
			Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			isMouseOver = false;
			Invalidate();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if ((e.Button & MouseButtons.Left) > 0)
			{
				isPressed = true;
				Invalidate();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if ((e.Button & MouseButtons.Left) > 0)
			{
				isPressed = false;
				Invalidate();
			}
		}
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
		}

		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			StiColors.InitColors();
		}

		#endregion

		#region Constructors
		public StiButton()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.Opaque, false);
			this.BackColor = Color.Transparent;
		}
		#endregion
	}
}
