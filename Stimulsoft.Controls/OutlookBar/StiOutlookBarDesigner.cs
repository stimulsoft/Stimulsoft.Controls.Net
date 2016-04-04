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
using System.Windows.Forms.Design;

namespace Stimulsoft.Controls
{
    #if !Profile
	internal class StiOutlookBarDesigner : ParentControlDesigner
	{
		#region Methods
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.outlookBar = (StiOutlookBar)base.Control;
		}

		#endregion

		#region Handlers
		private void OnAddPanel(object sender, EventArgs e)
		{
			IDesignerHost host = ((IDesignerHost) base.GetService(typeof(IDesignerHost)));
			DesignerTransaction transaction = host.CreateTransaction("Add Panel");
			StiOutlookPanel panel = ((StiOutlookPanel)host.CreateComponent(typeof(StiOutlookPanel)));

			outlookBar.Controls.Add(panel);
			outlookBar.Controls.SetChildIndex(panel, 0);
			
			panel.Dock = DockStyle.Top;
			transaction.Commit();
		}
		#endregion
		
		#region Fields
		private DesignerVerb addPanel;
		private StiOutlookBar outlookBar;
		#endregion

		#region Properties
		public override DesignerVerbCollection Verbs
		{
			get
			{
				return new DesignerVerbCollection(new DesignerVerb[]{
																		addPanel
																	}); 
			}
		}

		protected override bool DrawGrid
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region Constructors
		public StiOutlookBarDesigner()
		{
			this.addPanel = new DesignerVerb("&Add Panel", new EventHandler(this.OnAddPanel));
		}
		#endregion
	}
    #endif
}