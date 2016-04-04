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
	/// Single tab page in a StiTabulator.
	/// </summary>
	[ToolboxItem(false)]
    #if !Profile
	[Designer(typeof(StiTabulatorPageDesigner))]
    #endif
	public class StiTabulatorPage : ContainerControl
	{
		#region Fields
		private Size titleSize;
		internal Size TitleSize
		{
			get
			{
				return titleSize;
			}
			set
			{
				titleSize = value;
			}
		}
		#endregion

		#region Properties
		public bool IsMouseOver
		{
			get
			{
				if (Parent != null)
				{
					using (Graphics g = Graphics.FromHwnd(this.Handle))
					{
						Rectangle rect = ((StiTabulator)Parent).GetTitlePageRectangle(g, this);
						return rect.Contains(Parent.PointToClient(Cursor.Position));
					}
				}
				return false;
			}
		}

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
		public virtual Image Image
		{
			get
			{
				return image;
			}
			set
			{
				image = value;
                MeasureTitle();
				Invalidate();
                if (Parent != null) ((StiTabulator)Parent).UpdateTitle();
			}
		}


		private ContextMenu titleContextMenu;
		/// <summary>
		/// Gets or sets the shortcut menu associated with the title control.
		/// </summary>
		[Category("Behavior")]
		public ContextMenu TitleContextMenu
		{
			get
			{
				return titleContextMenu;
			}
			set
			{
				titleContextMenu = value;
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
				MeasureTitle();
				Invalidate();
				if (Parent != null)((StiTabulator)Parent).UpdateTitle();
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

			    if (!DesignMode)
			    {
			        var tabControl = Parent as StiTabulator;

			        if (tabControl != null)
			        {

			            if (tabControl.SelectedTab == null && (!invisible))
			            {
			                tabControl.SelectedTab = this;
			            }
			            else if (tabControl.SelectedTab == this)
			            {
			                foreach (StiTabulatorPage page in tabControl.Controls)
			                {
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
			    if (Parent != null)Parent.Invalidate();
			}
		}
		#endregion

		#region Methods
		internal void MeasureTitle()
		{
			if (Parent != null)
			{
				using (Graphics g = Graphics.FromHwnd(this.Handle))
				{
					TitleSize = ((StiTabulator)Parent).Mode.MeasureTitle(g, this);
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
				if (Parent != null && this == ((StiTabulator)Parent).SelectedTab)
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

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			MeasureTitle();			
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
                ((StiTabulator)Parent).Mode.DrawPage(p.Graphics, this);
			}
		}
	

		#endregion
	
		#region Constructors
		public StiTabulatorPage() : this("TabulatorPage")
        {
        }

        public StiTabulatorPage(string text)
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
		#endregion
	}
}