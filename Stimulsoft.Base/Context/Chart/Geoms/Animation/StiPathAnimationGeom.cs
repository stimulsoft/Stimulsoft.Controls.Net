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
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Context
{
    public class StiPathAnimationGeom : StiAnimationGeom
    {
        #region IStiJsonReportObject.override
        public override JObject SaveToJsonObject(StiJsonSaveMode mode)
        {
            var jObject = base.SaveToJsonObject(mode);

            jObject.Add(new JProperty("Rect", SaveRectToJsonObject(Rect)));
            if (Background != null) jObject.Add(new JProperty("Background", SaveBrushToJsonObject(Background, mode)));
            if (Pen != null) jObject.Add(new JProperty("Pen", Pen.SaveToJsonObject(mode)));
            if (Geoms != null) jObject.Add(new JProperty("Geoms", SaveGeomListToJsonObject(Geoms, mode)));

            if (Tag is string) jObject.Add(new JProperty("Tag", Tag as string));    // !!!

            return jObject;
        }

        public override void LoadFromJsonObject(JObject jObject)
        {
        }
        #endregion

        public static object GetBoundsState = new object();

        #region Fields
        public object Background;
        public StiPenGeom Pen;
        public object Rect;
        public List<StiSegmentGeom> Geoms;
        public object Tag;
        #endregion

        #region Properties
        public override StiGeomType Type
        {
            get
            {
                return StiGeomType.AnimationPath;
            }
        }
        #endregion
        
        public StiPathAnimationGeom(object background, StiPenGeom pen, List<StiSegmentGeom> geoms, object rect, TimeSpan duration, TimeSpan? beginTime, object tag)
            : base(duration, beginTime)
        {
            this.Background = background;
            this.Pen = pen;
            this.Geoms = geoms;
            this.Rect = rect;
            this.Tag = tag;
        }
    }
}
