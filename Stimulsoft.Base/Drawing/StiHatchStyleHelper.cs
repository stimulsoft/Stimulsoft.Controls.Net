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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing
{
	public sealed class StiHatchStyleHelper
	{
		public static int GetWidthOfHatchStyle(Graphics g, Font font, int index)
		{
			HatchStyle hatchStyle = StiBrushes.HatchStyles[index];
			string str = StiLocalization.Get("PropertyHatchStyle", hatchStyle.ToString());

			int strWidth = (int)g.MeasureString(str, font).Width;
			return strWidth + 40;
		}


		public static void DrawHatchStyle(DrawItemEventArgs e)
		{
			if (e.Index >= 0)
			{
				var g = e.Graphics;
				var rect = e.Bounds;
				if ((e.State & DrawItemState.Selected) > 0)rect.Width--;
				var hatchRect = new Rectangle(rect.X + 2, rect.Y + 2, 19, rect.Height - 5);
			
				#region Fill
				StiControlPaint.DrawItem(g, rect, e.State, SystemColors.Window, SystemColors.ControlText);
				#endregion

			    using (var brush = new HatchBrush(StiBrushes.HatchStyles[e.Index], Color.Black, Color.White))
			    {
			        g.FillRectangle(brush, hatchRect);
			    }

				g.DrawRectangle(Pens.Black, hatchRect);

				#region Paint name
				using (var font = new Font("Arial", 8))
				{
					using (var stringFormat = new StringFormat())
					{
						stringFormat.LineAlignment = StringAlignment.Center;
						stringFormat.FormatFlags = StringFormatFlags.NoWrap;
						stringFormat.Trimming = StringTrimming.EllipsisCharacter;

						var hatchStyle = StiBrushes.HatchStyles[e.Index];
						string name = StiLocalization.Get("PropertyHatchStyle", hatchStyle.ToString());

						e.Graphics.DrawString(name, 
							font, Brushes.Black, 
							new Rectangle(25, rect.Top, rect.Width - 18, 16),
							stringFormat);
					}
				}
				#endregion
			}
		}
	}
}
