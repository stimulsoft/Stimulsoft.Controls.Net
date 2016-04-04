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
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Base.Drawing
{    

    public sealed class StiPenUtils
    {
        public static DashStyle GetPenStyle(StiPenStyle penStyle)
        {
            switch (penStyle)
            {
                case StiPenStyle.Dash:
                    return DashStyle.Dash;

                case StiPenStyle.DashDot:
                    return DashStyle.DashDot;

                case StiPenStyle.DashDotDot:
                    return DashStyle.DashDotDot;

                case StiPenStyle.Dot:
                    return DashStyle.Dot;

				case StiPenStyle.Double:
					return DashStyle.Solid;

                default:
                    return DashStyle.Solid;
            }
        }

        public static LineCap GetLineCap(StiPenLineCap lineCap)
        {
            switch (lineCap)
            {
                case StiPenLineCap.ArrowAnchor:
                    return LineCap.ArrowAnchor;

                case StiPenLineCap.DiamondAnchor:
                    return LineCap.DiamondAnchor;

                case StiPenLineCap.Flat:
                    return LineCap.Flat;

                case StiPenLineCap.NoAnchor:
                    return LineCap.NoAnchor;

                case StiPenLineCap.Round:
                    return LineCap.Round;

                case StiPenLineCap.RoundAnchor:
                    return LineCap.RoundAnchor;

                case StiPenLineCap.Square:
                    return LineCap.Square;

                case StiPenLineCap.SquareAnchor:
                    return LineCap.SquareAnchor;

                case StiPenLineCap.Triangle:
                    return LineCap.Triangle;

                default:
                    return LineCap.Square;
            }
        }
    }
}
