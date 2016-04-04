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
using System.Windows.Forms.Design;

namespace Stimulsoft.Controls
{
    #if !Profile
	internal class StiToolButtonDesigner : ParentControlDesigner
	{
		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			toolButton = component as StiToolButton;
		}

		public override DesignerVerbCollection Verbs
		{
			get
			{
				return new DesignerVerbCollection(new DesignerVerb[]{
																		moveLeft,
																		moveRight
																	}); 
			}
		}


		private void OnMoveRight(object sender, EventArgs e)
		{
			int index = toolButton.Parent.Controls.GetChildIndex(toolButton);
			if (index > 0)
			{
				IDesignerHost host = (IDesignerHost) this.GetService(typeof(IDesignerHost));
				IComponentChangeService service = (IComponentChangeService) this.GetService(typeof(IComponentChangeService));
				DesignerTransaction transaction = host.CreateTransaction("Move Left");
				service.OnComponentChanging(toolButton.Parent, TypeDescriptor.GetProperties(toolButton.Parent)["Controls"]);
				toolButton.Parent.Controls.SetChildIndex(toolButton, index - 1);
				service.OnComponentChanged(toolButton.Parent, TypeDescriptor.GetProperties(toolButton.Parent)["Controls"], null, null);
				transaction.Commit();

				toolButton.Parent.Invalidate();
			}
		} 

		private void OnMoveLeft(object sender, EventArgs e)
		{
			int index = toolButton.Parent.Controls.GetChildIndex(toolButton);
			if (index < toolButton.Parent.Controls.Count - 1)
			{
				IDesignerHost host = (IDesignerHost) this.GetService(typeof(IDesignerHost));
				IComponentChangeService service = (IComponentChangeService) this.GetService(typeof(IComponentChangeService));
				DesignerTransaction transaction = host.CreateTransaction("Move Right");
				service.OnComponentChanging(toolButton.Parent, TypeDescriptor.GetProperties(toolButton.Parent)["Controls"]);
				toolButton.Parent.Controls.SetChildIndex(toolButton, index + 1);
				service.OnComponentChanged(toolButton.Parent, TypeDescriptor.GetProperties(toolButton.Parent)["Controls"], null, null);
				transaction.Commit();

				toolButton.Parent.Invalidate();
			}
		} 

		private StiToolButton toolButton;
		private DesignerVerb moveLeft;
		private DesignerVerb moveRight;


		#region Constructors
		public StiToolButtonDesigner()
		{
			this.moveLeft = new DesignerVerb("Move Left", new EventHandler(this.OnMoveLeft));
			this.moveRight = new DesignerVerb("Move Right", new EventHandler(this.OnMoveRight));
		}
		#endregion
	}
    #endif
}