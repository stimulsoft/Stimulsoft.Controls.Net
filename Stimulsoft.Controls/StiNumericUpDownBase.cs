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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Stimulsoft.Base.Drawing;


namespace Stimulsoft.Controls
{
	internal enum StiNumericDirection
	{
		Up,
		Down
	}

	/// <summary>
	/// Defines base NumericUpDown Control.
	/// </summary>
	[ToolboxItem(false)]
	public class StiNumericUpDownBase : StiButtonEditBase
	{
		#region Fields
		protected Bitmap buttonUpBitmap = null;
		protected Bitmap buttonDownBitmap = null;

		private StiNumericDirection direction;
		private Timer timer = new Timer();
		private int tickCount = 0;

		internal bool isUpPressed = false;
		internal bool isDownPressed = false;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the value, indicates that mouse is over on an up button.
		/// </summary>
		[Browsable(false)]
		public bool IsUpMouseOver
		{
			get
			{
				return this.RectangleToScreen(GetUpButtonRect(ClientRectangle, Flat, 
					RightToLeft)).Contains(Cursor.Position);
			}
		}

		/// <summary>
		/// Gets or sets the value indicates that mouse is over on a down button.
		/// </summary>
		[Browsable(false)]
		public bool IsDownMouseOver
		{
			get
			{
				return this.RectangleToScreen(GetDownButtonRect(ClientRectangle, Flat,
					RightToLeft)).Contains(Cursor.Position);
			}
		}

		#endregion

		#region Events
		[Category("Behavior")]
		public event EventHandler UpButtonClick;
		[Category("Behavior")]
		public event EventHandler DownButtonClick;

		protected virtual void OnUpButtonClick(System.EventArgs e)
		{
			if (UpButtonClick != null)UpButtonClick(this, e);
		}

		protected virtual void OnDownButtonClick(System.EventArgs e)
		{
			if (DownButtonClick != null)DownButtonClick(this, e);
		}

		public void InvokeUpButtonClick(EventArgs e)
		{
			OnUpButtonClick(e);
		}

		public void InvokeDownButtonClick(EventArgs e)
		{
			OnDownButtonClick(e);
		}
		#endregion

		#region Methods
		public static void DrawNumericUpDown(Graphics g, 
			Rectangle rect,		
			string text, Font font, 
			Color foreColor, Color backColor,
			RightToLeft rightToLeft, 
			bool isEnabled, bool readOnly, bool isMouseOver,
			bool isUpMouseOver, bool isDownMouseOver, 
			bool isUpPressed, bool isDownPressed, bool isFocused, 
			bool flat, bool fillContent,
			Bitmap buttonUpBitmap, Bitmap buttonDownBitmap)
		{
			if (isEnabled)text = string.Empty;
			
			if (fillContent)
			{
				using (var brush = new SolidBrush(backColor))
				{
					g.FillRectangle(brush, rect);
				}
			}

			if (!isEnabled)
			{
				using (var brush = new SolidBrush(SystemColors.Control))
				{
					var rcContent = rect;
					rcContent.X += 2;
					rcContent.Y += 2;
					rcContent.Height -= 3;
					rcContent.Width -= 2;
					g.FillRectangle(brush, rcContent);
				}
			}

			var upButtonRect = GetUpButtonRect(rect, flat, rightToLeft);
			var downButtonRect = GetDownButtonRect(rect, flat, rightToLeft);
			var contentRect = StiControlPaint.GetContentRect(rect, flat, rightToLeft);
			contentRect.Height++;

			if (readOnly)isMouseOver = false;

			StiControlPaint.DrawBorder(g, rect, isMouseOver | isFocused, flat);			

			#region Paint up button
			Color color = SystemColors.ControlDark;
			
			if (isUpMouseOver)color = StiColors.SelectedText;
			if (buttonUpBitmap != null && buttonDownBitmap != null)
			{
				StiControlPaint.DrawButton(g, upButtonRect, buttonUpBitmap, isUpPressed, isUpMouseOver | isFocused, 
					isUpMouseOver, isEnabled, flat);
			}
			#endregion			
			
			#region Paint down button
			color = SystemColors.ControlDark;
			
			if (isDownMouseOver)color = StiColors.SelectedText;
			if (buttonUpBitmap != null && buttonDownBitmap != null)
			{
				StiControlPaint.DrawButton(g, downButtonRect, buttonDownBitmap, isDownPressed, isDownMouseOver | isFocused, 
					isDownMouseOver, isEnabled, flat);
			}
			#endregion

			if (text != null)
			{
				Color textColor = foreColor;
				if (!isEnabled)textColor = SystemColors.ControlDark;
				using (var sf = new StringFormat())
				using (var brush = new SolidBrush(textColor))
				{
					sf.FormatFlags = StringFormatFlags.NoWrap;
					sf.LineAlignment = StringAlignment.Center;
					g.DrawString(text, font, brush, rect, sf); 
				}
			}

			if (isMouseOver)
			{
				g.DrawRectangle(StiPens.SelectedText, rect);
			}
		}


		private static Rectangle GetUpButtonRect(Rectangle clientRect, bool flat, RightToLeft rightToLeft)
		{
			var rect = StiControlPaint.GetButtonRect(clientRect, flat, rightToLeft);
			int size = rect.Height / 2 - 1;

			if (flat)return new Rectangle(rect.X, rect.Y, rect.Width, size);
			return new Rectangle(rect.X, rect.Y, rect.Width, size + 1);
			
		}

		private static Rectangle GetDownButtonRect(Rectangle clientRect, bool flat, RightToLeft rightToLeft)
		{
			var rect = StiControlPaint.GetButtonRect(clientRect, flat, rightToLeft);
			int size = rect.Height / 2 + 2;
			
			if (flat)return new Rectangle(rect.X, rect.Y + size, rect.Width, rect.Height - size);
			return new Rectangle(rect.X, rect.Y + size - 1, rect.Width, rect.Height - size + 1);
		}


		protected virtual void DoUp()
		{
		
		}

		protected virtual void DoDown()
		{
			
		}

		#endregion
		
		#region Handlers
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			StiColors.InitColors();
		}

		protected override void OnPaint(PaintEventArgs p)
		{
			Graphics g = p.Graphics;

			Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

			DrawNumericUpDown(g, rect, Text, Font, ForeColor, BackColor,
				RightToLeft, Enabled, ReadOnly, IsMouseOver, IsUpMouseOver, IsDownMouseOver,
				isUpPressed, isDownPressed, Focused, Flat, false, buttonUpBitmap, buttonDownBitmap);
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			if (IsMouseOverButton)
			{
				if (IsUpMouseOver)OnUpButtonClick(e);
				if (IsDownMouseOver)OnDownButtonClick(e);
				OnButtonClick(e);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if ((e.Button & MouseButtons.Left) > 0)
			{
				if (IsMouseOverButton)
				{
					if (IsUpMouseOver)
					{
						direction = StiNumericDirection.Up;
						DoUp();
					}
					if (IsDownMouseOver)
					{
						direction = StiNumericDirection.Down;
						DoDown();
					}
					tickCount = 0;
					timer.Interval = 300;
					timer.Enabled = true;
					isUpPressed = IsUpMouseOver;
					isDownPressed = IsDownMouseOver;
				}
				this.Focus();
				Invalidate();

			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if ((e.Button & MouseButtons.Left) > 0)
			{
				isUpPressed = false;
				isDownPressed = false;
				timer.Enabled = false;
				Invalidate();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			Invalidate();
		}

		private void Tick(object sender, EventArgs e)
		{
			if (direction == StiNumericDirection.Up)DoUp();
			else DoDown();
			tickCount++;
			if (tickCount > 2 && timer.Interval == 300)
			{
				timer.Interval = 100;
			}
			if (tickCount > 10 && timer.Interval == 100)
			{
				timer.Interval = 30;
			}

		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (buttonUpBitmap != null)buttonUpBitmap.Dispose();
				if (buttonDownBitmap != null)buttonDownBitmap.Dispose();
			}
			base.Dispose(disposing); 
		}

		#endregion

		#region Constructors
		public StiNumericUpDownBase()
		{
			buttonUpBitmap = StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.SpinUp.bmp");
			buttonDownBitmap = StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.SpinDown.bmp");

			timer.Tick += new EventHandler(Tick);
		}
		#endregion

	}
}
