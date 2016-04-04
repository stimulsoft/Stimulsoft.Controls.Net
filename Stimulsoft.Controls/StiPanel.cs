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
	public enum StiBorderStyle
	{
		RaisedOuter = 1,
		SunkenOuter = 2,
		RaisedInner = 4,
		SunkenInner = 8,
		Bump = 9,
		Etched = 6,
		Flat = 0x400a,
		Raised = 5,
		Sunken = 10,
		None = 0
	}

	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiButton), "Toolbox.StiPanel.bmp")]
	public class StiPanel : Panel
	{
		#region Properties
		private StiBorderStyle borderStyle = StiBorderStyle.None;
		[Category("Appearance")]
		[DefaultValue(StiBorderStyle.None)]
		public new virtual StiBorderStyle BorderStyle
		{
			get
			{
				return borderStyle;
			}
			set
			{				
				borderStyle = value;
				Invalidate();
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
			var rect = new Rectangle(0, 0, Width, Height);
			using (var brush = new SolidBrush(BackColor))
			{
				g.FillRectangle(brush, rect);
			}
			
			if (BorderStyle != StiBorderStyle.None)
			{
				ControlPaint.DrawBorder3D(g, rect, (Border3DStyle)BorderStyle);
			}
		}

		#endregion

		#region Constructors
		public StiPanel()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, false);
		}
		#endregion
	}
}
