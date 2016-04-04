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
	/// Use the StiGroupLine to group controls.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiOffice2013GroupBox), "Toolbox.StiGroupLine.bmp")]
	public class StiOffice2013GroupBox : ScrollableControl
	{
		#region Properties
		[Browsable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				Invalidate();
			}
		}

	    private int headerHeight = 24;
        [Browsable(true)]
        public int HeaderHeight
        {
            get
            {
                return headerHeight;
            }
            set
            {
                headerHeight = value;
                Invalidate();
            }
        }
		#endregion

		#region Handlers
		protected override void OnPaint(PaintEventArgs p)
		{
			const int startTextPosX = 5;
			var g = p.Graphics;

			var rect = this.ClientRectangle;

            using (var font = new Font("Microsoft Sans Serif", 10f))
			using (var backBrush = new SolidBrush(this.BackColor))
			{
				g.FillRectangle(backBrush, rect);

                var textSize = g.MeasureString(this.Text, font);
				if (textSize.Width > rect.Width - 10 - startTextPosX)
					textSize.Width = rect.Width - 10 - startTextPosX;

                var textRect = new Rectangle(0, 0, this.Width, headerHeight);
			    using (var brush = new SolidBrush(Color.FromArgb(238, 238, 238)))
				{
                    g.FillRectangle(brush, textRect);
				}

				if (this.Text.Length > 0)
				{
				    textRect.X += 12;
				    textRect.Width = textRect.Width + 5;

					#region Draw text
					using (var sf = new StringFormat())
					{
						sf.FormatFlags = StringFormatFlags.NoWrap;
						sf.Trimming = StringTrimming.EllipsisCharacter;
						sf.Alignment = StringAlignment.Near;
                        sf.LineAlignment = StringAlignment.Center;
						sf.HotkeyPrefix = HotkeyPrefix.Show;

						if (RightToLeft == RightToLeft.Yes)
							sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

                        using (var foreBrush = new SolidBrush(Color.FromArgb(136, 136, 136)))
                            g.DrawString(Text, font, foreBrush, textRect, sf);
					}
					#endregion
				}
			}
		}
		#endregion

        public StiOffice2013GroupBox()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.Selectable, false);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}
	}
}