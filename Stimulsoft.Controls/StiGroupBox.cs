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
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using System.Windows.Forms.VisualStyles;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Represents a Windows group box.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiGroupBox), "Toolbox.StiGroupBox.bmp")]
	public class StiGroupBox : GroupBox
	{
		#region Field
		protected bool isMouseOverCollapse = false;
		#endregion

		#region Properties
        public virtual int HeaderSize
        {
            get
            {
                return 20;
            }
        }

		private int resHeight = 0;
		[Category("Behavior")]
		[Browsable(false)]
		public int ResHeight
		{
			get
			{
				return resHeight;
			}
			set
			{
				resHeight = value;
			}
		}
		
		
		[Browsable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				Invalidate();
			}
		}


		private bool collapsed = false;
		[Category("Behavior")]
		[DefaultValue(false)]
		public bool Collapsed
		{
			get
			{
				return collapsed;
			}
			set
			{
				if (collapsed != value)
				{
					collapsed = value;
					if (value)
					{
						ResHeight = this.Height;
						this.Height = HeaderSize;
						foreach (Control control in Controls)control.Visible = false;
						
						if (ChangeFormWhenCollapse)
						{
							Form form = this.FindForm();
							if (form != null)form.Height -= ResHeight - HeaderSize;
						}
						InvokeCollapsedChange(EventArgs.Empty);
					}
					else 
					{
						this.Height = resHeight;
						foreach (Control control in Controls)control.Visible = true;
						if (ChangeFormWhenCollapse)
						{
							Form form = this.FindForm();
							if (form != null)form.Height += ResHeight - HeaderSize;
						}
						InvokeCollapsedChange(EventArgs.Empty);
					}
					Invalidate();
				}
			}
		}

		
		private bool allowCollapse = false;
		[Category("Behavior")]
		[DefaultValue(false)]
		public bool AllowCollapse
		{
			get
			{
				return allowCollapse;
			}
			set
			{
				allowCollapse = value;
				Invalidate();
			}
		}


		private bool changeFormWhenCollapse = true;
		[Category("Behavior")]
		[DefaultValue(true)]
		public bool ChangeFormWhenCollapse
		{
			get
			{
				return changeFormWhenCollapse;
			}
			set
			{
				changeFormWhenCollapse = value;
				Invalidate();
			}
		}

		#endregion

		#region Methods
		protected virtual Rectangle GetCollapseRectangle()
		{
			if (RightToLeft == RightToLeft.Yes)
                return new Rectangle(this.Width - 17, 1, 10, 10);
			else
                return new Rectangle(7, 1, 10, 10);
		}


		private bool GetIsMouseOverCollapse()
		{
			if (DesignMode)return false;
			Point pos = this.PointToClient(Cursor.Position);
			return GetCollapseRectangle().Contains(pos);
		}

	
		private void DrawCollapse()
		{
			using (Graphics g = this.CreateGraphics())
			{
				DrawCollapse(g);
			}
		}


		protected void DrawCollapse(Graphics g)
		{			
			Rectangle rect = GetCollapseRectangle();
			
			Pen pen;
			if (!isMouseOverCollapse)
			{
				g.FillRectangle(StiBrushes.Content, rect);
				g.DrawRectangle(SystemPens.ControlDark, rect);				
				pen = SystemPens.ControlDark;
			}
			else
			{
				g.FillRectangle(StiBrushes.Selected, rect);
				g.DrawRectangle(StiPens.SelectedText, rect);
				pen = StiPens.SelectedText;
			}

			g.DrawLine(pen, rect.X + 2, rect.Y + rect.Height / 2, rect.Right - 2, rect.Y + rect.Height / 2);
			if (Collapsed)g.DrawLine(pen, rect.X + rect.Width / 2, rect.Y + 2, rect.X + rect.Width / 2, rect.Bottom - 2);
		}

		
		private void CheckMouseOver()
		{
			bool resMouseOver = isMouseOverCollapse;
			isMouseOverCollapse = GetIsMouseOverCollapse();
			if (resMouseOver != isMouseOverCollapse && AllowCollapse)DrawCollapse();
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

			int startTextPos = 8;
			if (AllowCollapse)startTextPos = 24;

			Rectangle rect = ClientRectangle;
			Rectangle textRect = rect;
		
			var size = g.MeasureString(this.Text, Font);
			textRect.Width = (int)size.Width + 4;
			textRect.Height = (int)size.Height;
		
			if (RightToLeft == RightToLeft.Yes)
			{
				textRect.X = (rect.Right - textRect.Width) - startTextPos;
			}
			else
			{
				textRect.X += startTextPos;
			}

			if (Text.Length == 0)textRect.Width = 0;

			Color disabledColor = StiColorUtils.GetDisabledColor(this);				
		
			#region Draw Group Box
			int headerHeight = rect.Top + (Font.Height / 2);
			if (Text.Length == 0)headerHeight = 0;

            bool styleUsed = false;
            try
            {
                if (VisualStyleInformation.IsEnabledByUser && VisualStyleInformation.IsSupportedByOS)
                {
                    VisualStyleElement element;
                    if (this.Enabled) 
                        element = VisualStyleElement.Button.GroupBox.Normal;
                    else 
                        element = VisualStyleElement.Button.GroupBox.Disabled;

                    if (VisualStyleRenderer.IsElementDefined(element))
                    {
                        var renderer = new VisualStyleRenderer(element);
                        renderer.DrawBackground(g, new Rectangle(rect.X, rect.Y + headerHeight, rect.Width, rect.Height - headerHeight));
                        styleUsed = true;
                    }
                }
            }
            catch //In some cases strange errors occurred. In this cases draw group box with help of default painting.
            {
                styleUsed = false;
            }
            if (!styleUsed)
            {
                using (var penLight = new Pen(ControlPaint.Light(disabledColor, 1f)))
                using (var penDark = new Pen(ControlPaint.Dark(disabledColor, 0f)))
                {
                    g.DrawLine(penLight, rect.Left + 1, headerHeight, rect.Left + 1, rect.Height - 1);
                    g.DrawLine(penDark, rect.Left, headerHeight - 1, rect.Left, rect.Height - 2);
                    g.DrawLine(penLight, rect.Left, rect.Height - 1, rect.Width, rect.Height - 1);
                    g.DrawLine(penDark, rect.Left, rect.Height - 2, rect.Width - 1, rect.Height - 2);
                    g.DrawLine(penLight, rect.Left + 1, headerHeight, textRect.X + 1, headerHeight);
                    g.DrawLine(penDark, rect.Left, headerHeight - 1, textRect.X, headerHeight - 1);
                    g.DrawLine(penLight, (textRect.X + textRect.Width), headerHeight, rect.Width - 1, headerHeight);
                    g.DrawLine(penDark, (textRect.X + textRect.Width) + 1, headerHeight - 1, rect.Width - 2, headerHeight - 1);
                    g.DrawLine(penLight, rect.Width - 1, headerHeight, rect.Width - 1, rect.Height - 1);
                    g.DrawLine(penDark, rect.Width - 2, headerHeight - 1, rect.Width - 2, rect.Height - 2);
                }
            }
           
			#endregion

            #region Draw text
            using (var sf = new StringFormat())
            {
                sf.FormatFlags = StringFormatFlags.NoWrap;
                sf.Trimming = StringTrimming.EllipsisCharacter;
                sf.Alignment = StringAlignment.Center;
                sf.HotkeyPrefix = HotkeyPrefix.Show;

                if (RightToLeft == RightToLeft.Yes)
                    sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

                if (styleUsed)
                {
                    using (var brush = new SolidBrush(this.BackColor))
                    {
                        g.FillRectangle(brush, textRect);
                    }
                }

                if (base.Enabled)
                {
                    using (var foreBrush = new SolidBrush(this.ForeColor))
                    {
                        g.DrawString(Text, Font, foreBrush, textRect, sf);
                    }
                }
                else
                {
                    ControlPaint.DrawStringDisabled(g, this.Text, this.Font, disabledColor, textRect, sf);
                }
            }
            #endregion

			if (AllowCollapse)DrawCollapse(g);
		}


		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			CheckMouseOver();
		}


		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			CheckMouseOver();
		}


		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			CheckMouseOver();
		}
		

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			Point pos = this.PointToClient(Cursor.Position);
			if (pos.Y >= 0 && pos.Y <= HeaderSize && AllowCollapse)
			{
				Collapsed = !Collapsed;
			}
		}


		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			e.Control.Visible = !Collapsed;
		}
		#endregion

		#region Events
		#region DropDown
		[Category("Behavior")]
		public event EventHandler CollapsedChange;

		protected virtual void OnCollapsedChange(System.EventArgs e)
		{
			if (CollapsedChange != null)
                CollapsedChange(this, e);
		}

		
		public void InvokeCollapsedChange(System.EventArgs e)
		{
			OnCollapsedChange(e);
		}
		#endregion 
		#endregion

		#region Constructors
		public StiGroupBox()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.Selectable, false);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}
		#endregion
	}
}
