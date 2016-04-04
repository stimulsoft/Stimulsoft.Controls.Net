#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports												}
{	                         										}
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

namespace Stimulsoft.Base.Context.Animation
{
    public class StiScaleAnimation : StiAnimation
    {
        public StiScaleAnimation(double StartScaleX, double EndScaleX, double StartScaleY, double EndScaleY, double centerX, double centerY, TimeSpan duration, TimeSpan? beginTime) : base(duration, beginTime)
        {
            this.CenterX = centerX;
            this.CenterY = centerY;

            this.StartScaleX = StartScaleX;
            this.StartScaleY = StartScaleY;

            this.EndScaleX = EndScaleX;
            this.EndScaleY = EndScaleY;
        }

        #region Fields
        public double StartScaleX;
        public double StartScaleY;

        public double EndScaleX;
        public double EndScaleY;

        public double CenterX;
        public double CenterY;
        #endregion
    }
}
