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
using System.ComponentModel;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Displays a text box control for the user input.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiTextBox), "Toolbox.StiTextBox.bmp")]
	public class StiTextBox : TextBox
	{
		#region Browsable(false)
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new BorderStyle BorderStyle
		{
			get
			{
				return base.BorderStyle;
			}
			set
			{
				base.BorderStyle = value;
			}
		}
		#endregion

		#region Fields
		private bool isMouseOver = false;
		#endregion

		#region Methods
		public static void DrawTextBox(Graphics g, Rectangle rect, string text, Font font, 
			Color foreColor, Color backColor, 
			bool isMouseOver, bool isFocused, bool flat, bool drawBorder)
		{
			using (var brush = new SolidBrush(backColor))
			{
				g.FillRectangle(brush, rect);
			}
			if (drawBorder)
			{
				StiControlPaint.DrawBorder(g, rect, isMouseOver | isFocused, flat);
			}
		}

		
		private void Draw()
		{
			using (Graphics g = Graphics.FromHwnd(this.Handle))
			{			
				Rectangle rect = new Rectangle(
					this.ClientRectangle.X,
					this.ClientRectangle.Y,
					this.ClientRectangle.Width - 1,
					this.ClientRectangle.Height - 1);

				if (Flat)
				StiControlPaint.DrawBorder(g, rect, isMouseOver | this.Focused, Flat);
			}
		}
		#endregion

		#region Properties
		private bool flat = true;
		[DefaultValue(true)]
		[Category("Appearance")]
		public virtual bool Flat
		{
			get
			{
				return flat;
			}
			set
			{
				flat = value;
				if (value)this.BorderStyle = BorderStyle.FixedSingle;
				else this.BorderStyle = BorderStyle.Fixed3D;
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

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if (Flat)Invalidate();
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if (Flat)Invalidate();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			isMouseOver = true;
			if (Flat)Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			isMouseOver = false;
			if (Flat)Invalidate();
		}

		protected override void WndProc(ref Message msg)
		{
			if (msg.Msg == (int)Win32.Msg.WM_PAINT)
			{
				base.DefWndProc(ref msg);
				if (Flat)Draw();
			}
			else base.WndProc(ref msg);
		}

		#endregion

		#region Constructors
		public StiTextBox()
		{
            this.BorderStyle = BorderStyle.FixedSingle;
		}
		#endregion
	}
}
