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
using System.Windows.Forms;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Represents a standard Windows vertical scroll bar.
	/// </summary>
	[ToolboxItem(false)]
	public class StiVScrollBar : VScrollBar
	{
		public bool IsStart
		{
			get
			{
				return Value == 0;
			}
		}


		public bool IsEnd
		{
			get
			{
				return Value == (Maximum - LargeChange);
			}
		}

		private void SetValue(int value)
		{
			if (value < 0)this.Value = 0;
			else this.Value = value;
		}


		public void DoUp()
		{
			if (this.Value - this.SmallChange < 0)SetValue(0);
			else SetValue(this.Value - this.SmallChange);
		}

		public void DoDown()
		{
			if (this.Value + this.SmallChange > (Maximum - LargeChange + 1))
				SetValue(Maximum - LargeChange + 1);
			else SetValue(this.Value + this.SmallChange);
		}

		public void DoPageUp()
		{
			if (this.Value - this.LargeChange < 0)SetValue(0);
			else SetValue(this.Value - this.LargeChange);
		}

		public void DoPageDown()
		{
			if (this.Value + this.LargeChange >= this.Maximum - this.LargeChange)
				SetValue(this.Maximum - this.LargeChange);
			else SetValue(this.Value + this.LargeChange);
		}

		public void DoTop()
		{
			SetValue(0);
			Invalidate();
		}

		public void DoBottom()
		{
			SetValue(this.Maximum - this.LargeChange);
		}
	}
}
