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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Drawing
{
	public sealed class StiFontUtils
	{
        public static FontStyle CorrectStyle(string fontName, FontStyle style)
        {
            var isCustomFont = StiFontCollection.IsCustomFont(fontName);
            FontFamily family = isCustomFont ? StiFontCollection.GetFontFamily(fontName) : new FontFamily(fontName);

            try
            {
                if (family.IsStyleAvailable(style))
                    return style;
                else
                {
                    if ((!family.IsStyleAvailable(FontStyle.Bold)) && (style & FontStyle.Bold) > 0)
                        style -= FontStyle.Bold;

                    if ((!family.IsStyleAvailable(FontStyle.Italic)) && (style & FontStyle.Italic) > 0)
                        style -= FontStyle.Italic;

                    if ((!family.IsStyleAvailable(FontStyle.Strikeout)) && (style & FontStyle.Strikeout) > 0)
                        style -= FontStyle.Strikeout;

                    if ((!family.IsStyleAvailable(FontStyle.Underline)) && (style & FontStyle.Underline) > 0)
                        style -= FontStyle.Underline;

                    if (!family.IsStyleAvailable(style))
                    {
                        if (family.IsStyleAvailable(FontStyle.Bold))
                            return FontStyle.Bold;
                        if (family.IsStyleAvailable(FontStyle.Italic))
                            return FontStyle.Italic;
                        else if (family.IsStyleAvailable(FontStyle.Underline))
                            return FontStyle.Underline;
                        else if (family.IsStyleAvailable(FontStyle.Strikeout))
                            return FontStyle.Strikeout;
                    }

                    return style;
                }
            }
            finally
            {
                if (!isCustomFont)
                    family.Dispose();
            }
        }

		public static Font ChangeFontName(Font font, string newFontName)
		{
			if (string.IsNullOrEmpty(newFontName)) return font;
			
			return new Font(
				StiFontCollection.GetFontFamily(newFontName),
				font.Size,
				font.Style,
				font.Unit,
				font.GdiCharSet,
				font.GdiVerticalFont);
		}

		public static Font ChangeFontSize(Font font, float newFontSize)
		{
			if (newFontSize < 1)newFontSize = 1;
			return new Font(
				font.FontFamily, 
				newFontSize,
				font.Style, 
				font.Unit, 
				font.GdiCharSet, 
				font.GdiVerticalFont);
		}

		public static Font ChangeFontStyleBold(Font font, bool bold)
		{
			FontStyle style = FontStyle.Regular;
			if (bold) style |= FontStyle.Bold;
			if (font.Italic) style |= FontStyle.Italic;
			if (font.Underline) style |= FontStyle.Underline;

			return new Font(
				font.FontFamily,
				font.Size,
				style,
				font.Unit,
				font.GdiCharSet,
				font.GdiVerticalFont);
		}

		public static Font ChangeFontStyleItalic(Font font, bool italic)
		{
			FontStyle style = FontStyle.Regular;
			if (font.Bold) style |= FontStyle.Bold;
			if (italic) style |= FontStyle.Italic;
			if (font.Underline) style |= FontStyle.Underline;

			return new Font(
				font.FontFamily,
				font.Size,
				style,
				font.Unit,
				font.GdiCharSet,
				font.GdiVerticalFont);
		}

		public static Font ChangeFontStyleUnderline(Font font, bool underline)
		{
			FontStyle style = FontStyle.Regular;
			if (font.Bold) style |= FontStyle.Bold;
			if (font.Italic) style |= FontStyle.Italic;
			if (underline) style |= FontStyle.Underline;

			return new Font(
				font.FontFamily,
				font.Size,
				style,
				font.Unit,
				font.GdiCharSet,
				font.GdiVerticalFont);
		}

        public static Font ChangeFontStyleStrikeout(Font font, bool strikeout)
        {
            FontStyle style = FontStyle.Regular;
            if (font.Bold) style |= FontStyle.Bold;
            if (font.Italic) style |= FontStyle.Italic;
			if (font.Underline) style |= FontStyle.Underline;
            if (strikeout) style |= FontStyle.Strikeout;

            return new Font(
                font.FontFamily,
                font.Size,
                style,
                font.Unit,
                font.GdiCharSet,
                font.GdiVerticalFont);
        }

    }
}
