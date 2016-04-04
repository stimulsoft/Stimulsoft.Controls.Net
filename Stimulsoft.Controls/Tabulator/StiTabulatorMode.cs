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
	public abstract class StiTabulatorMode
	{
		#region Fields
		internal int startPosTitle = 8;
		internal int endPosTitle = 8;
		internal int titleSpace = 3;
		protected StiTabulator tabulator = null;
		#endregion

		#region Methods
		public abstract Rectangle GetTitlePageRectangle(StiTabulatorPage page);
		public abstract Rectangle GetTitleRectangle();
		public void DrawPage(Graphics g, StiTabulatorPage page)
		{
            tabulator.Style.DrawPage(g, TabTitlePosition, page);
		}

		public void DrawPageTitle(Graphics g, StiTabulatorPage page)
		{
            tabulator.Style.DrawPageTitle(g, TabTitlePosition, page);
		}
		public abstract Rectangle GetClientRectangle();
		public abstract void SetLayout();
		
		public virtual Size MeasureTitle(Graphics g, StiTabulatorPage page)
		{
			Size titleSize = g.MeasureString(page.Text, tabulator.Font).ToSize();
			titleSize.Width += 6;
			titleSize.Height += 6;

			Rectangle imageRect = GetTitleImageRectangle(Rectangle.Empty, page);
			titleSize.Width += imageRect.Width + 6;
			titleSize.Height = Math.Max(titleSize.Height, imageRect.Height + 6);

			if (tabulator.TitleWidth > 0)titleSize.Width = tabulator.TitleWidth;
			if (tabulator.TitleHeight > 0)titleSize.Height = tabulator.TitleHeight;

			return titleSize;
		}

		public virtual Rectangle GetTitleImageRectangle(Rectangle rect, StiTabulatorPage page)
		{
			SizeF imageSize = SizeF.Empty;
			if (tabulator.ImageList != null || page.Image != null)
			{
				if (tabulator.ImageList != null || page.Image != null)
				{
					if (page.Image != null)imageSize = page.Image.Size;
					else imageSize = tabulator.ImageList.ImageSize;

					return new Rectangle(rect.X + 4,
						(int)(rect.Y + (rect.Height - imageSize.Height) / 2), 
						(int)imageSize.Width, (int)imageSize.Height);
					
				}
			}
			return Rectangle.Empty;
		}

		public virtual Rectangle GetTitleTextRectangle(Rectangle rect, StiTabulatorPage page)
		{
			Rectangle imageRect	= GetTitleImageRectangle(rect, page);
			if (imageRect.IsEmpty)return rect;
			rect.Width -= imageRect.Width;
			rect.X += imageRect.Width;
			return rect;
		}

		public virtual void DrawTitleImage(Graphics g, Rectangle rect, StiTabulatorPage page)
		{
			Rectangle imageRect = GetTitleImageRectangle(rect, page);

			if (!imageRect.IsEmpty)
			{
				int pageIndex = page.PageIndex;

				if (tabulator.ImageList != null && tabulator.ImageList.Images.Count > pageIndex)
				{
					tabulator.ImageList.Draw(g, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height, pageIndex);
				}
				else if (page.Image != null)
				{
					g.DrawImage(page.Image, imageRect.X, imageRect.Y, imageRect.Width, imageRect.Height);
				}
			}
		}

		public virtual void DrawTitleText(Graphics g, Rectangle rect, StiTabulatorPage page)
		{
			Rectangle textRect = GetTitleTextRectangle(rect, page);
			g.DrawString(page.Text, tabulator.Font, SystemBrushes.ControlText, textRect, tabulator.sfTabs);
		}
		
		public virtual void MeasureTitles()
		{
			foreach (StiTabulatorPage page in tabulator.Controls)
			{
				page.MeasureTitle();
			}			
		}
		#endregion

		#region Properties
		public abstract StiTabTitlePosition TabTitlePosition
		{
			get;
		}

		#endregion		

		public StiTabulatorMode(StiTabulator tabulator)
		{
			this.tabulator = tabulator;
		}
	}
}
