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
using System.Reflection;
using System.ComponentModel.Design.Serialization;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Localization
{
	internal class InvariantComparer : IComparer
	{
		static InvariantComparer()
		{
			InvariantComparer.Default = new InvariantComparer();

		}

		internal InvariantComparer()
		{
			this.m_compareInfo = CultureInfo.InvariantCulture.CompareInfo;

		}

		public int Compare(object a, object b)
		{
			string strA = a as string;
			string strB = b as string;
			if ((strA != null) && (strB != null))
			{
				return this.m_compareInfo.Compare(strA, strB); 
			}
			return Comparer.Default.Compare(a, b);
		}

		internal static readonly InvariantComparer Default;

		private CompareInfo m_compareInfo;
 
	}
}
