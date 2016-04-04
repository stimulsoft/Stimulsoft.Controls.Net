#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports.Net											}
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
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Gauge;

namespace Stimulsoft.Base.Gauge.GaugeGeoms
{
    public class StiPushMatrixGaugeGeom : StiGaugeGeom
    {
        #region Fields
        public readonly float Angle;
        public readonly PointF CenterPoint;
        #endregion

        #region Properties
        public override StiGaugeGeomType Type
        {
            get 
            {
                return StiGaugeGeomType.PushMatrix;
            }
        }
        #endregion

        public StiPushMatrixGaugeGeom(float angle, PointF centerPoint)
        {
            this.Angle = angle;
            this.CenterPoint = centerPoint;
        }
    }
}