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

namespace Stimulsoft.Base
{
	/// <summary>
	/// Describes a collection of object properties.
	/// </summary>
	public class StiNameTypeCollection : CollectionBase 
	{
		#region Collection
		public void Add(StiNameTypeEntry entry)
		{
			List.Add(entry);
		}

		public void Add(string name, Type type)
		{
			Add(new StiNameTypeEntry(name, type));
		}


		public void Insert(int index, StiNameTypeEntry entry)
		{
			List.Insert(index, entry);
		}

		public void Insert(int index, string name, Type type)
		{
			List.Insert(index, new StiNameTypeEntry(name, type));
		}


		public void Remove(string name)
		{
			int index = 0;
			while (index < List.Count)
			{
				if (this[index].Name == name)
				{
					List.RemoveAt(index);
				}
                else index++;
			}
		}

		public StiNameTypeEntry this[int index]
		{
			get
			{
				return (StiNameTypeEntry)List[index];
			}
			set
			{
				List[index] = value;
			}
		}
		#endregion
	}
}
