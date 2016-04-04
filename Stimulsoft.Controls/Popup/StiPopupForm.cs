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

using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{	
	public class StiPopupForm : StiPopupBaseForm
	{
		#region Fields
		private StiShadowPopupForm shadow = null;
		#endregion

		#region Methods
		public override void SetLocation(int x, int y, int width, int height)
		{
			base.SetLocation(x, y, width, height);

			if (IsDrawShadow && (!is64Bit))
			{				
				this.shadow.CreateControl();
				this.shadow.SetBounds(
					this.Left + StiShadow.ShadowSize + 1, 
					this.Top + StiShadow.ShadowSize + 1, 
					this.Width, 
					this.Height, BoundsSpecified.All);
			}
		}		


		public override void ClosePopupForm()
		{
			if (IsDrawShadow && (!is64Bit))
			{
				if (shadow != null)
				{
					shadow.ClosePopupForm();
					shadow.Dispose();
					shadow = null;
				}
			}
			base.ClosePopupForm();
		}


		public override void ShowPopupForm()
		{
			base.ShowPopupForm();
			ShowShadow();
		}	


		protected void ShowShadow()
		{
            if (IsDrawShadow && isDrawShadowInPopupWindow && shadow != null && (!shadow.IsDisposed) && (!is64Bit))
			{
                shadow.UpdateZOrder(this.Handle);
				shadow.ShowPopupForm();				
			}
		}
		#endregion

		#region Propeties
		protected override bool Layered
		{
			get
			{
				return false;
			}
		}

		protected bool IsDrawShadow
		{
			get
			{
				return StiOSFeature.IsLayeredWindows;
			}
		}

        private bool isDrawShadowInPopupWindow = true;
        public bool IsDrawShadowInPopupWindow
        {
            get
            {
                return isDrawShadowInPopupWindow;
            }
            set
            {
                isDrawShadowInPopupWindow = value;
            }
        }
		#endregion
 
		public StiPopupForm() : this(null)
		{
		}

		public StiPopupForm(Control parentControl) : base(parentControl)
		{
			if (IsDrawShadow)
			{
				this.shadow = new StiShadowPopupForm(parentControl);
			}

            this.AutoScaleMode = AutoScaleMode.Font;
			this.SetStyle(ControlStyles.DoubleBuffer, false);
			//this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
		}
	}
}
