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
    public class StiFloatingForm : Form
    {
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

		
		public StiFloatingForm(StiDockingPanel panel)
		{
			SetStyle(ControlStyles.ResizeRedraw, true);
			InitializeComponent();

            this.AutoScaleMode = AutoScaleMode.None;

			this.DockPadding.All = 4;
			this.Panel = panel;
		}


        #region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
        private void InitializeComponent()
        {
			// 
			// StiFloatingForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(228, 294);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MinimumSize = new System.Drawing.Size(100, 55);
			this.Name = "StiFloatingForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Floating Form";
			this.SizeChanged += new System.EventHandler(this.StiFloatingForm_SizeChanged);

		}

		#endregion

		#region Handlers
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            this.Panel.InvalidateTitle();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            this.Panel.InvalidateTitle();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
			ControlPaint.DrawBorder3D(e.Graphics, 0, 0, Width, Height, Border3DStyle.Raised, Border3DSide.All);
		}

		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			e.Control.Dock = DockStyle.Fill;
			((StiDockingPanel)e.Control).FloatingForm = this;
		}

		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);

			((StiDockingPanel)e.Control).FloatingForm = null;
			
			if (Controls.Count == 0)
			{
				Close();
				Dispose();
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == (int)Win32.Msg.WM_NCHITTEST)
			{
				IntPtr ptrPoint = m.LParam;
				m.Result = new IntPtr((int)this.GetNCHitTest(new Point(ptrPoint.ToInt32())));
				return;
			}
			base.WndProc(ref m);
		}

		#endregion

		private Stimulsoft.Base.Drawing.Win32.MousePosition GetNCHitTest(Point p)
		{
			Size frameSize = SystemInformation.FrameBorderSize;
			p.Offset(- Left, - Top);

			if (p.X <= 20 && p.Y <= frameSize.Height || 
				p.Y <= 20 && p.X <= frameSize.Height)return Win32.MousePosition.HTTOPLEFT;

			if (p.X >= Width - 20 && p.Y <= frameSize.Height || 
				p.Y <= 20 && p.X >= Width - frameSize.Height)return Win32.MousePosition.HTTOPRIGHT;

			if (p.X <= 20 && p.Y >= Height - frameSize.Height || 
				p.Y >= Height - 20 && p.X <= frameSize.Height)return Win32.MousePosition.HTBOTTOMLEFT;

			if (p.X >= Width - 20 && p.Y >= Height - frameSize.Height || 
				p.Y >= Height - 20 && p.X >= Width - frameSize.Height)return Win32.MousePosition.HTBOTTOMRIGHT;

			if (p.X <= frameSize.Height)return Win32.MousePosition.HTLEFT;
			if (p.Y <= frameSize.Height)return Win32.MousePosition.HTTOP;
			if (p.X >= Width - frameSize.Height)return Win32.MousePosition.HTRIGHT;
			if (p.Y >= Height - frameSize.Height)return Win32.MousePosition.HTBOTTOM;
            
			return Win32.MousePosition.HTCLIENT;
		}


        private StiDockingPanel Panel;

		private void StiFloatingForm_SizeChanged(object sender, System.EventArgs e)
		{
			Panel.SetLayout();
			Panel.Invalidate();
		}
	}
}

