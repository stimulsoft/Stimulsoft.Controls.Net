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
	/// Single tab page in a StiTabControl.
	/// </summary>
	[ToolboxItem(false)]
    #if !Profile
	[Designer(typeof(StiTabPageDesigner))]
    #endif
	public class StiTabPage : Panel
	{
		#region Properties
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
				Invalidate();
				if (Parent != null)Parent.Invalidate();
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
				Invalidate();
				if (Parent != null)Parent.Invalidate();
			}
		}
		
		
		private bool drawBorder = true;
		/// <summary>
		/// Gets or sets the value indicates control to draw border.
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
				if (Parent != null)Parent.Invalidate();
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
				invisible = value;

				if (!DesignMode)
				{
					StiTabControl tabControl = Parent as StiTabControl;

					if (tabControl != null)
					{

						if (tabControl.SelectedTab == null && (!invisible))
						{
							tabControl.SelectedTab = this;
						}
						else if (tabControl.SelectedTab == this)
						{
							foreach (StiTabPage page in tabControl.Controls)
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

		#region Browsable(false)
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		public new bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				if (Parent != null && this == ((StiTabControl)Parent).SelectedTab)
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

		#region Handlers
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			StiColors.InitColors();
		}

		protected override void OnPaint(PaintEventArgs p)
		{
			var g = p.Graphics;

			var rect = new Rectangle(0, 0, Width - 1, Height - 1);

			if (DrawBorder && (!Invisible))
			{
                var tabControl = this.Parent as StiTabControl;

                #region Office2013
                if (tabControl.ControlStyle == StiControlStyle.Office2013Blue)
                {
                    if (!tabControl.PositionAtBottom)
                    {
                        using (var pen = new Pen(StiStyleOffice2013Blue.ColorLineGray))
                        {
                            g.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom);
                            g.DrawLine(pen, rect.X, rect.Bottom, rect.Right, rect.Bottom);
                            g.DrawLine(pen, rect.Right, rect.Top, rect.Right, rect.Bottom);
                        }
                    }
                    else
                    {
                        using (var pen = new Pen(StiStyleOffice2013Blue.ColorLineGray))
                        {
                            g.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom);
                            g.DrawLine(pen, rect.X, rect.Bottom, rect.Right, rect.Bottom);
                            g.DrawLine(pen, rect.Right, rect.Top, rect.Right, rect.Bottom);
                        }
                    }
                }
                #endregion

                #region Flat
                else
                {
                    if (!tabControl.PositionAtBottom)
                    {
                        g.DrawLine(SystemPens.ControlLightLight, rect.X, rect.Y, rect.X, rect.Bottom);
                        g.DrawLine(SystemPens.ControlDark, rect.X, rect.Bottom, rect.Right, rect.Bottom);
                        g.DrawLine(SystemPens.ControlDark, rect.Right, rect.Top, rect.Right, rect.Bottom);
                    }
                    else
                    {
                        g.DrawLine(SystemPens.ControlLightLight, rect.X, rect.Y, rect.X, rect.Bottom);
                        g.DrawLine(SystemPens.ControlLightLight, rect.X, rect.Top, rect.Right, rect.Top);
                        g.DrawLine(SystemPens.ControlDark, rect.Right, rect.Top, rect.Right, rect.Bottom);
                    }
                }
                #endregion
            }
		}

		#endregion
	
		#region Constructors
		public StiTabPage() : this("TabPage")
		{
		}

		public StiTabPage(string text)
		{
			this.Text = text;

			this.DockPadding.Top = 
			this.DockPadding.Left = 
			this.DockPadding.Bottom = 
			this.DockPadding.Right = 4;
			this.Visible = false;

			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
		}
		#endregion
	}
}
