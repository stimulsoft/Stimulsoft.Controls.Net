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

namespace Stimulsoft.Controls
{
	/// <summary>
	/// ToolButton that lets users select a color.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiColorButton), "Toolbox.StiColorButton.bmp")]
	public class StiColorButton : StiToolButton, IStiPopupParentControl
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
		private Color selectedColor = Color.Transparent;
		/// <summary>
		/// Gets or sets a Selected color.
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
		/// Occurs when the drop-down portion of a StiColorButton is shown.
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
		public void InvokeDropDown(EventArgs e)
		{
			OnDropDown(e);
		}

		#endregion

		#region DropUp
		/// <summary>
		/// Occurs when the drop-down portion of a StiColorBox is closed.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler DropUp;

		protected virtual void OnDropUp(System.EventArgs e)
		{
			if (DropUp != null)DropUp(this, e);
		}


		/// <summary>
		/// Raises the DropUp event for this control.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		public void InvokeDropUp(System.EventArgs e)
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
		protected override void OnPaint(PaintEventArgs p)
		{
			base.OnPaint(p);
			Rectangle rect = this.ClientRectangle;
			Graphics g = p.Graphics;
			Rectangle lineRect = new Rectangle(rect.X + 2, rect.Bottom - 7, rect.Width - 14, 5);

			if (Enabled)
			{
				using (Brush brush = new SolidBrush(this.SelectedColor))
				{
					g.FillRectangle(brush, lineRect);
					if (SelectedColor == Color.Transparent)
						g.DrawRectangle(SystemPens.ControlDark,
							lineRect.X, lineRect.Y, lineRect.Width - 1, lineRect.Height - 1);
				}
			}
	
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if ((e.Button & MouseButtons.Left) > 0)
			{
				if ((!LockPopupInvoke) && ColorBoxPopupForm == null)
				{
					//this.Pushed = true;
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
				else 
				{
					LockPopupInvoke = false;
					this.Pushed = true;
				}
			}
		}

		private void OnClosePopup(object sender, EventArgs e)
		{			
			if (ColorBoxPopupForm != null)
			{
				this.Pushed = false;
				InvokeDropUp(e);

				Color value = ColorBoxPopupForm.SelectedColor;
				if (this.SelectedColor != value)this.SelectedColor = value;

				ColorBoxPopupForm.Dispose();
				ColorBoxPopupForm = null;
			}
		}

		#endregion

		public StiColorButton()
		{
			IsDrawDropDownArrow = true;
			base.Style = ToolBarButtonStyle.PushButton;
		}
	}
}