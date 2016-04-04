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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Controls
{
    #if !Profile
	internal class StiTabPageDesigner : ParentControlDesigner
	{
		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			return (parentDesigner.Component as StiTabControl) != null;
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.tabPage = component as StiTabPage;
		}

		public override SelectionRules SelectionRules
		{
			get
			{
				return SelectionRules.None;
			}
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


		private void OnMoveLeft(object sender, EventArgs e)
		{
			int index = tabPage.Parent.Controls.GetChildIndex(tabPage);
			if (index > 0)
			{
				IDesignerHost host = (IDesignerHost) this.GetService(typeof(IDesignerHost));
				IComponentChangeService service = (IComponentChangeService) this.GetService(typeof(IComponentChangeService));
				DesignerTransaction transaction = host.CreateTransaction("Move Left");
				service.OnComponentChanging(tabPage.Parent, TypeDescriptor.GetProperties(tabPage.Parent)["Controls"]);
				tabPage.Parent.Controls.SetChildIndex(tabPage, index - 1);
				service.OnComponentChanged(tabPage.Parent, TypeDescriptor.GetProperties(tabPage.Parent)["Controls"], null, null);
				transaction.Commit();

				tabPage.Parent.Invalidate();
			}
		} 

		private void OnMoveRight(object sender, EventArgs e)
		{
			int index = tabPage.Parent.Controls.GetChildIndex(tabPage);
			if (index < tabPage.Parent.Controls.Count - 1)
			{
				IDesignerHost host = (IDesignerHost) this.GetService(typeof(IDesignerHost));
				IComponentChangeService service = (IComponentChangeService) this.GetService(typeof(IComponentChangeService));
				DesignerTransaction transaction = host.CreateTransaction("Move Right");
				service.OnComponentChanging(tabPage.Parent, TypeDescriptor.GetProperties(tabPage.Parent)["Controls"]);
				tabPage.Parent.Controls.SetChildIndex(tabPage, index + 1);
				service.OnComponentChanged(tabPage.Parent, TypeDescriptor.GetProperties(tabPage.Parent)["Controls"], null, null);
				transaction.Commit();

				tabPage.Parent.Invalidate();
			}
		} 

		private StiTabPage tabPage;
		private DesignerVerb moveLeft;
		private DesignerVerb moveRight;

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			if (tabPage != null)
			using(var pen = new Pen(SystemColors.ControlDark))
			{
				pen.DashStyle = DashStyle.Dash;
				pe.Graphics.DrawRectangle(pen, 0, 0, tabPage.Width - 1, tabPage.Height - 1);
			}
		}

		#region Constructors
		public StiTabPageDesigner()
		{
			this.moveLeft = new DesignerVerb("Move Left", new EventHandler(this.OnMoveLeft));
			this.moveRight = new DesignerVerb("Move Right", new EventHandler(this.OnMoveRight));
		}
		#endregion
	}
    #endif
}