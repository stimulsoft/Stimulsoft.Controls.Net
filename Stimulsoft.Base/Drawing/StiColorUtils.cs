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
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Text;


namespace Stimulsoft.Base.Drawing
{
    public sealed class StiColorUtils
    {
		/// <summary>
		/// Retrieves the current color of the specified display element.
		/// </summary>
		/// <param name="colorType">Specifies the display element whose color is to be retrieved.</param>
		/// <returns>Color value of the given element.</returns>
		public static Color GetSysColor(Win32.ColorType colorType)
		{
			return ColorTranslator.FromWin32(Win32.GetSysColor((int)colorType));
		}

        public static Color Light(Color baseColor, byte value)
        {
            byte R = baseColor.R;
            byte G = baseColor.G;
            byte B = baseColor.B;

            if ((R + value) > 255) R = 255;
            else R += value;

            if ((G + value) > 255) G = 255;
            else G += value;

            if ((B + value) > 255) B = 255;
            else B += value;

            return Color.FromArgb(R, G, B);
        }

        public static Color MixingColors(Color color1, Color color2, int alpha)
        {
            int r = (color2.R * alpha) / 255 + color1.R * (255 - alpha) / 255;
            int g = (color2.G * alpha) / 255 + color1.G * (255 - alpha) / 255;
            int b = (color2.B * alpha) / 255 + color1.B * (255 - alpha) / 255;

            return Color.FromArgb(255, r, g, b);
        }

        public static Color Dark(Color baseColor, byte value)
        {
            byte R = baseColor.R;
            byte G = baseColor.G;
            byte B = baseColor.B;

            if ((R - value) < 0) R = 0;
            else R -= value;

            if ((G - value) < 0) G = 0;
            else G -= value;

            if ((B - value) < 0) B = 0;
            else B -= value;

            return Color.FromArgb(R, G, B);
        }


		public static Color GetDisabledColor(Control control)
		{
			Color color = control.BackColor;
			if (color.A == 0)
			{
				for (Control ctrl = control.Parent; color.A == 0 || color.IsEmpty; ctrl = control.Parent)
				{
					if (ctrl == null)return SystemColors.Control;		
					color = ctrl.BackColor;
				}
			}
			return color;
		}
		

		public static int GetColorRop(Color color, int darkROP, int lightROP)
		{
			if (color.GetBrightness() < 0.5f)return darkROP;
			return lightROP;
		}

    }
}
