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
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	internal class StiDockingWindow : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		internal StiDockingWindow()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            this.AutoScaleMode = AutoScaleMode.None;

			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// StiDockingWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(296, 240);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "StiDockingWindow";
			this.Opacity = 0.5;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "StiDockWindow";

		}
		#endregion

		protected override void OnPaintBackground(PaintEventArgs e)
		{
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;
			Rectangle rect = e.ClipRectangle;

			rect.Width --;
			rect.Height --;

			g.DrawRectangle(SystemPens.ControlDark, rect);
			rect.Inflate(-1, -1);
			g.DrawRectangle(SystemPens.ControlDark, rect);
			rect.Inflate(-1, -1);			
			rect.Width ++;
			rect.Height ++;
			g.FillRectangle(SystemBrushes.ActiveCaption, rect);
		}


		internal static StiDockingWindow DockingWindow = null;
		internal static StiDockingPanel panel = null;
		
		internal static Control ControlToDock = null;
		internal static DockStyle DockType;

		private static Form GetForm()
		{
			if (panel.LastDockForm != null)	return panel.LastDockForm;
			return panel.FindForm();
		}


		private static Rectangle GetDockRectangle(Form form)
		{
			Rectangle rect = form.ClientRectangle;

			for (int index = form.Controls.Count - 1; index >= 0; index--)
			{
				Control control = form.Controls[index];
				if (control.Visible)
				switch (control.Dock)
				{
					case DockStyle.Left:
						rect.X += control.Width;
						rect.Width -= control.Width;
						break;

					case DockStyle.Right:
						rect.Width -= control.Width;
						break;

					case DockStyle.Top:
						rect.Y += control.Height;
						rect.Height -= control.Height;
						break;

					case DockStyle.Bottom:
						rect.Height -= control.Height;
						break;

					case DockStyle.Fill:
						return form.RectangleToScreen(rect);
				}
			}
			return form.RectangleToScreen(rect);

		}

		private static Rectangle GetWindowDockRectangle(Rectangle rect)
		{
			Form form = GetForm();
			Rectangle bounds = GetDockRectangle(form);

			Point pos = Cursor.Position;

			bool leftDock = false;
			bool rightDock = false;
			bool topDock = false;
			bool bottomDock = false;

			foreach(Control control in form.Controls)
			{
				if (control is StiDockingPanel)
				{
					switch (control.Dock)
					{
						case DockStyle.Left:
							leftDock = true;
							break;

						case DockStyle.Right:
							rightDock = true;
							break;

						case DockStyle.Top:
							topDock = true;
							break;

						case DockStyle.Bottom:
							bottomDock = true;
							break;
					}
				}
			}

			ControlToDock = form;

			#region Top
			if ((!topDock) && new Rectangle(bounds.Left, bounds.Top, bounds.Width, 20).Contains(pos))
			{
				DockType = DockStyle.Top;
				return new Rectangle(
					bounds.X, 
					bounds.Y, 
					bounds.Width, 
					150);
			}
			#endregion

			#region Bottom
			if ((!bottomDock) && new Rectangle(bounds.Left, bounds.Bottom - 20, bounds.Width, 20).Contains(pos))
			{
				DockType = DockStyle.Bottom;
				return new Rectangle(
					bounds.X, 
					bounds.Bottom - 150,
					bounds.Width, 
					150);
			}
			#endregion Bottom

			#region Left
			if ((!leftDock) && new Rectangle(bounds.Left, bounds.Top, 20, bounds.Height).Contains(pos))
			{
				DockType = DockStyle.Left;
				return new Rectangle(
					bounds.X, 
					bounds.Y, 
					200, 
					bounds.Height);
			}
			#endregion

			#region Right
			if ((!rightDock) && new Rectangle(bounds.Right - 20, bounds.Top, 20, bounds.Height).Contains(pos))	
			{
				DockType = DockStyle.Right;
				return new Rectangle(
					bounds.Right - 200, 
					bounds.Y, 
					200, 
					bounds.Height);
			}
			#endregion

			return Rectangle.Empty;
		}

		private static Rectangle GetPanelDockRectangle(StiDockingPanel dockPanel, Rectangle rect)
		{
			Point pos = Cursor.Position;
			ControlToDock = dockPanel;

			if (dockPanel.Collapsed)
			{
				Rectangle clientRectangle = dockPanel.RectangleToScreen(dockPanel.ClientRectangle);
				if (clientRectangle.Contains(pos))return clientRectangle;
			}

			Rectangle bounds = dockPanel.RectangleToScreen(dockPanel.PanelBounds);
			if (!bounds.Contains(pos))return Rectangle.Empty;

			
			if (dockPanel == panel)
			{
				return Rectangle.Empty;
			}		

			int tabHt = 0;
			
			if (dockPanel.Controls.Count > 1)
			{
				tabHt = StiDockingPanel.TabHeight;
			}

			#region Title
			if (new Rectangle(bounds.Left, bounds.Top, bounds.Width, StiDockingPanel.TitleHeight).Contains(pos))
			{
				DockType = DockStyle.Fill;
				return bounds;
			}
			#endregion

			#region Tabs
			if (tabHt != 0 && new Rectangle(bounds.Left, bounds.Bottom - StiDockingPanel.TabHeight,
				bounds.Width, StiDockingPanel.TabHeight).Contains(pos))
			{
				DockType = DockStyle.Fill;
				return bounds;
			}
			#endregion

			switch (dockPanel.Dock)
			{
				case DockStyle.Left:
					if (new Rectangle(bounds.Left, bounds.Top, 20, bounds.Height).Contains(pos))
					{
						DockType = DockStyle.Fill;
						return bounds;
					}
					break;

				case DockStyle.Right:
					if (new Rectangle(bounds.Right - 20, bounds.Top, 20, bounds.Height).Contains(pos))
					{
						DockType = DockStyle.Fill;
						return bounds;
					}
					break;

				case DockStyle.Bottom:
					if (new Rectangle(bounds.Left, bounds.Bottom - 20, bounds.Width, 20).Contains(pos))
					{
						DockType = DockStyle.Fill;
						return bounds;
					}
					break;
			}

			return Rectangle.Empty;
		}

		private static Rectangle GetPanelsDockRectangle(Rectangle rect)
		{
			Form form = GetForm();
			foreach (Control control in form.Controls)
			{
				var dockingPanel = control as StiDockingPanel;
				if (dockingPanel != null)
				{
					Rectangle dockRect = GetPanelDockRectangle(dockingPanel, rect);
					if (!dockRect.IsEmpty)return dockRect;
				}
			}
			
			foreach (var dockingPanel in panel.Manager.UndockedPanels)
			{
				Rectangle dockRect = GetPanelDockRectangle(dockingPanel, rect);
				if (!dockRect.IsEmpty)return dockRect;
			}

			return Rectangle.Empty;
		}

		internal static Rectangle GetDockRectangle(Rectangle rect)
		{
			Rectangle dockRect = GetPanelsDockRectangle(rect);
			if (!dockRect.IsEmpty)return dockRect;

			dockRect = GetWindowDockRectangle(rect);
			if (!dockRect.IsEmpty)return dockRect;

			return rect;
		}


		internal static void RefreshDockingWindow(Rectangle rect)
		{
			DockingWindow.Bounds = GetDockRectangle(rect);
		}

		internal static void StopDockingWindow()
		{
			if (DockingWindow != null)
			{
				DockingWindow.Close();
				DockingWindow.Dispose();
				DockingWindow = null;
			}
		}

		internal static void StartDockingWindow(StiDockingPanel pn, Rectangle rect)
		{
			panel = pn;
			DockingWindow = new StiDockingWindow();
			
			Win32.SetWindowPos(
				DockingWindow.Handle, 
				new IntPtr((int)-1),
				0, 0, rect.Width, rect.Height, 
				(int)Win32.SetWindowPosFlags.SWP_NOACTIVATE |
				(int)Win32.SetWindowPosFlags.SWP_SHOWWINDOW | 
				(int)Win32.SetWindowPosFlags.SWP_NOSIZE | 
				(int)Win32.SetWindowPosFlags.SWP_NOMOVE);
				
		}
	}
}
