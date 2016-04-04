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
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace Stimulsoft.Controls
{
    #if !Profile
	internal class StiDockingManagerDesigner : ComponentDesigner
	{
		#region Methods
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			IDesignerHost host = (IDesignerHost) this.GetService(typeof(IDesignerHost));
			this.dockingManager = (StiDockingManager) component;
			this.dockingManager.ParentForm = host.RootComponent as Form;
		}		
		
		#endregion
		
		#region Handlers
		private void OnAddToLeft(object sender, EventArgs e)
		{
			IDesignerHost host = (IDesignerHost) this.GetService(typeof(IDesignerHost));
			DesignerTransaction transaction = host.CreateTransaction("Add to left");
			StiDockingPanel panel = (StiDockingPanel)host.CreateComponent(typeof(StiDockingPanel));
			((Form)host.RootComponent).Controls.Add(panel);
			dockingManager.DockedPanels.Add(panel);
			panel.Manager = dockingManager;
			panel.Dock = DockStyle.Left;
			transaction.Commit();
		}

		private void OnAddToRight(object sender, EventArgs e)
		{
			IDesignerHost host = (IDesignerHost) this.GetService(typeof(IDesignerHost));
			DesignerTransaction transaction = host.CreateTransaction("Add to right");
			StiDockingPanel panel = (StiDockingPanel) host.CreateComponent(typeof(StiDockingPanel));
			((Form)host.RootComponent).Controls.Add(panel);
			dockingManager.DockedPanels.Add(panel);
			panel.Manager = dockingManager;
			panel.Dock = DockStyle.Right;
			transaction.Commit();
		}

		private void OnAddToTop(object sender, EventArgs e)
		{
			IDesignerHost host = (IDesignerHost) this.GetService(typeof(IDesignerHost));
			DesignerTransaction transaction = host.CreateTransaction("Add to top");
			StiDockingPanel panel = (StiDockingPanel) host.CreateComponent(typeof(StiDockingPanel));
			((Form)host.RootComponent).Controls.Add(panel);
			dockingManager.DockedPanels.Add(panel);
			panel.Manager = dockingManager;
			panel.Dock = DockStyle.Top;
			transaction.Commit();
		}

		private void OnAddToBottom(object sender, EventArgs e)
		{
			IDesignerHost host = (IDesignerHost) this.GetService(typeof(IDesignerHost));
			DesignerTransaction transaction = host.CreateTransaction("Add to bottom");
			StiDockingPanel panel = (StiDockingPanel) host.CreateComponent(typeof(StiDockingPanel));
			((Form)host.RootComponent).Controls.Add(panel);
			dockingManager.DockedPanels.Add(panel);
			panel.Manager = dockingManager;
			panel.Dock = DockStyle.Bottom;
			transaction.Commit();
		}

		#endregion

		#region Properties
		public override DesignerVerbCollection Verbs
		{
			get
			{
				return new DesignerVerbCollection(new DesignerVerb[]{
																		addToLeft,
																		addToRight,
																		addToTop,
																		addToBottom
				});
			}
		}

		#endregion

		#region Fields
		private StiDockingManager dockingManager;
		private DesignerVerb addToLeft;
		private DesignerVerb addToRight;
		private DesignerVerb addToTop;
		private DesignerVerb addToBottom;
		#endregion

		public StiDockingManagerDesigner()
		{
			this.addToLeft =	new DesignerVerb("Add to left",		new EventHandler(this.OnAddToLeft));
			this.addToRight =	new DesignerVerb("Add to right",	new EventHandler(this.OnAddToRight));
			this.addToTop =		new DesignerVerb("Add to top",		new EventHandler(this.OnAddToTop));
			this.addToBottom =	new DesignerVerb("Add to bottom",	new EventHandler(this.OnAddToBottom));
		}
	}
    #endif
}