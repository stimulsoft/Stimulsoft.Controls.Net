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

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Defines a base class for controls that support scrolling behavior.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiScrollableControl), "Toolbox.StiScrollableControl.bmp")]
	public class StiScrollableControl : ScrollableControl
	{
		#region Handlers
		protected override void OnSizeChanged(System.EventArgs e)
		{
			base.OnSizeChanged(e);
			SetPos();
			this.SetDisplayRectLocation(10, 10);
		}

		private void OnValueChanged(object sender, EventArgs e)
		{
			SetPos();		
			Draw();
		}
		#endregion
	
		#region Fields
		private Panel edgePanel = new Panel();
		#endregion

		#region Browsable(false)
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
		public new ScrollableControl.DockPaddingEdges DockPadding
		{
			get
			{
				return base.DockPadding;
			}
		}
		#endregion

		#region Properties
		private StiVScrollBar vScrollBar;
		/// <summary>
		/// Gets or sets the vertical ScrollBar.
		/// </summary>
		[Browsable(false)]
		public StiVScrollBar VScrollBar
		{
			get
			{
				return vScrollBar;
			}
			set
			{
				vScrollBar = value;
			}
		}

		private StiHScrollBar hScrollBar;
		/// <summary>
		/// Gets or sets the horizontal ScrollBar.
		/// </summary>
		[Browsable(false)]
		public StiHScrollBar HScrollBar
		{
			get
			{
				return hScrollBar;
			}
			set
			{
				hScrollBar = value;
			}
		}

		private int scrollWidth = 100;
		/// <summary>
		/// Gets or sets the value indicates width of a scrolling area.
		/// </summary>
		[Category("Behavior")]
		public virtual int ScrollWidth
		{
			get
			{
				return scrollWidth;
			}
			set
			{
				scrollWidth = value;
				SetPos();
				Draw();
			}
		}


		private int scrollHeight = 100;
		/// <summary>
		/// Gets or sets the value indicates height of a scrolling area.
		/// </summary>
		[Category("Behavior")]
		public virtual int ScrollHeight
		{
			get
			{
				return scrollHeight;
			}
			set
			{
				scrollHeight = value;
				SetPos();
				Draw();
			}
		}


		private int scrollTop = 0;
		/// <summary>
		/// Gets or sets the top position of a scrolling area.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ScrollTop
		{
			get
			{
				return scrollTop;
			}
			set
			{
				int dist = value - scrollTop;
				if (dist != 0)
				{
					foreach (Control control in Controls)
						if (!IsScrollBar(control))						
							control.Top += dist;
					
					scrollTop = value;
				}
			}
		}


		private int scrollLeft = 0;
		/// <summary>
		/// Gets or sets the left position of a scrolling area.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ScrollLeft
		{
			get
			{
				return scrollLeft;
			}
			set
			{
				
				int dist = value - scrollLeft;
				if (dist != 0)
				{
					//HScrollBar.Value = value;
					foreach (Control control in Controls)
						if (!IsScrollBar(control))
							control.Left += dist;
					
					scrollLeft = value;
				}
			}
		}

        /// <summary>
        /// Gets or sets the width of the client area of the control.
        /// </summary>   
		[Browsable(false)]
		public int ClientWidth
		{
			get
			{
				if (VScrollBar.Visible)return this.Width - 16;
				else return this.Width;
			}
		}
		
		/// <summary>
		/// Gets or sets the height of the client area of the control.
		/// </summary>
		[Browsable(false)]
		public int ClientHeight
		{
			get
			{
				if (HScrollBar.Visible)return this.Height - 16;
				else return this.Height;
			}
		}

		#endregion
		
		#region Methods
		private void Draw()
		{
			Invalidate(this.ClientRectangle);
		}

		private bool IsScrollBar(Control control)
		{
			return (control == VScrollBar) || (control == HScrollBar) || (control == edgePanel);
		}

		protected void SetPos()
		{
			try
			{
				if (ClientWidth >= ScrollWidth)
				{
					HScrollBar.Visible = false;
				}
				else HScrollBar.Visible = true;

				if (ClientHeight >= ScrollHeight)
				{
					VScrollBar.Visible = false;
				}
				else VScrollBar.Visible = true;

				#region VScrollBar
				VScrollBar.Width = 16;
				if (HScrollBar.Visible)VScrollBar.Height = this.Height - 16;
				else VScrollBar.Height = this.Height;
				VScrollBar.Top = 0;
				VScrollBar.Left = this.Width - VScrollBar.Width;
				#endregion
			
				#region HScrollBar
				HScrollBar.Width = this.Width - 16;
				if (VScrollBar.Visible)HScrollBar.Width = this.Width - 16;
				else HScrollBar.Width = this.Width;
				HScrollBar.Height = 16;
				HScrollBar.Left = 0;
				HScrollBar.Top = this.Height - HScrollBar.Height;
				#endregion

				#region EdgePanel
				edgePanel.Left = HScrollBar.Right;
				edgePanel.Top = VScrollBar.Bottom;
				edgePanel.Width = VScrollBar.Width;
				edgePanel.Height = HScrollBar.Height;
				edgePanel.Visible = HScrollBar.Visible & VScrollBar.Visible;
				edgePanel.BackColor = SystemColors.Control;
				#endregion

				ScrollTop = - VScrollBar.Value;
				ScrollLeft = - HScrollBar.Value;

				VScrollBar.Minimum = 0;
				VScrollBar.Maximum = ScrollHeight;
				VScrollBar.LargeChange = Math.Max(this.ClientHeight, 0);
				VScrollBar.SmallChange = VScrollBar.LargeChange / 10;

				HScrollBar.Minimum = 0;
				HScrollBar.Maximum = ScrollWidth;
				HScrollBar.LargeChange = Math.Max(this.ClientWidth, 0);
				HScrollBar.SmallChange = HScrollBar.LargeChange / 10;
			}
			catch
			{
			}
			
		}

		#endregion

		#region Constructors
		public StiScrollableControl()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.ContainerControl, false);

			#region ScrollBars
			VScrollBar = new StiVScrollBar();
			VScrollBar.ValueChanged += new EventHandler(OnValueChanged);

			HScrollBar = new StiHScrollBar();
			HScrollBar.ValueChanged += new EventHandler(OnValueChanged);
		
			this.Controls.Add(VScrollBar);
			this.Controls.Add(HScrollBar);
			#endregion

			#region EdgePanel
			this.Controls.Add(edgePanel);
			#endregion

			SetPos();

			Width = 100;
			Height = 100;

			this.ClientSize = new Size(40, 40);// = Width - 40;
		}
		#endregion
	}
}
