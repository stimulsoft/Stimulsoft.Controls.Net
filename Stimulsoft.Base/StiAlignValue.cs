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

namespace Stimulsoft.Base
{
	/// <summary>
	/// Helps to aligning values.
	/// </summary>
	public sealed class StiAlignValue
	{
		/// <summary>
		/// Aligning value on grid to greater.
		/// </summary>
		/// <param name="value">Value to align.</param>
		/// <param name="gridSize">Grid size.</param>
		/// <param name="aligningToGrid">Align or no.</param>
		/// <returns>Aligned value.</returns>
		public static double AlignToMaxGrid(double value, double gridSize, bool aligningToGrid)
		{
			if (aligningToGrid)
			{
				double b = Math.Round((value / gridSize)) * gridSize;
				if (value > b)b += gridSize;
				value = b;
			}
			return value;
		}

	
		/// <summary>
		/// Aligning value on grid to less.
		/// </summary>
		/// <param name="value">Value to align.</param>
		/// <param name="gridSize">Grid size.</param>
		/// <param name="aligningToGrid">Align or no.</param>
		/// <returns>Aligned value.</returns>
		public static double AlignToMinGrid(double value, double gridSize, bool aligningToGrid)
		{
			if (aligningToGrid)
			{
				double b = Math.Round((value / gridSize)) * gridSize;
				if (value < b)b -= gridSize;
				value = b;
			}
			return value;
		}

		
		/// <summary>
		/// Aligning value on grid.
		/// </summary>
		/// <param name="value">Value to align.</param>
		/// <param name="gridSize">Grid size.</param>
		/// <param name="aligningToGrid">Align or no.</param>
		/// <returns>Aligned value.</returns>
		public static double AlignToGrid(double value, double gridSize, bool aligningToGrid)
		{
			if (aligningToGrid)
			{
				double b = Math.Round((value / gridSize)) * gridSize;
				value = b;
			}
			return value;
		}		
	}
}
