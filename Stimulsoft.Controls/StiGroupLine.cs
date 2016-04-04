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
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
    public enum StiLineStyle
    {
        Dot,
        Line
    }

    public enum StiLinePosition
    {
        Top,
        Bottom,
        Right
    }

	/// <summary>
	/// Use the StiGroupLine to group controls.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiGroupLine), "Toolbox.StiGroupLine.bmp")]
	public class StiGroupLine : ScrollableControl
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

        [Browsable(true)]
        private StiLineStyle lineStyle = StiLineStyle.Line;
        public StiLineStyle LineStyle
        {
            get
            {
                return lineStyle;
            }
            set
            {
                lineStyle = value;
                Invalidate();
            }
        }

        [Browsable(true)]
        private StiLinePosition linePosition = StiLinePosition.Top;
        public StiLinePosition LinePosition
        {
            get
            {
                return linePosition;
            }
            set
            {
                linePosition = value;
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
			
			using (var backBrush = new SolidBrush(this.BackColor))
			{
				g.FillRectangle(backBrush, rect);

				SizeF textSize = g.MeasureString(this.Text, this.Font);
				if (textSize.Width > rect.Width - 10 - startTextPosX)
					textSize.Width = rect.Width - 10 - startTextPosX;

				int leftSpace = rect.X + startTextPosX + 4;
				int rightSpace = rect.X + startTextPosX + (int)textSize.Width + 8;

				if (Text.Length == 0)leftSpace = rightSpace;

				if (this.RightToLeft == RightToLeft.Yes)
				{
					leftSpace = rect.Right - startTextPosX - (int)textSize.Width - 8;
					rightSpace = rect.Right - startTextPosX - 4;
				}

				int centerY = rect.Y + (int)(textSize.Height / 2);
				
				Color disabledColor = StiColorUtils.GetDisabledColor(this);

				using (var penLight = new Pen(ControlPaint.Light(disabledColor, 1f)))
				using (var penDark = new Pen(ControlPaint.Dark(disabledColor, 0f)))
				{
                    if (linePosition == StiLinePosition.Top)
				    {
				        if (this.lineStyle == StiLineStyle.Dot)
				        {
				            penDark.DashPattern = new float[] {1f, 2f};

				            var p1 = new Point(rect.X, centerY);
				            var p2 = new Point(leftSpace, centerY);
				            var p3 = new Point(rightSpace, centerY);
				            var p4 = new Point(rect.Right, centerY);

				            if (string.IsNullOrEmpty(this.Text))
				            {
				                g.DrawLine(penDark, p1, p4);
				            }
				            else
				            {
				                g.DrawLine(penDark, p1, p2);
				                g.DrawLine(penDark, p3, p4);
				            }
				        }
				        else
				        {
				            var p1 = new Point(rect.X, centerY);
				            var p2 = new Point(leftSpace, centerY);
				            var p3 = new Point(rightSpace, centerY);
				            var p4 = new Point(rect.Right, centerY);

				            g.DrawLine(penDark, p1, p2);
				            g.DrawLine(penDark, p3, p4);
				            p1.Y++;
				            p2.Y++;
				            p3.Y++;
				            p4.Y++;
				            g.DrawLine(penLight, p1, p2);
				            g.DrawLine(penLight, p3, p4);
				        }
				    }
                    else if (linePosition == StiLinePosition.Bottom)
				    {
                        if (this.lineStyle == StiLineStyle.Dot)
                        {
                            penDark.DashPattern = new float[] { 1f, 2f };

                            var p1 = new Point(rect.X, rect.Bottom - 1);
                            var p2 = new Point(rect.Right, rect.Bottom - 1);

                            g.DrawLine(penDark, p1, p2);
                        }
				    }
                    else if (linePosition == StiLinePosition.Right)
                    {
                        if (this.lineStyle == StiLineStyle.Dot)
                        {
                            penDark.DashPattern = new float[] { 1f, 2f };

                            var p1 = new Point(rect.Right - 1, rect.Y);
                            var p2 = new Point(rect.Right - 1, rect.Bottom - 1);

                            g.DrawLine(penDark, p1, p2);
                        }
                    }
				}

				if (this.Text.Length > 0)
				{
					var textRect = new Rectangle(leftSpace, 0, (int)textSize.Width + 5, 16);

					#region Draw text
					using (var sf = new StringFormat())
					{
						sf.FormatFlags = StringFormatFlags.NoWrap;
						sf.Trimming = StringTrimming.EllipsisCharacter;
						sf.Alignment = StringAlignment.Center;
						sf.HotkeyPrefix = HotkeyPrefix.Show;

						if (RightToLeft == RightToLeft.Yes)
							sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

						if (base.Enabled)
						{							
							using (var foreBrush = new SolidBrush(ForeColor))
								g.DrawString(Text, Font, foreBrush, textRect, sf);
						}			
						else
						{
							ControlPaint.DrawStringDisabled(g, this.Text, this.Font, disabledColor, textRect, sf);
						}
					}
					#endregion
					
				}
			}
		}
		#endregion

		#region Constructors
		public StiGroupLine()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.Selectable, false);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
		}
		#endregion
	}
}
