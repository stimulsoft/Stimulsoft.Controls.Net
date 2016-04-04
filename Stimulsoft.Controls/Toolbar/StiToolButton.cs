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
using System.ComponentModel;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	#region StiTextAndImageAlign
	public enum StiTextAndImageAlign
	{
		Left, 
		Center, 
		Right
	}
	#endregion

	/// <summary>
	/// Represents a Windows toolbar button.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiOutlookBar), "Toolbox.StiToolButton.bmp")]
    #if !Profile
	[Designer(typeof(StiToolButtonDesigner))]
    #endif
	public class StiToolButton : Button
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
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}
		#endregion

		#region Methods
		public new void PerformClick()
		{
			this.OnClick(EventArgs.Empty);
		}

		private void CalcRectangles(out Rectangle textRect, out Rectangle imageRect, out Rectangle dropDownArrowRect,
			Rectangle rect, Graphics g, bool drawDropDownArrow)
		{
			dropDownArrowRect = Rectangle.Empty;
			
			if (drawDropDownArrow)
			{
				dropDownArrowRect.X = rect.Right - 5;
				dropDownArrowRect.Y = rect.Y + (rect.Height - dropDownArrowBitmap.Size.Height) / 2 + 1;
				dropDownArrowRect.Size = dropDownArrowBitmap.Size;
				rect.Width -= 10;

			}

			Size imageSize = Size.Empty;
			if (ImageList != null && ImageIndex >= 0 && ImageIndex < ImageList.Images.Count)
			{
				imageSize = ImageList.ImageSize;
			}

			try
			{
				if (imageSize.IsEmpty && Image != null) imageSize = new Size(Image.Width, Image.Height);
			}
			catch
			{
				imageSize = new Size(16, 16);
			}

			SizeF textSize;

			using (StringFormat sf = GetStringFormat())
			{
				textSize = g.MeasureString(Text, Font, new SizeF(rect.Width, rect.Height), sf);
			}

			textRect = rect;
			imageRect = new Rectangle(rect.X, rect.Y, imageSize.Width, imageSize.Height);

			if (TextAlign == ContentAlignment.MiddleCenter && ImageAlign == ContentAlignment.MiddleCenter)
			{
				imageRect.Y = rect.Y + (rect.Height - imageSize.Height) / 2;

				switch (TextAndImageAlign)
				{
					case StiTextAndImageAlign.Center:
						textRect.X += imageSize.Width;
						textRect.Width -= imageSize.Width;

						int pos = (int)((rect.Width - imageSize.Width - textSize.Width) / 2);
						imageRect.X = rect.X + pos;
						break;

					case StiTextAndImageAlign.Left:
						imageRect.X = 2;
						textRect.X = imageRect.Right;
						textRect.Width = (int)textSize.Width + 4;
						break;

					case StiTextAndImageAlign.Right:
						imageRect.X = rect.Width - imageSize.Width + 4;
						textRect.X =  rect.Width - imageRect.Width - (int)textSize.Width + 4;
						textRect.Width = (int)textSize.Width + 4;
						break;
				}
			}
			else
			{
				switch (ImageAlign)
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
		}


		private Size GetImageSize()
		{
			if ((ImageList != null && ImageIndex >= 0 && ImageIndex < ImageList.Images.Count) || Image != null)
			{
				if (ImageList != null && ImageIndex >= 0 && ImageIndex < ImageList.Images.Count)
				{
					return ImageList.ImageSize;
				}
				else if (Image != null)return Image.Size;
			}
			return Size.Empty;
		}
	

		private void DrawImage(Graphics g, Rectangle imageRect)
		{
			if ((ImageList != null && ImageIndex >= 0 && ImageIndex < ImageList.Images.Count) || Image != null)
			{
				if (ImageList != null && ImageIndex >= 0 && ImageIndex < ImageList.Images.Count)
				{
					if (Enabled)ImageList.Draw(g, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height, ImageIndex);
					else StiControlPaint.DrawImageDisabled(g, ImageList.Images[ImageIndex], imageRect.Left, imageRect.Top);
				}
				else
				{
					if (Enabled)g.DrawImage(Image, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height);
					else StiControlPaint.DrawImageDisabled(g, Image, imageRect.Left, imageRect.Top);
				}
			}
		}


		protected void DrawDropDownArrow(Graphics g, Rectangle rect)
		{
			if (IsDrawDropDownArrow || DropDownMenu != null)
			{	
				if (Enabled)g.DrawImage(dropDownArrowBitmap, rect.X, rect.Y, rect.Width, rect.Height);
				else StiControlPaint.DrawImageDisabled(g, dropDownArrowBitmap, rect.Left, rect.Top);
			}			
		}


		private StringFormat GetStringFormat()
		{
			var sf = new StringFormat
			{
			    Trimming = StringTrimming.EllipsisCharacter,
			    FormatFlags = 0,
			    HotkeyPrefix = HotkeyPrefix.Show
			};
		    if (!wordWrap)
                sf.FormatFlags = StringFormatFlags.NoWrap;
			if (RightToLeft == RightToLeft.Yes)
				sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

			#region TextAlignment
			if (TextAlign == ContentAlignment.MiddleCenter && ImageAlign == ContentAlignment.MiddleCenter)
			{
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Center;
			}
			else
			{			
				switch (TextAlign)
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

        protected void DrawText(Graphics g)
        {
            DrawText(g, GetTextRect(g));
        }
	
		protected void DrawText(Graphics g, Rectangle textRect)
		{
			if (!string.IsNullOrEmpty(Text))
			{
				using (var sf = GetStringFormat())
				{
				    if (Enabled)
				    {
				        using (var brush = new SolidBrush(ForeColor))
				        {
				            g.DrawString(Text, Font, brush, textRect, sf);
				        }
				    }
				    else
				    {
				        ControlPaint.DrawStringDisabled(g, Text, Font, SystemColors.ControlLight, textRect, sf);
				    }
				}
			}
		}


		private Size GetActualSize()
		{
			using (var g = this.CreateGraphics())
			using (var sf = GetStringFormat())				
			{
				Size textSize = g.MeasureString(this.Text, this.Font, 1000000, sf).ToSize();
				if (this.TextAlign == ContentAlignment.MiddleCenter)
				{
					Size imageSize = GetImageSize();
					textSize.Width += imageSize.Width;
				}

				return textSize;
			}
		}


		private void UpdateSize()
		{
			if (AutoSize)
			{
				Size size = GetActualSize();
				size.Width += 10;
				size.Height += 10;

				if (this.Size != size)
				{
					switch (Dock)
					{
						case DockStyle.None:
							if (this.Width != size.Width && this.Height != size.Height)this.Size = size;
							break;

						case DockStyle.Left:
						case DockStyle.Right:
							if (this.Width != size.Width)this.Width = size.Width;
							break;

						case DockStyle.Top:
						case DockStyle.Bottom:
							if (this.Height != size.Height)this.Height = size.Height;
							break;
					}
				}
			}
		}

		/// <summary>
		/// Resets all pushed ToolButton.
		/// </summary>
		private void ResetPushed()
		{
			if (Parent != null)
			{
				foreach (Control cn in Parent.Controls)
					if (cn != this && cn is StiToolButton && ((StiToolButton)cn).Pushed)
						((StiToolButton)cn).Pushed = false;
			}
		}
		#endregion

		#region Fields
		private bool lockClick = false;
		protected bool IsMouseOver = false;
		protected bool IsPressed = false;
		#endregion

		#region Properties

        private StiControlStyle controlStyle = StiControlStyle.Flat;
        [DefaultValue(StiControlStyle.Flat)]
        public StiControlStyle ControlStyle
        {
            get
            {
                return controlStyle;
            }
            set
            {
                controlStyle = value;
            }
        }

		private StiTextAndImageAlign textAndImageAlign = StiTextAndImageAlign.Center;
		[Category("Appearance")]
		[DefaultValue(StiTextAndImageAlign.Center)]
		public StiTextAndImageAlign TextAndImageAlign
		{
			get
			{
				return textAndImageAlign;
			}
			set
			{
				textAndImageAlign = value;
				Invalidate();
			}
		}

        [Category("Behavior")]
		[DefaultValue(false)]
        public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;

				if (value)UpdateSize();

				Invalidate();
			}
		}

        [DefaultValue(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected virtual bool AllowDrawText
        {
            get
            {
                return true;
            }
        }

        [DefaultValue(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected virtual bool AllowDrawImage
        {
            get
            {
                return true;
            }
        }

        [DefaultValue(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected virtual bool AllowDrawDrawDropDownArrow
        {
            get
            {
                return true;
            }
        }

		private bool wordWrap = false;
		[Category("Behavior")]
		[DefaultValue(false)]
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


		private bool isDrawDropDownArrow = false;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsDrawDropDownArrow
		{
			get
			{
				return isDrawDropDownArrow;
			}
			set
			{
				isDrawDropDownArrow = value;
			}
		}


		private bool hotDragDrop = false;
		/// <summary>
		/// Internal use only.
		/// </summary>
		[Browsable(false)]
		public bool HotDragDrop
		{
			get
			{
				return hotDragDrop;
			}
			set
			{
				hotDragDrop = value;
			}
		}


		private bool draw3DButton = false;
		/// <summary>
		/// Gets or sets value, indicates where this tool button draws as 3D button.
		/// </summary>
		[Browsable(false)]
		public bool Draw3DButton
		{
			get
			{
				return draw3DButton;
			}
			set
			{
				draw3DButton = value;
			}
		}


		private bool allowAllUp = false;
		/// <summary>
		/// Gets or sets value, indicates where this tool button can unpush all buttons in this groups.
		/// </summary>
		[DefaultValue(false)]
		[Category("Behavior")]
		public bool AllowAllUp
		{
			get
			{
				return allowAllUp;
			}
			set
			{
				allowAllUp = value;
			}
		}


		private Menu dropDownMenu;
		/// <summary>
		/// Gets or sets the menu to be displayed in the drop-down toolbar button.
		/// </summary>
		[Category("Behavior")]
		public Menu DropDownMenu
		{
			get
			{
				return dropDownMenu;
			}
			set
			{
				dropDownMenu = value;
			}
		}

		
		private ToolBarButtonStyle style = ToolBarButtonStyle.ToggleButton;
		/// <summary>
		/// Gets or sets the style of a toolbar button.
		/// </summary>
		[DefaultValue(ToolBarButtonStyle.ToggleButton)]
		[Category("Behavior")]
		public ToolBarButtonStyle Style
		{
			get
			{
				return style;
			}
			set
			{
				style = value;
				Invalidate();
			}
		}


		private bool pushed = false;
		/// <summary>
		/// Gets or sets a value indicating whether a toggle-style toolbar button is currently in the pushed state.
		/// </summary>
		[DefaultValue(false)]
		[Category("Behavior")]
		public bool Pushed
		{
			get
			{
				return pushed;
			}
			set
			{
				if (style == ToolBarButtonStyle.PushButton)
				{
					if (value && AllowAllUp)ResetPushed();
					pushed = value;
				}
				else pushed = false;
				Invalidate();
			}
		}

		#endregion

		#region Handlers
		protected override bool ProcessMnemonic(char charCode)
		{
			if (IsMnemonic(charCode, this.Text))
			{
				this.OnClick(EventArgs.Empty);

				lockClick = false;
                
				return true;
			}
                        
			return base.ProcessMnemonic(charCode);

		}

		
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			StiColors.InitColors();
		}

		protected override void OnPaint(PaintEventArgs p)
		{
			UpdateSize();
			Graphics g = p.Graphics;
			var rect = new Rectangle(0, 0, Width, Height);

			#region Separator
			if (Style == ToolBarButtonStyle.Separator)
			{
				if (Dock == DockStyle.Top || Dock == DockStyle.Bottom)
				{
					int py = Height / 2 - 1;
					g.DrawLine(SystemPens.ControlDark, 2, py, Width - 4, py);
					g.DrawLine(SystemPens.ControlLightLight, 2, py + 1, Width - 4, py + 1);
				}
				else
				{
					int px = Width / 2 - 1;
					g.DrawLine(SystemPens.ControlDark, px, 2, px, Height - 4);
					g.DrawLine(SystemPens.ControlLightLight, px + 1, 2, px + 1, Height - 4);
				}
			}
			#endregion

			else
			{
                bool isOffice2013Blue = false;
                if (Parent != null)
                {
                    var toolBar = Parent as StiToolBar;
                    if (toolBar != null && toolBar.ControlStyle == StiControlStyle.Office2013Blue)
                    {
                        isOffice2013Blue = true;
                    }
                }

                if (isOffice2013Blue)
                {
                    #region pushed | pressed
                    if (Enabled && (Pushed || IsPressed))
                    {
                        if (IsPressed)
                        {
                            using (var brush = new SolidBrush(StiStyleOffice2013Blue.ColorButtonPressed))
                            {
                                g.FillRectangle(brush, rect);
                            }
                        }
                        else g.FillRectangle(StiBrushes.Selected, rect);
                    }
                    #endregion

                    if (!IsPressed)
                    {
                        if (IsMouseOver && (!DesignMode))
                        {
                            using (var brush = new SolidBrush(StiStyleOffice2013Blue.ColorButtonButtonMouseOver))
                            {
                                g.FillRectangle(brush, rect);
                            }
                        }
                        else if (DesignMode || Draw3DButton) ControlPaint.DrawBorder3D(g, rect, Border3DStyle.RaisedInner, Border3DSide.All);
                    }

                    rect = new Rectangle(0, 0, Width, Height);
                    Rectangle imageRect;
                    Rectangle textRect;
                    Rectangle dropDownArrowRect;

                    CalcRectangles(out textRect, out imageRect, out dropDownArrowRect,
                        new Rectangle(rect.X + 4, rect.Y + 4, rect.Width - 8, rect.Height - 8), g,
                        DropDownMenu != null || IsDrawDropDownArrow);

                    try
                    {
                        if (AllowDrawImage) DrawImage(g, imageRect);
                        if (AllowDrawText) DrawText(g, textRect);
                        if (AllowDrawDrawDropDownArrow) DrawDropDownArrow(g, dropDownArrowRect);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    #region pushed | pressed
                    if (Enabled && (Pushed || IsPressed))
                    {
                        rect.X++;
                        rect.Y++;
                        rect.Width -= 2;
                        rect.Height -= 2;

                        if (IsPressed)
                        {
                            using (var brush = new SolidBrush(StiColorUtils.Dark(StiColors.Selected, 50)))
                            {
                                g.FillRectangle(brush, rect);
                            }
                        }
                        else g.FillRectangle(StiBrushes.Selected, rect);

                        rect.Width--;
                        rect.Height--;
                        g.DrawRectangle(StiPens.SelectedText, rect);

                        rect.X--;
                        rect.Y--;
                        rect.Width += 3;
                        rect.Height += 3;
                    }
                    #endregion

                    if (!IsPressed)
                    {
                        if (IsMouseOver && (!DesignMode))
                        {
                            g.FillRectangle(StiBrushes.Focus, rect);

                            rect.Width--;
                            rect.Height--;
                            g.DrawRectangle(StiPens.SelectedText, rect);

                            if (IsDrawDropDownArrow || DropDownMenu != null)
                            {
                                g.DrawLine(StiPens.SelectedText, rect.Right - 10, rect.Y, rect.Right - 10, rect.Bottom);
                            }
                        }
                        else if (DesignMode || Draw3DButton) ControlPaint.DrawBorder3D(g, rect, Border3DStyle.RaisedInner, Border3DSide.All);
                    }

                    rect = new Rectangle(0, 0, Width, Height);
                    Rectangle imageRect;
                    Rectangle textRect;
                    Rectangle dropDownArrowRect;

                    CalcRectangles(out textRect, out imageRect, out dropDownArrowRect,
                        new Rectangle(rect.X + 4, rect.Y + 4, rect.Width - 8, rect.Height - 8), g,
                        DropDownMenu != null || IsDrawDropDownArrow);

                    try
                    {
                        if (AllowDrawImage) DrawImage(g, imageRect);
                        if (AllowDrawText) DrawText(g, textRect);
                        if (AllowDrawDrawDropDownArrow) DrawDropDownArrow(g, dropDownArrowRect);
                    }
                    catch
                    {
                    }
                }
			}			
		}

        protected Rectangle GetTextRect(Graphics g)
        {
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            Rectangle imageRect;
            Rectangle textRect;
            Rectangle dropDownArrowRect;

            CalcRectangles(out textRect, out imageRect, out dropDownArrowRect,
                new Rectangle(rect.X + 4, rect.Y + 4, rect.Width - 8, rect.Height - 8), g,
                DropDownMenu != null || IsDrawDropDownArrow);

            return textRect;
        }
		
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			IsMouseOver = true;
			Invalidate();
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			IsMouseOver = false;
			Invalidate();
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			Point pos = this.PointToClient(Cursor.Position);
			if (IsPressed && HotDragDrop && (!this.ClientRectangle.Contains(pos)))
			{
				if (StartDragDrop != null)StartDragDrop(this, EventArgs.Empty);
				IsPressed = false;
				Invalidate();
			}
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (DropDownMenu != null)DropDownMenu.GetContextMenu().Show(this, new Point(0, Height));
			else IsPressed = true;
			Invalidate();
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			lockClick = false;
			IsPressed = false;
			Invalidate();
		}		

		
		protected override void OnClick(EventArgs e)
		{
			if (!lockClick)
			{
				lockClick = true;
				if (DropDownMenu != null)
					DropDownMenu.GetContextMenu().Show(this, new Point(0, Height));
				if (style == ToolBarButtonStyle.PushButton)Pushed = !Pushed;

				base.OnClick(e);				
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);

			UpdateSize();
		}

		protected override void OnSizeChanged(EventArgs e)
		{			
			base.OnSizeChanged(e);	
			UpdateSize();
		}
		#endregion

		#region Events
		/// <summary>
		/// Internal use only
		/// </summary>
		public event EventHandler StartDragDrop;
		#endregion

		#region Constructors
		public StiToolButton()
		{
			
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			//this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Selectable, false);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.Opaque, false);
			
			this.TabStop = false;
			this.Height = 22;
			this.Width = 22;
			this.BackColor = Color.Transparent;
			
		}

		private static Image dropDownArrowBitmap;

		static StiToolButton()
		{
			dropDownArrowBitmap = StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.DropDownArrow.bmp");
		}
		#endregion
	}
}
