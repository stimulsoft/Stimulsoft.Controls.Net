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

using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context
{
    public class StiPenGeom : StiGeom
    {
        #region IStiJsonReportObject.override
        public override JObject SaveToJsonObject(StiJsonSaveMode mode)
        {
            var jObject = base.SaveToJsonObject(mode);

            if (Brush != null) jObject.Add(new JProperty("Brush", SaveBrushToJsonObject(Brush, mode)));
            jObject.Add(new JProperty("Thickness", Thickness));
            jObject.Add(new JProperty("PenStyle", PenStyle.ToString()));
            jObject.Add(new JProperty("Alignment", Alignment.ToString()));
            jObject.Add(new JProperty("StartCap", StartCap.ToString()));
            jObject.Add(new JProperty("EndCap", EndCap.ToString()));

            return jObject;
        }

        public override void LoadFromJsonObject(JObject jObject)
        {
        }
        #endregion

        #region Fields
        public object Brush;
        public float Thickness = 1f;
        public StiPenStyle PenStyle = StiPenStyle.Solid;
        public StiPenAlignment Alignment = StiPenAlignment.Center;
        public StiPenLineCap StartCap = StiPenLineCap.Flat;
        public StiPenLineCap EndCap = StiPenLineCap.Flat;
        #endregion

        #region Properties
        public override StiGeomType Type
        {
            get
            {
                return StiGeomType.Pen;
            }
        }
        #endregion

        public StiPenGeom(object brush)
            :
            this(brush, 1f)
        {
        }

        public StiPenGeom(object brush, float thickness)
        {
            this.Brush = brush;
            this.Thickness = thickness;
        }
    }
}
