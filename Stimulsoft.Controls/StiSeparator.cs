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
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls
{
    [ToolboxItem(true)]
    public class StiSeparator : Control
    {
        #region Methods
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.Width > 6)
            {
                var g = e.Graphics;
                Color color = Color.FromArgb(160, 160, 160);
                g.DrawLine(new Pen(color), 3, 2, this.Width - 3, 2);
            }
        }
        #endregion

        #region Constructors
        public StiSeparator()
        {
            this.MinimumSize = new Size(0, 5);
            this.MaximumSize = new Size(0, 5);
        }
        #endregion
    }
}