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
	/// Represents a Windows radio button.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiRadioButton), "Toolbox.StiRadioButton.bmp")]
	public class StiRadioButton : RadioButton
	{
		#region Browsable(false)
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		public new ImageList ImageList
		{
			get
			{
				return base.ImageList;
			}
			set
			{
				base.ImageList = value;
			}
		}


		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		public new int ImageIndex
		{
			get
			{
				return base.ImageIndex;
			}
			set
			{
				base.ImageIndex = value;
			}
		}


		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		public new Image Image
		{
			get
			{
				return base.Image;
			}
			set
			{
				base.Image = value;
			}
		}


		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		public new ContentAlignment ImageAlign
		{
			get
			{
				return base.ImageAlign;
			}
			set
			{
				base.ImageAlign = value;
			}
		}


		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		public override ContentAlignment TextAlign
		{
			get
			{
				return base.TextAlign;
			}
			set
			{
				base.TextAlign = value;
			}
		}


		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		public new ContentAlignment CheckAlign
		{
			get
			{
				return base.CheckAlign;
			}
			set
			{
				base.CheckAlign = value;
			}
		}

		
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
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
		#endregion

		#region Fields
		private bool isMouseOver = false;
		#endregion

		#region Methods
		public static void DrawRadioButton(Graphics g, Rectangle rect, string text, Font font, 
			Color foreColor, Color backColor,
			RightToLeft rightToLeft, bool isEnabled, 
			bool isMouseOver, bool isFocused, bool isChecked,
			Appearance appearance)
		{
			Rectangle checkRect = Rectangle.Empty;
			Rectangle textRect = Rectangle.Empty;

			textRect = new Rectangle(rect.X + 4, rect.Y, rect.Width - 4, rect.Height);
			if (rightToLeft == RightToLeft.No)
			{
				checkRect = new Rectangle(rect.X, rect.Y + (rect.Height - 12) / 2 - 1, 12, 12);
			}
			else
			{
				checkRect = new Rectangle(rect.Right - 15, rect.Y + (rect.Height - 12) / 2 - 1, 12, 12);				
			}

			if (appearance == Appearance.Normal)
			{
				if (rightToLeft == RightToLeft.No)
				{
					textRect.X += 10;
					textRect.Width -= 10;
				}
				else
				{
					textRect.Width -= 16;
				}

				var oldSmoothingMode = g.SmoothingMode;
				g.SmoothingMode = SmoothingMode.AntiAlias;
			
				if (isEnabled && (isMouseOver || isFocused))
				{
					g.DrawEllipse(StiPens.SelectedText, 
						checkRect.X, checkRect.Y,  checkRect.Width, checkRect.Height);
				}
				else 
				{
					if (!isEnabled)
					{
						using (var pen = new Pen(StiColorUtils.Dark(SystemColors.Control, 30)))
						{
							g.DrawEllipse(pen, 
								checkRect.X, checkRect.Y,  checkRect.Width, checkRect.Height);
						}
					}
					else g.DrawEllipse(SystemPens.ControlDark, 
							 checkRect.X, checkRect.Y,  checkRect.Width, checkRect.Height);
				}
			
				if (isEnabled)
				{
					using (var brush = new LinearGradientBrush(checkRect, 
							   StiColors.ContentDark, SystemColors.ControlLightLight, 45))
					{
						g.FillEllipse(brush, checkRect.X + 1, checkRect.Y + 1, 10, 10);
					}
				}
				else
				{
					g.FillEllipse(SystemBrushes.Control, checkRect.X + 1, checkRect.Y + 1, 10, 10);
				}

				if (isChecked)
				{
					if (isEnabled)
					{
						g.FillEllipse(Brushes.DarkGray, checkRect.X + 3, checkRect.Y + 3, 6, 6);
						g.FillEllipse(Brushes.Black, checkRect.X + 4, checkRect.Y + 4, 4, 4);
					}
					else
					{
						g.FillEllipse(Brushes.DarkGray, checkRect.X + 3, checkRect.Y + 3, 6, 6);
						g.FillEllipse(Brushes.DimGray, checkRect.X + 4, checkRect.Y + 4, 4, 4);
					}
				}

				g.SmoothingMode = oldSmoothingMode;

				#region Paint focus
				if (isFocused)
				{
					Rectangle focusRect = textRect;
					SizeF sizeText = g.MeasureString(text, font);
					focusRect.Width = (int)sizeText.Width;
					focusRect.Y = (focusRect.Height - ((int)sizeText.Height + 2)) / 2;
					focusRect.Height = (int)sizeText.Height + 2;

					if (rightToLeft == RightToLeft.Yes)
					{
						focusRect.X = textRect.Right - focusRect.Width;
					}

					ControlPaint.DrawFocusRectangle(g, focusRect);
				}
				#endregion
			}
			else
			{				
				StiButton.DrawButton(g, rect, text, font, foreColor, backColor, null, -1, null, rightToLeft,
					false, isEnabled, isMouseOver, isChecked, false, isFocused, 
					ContentAlignment.BottomCenter, ContentAlignment.MiddleCenter);
			}

			#region Paint string
			if (appearance == Appearance.Normal)
			{
				using (var sf = new StringFormat())
				{
					sf.FormatFlags = StringFormatFlags.NoWrap;
					sf.LineAlignment = StringAlignment.Center;
					sf.Trimming = StringTrimming.EllipsisCharacter;
					sf.HotkeyPrefix = HotkeyPrefix.Show;

					if (rightToLeft == RightToLeft.Yes)
						sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

					using (var foreBrush = new SolidBrush(foreColor))
					{
						if (isEnabled)
                            g.DrawString(text, font, foreBrush, textRect, sf);
						else 
                            ControlPaint.DrawStringDisabled(g, text, font, backColor, textRect, sf);
					}
				}
			}
			#endregion
			
		}
		#endregion

		#region Properties
		private bool flat = true;
		[DefaultValue(false)]
		[Category("Behavior")]
		public bool Flat
		{
			get
			{
				return flat;
			}
			set
			{
				flat = value;
				Invalidate();
			}
		}
		#endregion

		#region Handlers
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			StiColors.InitColors();
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

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			if (!flat)base.OnPaint(e);
			else 
			{
				Graphics g = e.Graphics;
				Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

				DrawRadioButton(g, rect, Text, Font, ForeColor, SystemColors.Control, RightToLeft,
					Enabled, isMouseOver, Focused, Checked, Appearance);
			}
		}

		#endregion

		#region Constructors
		public StiRadioButton()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, false);
			this.SetStyle(ControlStyles.Opaque, false);
		}
		#endregion
	}
}
