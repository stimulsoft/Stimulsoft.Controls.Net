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
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context
{
    public class StiShadowAnimationGeom : StiAnimationGeom
    {
        #region IStiJsonReportObject.override
        public override JObject SaveToJsonObject(StiJsonSaveMode mode)
        {
            var jObject = base.SaveToJsonObject(mode);

            jObject.Add(new JProperty("Rect", SaveRectangleFToJsonObject(Rect)));
            jObject.Add(new JProperty("RadiusX", RadiusX));
            jObject.Add(new JProperty("RadiusY", RadiusY));
            jObject.Add(new JProperty("ShadowWidth", ShadowWidth));

            return jObject;
        }

        public override void LoadFromJsonObject(JObject jObject)
        {
        }
        #endregion

        #region Fields
        public RectangleF Rect;
        public double RadiusX;
        public double RadiusY;
        public int ShadowWidth;
        #endregion

        #region Properties override
        public override StiGeomType Type
        {
            get
            {
                return StiGeomType.AnimationShadow;
            }
        }
        #endregion
        
        public StiShadowAnimationGeom(RectangleF rect, int shadowWidth, TimeSpan duration, TimeSpan? beginTime) : base(duration, beginTime)
        {
            this.Rect = rect;
            this.RadiusX = 0;
            this.RadiusY = 0;
            this.ShadowWidth = shadowWidth;
        }

        public StiShadowAnimationGeom(RectangleF rect, double radiusX, double radiusY, int shadowWidth, TimeSpan duration, TimeSpan? beginTime)
            : base(duration, beginTime)
        {
            this.Rect = rect;
            this.RadiusX = radiusX;
            this.RadiusY = radiusY;
            this.ShadowWidth = shadowWidth;
        }
    }
}
