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

namespace Stimulsoft.Controls
{
	public class StiStringCollection : CollectionBase, ICloneable
	{
		#region Collection
		public void Add(string str)
		{
			lock(List)List.Add(str);
		}

		public void AddRange(string[] strs)
		{
			lock(InnerList)base.InnerList.AddRange(strs);
		}

		public void AddRange(StiStringCollection strs)
		{
			foreach (string str in strs)this.Add(str);
				
		}

		public bool Contains(string str)
		{
			lock(List)return List.Contains(str);
		}
		
		public int IndexOf(string str)
		{
			lock(List)return List.IndexOf(str);
		}

		public void Insert(int index, string str)
		{
			lock(List)List.Insert(index, str);
		}

		public void Remove(string str)
		{
			lock(List)List.Remove(str);
		}
		public string this[int index]
		{
			get
			{
				lock(List)return (string)List[index];
			}
			set
			{
				lock(List)List[index] = value;
			}
		}

		public string this[string name]
		{
			get
			{
				lock(List)
				{
					foreach (string str in List)
						if (str == name)return str;
					return null;
				}
			}
		}

		public string[] Items
		{
			get
			{
				lock(InnerList)return (string[])InnerList.ToArray(typeof(string));
			}
		}


		#endregion

		#region ICloneable
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			lock(List)
			{
				StiStringCollection strs = new StiStringCollection();
				foreach (string str in List)strs.Add((string)str.Clone());
				return strs;
			}
		}
		#endregion
	}
}