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

namespace Stimulsoft.Controls
{
    internal sealed class StiCloudMode
    {
        #region Methods

        public void DrawTitleImage(Graphics g, Rectangle rect, StiCloudTabulatorPage page)
        {
            var imageRect = GetTitleImageRectangle(rect, page);

            if (!imageRect.IsEmpty)
            {
                if (page.Image != null)
                {
                    g.DrawImage(page.Image, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height);
                }
            }
        }

        public Size MeasureTitle(Graphics g, StiCloudTabulatorPage page)
        {
            Size titleSize = MeasureWrappedString(g, tabulator.Font, page.Text, 95);//Артем: статическая ширина заголовков
            titleSize.Width += 6;
            titleSize.Height += 6;

            Rectangle imageRect = GetTitleImageRectangle(Rectangle.Empty, page);
            
            titleSize.Width = Math.Max(titleSize.Width, imageRect.Width + 6);
            titleSize.Height += imageRect.Height + 6;

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

        public void DrawTitleText(Graphics g, Rectangle rect, StiCloudTabulatorPage page)
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

        public Rectangle GetTitleImageRectangle(Rectangle rect, StiCloudTabulatorPage page)
        {
            SizeF imageSize = SizeF.Empty;
            if (page.Image != null)
            {
                imageSize = page.Image.Size;

                int width = 4;
                if (rect.Width > 0)
                {
                    width = ((int)((rect.Width - imageSize.Width) / 2));
                }

                return new Rectangle(rect.X + width,
                    rect.Y + 10,
                    (int)imageSize.Width, (int)imageSize.Height);
            }
            return Rectangle.Empty;
        }

        public Rectangle GetTitleTextRectangle(Rectangle rect, StiCloudTabulatorPage page)
        {
            Rectangle imageRect = GetTitleImageRectangle(rect, page);
            if (imageRect.IsEmpty) return rect;
            rect.Y += imageRect.Height + 4;
            rect.Height -= imageRect.Height;
            
            return rect;
        }

		#endregion

        #region Fields
        internal int titleSpace = 3;

        private StiCloudTabulator tabulator;
        #endregion

        public StiCloudMode(StiCloudTabulator tabulator)
        {
            this.tabulator = tabulator;
        }
    }
}
