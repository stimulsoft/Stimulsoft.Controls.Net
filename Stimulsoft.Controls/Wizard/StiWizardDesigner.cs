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

namespace Stimulsoft.Controls
{
    #if !Profile
	internal class StiWizardDesigner : ParentControlDesigner
	{
		public override bool CanParent(Control control)
		{
			if (control is StiWizardPage)return true;
			return false;
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.wizardControl = (StiWizard)base.Control;
		}

		private void OnAddControl(object sender, EventArgs e)
		{
			IDesignerHost host = ((IDesignerHost) base.GetService(typeof(IDesignerHost)));
			DesignerTransaction transaction = host.CreateTransaction("Add Page");
			StiWizardPage control = ((StiWizardPage)host.CreateComponent(typeof(StiWizardPage)));
			wizardControl.Controls.Add(control);
			control.Dock = DockStyle.Fill;
			wizardControl.Invalidate();
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
		private StiWizard wizardControl;

		private const int WM_LBUTTONDOWN = 0x0201;

		protected override void WndProc(ref Message msg)
		{
			if (msg.Msg == WM_LBUTTONDOWN)
			{
				Point pos = wizardControl.PointToClient(Cursor.Position);
			
				#region Switch Pages
				StiWizardPage page = wizardControl.GetWizardPageAtPoint(pos);
				if (page != null)
				{
					wizardControl.SelectedPage = page;

					var controls = new ArrayList();
					controls.Add(page);
					ISelectionService service = ((ISelectionService) this.GetService(typeof(ISelectionService)));
					service.SetSelectedComponents(controls);
					return;
				}
				#endregion

				/*
				Rectangle rect = wizardControl.GetBackButtonRect();
				if (rect.Contains(pos))wizardControl.InvokeBackClick(EventArgs.Empty);

				rect = wizardControl.GetNextButtonRect();
				if (rect.Contains(pos))wizardControl.InvokeNextClick(EventArgs.Empty);
				*/
			}		
			base.WndProc(ref msg);
		}

		protected override bool GetHitTest(Point point)
		{	
			StiWizardPage page = wizardControl.GetWizardPageAtPoint(wizardControl.PointToClient(point));
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
		public StiWizardDesigner()
		{
			this.addControl = new DesignerVerb("Add Page", new EventHandler(this.OnAddControl));
		}
		#endregion
	}
    #endif
}