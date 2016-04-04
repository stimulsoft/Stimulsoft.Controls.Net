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
using System.Text;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Colors of StiControls.
	/// </summary>
	public sealed class StiColors
	{
		#region Properties
		#region CustomColors
		private static Color[] customColors = new Color[]{

															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 192, 192),
															 Color.FromArgb(255, 224, 192),
															 Color.FromArgb(255, 255, 192),
															 Color.FromArgb(192, 255, 192),
															 Color.FromArgb(192, 255, 255),
															 Color.FromArgb(192, 192, 255),
															 Color.FromArgb(255, 192, 255),
															 Color.FromArgb(224, 224, 224),
															 Color.FromArgb(255, 128, 128),
															 Color.FromArgb(255, 192, 128),
															 Color.FromArgb(255, 255, 128),
															 Color.FromArgb(128, 255, 128),
															 Color.FromArgb(128, 255, 255),
															 Color.FromArgb(128, 128, 255),
															 Color.FromArgb(255, 128, 255),
															 Color.FromArgb(192, 192, 192),
															 Color.FromArgb(255, 0, 0),
															 Color.FromArgb(255, 128, 0),
															 Color.FromArgb(255, 255, 0),
															 Color.FromArgb(0, 255, 0),
															 Color.FromArgb(0, 255, 255),
															 Color.FromArgb(0, 0, 255),
															 Color.FromArgb(255, 0, 255),
															 Color.FromArgb(128, 128, 128),
															 Color.FromArgb(192, 0, 0),
															 Color.FromArgb(192, 64, 0),
															 Color.FromArgb(192, 192, 0),
															 Color.FromArgb(0, 192, 0),
															 Color.FromArgb(0, 192, 192),
															 Color.FromArgb(0, 0, 192),
															 Color.FromArgb(192, 0, 192),
															 Color.FromArgb(64, 64, 64),
															 Color.FromArgb(128, 0, 0),
															 Color.FromArgb(128, 64, 0),
															 Color.FromArgb(128, 128, 0),
															 Color.FromArgb(0, 128, 0),
															 Color.FromArgb(0, 128, 128),
															 Color.FromArgb(0, 0, 128),
															 Color.FromArgb(128, 0, 128),
															 Color.FromArgb(0, 0, 0),
															 Color.FromArgb(64, 0, 0),
															 Color.FromArgb(128, 64, 64),
															 Color.FromArgb(64, 64, 0),
															 Color.FromArgb(0, 64, 0),
															 Color.FromArgb(0, 64, 64),
															 Color.FromArgb(0, 0, 64),
															 Color.FromArgb(64, 0, 64),
										  
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255),
															 Color.FromArgb(255, 255, 255)
														 };
		public static Color[] CustomColors
		{
			get
			{
				return customColors;
			}
		}
		#endregion

		#region SystemColors
		private static Color[] systemColors = new Color[]{
															 System.Drawing.SystemColors.ActiveBorder,
															 System.Drawing.SystemColors.ActiveCaption,
															 System.Drawing.SystemColors.ActiveCaptionText,
															 System.Drawing.SystemColors.AppWorkspace,
															 System.Drawing.SystemColors.Control,
															 System.Drawing.SystemColors.ControlDark,
															 System.Drawing.SystemColors.ControlDarkDark,
															 System.Drawing.SystemColors.ControlLight,
															 System.Drawing.SystemColors.ControlLightLight,
															 System.Drawing.SystemColors.ControlText,
															 System.Drawing.SystemColors.Desktop,
															 System.Drawing.SystemColors.GrayText,
															 System.Drawing.SystemColors.Highlight,
															 System.Drawing.SystemColors.HighlightText,
															 System.Drawing.SystemColors.HotTrack,
															 System.Drawing.SystemColors.InactiveBorder,
															 System.Drawing.SystemColors.InactiveCaption,
															 System.Drawing.SystemColors.InactiveCaptionText,
															 System.Drawing.SystemColors.Info,
															 System.Drawing.SystemColors.InfoText,
															 System.Drawing.SystemColors.Menu,
															 System.Drawing.SystemColors.MenuText,
															 System.Drawing.SystemColors.ScrollBar,
															 System.Drawing.SystemColors.Window,
															 System.Drawing.SystemColors.WindowFrame,
															 System.Drawing.SystemColors.WindowText
														 };
		public static Color[] SystemColors
		{
			get
			{
				return systemColors;
			}
		}
		#endregion

		#region Colors
		private static Color[] colors = new Color[]{
													   Color.Transparent,
													   Color.Black,
													   Color.DimGray,
													   Color.Gray,
													   Color.DarkGray,
													   Color.Silver,
													   Color.LightGray,
													   Color.Gainsboro,
													   Color.WhiteSmoke,
													   Color.White,
													   Color.RosyBrown,
													   Color.IndianRed,
													   Color.Brown,
													   Color.Firebrick,
													   Color.LightCoral,
													   Color.Maroon,
													   Color.DarkRed,
													   Color.Red,
													   Color.Snow,
													   Color.MistyRose,
													   Color.Salmon,
													   Color.Tomato,
													   Color.DarkSalmon,
													   Color.Coral,
													   Color.OrangeRed,
													   Color.LightSalmon,
													   Color.Sienna,
													   Color.SeaShell,
													   Color.Chocolate,
													   Color.SaddleBrown,
													   Color.SandyBrown,
													   Color.PeachPuff,
													   Color.Peru,
													   Color.Linen,
													   Color.Bisque,
													   Color.DarkOrange,
													   Color.BurlyWood,
													   Color.Tan,
													   Color.AntiqueWhite,
													   Color.NavajoWhite,
													   Color.BlanchedAlmond,
													   Color.PapayaWhip,
													   Color.Moccasin,
													   Color.Orange,
													   Color.Wheat,
													   Color.OldLace,
													   Color.FloralWhite,
													   Color.DarkGoldenrod,
													   Color.Goldenrod,
													   Color.Cornsilk,
													   Color.Gold,
													   Color.Khaki,
													   Color.LemonChiffon,
													   Color.PaleGoldenrod,
													   Color.DarkKhaki,
													   Color.Beige,
													   Color.LightGoldenrodYellow,
													   Color.Olive,
													   Color.Yellow,
													   Color.LightYellow,
													   Color.Ivory,
													   Color.OliveDrab,
													   Color.YellowGreen,
													   Color.DarkOliveGreen,
													   Color.GreenYellow,
													   Color.Chartreuse,
													   Color.LawnGreen,
													   Color.DarkSeaGreen,
													   Color.ForestGreen,
													   Color.LimeGreen,
													   Color.LightGreen,
													   Color.PaleGreen,
													   Color.DarkGreen,
													   Color.Green,
													   Color.Lime,
													   Color.Honeydew,
													   Color.SeaGreen,
													   Color.MediumSeaGreen,
													   Color.SpringGreen,
													   Color.MintCream,
													   Color.MediumSpringGreen,
													   Color.MediumAquamarine,
													   Color.Aquamarine,
													   Color.Turquoise,
													   Color.LightSeaGreen,
													   Color.MediumTurquoise,
													   Color.DarkSlateGray,
													   Color.PaleTurquoise,
													   Color.Teal,
													   Color.DarkCyan,
													   Color.Aqua,
													   Color.Cyan,
													   Color.LightCyan,
													   Color.Azure,
													   Color.DarkTurquoise,
													   Color.CadetBlue,
													   Color.PowderBlue,
													   Color.LightBlue,
													   Color.DeepSkyBlue,
													   Color.SkyBlue,
													   Color.LightSkyBlue,
													   Color.SteelBlue,
													   Color.AliceBlue,
													   Color.DodgerBlue,
													   Color.SlateGray,
													   Color.LightSlateGray,
													   Color.LightSteelBlue,
													   Color.CornflowerBlue,
													   Color.RoyalBlue,
													   Color.MidnightBlue,
													   Color.Lavender,
													   Color.Navy,
													   Color.DarkBlue,
													   Color.MediumBlue,
													   Color.Blue,
													   Color.GhostWhite,
													   Color.SlateBlue,
													   Color.DarkSlateBlue,
													   Color.MediumSlateBlue,
													   Color.MediumPurple,
													   Color.BlueViolet,
													   Color.Indigo,
													   Color.DarkOrchid,
													   Color.DarkViolet,
													   Color.MediumOrchid,
													   Color.Thistle,
													   Color.Plum,
													   Color.Violet,
													   Color.Purple,
													   Color.DarkMagenta,
													   Color.Magenta,
													   Color.Fuchsia,
													   Color.Orchid,
													   Color.MediumVioletRed,
													   Color.DeepPink,
													   Color.HotPink,
													   Color.LavenderBlush,
													   Color.PaleVioletRed,
													   Color.Crimson,
													   Color.Pink,
													   Color.LightPink 
												   };
		public static Color[] Colors
		{
			get
			{
				return colors;
			}
		}
		#endregion


		private static Color focus;
		/// <summary>
		/// Gets a Focus color.
		/// </summary>
		public static Color Focus
		{
			get
			{
				return focus;
			}
			set
			{
				focus = value;
			}
		}


		private static Color controlStart;
		/// <summary>
		/// Gets a ControlStart color.
		/// </summary>
		public static Color ControlStart
		{
			get
			{
				return controlStart;
			}
			set
			{
				controlStart = value;
			}
		}


		private static Color controlEnd;
		/// <summary>
		/// Gets a ControlEnd color.
		/// </summary>
		public static Color ControlEnd
		{
			get
			{
				return controlEnd;
			}
			set
			{
				controlEnd = value;
			}
		}


		private static Color content;
		/// <summary>
		/// Gets a Content color.
		/// </summary>
		public static Color Content
		{
			get
			{
				return content;
			}
			set
			{
				content = value;
			}
		}

		private static Color contentDark;
		/// <summary>
		/// Gets a Dark Content color.
		/// </summary>
		public static Color ContentDark
		{
			get
			{
				return contentDark;
			}
			set
			{
				contentDark = value;
			}
		}


		private static Color controlText;
		/// <summary>
		/// Gets a ControlText color.
		/// </summary>
		public static Color ControlText
		{
			get
			{
				return controlText;
			}
			set
			{
				controlText = value;
			}
		}


		private static Color selected;
		/// <summary>
		/// Gets a Selected color.
		/// </summary>
		public static Color Selected
		{
			get
			{
				return selected;
			}
			set
			{
				selected = value;
			}
		}


		private static Color selectedText;
		/// <summary>
		/// Gets a SelectedText color.
		/// </summary>
		public static Color SelectedText
		{
			get
			{
				return selectedText;
			}
			set
			{
				selectedText = value;
			}
		}
		

		private static Color controlStartLight;
		/// <summary>
		/// Gets a ControlStartLight color.
		/// </summary>
		public static Color ControlStartLight
		{
			get
			{
				return controlStartLight;
			}
			set
			{
				controlStartLight = value;
			}
		}


		private static Color controlEndLight;
		/// <summary>
		/// Gets a ControlEndLight color.
		/// </summary>
		public static Color ControlEndLight
		{
			get
			{
				return controlEndLight;
			}
			set
			{
				controlEndLight = value;
			}
		}


		private static Color controlStartDark;
		/// <summary>
		/// Gets a ControlStartDark color.
		/// </summary>
		public static Color ControlStartDark
		{
			get
			{
				return controlStartDark;
			}
			set
			{
				controlStartDark = value;
			}
		}


		private static Color controlEndDark;
		/// <summary>
		/// Gets a ControlEndDark color.
		/// </summary>
		public static Color ControlEndDark
		{
			get
			{
				return controlEndDark;
			}
			set
			{
				controlEndDark = value;
			}
		}
        

		private static Color activeCaptionStart;
		/// <summary>
		/// Gets an ActiveCaptionStart color.
		/// </summary>
		public static Color ActiveCaptionStart
		{
			get
			{
				return activeCaptionStart;
			}
			set
			{
				activeCaptionStart = value;
			}
		}


		private static Color activeCaptionEnd;
		/// <summary>
		/// Gets an ActiveCaptionEnd color.
		/// </summary>
		public static Color ActiveCaptionEnd
		{
			get
			{
				return activeCaptionEnd;
			}
			set
			{
				activeCaptionEnd = value;
			}
		}

		private static Color inactiveCaptionStart;
		public static Color InactiveCaptionStart
		{
			get
			{
				return inactiveCaptionStart;
			}
			set
			{
				inactiveCaptionStart = value;
			}
		}


		private static Color inactiveCaptionEnd;
		public static Color InactiveCaptionEnd
		{
			get
			{
				return inactiveCaptionEnd;
			}
			set
			{
				inactiveCaptionEnd = value;
			}
		}
		#endregion

		#region Methods
		public static void InitColors()
		{            
            activeCaptionEnd =		System.Drawing.SystemColors.ActiveCaption;			
			inactiveCaptionEnd =	System.Drawing.SystemColors.InactiveCaption;

			focus =					CalcColor(System.Drawing.SystemColors.Highlight, System.Drawing.SystemColors.Window, 70);
			selected =				CalcColor(System.Drawing.SystemColors.Highlight, System.Drawing.SystemColors.Window, 30);
			selectedText =			CalcColor(System.Drawing.SystemColors.Highlight, System.Drawing.SystemColors.Window, 220);
			content =				CalcColor(System.Drawing.SystemColors.Window,	 System.Drawing.SystemColors.Control, 200);
			contentDark =			StiColorUtils.Dark(StiColors.Content, 10);
			controlStart =			StiColorUtils.Light(System.Drawing.SystemColors.Control, 30);
			controlEnd =			StiColorUtils.Dark(System.Drawing.SystemColors.Control, 10);
			controlText =			System.Drawing.SystemColors.ControlText;			

			controlStartLight =		StiColorUtils.Light(ControlStart, 20);
			controlEndLight =		StiColorUtils.Light(ControlEnd, 20);
			controlStartDark =		StiColorUtils.Dark(ControlStart, 20);
			controlEndDark =		StiColorUtils.Dark(ControlEnd, 20);

            activeCaptionStart = StiColorUtils.GetSysColor(Win32.ColorType.COLOR_GRADIENTACTIVECAPTION);
            inactiveCaptionStart =	StiColorUtils.GetSysColor(Win32.ColorType.COLOR_GRADIENTINACTIVECAPTION);
        }

		private static Color CalcColor(Color front, Color back, int alpha)
		{
			Color frontColor =	Color.FromArgb(255, front);
			Color backColor =	Color.FromArgb(255, back);
									
			float frontRed =	frontColor.R;
			float frontGreen =	frontColor.G;
			float frontBlue =	frontColor.B;
			float backRed =		backColor.R;
			float backGreen =	backColor.G;
			float backBlue =	backColor.B;
			
			float fRed = frontRed*alpha / 255 + backRed * ((float)(255 - alpha) / 255);
			byte newRed = (byte)fRed;
			float fGreen = frontGreen * alpha / 255 + backGreen * ((float)(255-alpha) / 255);
			byte newGreen = (byte)fGreen;
			float fBlue = frontBlue * alpha / 255 + backBlue * ((float)(255 - alpha) / 255);
			byte newBlue = (byte)fBlue;

			return  Color.FromArgb(255, newRed, newGreen, newBlue);
		}

		#endregion

		#region Constructors
		static StiColors()
		{			
			InitColors();
		}

		private StiColors()
		{
		}
		#endregion
	}
}
