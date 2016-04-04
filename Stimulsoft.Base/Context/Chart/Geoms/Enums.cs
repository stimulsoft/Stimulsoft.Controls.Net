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
using System.Text;

namespace Stimulsoft.Base.Context
{
    #region StiGeomType
    public enum StiGeomType
    {
        None = 0,
        Border = 1,
        CachedShadow = 2,
        Curve = 3,
        Ellipse = 4,
        Font = 5,
        Line = 6,
        Lines = 7,
        Path = 8,
        Pen = 9,
        PopSmothingMode = 10,
        PopTextRenderingHint = 11,
        PopTransform = 12,
        PopClip = 13,
        PushClip = 14,
        PushRotateTransform = 15,
        PushSmothingMode = 16,
        PushSmothingModeToAntiAlias = 17,
        PushTextRenderingHint = 18,
        PushTextRenderingHintToAntiAlias = 19,
        PushTranslateTransform = 20,
        Segment = 21,
        Shadow = 22,
        Text = 23,
        StringFormat = 24,

        AnimationBar,
        AnimationBorder,
        AnimationColumn,
        AnimationEllipse,
        AnimationPath,
        AnimationPathElement,
        AnimationLines,
        AnimationCurve,
        AnimationLabel,
        AnimationShadow
    }
    #endregion

    #region StiPenAlignment
    public enum StiPenAlignment
    {
        Center,
        Inset,
        Outset,
        Left,
        Right
    }
    #endregion

    #region StiPenLineCap
    public enum StiPenLineCap
    {
        Flat,
	    Square,
	    Round,
	    Triangle,
	    NoAnchor,
	    SquareAnchor,
	    RoundAnchor,
	    DiamondAnchor,
	    ArrowAnchor
    }
    #endregion
}
