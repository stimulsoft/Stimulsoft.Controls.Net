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

using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Controls
{
    #if !Profile
	internal class StiWizardPageDesigner : ParentControlDesigner
	{
		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			return (parentDesigner.Component as StiWizard) != null;
		}

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			this.tabPage = component as StiWizardPage;
		}

		public override SelectionRules SelectionRules
		{
			get
			{
				return SelectionRules.None;
			}
		}


		private StiWizardPage tabPage;

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			if (tabPage != null)
				using(var pen = new Pen(SystemColors.ControlDark))
				{
					pen.DashStyle = DashStyle.Dash;
					pe.Graphics.DrawRectangle(pen, 2, 2, tabPage.Width - 5, tabPage.Height - 5);
				}
		}

		#region Constructors
		public StiWizardPageDesigner()
		{
		}
		#endregion
	}
    #endif
}