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
    public class StiTextGeom : StiGeom
    {
        #region IStiJsonReportObject.override
        public override JObject SaveToJsonObject(StiJsonSaveMode mode)
        {
            var jObject = base.SaveToJsonObject(mode);

            jObject.Add(new JProperty("Text", Text));
            jObject.Add(new JProperty("Font", Font.SaveToJsonObject(mode)));
            jObject.Add(new JProperty("IsRounded", IsRounded));
            jObject.Add(new JProperty("IsRotatedText", IsRotatedText));
            jObject.Add(new JProperty("Angle", Angle));
            jObject.Add(new JProperty("Antialiasing", Antialiasing));
            jObject.Add(new JProperty("MaximalWidth", MaximalWidth));
            if (Brush != null) jObject.Add(new JProperty("Brush", SaveBrushToJsonObject(Brush, mode)));
            if (StringFormat != null) jObject.Add(new JProperty("StringFormat", StringFormat.SaveToJsonObject(mode)));
            if (RotationMode != null) jObject.Add(new JProperty("RotationMode", RotationMode.Value.ToString()));

            if (Location is PointF) jObject.Add(new JProperty("Location", SavePointFToJsonObject((PointF)Location)));
            if (Location is Rectangle) jObject.Add(new JProperty("Location", SaveRectangleToJsonObject((Rectangle)Location)));
            if (Location is RectangleF) jObject.Add(new JProperty("Location", SaveRectangleFToJsonObject((RectangleF)Location)));
            if (Location is RectangleD) jObject.Add(new JProperty("Location", SaveRectangleDToJsonObject((RectangleD)Location)));

            return jObject;
        }

        public override void LoadFromJsonObject(JObject jObject)
        {
        }
        #endregion

        #region Fields
        public string Text;
        public StiFontGeom Font;
        public bool IsRounded = false;  //Special flag which simulate drawing as for Rectangle
        public bool IsRotatedText = false;
        public object Brush;
        public object Location;
        public StiStringFormatGeom StringFormat;
        public float Angle;
        public bool Antialiasing;
        public int? MaximalWidth;
        public StiRotationMode? RotationMode;
        #endregion

        #region Properties
        public override StiGeomType Type
        {
            get
            {
                return StiGeomType.Text;
            }
        }
        #endregion

        public StiTextGeom(string text, StiFontGeom font, object brush, object location, StiStringFormatGeom stringFormat, bool isRotatedText)
            :
            this(text, font, brush, location, stringFormat, 0f, false, null, null, isRotatedText)
        {
        }

        public StiTextGeom(string text, StiFontGeom font, object brush, object location, StiStringFormatGeom stringFormat, float angle, bool antialiasing, bool isRotatedText)
            :
            this(text, font, brush, location, stringFormat, angle, antialiasing, null, null, isRotatedText)
        {

        }

        public StiTextGeom(string text, StiFontGeom font, object brush, object location, StiStringFormatGeom stringFormat, float angle, bool antialiasing, StiRotationMode? rotationMode, bool isRotatedText)
            :
            this(text, font, brush, location, stringFormat, angle, antialiasing, null, rotationMode, isRotatedText)
        {
        }

        public StiTextGeom(string text, StiFontGeom font, object brush, object location, StiStringFormatGeom stringFormat, float angle, bool antialiasing, int? maximalWidth, StiRotationMode? rotationMode, bool isRotatedText)
        {
            this.IsRotatedText = isRotatedText;
            this.Text = text;
            this.Font = font;
            this.Brush = brush;
            this.Location = location;
            this.StringFormat = stringFormat;
            this.Angle = angle;
            this.Antialiasing = antialiasing;
            this.MaximalWidth = maximalWidth;
            this.RotationMode = rotationMode;
        }
    }
}
