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
using System.Resources;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Brushes for StimulControls colors.
	/// </summary>
	public sealed class StiBrushes
	{
		#region Properties
		private static Brush content = new SolidBrush(StiColors.Content);
		/// <summary>
		/// Gets a defined Brush object with a Content color.
		/// </summary>
		public static Brush Content
		{
			get
			{
				return content;
			}
		}


		private static Brush contentDark = new SolidBrush(StiColors.ContentDark);
		/// <summary>
		/// Gets a defined Brush object with a Dark Content color.
		/// </summary>
		public static Brush ContentDark
		{
			get
			{
				return contentDark;
			}
		}


		private static Brush focus = new SolidBrush(StiColors.Focus);
		/// <summary>
		/// Gets a defined Brush object with a Focus color.
		/// </summary>
		public static Brush Focus
		{
			get
			{
				return focus;
			}
		}


		private static Brush selected = new SolidBrush(StiColors.Selected);
		/// <summary>
		/// Gets a defined Brush object with the Selected color.
		/// </summary>
		public static Brush Selected
		{
			get
			{
				return selected;
			}
		}


		private static Brush selectedText = new SolidBrush(StiColors.SelectedText);
		/// <summary>
		/// Gets a defined Brush object with the SelectedText color.
		/// </summary>
		public static Brush SelectedText
		{
			get
			{
				return selectedText;
			}
		}

		private static Brush window = new SolidBrush(SystemColors.Window);
		/// <summary>
		/// Gets a defined Brush object with the SelectedText color.
		/// </summary>
		public static Brush Window
		{
			get
			{
				return window;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Creates a HatchStyle from the specified name of a HatchStyle object.
		/// </summary>
		/// <param name="name">A string that is the name of a HatchStyle object.</param>
		///	<returns>This method creates The HatchStyle object.</returns>
		public static HatchStyle HatchStyleFromName(string name)
		{
			foreach (var hs in HatchStyles)
			{
				if (hs.ToString() == name)
				{
					return hs;
				}
				
			}
			return HatchStyle.BackwardDiagonal;
		}


		/// <summary>
		/// Creates a ControlBrush from the specified Rectangle and Angle.
		/// </summary>
		/// <param name="rectangle">Rectangle that defines the starting and ending points of the gradient.</param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient orientation line.</param>
		/// <returns>This method creates The LinearGradientBrush object.</returns>
		public static Brush GetControlBrush(Rectangle rectangle, float angle)
		{
			return new LinearGradientBrush(rectangle, StiColors.ControlStart, StiColors.ControlEnd, angle);
		}


		/// <summary>
		/// Creates a ControlDark Brush from the specified Rectangle and Angle.
		/// </summary>
		/// <param name="rectangle">Rectangle that defines the starting and ending points of the gradient.</param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient orientation line.</param>
		/// <returns>This method creates The LinearGradientBrush object.</returns>
		public static Brush GetControlDarkBrush(Rectangle rectangle, float angle)
		{
			return new LinearGradientBrush(rectangle, StiColors.ControlStartDark, StiColors.ControlEndDark, angle);
		}


		/// <summary>
		/// Creates a ControlLight Brush from the specified Rectangle and Angle.
		/// </summary>
		/// <param name="rectangle">Rectangle that defines the starting and ending points of the gradient.</param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient orientation line.</param>
		/// <returns>This method creates The LinearGradientBrush object.</returns>
		public static Brush GetControlLightBrush(Rectangle rectangle, float angle)
		{
			return new LinearGradientBrush(rectangle, StiColors.ControlStartLight, StiColors.ControlEndLight, angle);
		}


		/// <summary>
		/// Creates the ActiveCaption from the specified Rectangle and Angle.
		/// </summary>
		/// <param name="rectangle">Rectangle that defines the starting and ending points of the gradient.</param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient orientation line.</param>
		/// <returns>This method creates The LinearGradientBrush object.</returns>
		public static Brush GetActiveCaptionBrush(Rectangle rectangle, float angle)
		{
			return new LinearGradientBrush(rectangle, StiColors.ActiveCaptionStart, StiColors.ActiveCaptionEnd, angle);
		}


		/// <summary>
		/// Creates an ActiveCaptionLight from the specified Rectangle and Angle.
		/// </summary>
		/// <param name="rectangle">Rectangle that defines the starting and ending points of the gradient.</param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient orientation line.</param>
		/// <returns>This method creates The LinearGradientBrush object.</returns>
		public static Brush GetActiveCaptionLightBrush(Rectangle rectangle, float angle)
		{
			return new LinearGradientBrush(rectangle, 
				StiColorUtils.Light(StiColors.ActiveCaptionStart, 20), 
				StiColorUtils.Light(StiColors.ActiveCaptionEnd, 20), angle);
		}

		#endregion
		
		#region HatchStyles
		private static HatchStyle[] hatchStyles = new HatchStyle[]
		{
			HatchStyle.BackwardDiagonal,
			HatchStyle.Cross,
			HatchStyle.DarkDownwardDiagonal,
			HatchStyle.DarkHorizontal,
			HatchStyle.DarkUpwardDiagonal,
			HatchStyle.DarkVertical,
			HatchStyle.DashedDownwardDiagonal,
			HatchStyle.DashedHorizontal,
			HatchStyle.DashedUpwardDiagonal,
			HatchStyle.DashedVertical,
			HatchStyle.DiagonalBrick,
			HatchStyle.DiagonalCross,
			HatchStyle.Divot,
			HatchStyle.DottedDiamond,
			HatchStyle.DottedGrid,
			HatchStyle.ForwardDiagonal,
			HatchStyle.Horizontal,
			HatchStyle.HorizontalBrick,
			HatchStyle.LargeCheckerBoard,
			HatchStyle.LargeConfetti,
			HatchStyle.LightDownwardDiagonal,
			HatchStyle.LightHorizontal,
			HatchStyle.LightUpwardDiagonal,
			HatchStyle.LightVertical,
			HatchStyle.NarrowHorizontal,
			HatchStyle.NarrowVertical,
			HatchStyle.OutlinedDiamond,
			HatchStyle.Percent05,
			HatchStyle.Percent10,
			HatchStyle.Percent20,
			HatchStyle.Percent25,
			HatchStyle.Percent30,
			HatchStyle.Percent40,
			HatchStyle.Percent50,
			HatchStyle.Percent60,
			HatchStyle.Percent70,
			HatchStyle.Percent75,
			HatchStyle.Percent80,
			HatchStyle.Percent90,
			HatchStyle.Plaid,
			HatchStyle.Shingle,
			HatchStyle.SmallCheckerBoard,
			HatchStyle.SmallConfetti,
			HatchStyle.SmallGrid,
			HatchStyle.SolidDiamond,
			HatchStyle.Sphere,
			HatchStyle.Trellis,
			HatchStyle.Vertical,
			HatchStyle.Weave,
			HatchStyle.WideDownwardDiagonal,
			HatchStyle.WideUpwardDiagonal,
			HatchStyle.ZigZag
		};
		/// <summary>
		/// Array contains all elements of HatchStyle enumeration.
		/// </summary>
		public static HatchStyle[] HatchStyles
		{
			get
			{
				return hatchStyles;
			}
		}

		#endregion

		#region Constructors
		private StiBrushes()
		{
		}
		#endregion
	}
}
