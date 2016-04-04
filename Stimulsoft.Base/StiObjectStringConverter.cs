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
using System.ComponentModel;
using System.Globalization;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace Stimulsoft.Base
{
	/// <summary>
	/// Helps a converts object to string and a converts string to object.
	/// </summary>
	public class StiObjectStringConverter
	{
		public virtual void SetProperty(PropertyInfo p, object parentObject, object obj)
		{
		}

		/// <summary>
		/// Converts object into the string.
		/// </summary>
		/// <param name="obj">Object for convertation.</param>
		/// <returns>String that represents object.</returns>
		public virtual string ObjectToString(object obj) 
		{
			if (obj is string)return (string)obj;
			if (obj is Type)return obj.ToString();
            var conv = TypeDescriptor.GetConverter(obj.GetType());
			return conv.ConvertToString(obj);			
		}


		/// <summary>
		/// Convertes string into object.
		/// </summary>
		/// <param name="str">String that represents object.</param>
		/// <param name="type">Object type.</param>
		/// <returns>Converted object.</returns>
		public virtual object StringToObject(string str, Type type) 
		{
			if (type == typeof(string))return str;
			else if (type == typeof(decimal))
			{
				return decimal.Parse(str);
			}
			else if (type == typeof(Type))
			{
				Type tp = StiTypeFinder.GetType(str);
				if (tp != null)return tp;

                var assemblys = AppDomain.CurrentDomain.GetAssemblies();
				foreach (Assembly assembly in assemblys)
				{
					tp = assembly.GetType(str);
					if (tp != null)return tp;
				}
				
				if (tp == null)
				{
					throw new TypeLoadException("Type \"" + str + "\" not found");
				}
				return null;
			}
			if (type == typeof(object))return str;

			TypeConverter converter = TypeDescriptor.GetConverter(type);
			return converter.ConvertFromString(str);
		}
	}
}
