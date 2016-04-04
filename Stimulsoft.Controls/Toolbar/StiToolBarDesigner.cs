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
	internal class StiToolBarDesigner : ParentControlDesigner
	{
		#region Methods
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.toolBar = (StiToolBar)base.Control;
		}

		#endregion

		#region Handlers
		private void OnAddToolButton(object sender, EventArgs e)
		{
			IDesignerHost host = ((IDesignerHost) base.GetService(typeof(IDesignerHost)));
			DesignerTransaction transaction = host.CreateTransaction("Add ToolButton");
			StiToolButton toolButton = ((StiToolButton)host.CreateComponent(typeof(StiToolButton)));
			toolButton.Dock = DockStyle.Left;
			this.toolBar.Controls.Add(toolButton);
			this.toolBar.Controls.SetChildIndex(toolButton, 0);
			transaction.Commit();
		} 

		private void OnAddSeparator(object sender, EventArgs e)
		{
			IDesignerHost host = ((IDesignerHost) base.GetService(typeof(IDesignerHost)));
			DesignerTransaction transaction = host.CreateTransaction("Add Separator");
			StiToolButton toolButton = ((StiToolButton)host.CreateComponent(typeof(StiToolButton)));
			toolButton.Dock = DockStyle.Left;
			toolButton.Style = ToolBarButtonStyle.Separator;
			toolButton.Width = 8;
			this.toolBar.Controls.Add(toolButton);
			this.toolBar.Controls.SetChildIndex(toolButton, 0);
			transaction.Commit();
		} 

		#endregion

		#region Properties
		public override DesignerVerbCollection Verbs
		{
			get
			{
				return new DesignerVerbCollection(new DesignerVerb[]{
																		addToolButton,
																		addSeparator
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
		private DesignerVerb addToolButton;
		private DesignerVerb addSeparator;
		private StiToolBar toolBar;
		#endregion

		#region Constructors
		public StiToolBarDesigner()
		{
			this.addToolButton = new DesignerVerb("Add ToolButton", new EventHandler(this.OnAddToolButton));
			this.addSeparator = new DesignerVerb("Add Separator", new EventHandler(this.OnAddSeparator));
		}
		#endregion
	}
    #endif
}