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
using System.Windows.Forms.VisualStyles;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	public enum StiButtonAlign
	{
		Left,
		Right
	}

	/// <summary>
	/// Defines the base button control.
	/// </summary>
	[ToolboxItem(false)]
	public class StiButtonEditBase : StiEditBase
	{
		#region Fields
		internal bool IsPressed = false;
		protected Image ButtonBitmap = null;		
		#endregion

		#region Methods
		public Rectangle GetButtonRect(Rectangle bounds)
		{
			if (!ShowButton)return Rectangle.Empty;

			Rectangle rect = StiControlPaint.GetButtonRect(bounds, Flat, ButtonWidth, RightToLeft);

			if (RightToLeft != RightToLeft.Yes)rect.X--;
			rect.Height--;
						
			if (ButtonAlign == StiButtonAlign.Right)
			{
				return rect;
			}
			else 
			{		
				int borderWidth = (int)SystemInformation.Border3DSize.Width;
				if (Flat)borderWidth = 1;

				rect.X = borderWidth;
				rect.Width -= 2;
				return rect;
			}
		}

		protected override Rectangle GetContentRect()
		{
			Rectangle contentRect = StiControlPaint.GetContentRect(this.ClientRectangle, Flat, RightToLeft);
			Rectangle buttonRect = GetButtonRect(this.ClientRectangle);
			buttonRect.Width += 2;
			contentRect.Width -= (buttonRect.Width - SystemInformation.HorizontalScrollBarArrowWidth + 
				SystemInformation.Border3DSize.Width);
			
			if (ButtonAlign == StiButtonAlign.Right)
			{
				contentRect.X += 2;
				return contentRect;
			}
			else
			{
				contentRect.X += buttonRect.Width;
				return contentRect;
			}
		}

        protected virtual void PaintButtons(Graphics g, Rectangle buttonRect, Image bmp, 
            bool isPressed, bool isMouseOverButton)
        {
            if (VisualStyleInformation.IsEnabledByUser && VisualStyleInformation.IsSupportedByOS)
            {
                buttonRect.Inflate(1, 1);
                buttonRect.X += 1;
                buttonRect.Width -= 1;

                VisualStyleElement element = null;
                
                if (DesignMode) element = VisualStyleElement.ComboBox.DropDownButton.Normal;
                else if (isPressed) element = VisualStyleElement.ComboBox.DropDownButton.Pressed;
                else if (!Enabled) element = VisualStyleElement.ComboBox.DropDownButton.Disabled;
                else element = VisualStyleElement.ComboBox.DropDownButton.Normal;

                try
                {
                    if (VisualStyleRenderer.IsElementDefined(element))
                    {
                        //if (bmp != null)
                        {
                            element = VisualStyleElement.Button.PushButton.Normal;

                            if (DesignMode) element = VisualStyleElement.Button.PushButton.Normal;
                            else if (isPressed) element = VisualStyleElement.Button.PushButton.Pressed;
                            //else if (IsMouseOver) element = VisualStyleElement.Button.PushButton.Hot;
                            else if (!Enabled) element = VisualStyleElement.Button.PushButton.Disabled;
                            else element = VisualStyleElement.Button.PushButton.Normal;

                            if (VisualStyleRenderer.IsElementDefined(element))
                            {
                                buttonRect.Inflate(1, 1);
                                VisualStyleRenderer renderer = new VisualStyleRenderer(element);
                                renderer.DrawBackground(g, buttonRect);

                                if (bmp == null) bmp = dropDownButtonBitmap;
                                Rectangle imageRect = new Rectangle(
                                    buttonRect.X + (buttonRect.Width - bmp.Width) / 2 + 1,
                                    buttonRect.Y + (buttonRect.Height - bmp.Height) / 2 + 1,
                                    bmp.Width, bmp.Height);

                                renderer.DrawImage(g, imageRect, bmp);

                                return;
                            }
                        }
                        /*
                        else
                        {
                            if (DesignMode) element = VisualStyleElement.ComboBox.DropDownButton.Normal;
                            else if (isPressed) element = VisualStyleElement.ComboBox.DropDownButton.Pressed;
                            //else if (IsMouseOver) element = VisualStyleElement.ComboBox.DropDownButton.Hot;
                            else if (!Enabled) element = VisualStyleElement.ComboBox.DropDownButton.Disabled;
                            else element = VisualStyleElement.ComboBox.DropDownButton.Normal;

                            if (VisualStyleRenderer.IsElementDefined(element))
                            {
                                VisualStyleRenderer renderer = new VisualStyleRenderer(element);
                                renderer.DrawBackground(g, buttonRect);
                            }
                        }*/
                    }
                }
                catch
                {
                }
            }
            else
            {
                StiControlPaint.DrawButton(g, buttonRect, bmp, isPressed, Focused | IsMouseOver,
                    isMouseOverButton, this.Enabled, Flat);

                if (ButtonBitmap == null)
                {
                    Rectangle imageRect = new Rectangle(
                            buttonRect.X + (buttonRect.Width - dropDownButtonBitmap.Width) / 2,
                            buttonRect.Y + (buttonRect.Height - dropDownButtonBitmap.Height) / 2,
                            dropDownButtonBitmap.Width, dropDownButtonBitmap.Height);
                    g.DrawImage(dropDownButtonBitmap, imageRect);
                }
            }
        }
		#endregion

		#region Properties
		protected virtual bool IsMouseOverButton
		{
			get
			{
				Rectangle rect = GetButtonRect(this.ClientRectangle);
				return rect.Contains(this.PointToClient(Cursor.Position));
			}
		}


		private StiButtonAlign buttonAlign = StiButtonAlign.Right;
		[Browsable(false)]
		[Category("Behavior")]
		[DefaultValue(StiButtonAlign.Right)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual StiButtonAlign ButtonAlign
		{
			get
			{
				return buttonAlign;
			}
			set
			{
				buttonAlign = value;
				SetPosTextBox();
				Invalidate();
			}
		}


		private bool showButton = true;
		[Category("Behavior")]
		[DefaultValue(true)]
		public virtual bool ShowButton
		{
			get
			{
				return showButton;
			}
			set
			{
				showButton = value;
				SetPosTextBox();
				Invalidate();
			}
		}

		
		private int buttonWidth = 16;
		[Browsable(false)]
		[Category("Behavior")]
		[DefaultValue(16)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int ButtonWidth
		{
			get
			{
				return buttonWidth;
			}
			set
			{
				if (value < 4)value = 4;
				else if (value > (Width - 10))value = Width - 10;

				buttonWidth = value;
				SetPosTextBox();
				Invalidate();
			}
		}
		#endregion

		#region Events
		#region ButtonClick
		/// <summary>
		/// Occurs when the user push button.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler ButtonClick;

		protected virtual void OnButtonClick(System.EventArgs e)
		{
			if (ButtonClick != null)ButtonClick(this, e);
		}

		/// <summary>
		/// Raises the ButtonClick event for this control.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		public void InvokeButtonClick(EventArgs e)
		{
			OnButtonClick(e);
		}
		#endregion
		#endregion

		#region Handlers
		protected override void OnPaint(PaintEventArgs p)
		{
			base.OnPaint(p);

			Graphics g = p.Graphics;

			Rectangle buttonRect = GetButtonRect(this.ClientRectangle);
			if (ShowButton)
			{
                PaintButtons(g, buttonRect, this.ButtonBitmap as Bitmap, this.IsPressed, this.IsMouseOverButton);
			}
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			if (IsMouseOverButton)
			{
				OnButtonClick(e);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if ((e.Button & MouseButtons.Left) > 0)
			{
				IsPressed = IsMouseOverButton;
				this.Focus();
				Invalidate();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if ((e.Button & MouseButtons.Left) > 0)
			{
				IsPressed = false;
				Invalidate();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (Flat)Invalidate();
		}

		#endregion

        protected static Image dropDownButtonBitmap;

        static StiButtonEditBase()
        {
            try
            {
				dropDownButtonBitmap = StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.DropDown.png");
            }
            catch
            {
            }
        }
	}
}
