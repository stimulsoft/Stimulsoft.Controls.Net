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
using System.Drawing.Text;
using System.Collections.Generic;
using System.Text;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context
{
    public class StiStringFormatGeom : StiGeom
    {
        #region IStiJsonReportObject.override
        public override JObject SaveToJsonObject(StiJsonSaveMode mode)
        {
            var jObject = base.SaveToJsonObject(mode);

            jObject.Add(new JProperty("IsGeneric", IsGeneric));
            jObject.Add(new JProperty("Alignment", Alignment.ToString()));
            jObject.Add(new JProperty("FormatFlags", FormatFlags.ToString()));
            jObject.Add(new JProperty("HotkeyPrefix", HotkeyPrefix.ToString()));
            jObject.Add(new JProperty("LineAlignment", LineAlignment.ToString()));
            jObject.Add(new JProperty("Trimming", Trimming.ToString()));

            return jObject;
        }

        public override void LoadFromJsonObject(JObject jObject)
        {
        }
        #endregion

        #region Fields
        public bool IsGeneric = false;
        public StringAlignment Alignment = StringAlignment.Near;
        public StringFormatFlags FormatFlags = (StringFormatFlags)0;
        public HotkeyPrefix HotkeyPrefix = HotkeyPrefix.None;
        public StringAlignment LineAlignment = StringAlignment.Near;
        public StringTrimming Trimming = StringTrimming.None;
        #endregion

        #region Properties
        public override StiGeomType Type
        {
            get
            {
                return StiGeomType.StringFormat;
            }
        }
        #endregion

        public StiStringFormatGeom(StringFormat sf)
        {
            this.Alignment = sf.Alignment;
            this.FormatFlags = sf.FormatFlags;
            this.HotkeyPrefix = sf.HotkeyPrefix;
            this.LineAlignment = sf.LineAlignment;
            this.Trimming = sf.Trimming;
        }
    }
}
