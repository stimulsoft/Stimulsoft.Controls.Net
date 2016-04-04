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
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Maps.Geoms
{
    public class StiMoveToMapGeom : StiMapGeom
    {
        #region Properties.override
        public override StiMapGeomType GeomType
        {
            get
            {
                return StiMapGeomType.MoveTo;
            }
        }
        #endregion

        #region Properties

        public double X;
        public double Y;

        #endregion

        #region Methods
        public override PointD GetLastPoint()
        {
            return new PointD(X, Y);
        }
        #endregion
    }
}