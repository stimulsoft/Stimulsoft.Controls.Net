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
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls
{
	[ToolboxItem(false)]
	internal class StiControlResize : Control
	{
		#region Handlers
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.FillRectangle(SystemBrushes.ControlDark, ResizeControl.ClientRectangle);
		}

		#endregion

		#region Fields
		private static StiControlResize ResizeControl;
		private static Rectangle startResizeRectangle;
		private static Rectangle lastResizeRectangle;
		private static StiDockingPanel dockingPanel;
		private static int minSize = 66;
		private static int maxSize = 400;
		private static Point lastResizePos;
		#endregion
		
		#region Methods
		/// <summary>
		/// Start resize Docking.
		/// </summary>
		public static void StartResize(StiDockingPanel panel, Rectangle rect, int minimumSize, int maximumSize)
		{
			minSize = minimumSize;
			maxSize = maximumSize;

			dockingPanel = panel;
			startResizeRectangle = lastResizeRectangle = rect;
			if (ResizeControl != null)
			{
				ResizeControl.Dispose();
				ResizeControl = null;
			}
			ResizeControl = new StiControlResize();
			ResizeControl.Bounds = rect;
			dockingPanel.Parent.Controls.Add(ResizeControl);
			dockingPanel.Parent.Controls.SetChildIndex(ResizeControl, 0);
			dockingPanel.IsResizing = true;
			lastResizePos = Cursor.Position;
		}

		/// <summary>
		/// Refreshes resize control.
		/// </summary>
		public static void RefreshResize()
		{
			Rectangle rect;
			switch (dockingPanel.Dock)
			{
				case DockStyle.Left:
				case DockStyle.Right:
					rect = lastResizeRectangle;
					rect.Offset(Cursor.Position.X - lastResizePos.X, 0);
					lastResizeRectangle = rect;
					rect.X = Math.Max(minSize, rect.X);
					rect.X = Math.Min(maxSize, rect.X);
					ResizeControl.Bounds = rect;
					break;

				case DockStyle.Top:
				case DockStyle.Bottom:
					rect = lastResizeRectangle;
					rect.Offset(0, Cursor.Position.Y - lastResizePos.Y);
					lastResizeRectangle = rect;
					rect.Y = Math.Max(minSize, rect.Y);
					rect.Y = Math.Min(maxSize, rect.Y);
					ResizeControl.Bounds = rect;
					break;
			}
			lastResizePos = Cursor.Position;
		}

		public static void StopResize()
		{
			StopResize(false);
		}
		
		/// <summary>
		/// Stoped resise.
		/// </summary>
		public static void StopResize(bool cancel)
		{
			if (!cancel)
			{
				Rectangle rect = ResizeControl.Bounds;		

				switch (dockingPanel.Dock)
				{
					case DockStyle.Left:
						dockingPanel.Width += rect.Left - startResizeRectangle.Left;
						break;

					case DockStyle.Right:
						dockingPanel.Width -= rect.Left - startResizeRectangle.Left;
						break;

					case DockStyle.Top:
						dockingPanel.Height += rect.Top - startResizeRectangle.Top;
						break;

					case DockStyle.Bottom:
						dockingPanel.Height -= rect.Top - startResizeRectangle.Top;
						break;			
				}
			}

			dockingPanel.Parent.Controls.Remove(ResizeControl);

			ResizeControl.Dispose();
			ResizeControl = null;
			dockingPanel.IsResizing = false;
		}

		#endregion

		#region Constructors
		public StiControlResize()
		{
            SetStyle(ControlStyles.DoubleBuffer, true);
		}
		#endregion
	}
}
