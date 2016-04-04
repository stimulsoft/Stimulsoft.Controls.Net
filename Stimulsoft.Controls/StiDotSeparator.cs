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
    public partial class StiDotSeparator : Control
    {
        public StiDotSeparator()
        {
            this.MinimumSize = new Size(0, 5);
            this.MaximumSize = new Size(0, 5);
        }

        #region Methods
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.Width > 6)
            {
                var g = e.Graphics;
                Color color = Color.FromArgb(198, 198, 198);
                g.DrawLine(new Pen(color){DashPattern = new float[] { 1f, 2f }}, 0, this.Height/2, this.Width, this.Height/2);
            }
        }
        #endregion
    }
}
