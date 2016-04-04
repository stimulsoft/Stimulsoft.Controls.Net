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
	public class StiLeftHorizontalMode : StiTabulatorMode
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

			return new Rectangle(titleSpace, 
				pageIndex * tabulator.MaxTitleHeight + startPosTitle, 
				tabulator.MaxTitleWidth, tabulator.MaxTitleHeight);
		}		

		public override Rectangle GetTitleRectangle()
		{
			if (tabulator.Controls.Count > 0)
			{
				return new Rectangle(0, 0, tabulator.MaxTitleWidth + titleSpace, tabulator.Height);
			}
			else 
			{
				return new Rectangle(0, 0, tabulator.Width, tabulator.Height);
			}
		}

		public override Rectangle GetClientRectangle()
		{
			return new Rectangle(tabulator.MaxTitleWidth, 0, 
				tabulator.Width - tabulator.MaxTitleWidth, tabulator.Height);
		}

		public override void SetLayout()
		{
			if (tabulator.IsDesignMode || tabulator.ShowTitle)
			{
				tabulator.DockPadding.Left = tabulator.MaxTitleWidth + titleSpace;
			}
			else
			{
				tabulator.DockPadding.Left = 0;
			}
			tabulator.DockPadding.Right = 0;
			tabulator.DockPadding.Top = 0;
			tabulator.DockPadding.Bottom = 0;
		}

		#endregion

		#region Properties
		public override StiTabTitlePosition TabTitlePosition
		{
			get
			{
				return StiTabTitlePosition.LeftHorizontal;
			}
		}

		#endregion

		public StiLeftHorizontalMode(StiTabulator tabulator) : base(tabulator)
		{
		}
	}
}
