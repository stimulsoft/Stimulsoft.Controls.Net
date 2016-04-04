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
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Manages a related set of tab pages
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiTabControl), "Toolbox.StiTabControl.bmp")]
    #if !Profile
	[Designer(typeof(StiTabControlDesigner))]
    #endif
	public class StiTabControl : Panel
	{
		#region Fields
		private int[] widthPages = null;
		private StringFormat sfTabs;
		private int titleHeight = 22;
		private bool locked = false;
		#endregion

		#region Browsable(false)
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

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
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

		#region Properties
		private bool positionAtBottom = false;
		[DefaultValue(false)]
		[Category("Behavior")]
		public virtual bool PositionAtBottom
		{
			get
			{
				return positionAtBottom;
			}
			set
			{
				positionAtBottom = value;
				SetLayout();
				Invalidate();
			}
		}


		private ImageList imageList = null;
		/// <summary>
		/// Gets or sets the collection of images available to the TabPage items.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(null)]
		public virtual ImageList ImageList
		{
			get
			{
				return imageList;
			}
			set
			{
				imageList = value;
			}
		}


		private StiTabPage selectedTab = null;
		/// <summary>
		/// Gets or sets the currently-selected tab page.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StiTabPage SelectedTab
		{
			get
			{
				return selectedTab;
			}
			set
			{
				foreach (StiTabPage page in this.Controls)
				{
					if (page == value)
					{
						page.Dock = DockStyle.Fill;
						page.Show();
					}
				}

				foreach (StiTabPage page in this.Controls)
				{
					if (page != value)page.Hide();
				}

				selectedTab = value;
				//if (selectedTab != null)selectedTab.Invisible = false;
				this.Invalidate();
				InvokeSelectedIndexChanged(this, EventArgs.Empty);
				
			}
		}


		/// <summary>
		/// Gets or sets the tab order of the control within its container.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedIndex
		{
			get
			{
				return this.Controls.IndexOf(this.SelectedTab);
			}
			set
			{
				if (value >= 0 && value < this.Controls.Count)
					SelectedTab = this.Controls[value] as StiTabPage;
			}
		}

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

		#endregion

		#region Events
		#region TitleClick
		/// <summary>
		/// Occurs when the control title is clicked.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler TitleClick;

		protected virtual void OnTitleClick(EventArgs e)
		{

		}

		/// <summary>
		/// Raises the TitleClick event for the specified control.
		/// </summary>
		/// <param name="sender">The Control to assign the TitleClick event to. </param>
		/// <param name="e">An EventArgs that contains the event data. </param>
		public virtual void InvokeTitleClick(object sender, EventArgs e)
		{
			try
			{
				OnTitleClick(EventArgs.Empty);
				if (this.TitleClick != null)this.TitleClick(null, EventArgs.Empty);
			}
			catch
			{
			}
		}

		#endregion

		#region SelectedIndexChanged
		/// <summary>
		/// Occurs when the SelectedIndex property is changed.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler SelectedIndexChanged;

		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{

		}

		/// <summary>
		/// Raises the SelectedIndexChanged event for the specified control.
		/// </summary>
		/// <param name="sender">The Control to assign the SelectedIndexChanged event to. </param>
		/// <param name="e">An EventArgs that contains the event data. </param>
		public virtual void InvokeSelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				OnSelectedIndexChanged(EventArgs.Empty);
				if (SelectedIndexChanged != null)SelectedIndexChanged(sender, e);
			}
			catch
			{
			}			
		}
		#endregion
		#endregion

		#region Methods
		/// <summary>
		/// Disables any updating of the tab control.
		/// </summary>
		public void LockRefresh()
		{
			locked = true;
		}


		/// <summary>
		/// Enables the updating of the tab control.
		/// </summary>
		public void UnlockRefresh()
		{
			locked = false;
		}

		/// <summary>
		/// Calculate width for each page
		/// </summary>
		private void CalculatePagesWidth(Graphics g)
		{
			widthPages = new int[Controls.Count];
			int index = 0;
			foreach (StiTabPage page in Controls)
			{
				widthPages[index++] = GetPageWidth(g, page);
			}
		}


		private int GetPageWidth(Graphics g, StiTabPage page)
		{			
			int imageWidth = 0;
			if (ImageList != null && ImageList.Images.Count > this.Controls.IndexOf(page))
				imageWidth = ImageList.ImageSize.Width;
			if (page.Image != null)imageWidth = page.Image.Width;
			return (int)(g.MeasureString(page.Text, Font, 1000, sfTabs).Width) + 10 + imageWidth;
			
		}

		private int GetPagesWidth(Graphics g)
		{
			int width = 0;
			int index = 0;
			foreach (StiTabPage page in this.Controls)
			{
				if ((!page.Invisible) || DesignMode)width += widthPages[index];
				index++;
			}
			return width;			
		}

		private int GetPanelStartPos()
		{
			return 5;
		}

		private Rectangle GetPageRectangle(Graphics g, StiTabPage page)
		{
			int startPos = GetPanelStartPos();
			int width = 10;
			int allWidth = GetPagesWidth(g);
			double k = 1;
			if (allWidth + startPos * 2 + 5 > Width)
				k = (double)Width / ((double)allWidth + (double)startPos * 2 + 5);

			int index = 0;
			foreach (StiTabPage pg in this.Controls)
			{
				if ((!pg.Invisible) || DesignMode)
				{
					if (pg != page)startPos += (int)(widthPages[index] * k);
					else 
					{
						width = (int)(widthPages[index] * k);
						break;
					}
				}
				index++;
			}

			Rectangle controlRect = Rectangle.Empty;
			
			if (PositionAtBottom)
			{
				controlRect = new Rectangle(startPos, Height - titleHeight - 1, width, titleHeight - 2);
			}
			else
			{
				controlRect = new Rectangle(startPos, 2, width, titleHeight - 2);
			}
			
			return controlRect;
		}


		/// <summary>
		/// Retrieves the tab page control that is located at the specified coordinates.
		/// </summary>
		/// <param name="p">A Point that contains the coordinates where you want to look for a control. 
		/// Coordinates are expressed relative to the upper-left corner of the control client area.</param>
		/// <returns>A tab page control that represents the control that is located at the specified point.</returns>
		public StiTabPage GetTabPageAtPoint(Point p)
		{
			using (Graphics g = Graphics.FromHwnd(this.Handle))
			{
				foreach (StiTabPage page in this.Controls)
				{
					if ((!page.Invisible) || DesignMode)
					{
						Rectangle rect = GetPageRectangle(g, page);
	
						if (rect.Contains(p))
						{
							return page;			
						}
					}
				}
			}
			return null;
		}


		protected virtual void SetLayout()
		{
			this.DockPadding.Left = 
				this.DockPadding.Right = 0;

			if (PositionAtBottom)
			{
				this.DockPadding.Top = 0;
				this.DockPadding.Bottom = titleHeight;
			}
			else
			{
				this.DockPadding.Top = titleHeight;
				this.DockPadding.Bottom = 0;
			}
		}


		private void DrawPage(Graphics g, StiTabPage page)
		{
			if ((!page.Invisible) || DesignMode)
			{
				var rect = GetPageRectangle(g, page);

				if (rect.Width != 0 && rect.Height != 0)
                {
                    #region Calculate Path

                    var pts = new Point [] {};

                    #region Office2013
                    if (ControlStyle == StiControlStyle.Office2013Blue)
                    {
                        pts = new Point[]{
											 new Point(rect.X, rect.Y),
											 new Point(rect.X, rect.Bottom),
											 new Point(rect.Right, rect.Bottom),
											 new Point(rect.Right, rect.Y)};
                    }
                    #endregion

                    #region Flat
                    else
                    {
                        if (!PositionAtBottom)
                        {
                            pts = new Point[]{
											 new Point(rect.X, rect.Y),
											 new Point(rect.X, rect.Bottom),
											 new Point(rect.Right + 5, rect.Bottom),
											 new Point(rect.Right - 5, rect.Y)};
                        }
                        else
                        {
                            pts = new Point[]{
											 new Point(rect.X, rect.Y),
											 new Point(rect.X, rect.Bottom),
											 new Point(rect.Right - 5, rect.Bottom),
											 new Point(rect.Right + 5, rect.Y)};
                        }
                    }
                    #endregion

                    #endregion


                    using (var path = new GraphicsPath(FillMode.Alternate))
                    {
                        path.AddPolygon(pts);

                        #region Draw page header

                        #region Office2013
                        if (ControlStyle == StiControlStyle.Office2013Blue)
                        {
                            if (SelectedTab == page)
                            {
                                using (var pathSelected = new GraphicsPath())
                                {
                                    pathSelected.AddPolygon(new[]{
                                        new Point(pts[0].X - 1, pts[0].Y - 1),
                                        new Point(pts[1].X - 1, pts[1].Y),
                                        new Point(pts[2].X + 1, pts[2].Y),
                                        new Point(pts[3].X + 1, pts[3].Y)});

                                    using (var br = new SolidBrush(Color.White))
                                    {
                                        g.FillPath(br, pathSelected);
                                    }
                                }
                            }
                            else
                            {
                                using (var br = new SolidBrush(Color.White))
                                {
                                    g.FillPath(br, path);
                                }
                            }
                        }
                        #endregion

                        #region Flat
                        else
                        {
                            
                            if (SelectedTab == page)
                            {
                                var tabPage = GetTabPageAtPoint(this.PointToClient(Cursor.Position));
                                if (tabPage == page)
                                {
                                    using (var br = StiBrushes.GetControlLightBrush(rect, 90))
                                    {
                                        g.FillPath(br, path);
                                    }
                                }
                                else
                                {
                                    using (var br = StiBrushes.GetControlBrush(rect, 90))
                                    {
                                        g.FillPath(br, path);
                                    }
                                }
                            }
                            else
                            {
                                var tabPage = GetTabPageAtPoint(this.PointToClient(Cursor.Position));
                                if (tabPage == page)
                                {
                                    using (var br = new SolidBrush(StiColorUtils.Light(StiColors.Content, 15)))
                                    {
                                        g.FillPath(br, path);
                                    }
                                }
                                else
                                {
                                    g.FillPath(StiBrushes.Content, path);
                                }
                            }
                        }
                        #endregion

                        #endregion
                    }


                    Color textColor = ForeColor;
                    if (page != this.SelectedTab) textColor = SystemColors.ControlDark;

					
					#region Paint Image
					SizeF imageSize = SizeF.Empty;
					if (ImageList != null || page.Image != null)
					{
						int pageIndex = this.Controls.IndexOf(page);

						if (ImageList != null || page.Image != null)
						{
							if (page.Image != null)imageSize = page.Image.Size;
							else imageSize = ImageList.ImageSize;

							Rectangle imageRect = new Rectangle(rect.X + 2, 
								(int)(rect.Y + (rect.Height - imageSize.Height) / 2), 
								(int)imageSize.Width, (int)imageSize.Height);

							if (imageSize.Width != 0)
							{
								if (imageList != null && ImageList.Images.Count > pageIndex)
								{
									ImageList.Draw(g, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height, pageIndex);
								}
								else if (page.Image != null)
								{
									g.DrawImage(page.Image, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height);
								}
							}
						}
					}
					#endregion

					#region Paint text
					var textRect = new Rectangle(rect.X + (int)imageSize.Width, rect.Y, rect.Width - (int)imageSize.Width, rect.Height);
					using (var brush = new SolidBrush(textColor))
					{
						g.DrawString(page.Text, Font, brush, textRect, sfTabs);
					}
					#endregion

					#region Draw header lines

                    #region Office2013 Style
                    if (ControlStyle == StiControlStyle.Office2013Blue)
                    {
                        using (var pen = new Pen(StiStyleOffice2013Blue.ColorLineGray))
                        {
                            if (SelectedTab == page)
                            {
                                if (!PositionAtBottom)
                                {
                                    g.DrawLine(pen, new Point(pts[0].X - 1, pts[0].Y - 1), new Point(pts[1].X - 1, pts[1].Y));
                                    g.DrawLine(pen, new Point(pts[0].X - 1, pts[0].Y - 1), new Point(pts[3].X + 1, pts[3].Y - 1));
                                    g.DrawLine(pen, new Point(pts[2].X + 1, pts[2].Y), new Point(pts[3].X + 1, pts[3].Y));
                                }
                                else
                                {
                                    g.DrawLine(pen, pts[1], pts[2]);
                                    g.DrawLine(pen, pts[2], pts[3]);
                                }
                            }
                            else
                            {

                                if (PositionAtBottom)
                                {
                                    g.DrawLine(pen, pts[0], pts[1]);
                                    g.DrawLine(pen, pts[1], pts[2]);
                                    g.DrawLine(pen, pts[2], pts[3]);
                                }
                                else
                                {
                                    g.DrawLine(pen, pts[0], pts[1]);
                                    g.DrawLine(pen, pts[0], pts[3]);
                                    g.DrawLine(pen, pts[2], pts[3]);
                                }

                            }
                        }
                    }
                    #endregion

                    #region Flat Style
                    else
                    {
                        if (SelectedTab == page)
                        {
                            g.DrawLine(SystemPens.ControlLightLight, pts[0], pts[1]);

                            if (!PositionAtBottom)
                            {
                                using (var brush = new LinearGradientBrush(
                                           new Rectangle(pts[3].X, pts[3].Y, pts[2].X - pts[3].X, pts[2].Y - pts[3].Y - 5),
                                           SystemColors.Control, SystemColors.ControlDark, 0f))

                                using (var pen = new Pen(brush))
                                    g.DrawLine(pen, pts[2], pts[3]);

                                g.DrawLine(SystemPens.ControlLightLight, pts[0], pts[3]);
                            }
                            else
                            {
                                g.DrawLine(SystemPens.ControlDark, pts[1], pts[2]);
                                g.DrawLine(SystemPens.ControlDark, pts[2], pts[3]);
                            }
                        }
                        else
                        {
                            using (var pen = new Pen(StiColorUtils.Dark(SystemColors.Control, 40)))
                            {
                                if (PositionAtBottom)
                                {
                                    g.DrawLine(pen, pts[0], pts[1]);
                                    g.DrawLine(pen, pts[1], pts[2]);
                                    g.DrawLine(pen, pts[2], pts[3]);
                                }
                                else
                                {
                                    g.DrawLine(pen, pts[0], pts[1]);
                                    g.DrawLine(pen, pts[0], pts[3]);
                                    g.DrawLine(pen, pts[2], pts[3]);
                                }
                            }
                        }
                    }
                    #endregion

                    #endregion
                }
			}
		}

		#endregion

		#region Handlers
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			StiColors.InitColors();
		}

		protected override bool ProcessMnemonic(char key)
		{
			foreach (StiTabPage page in this.Controls)
			{
				if (IsMnemonic(key, page.Text))
				{
					this.SelectedTab = page;
                
					return true;
				}
			}
                        
			return false;
		}


		protected override void OnPaint(PaintEventArgs p)
		{
			base.OnPaint(p);

			if (!locked && Width != 0 && Height != 0)
			{
				Graphics g = p.Graphics;

				Rectangle clientRect = Rectangle.Empty;
				Rectangle titleRect = Rectangle.Empty;

				if (PositionAtBottom)
				{
					clientRect = new Rectangle(0, 0, Width, Height - titleHeight);
					titleRect = new Rectangle(0, Height - titleHeight, Width, titleHeight);
				}
				else
				{
					clientRect = new Rectangle(0, titleHeight, Width, Height - titleHeight);
					titleRect = new Rectangle(0, 0, Width, titleHeight);
				}

				CalculatePagesWidth(g);

				#region Paint tabControl line
                if (this.ControlStyle == StiControlStyle.Office2013Blue)
                {
                    using (var brush = new SolidBrush(Color.White))
                    {
                        g.FillRectangle(brush, titleRect);
                    }
                }
                else
                {
                    g.FillRectangle(StiBrushes.ContentDark, titleRect);
                }
				
				#endregion
			
				#region Paint tab pages
				int count = 0;
				foreach (StiTabPage tb in Controls)
				{
					if (!tb.Invisible)count++;
				}
				if (DesignMode)count = Controls.Count;

				
				if (count > 0)
				{
					if (SelectedTab == null)SelectedTab = this.Controls[0] as StiTabPage;
					int index = 0;
					
					for (int pageIndex = Controls.Count - 1; pageIndex >= 0; pageIndex--)
					{
						var page = Controls[pageIndex] as StiTabPage;
						if (page != this.SelectedTab && ((!page.Invisible) || DesignMode))DrawPage(g, page);
						index++;
					}

					DrawPage(g, this.SelectedTab);

					Rectangle pageRect = GetPageRectangle(g, this.SelectedTab);
					
					if (PositionAtBottom)
					{
                        if (this.ControlStyle == StiControlStyle.Office2013Blue)
                        {
                            using (var pen = new Pen(StiStyleOffice2013Blue.ColorLineGray))
                            {
                                g.DrawLine(SystemPens.ControlDark, 0, Height - titleHeight, pageRect.X - 1, Height - titleHeight);
                                g.DrawLine(pen, pageRect.Right + 1, Height - titleHeight, this.Width + 1, Height - titleHeight);
                            }
                        }
                        else
                        {
                            g.DrawLine(SystemPens.ControlDark,
                                0, Height - titleHeight, pageRect.X - 1, Height - titleHeight);

                            g.DrawLine(SystemPens.ControlDark,
                                pageRect.Right + 5, Height - titleHeight, this.Width + 1, Height - titleHeight);
                        }
					}
					else 
					{
                        if (this.ControlStyle == StiControlStyle.Office2013Blue)
                        {
                            using (var pen = new Pen(StiStyleOffice2013Blue.ColorLineGray))
                            {
                                g.DrawLine(pen, 0, titleHeight - 1, pageRect.X - 1, titleHeight - 1);
                                g.DrawLine(pen,pageRect.Right + 1, titleHeight - 1, this.Width + 1, titleHeight - 1);
                            }
                        }
                        else
                        {
                            g.DrawLine(SystemPens.ControlLightLight,
                                0, titleHeight - 1, pageRect.X - 1, titleHeight - 1);

                            g.DrawLine(SystemPens.ControlDark,
                                pageRect.Right + 5, titleHeight - 1, this.Width + 1, titleHeight - 1);
                        }
					}

				}
				#endregion
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			Invalidate();
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			Invalidate();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			Invalidate();
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			Point pt = new Point(e.X, e.Y);
			using (Graphics g = Graphics.FromHwnd(this.Handle))
			{
				CalculatePagesWidth(g);
			}
			StiTabPage tabPage = GetTabPageAtPoint(pt);
			if (tabPage != null && tabPage.Enabled)
			{
				SelectedTab = tabPage;
				InvokeTitleClick(this, e);

				if ((e.Button & MouseButtons.Right) > 0 && SelectedTab.TitleContextMenu != null)
				{
					SelectedTab.TitleContextMenu.Show(tabPage, tabPage.PointToClient(Cursor.Position));
				}			
			}
			
			base.OnMouseDown(e);
		}

		#endregion
		
		#region Control Added, removed
		private void PageAdded(object sender, ControlEventArgs e)
		{
			e.Control.Dock = DockStyle.Fill;
			e.Control.CreateGraphics();

			if (Controls.Count == 1)this.SelectedTab = Controls[0] as StiTabPage;
			Invalidate();
		}

		private void PageRemoved(object sender, ControlEventArgs e)
		{
			foreach (StiTabPage page in Controls)
				if (page == SelectedTab)
				{
					Invalidate();
					return;
				}
			
			if (Controls.Count > 0)
			{
				SelectedTab = Controls[0] as StiTabPage;
				Invalidate();
			}
			SelectedTab = null;
			Invalidate();
			
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				sfTabs.Dispose();
			}
			base.Dispose(disposing);
 
		}

		#endregion
		
		#region Constructors
		public StiTabControl()
		{			
			sfTabs = new StringFormat
			{
			    HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show,
			    LineAlignment = StringAlignment.Center,
			    Alignment = StringAlignment.Center,
			    FormatFlags = StringFormatFlags.NoWrap,
			    Trimming = StringTrimming.EllipsisCharacter
			};
		    if (this.RightToLeft == RightToLeft.Yes)
				sfTabs.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);

			this.ControlAdded += new ControlEventHandler(this.PageAdded);
			this.ControlRemoved += new ControlEventHandler(this.PageRemoved);

			SetLayout();
		}
		#endregion
	}
}
