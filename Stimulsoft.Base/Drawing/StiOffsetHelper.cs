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
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Drawing
{
	public sealed class StiOffsetHelper
	{
		/// <summary>
		/// Changes the sizes of the rectangle.
		/// </summary>
		/// <param name="offsettingRectangle">Data for change the size.</param>
		/// <returns>Changed rectangle.</returns>
		public static Rectangle OffsetRect(Rectangle rect, Rectangle offsettingRectangle)
		{
			return new Rectangle(
				rect.X - offsettingRectangle.X,
				rect.Y - offsettingRectangle.Y,
				rect.Width + offsettingRectangle.Width,
				rect.Height + offsettingRectangle.Height);
		}
	}
}
