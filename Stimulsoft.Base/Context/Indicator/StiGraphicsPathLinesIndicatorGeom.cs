﻿#region Copyright (C) 2003-2016 Stimulsoft
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

using System.Drawing;

namespace Stimulsoft.Base.Indicator
{
    public class StiGraphicsPathLinesIndicatorGeom : StiIndicatorGeom
    {
        #region Fields
        public readonly PointF[] points;
        #endregion

        #region Properties
        public override StiIndicatorGeomType Type
        {
            get
            {
                return StiIndicatorGeomType.GraphicsPathLines;
            }
        }
        #endregion

        public StiGraphicsPathLinesIndicatorGeom(PointF[] points)
        {
            this.points = points;
        }
    }
}
