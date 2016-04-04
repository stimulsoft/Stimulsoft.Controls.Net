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
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms.PropertyGridInternal;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Base.Design
{
	public class StiPropertyGridOptions
	{
		#region Properties
	    public static StiLevel? PropertyLevel { get; set; }


	    /// <summary>
		/// Gets or sets value, indicates - show StiPropertyGrid description or not.
		/// </summary>
		public static bool ShowDescription { get; set; }
	    

	    /// <summary>
		/// Gets or sets value indicates that the panel of properties is localizable.
		/// </summary>
		public static bool Localizable { get; set; }
	    #endregion

        static StiPropertyGridOptions()
        {
            Localizable = true;
            ShowDescription = true;
            PropertyLevel = null;
        }
	}
}