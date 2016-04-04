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
    #if !Profile
    [Designer(typeof(StiDockingControlDesigner))]
    #endif
	[ToolboxItem(false)]
    public class StiDockingControl : Panel
    {
		#region Events
		#region Closing
		/// <summary>
		/// Occurs when the StiDockingControl is closing.
		/// </summary>
		[Category("Behavior")]
		public event CancelEventHandler Closing;

		protected virtual void OnClosing(CancelEventArgs e)
		{
			if (Closing != null)Closing(this, e);
		}


		/// <summary>
		/// Raises the Closing event for this control.
		/// </summary>
		/// <param name="e">An CancelEventArgs that contains the event data.</param>
		public void InvokeClosing(CancelEventArgs e)
		{
			OnClosing(e);
		}
		#endregion

		#region Closed
		/// <summary>
		/// Occurs when the StiDockingControl is closed.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler Closed;

		protected virtual void OnClosed(EventArgs e)
		{
			if (Closed != null)Closed(this, e);
		}


		/// <summary>
		/// Raises the Closed event for this control.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		public void InvokeClosed(EventArgs e)
		{
			OnClosed(e);
		}
		#endregion
		#endregion

		#region Properties
		private string guid;
		[Category("Behavior")]
		[Browsable(false)]
		public string Guid
		{
			get
			{
				return guid;
			}
			set
			{
				guid = value;
			}
		}


		private bool isSelected; 
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsSelected
		{
			get
			{
				return isSelected;
			}
			set
			{
				isSelected = value;
			}
		}


		private Image image;
		[Category("Appearance")]
		[Browsable(true)]
		public Image Image
		{
			get
			{
				return image;
			}
			set
			{
				if (value is Bitmap)StiImageUtils.MakeImageBackgroundAlphaZero(value as Bitmap);
				image = value;
				Invalidate();
				if (Parent != null)Parent.Invalidate();
			}
		}


		[Browsable(false)]
		public bool IsVisible
		{
			get
			{
				return (Parent == null) 
                    ? false 
                    : ((StiDockingPanel)Parent).Manager.ClosedControls.IndexOf(this) == -1;
			}
		}


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
				if (base.Parent != null)
				{
					StiDockingPanel panel = (StiDockingPanel) base.Parent;
					if (panel.SelectedControl == this)
					{
						panel.InvalidateTitle();
						panel.InvalidateTabs();
						if (panel.FloatingForm != null)
						{
							panel.FloatingForm.Text = value;
						}
					}
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

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			((StiDockingPanel)Parent).Invalidate();
		}
	    
		#endregion

		#region Constructors
		public StiDockingControl()
		{
			DockPadding.All = 2;

			base.SetStyle(ControlStyles.ResizeRedraw, true);
			this.Text = "StiDockingControl";

			this.guid = System.Guid.NewGuid().ToString();
		} 
		#endregion
    }
}