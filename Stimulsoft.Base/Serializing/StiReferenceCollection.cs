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
using System.Reflection;

namespace Stimulsoft.Base.Serializing
{
	/// <summary>
	/// Describes a collection of delayed references.
	/// </summary>
	public class StiReferenceCollection : CollectionBase
	{
		#region Collection
		public void Add(StiPropertyInfo propInfo)
		{
			List.Add(new StiReference(propInfo));
		}

		public void Add(StiPropertyInfo propInfo, object value, PropertyInfo propertyInfo)
		{
			List.Add(new StiReference(propInfo, value, propertyInfo));
		}

		public void Add(StiReference reference)
		{
			List.Add(reference);
		}

		public void AddRange(StiReference[] reference)
		{
			base.InnerList.AddRange(reference);
		}

		public void AddRange(StiReferenceCollection references)
		{
			base.InnerList.AddRange(references.Items);
		}

		public bool Contains(StiReference reference)
		{
			return List.Contains(reference);
		}
		
		public int IndexOf(StiReference reference)
		{
			return List.IndexOf(reference);
		}

		public void Insert(int index, StiReference reference)
		{
			List.Insert(index, reference);
		}

		public void Remove(StiReference reference)
		{
			List.Remove(reference);
		}

		public StiReference this[int index]
		{
			get
			{
				return (StiReference)List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		public StiReference[] Items
		{
			get
			{
				return (StiReference[])InnerList.ToArray(typeof(StiReference));
			}
		}
		#endregion
	}
}
