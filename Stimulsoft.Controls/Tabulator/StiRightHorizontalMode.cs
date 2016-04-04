#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports												}
{	                         										}
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
using Stimulsoft.Controls;

namespace Stimulsoft.Controls
{
    public class StiRightHorizontalMode : StiTabulatorMode
    {
        #region Methods
		public override Rectangle GetTitlePageRectangle(StiTabulatorPage page)
		{
			int pageIndex = 0;
			foreach (StiTabulatorPage pg in tabulator.Controls)
			{
				if (pg.Invisible && (!tabulator.IsDesignMode))continue;
				if (pg == page)break;
				pageIndex++;				
			}

            return new Rectangle(tabulator.Width - tabulator.MaxTitleWidth,
                pageIndex * tabulator.MaxTitleHeight,
                tabulator.MaxTitleWidth - titleSpace, tabulator.MaxTitleHeight);
		}		

		public override Rectangle GetTitleRectangle()
		{
			if (tabulator.Controls.Count > 0)
			{
                return new Rectangle(tabulator.Width - tabulator.MaxTitleWidth - 2 * titleSpace, 0, tabulator.MaxTitleWidth + 2 * titleSpace, tabulator.Height);
			}
			else 
			{
				return new Rectangle(0, 0, tabulator.Width, tabulator.Height);
			}
		}

		public override Rectangle GetClientRectangle()
		{
            return new Rectangle(0, 0, tabulator.Width - tabulator.MaxTitleWidth - 2 * titleSpace, tabulator.Height);
		}

		public override void SetLayout()
		{
			if (tabulator.IsDesignMode || tabulator.ShowTitle)
			{
                tabulator.DockPadding.Right = tabulator.MaxTitleWidth + titleSpace;
			}
			else
			{
                tabulator.DockPadding.Right = 0;
			}
			tabulator.DockPadding.Left = 0;
			tabulator.DockPadding.Top = 0;
			tabulator.DockPadding.Bottom = 0;
		}

        public override Size MeasureTitle(Graphics g, StiTabulatorPage page)
        {
            Size titleSize = MeasureWrappedString(g, tabulator.Font, page.Text, 95);//Артем: статическая ширина заголовков
            titleSize.Width += 6;
            titleSize.Height += 6;

            Rectangle imageRect = GetTitleImageRectangle(Rectangle.Empty, page);
            
            titleSize.Width = Math.Max(titleSize.Width, imageRect.Width + 6);
            titleSize.Height += imageRect.Height + 6;

            if (tabulator.TitleWidth > 0) titleSize.Width = tabulator.TitleWidth;
            if (tabulator.TitleHeight > 0) titleSize.Height = tabulator.TitleHeight;

            return titleSize;
        }

        private Size MeasureWrappedString(Graphics g, Font font, string str, int maxWidth, int lineSpacing = 0)
        {
            SizeF size = g.MeasureString(str, font);
            string lineBuffer = "";
            float lineHeight = size.Height;
            int resultHeight = (int)lineHeight;
            

            foreach (char c in str)
            {
                size = g.MeasureString(lineBuffer, font);
                if (size.Width >= maxWidth/* && c == ' '*/)
                {
                    resultHeight += (int)(lineHeight + lineSpacing);
                    lineBuffer = "";
                }
                lineBuffer += c.ToString();
            }

            return new Size(maxWidth, resultHeight);
        }

        public override void DrawTitleText(Graphics g, Rectangle rect, StiTabulatorPage page)
        {
            Rectangle textRect = GetTitleTextRectangle(rect, page);

            var stringFormat = new StringFormat
            {
                HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show,
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter
            };

            g.DrawString(page.Text, tabulator.Font, SystemBrushes.ControlText, textRect, stringFormat);
        }

        public override Rectangle GetTitleImageRectangle(Rectangle rect, StiTabulatorPage page)
        {
            SizeF imageSize = SizeF.Empty;
            if (tabulator.ImageList != null || page.Image != null)
            {
                if (tabulator.ImageList != null || page.Image != null)
                {
                    if (page.Image != null) imageSize = page.Image.Size;
                    else imageSize = tabulator.ImageList.ImageSize;

                    int width = 4;
                    if (rect.Width > 0)
                    {
                        width = ((int)((rect.Width - imageSize.Width) / 2));
                    }

                    return new Rectangle(rect.X + width,
                        rect.Y + 10,
                        (int)imageSize.Width, (int)imageSize.Height);

                }
            }
            return Rectangle.Empty;
        }

        public override Rectangle GetTitleTextRectangle(Rectangle rect, StiTabulatorPage page)
        {
            Rectangle imageRect = GetTitleImageRectangle(rect, page);
            if (imageRect.IsEmpty) return rect;
            rect.Y += imageRect.Height + 4;
            rect.Height -= imageRect.Height;
            
            return rect;
        }

		#endregion

		#region Properties
		public override StiTabTitlePosition TabTitlePosition
		{
			get
			{
				return StiTabTitlePosition.RightHorizontal;
			}
		}

		#endregion

        public StiRightHorizontalMode(StiTabulator tabulator): base(tabulator)
		{
		}
    }
}
