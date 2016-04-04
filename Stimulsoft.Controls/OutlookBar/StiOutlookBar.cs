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

	/// <summary>
	/// Manage collections of the StiOutlookPanel.
	/// </summary>
	[ToolboxItem(true)]
    #if !Profile
	[Designer(typeof(StiOutlookBarDesigner))]
    #endif
	[ToolboxBitmap(typeof(StiOutlookBar), "Toolbox.StiOutlookBar.bmp")]
	public class StiOutlookBar : Panel
	{
		#region Handlers
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			StiColors.InitColors();
		}

		protected override void OnPaint(PaintEventArgs p)
		{
			if (DrawBorder)
			{
				Graphics g = p.Graphics;
				Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
				g.DrawRectangle(SystemPens.ControlDark, rect);
			}
		}

		protected override void OnBackColorChanged(EventArgs e)
		{
			base.OnBackColorChanged(e);
			Invalidate();
		}

		#endregion
		
		#region Properties
		private bool drawBorder = false;
		/// <summary>
		/// Gets or sets the value, indicates control to draws border.
		/// </summary>
		[DefaultValue(false)]
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
				if (value)DockPadding.All = 3;
				else DockPadding.All = 0;
				Invalidate();
			}
		}
		
		#endregion

		#region Methods
		/// <summary>
		/// Adds category to a collection.
		/// </summary>
		/// <param name="name">The name of category.</param>
		/// <returns>Created category panel.</returns>
		public StiOutlookPanel AddCategory(string name)
		{
			StiOutlookPanel panel = new StiOutlookPanel();
			panel.Text = name;
			this.Controls.Add(panel);
			panel.Dock = DockStyle.Top;
			this.Controls.SetChildIndex(panel, 0);
			return panel;
		}

		#endregion

		#region Properties [Browsable(false)]
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				base.AutoScroll = value;
			}
		}
		#endregion

		#region Constructors
		public StiOutlookBar()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.AutoScroll = true;
		}
		#endregion
	}
}