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
using System.Windows.Forms;
using System.ComponentModel;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{	
	/// <summary>
	/// Allows users to select a value from a TreeView.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiTreeViewBox), "Toolbox.StiTreeViewBox.bmp")]
	public class StiTreeViewBox : 
		StiButtonEditBase, 
		IStiPopupParentControl
	{
		#region IStiPopupParentControl
		private bool lockPopupInvoke = false;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool LockPopupInvoke
		{
			get
			{
				return lockPopupInvoke;
			}
			set
			{
				lockPopupInvoke = value;
			}
		}
		#endregion

		#region Fields
		internal StiTreeViewBoxPopupForm TreeViewBoxPopupForm = null;
		internal static Bitmap dropdownBitmap = null;
		#endregion

		#region Events
		#region DropDown
		/// <summary>
		/// Occurs when the drop-down portion of a StiComboBox is shown.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler DropDown;
		
		protected virtual void OnDropDown(System.EventArgs e)
		{
			if (DropDown != null)DropDown(this, e);
		}


		/// <summary>
		/// Raises the DropDown event for this control.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		public void InvokeDropDown(System.EventArgs e)
		{
			OnDropDown(e);
		}
		#endregion		
	
		#region DropUp
		/// <summary>
		/// Occurs when the drop-down portion of a ComboBox is closed.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler DropUp;

		protected virtual void OnDropUp(System.EventArgs e)
		{
			if (DropUp != null)DropUp(this, e);
		}


		/// <summary>
		/// Raises the DropUp event for this control.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		public void InvokeDropUp(System.EventArgs e)
		{
			OnDropUp(e);
		}
		#endregion

		#region ValueChanged
		/// <summary>
		/// Represents the method that handles the ValueChanged event.
		/// </summary>
		public delegate void ValueChangedEventHandler(object sender, StiValueChangedEventArgs e);

		/// <summary>
		/// Occurs when the SelectedItem property has changed.
		/// </summary>
		[Category("Behavior")]
		public event ValueChangedEventHandler ValueChanged;

		protected virtual void OnValueChanged(StiValueChangedEventArgs e)
		{
			if (ValueChanged != null)ValueChanged(this, e);
		}


		/// <summary>
		/// Raises the ValueChanged event for this control.
		/// </summary>
		/// <param name="e">An StiValueChangedEventArgs that contains the event data.</param>
		public void InvokeValueChanged(StiValueChangedEventArgs e)
		{
			OnValueChanged(e);
		}
		#endregion

		#region TreeNodesFill
		/// <summary>
		/// Represents the method that handles the TreeNodesFill event.
		/// </summary>
		public delegate void TreeNodesFillEventHandler(object sender, StiTreeNodesFillEventArgs e);

		/// <summary>
		/// Occurs when the TreeView needs fill.
		/// </summary>
		[Category("Behavior")]
		public event TreeNodesFillEventHandler TreeNodesFill;

		protected virtual void OnTreeNodesFill(StiTreeNodesFillEventArgs e)
		{
			if (TreeNodesFill != null)TreeNodesFill(this, e);
		}


		/// <summary>
		/// Raises the TreeNodesFill event for this control.
		/// </summary>
		/// <param name="e">A StiTreeNodesFillEventArgs that contains the event data.</param>
		public void InvokeTreeNodesFill(StiTreeNodesFillEventArgs e)
		{
			OnTreeNodesFill(e);
		}
		#endregion
		#endregion

		#region Handlers
		protected override void OnEnabledChanged(System.EventArgs e)
		{
			base.OnEnabledChanged(e);
			TextBox.Visible = false;
			Invalidate();
		}

	
		protected override void OnPaint(PaintEventArgs p)
		{
			base.OnPaint(p);

			if (this.Enabled)
			{
				var g = p.Graphics;

				var contentRect = GetContentRect();
				contentRect.X -= 1;
				
				using (var sf = new StringFormat())
				{
					sf.FormatFlags |= StringFormatFlags.NoWrap;
					sf.Trimming = StringTrimming.EllipsisCharacter;
					sf.LineAlignment = StringAlignment.Center;

				    if (this.Enabled)
				    {
				        using (var brush = new SolidBrush(this.ForeColor))
				            g.DrawString(this.Text, this.Font, brush, contentRect, sf);
				    }
				    else
				    {
				        g.DrawString(this.Text, this.Font, SystemBrushes.ControlDark, contentRect, sf);
				    }
				}
				
				#region Paint focus
				if (this.Focused && AllowDrawFocus)
				{
					var focusRect = GetContentRect();
					focusRect.X -= 1;
					focusRect.Y += 1;
					focusRect.Width --;
					focusRect.Height -= 2;
					ControlPaint.DrawFocusRectangle(g, focusRect);
				}
				#endregion
			}	
		}

		
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if ((e.Button & MouseButtons.Left) > 0)
			{
				if ((!LockPopupInvoke) && TreeViewBoxPopupForm == null)
				{
					InvokeDropDown(EventArgs.Empty);
					TreeViewBoxPopupForm = new StiTreeViewBoxPopupForm(this);
					TreeViewBoxPopupForm.ClosePopup += new EventHandler(OnClosePopup);
		
					Rectangle rect = this.RectangleToScreen(this.ClientRectangle);
					int cbWidth = DropDownWidth;
					if (cbWidth < this.Width)cbWidth = this.Width;

                    TreeViewBoxPopupForm.SetLocation(rect.Left, rect.Top + this.Height + 1, cbWidth, this.MaxDropDownHeight);

					StiTreeNodesFillEventArgs ea = new StiTreeNodesFillEventArgs(
						TreeViewBoxPopupForm.tvNodes.Nodes, 
						TreeViewBoxPopupForm.tvNodes);
					InvokeTreeNodesFill(ea);
					TreeViewBoxPopupForm.ShowPopupForm();

					InvokeDropDown(EventArgs.Empty);
				}
				else LockPopupInvoke = false;
			}
		}

		
		private void OnClosePopup(object sender, EventArgs e)
		{			
			if (TreeViewBoxPopupForm != null)
			{
				InvokeDropUp(e);
				TreeViewBoxPopupForm.Dispose();
				TreeViewBoxPopupForm = null;
			}
		}


		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged (e);

			Invalidate();
		}
		#endregion

		#region Properties
		private bool allowDrawFocus = true;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool AllowDrawFocus
		{
			get
			{
				return allowDrawFocus;
			}
			set
			{
				allowDrawFocus = value;
			}
		}

		[DefaultValue(true)]
		public override bool ReadOnly
		{
			get
			{
				return !TextBox.Visible;
			}
			set
			{
				TextBox.Visible = !value;
				BackColor = SystemColors.Window;
			}
		}


		private object selectedItem = null;
		/// <summary>
		/// Gets or sets the currently selected item.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual object SelectedItem
		{ 
			get
			{
				return selectedItem;
			}
			set
			{
				selectedItem = value;
			}
		}


		private int dropDownWidth = 0;
		/// <summary>
		/// Gets or sets the width of the drop-down portion of a tree view.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(0)]
		public virtual int DropDownWidth
		{
			get
			{
				return dropDownWidth;
			}
			set
			{
				dropDownWidth = value;
			}
		}


		private bool selectOnlyTagNotNull = false;
		/// <summary>
		/// Gets or sets the value indicates where user can select only the TreeNode with tag property not equal null value.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]
		public virtual bool SelectOnlyTagNotNull
		{
			get
			{
				return selectOnlyTagNotNull;
			}
			set
			{
				selectOnlyTagNotNull = value;
			}
		}


		private int maxDropDownHeight = 200;
		/// <summary>
		/// Gets or sets the maximum height of the drop-down portion of a combo box.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(200)]
		public virtual int MaxDropDownHeight
		{
			get
			{
				return maxDropDownHeight;
			}
			set
			{
				maxDropDownHeight = value;
			}
		}
		#endregion

		#region Constructors
		static StiTreeViewBox()
		{
			dropdownBitmap = StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.ComboDown.bmp");
		}


		public StiTreeViewBox()
		{
			ReadOnly = true;
			//base.ButtonBitmap = dropdownBitmap;
		}
		#endregion
	}
}
