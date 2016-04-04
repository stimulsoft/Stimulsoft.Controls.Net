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
using System.Collections.Generic;
using System.Text;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context
{
    public class StiFontGeom : StiGeom
    {
        #region IStiJsonReportObject.override
        public override JObject SaveToJsonObject(StiJsonSaveMode mode)
        {
            var jObject = base.SaveToJsonObject(mode);

            jObject.Add(new JProperty("FontName", FontName));
            jObject.Add(new JProperty("FontSize", FontSize));
            jObject.Add(new JProperty("FontStyle", FontStyle.ToString()));
            jObject.Add(new JProperty("Unit", Unit.ToString()));
            jObject.Add(new JProperty("GdiCharSet", GdiCharSet));
            jObject.Add(new JProperty("GdiVerticalFont", GdiVerticalFont));

            return jObject;
        }

        public override void LoadFromJsonObject(JObject jObject)
        {
        }
        #endregion

        #region Methods
        public static StiFontGeom ChangeFontSize(Font font, float newFontSize)
        {
            if (newFontSize < 1) newFontSize = 1;
            return new StiFontGeom(
                font.FontFamily.Name,
                newFontSize,
                font.Style,
                font.Unit,
                font.GdiCharSet,
                font.GdiVerticalFont);
        }
        #endregion

        #region Fields
        public string FontName;
        public float FontSize;
        public FontStyle FontStyle;
        public GraphicsUnit Unit;
        public byte GdiCharSet;
        public bool GdiVerticalFont;
        #endregion

        #region Properties
        public override StiGeomType Type
        {
            get
            {
                return StiGeomType.Font;
            }
        }
        #endregion

        public StiFontGeom(Font font)
            : this(font.FontFamily.Name, font.Size, font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont)
        {
        }

        public StiFontGeom(string fontName, float fontSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
        {
            this.FontName = fontName;
            this.FontSize = fontSize;
            this.FontStyle = style;
            this.Unit = unit;
            this.GdiCharSet = gdiCharSet;
            this.GdiVerticalFont = gdiVerticalFont;
            
        }
    }
}
