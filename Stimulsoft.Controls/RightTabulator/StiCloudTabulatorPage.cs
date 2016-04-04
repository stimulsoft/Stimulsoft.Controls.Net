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
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Single tab page in a StiCloudTabulator.
	/// </summary>
	[ToolboxItem(false)]
	public sealed class StiCloudTabulatorPage : ContainerControl
	{
		#region Fields
        internal Size TitleSize;

        private Rectangle titleRect = Rectangle.Empty;
	    internal Rectangle TitleRect
	    {
	        get
	        {
	            return titleRect;
	        }
	        set
	        {
	            titleRect = value;
	        }
	    }
		#endregion

		#region Properties
		public int PageIndex
		{
			get
			{
				if (Parent != null)return Parent.Controls.IndexOf(this);
				return -1;
			}
		}


		private Image image = null;
		[DefaultValue(null)]
		[Category("Appearance")]
		public Image Image
		{
			get
			{
				return image;
			}
			set
			{
				image = value;

                if (Parent != null) 
                    ((StiCloudTabulator)Parent).UpdateTitle();

                Invalidate();
			}
		}


		/// <summary>
		/// Gets or sets the text associated with this control.
		/// </summary>
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

                if (Parent != null) 
                    ((StiCloudTabulator)Parent).UpdateTitle();

                Invalidate();
			}
		}
		
		
		private bool invisible = false;
		/// <summary>
		/// Gets or sets the value indicates page is invisible.
		/// </summary>
		[DefaultValue(false)]
		[Category("Appearance")]
		public bool Invisible
		{
			get
			{
				return invisible;
			}
			set
			{
			    if (invisible == value) return;
			    invisible = value;

                var tabControl = Parent as StiCloudTabulator;

			    if (!DesignMode)
			    {
			        if (tabControl != null)
			        {
			            if (tabControl.SelectedTab == null && (!invisible))
			            {
			                tabControl.SelectedTab = this;
			            }
			            else if (tabControl.SelectedTab == this)
			            {
                            for (int index = 0; index < tabControl.Controls.Count; index++)
                            {
                                var page = tabControl.Controls[index] as StiCloudTabulatorPage;
                                if (page == null)
                                    continue;

			                    if (!page.Invisible)
			                    {
			                        tabControl.SelectedTab = page;
			                        break;
			                    }
			                }
			            }
				
			            if (tabControl.SelectedTab == this && invisible)
			                tabControl.SelectedTab = null;
			        }
			    }

			    Invalidate();
                if (tabControl != null)
                    tabControl.CheckScrollBar();
			}
		}
		#endregion

		#region Methods
		internal void MeasureTitle()
		{
			if (Parent != null)
			{
				using (var g = Graphics.FromHwnd(this.Handle))
				{
                    TitleSize = ((StiCloudTabulator)Parent).Mode.MeasureTitle(g, this);
				}
			}
		}

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
		public bool InvisibleVisible
		{
			get
			{
				return base.Visible;
			}
			set
			{
                if (Parent != null && this == ((StiCloudTabulator)Parent).SelectedTab)
				{
					base.Visible = true;
					Dock = DockStyle.Fill;
				}
				else base.Visible = value;
			}
		}

		
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		public new DockStyle Dock
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
		#endregion

		#region Handlers
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			StiColors.InitColors();
		}

		protected override void OnPaintBackground(PaintEventArgs p)
		{
			var g = p.Graphics;
			var rect = new Rectangle(0, 0, Width, Height);
            
			if (Parent != null)
			{
				g.FillRectangle(Brushes.White, rect);
			}			
		}		

		protected override void OnPaint(PaintEventArgs p)
		{
			if (Parent != null)
			{
			    var g = p.Graphics;
                var rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

                using (var brush = new SolidBrush(Color.FromArgb(198, 198, 198)))
                {
                    g.FillRectangle(brush, 0, 0, 1, 1);
                    g.FillRectangle(brush, rect.Right, 0, 1, 1);
                    g.FillRectangle(brush, rect.Right, rect.Bottom, 1, 1);
                    g.FillRectangle(brush, 0, rect.Bottom, 1, 1);
                }

                using (var penDark = new Pen(Color.FromArgb(198, 198, 198)))
                {
                    penDark.Color = Color.FromArgb(198, 198, 198);
                    penDark.DashPattern = new float[] { 1f, 2f };
                    g.DrawLine(penDark, rect.Right, rect.Top, rect.Right, rect.Bottom);
                }
			}
		}

		#endregion
	
        public StiCloudTabulatorPage()
            : this("StiCloudTabulatorPage")
        {

        }

        public StiCloudTabulatorPage(string text)
        {
            this.Text = text;

            this.DockPadding.Top =
                this.DockPadding.Left =
                this.DockPadding.Bottom = 0;
            this.DockPadding.Right = 1;

            this.Visible = false;

            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
		}
	}
}