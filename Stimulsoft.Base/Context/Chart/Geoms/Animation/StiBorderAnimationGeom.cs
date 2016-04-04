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
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Context
{
    public class StiBorderAnimationGeom : StiAnimationGeom
    {
        #region IStiJsonReportObject.override
        public override JObject SaveToJsonObject(StiJsonSaveMode mode)
        {
            var jObject = base.SaveToJsonObject(mode);

            if (Background != null) jObject.Add(new JProperty("Background", SaveBrushToJsonObject(Background, mode)));
            if (BorderPen != null) jObject.Add(new JProperty("BorderPen", BorderPen.SaveToJsonObject(mode)));
            jObject.Add(new JProperty("Rect", SaveRectToJsonObject(Rect)));
            jObject.Add(new JProperty("AnimationType", AnimationType.ToString()));

            return jObject;
        }

        public override void LoadFromJsonObject(JObject jObject)
        {
        }
        #endregion

        #region Fields
        public object Background;
        public StiPenGeom BorderPen;
        public object Rect;
        public StiAnimationType AnimationType;
        #endregion

        #region Properties
        public override StiGeomType Type
        {
            get
            {
                return StiGeomType.AnimationBorder;
            }
        }
        #endregion

        public StiBorderAnimationGeom(object background, StiPenGeom borderPen, object rect, TimeSpan duration, TimeSpan? beginTime, StiAnimationType animationType) : 
            base(duration, beginTime)
        {
            this.Background = background;
            this.BorderPen = borderPen;
            this.Rect = rect;

            this.AnimationType = animationType;
        }
    }
}
