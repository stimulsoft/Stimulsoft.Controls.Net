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
	/// Outlook panel.
	/// </summary>
	[ToolboxItem(false)]
    #if !Profile
	[Designer(typeof(StiOutlookPanelDesigner))]
    #endif
	public class StiOutlookPanel : Panel
	{
		#region Fields
		private Bitmap upBitmap = null;
		private Bitmap downBitmap = null;
		private bool isMouseOver = false;
		#endregion

		#region Browsable(false)
		[Browsable(false)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = value;
			}
		}


		[Browsable(false)]
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


		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ScrollableControl.DockPaddingEdges DockPadding
		{
			get
			{
				return base.DockPadding;
			}
		}
		#endregion
		
		#region Properties
		private bool drawBorder = true;
		/// <summary>
		/// Gets or sets the value, indicates control to draws border.
		/// </summary>
		[DefaultValue(true)]
		[Category("Appearance")]
		public bool DrawBorder
		{
			get
			{
				return drawBorder;
			}
			set
			{
				drawBorder = value;
				Invalidate();
			}
		}


		private Pen borderPen = new Pen(SystemColors.ControlDark);
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Pen BorderPen
		{
			get
			{
				return borderPen;
			}
			set
			{
				borderPen = value;
				Invalidate();
			}
		}


		private int panelDistance = 5;
		[Category("Behavior")]
		public int PanelDistance
		{
			get
			{
				return panelDistance;
			}
			set
			{
				panelDistance = value;
				SetLayout();
				Invalidate();
			}
		}


		private int collapsedHeight = 20;
		[Browsable(false)]
		public virtual int CollapsedHeight
		{
			get
			{
				return collapsedHeight;
			}
			set
			{
				collapsedHeight = value;	
				SetLayout();
				Invalidate();
			}
		}


		private Image headerBackgroundImage = null;
		[DefaultValue(null)]
		[Category("Appearance")]
		public virtual Image HeaderBackgroundImage
		{
			get
			{
				return headerBackgroundImage;
			}
			set
			{
				headerBackgroundImage = value;
				Invalidate();
			}
		}
 

		private int headerHeight = 20;
		[DefaultValue(20)]
		[Category("Appearance")]
		public virtual int HeaderHeight
		{
			get
			{
				return headerHeight;
			}
			set
			{
				headerHeight = value;	
				SetLayout();
				Invalidate();
			}
		}


		private Font titleFont = new Font("Arial", 8);
		[Category("Appearance")]
		public virtual Font TitleFont
		{
			get
			{
				return titleFont;
			}
			set
			{
				if (titleFont != value)
				{
					titleFont = value; 
					Invalidate();
				}
			}
		}


		private Color titleColor = Color.Empty;
		[Category("Appearance")]
		public virtual Color TitleColor
		{
			get
			{
				return titleColor;
			}
			set
			{
				if (titleColor != value)
				{
					titleColor = value; 
					Invalidate();
				}
			}
		}


		private Image image = null;
		[DefaultValue(null)]
		[Category("Appearance")]
		public virtual Image Image
		{
			get
			{
				return image;
			}
			set
			{
				if (value is Bitmap)StiImageUtils.MakeImageBackgroundAlphaZero(value as Bitmap);
				image = value;				
				Invalidate();
			}
		}


		/// <summary>
		/// Gets or sets values, indicates where the panel is selected.
		/// </summary>
		[Category("Behavior")]
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
			}
		}


		private bool selected = false;
		/// <summary>
		/// Gets or sets values, indicates where the panel is selected.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]
		public bool Selected
		{
			get
			{
				return selected;
			}
			set
			{
				selected = value;

				if (value && Parent != null)
				{
					foreach (Control control in Parent.Controls)
					{
						if (control is StiOutlookPanel && control != this)
						{
							if (((StiOutlookPanel)control).Selected)
							{
								((StiOutlookPanel)control).Selected = false;
								((StiOutlookPanel)control).Invalidate();
							}
						}
					}
				}
				Invalidate();
			}
		}


		private bool collapsed = true;
		/// <summary>
		/// Gets a value indicating whether the panel is in the collapsed state.
		/// </summary>
		[Category("Behavior")]
		[Browsable(false)]
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
					if (!value)
					{
						CollapsedHeight = this.Height;
						this.Height = HeaderHeight + PanelDistance;
					}
					else
					{
						this.Height = CollapsedHeight;
					}
				}
				collapsed = value;
				SetLayout();
				Invalidate();
			}
		}

        [Category("Behavior")]
        [DefaultValue(true)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;

                SetLayout();

                Invalidate();
            }
        }


		private Color headerStartColor;
		[Category("Behavior")]
		[Browsable(true)]
		public Color HeaderStartColor
		{
			get
			{
				return headerStartColor;
			}
			set
			{
				headerStartColor = value;
				Invalidate();
			}
		}


		private Color headerEndColor;
		[Category("Behavior")]
		[Browsable(true)]
		public Color HeaderEndColor
		{
			get
			{
				return headerEndColor;
			}
			set
			{
				headerEndColor = value;
				Invalidate();
			}
		}


		private Color selectedHeaderStartColor;
		[Category("Behavior")]
		[Browsable(true)]
		public Color SelectedHeaderStartColor
		{
			get
			{
				return selectedHeaderStartColor;
			}
			set
			{
				selectedHeaderStartColor = value;
				Invalidate();
			}
		}


		private Color selectedHeaderEndColor;
		[Category("Behavior")]
		[Browsable(true)]
		public Color SelectedHeaderEndColor
		{
			get
			{
				return selectedHeaderEndColor;
			}
			set
			{
				selectedHeaderEndColor = value;
				Invalidate();
			}
		}
		#endregion

		#region Methods
		private void CheckHeight()
		{
			if (!Collapsed)
			{
				Height = HeaderHeight + 2;
				return;
			}
		
			int maxHeight = HeaderHeight + 50;
			if (Controls.Count > 0)
			{
				maxHeight = HeaderHeight + 10;

				foreach (Control control in Controls)
				{
					maxHeight = Math.Max(control.Bottom + 10, maxHeight);
				}
			}

			Height = maxHeight;
		}

		private void SetLayout()
		{
			try
			{
				if (AutoSize)CheckHeight();

				if (!Collapsed)this.DockPadding.Bottom = 1000;
				else this.DockPadding.Bottom = PanelDistance + 1;

				this.DockPadding.Top = HeaderHeight + 2;
			}
			catch
			{
			}
		}

		private void CheckMouseOver()
		{
			bool over = this.RectangleToScreen(GetHeaderRect()).Contains(Cursor.Position);
			if (over != isMouseOver)
			{
				isMouseOver = over;
				Invalidate(GetHeaderRect());
			}
		}

		private Rectangle GetHeaderRect()
		{
			return new Rectangle(0, 0, Width - 1, HeaderHeight);
		}
		#endregion

		#region Handlers
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			StiColors.InitColors();
		}

		
		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			SetLayout();
		}
		
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);
			SetLayout();
		}
		
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			CheckMouseOver();
		}

		
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			CheckMouseOver();
		}

		
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			CheckMouseOver();
		}

		
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			Collapsed = ! Collapsed;
			Invalidate();
		}

		
		protected override void OnPaintBackground(PaintEventArgs p)
		{
			var fillColor = SystemColors.Control;
			if (Parent != null)fillColor = Parent.BackColor;

			var g = p.Graphics;
			using (var brush = new SolidBrush(fillColor))
			{
				g.FillRectangle(brush, p.ClipRectangle); 
			}
		}

		
		protected override void OnPaint(PaintEventArgs p)
		{
			var g = p.Graphics;
			
			#region Content
			var contentRect = new Rectangle(0, HeaderHeight, Width, Height - HeaderHeight - PanelDistance);

			if (BackgroundImage == null)
			{
				using (var brush = new SolidBrush(BackColor))
				{
					g.FillRectangle(brush, contentRect.Left, contentRect.Top, contentRect.Width + 1, contentRect.Height + 1);
				}
			}
			else
			{
				StiControlPaint.DrawImageBackground(p.Graphics, BackgroundImage, contentRect);
			}

		    if (DrawBorder)
		    {
		        g.DrawRectangle(BorderPen, 0, 0, Width - 1, Height - PanelDistance);
		    }
			#endregion
		
			#region Header
			var headerRect = GetHeaderRect();
			var image = !Collapsed ? upBitmap : downBitmap;

			Color textColor = Color.Black;

			#region Fill rectangle
			headerRect.Width ++;
			headerRect.Height ++;
			if (HeaderBackgroundImage == null)
			{				
				if (Selected)
				{
					if (isMouseOver)
					{
						using (var brush = new LinearGradientBrush(headerRect, 
								   StiColorUtils.Light(SelectedHeaderStartColor, 20), 
								   StiColorUtils.Light(SelectedHeaderEndColor, 20), 90))
							g.FillRectangle(brush, headerRect);
					}
					else 
					{					
						using (var brush = new LinearGradientBrush(headerRect, 
								   SelectedHeaderStartColor, 
								   SelectedHeaderEndColor, 90))
							g.FillRectangle(brush, headerRect);
					}
					textColor = SystemColors.ActiveCaptionText;
				}
				else
				{
					if (isMouseOver)
					{
						using (var brush = new LinearGradientBrush(headerRect, 
								   StiColorUtils.Light(HeaderStartColor, 20), 
								   StiColorUtils.Light(HeaderEndColor, 20), 90))
							g.FillRectangle(brush, headerRect);
					}
					else 
					{					
						using (var brush = new LinearGradientBrush(headerRect, HeaderStartColor, HeaderEndColor, 90))
							g.FillRectangle(brush, headerRect);
					}
				}
			}
			else
			{
				StiControlPaint.DrawImageBackground(p.Graphics, HeaderBackgroundImage, headerRect);
			}
			headerRect.Width --;
			headerRect.Height --;
			#endregion

			#region Draw button image
			if (image != null)
			{
				var imageRect = new Rectangle(headerRect.Width - 18, (headerRect.Height - 16) / 2, 16, 16);

				if (textColor != Color.Black && image != null)
					image = StiImageUtils.ReplaceImageColor((Bitmap)image, textColor, Color.Black);
				g.DrawImage(image, imageRect);
			}
			#endregion

			#region Draw image
			int imageWidth = 0;
			if (Image != null)
			{
				var imageRect = new Rectangle(headerRect.X + 4, 
					(headerRect.Height - Image.Size.Height) / 2, Image.Size.Width, Image.Size.Height);

				imageWidth = imageRect.Width + 2;

				g.DrawImage(Image, imageRect);
			}
			#endregion

			#region Draw header text
			var textRect = new Rectangle(5 + imageWidth, 0, headerRect.Width - 25 - imageWidth, headerRect.Height);

			if (textRect.Width > 0)
			{
				using (var sf = new StringFormat())
				{
					sf.LineAlignment = StringAlignment.Center;
					if (RightToLeft == RightToLeft.Yes)
						sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
				
					sf.FormatFlags = StringFormatFlags.NoWrap;
					sf.Trimming = StringTrimming.EllipsisCharacter;

					sf.HotkeyPrefix = HotkeyPrefix.Hide;

					if (!TitleColor.IsEmpty)textColor = TitleColor;
					using (Brush brush = new SolidBrush(textColor))
					{
						g.DrawString(Text, TitleFont, brush, textRect, sf);
					}
				}
			}
			#endregion

			if (DrawBorder)g.DrawRectangle(BorderPen, headerRect);
			#endregion
		}
		

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (upBitmap != null)upBitmap.Dispose();
				if (downBitmap != null)downBitmap.Dispose();
			}
			base.Dispose(disposing);
 
		}
		#endregion

		#region Constructors
		public StiOutlookPanel()
		{
			BackColor = StiColors.Content;
			upBitmap = StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.Up.bmp");
			downBitmap = StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.Down.bmp");

			headerStartColor = StiColors.ControlStart;
			headerEndColor = StiColors.ControlEnd;
			selectedHeaderStartColor = StiColors.ActiveCaptionStart;
			selectedHeaderEndColor = StiColors.ActiveCaptionEnd;
			
			this.DockPadding.Left = 			
				this.DockPadding.Right = 2;

			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.Selectable, true);
			
			this.Height = 50;
			this.Text = "outlookPanel";

			SetLayout();
		}
		#endregion
	}
}
