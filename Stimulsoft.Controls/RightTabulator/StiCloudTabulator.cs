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
	[ToolboxItem(false)]
    public class StiCloudTabulator : StiScrollableControl
	{
		#region Fields
        internal StiCloudMode Mode;

	    internal int MaxTitleWidth = 0;
	    internal int MaxTitleHeight = 0;
        internal StiCloudTabulatorPage MouseOverPage = null;

	    private bool LockUpdateTitle;
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
		#endregion

		#region Properties
		[Browsable(false)]
		internal bool IsDesignMode
		{
			get
			{
				return DesignMode;
			}
		}
		

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Color BackColor
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

        private Color titleBackColor = Color.FromArgb(255, 250, 250, 250);
		public Color TitleBackColor
		{
			get
			{
				return titleBackColor;
			}
			set
			{
				titleBackColor = value;
				UpdateTitle();
			}
		}		


		private StiCloudTabulatorPage selectedTab = null;
		/// <summary>
		/// Gets or sets the currently-selected tab page.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StiCloudTabulatorPage SelectedTab
		{
			get
			{
				return selectedTab;
			}
			set
			{
			    this.MouseOverPage = null;
                for (int index = 0; index < this.Controls.Count; index++)
				{
                    var page = this.Controls[index] as StiCloudTabulatorPage;
				    if (page == null) continue;

					if (page == value)
					{
						try
						{
							page.Dock = DockStyle.Fill;
							page.Show();
								
							break;
						}
						catch
						{
						}
					}
				}

                for (int index = 0; index < this.Controls.Count; index++)
				{
                    var page = this.Controls[index] as StiCloudTabulatorPage;
                    if (page != null && page != value)
                        page.Hide();
				}

				if (selectedTab != value)
				{
					selectedTab = value;
					this.Invalidate();
					InvokeSelectedIndexChanged(this, EventArgs.Empty);
				}				
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
			    int selectedIndex = 0;
                for (int index = 0; index < this.Controls.Count; index++)
			    {
                    var page = this.Controls[index] as StiCloudTabulatorPage;
			        if (page == null)
			            continue;

			        if (page == selectedTab)
			            return selectedIndex;

			        selectedIndex++;
			    }

				return this.Controls.IndexOf(this.SelectedTab);
			}
			set
			{
			    if (value >= 0 && value < this.Controls.Count)
			    {
                    int selectedIndex = 0;
                    for (int index = 0; index < this.Controls.Count; index++)
                    {
                        var page = this.Controls[index] as StiCloudTabulatorPage;
                        if (page == null)
                            continue;

                        if (selectedIndex == value)
                        {
                            SelectedTab = page;
                            return;
                        }

                        selectedIndex++;
                    }
			    }
			}
		}

		#endregion

		#region Events

		#region SelectedIndexChanged
		/// <summary>
		/// Occurs when the SelectedIndex property is changed.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler SelectedIndexChanged;
		private void InvokeSelectedIndexChanged(object sender, EventArgs e)
		{
			if (SelectedIndexChanged != null)SelectedIndexChanged(sender, e);
		}
		#endregion

		#endregion

		#region Methods

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0)
                this.VScrollBar.DoUp();
            else
                this.VScrollBar.DoDown();
        }

        public StiCloudTabulatorPage GetTabPageAtPoint(Point p)
		{
            for (int index = 0; index < this.Controls.Count; index++)
            {
                var page = this.Controls[index] as StiCloudTabulatorPage;
                if (page != null && (!page.Invisible) || DesignMode)
				{
                    var rect = new Rectangle(page.TitleRect.X, page.TitleRect.Y, page.TitleRect.Width, page.TitleRect.Height);
				    if (ShowVerticalScroll)
                        rect.Y -= this.VScrollBar.Value;
	
					if (rect.Contains(p))
					{
						return page;			
					}
				}
			}

			return null;
		}

		private Size GetMaxPageTitleSize()
		{
			int maxWidth = 0;
			int maxHeight = 0;
            for (int index = 0; index < this.Controls.Count; index++)
			{
                var page = this.Controls[index] as StiCloudTabulatorPage;
			    if (page == null) continue;

				Size textSize = page.TitleSize;
				maxWidth = Math.Max(textSize.Width, maxWidth);
				maxHeight = Math.Max(textSize.Height, maxHeight);
			}		

			return new Size(maxWidth, maxHeight);
		}

	    private int GetCountVisiblePages()
	    {
	        int count = 0;
            for (int index = 0; index < this.Controls.Count; index++)
            {
                var page = this.Controls[index] as StiCloudTabulatorPage;
                if (page != null && !page.Invisible)
                    count++;
            }

	        return count;
	    }

		internal void UpdateTitle()
		{
            if (LockUpdateTitle)
                return;

			MeasureTitles();

			Size maxSize = GetMaxPageTitleSize();
			MaxTitleWidth = maxSize.Width;
			MaxTitleHeight = maxSize.Height;

            CheckScrollBar();
		}

		private void MeasureTitles()
		{
		    if (this.Controls.Count == 0) 
                return;

		    using (var g = Graphics.FromHwnd(this.Handle))
		    {
		        for(int index = 0; index < this.Controls.Count; index++)
		        {
                    var page = this.Controls[index] as StiCloudTabulatorPage;

		            if (page != null)
		            {
		                page.TitleSize = this.Mode.MeasureTitle(g, page);
		            }
		        }
		    }
		}

	    private void ApplyTitleSize()
	    {
            int left = this.Width - this.MaxTitleWidth;
            int width = this.MaxTitleWidth;
	        int top = 0;

            if (ShowVerticalScroll)
            {
                left -= CloudScrollWidth + 6;
                width += CloudScrollWidth;
            }

            for (int index = 0; index < this.Controls.Count; index++)
            {
                var page = this.Controls[index] as StiCloudTabulatorPage;
                if (page == null || page.Invisible) continue;

                page.TitleRect = new Rectangle(left, top, width, MaxTitleHeight);
                top += MaxTitleHeight;
            }
	    }

	    #endregion

		#region Handlers

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

		    var pos = Parent.PointToClient(Cursor.Position);
            for (int index = 0; index < this.Controls.Count; index++)
            {
                var page = this.Controls[index] as StiCloudTabulatorPage;
                if (page == null || page.Invisible)
                    continue;

                var rect = new Rectangle(page.TitleRect.X, page.TitleRect.Y, page.TitleRect.Width, page.TitleRect.Height);
                if (this.ShowVerticalScroll)
                    rect.Y -= this.VScrollBar.Value;

                if (rect.Contains(pos))
                {
                    if (MouseOverPage != page)
                    {
                        MouseOverPage = page;
                        Invalidate();
                    }
                    return;
                }
            }

            if (MouseOverPage != null)
		    {
                MouseOverPage = null;
                Invalidate();
		    }
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

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			UpdateTitle();
		}

		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			StiColors.InitColors();
			BackColor = StiColorUtils.Light(SystemColors.Control, 10);
		}

		protected override bool ProcessMnemonic(char key)
		{
            for (int index = 0; index < this.Controls.Count; index++)
			{
                var page = this.Controls[index] as StiCloudTabulatorPage;
			    if (page == null)
			        continue;

				if (IsMnemonic(key, page.Text))
				{
					this.SelectedTab = page;
                
					return true;
				}
			}
                        
			return false;
		}

		protected override void OnPaintBackground(PaintEventArgs p)
		{

		}

		protected override void OnPaint(PaintEventArgs p)
		{
		    if (LockUpdateTitle)
		    {
		        LockUpdateTitle = false;
                UpdateTitle();
		    }

		    var g = p.Graphics;
		    Rectangle rect;
            if (this.Controls.Count > 0)
            {
                int left = this.Width - this.MaxTitleWidth - 2 * Mode.titleSpace;
                int width = this.MaxTitleWidth + 2*Mode.titleSpace;

                if (ShowVerticalScroll)
                {
                    left -= CloudScrollWidth + 4;
                    width += CloudScrollWidth + 4;
                }

                rect = new Rectangle(left, 0, width, this.Height);

                if (Width != 0 && Height != 0)
                {
                    #region Fill Title
                    g.FillRectangle(Brushes.White, rect);
                    #endregion

                    #region Paint tab pages

                    if (selectedTab == null)
                    {
                        for (int index = 0; index < this.Controls.Count; index++)
                        {
                            var page = this.Controls[index] as StiCloudTabulatorPage;
                            if (page == null) continue;

                            this.SelectedTab = page;
                            return;
                        }
                    }

                    for (int index = 0; index < Controls.Count; index++)
                    {
                        var page = this.Controls[index] as StiCloudTabulatorPage;
                        if (page == null) continue;

                        if (!page.Invisible || DesignMode)
                            DrawPageTitle(g, page);
                    }
                    #endregion
                }
            }
            else
            {
                rect = new Rectangle(0, 0, this.Width, this.Height);

                if (Width != 0 && Height != 0)
                {
                    #region Fill Title
                    g.FillRectangle(Brushes.White, rect);
                    #endregion
                }
            }
		}

        protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			var pt = new Point(e.X, e.Y);

            var tabPage = GetTabPageAtPoint(pt);
            if (tabPage != null)
            {
                if ((e.Button & MouseButtons.Left) > 0)
                {
                    SelectedTab = tabPage;
                }
            }
			
			base.OnMouseDown(e);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

            if (Mode == null)
                return;
			UpdateTitle();
        }

        #endregion

        #region Methods.Draw
        private void DrawPageTitle(Graphics g, StiCloudTabulatorPage page)
        {
            var rect = new Rectangle(page.TitleRect.X, page.TitleRect.Y, page.TitleRect.Width, page.TitleRect.Height);
            if (this.ShowVerticalScroll)
            {
                rect.Y -= this.VScrollBar.Value;
            }

            if (rect.Width != 0 && rect.Height != 0)
            {
                var contentRect = new Rectangle(rect.X, rect.Y + 3, rect.Width, rect.Height);

                if (page != SelectedTab)
                {
                    if (page == this.MouseOverPage)
                    {
                        using (var br = new SolidBrush(StiStyleOffice2013Blue.ColorButtonButtonMouseOver))
                        {
                            g.FillRectangle(br, contentRect);
                        }
                    }
                    else
                    {
                        using (var br = new SolidBrush(Color.White))
                        {
                            g.FillRectangle(br, contentRect);
                        }
                    }
                }
                else
                {
                    using (var br = new SolidBrush(StiStyleOffice2013Blue.ColorButtonChecked))
                    {
                        g.FillRectangle(br, contentRect);
                    }
                }

                this.Mode.DrawTitleText(g, rect, page);
                this.Mode.DrawTitleImage(g, rect, page);
            }
        }
        #endregion

        #region Vertical Scroll

        internal const int CloudScrollWidth = 22;
	    private int FullPagesHeight = 0;
        internal bool ShowVerticalScroll = false;

	    internal void CheckScrollBar()
	    {
            if (LockUpdateTitle)
                return;

            this.FullPagesHeight = MaxTitleHeight * GetCountVisiblePages();
	        if (FullPagesHeight > this.Height)
	        {
	            this.ScrollHeight = FullPagesHeight;
	            this.ShowVerticalScroll = true;
	        }
	        else
	        {
	            this.ScrollHeight = 0;
                this.ShowVerticalScroll = false;
	        }

            int right = this.MaxTitleWidth + Mode.titleSpace;
            if (ShowVerticalScroll)
            {
                right += CloudScrollWidth + 6;
            }
            this.DockPadding.Right = right;

            ApplyTitleSize();
            Invalidate();

            if (SelectedTab != null)
                SelectedTab.Invalidate();
	    }

        #endregion

        #region Control Added, removed

        private void PageAdded(object sender, ControlEventArgs e)
		{
			e.Control.Dock = DockStyle.Fill;
			e.Control.CreateGraphics();

            // Т.к. он наследуется от StiScrollableControl, то он добавляет 3 контрола, только потом идут страницы
            if (Controls.Count == 4)
            {
                this.SelectedTab = Controls[3] as StiCloudTabulatorPage;
            }

			UpdateTitle();
		}

		private void PageRemoved(object sender, ControlEventArgs e)
		{
            for (int index = 0; index < this.Controls.Count; index++)
            {
                var page = this.Controls[index] as StiCloudTabulatorPage;
                if (page == null) continue;

		        if (page == SelectedTab)
		        {
		            Invalidate();
		            return;
		        }
		    }

            // Т.к. он наследуется от StiScrollableControl, то он добавляет 3 контрола, только потом идут страницы
		    if (this.Controls.Count > 3)
			{
                SelectedTab = this.Controls[4] as StiCloudTabulatorPage;
				Invalidate();
			}

			SelectedTab = null;
			UpdateTitle();
		}

		#endregion
		
        public StiCloudTabulator()
        {
            this.Mode = new StiCloudMode(this);

            this.DockPadding.Left = 0;
            this.DockPadding.Top = 0;
            this.DockPadding.Bottom = 0;
            this.DockPadding.Right = 110;

            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);

            this.ControlAdded += this.PageAdded;
            this.ControlRemoved += this.PageRemoved;

            LockUpdateTitle = true;
        }
    }
}