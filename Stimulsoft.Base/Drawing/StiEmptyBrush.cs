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
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Class describes Empty Brush.
	/// </summary>
	public class StiEmptyBrush : StiBrush
	{
        #region IEquatable
	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        if (obj.GetType() != this.GetType()) return false;
	        return true;
	    }

	    public override int GetHashCode()
	    {
            return defaultHashCode;
	    }
        private static int defaultHashCode = "StiEmptyBrush".GetHashCode();
	    #endregion

		#region Methods
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        //public override bool Equals(Object obj) 
        //{
        //    StiEmptyBrush brush = obj as StiEmptyBrush;

        //    if (brush == null)return false;

        //    return true;
        //}
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of the StiEmptyBrush class.
		/// </summary>
		public StiEmptyBrush()
		{
		}
		#endregion
	}
}
