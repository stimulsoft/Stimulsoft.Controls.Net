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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using Stimulsoft.Controls;
using Stimulsoft.Base.Drawing;


namespace Stimulsoft.Controls
{	
	/// <summary>
	/// Allows users to select a value from a TreeView.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiTreeViewMultiBox), "Toolbox.StiTreeViewBox.bmp")]
	public class StiTreeViewMultiBox : StiTreeViewBox
    {
        #region Fields
        internal bool IsPressed2 = false;
        #endregion

        #region Methods
        public Rectangle GetButtonRect2(Rectangle bounds)
        {
            Rectangle rect = base.GetButtonRect(bounds);
            rect.X -= rect.Width + 2;

            return rect;
        }        

        protected override Rectangle GetContentRect()
        {
            Rectangle contentRect = base.GetContentRect();            
            Rectangle buttonRect = GetButtonRect2(this.ClientRectangle);

            if (ButtonAlign == StiButtonAlign.Right)
            {
                contentRect.Width -= buttonRect.Width + 1;
                return contentRect;
            }
            else
            {
                contentRect.X += buttonRect.Width + 1;
                contentRect.Width -= buttonRect.Width + 1;
                return contentRect;
            }
        }
        #endregion

        #region Properties
        protected bool IsMouseOverButton2
        {
            get
            {
                if (!ShowButton2) return false;
                Rectangle rect2 = GetButtonRect2(this.ClientRectangle);
                return rect2.Contains(this.PointToClient(Cursor.Position));
            }
        }

        private Image buttonBitmap2 = null;
        public Image ButtonBitmap2
        {
            get
            {
                return buttonBitmap2;
            }
            set
            {
                buttonBitmap2 = value;
            }
        }

        private bool showButton2 = true;
        public bool ShowButton2
        {
            get
            {
                return showButton2;
            }
            set
            {
                showButton2 = value;
            }
        }
        #endregion

        #region Events
        #region ButtonClick2
        /// <summary>
        /// Occurs when the user push button.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler ButtonClick2;

        protected virtual void OnButtonClick2(System.EventArgs e)
        {
            if (ButtonClick2 != null) ButtonClick2(this, e);
        }

        /// <summary>
        /// Raises the ButtonClick event for this control.
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data.</param>
        public void InvokeButtonClick2(EventArgs e)
        {
            OnButtonClick2(e);
        }
        #endregion
        #endregion

        #region Handlers
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (IsMouseOverButton2)
            {
                OnButtonClick2(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
			if ((e.Button & MouseButtons.Left) > 0)
			{				
				if (IsMouseOverButton2)
				{
					IsPressed2 = IsMouseOverButton2;
					this.Focus();
					Invalidate();
					return;
				}

				base.OnMouseDown(e);
			}
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) > 0)
            {
                IsPressed = false;
				IsPressed2 = false;

                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs p)
        {
            base.OnPaint(p);

            Graphics g = p.Graphics;

            Rectangle buttonRect2 = GetButtonRect2(this.ClientRectangle);
            if (ShowButton && ShowButton2)
            {
                PaintButtons(g, buttonRect2, this.ButtonBitmap2 as Bitmap, this.IsPressed2, this.IsMouseOverButton2);
            }
        }
        #endregion
    }
}
