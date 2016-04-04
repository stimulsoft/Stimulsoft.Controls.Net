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
using System.Drawing.Text;
using System.Windows.Forms;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Represents a Windows group box.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiGroupBox), "Toolbox.StiGroupBox.bmp")]
	public class StiDatabaseSelectGroupBox : StiGroupBox
	{
        public StiDatabaseSelectGroupBox()
        {
            AllowCollapse = true;
        }
        
        #region Properties
        public override int HeaderSize
        {
            get
            {
                return 24;
            }
        }
		#endregion

		#region Handlers

		protected override void OnPaint(PaintEventArgs p)
		{
			Graphics g = p.Graphics;

            using (var brush = new SolidBrush(Color.FromArgb(255, 238, 238, 238)))
            {
                g.FillRectangle(brush, 0, 0, this.Width, 24);
            }

            if (this.Text.Length > 0)
            {
                int startTextPos = 24;

                Rectangle rect = ClientRectangle;
                Rectangle textRect = rect;

                textRect.Width = this.Width - 30;
                textRect.Height = 24;

                if (RightToLeft == RightToLeft.Yes)
                {
                    textRect.X = (rect.Right - textRect.Width) - startTextPos;
                }
                else
                {
                    textRect.X += startTextPos;
                }

                #region Draw text
                using (var font = new Font("Microsoft Sans Serif", 10f))
                using (var foreBrush = new SolidBrush(Color.FromArgb(255, 136, 136, 136)))
                using (var sf = new StringFormat())
                {
                    sf.FormatFlags = StringFormatFlags.NoWrap;
                    sf.Trimming = StringTrimming.EllipsisCharacter;
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    sf.HotkeyPrefix = HotkeyPrefix.Show;

                    if (RightToLeft == RightToLeft.Yes)
                        sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

                    g.DrawString(Text, font, foreBrush, textRect, sf);
                }
                #endregion
            }

            DrawCollapse(g);
		}

        protected override Rectangle GetCollapseRectangle()
        {
            return (this.RightToLeft == RightToLeft.Yes)
                ? new Rectangle(this.Width - 17, 7, 10, 10)
                : new Rectangle(7, 7, 10, 10);
        }

        #endregion
    }
}
