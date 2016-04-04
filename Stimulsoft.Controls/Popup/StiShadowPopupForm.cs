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

using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls
{	
	public class StiShadowPopupForm : StiPopupBaseForm
	{
		#region Handlers
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics g = e.Graphics;
			Rectangle rect = this.ClientRectangle;
			rect.Width --;
			rect.Height --;

			StiShadow.DrawShadow(g, rect);
		}
		#endregion

		#region Properties
		protected override bool Layered
		{
			get
			{
				return false;
			}
		}

		protected override bool HookMouseButtonsMessages
		{
			get
			{
				return false;
			}
		}
		#endregion

		public StiShadowPopupForm() : this(null)
		{
		}

		public StiShadowPopupForm(Control parentControl) : base(parentControl)
		{
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = Color.Transparent;
		}
	}
}