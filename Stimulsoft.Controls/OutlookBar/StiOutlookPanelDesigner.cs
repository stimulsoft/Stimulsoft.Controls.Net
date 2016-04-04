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
	internal class StiOutlookPanelDesigner : ParentControlDesigner
	{
		#region Handlers
		private void OnAddToolButton(object sender, EventArgs e)
		{
			StiToolButton toolbutton = new StiToolButton();
			toolbutton.Dock = DockStyle.Top;
			outlookPanel.Controls.Add(toolbutton);
			outlookPanel.Invalidate();
		}

		private void OnExpand(object sender, EventArgs e)
		{
			outlookPanel.Collapsed = true;
			outlookPanel.Invalidate();
		}

		private void OnCollapse(object sender, EventArgs e)
		{
			outlookPanel.Collapsed = false;
			outlookPanel.Invalidate();
		}
		#endregion

		#region Methods
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.outlookPanel = (StiOutlookPanel)base.Control;
		}

		#endregion
		
		#region Properties
		public override DesignerVerbCollection Verbs
		{
			get
			{
				return new DesignerVerbCollection(new DesignerVerb[]{
																		//addToolButton,
																		expand,
																		collapse
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
		
		#region Fields
		//private DesignerVerb addToolButton;
		private DesignerVerb expand;
		private DesignerVerb collapse;
		private StiOutlookPanel outlookPanel;
		#endregion

		#region Constructors
		public StiOutlookPanelDesigner()
		{
			this.expand = new DesignerVerb("Expand", new EventHandler(this.OnExpand));
			this.collapse = new DesignerVerb("Collapse", new EventHandler(this.OnCollapse));
			//this.addToolButton = new DesignerVerb("Add ToolButton", new EventHandler(this.OnAddToolButton));
		}
		#endregion	
	}
    #endif
}