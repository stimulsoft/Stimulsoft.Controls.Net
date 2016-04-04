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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Manage collections of docking control.
	/// </summary>
    #if !Profile
	[Designer(typeof(StiDockingPanelDesigner))]
    #endif
	[ToolboxItem(false)]
    public class StiDockingPanel : Panel
    {
		#region Handlers
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			StiColors.InitColors();
		}

		protected override void OnPaint(PaintEventArgs e)
		{			
			Graphics g = e.Graphics;

			foreach (Control control in Controls)
			{
				if (control != this.SelectedControl && control.Visible)control.Hide();
			}

			#region Draw resizebar
			if ((!Collapsed) && (!ResizeBounds.IsEmpty))
			{
				e.Graphics.FillRectangle(StiBrushes.ContentDark, ResizeBounds);
			}
			#endregion

			#region Draw Title
			if ((!TitleBounds.IsEmpty) && TitleBounds.Width != 0 && TitleBounds.Height != 0)
			{
				Color textColor = SystemColors.ControlText;

				if (this.IsSelected)
				{
					using (Brush brush = StiBrushes.GetActiveCaptionBrush(TitleBounds, 90))
					{
						g.FillRectangle(brush, TitleBounds);
						textColor = SystemColors.ActiveCaptionText;
					}
				}
				else 
				{
					using (Brush brush = StiBrushes.GetControlBrush(TitleBounds, 90))
						g.FillRectangle(brush, TitleBounds);
				}				
				
				g.DrawRectangle(SystemPens.ControlDark, PanelBounds.X, PanelBounds.Y, 
					PanelBounds.Width - 1, PanelBounds.Height - 1);
				
				g.DrawRectangle(SystemPens.ControlDark, 
					TitleBounds.X, TitleBounds.Y, TitleBounds.Width - 1, TitleBounds.Height - 1);
				
				
				#region Draw Image
				int imageWidth = 0;
				if (SelectedControl != null && SelectedControl.Image != null && TitleBounds.Width > 55)
				{
					imageWidth = SelectedControl.Image.Width;

					Rectangle imageRect = new Rectangle(TitleBounds.X + 3, 
						TitleBounds.Y + (TitleBounds.Height - SelectedControl.Image.Height) / 2, 
						imageWidth, SelectedControl.Image.Height);
				
					if (imageWidth != 0)g.DrawImage(SelectedControl.Image, imageRect);
				}
				#endregion

				#region Draw DockingControl text
				if (this.SelectedControl != null)
				{
					using (var sf = new StringFormat())
					{
						sf.LineAlignment = StringAlignment.Center;
						if (this.RightToLeft == RightToLeft.Yes)
							sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

						sf.FormatFlags = StringFormatFlags.NoWrap;
						sf.Trimming = StringTrimming.EllipsisCharacter;
						sf.HotkeyPrefix = HotkeyPrefix.Hide;

						if (this.RightToLeft == RightToLeft.Yes)
							sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
					
						using (Brush brush = new SolidBrush(textColor))
						{
							Rectangle textBounds = TextBounds;
							if (!textBounds.IsEmpty)
							{
								g.DrawString(this.SelectedControl.Text, Font, brush, textBounds, sf);
							}
						}
					}
				}
				#endregion				

				if (FloatingForm == null && Width > (48 + AutoHideBarSize) && ShowHideButton)PaintAutoHideButton(g);
				if (ShowCloseButton)PaintCloseButton(g);
			}
			#endregion			

			#region Draw Tabs
			if ((!this.AutoHide) && (Controls.Count > 1 || DesignMode) && TabsBounds.Width != 0 && TabsBounds.Height != 0)
			{
				#region Draw controls
				g.FillRectangle(StiBrushes.ContentDark, TabsBounds);

				for (int index = Controls.Count - 1; index >= 0; index--)
				{
					StiDockingControl dockingControl = Controls[index] as StiDockingControl;
					if (dockingControl != SelectedControl)DrawDockingControl(g, dockingControl);
				}

				if (SelectedControl != null)DrawDockingControl(g, SelectedControl);
				#endregion
			}
			#endregion

			#region AutoHide buttons
			
			if (this.AutoHide && AutoHideBarBounds.Width != 0 && AutoHideBarBounds.Height != 0)
			{
				#region Draw autohide panel
				e.Graphics.FillRectangle(StiBrushes.ContentDark, AutoHideBarBounds);
				#endregion

				foreach (StiDockingControl control in Controls)
				{
					Rectangle rect = GetAutoHideControlBounds(control);

					if (rect.Width > 0 && rect.Height > 0)
					{

						#region Draw panels
						if (Dock == DockStyle.Left || Dock == DockStyle.Right)
						{
							if (rect.Contains(PointToClient(Cursor.Position)) && (!DesignMode))
								using (Brush brush = StiBrushes.GetControlLightBrush(rect, 0))
									g.FillRectangle(brush, rect);
							else
								using (Brush brush = StiBrushes.GetControlBrush(rect, 0))
									g.FillRectangle(brush, rect);
						}
						else 
						{
							if (rect.Contains(PointToClient(Cursor.Position)) && (!DesignMode))
								using (Brush brush = StiBrushes.GetControlLightBrush(rect, 90))
									g.FillRectangle(brush, rect);
						
							else
								using (Brush brush = StiBrushes.GetControlBrush(rect, 90))
									g.FillRectangle(brush, rect);
						}
						g.DrawRectangle(SystemPens.ControlDark, rect);
						#endregion
						
						#region Image
						int imageSize = 0;
						if (control.Image != null)
						{
							Rectangle imageRect = Rectangle.Empty;

							if (this.Dock == DockStyle.Left || this.Dock == DockStyle.Right)
							{
								imageSize = control.Image.Height + 2;

								imageRect = new Rectangle(
									rect.X + (rect.Width - control.Image.Width) / 2, 
									rect.Y + 2, 
									control.Image.Width, control.Image.Height);
							}
							else if (this.Dock == DockStyle.Top || this.Dock == DockStyle.Bottom)
							{
								imageSize = control.Image.Width + 2;

								imageRect = new Rectangle(rect.X + 2, 
									rect.Y + (rect.Height - control.Image.Height) / 2, 
									control.Image.Width, control.Image.Height);

								
							}
							g.DrawImage(control.Image, imageRect);
						}
						#endregion
					
						#region Draw text
						if (control == SelectedControl)
						{
							using (var sf = new StringFormat())
							{
								sf.Alignment = StringAlignment.Near;
								sf.LineAlignment = StringAlignment.Center;
								sf.FormatFlags = StringFormatFlags.NoWrap;
			
								if (this.RightToLeft == RightToLeft.Yes)
									sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

								if ((this.Dock == DockStyle.Right) || (this.Dock == DockStyle.Left))
								{
									rect.Y = rect.Y + imageSize;
									rect.Height = rect.Height - imageSize;

									StiControlPaint.DrawString(g, control.Text, Font, 
										SystemBrushes.ControlDarkDark, rect, sf, 90);
								}
								else
								{
									rect.X = rect.X + imageSize;
									rect.Width = rect.Width - imageSize;

									StiControlPaint.DrawString(g, control.Text, Font, 
										SystemBrushes.ControlDarkDark, rect, sf, 0);
								}
							}
						}
						#endregion
					}
				}
			}
			#endregion
		}

		protected override void OnDockChanged(EventArgs e)
		{
			base.OnDockChanged(e);
			SetLayout();
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			Invalidate();
			if (AutoHide)autoHideTimer.Enabled = true;
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			Invalidate();
		}
		
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
			
			if ((e.Button & MouseButtons.Left) > 0)
			{
				this.Focus();

				#region Check buttons pressed
				if (ShowCloseButton && CloseButtonBounds.Contains(e.X, e.Y))
				{
					IsCloseButtonPressed = true;
					InvalidateTitle();
					return;
				}

				if (ShowHideButton && (this.FloatingForm == null && 
					AutoHideButtonBounds.Contains(e.X, e.Y)) && Width > 50)
				{
					IsAutoHideButtonPressed = true;
					InvalidateTitle();
					return;
				}
				#endregion
				
				#region Check DockingControls tab pressed
				if (!AutoHide)
				{
					StiDockingControl control = GetDockingControlAt(e.X, e.Y);
					if (control != null)
					{
						this.SelectedControl = control;
                    
						if (Manager != null && (!Manager.LockDockingPanels))isMoving = true;

						stopDockingRectanle = TabsBounds;
						isDockingControl = true;
						isDockingWindowActive = false;
						dockingWindowRectanle.Location = PointToScreen(ClientRectangle.Location);
						dockingWindowRectanle.Width = FloatingWidth;
						dockingWindowRectanle.Height = FloatingHeight;
						dockingWindowRectanle.Y = Cursor.Position.Y - 5;
						if (!dockingWindowRectanle.Contains(Cursor.Position))
						{
							dockingWindowRectanle.X = Cursor.Position.X - dockingWindowRectanle.Width / 2;
						}
						lastPosition = Cursor.Position;
						return;
					}
				}
				#endregion

				#region Check title pressed
				if ((!AutoHide) && TitleBounds.Contains(PointToClient(Cursor.Position)))
				{
					if (FloatingForm != null)
					{
						if (Manager != null && (!Manager.LockDockingPanels))isFloatingMoving = true;
						isDockingWindowActive = false;
						isDockingControl = false;
					}
					else 
					{
						if (Manager != null && (!Manager.LockDockingPanels))isMoving = true;
						stopDockingRectanle = TitleBounds;
						isDockingControl = false;
						isDockingWindowActive = false;
						dockingWindowRectanle.Location = PointToScreen(ClientRectangle.Location);
						dockingWindowRectanle.Width = FloatingWidth;
						dockingWindowRectanle.Height = FloatingHeight;
						if (!dockingWindowRectanle.Contains(Cursor.Position))
						{
							dockingWindowRectanle.X = Cursor.Position.X - dockingWindowRectanle.Width / 2;
						}

					}
					lastPosition = Cursor.Position;
				}
				#endregion

				#region ResizeBounds
				if (ResizeBounds.Contains(e.X, e.Y) && (!Collapsed) && AllowResize)
				{
					StiControlResize.StartResize(this, 
						Parent.RectangleToClient(RectangleToScreen(ResizeBounds)), 
						GetMinimumSize(), 
						GetMaximumSize());
				}
				#endregion
			
				#region Buttons
				if(AutoHide)SelectControl(e.X, e.Y);
				#endregion
			}			
			
			if ((e.Button & MouseButtons.Right) > 0)
			{
				if (IsResizing)
				{
					StiControlResize.StopResize(true);
				}

				isMoving = false;
				isFloatingMoving = false;

				if (isDockingWindowActive)
				{
					isDockingWindowActive = false;
					StiDockingWindow.StopDockingWindow();
				}
			}
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
			if (this.AutoHide)
			{
				autoHideTimer.Enabled = false;
				autoHideTimer.Enabled = true;
			}
            if ((!IsCloseButtonHover) && (!IsAutoHideButtonHover))return;
            IsCloseButtonHover = false;
            IsAutoHideButtonHover = false;            
			this.Cursor = Cursors.Default;
			

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

			#region Floating form moving
			if (isFloatingMoving)
			{
				int dx = lastPosition.X - Cursor.Position.X;
				int dy = lastPosition.Y - Cursor.Position.Y;
                FloatingForm.Left -= dx;
				FloatingForm.Top -= dy;

				lastPosition = Cursor.Position;
				StiDockingWindow.panel = this;

				Rectangle rect = StiDockingWindow.GetDockRectangle(
					FloatingForm.RectangleToScreen(FloatingForm.ClientRectangle));

				if (rect != FloatingForm.RectangleToScreen(FloatingForm.ClientRectangle))
				{
					if (!isDockingWindowActive)
					{
						isDockingWindowActive = true;
						StiDockingWindow.StartDockingWindow(this, rect);
					}
					else StiDockingWindow.RefreshDockingWindow(rect);
				}
				else
				{
					if (isDockingWindowActive)
					{
						isDockingWindowActive = false;
						StiDockingWindow.StopDockingWindow();
					}
				}
			}
			#endregion

			#region Dock moving
			else if (isMoving)
			{
				Point pos = this.PointToClient(Cursor.Position);
				int dx = lastPosition.X - Cursor.Position.X;
				int dy = lastPosition.Y - Cursor.Position.Y;
				dockingWindowRectanle.X -= dx;
				dockingWindowRectanle.Y -= dy;

				if (isDockingControl)
				{
					StiDockingControl control = GetDockingControlAt(e.X, e.Y);
					
					if (control != null)
					{
						int index = Controls.IndexOf(control);
						int selectedIndex = Controls.IndexOf(SelectedControl);
					
						Controls.SetChildIndex(control, selectedIndex);
						Controls.SetChildIndex(SelectedControl, index);
						Invalidate();
					}
				}
				
				if (stopDockingRectanle.Contains(pos))
				{
					if (isDockingWindowActive)
					{
						isDockingWindowActive = false;
						StiDockingWindow.StopDockingWindow();
					}
				}
				else
				{
					if (!isDockingWindowActive)
					{
						isDockingWindowActive = true;
						StiDockingWindow.StartDockingWindow(this, dockingWindowRectanle);
					}
					else StiDockingWindow.RefreshDockingWindow(dockingWindowRectanle);
				}				
				lastPosition = Cursor.Position;
			}
			#endregion

			else 
			{
				IsCloseButtonHover = false;
				IsAutoHideButtonHover = false;		
			
				IsCloseButtonHover = CloseButtonBounds.Contains(e.X, e.Y);
				IsAutoHideButtonHover = AutoHideButtonBounds.Contains(e.X, e.Y);
				
				InvalidateTitle();

				if (AutoHideBarBounds.Contains(e.X, e.Y))InvalidateAutoHideBar();
				if (TabsBounds.Contains(e.X, e.Y))InvalidateTabs();
			
				if (IsResizing)
				{
					StiControlResize.RefreshResize();
					Cursor.Current = GetResizeCursor();
				}
				else 
				{
					if (ResizeBounds.Contains(e.X, e.Y) && AllowResize)
					{
						Cursor.Current = GetResizeCursor();
					}
					else Cursor.Current = Cursors.Arrow;
				}				
			}
		}

        protected override void OnMouseUp(MouseEventArgs e)
        {
			base.OnMouseUp(e);

			if ((e.Button & MouseButtons.Left) > 0)
			{
				#region isMoving
				if (isMoving && isDockingWindowActive)
				{
					isDockingWindowActive = false;
					StiDockingWindow.StopDockingWindow();
					Rectangle rect = StiDockingWindow.GetDockRectangle(dockingWindowRectanle);
					
					if (rect != dockingWindowRectanle)DoDock();
					else
					{
						if (isDockingControl && Controls.Count > 1)
							UndockControl(SelectedControl, dockingWindowRectanle, true, this.Parent);
						else UndockPanel(dockingWindowRectanle, true);
						SetLayoutAllPanels();
					}
				}
				#endregion

				#region isFloatingMoving
				else if (isFloatingMoving && isDockingWindowActive)
				{
					isDockingWindowActive = false;
					StiDockingWindow.StopDockingWindow();
					Rectangle rect = StiDockingWindow.GetDockRectangle(dockingWindowRectanle);
					if (!rect.IsEmpty)
					{
						DoDock();
					}
				}
				#endregion
			
				#region Close button
				if (ShowCloseButton && IsCloseButtonPressed)
				{
					IsCloseButtonPressed = false;
					InvalidateTitle();
            
					if (IsCloseButtonHover && (!Collapsed))
					{	
						if (this.floatingForm != null)
						{
							while (this.Controls.Count > 0)
							{
								DoClose(this.SelectedControl);
							}
						}
						else DoClose(this.SelectedControl);

						SetLayoutAllPanels();
					}
				}
				#endregion

				#region AutoHide button
				if (IsAutoHideButtonPressed)
				{
					IsAutoHideButtonPressed = false;
					InvalidateTitle();
					if (IsAutoHideButtonHover)
					{
						this.AutoHide = ! this.AutoHide;
						SetLayoutAllPanels();
					}
				}
				#endregion

				if (IsResizing)
				{
					StiControlResize.StopResize();
					SetLayout();
				}

				isFloatingMoving = false;
				isMoving = false;
			}
            
        }

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			SetLayout();
			Invalidate();
		}

		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);			
			if (SelectedControl == null)
			{
				SelectedControl = e.Control as StiDockingControl;
			}
			e.Control.Dock = DockStyle.Fill;
			SetLayout();
			Invalidate();
		}
		
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);
			
			if (SelectedControl == e.Control)
			{
				if (Controls.Count == 0)SelectedControl = null;
				else SelectedControl = Controls[0] as StiDockingControl;
			}
			SetLayout();
			Invalidate();
		}
		protected override void WndProc(ref Message msg)
		{
			if (msg.Msg == (int)Win32.Msg.WM_MOUSELEAVE)
			{
				InvalidateTitle();
				InvalidateTabs();
				InvalidateAutoHideBar();
			}
			base.WndProc(ref msg);
		}

		private void OnAutoHideTick(object sender, EventArgs e)
		{
			if (Parent != null && 
				AutoHide &&
				(!Collapsed) &&
				(!this.ContainsFocus) && 
				(!this.ClientRectangle.Contains(PointToClient(Cursor.Position))))
			{
				autoHideTimer.Enabled = false;
				Collapsed = true;
				SetLayoutAllPanels();
			}
		}

		#endregion

		#region Methods
		public void DoClose(Control control)
		{
			var e = new CancelEventArgs(false);
		    var docking = (StiDockingControl) control;
            docking.InvokeClosing(e);

			if (!e.Cancel)
			{
                if (!this.manager.ClosedControls.Contains(docking))
                    this.manager.ClosedControls.Add(docking);

				if (floatingForm != null)
				{
					Controls.Remove(control);
					if (Controls.Count == 0)
					{
						this.manager.RemoveUndockingPanels(this);
						Parent.Controls.Remove(this);
					}

                    docking.InvokeClosed(EventArgs.Empty);
					SetLayoutAllPanels();
					return;
				}
				else if ((Controls.Count > 1) && (control != null))
				{
				    Controls.Remove(control);
                    docking.InvokeClosed(EventArgs.Empty);
				    Invalidate();
				    return;
				}
				else
				{
				    if (Parent != null)Parent.Controls.Remove(this);
				}

                docking.InvokeClosed(EventArgs.Empty);
			}
		}

		
		private void DoDock()
		{
			#region DockControl
			if (isDockingControl && Controls.Count > 1)
			{
				if (StiDockingWindow.ControlToDock is Form)
				{
					DockControlToForm(
						SelectedControl,
						StiDockingWindow.ControlToDock as Form, 
						StiDockingWindow.DockType);
				}
				else
				{
					DockControlToPanel(StiDockingWindow.ControlToDock as StiDockingPanel);
				}								
			}
			#endregion

			#region DockPanel
			else 
			{
				if (StiDockingWindow.ControlToDock is Form)
				{
					DockPanelToForm(
						StiDockingWindow.ControlToDock as Form, 
						StiDockingWindow.DockType);
				}
				else
				{
					DockPanelToPanel(StiDockingWindow.ControlToDock as StiDockingPanel);
				}
			}
			#endregion							

			SetLayoutAllPanels();
		}


		private StringFormat GetStringFormatTabs()
		{
			var sfTab = new StringFormat
			{
			    LineAlignment = StringAlignment.Center,
			    Alignment = StringAlignment.Center,
			    FormatFlags = StringFormatFlags.NoWrap,
			    Trimming = StringTrimming.EllipsisCharacter
			};

		    if (this.RightToLeft == RightToLeft.Yes)
				sfTab.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

			return sfTab;
		}

	
		internal void SetLayoutAllPanels()
		{
			if (manager != null && manager.ParentForm != null)
			{
				foreach (Control control in manager.ParentForm.Controls)
				{
					if (control is StiDockingPanel)
					{
						((StiDockingPanel)control).SetLayout();
					}
				}

				foreach (var control in manager.UndockedPanels)
				{
					control.SetLayout();
				}
			}			
		}

		internal void SetLayout()
		{
			try
			{
				if (Manager != null && (!Manager.LockLayout))
				{
					this.Refresh();
					switch (Dock)
					{			
						#region Top
						case DockStyle.Top:
						{
							DockPadding.Left = 1;
							DockPadding.Right = 1;
							DockPadding.Top = AutoHideBarSize + TitleHeight;
							DockPadding.Bottom = ResizeBarSize + TabsBarSize;

							resizeBounds = new Rectangle(0, Height - ResizeBarSize, Width, ResizeBarSize);
							titleBounds = new Rectangle(0, AutoHideBarSize, Width, TitleHeight);
							panelBounds = new Rectangle(0, AutoHideBarSize, Width, Height - ResizeBarSize - AutoHideBarSize);
							tabsBounds = new Rectangle(0, Height - TabsBarSize - ResizeBarSize, Width - AutoHideBarSize, TabsBarSize);
							break;					
						}
						#endregion

						#region Bottom
						case DockStyle.Bottom:
						{
							DockPadding.Left = 1;
							DockPadding.Right = 1;
							DockPadding.Top = ResizeBarSize + TitleHeight;
							DockPadding.Bottom = AutoHideBarSize + TabsBarSize;

							resizeBounds = new Rectangle(0, 0, Width, ResizeBarSize);
							titleBounds = new Rectangle(0, ResizeBarSize, Width, TitleHeight);
							panelBounds = new Rectangle(0, ResizeBarSize, Width, Height - ResizeBarSize - AutoHideBarSize);
							tabsBounds = new Rectangle(0, Height - TabsBarSize, Width - AutoHideBarSize, TabsBarSize);
							break;
						}
						#endregion

						#region Left
						case DockStyle.Left:
						{
							DockPadding.Left = AutoHideBarSize + 1;
							DockPadding.Right = ResizeBarSize + 1;
							DockPadding.Top = TitleHeight;
							DockPadding.Bottom = TabsBarSize;
						
							resizeBounds = new Rectangle(Width - ResizeBarSize, 0, ResizeBarSize, Height);					
							titleBounds = new Rectangle(AutoHideBarSize, 0, Width - ResizeBarSize - AutoHideBarSize, TitleHeight);
							panelBounds = new Rectangle(AutoHideBarSize, 0, Width - ResizeBarSize - AutoHideBarSize, Height);
							tabsBounds = new Rectangle(0, Height - TabsBarSize, Width - ResizeBarSize - AutoHideBarSize, TabsBarSize);
							break;
						}
						#endregion

						#region Right
						case DockStyle.Right:
						{
							DockPadding.Left = ResizeBarSize + 1;
							DockPadding.Right = AutoHideBarSize + 1;
							DockPadding.Top = TitleHeight;
							DockPadding.Bottom = TabsBarSize;
					
							resizeBounds = new Rectangle(0, 0, ResizeBarSize, Height);
							titleBounds = new Rectangle(ResizeBarSize, 0, Width - ResizeBarSize - AutoHideBarSize, TitleHeight);
							panelBounds = new Rectangle(ResizeBarSize, 0, Width - ResizeBarSize - AutoHideBarSize, Height);
							tabsBounds = new Rectangle(ResizeBarSize, Height - TabsBarSize, Width - ResizeBarSize - AutoHideBarSize, TabsBarSize);
							break;
						}
						#endregion

						#region Fill
						case DockStyle.Fill:
						{
							DockPadding.Left = 1;
							DockPadding.Right = 1;
							DockPadding.Top = TitleHeight;
							DockPadding.Bottom = TabsBarSize;
					
							resizeBounds = new Rectangle(0, 0, 0, Height);
							titleBounds = new Rectangle(0, 0, Width,  TitleHeight);
							panelBounds = new Rectangle(0, 0, Width, Height);
							tabsBounds = new Rectangle(0, Height - TabsBarSize, Width, TabsBarSize);
							break;
						}
						#endregion
					}

					foreach (StiDockingControl control in Controls)
					{
						if (control.Visible && SelectedControl != control)
						{
							control.Visible = false;
							selectedControl = SelectedControl;
							selectedControl.Visible = true;
							break;
						}
					}
					base.UpdateStyles();
				}
			}
			catch
			{
			}

		}
		

		private int GetMinimumSize()
		{
			switch(Dock)
			{
				case DockStyle.Right:
					if (!AutoHide)return Bounds.Right - 400;
					else return Bounds.Right - AutoHideBarSize - 400;

				case DockStyle.Bottom:
					if (!AutoHide)return Bounds.Bottom - 400;
					else return Bounds.Bottom - AutoHideBarSize - 400;

				case DockStyle.Left:
					if (!AutoHide)return Left + 30;
					else return Left + AutoHideBarSize + 30;

				default:
					if (!AutoHide)return Top + 50;
					else return Top + AutoHideBarSize + 50;
			}
		}

		private int GetMaximumSize()
		{
			switch(Dock)
			{
				case DockStyle.Right:
					if (!AutoHide)return Bounds.Right - 34;
					else return Bounds.Right - AutoHideBarSize - 34;

				case DockStyle.Bottom:
					if (!AutoHide)return Bounds.Bottom - 54;
					else return Bounds.Bottom - AutoHideBarSize - 54;

				case DockStyle.Left:
					if (!AutoHide)return 400 + Left;
					else return AutoHideBarSize + 400 + Left;

				default:
					if (!AutoHide)return 400 + Top;
					else return AutoHideBarSize + 400 + Top;
					
			}
		}


		private void InvalidateAutoHideBar()
		{			
			if (AutoHideBarBounds.Width != 0 && AutoHideBarBounds.Height != 0)Invalidate(AutoHideBarBounds);
		}

		internal void InvalidateTitle()
		{
			if (TitleBounds.Width != 0 && TitleBounds.Height != 0)Invalidate(TitleBounds);
		}

		internal void InvalidateTabs()
		{
			if (TabsBounds.Width != 0 && TabsBounds.Height != 0)Invalidate(TabsBounds);
		}

		
		internal Cursor GetResizeCursor()
		{
			if (!Collapsed)
			{
				switch (Dock)
				{
					case DockStyle.Left:
					case DockStyle.Right:
						return Cursors.VSplit;

					case DockStyle.Top:
					case DockStyle.Bottom:
						return Cursors.HSplit;
				}
			}
			return Cursors.Arrow;			
		}			
		

		private StiDockingControl GetControlAt(int x, int y)
		{
			if (AutoHide)
			{
				foreach (StiDockingControl control in Controls)
				{
					Rectangle rect = GetAutoHideControlBounds(control);
					if (rect.Contains(x, y))
					{
						return control;
					}
				}
			}
			return null;
		}

		private void SelectControl(int x, int y)
		{
			StiDockingControl control = GetControlAt(x, y);
			if (control != null)
			{
				if (Collapsed)Collapsed = false;
				SelectedControl = control;
				SetLayoutAllPanels();
				return;
			}
		}


		internal void DockPanel(Form form)
		{
			form.Controls.Add(this);
			Manager.RemoveUndockingPanels(this);
			SetLayout();
			this.Focus();
			this.LastDockForm = null;
		}


		public void DockControlToForm(StiDockingControl control, Form form, DockStyle dockType)
		{
			var pn = UndockControl(control, Rectangle.Empty, false, null);
			pn.DockPanelToForm(form, dockType);
			pn.Focus();
		}

		public void DockControlToPanel(StiDockingPanel panel)
		{
            var pn = UndockControl(SelectedControl, Rectangle.Empty, false, this.Parent);
			pn.DockPanelToPanel(panel);
		}
		
		public void DockPanelToPanel(StiDockingPanel panel)
		{
            var al = new List<Control>();

			foreach (Control control in this.Controls)
			{
				al.Add(control);
			}

			panel.Controls.AddRange(al.ToArray());		

			if (Parent != null)
                this.Parent.Controls.Remove(this);

			panel.SetLayout();

			if (FloatingForm != null)
                panel.manager.RemoveUndockingPanels(this);

			this.Focus();
		}
				
		public void DockPanelToForm(Form form, DockStyle dockType)
		{
			StiDockingPanel dockingPanel = null;
			foreach (Control control in form.Controls)
			{
				if (control is StiDockingPanel && control.Dock == dockType)
				{
					dockingPanel = control as StiDockingPanel;
					break;
				}
			}

			if (dockingPanel == null)
			{
				dockingPanel = new StiDockingPanel(Manager)
				{
				    Width = FloatingWidth,
				    Height = FloatingHeight/2,
				    Dock = dockType,
				    LastDockForm = null
				};
			    form.Controls.Add(dockingPanel);
				form.Controls.SetChildIndex(dockingPanel, 0);
				
				foreach (Control control in form.Controls)
				{
					if (control.Dock == DockStyle.Fill)
					{
						form.Controls.SetChildIndex(control, 0);
						break;
					}
				}
			}
			DockPanelToPanel(dockingPanel);
		}
		
		
		public void UndockPanel(Rectangle rect, bool createForm)
		{
			rect.Size = new Size(FloatingWidth, FloatingHeight);
			
			if (createForm)
			{
				Manager.AddUndockingPanels(this);

				var form = new StiFloatingForm(this)
				{
				    AutoScaleMode = AutoScaleMode.None
				};

			    if (this.LastDockForm == null)
				{
					if (this.Parent != null && Manager.ShowingUndocked)((Form)this.Parent).AddOwnedForm(form);
					this.LastDockForm = this.Parent as Form;
					this.LastDock = this.Dock;
				}
				else if (Manager.ShowingUndocked)this.LastDockForm.AddOwnedForm(form);
           
				if (rect.Left == -1)
				{
					form.StartPosition = FormStartPosition.CenterScreen;
					form.ClientSize = new Size(rect.Width, rect.Height);
				}
				else
				{
					form.Location = rect.Location;
					form.ClientSize = new Size(rect.Width, rect.Height);
				}			
				form.Controls.Add(this);
				if (Manager.ShowingUndocked)form.Show();
			}
			this.SetLayout();
			this.Focus();
		}

		public StiDockingPanel UndockControl(StiDockingControl control, Rectangle rect, bool createForm, Control parent)
		{
			var panel = new StiDockingPanel(Manager)
			{
			    LastDockForm = this.LastDockForm, 
                Parent = parent
			};
		    panel.Controls.Add(control);
			panel.UndockPanel(rect, createForm);

			if (this.Controls.Count == 0 && Parent != null)
			{
				Parent.Controls.Remove(this);
			}
			return panel;
		}

		
		private void PaintButton(Graphics g, Rectangle rect, Image image, bool isHover, bool isPressed)
		{
			if (isHover || isPressed)
			{
				Color color = StiColors.Focus;
				if (isPressed)
                    color = Color.FromArgb(100, color);

			    using (var brush = new SolidBrush(color))
			        g.FillRectangle(brush, rect);

				g.DrawRectangle(StiPens.SelectedText, rect);
			}

			if (image != null)
			{
				if (this.IsSelected)
					image = StiImageUtils.ReplaceImageColor((Bitmap)image, SystemColors.ActiveCaptionText, Color.Black);

				g.DrawImage(image, new Rectangle(
					rect.X + (rect.Width - image.Width) / 2 + 1,
					rect.Y + (rect.Height - image.Height) / 2, 
					image.Width, image.Height));
			}
		}

		private void PaintCloseButton(Graphics g)
		{
			if (imageClose != null)
				PaintButton(g, CloseButtonBounds, imageClose, IsCloseButtonHover, IsCloseButtonPressed);
		}

		private void PaintAutoHideButton(Graphics g)
		{
			if (imageAutoHideOn != null && imageAutoHideOff != null)
			{
				if (AutoHide)PaintButton(g, AutoHideButtonBounds, imageAutoHideOn, IsAutoHideButtonHover, IsAutoHideButtonPressed);
				else PaintButton(g, AutoHideButtonBounds, imageAutoHideOff, IsAutoHideButtonHover, IsAutoHideButtonPressed);
			}
		}


		private int GetTabWidth(Graphics g, StiDockingControl control)
		{
			int imageWidth = 0;
			if (control.Image != null)imageWidth = control.Image.Width;
			using (var sf = GetStringFormatTabs())
			{
				return (int)(g.MeasureString(control.Text, Font, 1000, sf).Width) + 10 + imageWidth;
			}
		}

		private int GetTotalWidth(Graphics g)
		{
			int width = 0;
		    foreach (StiDockingControl control in this.Controls)
		    {
		        width += GetTabWidth(g, control);
		    }
			return width;			
		}


		internal Rectangle GetAutoHideControlBounds(StiDockingControl dockingControl)
		{
			using (var g = Graphics.FromHwnd(this.Handle))
			{
				int pos = 0;

				int size = 0;
				switch (Dock)
				{
					case DockStyle.Left:
					case DockStyle.Right:
						size = AutoHideBarBounds.Height;
						break;

					case DockStyle.Top:
					case DockStyle.Bottom:
						size = AutoHideBarBounds.Width;
						break;
				}
	
				foreach (StiDockingControl control in Controls)
				{
					var rect = GetTabRectangle(g, control, size);
					rect.Width -= 10;
					if (control != SelectedControl)rect.Width = 16;

					if (control == dockingControl)
					{
						switch (Dock)
						{
							case DockStyle.Left:
								return new Rectangle(-1, pos, 20, rect.Width + 5);
							case DockStyle.Right:
								return new Rectangle(this.Width - 20, pos, 20, rect.Width + 5);
							case DockStyle.Top:
								return new Rectangle(pos, -1, rect.Width + 5, 20);
							case DockStyle.Bottom:
								return new Rectangle(pos, this.Height - 20, rect.Width + 5, 20);
						}
					}
					pos += rect.Width + 8;					
				}
				return Rectangle.Empty;
			}
		}

		internal Rectangle GetTabRectangle(Graphics g, StiDockingControl dockingControl)
		{
			return GetTabRectangle(g, dockingControl, 0);
		}

		internal Rectangle GetTabRectangle(Graphics g, StiDockingControl dockingControl, int size)
		{
			int startPos = 5 + TabsBounds.X;
			int width = 10;
			int allWidth = GetTotalWidth(g);

			if (size == 0)size = TabsBounds.Width;

			double k = 1;
			if (allWidth + startPos * 2 + 5 > size)
				k = (double)size / ((double)allWidth + (double)startPos * 2 + 5);

			foreach (StiDockingControl control in this.Controls)
			{
				if (control != dockingControl)startPos += (int)(GetTabWidth(g, control) * k);
				else 
				{
					width = (int)(GetTabWidth(g, control) * k);
					break;
				}
			}

			var controlRect = new Rectangle(startPos, TabsBounds.Y, width, TabControlHeight);
			
			return controlRect;
		}


		private void DrawDockingControl(Graphics g, StiDockingControl dockingControl)
		{
			Rectangle rect = GetTabRectangle(g, dockingControl);
			Point [] pts = new Point[]{
										  new Point(rect.X, rect.Y),
										  new Point(rect.X, rect.Bottom),
										  new Point(rect.Right - 5, rect.Bottom),
										  new Point(rect.Right + 5, rect.Y)};


		    using (var path = new GraphicsPath(FillMode.Alternate))
		    {
		        path.AddPolygon(pts);

		        #region Draw dock title

		        Point pos = this.PointToClient(Cursor.Position);
		        if (SelectedControl == dockingControl)
		        {
		            if (rect.Contains(pos))
		            {
		                using (var br = StiBrushes.GetControlLightBrush(rect, 90))
		                {
		                    g.FillPath(br, path);
		                }
		            }
		            else
		            {
		                using (var br = StiBrushes.GetControlBrush(rect, 90))
		                {
		                    g.FillPath(br, path);
		                }
		            }
		        }
		        else
		        {
		            if (rect.Contains(pos))
		            {
		                using (var br = new SolidBrush(StiColorUtils.Light(StiColors.Content, 15)))
		                {
		                    g.FillPath(br, path);
		                }
		            }
		            else
		            {
		                g.FillPath(StiBrushes.Content, path);
		            }
		        }

		        #endregion
		    }

		    Color textColor = ForeColor;
			if (dockingControl != SelectedControl)textColor = SystemColors.ControlDark;

			#region Draw image
			int imageWidth = 0;
			if (dockingControl.Image != null)
			{
				imageWidth = dockingControl.Image.Width + 2;

				Rectangle imageRect = new Rectangle(rect.X + 2, 
					rect.Y + (rect.Height - dockingControl.Image.Height) / 2, 
					dockingControl.Image.Width, dockingControl.Image.Height);
				
				if (imageWidth != 0)g.DrawImage(dockingControl.Image, imageRect);
			}
			#endregion			

			#region Draw text
			var textRect = new Rectangle(rect.X + imageWidth, rect.Y, rect.Width - imageWidth, rect.Height);
			using (var brush = new SolidBrush(textColor))
			using (var sf = GetStringFormatTabs())
			{
			    g.DrawString(dockingControl.Text, Font, brush, textRect, sf);
			}
			#endregion

			#region Draw tab lines
			if (SelectedControl == dockingControl)
			{
				g.DrawLine(SystemPens.ControlLightLight, pts[0], pts[1]);
				g.DrawLine(SystemPens.ControlDark, pts[1], pts[2]);
				g.DrawLine(SystemPens.ControlDark, pts[2], pts[3]);

				g.DrawLine(SystemPens.ControlDark, TabsBounds.X, pts[0].Y, pts[0].X, pts[0].Y);					
				g.DrawLine(SystemPens.ControlDark, pts[3].X, pts[0].Y, TabsBounds.Right - 1, pts[0].Y);
			}
			else
			{
				using (var pen = new Pen(StiColorUtils.Dark(SystemColors.Control, 40)))
				{
					g.DrawLine(pen, pts[0], pts[1]);
					g.DrawLine(pen, pts[1], pts[2]);
					g.DrawLine(pen, pts[2], pts[3]);
				}
			}
			#endregion
		}


		/// <summary>
		/// Retrieves the docking control that is located at the specified coordinates.
		/// </summary>
		/// <param name="p">A Point that contains the coordinates where you want to look for a control. 
		/// Coordinates are expressed relative to the upper-left corner of the control client area.</param>
		/// <returns>A docking control that represents the control that is located at the specified point.</returns>
		public StiDockingControl GetDockingControlAt(int x, int y)
		{
			using (var g = Graphics.FromHwnd(this.Handle))
			{
				foreach (StiDockingControl control in Controls)
				{
					var rectangle = GetTabRectangle(g, control);
					if (!rectangle.Contains(x, y))
					{
						continue;
					}
					return control;
				}
				return null;
			}
		}


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (imageClose != null)imageClose.Dispose();
				if (imageAutoHideOn != null)imageAutoHideOn.Dispose();
				if (imageAutoHideOff != null)imageAutoHideOff.Dispose();
			}
			base.Dispose(disposing); 
		}
		#endregion

		#region Properties
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool AllowDrop
		{
			get
			{
				return base.AllowDrop;
			}
			set
			{
				base.AllowDrop = value;
			}
		}


		private StiDockingManager manager = null;
		[Browsable(false)]
		public StiDockingManager Manager
		{
			get
			{
				return manager;
			}
			set
			{
				manager = value;				
			}
		}


		[DefaultValue(1000)]
		public int AutoHideTime
		{
			get
			{
				return autoHideTimer.Interval;
			}
			set
			{
				autoHideTimer.Interval = value;				
			}
		}


		internal int AutoHideBarSize
		{
			get
			{
				return AutoHide ? AutoHideHeight : 0;
			}
		}


		internal int TabsBarSize
		{
			get
			{
				if (Controls.Count > 1 && (!AutoHide))return TabHeight;
				return 1;
			}
		}


		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Rectangle AutoHideBarBounds
		{
			get
			{
				int tbh = AutoHideBarSize;
				if (!AutoHide)tbh = 0;
				switch (Dock)
				{
					case DockStyle.Top:
						return new Rectangle(0, 0, Width, tbh);							

					case DockStyle.Bottom:
						return new Rectangle(0, Height - tbh, Width, tbh);

					case DockStyle.Left:
						return new Rectangle(0, 0, tbh, Height);

					default:
						return new Rectangle(Width - tbh, 0, tbh, Height);
				}
			}
		}


		private bool collapsed = false;
		/// <summary>
		/// Gets or sets a value indicating whether the docking panel is in the collapsed state.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Collapsed
		{
			get
			{
				return collapsed;
			}
			set
			{
				collapsed = value;
				
				if (value)
				{
					CollapsedSize = ClientSize;
					switch (Dock)
					{
						case DockStyle.Left:
						case DockStyle.Right:
							Width = AutoHideBarSize;
							break;

						case DockStyle.Top:
						case DockStyle.Bottom:
							Height = AutoHideBarSize;
							break;
					}
				}
				else
				{
					ClientSize = CollapsedSize;
				}
			}
		}


		private Rectangle resizeBounds;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal Rectangle ResizeBounds
		{
			get
			{
				return resizeBounds;
			}
		}


		private Rectangle panelBounds;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal Rectangle PanelBounds
		{
			get
			{
				return panelBounds;
			}
		}


		private Rectangle tabsBounds;
		internal Rectangle TabsBounds
		{
			get
			{
				return tabsBounds;
			}
		}


		private Rectangle titleBounds;
		internal Rectangle TitleBounds
		{
			get
			{
				return titleBounds;
			}
		}


		internal Rectangle TextBounds
		{
			get
			{
				int imageWidth = 0;
				if (SelectedControl != null && SelectedControl.Image != null)
				{
					imageWidth = SelectedControl.Image.Width;
				}

				Rectangle textRect = TitleBounds;
				textRect.X += imageWidth + 4;
				textRect.Width -= 5 + imageWidth + buttonWidth;
				if (FloatingForm == null)textRect.Width -= 20;
				if (textRect.Width == 0)return Rectangle.Empty;
				return textRect;
			}
		}


		private bool isSelected = false; 
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal bool IsSelected
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

    
		private bool isAutoHideButtonHover = false;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal bool IsAutoHideButtonHover
		{
			get
			{
				return isAutoHideButtonHover;
			}
			set
			{
				isAutoHideButtonHover = value;
			}
		}


		private bool isCloseButtonHover = false;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal bool IsCloseButtonHover
		{
			get
			{
				return isCloseButtonHover;
			}
			set
			{
				isCloseButtonHover = value;
			}
		}


		private bool isAutoHideButtonPressed = false;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal bool IsAutoHideButtonPressed
		{
			get
			{
				return isAutoHideButtonPressed;
			}
			set
			{
				isAutoHideButtonPressed = value;
			}
		}
		

		private bool isCloseButtonPressed = false;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal bool IsCloseButtonPressed
		{
			get
			{
				return isCloseButtonPressed;
			}
			set
			{
				isCloseButtonPressed = value;
			}
		}

       
		private bool autoHide = false;
		/// <summary>
		/// Gets or sets a value indicating whether the docking panel is in the autohide state.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoHide
        {
            get
            {
                return autoHide;
            }
            set
            {
				autoHide = value;
				
				#region Correct size DockingManager
				switch (Dock)
				{
					case DockStyle.Left:
					case DockStyle.Right:
						Width += AutoHide ? 21 : -21;
						break;
				}
				#endregion

				SetLayout();
				Invalidate();
            }
        }


		private bool showCloseButton = true;
		/// <summary>
		/// Gets or sets a value indicating whether the docking panel is closeable.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[Category("Behavior")]
		public bool ShowCloseButton
		{
			get
			{
				return showCloseButton;
			}
			set
			{
				showCloseButton = value;

				Invalidate();
			}
		}


		private bool showHideButton = true;
		/// <summary>
		/// Gets or sets a value indicating whether the docking panel is may be hide.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[Category("Behavior")]
		public bool ShowHideButton
		{
			get
			{
				return showHideButton;
			}
			set
			{
				showHideButton = value;

				Invalidate();
			}
		}


		private bool allowResize = true;
		[Browsable(true)]
		[DefaultValue(true)]
		[Category("Behavior")]
		public bool AllowResize
		{
			get
			{
				return allowResize;
			}
			set
			{
				allowResize = value;
			}
		}


		private Size collapsedSize;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal Size CollapsedSize
        {
            get
            {
                return collapsedSize;
            }
            set
            {
				collapsedSize = value;
            }
        }


		private StiDockingControl selectedControl = null;
		/// <summary>
		/// Gets or sets the currently-selected control.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StiDockingControl SelectedControl
		{
			get
			{
				return selectedControl;
			}
			set
			{
				foreach (Control control in this.Controls)
				{
					if (control == value)
					{
						try
						{
							control.Dock = DockStyle.Fill;
							control.Show();
								
							break;
						}
						catch
						{
						}
					}
				}
				foreach (Control control in this.Controls)
				{
					if (control != value)control.Hide();
				}

				if (selectedControl != value)
				{
					selectedControl = value;
					this.Invalidate();
				}	
				SetLayout();
			}
		}


		private StiFloatingForm floatingForm;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal StiFloatingForm FloatingForm
		{
			get
			{
				return floatingForm;
			}
			set
			{
				floatingForm = value;
			}
		}


		private Rectangle AutoHideButtonBounds
		{
			get
			{
				Rectangle rect = CloseButtonBounds;
				rect = new Rectangle(rect.X, rect.Y, buttonWidth, rect.Height);
				if (ShowCloseButton)rect.X -= buttonWidth;
				return rect;
			}
		}


		private Rectangle CloseButtonBounds
		{
			get
			{
				Rectangle rect = this.TitleBounds;
				rect = new Rectangle(rect.Right - buttonWidth - 3, rect.Y + 2, buttonWidth, 15);

				if (!ShowCloseButton)rect.Width = 0;

				return rect;
			}
		}

		#endregion
  
		#region Browsable(false)
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override AnchorStyles Anchor
		{
			get
			{
				return base.Anchor;
			}
			set
			{
				base.Anchor = value;
			}
		}


		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Cursor Cursor
		{
			get
			{
				return base.Cursor;
			}
			set
			{
				base.Cursor = value;
			}
		}


		/// <summary>
		/// Do not use this property.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

	
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override Image BackgroundImage
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
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

   
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
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

		#endregion

		#region Fields
		internal bool IsResizing = false;	
		internal const int TabHeight = 24;
		internal const int ResizeBarSize = 4;
		internal const int TabControlHeight = 20;
		internal const int TitleHeight = 20;
		internal const int AutoHideHeight = 23;
		internal const int buttonWidth = 20;
		
		private int floatingWidth = 200;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int FloatingWidth
		{
			get
			{
				return floatingWidth;
			}
			set
			{
				floatingWidth = value;
			}
		}

		private int floatingHeight = 300;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int FloatingHeight
		{
			get
			{
				return floatingHeight;
			}
			set
			{
				floatingHeight = value;
			}
		}

		private bool isDockingWindowActive = false;
		private Rectangle dockingWindowRectanle = Rectangle.Empty;
		private Rectangle stopDockingRectanle;
		private bool isDockingControl = false;

		private bool isFloatingMoving = false;
		private bool isMoving = false;

		private Point lastPosition;

		private Timer autoHideTimer = new Timer();

		private Image imageClose;
		private Image imageAutoHideOn;
		private Image imageAutoHideOff;

		internal Form LastDockForm = null;
		internal DockStyle LastDock;
		#endregion	
		
		#region Constructors
		public StiDockingPanel() : this(null)
		{
		}

		public StiDockingPanel(StiDockingManager manager)
		{
			this.manager = manager;

			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);

			Dock = DockStyle.Right;
			AllowDrop = true;

            autoHideTimer.Interval = 1000;
			autoHideTimer.Tick += new EventHandler(OnAutoHideTick);
			
			imageClose = StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.Close.bmp");
			imageAutoHideOn = StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.AutoHideOn.bmp");
			imageAutoHideOff = StiImageUtils.GetImage("Stimulsoft.Controls", "Stimulsoft.Controls.Bmp.AutoHideOff.bmp");

			base.SetStyle(ControlStyles.ResizeRedraw, true);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.DoubleBuffer, true);
			base.SetStyle(ControlStyles.UserPaint, true);

			SetLayout();
		}
        
		#endregion
    }
}