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

namespace Stimulsoft.Base.Drawing
{
	#region StiAction
	/// <summary>
	/// Actions, which indicate actions in the rectangle.
	/// </summary>
	public enum StiAction
	{
		/// <summary>
		/// No action.
		/// </summary>
		None, 
		/// <summary>
		/// Move.
		/// </summary>
		Move, 
		/// <summary>
		/// Select.
		/// </summary>
		Select, 
		/// <summary>
		/// Left side.
		/// </summary>
		SizeLeft, 
		/// <summary>
		/// Right side.
		/// </summary>
		SizeRight, 
		/// <summary>
		/// Top side.
		/// </summary>
		SizeTop, 
		/// <summary>
		/// Bottom side.
		/// </summary>
		SizeBottom,
		/// <summary>
		/// Left top side.
		/// </summary>
		SizeLeftTop, 
		/// <summary>
		/// Left bottom side.
		/// </summary>
		SizeLeftBottom, 
		/// <summary>
		/// Right top side.
		/// </summary>
		SizeRightTop, 
		/// <summary>
		/// Right bottom side.
		/// </summary>
		SizeRightBottom,
		/// <summary>
		/// Resize columns.
		/// </summary>
		ResizeColumns,
		/// <summary>
		/// Resize rows.
		/// </summary>
		ResizeRows,
        /// <summary>
        /// SelectColumn
        /// </summary>
        SelectColumn,
        /// <summary>
        /// SelectRow
        /// </summary>
        SelectRow
	}
	#endregion

	#region StiBorderSides
	/// <summary>
	/// Sides of the border.
	/// </summary>
	[Flags]
	public enum StiBorderSides
	{
		/// <summary>
		/// No border.
		/// </summary>
		None = 0, 
		/// <summary>
		/// Border from all sides.
		/// </summary>
		All = 15, 
		/// <summary>
		/// Border on the top.
		/// </summary>
		Top = 1, 
		/// <summary>
		/// Border on the left.
		/// </summary>
		Left = 2, 
		/// <summary>
		/// Border on the right.
		/// </summary>
		Right = 4, 
		/// <summary>
		/// Border on the bottom.
		/// </summary>
		Bottom = 8
	}
	#endregion    

    #region StiPenStyle
    [Editor("Stimulsoft.Base.Drawing.Design.StiPenStyleEditor, Stimulsoft.Report.Design, " + StiVersion.VersionInfo, typeof(UITypeEditor))]
	public enum StiPenStyle
	{
		/// <summary>
		/// Solid line.
		/// </summary>
		Solid = 0,
		/// <summary>
		/// Dotted line.
		/// </summary>
		Dash,
		/// <summary>
		/// Dash-dotted line.
		/// </summary>
		DashDot,
		/// <summary>
		/// Dotted-dash-dash line.
		/// </summary>
		DashDotDot,
		/// <summary>
		/// Dotted line.
		/// </summary>
		Dot,
		/// <summary>
		/// Double line.
		/// </summary>
		Double,
		/// <summary>
		/// No.
		/// </summary>
		None
	}
    #endregion

	#region StiRotationMode
	public enum StiRotationMode
	{
		LeftTop,
		LeftCenter,
		LeftBottom,
		CenterTop,
		CenterCenter,
		CenterBottom,
		RightTop,
		RightCenter,
		RightBottom
	}
	#endregion

	#region StiShadowSides
	[Flags]
	public enum StiShadowSides
	{
		Top = 1,
		Right = 2,
		Edge = 4,		
		Bottom = 8,
		Left = 16,
		All = 31
	}
	#endregion

	#region StiVertAlignment
	/// <summary>
	/// Vertical alignment of an object.
	/// </summary>
	public enum StiVertAlignment 
	{
		/// <summary>
		/// Align top.
		/// </summary>
		Top, 
		/// <summary>
		/// Align center.
		/// </summary>
		Center, 
		/// <summary>
		/// Align bottom.
		/// </summary>
		Bottom
	}
	#endregion

	#region StiTextHorAlignment
	/// <summary>
	/// Horizontal alignment of a text.
	/// </summary>
	public enum StiTextHorAlignment 
	{
		/// <summary>
		/// Align left.
		/// </summary>
		Left, 
		/// <summary>
		/// Align center.
		/// </summary>
		Center, 
		/// <summary>
		/// Align right.
		/// </summary>
		Right, 
		/// <summary>
		/// Align width.
		/// </summary>
		Width
	}
	#endregion

	#region StiHorAlignment
	/// <summary>
	/// Horizontal alignment of an object.
	/// </summary>
	public enum StiHorAlignment
	{
		/// <summary>
		/// Align left.
		/// </summary>
		Left = 1, 
		/// <summary>
		/// Align center.
		/// </summary>
		Center = 2,
		/// <summary>
		/// Align right.
		/// </summary>
		Right = 3
	}
	#endregion

	#region StiTextDockMode
	public enum StiTextDockMode
	{
		Top,
		Bottom,
		Left,
		Right
	}
	#endregion
}
