#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports 									            }
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
{	TRADE SECRETS OF STIMULSOFT										}
{																	}
{	CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON		}
{	ADDITIONAL RESTRICTIONS.										}
{																	}
{*******************************************************************}
*/
#endregion Copyright (C) 2003-2016 Stimulsoft

using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context
{
    public class StiLabelAnimationGeom : StiAnimationGeom
    {
        #region IStiJsonReportObject.override
        public override JObject SaveToJsonObject(StiJsonSaveMode mode)
        {
            var jObject = base.SaveToJsonObject(mode);

            jObject.Add(new JProperty("Text", Text));
            jObject.Add(new JProperty("Font", Font.SaveToJsonObject(mode)));
            jObject.Add(new JProperty("Rectangle", SaveRectangleToJsonObject(Rectangle)));
            jObject.Add(new JProperty("Angle", Angle));
            jObject.Add(new JProperty("DrawBorder", DrawBorder));
            if (TextBrush != null) jObject.Add(new JProperty("TextBrush", SaveBrushToJsonObject(TextBrush, mode)));
            if (LabelBrush != null) jObject.Add(new JProperty("LabelBrush", SaveBrushToJsonObject(LabelBrush, mode)));
            if (PenBorder != null) jObject.Add(new JProperty("PenBorder", PenBorder.SaveToJsonObject(mode)));
            if (StringFormat != null) jObject.Add(new JProperty("StringFormat", StringFormat.SaveToJsonObject(mode)));
            if (RotationMode != null) jObject.Add(new JProperty("RotationMode", RotationMode.Value.ToString()));

            return jObject;
        }

        public override void LoadFromJsonObject(JObject jObject)
        {
        }
        #endregion

        #region Fields
        public string Text;
        public StiFontGeom Font;
        public object TextBrush;
        public object LabelBrush;
        public StiPenGeom PenBorder;
        public Rectangle Rectangle;
        public StiStringFormatGeom StringFormat;
        public float Angle;
        public StiRotationMode? RotationMode;
        public bool DrawBorder;
        #endregion

        #region Properties override
        public override StiGeomType Type
        {
            get
            {
                return StiGeomType.AnimationLabel;
            }
        }
        #endregion

        public StiLabelAnimationGeom(string text, StiFontGeom font, object textBrush, object LabelBrush, StiPenGeom penBorder, Rectangle rect, StiStringFormatGeom sf, StiRotationMode rotationMode,
            float angle, bool drawBorder, TimeSpan duration, TimeSpan? beginTime) : base(duration, beginTime)
        {
            this.Text = text;
            this.Font = font;
            this.TextBrush = textBrush;
            this.LabelBrush = LabelBrush;
            this.PenBorder = penBorder;
            this.Rectangle = rect;
            this.StringFormat = sf;
            this.RotationMode = rotationMode;
            this.Angle = angle;
            this.DrawBorder = drawBorder;
        }
    }
}
