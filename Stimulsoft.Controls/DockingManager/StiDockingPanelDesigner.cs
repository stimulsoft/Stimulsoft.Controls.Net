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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
    #if !Profile
    internal class StiDockingPanelDesigner : ParentControlDesigner
    {
		#region Methods
		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			return (parentDesigner.Component as Form) != null;
		}

		protected override void WndProc(ref Message msg)
		{
			if (msg.Msg == (int)Win32.Msg.WM_LBUTTONDOWN)
			{
				Point p = dockingPanel.PointToClient(Cursor.Position);
				var control = dockingPanel.GetDockingControlAt(p.X, p.Y);
				if (control != null)
				{
					dockingPanel.SelectedControl = control;

					var controls = new ArrayList();
					controls.Add(control);
					var service = ((ISelectionService) this.GetService(typeof(ISelectionService)));
					service.SetSelectedComponents(controls);
					dockingPanel.IsSelected = true;
					dockingPanel.Invalidate();
				}
			}
			if (msg.Msg == (int)Win32.Msg.WM_MOUSEMOVE)
			{
				Point p = dockingPanel.PointToClient(Cursor.Position);
				if (dockingPanel.TabsBounds.Contains(p.X, p.Y))
				{
					dockingPanel.InvalidateTabs();
				}
			}
			if (msg.Msg == (int)Win32.Msg.WM_MOUSELEAVE)
			{
				dockingPanel.InvalidateTabs();
			}
			base.WndProc(ref msg);
		}

		protected override bool GetHitTest(Point point)
		{
			Rectangle rect = new Rectangle(
				0, dockingPanel.ClientRectangle.Height - 22, 
				dockingPanel.ClientRectangle.Width, 22);
			return rect.Contains(this.dockingPanel.PointToClient(point));
		}
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.dockingPanel = ((StiDockingPanel) component);
			ISelectionService service = ((ISelectionService) this.GetService(typeof(ISelectionService)));
			service.SelectionChanged += new EventHandler(this.OnSelectionChanged);
		}		
		#endregion        
	
		#region Handlers
		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			
		}

		private void OnSelectionChanged(object sender, EventArgs e)
		{
			ISelectionService service = ((ISelectionService) this.GetService(typeof(ISelectionService)));
			ICollection controls = service.GetSelectedComponents();
			foreach (Control control in controls)
			{
				if (control == dockingPanel)
				{
					dockingPanel.IsSelected = true;
					dockingPanel.Invalidate();
					return;
				}
			}
			dockingPanel.IsSelected = false;
			dockingPanel.Invalidate();
		}

        private void OnAddControl(object sender, EventArgs e)
        {
            IDesignerHost host = (IDesignerHost) this.GetService(typeof(IDesignerHost));
            DesignerTransaction transaction = host.CreateTransaction("Add Control");
            StiDockingControl control = (StiDockingControl) host.CreateComponent(typeof(StiDockingControl));
            this.dockingPanel.Controls.Add(control);
			control.Dock = DockStyle.Fill;
            this.dockingPanel.SelectedControl = control;
            transaction.Commit();
        }

		#endregion

		#region Properties
		protected override bool DrawGrid
		{
			get
			{
				return false;
			}
		}

        public override DesignerVerbCollection Verbs
        {
            get
            {
				return new DesignerVerbCollection(new DesignerVerb[]{addControl});
            }
        }

		#endregion

		#region Fields
        private DesignerVerb addControl;
        private StiDockingPanel dockingPanel;
		#endregion

		public StiDockingPanelDesigner()
		{
			this.addControl = new DesignerVerb("Add Control", new EventHandler(this.OnAddControl));
		}
    }
    #endif
}