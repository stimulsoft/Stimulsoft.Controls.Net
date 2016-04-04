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
	public class StiTopHorizontalMode : StiTabulatorMode
	{
		#region Methods
		public override Rectangle GetTitlePageRectangle(StiTabulatorPage page)
		{
			int pos = startPosTitle;
			foreach (StiTabulatorPage pg in page.Parent.Controls)
			{
				if (pg == page)break;
				if ((!pg.Invisible) || tabulator.IsDesignMode)pos += pg.TitleSize.Width;
			}
			return new Rectangle(
				pos, titleSpace,
				 page.TitleSize.Width, tabulator.MaxTitleHeight);
		}

		public override Rectangle GetTitleRectangle()
		{
			if (tabulator.Controls.Count > 0)
			{
				return new Rectangle(0, 0, tabulator.Width, tabulator.MaxTitleHeight + titleSpace);
			}
			else 
			{
				return new Rectangle(0, 0, tabulator.Width, tabulator.Height);
			}
		}
	
		public override Rectangle GetClientRectangle()
		{
			return new Rectangle(0, tabulator.MaxTitleHeight, 
				tabulator.Width, tabulator.Height - tabulator.MaxTitleHeight);
		}

		public override void SetLayout()
		{
			if (tabulator.IsDesignMode || tabulator.ShowTitle)
			{
				tabulator.DockPadding.Top = tabulator.MaxTitleHeight + titleSpace;
			}
			else
			{
				tabulator.DockPadding.Top = 0;
			}
			tabulator.DockPadding.Left = 0;
			tabulator.DockPadding.Right = 0;			
			tabulator.DockPadding.Bottom = 0;
		}

		public override void MeasureTitles()
		{
			base.MeasureTitles();
            /*
			int width = 0;
			foreach (StiTabulatorPage page in tabulator.Controls)
			{
				if ((!page.Invisible) || tabulator.IsDesignMode)width += page.TitleSize.Width;
			}

			int maxWidth = tabulator.Width - startPosTitle - endPosTitle;

            if (width > maxWidth)
            {
                float factor = (float)maxWidth / (float)width;

                foreach (StiTabulatorPage page in tabulator.Controls)
                {
                    Size size = page.TitleSize;
                    size.Width = (int)((float)size.Width * factor);
                    page.TitleSize = size;
                }
            }
*/
        }		
		#endregion

		#region Properties
		public override StiTabTitlePosition TabTitlePosition
		{
			get
			{
				return StiTabTitlePosition.TopHorizontal;
			}
		}

		#endregion

		public StiTopHorizontalMode(StiTabulator tabulator) : base(tabulator)
		{			
		}
	}
}
