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
using System.Windows.Forms.Design;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Stimulsoft.Controls;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
    #if !Profile
	internal class StiTabulatorDesigner : ParentControlDesigner
	{
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.tabControl = (StiTabulator)base.Control;
		}

		private void OnAddControl(object sender, EventArgs e)
		{
			IDesignerHost host = ((IDesignerHost) base.GetService(typeof(IDesignerHost)));
			DesignerTransaction transaction = host.CreateTransaction("Add Page");
			StiTabulatorPage control = ((StiTabulatorPage)host.CreateComponent(typeof(StiTabulatorPage)));
			this.tabControl.Controls.Add(control);
			control.Dock = DockStyle.Fill;
			this.tabControl.Invalidate();
			transaction.Commit();
		} 

		public override DesignerVerbCollection Verbs
		{
			get
			{
				return new DesignerVerbCollection(new DesignerVerb[]{
																		addControl
																	}); 
			}
		}

		
		private DesignerVerb addControl;
		private StiTabulator tabControl;

		protected override void WndProc(ref Message msg)
		{
			if (msg.Msg == (int)Win32.Msg.WM_LBUTTONDOWN)
			{
				Point p = tabControl.PointToClient(Cursor.Position);
				StiTabulatorPage page = tabControl.GetTabPageAtPoint(p);
				if (page != null)
				{
					tabControl.SelectedTab = page;
					var controls = new ArrayList();
					controls.Add(page);
					ISelectionService service = ((ISelectionService) this.GetService(typeof(ISelectionService)));
					service.SetSelectedComponents(controls);
				}
			}		
			base.WndProc(ref msg);
		}

		protected override bool GetHitTest(Point point)
		{
	
			StiTabulatorPage page = tabControl.GetTabPageAtPoint(tabControl.PointToClient(point));
			return page != null;
		}

		protected override bool DrawGrid
		{
			get
			{
				return false;
			}
		}


		#region Constructors
		public StiTabulatorDesigner()
		{
			this.addControl = new DesignerVerb("Add Page", new EventHandler(this.OnAddControl));
		}
		#endregion
	}
    #endif
}
