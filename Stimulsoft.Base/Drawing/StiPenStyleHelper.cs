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
using Stimulsoft.Base.Localization;
using Stimulsoft.Base;

namespace Stimulsoft.Base.Drawing
{
	public sealed class StiPenStyleHelper
	{
		public static int GetMaxWidthOfPenStyles(Graphics g, Font font)
		{
			Array obj = Enum.GetValues(typeof(StiPenStyle));
			
			int maxWidth = 0;

			for (int k = 0; k < obj.Length; k++)
			{
				string str = ((StiPenStyle)obj.GetValue(k)).ToString();
				string locName = StiLocalization.Get("PropertyEnum", "StiPenStyle" + str);
					
				if (locName != null)str = locName;

				int strWidth = (int)g.MeasureString(str, font).Width;

				maxWidth = Math.Max(maxWidth, strWidth);
			}
			return maxWidth + 60;
		}


		public static void DrawPenStyle(DrawItemEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle rect = e.Bounds;
			Rectangle borderRect = new Rectangle(rect.X + 2, rect.Y + 2, 52, 14);
			
			#region Fill
			rect.Width--;
			StiControlPaint.DrawItem(g, rect, e.State, SystemColors.Window, SystemColors.ControlText);
			#endregion
			
			#region Paint border style
			Array obj = Enum.GetValues(typeof(StiPenStyle));

			using (Pen pen = new Pen(Color.Black, 2))
			{
				StiPenStyle penStyle = StiPenStyle.Solid;
				if (e.Index != -1)penStyle = (StiPenStyle)obj.GetValue(e.Index);
				pen.DashStyle = StiPenUtils.GetPenStyle(penStyle);
            			
				g.FillRectangle(Brushes.White, borderRect);

				int center = rect.Top + rect.Height /2;

				if (penStyle == StiPenStyle.Double)
				{
					pen.Width = 1;
					g.DrawLine(pen, 2, center - 1, 54, center - 1);
					g.DrawLine(pen, 2, center + 1, 54, center + 1);
				}
				else if (penStyle != StiPenStyle.None)
				{
					g.DrawLine(pen, 2, center, 54, center);
				}
			}

			g.DrawRectangle(Pens.Black, borderRect);
			#endregion

			#region Paint name
			using (Font font = new Font("Arial", 8))
			{
				using (StringFormat stringFormat = new StringFormat())
				{
					stringFormat.LineAlignment = StringAlignment.Center;

					string name = ((StiPenStyle)obj.GetValue(e.Index)).ToString();

					string locName = StiLocalization.Get("PropertyEnum", "StiPenStyle" + name);
					
					if (locName != null)name = locName;

					e.Graphics.DrawString(name, 
						font, Brushes.Black, 
						new Rectangle(55, rect.Top + 4, rect.Width - 18, 10), 
						stringFormat);
				}
			}
			#endregion

		}
	}
}
