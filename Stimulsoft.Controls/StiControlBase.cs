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
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Defines the base control.
	/// </summary>
	[ToolboxItem(false)]
	public class StiControlBase : Control
	{
		#region Properties
		private bool flat = true;
		[DefaultValue(true)]
		[Category("Appearance")]
		public virtual bool Flat
		{
			get
			{
				return flat;
			}
			set
			{
				flat = value;
				Invalidate();
			}
		}


		private bool isMouseOver = false;
		/// <summary>
		/// Gets or sets the value, indicates that mouse is over on object.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool IsMouseOver
		{
			get
			{
				return isMouseOver;
			}
			set
			{
				isMouseOver = value;
			}
		}

	
		private bool hotFocus = true;
		/// <summary>
		/// Gets or sets the value, indicates that control draws focus when mouse over control.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(true)]
		public bool HotFocus
		{
			get
			{
				return hotFocus;
			}
			set
			{
				hotFocus = value;
			}
		}
		#endregion

		#region Handlers
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			StiColors.InitColors();
		}

		protected override void OnGotFocus(System.EventArgs e)
		{	
			base.OnGotFocus(e);
			if (Flat)Invalidate();
		}

		protected override void OnLostFocus(System.EventArgs e)
		{
			base.OnLostFocus(e);
			if (Flat)Invalidate();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			if (Flat)
			{
				IsMouseOver = true;
				Invalidate();
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (Flat)
			{
				IsMouseOver = false;
				Invalidate();
			}
		}

		#endregion

		#region Browsable(false)
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
			}
		}
		#endregion

		#region Constructors
		public StiControlBase()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.Selectable, true);

			this.TabStop = true;
		}
		#endregion
	}
}
