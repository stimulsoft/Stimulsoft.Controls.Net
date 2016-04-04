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
	#region StiTabTitlePosition
	public enum StiTabTitlePosition
	{
		LeftHorizontal,
		TopHorizontal,
        RightHorizontal
	}
	#endregion
    
	[ToolboxItem(false)]
	//[ToolboxBitmap(typeof(StiTabulator), "Toolbox.StiTabulator.bmp")]
    #if !Profile
	[Designer(typeof(StiTabulatorDesigner))]
    #endif
	public class StiTabulator : ContainerControl
	{
		#region Fields
		internal StringFormat sfTabs;
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


		private StiTabTitlePosition tabTitlePosition = StiTabTitlePosition.TopHorizontal;
		[DefaultValue(StiTabTitlePosition.TopHorizontal)]
		public StiTabTitlePosition TabTitlePosition
		{
			get
			{
				return tabTitlePosition;
			}
			set
			{
				tabTitlePosition = value;

				switch (value)
				{
					case StiTabTitlePosition.LeftHorizontal:
						Mode = new StiLeftHorizontalMode(this);
						break;

					case StiTabTitlePosition.TopHorizontal:
						Mode = new StiTopHorizontalMode(this);
						break;

                    case StiTabTitlePosition.RightHorizontal:
                        Mode = new StiRightHorizontalMode(this);
                        break;
				}
			}
		}


		private StiTabulatorMode mode;
		[Browsable(false)]
		public StiTabulatorMode Mode
		{
			get
			{
				return mode;
			}
			set
			{
				mode = value;
				UpdateTitle();
				Invalidate();
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

				switch (value)
				{
					case StiControlStyle.Flat:
						Style = new StiTabulatorStyle(this);
						break;

					case StiControlStyle.Office2013Blue:
                        Style = new StiTabulatorStyleOffice2013Blue(this);
						break;
				}
				
			}
		}


		private bool showTitle = true;
		[DefaultValue(true)]
		public bool ShowTitle
		{
			get
			{
				return showTitle;
			}
			set
			{
				showTitle = value;	
				UpdateTitle();
				Invalidate();
			}
		}


        private IStiTabulatorStyle style;
		[Browsable(false)]
        public IStiTabulatorStyle Style
		{
			get
			{
				return style;
			}
			set
			{
				style = value;
				UpdateTitle();
				Invalidate();
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


		private int maxTitleWidth = 0;
		[Browsable(false)]
		internal int MaxTitleWidth
		{
			get
			{
				return maxTitleWidth;
			}
			set
			{
                maxTitleWidth = value;
			}
		}


		private int maxTitleHeight = 0;
		[Browsable(false)]
		internal int MaxTitleHeight
		{
			get
			{
				return maxTitleHeight;
			}
			set
			{
				maxTitleHeight = value;
			}
		}


		private int titleWidth = 0;
		[DefaultValue(0)]
		public int TitleWidth
		{
			get
			{
				return titleWidth;
			}
			set
			{
                titleWidth = value;
				UpdateTitle();
				Invalidate();
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
				Invalidate();
			}
		}		
	

		private int titleHeight = 0;
		[DefaultValue(0)]
		public int TitleHeight
		{
			get
			{
				return titleHeight;
			}
			set
			{
				titleHeight = value;
				UpdateTitle();
				Invalidate();
			}
		}


		private StiTabulatorPage selectedTab = null;
		/// <summary>
		/// Gets or sets the currently-selected tab page.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StiTabulatorPage SelectedTab
		{
			get
			{
				return selectedTab;
			}
			set
			{				
				foreach (StiTabulatorPage page in this.Controls)
				{
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

				foreach (StiTabulatorPage page in this.Controls)
				{
					if (page != value)page.Hide();
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
				return this.Controls.IndexOf(this.SelectedTab);
			}
			set
			{
				if (value >= 0 && value < this.Controls.Count)
					SelectedTab = this.Controls[value] as StiTabulatorPage;
			}
		}


		private ImageList imageList = null;
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

				UpdateTitle();
				Invalidate();
			}
		}

		#endregion

		#region Events
		#region TitleMouseUp
		/// <summary>
		/// Occurs when the mouse up event occurs on title.
		/// </summary>
		[Category("Behavior")]
		public event MouseEventHandler TitleMouseUp;

		protected virtual void OnTitleMouseUp(MouseEventArgs e)
		{

		}

		/// <summary>
		/// Raises the TitleMouseUp event for the specified control.
		/// </summary>
		/// <param name="sender">The Control to assign the TitleMouseUp event to. </param>
		/// <param name="e">An EventArgs that contains the event data. </param>
		public virtual void InvokeTitleMouseUp(object sender, MouseEventArgs e)
		{
			try
			{
				OnTitleMouseUp(e);
				if (this.TitleMouseUp != null)this.TitleMouseUp(sender, e);
			}
			catch
			{
			}
		}
		#endregion

		#region TitleMouseMove
		/// <summary>
		/// Occurs when the mouse move event occurs on title.
		/// </summary>
		[Category("Behavior")]
		public event MouseEventHandler TitleMouseMove;

		protected virtual void OnTitleMouseMove(MouseEventArgs e)
		{

		}

		/// <summary>
		/// Raises the TitleMouseMove event for the specified control.
		/// </summary>
		/// <param name="sender">The Control to assign the TitleMouseMove event to. </param>
		/// <param name="e">An EventArgs that contains the event data. </param>
		public virtual void InvokeTitleMouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				OnTitleMouseMove(e);
				if (this.TitleMouseMove != null)this.TitleMouseMove(sender, e);
			}
			catch
			{
			}
		}
		#endregion

		#region TitleMouseDown
		/// <summary>
		/// Occurs when the mouse down event occurs on title.
		/// </summary>
		[Category("Behavior")]
		public event MouseEventHandler TitleMouseDown;

		protected virtual void OnTitleMouseDown(MouseEventArgs e)
		{

		}

		/// <summary>
		/// Raises the TitleMouseDown event for the specified control.
		/// </summary>
		/// <param name="sender">The Control to assign the TitleMouseDown event to. </param>
		/// <param name="e">An EventArgs that contains the event data. </param>
		public virtual void InvokeTitleMouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				OnTitleMouseDown(e);
				if (this.TitleMouseDown != null)this.TitleMouseDown(sender, e);
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
			OnSelectedIndexChanged(EventArgs.Empty);
			if (SelectedIndexChanged != null)SelectedIndexChanged(sender, e);
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


		internal Rectangle GetTitlePageRectangle(Graphics g, StiTabulatorPage page)
		{			
			return Mode.GetTitlePageRectangle(page);
		}

		public StiTabulatorPage GetTabPageAtPoint(Point p)
		{
			using (var g = Graphics.FromHwnd(this.Handle))
			{
				foreach (StiTabulatorPage page in this.Controls)
				{
					if ((!page.Invisible) || DesignMode)
					{
						Rectangle rect = GetTitlePageRectangle(g, page);
	
						if (rect.Contains(p))
						{
							return page;			
						}
					}
				}
			}
			return null;
		}

		private Size GetMaxPageTitleSize()
		{
			int maxWidth = 0;
			int maxHeight = 0;
			foreach (StiTabulatorPage page in Controls)
			{
				Size textSize = page.TitleSize;
				maxWidth = Math.Max(textSize.Width, maxWidth);
				maxHeight = Math.Max(textSize.Height, maxHeight);
			}		

			return new Size(maxWidth, maxHeight);
		}

		internal void UpdateTitle()
		{
			MeasureTitles();
			Size maxSize = GetMaxPageTitleSize();
			MaxTitleWidth = maxSize.Width;
			MaxTitleHeight = maxSize.Height;
			SetLayout();
			Invalidate();
			if (SelectedTab != null)SelectedTab.Invalidate();
		}

		private void MeasureTitles()
		{
			Mode.MeasureTitles();
		}

		private void SetLayout()
		{
			Mode.SetLayout();
		}

		private void DrawPageTitle(Graphics g, StiTabulatorPage page)
		{
			if ((!page.Invisible) || DesignMode)
			{
				Mode.DrawPageTitle(g, page);
			}
		}

		#endregion

		#region Handlers
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			var pt = new Point(e.X, e.Y);
            
			var tabPage = GetTabPageAtPoint(pt);
			if (tabPage != null)
			{
				InvokeTitleMouseMove(this, e);
			}

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

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			MeasureTitles();
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
			foreach (StiTabulatorPage page in this.Controls)
			{
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
		    if (locked) return;

		    var g = p.Graphics;
		    var rect = Mode.GetTitleRectangle();			

		    if (Width != 0 && Height != 0)
		    {
		        #region Fill Title
		        g.FillRectangle(Brushes.White, rect);
		        #endregion

		        #region Paint tab pages
		        int count = 0;
		        int sumWidth = 0;
		        int countInvisiblePages = 0;
		        foreach (StiTabulatorPage tb in Controls)
		        {
		            if (!tb.Invisible)count++;
		            if (!tb.Invisible) sumWidth += tb.TitleSize.Width;
		            else
		                countInvisiblePages++;
		        }

		        #region ScrollButton
                if (tabTitlePosition == StiTabTitlePosition.TopHorizontal)
                {
                    if (sumWidth < this.Width && countInvisiblePages > 0)
                    {
                        VisibleRightScrollButton = false;
                        VisibleLeftScrollButton = true;
                        mode.startPosTitle = 18;
                        ScrollButtonVisible = true;
                        ResizeScrollButton();
                    }
                    else
                    {
                        if (sumWidth > this.Width && countInvisiblePages == 0)
                        {
                            VisibleRightScrollButton = true;
                            VisibleLeftScrollButton = false;
                            mode.startPosTitle = 8;
                            ScrollButtonVisible = true;
                            ResizeScrollButton();
                        }
                        else
                        {
                            if (sumWidth < this.Width)
                            {
                                mode.startPosTitle = 8;
                                ScrollButtonVisible = false;
                                VisiblePage();
                            }
                            else
                            {
                                VisibleRightScrollButton = true;
                                VisibleLeftScrollButton = true;
                                mode.startPosTitle = 18;
                                ScrollButtonVisible = true;
                                ResizeScrollButton();
                            }
                        }
                    }
                }
		        #endregion

		        if (DesignMode)
                    count = Controls.Count;

                if (count > 0)
                {
                    if (selectedTab == null)
                        this.SelectedTab = this.Controls[0] as StiTabulatorPage;

                    for (int index = Controls.Count - 1; index >= 0; index--)
                    {
                        var page = (StiTabulatorPage)Controls[index];
                        if (page != this.selectedTab && ((!page.Invisible) || DesignMode))
                            DrawPageTitle(g, page);
                    }

                    DrawPageTitle(g, this.selectedTab);
                }
		        #endregion
		    }

		    #region Paint scroll button 
            if (tabTitlePosition == StiTabTitlePosition.TopHorizontal)
            {
                if (ScrollButtonVisible)
                {
                    if (VisibleRightScrollButton)
                        if (stateRight)
                            ControlPaint.DrawScrollButton(g, RightRect, ScrollButton.Right, ButtonState.Checked);
                        else
                            ControlPaint.DrawScrollButton(g, RightRect, ScrollButton.Right, ButtonState.Normal);

                    if (VisibleLeftScrollButton)
                        if (stateLeft)
                            ControlPaint.DrawScrollButton(g, LeftRect, ScrollButton.Left, ButtonState.Checked);
                        else
                            ControlPaint.DrawScrollButton(g, LeftRect, ScrollButton.Left, ButtonState.Normal);
                }
            }
		    #endregion
		}

        protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
            UpScrollButton();
			Point pt = new Point(e.X, e.Y);

			var tabPage = GetTabPageAtPoint(pt);
			if (tabPage != null)
			{
				InvokeTitleMouseUp(this, e);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			Point pt = new Point(e.X, e.Y);

            if (!GetScrollButtonAtPoint(pt))
            {
                var tabPage = GetTabPageAtPoint(pt);
                if (tabPage != null)
                {
                    InvokeTitleMouseDown(this, e);
                    if ((e.Button & MouseButtons.Left) > 0)
                    {
                        SelectedTab = tabPage;
                    }

                    if ((e.Button & MouseButtons.Right) > 0 && SelectedTab.TitleContextMenu != null)
                    {
                        SelectedTab.TitleContextMenu.Show(tabPage, tabPage.PointToClient(Cursor.Position));
                    }
                }
            }
			
			base.OnMouseDown(e);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			MeasureTitles();
			UpdateTitle();

        }
        #endregion

        #region Scroll Button

        #region Fields
        private Rectangle RightRect;
        private Rectangle LeftRect;
        private bool VisibleRightScrollButton = false;
        private bool VisibleLeftScrollButton = false;
        private bool stateRight = false;
        private bool stateLeft = false;
        private bool ScrollButtonVisible = false;
        private int countPage = 0;
        private Timer timer1 = new Timer();
        #endregion

        #region Handlers
        private void InitializeButtonSize()
        {
            timer1.Interval = 150;
            timer1.Tick += new EventHandler(timer1_Tick);
            RightRect = new Rectangle(this.Width - 18, 3, 16, 23);
            LeftRect = new Rectangle(0, 3, 16, 23);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (stateRight && VisibleRightScrollButton)
                RightButton_Click();
            if (stateLeft)
                LeftButton_Click();
        }

        private void ResizeScrollButton()
        {
            RightRect.X = this.Width - 16;
        }

        private bool GetScrollButtonAtPoint(Point p)
        {
            if (VisibleRightScrollButton)
                if (RightRect.Contains(p))
                {
                    stateRight = true;
                    RightButton_Click();
                    timer1.Enabled = true;
                    return true;
                }
            if (VisibleLeftScrollButton)
                if (LeftRect.Contains(p))
                {
                    stateLeft = true;
                    LeftButton_Click();
                    timer1.Enabled = true;
                    return true;
                }

            return false;
        }

        private void UpScrollButton()
        {
            timer1.Enabled = false;
            stateLeft = false;
            stateRight = false;
            timer1.Enabled = false;
        }

        private void RightButton_Click()
        {
            if (countPage + 1 < Controls.Count)
            {
                countPage++;
                if (countPage != 0)
                    for (int i = 0; i < Controls.Count; i++)
                    {
                        if (i < countPage)
                            ((StiTabulatorPage)Controls[i]).Invisible = true;
                        else
                            ((StiTabulatorPage)Controls[i]).Invisible = false;
                    }
            }
            this.selectedTab.Invalidate();
        }

        private void LeftButton_Click()
        {
            if (countPage != 0)
            {
                countPage--;
                for (int i = 0; i < Controls.Count; i++)
                {
                    if (i < countPage)
                        ((StiTabulatorPage)Controls[i]).Invisible = true;
                    else
                        ((StiTabulatorPage)Controls[i]).Invisible = false;
                }
            }
            this.selectedTab.Invalidate();
        }

        #endregion

        private void VisiblePage()
        {
            if (countPage > 0)
            {
                countPage = 0;
                for (int i = 0; i < Controls.Count; i++)
                {
                    ((StiTabulatorPage)Controls[i]).Invisible = false;
                }
            }
        }
        #endregion

        #region Control Added, removed
        private void PageAdded(object sender, ControlEventArgs e)
		{
			e.Control.Dock = DockStyle.Fill;
			e.Control.CreateGraphics();

			if (Controls.Count == 1)this.SelectedTab = Controls[0] as StiTabulatorPage;
			MeasureTitles();
			UpdateTitle();
		}

		private void PageRemoved(object sender, ControlEventArgs e)
		{
			foreach (StiTabulatorPage page in Controls)
				if (page == SelectedTab)
				{
					Invalidate();
					return;
				}
			
			if (Controls.Count > 0)
			{
				SelectedTab = Controls[0] as StiTabulatorPage;
				Invalidate();
			}
			SelectedTab = null;
			MeasureTitles();
			UpdateTitle();
			
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
        public StiTabulator()
        {
            mode = new StiTopHorizontalMode(this);
            style = new StiTabulatorStyle(this);

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

            InitializeButtonSize();
            SetLayout();

            // BackColor = StiColorUtils.Light(SystemColors.Control, 10);
        }

        #endregion
       
    }
}