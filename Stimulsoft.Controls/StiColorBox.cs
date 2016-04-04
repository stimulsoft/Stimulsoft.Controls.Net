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
using System.Windows.Forms;
using System.ComponentModel;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Allows the user to select a color.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiTabControl), "Toolbox.StiColorBox.bmp")]
	public class StiColorBox : StiButtonEditBase, IStiPopupParentControl
	{
		#region IStiPopupParentControl
		private bool lockPopupInvoke = false;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool LockPopupInvoke
		{
			get
			{
				return lockPopupInvoke;
			}
			set
			{
				lockPopupInvoke = value;
			}
		}
		#endregion

		#region Fields
		internal StiColorBoxPopupForm ColorBoxPopupForm = null;
		#endregion

		#region Browsable(false)

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		#endregion
		
		#region Properties
		private bool showColorBox = true;
		/// <summary>
		/// Gets or sets a value indicating for draws color box with selected color.
		/// </summary>
		[DefaultValue(true)]
		[Category("Behavior")]
		public bool ShowColorBox
		{
			get
			{
				return showColorBox;
			}
			set
			{
				showColorBox = value;
			}
		}


		private bool showColorName = true;
		/// <summary>
		/// Gets or sets a value indicating for draws color name of selected color.
		/// </summary>
		[DefaultValue(true)]
		[Category("Behavior")]
		public bool ShowColorName
		{
			get
			{
				return showColorName;
			}
			set
			{
				showColorName = value;
				Invalidate();
			}
		}


		private Color selectedColor = Color.Transparent;
		/// <summary>
		/// Gets or sets the Selected color.
		/// </summary>
		[Category("Behavior")]
		public Color SelectedColor
		{
			get
			{
				return selectedColor;
			}
			set
			{
				selectedColor = value;
				this.OnSelectedIndexChanged(EventArgs.Empty);
				Invalidate();
			}
		}

		#endregion

		#region Events
		#region DropDown
		/// <summary>
		/// Occurs when the drop-down portion of a StiColorBox is shown.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler DropDown;

		protected virtual void OnDropDown(System.EventArgs e)
		{
			if (DropDown != null)DropDown(this, e);
		}

		
		/// <summary>
		/// Raises the DropDown event for this control.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		public void InvokeDropDown(System.EventArgs e)
		{
			OnDropDown(e);
		}
		#endregion 

		#region DropUp
		/// <summary>
		/// Occurs when the drop-down portion of a StiColorBox is close.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler DropUp;

		protected virtual void OnDropUp(EventArgs e)
		{
			if (DropUp != null)DropUp(this, e);
		}


		/// <summary>
		/// Raises the DropUp event for this control.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		public void InvokeDropUp(EventArgs e)
		{
			OnDropUp(e);
		}
		#endregion

		#region SelectedIndexChanged
		/// <summary>
		/// Occurs when the SelectedIndex property has changed.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler SelectedIndexChanged;

		protected virtual void OnSelectedIndexChanged(System.EventArgs e)
		{
			if (SelectedIndexChanged != null)SelectedIndexChanged(this, e);
		}

		/// <summary>
		/// Raises the SelectedIndexChanged event for this control.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		public void InvokeSelectedIndexChanged(System.EventArgs e)
		{
			OnSelectedIndexChanged(e);
		}
		#endregion		
		#endregion

		#region Handlers
		protected override void OnEnabledChanged(System.EventArgs e)
		{
			base.OnEnabledChanged(e);
			TextBox.Visible = false;
			Invalidate();
		}
		protected override void OnPaint(PaintEventArgs p)
		{
			base.OnPaint(p);
			
			Graphics g = p.Graphics;

			Rectangle colorRect = Rectangle.Empty;
			Rectangle textRect = Rectangle.Empty;

			Rectangle buttonRect = StiControlPaint.GetButtonRect(this.ClientRectangle, true, RightToLeft);
			Rectangle contentRect = StiControlPaint.GetContentRect(this.ClientRectangle, true, RightToLeft);
			Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
			
			if (showColorBox)
			{
				if (showColorName)
				{
					colorRect = new Rectangle(rect.X + 4, rect.Y + 4, 22, Height - 9);
					if (RightToLeft == RightToLeft.Yes)colorRect.X = rect.Width - colorRect.Width - 4;
				}
				else 
				{
					colorRect = new Rectangle(rect.X + 4, rect.Y + 4, 
						Width - buttonRect.Width - 10, Height - 9);

					if (RightToLeft == RightToLeft.Yes)colorRect.X = rect.Width - colorRect.Width - 4;
				}
				
			}

			if (showColorName)
			{
				if (showColorBox)
				{
					textRect = new Rectangle(colorRect.Right + 2, 1,
						Width - colorRect.Width - buttonRect.Width - 4, Height - 2);

					if (RightToLeft == RightToLeft.Yes)textRect.X = contentRect.X - 4;
				}
				else 
				{
					textRect = new Rectangle(2, 1, Width - buttonRect.Width - 2, Height - 2);
					if (RightToLeft == RightToLeft.Yes)textRect.X = contentRect.X - 4;
				}
			}

			#region Paint focus
			if (this.Focused)
			{
				Rectangle focusRect = GetContentRect();
				focusRect.X -= 1;
				focusRect.Y += 1;
				focusRect.Width --;
				focusRect.Height -= 2;
				ControlPaint.DrawFocusRectangle(g, focusRect);
			}
			#endregion

			#region Paint color
			if (showColorBox)
			{
				using (var brush = new SolidBrush(SelectedColor))
					g.FillRectangle(brush, colorRect);
				if (this.Enabled)g.DrawRectangle(Pens.Black, colorRect);
				else g.DrawRectangle(SystemPens.ControlDark, colorRect);
			}
			#endregion

			#region Paint color name
			if (showColorName)
			{
				using (var sf = new StringFormat())
				{
					sf.LineAlignment = StringAlignment.Center;
					sf.FormatFlags = StringFormatFlags.NoWrap;

					if (this.RightToLeft == RightToLeft.Yes)
						sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

					string colorName = null;
					if (SelectedColor.IsSystemColor)
					{
						colorName = StiLocalization.Get("PropertySystemColors", SelectedColor.Name);
					}
					else if (SelectedColor.IsKnownColor)
					{
						colorName = StiLocalization.Get("PropertyColor", SelectedColor.Name);
					}
					else 
					{
						colorName = 
							StiLocalization.Get("FormColorBoxPopup", "Color") + 
							"[A=" + SelectedColor.A.ToString() + 
							", R=" + SelectedColor.R.ToString() + 
							", G=" + SelectedColor.G.ToString() + 
							", B=" + SelectedColor.B.ToString() + "]";
					}

				    if (this.Enabled)
				    {
				        using (var brush = new SolidBrush(this.ForeColor))
				        {
				            g.DrawString(colorName, Font, brush, textRect, sf);
				        }
				    }
				    else
				    {
				        g.DrawString(colorName, Font, SystemBrushes.ControlDark, textRect, sf);
				    }
				}
			}
			#endregion
	
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if ((e.Button & MouseButtons.Left) > 0 && (!ReadOnly))
			{
				if ((!LockPopupInvoke) && ColorBoxPopupForm == null)
				{
					InvokeDropDown(EventArgs.Empty);
					ColorBoxPopupForm = new StiColorBoxPopupForm(this, this.SelectedColor);
					ColorBoxPopupForm.ClosePopup += new EventHandler(OnClosePopup);
		
					Rectangle rect = this.RectangleToScreen(this.ClientRectangle);
                    ColorBoxPopupForm.SetLocation(rect.Left, rect.Top + this.Height + 1, 210, 270);
				
					ColorBoxPopupForm.Font = this.Font;
					ColorBoxPopupForm.ShowPopupForm();

					ColorBoxPopupForm.TabControl.SelectedIndex = 
						ColorBoxPopupForm.TabControl.SelectedIndex;
					InvokeDropDown(EventArgs.Empty);
				}
				else LockPopupInvoke = false;
			}
		}

		private void OnClosePopup(object sender, EventArgs e)
		{
			if (ColorBoxPopupForm != null)
			{
				InvokeDropUp(e);
				Color value = ColorBoxPopupForm.SelectedColor;
				if (this.SelectedColor != value)
				{
					this.SelectedColor = value;
					InvokeSelectedIndexChanged(EventArgs.Empty);
				}

				ColorBoxPopupForm.Dispose();
				ColorBoxPopupForm = null;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && ButtonBitmap != null)
			{
				ButtonBitmap.Dispose();
			}
			base.Dispose(disposing);
 
		}

		#endregion

		#region Constructors
		public StiColorBox()
		{
			this.TextBox.Visible = false;			
		}        
		#endregion
	}
}
