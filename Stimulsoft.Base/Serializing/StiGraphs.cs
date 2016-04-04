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

namespace Stimulsoft.Base.Serializing
{
	/// <summary>
	/// Describes the collection of graphs.
	/// </summary>
	public class StiGraphs
	{
		#region Collection
		public void Add(object obj)
		{
			if (obj != null && hashObj[obj] == null)
			{
				hashCode.Add(hashCode.Count, obj);
				hashObj.Add(obj, hashObj.Count);
			}
		}

		public void Add(object obj, int index)
		{
			if (obj != null && hashObj[obj] == null)
			{
				hashCode.Add(index, obj);
				hashObj.Add(obj, index);
			}
		}

		public object this[int code]
		{
			get
			{
				return hashCode[code];
			}
			set
			{
				hashCode[code] = value;
			}
		}

		public int this[object obj]
		{
			get
			{
				object code = hashObj[obj];
				if (code == null)return -1;
				return (int)code;
			}
			set
			{
				hashCode[obj] = value;
			}
		}

		private Hashtable hashCode = new Hashtable();
		private Hashtable hashObj = new Hashtable();
		#endregion
	}
}
