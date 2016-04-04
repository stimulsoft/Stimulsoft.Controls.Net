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
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context
{
    public class StiPieSegmentGeom : StiSegmentGeom
    {
        #region IStiJsonReportObject.override
        public override JObject SaveToJsonObject(StiJsonSaveMode mode)
        {
            var jObject = base.SaveToJsonObject(mode);

            jObject.Add(new JProperty("Rect", SaveRectangleFToJsonObject(Rect)));
            jObject.Add(new JProperty("StartAngle", StartAngle));
            jObject.Add(new JProperty("SweepAngle", SweepAngle));

            return jObject;
        }

        public override void LoadFromJsonObject(JObject jObject)
        {
        }
        #endregion

        #region Fields
        public RectangleF Rect;
        public float StartAngle;
        public float SweepAngle;
        #endregion

        public StiPieSegmentGeom(RectangleF rect, float startAngle, float sweepAngle)
        {
            this.Rect = rect;
            this.StartAngle = startAngle;
            this.SweepAngle = sweepAngle;
        }
    }
}
