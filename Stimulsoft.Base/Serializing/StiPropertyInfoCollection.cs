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
	/// Describes a collection of object properties.
	/// </summary>
	public class StiPropertyInfoCollection : CollectionBase
	{
		#region Collection
		public void Add(StiPropertyInfo propertyInfo)
		{
			propertyInfo.Parent = parent;
			List.Add(propertyInfo);
		}

		public void AddRange(StiPropertyInfo[] propertyInfos)
		{
			foreach (StiPropertyInfo propertyInfo in propertyInfos)Add(propertyInfo);
		}

		public void AddRange(StiPropertyInfoCollection propertyInfos)
		{
			foreach (StiPropertyInfo propertyInfo in propertyInfos)Add(propertyInfo);
		}

		public bool Contains(StiPropertyInfo propertyInfo)
		{
			return List.Contains(propertyInfo);
		}
		
		public int IndexOf(StiPropertyInfo propertyInfo)
		{
			return List.IndexOf(propertyInfo);
		}

		public void Insert(int index, StiPropertyInfo propertyInfo)
		{
			List.Insert(index, propertyInfo);
		}

		public void Remove(StiPropertyInfo propertyInfo)
		{
			List.Remove(propertyInfo);
		}

		public StiPropertyInfo this[int index]
		{
			get
			{
				return (StiPropertyInfo)List[index];
			}
			set
			{
				List[index] = value;
			}
		}

		public StiPropertyInfo[] Items
		{
			get
			{
				return (StiPropertyInfo[])InnerList.ToArray(typeof(StiPropertyInfo));
			}
		}
		#endregion

		private StiPropertyInfo parent;
		public StiPropertyInfoCollection() : this(null)
		{
		}

		public StiPropertyInfoCollection(StiPropertyInfo parentPropertyInfo)
		{
			this.parent = parentPropertyInfo;
		}
	}
}
