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
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Context
{
    public abstract class StiGeom : IStiJsonReportObject
    {
        #region IStiJsonReportObject
        public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
        {
            var jObject = new JObject();

            jObject.Add(new JProperty("Ident", this.GetType().Name));
            jObject.Add(new JProperty("Type", Type.ToString()));

            return jObject;
        }

        protected JObject[] SaveGeomListToJsonObject(List<StiSegmentGeom> geoms, StiJsonSaveMode mode)
        {
            if (geoms != null)
            {
                var jObjectGeoms = new JObject[geoms.Count];
                for (int index = 0; index < geoms.Count; index++)
                {
                    jObjectGeoms[index] = geoms[index].SaveToJsonObject(mode);
                }
                return jObjectGeoms;
            }
            return null;
        }

        protected JObject[] SavePointFArrayToJsonObject(PointF[] points)
        {
            if (points != null)
            {
                var jObjectPoints = new JObject[points.Length];
                for (int index = 0; index < points.Length; index++)
                {
                    jObjectPoints[index] = SavePointFToJsonObject(points[index]);
                }
                return jObjectPoints;
            }
            return null;
        }

        protected string SaveBrushToJsonObject(object brush, StiJsonSaveMode mode)
        {
            if (brush is Color)
            {
                var color = (Color)brush;
                return string.Format("Color,{0},{1},{2},{3}", color.A, color.R, color.G, color.B);
            }

            if (brush is StiBrush)
            {
                return StiJsonReportObjectHelper.Serialize.JBrush((StiBrush)brush);
            }
            return null;
        }

        protected JObject SaveRectToJsonObject(object rect)
        {
            if (rect is Rectangle)
                return SaveRectangleToJsonObject((Rectangle)rect);
            if (rect is RectangleF)
                return SaveRectangleFToJsonObject((RectangleF)rect);
            if (rect is RectangleD)
                return SaveRectangleDToJsonObject((RectangleD)rect);
            return null;
        }

        public static JObject SavePointFToJsonObject(System.Drawing.PointF pos)
        {
            var jObject = new JObject();

            jObject.Add(new JProperty("X", pos.X));
            jObject.Add(new JProperty("Y", pos.Y));

            return jObject;
        }

        public static JObject SaveRectangleToJsonObject(Rectangle rect)
        {
            var jObject = new JObject();

            jObject.Add(new JProperty("X", rect.X));
            jObject.Add(new JProperty("Y", rect.Y));
            jObject.Add(new JProperty("Width", rect.Width));
            jObject.Add(new JProperty("Height", rect.Height));

            return jObject;
        }

        public static JObject SaveRectangleFToJsonObject(RectangleF rect)
        {
            var jObject = new JObject();

            jObject.Add(new JProperty("X", rect.X));
            jObject.Add(new JProperty("Y", rect.Y));
            jObject.Add(new JProperty("Width", rect.Width));
            jObject.Add(new JProperty("Height", rect.Height));

            return jObject;
        }

        public static JObject SaveRectangleDToJsonObject(RectangleD rect)
        {
            var jObject = new JObject();

            jObject.Add(new JProperty("X", rect.X));
            jObject.Add(new JProperty("Y", rect.Y));
            jObject.Add(new JProperty("Width", rect.Width));
            jObject.Add(new JProperty("Height", rect.Height));

            return jObject;
        }

        public virtual void LoadFromJsonObject(JObject jObject)
        {
        }
        #endregion

        public abstract StiGeomType Type
        {
            get;
        }
    }
}
