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
	/// Summary description for StiPopupForm.
	/// </summary>
	public abstract class StiPopupBaseForm : Form, IMessageFilter
	{
		#region FormHandler
		private class FormHandler : System.Windows.Forms.NativeWindow
		{		
			protected const int WM_NCACTIVATE = 0x086;
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == WM_NCACTIVATE && (int) m.WParam == 0)m.Result = (IntPtr) 1;
				else base.WndProc(ref m);
			}

			public FormHandler(Control control)
			{
				if (control != null && control.TopLevelControl != null)
				{
					this.AssignHandle(control.TopLevelControl.Handle);
				}
			}
		}
		#endregion

		#region Fields
		private FormHandler ActivetedHandler;
		protected Control ParentControl = null;
		protected bool IsClosed = false;
		#endregion

		#region Methods
		public virtual void SetLocation(int x, int y, int width, int height)
		{
            if (Screen.AllScreens.Length == 1)
            {
                if ((y + height) > Screen.PrimaryScreen.WorkingArea.Bottom)
                {
                    y = Screen.PrimaryScreen.WorkingArea.Bottom - height - 1;
                }

                if ((x + width) > Screen.PrimaryScreen.WorkingArea.Right)
                {
                    x = Screen.PrimaryScreen.WorkingArea.Right - width - 1;
                }
            }

			this.CreateControl();
			this.SetBoundsCore(x, y, width, height, BoundsSpecified.All);
		}

		public virtual void ClosePopupForm()
		{
			this.Close();
			this.IsClosed = true;
			InvokeClosePopup(EventArgs.Empty);
		}

		public virtual void ShowPopupForm()
		{
            //При double click по кнопке валилось со странной ошибкой

			if (is64Bit)
                Show();
			else Win32.ShowWindow(this.Handle, (short)Win32.ShowWindowCommand.SW_SHOWNOACTIVATE);

			InvokeShowPopup(EventArgs.Empty);
		}
		#endregion
		
		#region Events
		[Category("Behavior")]
		public event EventHandler ClosePopup;

		[Category("Behavior")]
		public event EventHandler ShowPopup;

		protected virtual void OnClosePopup(EventArgs e)
		{
			if (ClosePopup != null)ClosePopup(this, e);
		}

		protected virtual void OnShowPopup(EventArgs e)
		{
			if (ShowPopup != null)ShowPopup(this, e);
		}


		public void InvokeClosePopup(EventArgs e)
		{
			OnClosePopup(e);
		}

		public void InvokeShowPopup(EventArgs e)
		{
			OnShowPopup(e);
		}

		#endregion

		#region Handlers
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{					
				case (int)Win32.Msg.WM_ACTIVATEAPP:
					ClosePopupForm();
					break;

				case (int)Win32.Msg.WM_MOUSEACTIVATE:
					m.Result = (IntPtr)Win32.MouseActivateFlags.MA_NOACTIVATE;
					break;

				default:					
					base.WndProc(ref m);
					break;
			}
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if ((!base.DesignMode) && HookMouseButtonsMessages)
			{
				Application.AddMessageFilter(this);
			}
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			if ((!base.DesignMode) && HookMouseButtonsMessages)
			{
				Application.RemoveMessageFilter(this);
			}
		}


		public virtual bool PreFilterMessage(ref System.Windows.Forms.Message m)
		{
			if ((m.Msg == (int)Win32.Msg.WM_LBUTTONDOWN) ||
				(m.Msg == (int)Win32.Msg.WM_MBUTTONDOWN) ||
				(m.Msg == (int)Win32.Msg.WM_RBUTTONDOWN) ||
				(m.Msg == (int)Win32.Msg.WM_XBUTTONDOWN) ||
				(m.Msg == (int)Win32.Msg.WM_NCLBUTTONDOWN) ||
				(m.Msg == (int)Win32.Msg.WM_NCMBUTTONDOWN) ||
				(m.Msg == (int)Win32.Msg.WM_NCRBUTTONDOWN) ||
				(m.Msg == (int)Win32.Msg.WM_NCXBUTTONDOWN))
			{
				Point pos = Cursor.Position;

				if (!((pos.X >= this.Left) && (pos.X <= this.Right) && (pos.Y >= this.Top) && (pos.Y <= this.Bottom)))
				{
					IStiPopupParentControl popupParentControl = ParentControl as IStiPopupParentControl;
					if (popupParentControl != null)
					{
						pos = ParentControl.PointToClient(pos);
						if ((pos.X >= 0) && (pos.X <= ParentControl.Width) && (pos.Y >= 0) && (pos.Y <= ParentControl.Height))
						{
							popupParentControl.LockPopupInvoke = true;
						}
					}
					ClosePopupForm();
					Application.DoEvents();
				}
			}
			
			return false;
		}

		
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);

			if (this.Visible)
			{
				if (ActivetedHandler  == null)ActivetedHandler = new FormHandler(ParentControl);
			}
			else
			{
				if (ActivetedHandler != null)ActivetedHandler.ReleaseHandle();
			}
		
		}

		#endregion	
	
		#region Properties
		protected virtual bool HookMouseButtonsMessages
		{
			get
			{
				return true;
			}
		}


		protected virtual bool Layered
		{
			get
			{
				return true;
			}
		}

		protected virtual bool UseCreateParams
		{
			get
			{
				return true;
			}
		}
		
		protected override CreateParams CreateParams
		{
			get
			{
				if (UseCreateParams)
				{
					uint WS_POPUP = 0x80000000;
					int WS_EX_TOPMOST = 0x00000008;
					int WS_EX_TOOLWINDOW = 0x00000080;
					int WS_EX_LAYERED = 0x00080000;

					CreateParams createParams = new CreateParams();

                    createParams.X = Location.X;
                    createParams.Y = Location.Y;
					createParams.Height = Size.Height;
					createParams.Width = Size.Width;

					createParams.Parent = IntPtr.Zero;

					createParams.Style = unchecked((int)WS_POPUP);
					createParams.ExStyle |= WS_EX_TOPMOST;
					createParams.ExStyle |= WS_EX_TOOLWINDOW;
					if (Layered) createParams.ExStyle |= WS_EX_LAYERED;

					return createParams;
				}
				else return base.CreateParams;
			}
		}

		internal void UpdateZOrder(IntPtr after) 
		{
			int SWP_NOSIZE = 0x0001;
			int SWP_NOMOVE = 0x0002;
			int SWP_NOACTIVATE = 0x0010;

			int flags = SWP_NOACTIVATE | SWP_NOSIZE | SWP_NOMOVE;
			Win32.SetWindowPos(this.Handle, after, Left, Top, Width, Height, flags);
		}
		
		#endregion

		protected static bool is64Bit = false;

		static StiPopupBaseForm()
		{
            is64Bit = IntPtr.Size == 8;
		}
		
		public StiPopupBaseForm() : this(null)
		{
		}

		public StiPopupBaseForm(Control parentControl)
		{
			this.SuspendLayout();

			this.ParentControl = parentControl;

			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.TopMost = false;			
			this.ResumeLayout();

			if (parentControl != null)
			{
				Form parentForm = parentControl.FindForm();
				if(parentForm != null)parentForm.AddOwnedForm(this);
			}
		}
	}
}
