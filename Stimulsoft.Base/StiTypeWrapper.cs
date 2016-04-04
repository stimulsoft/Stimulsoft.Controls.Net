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
using System.Drawing;
using System.Collections;

namespace Stimulsoft.Base
{
	/// <summary>
	/// Class describes a wrapper for the type.
	/// </summary>
	public class StiTypeWrapper
	{
		private Type type;
		/// <summary>
		/// Gets or sets the Type for which is described wrapper.
		/// </summary>
		public Type Type
		{
			get
			{
				return type;
			}
		}
			

		public override string ToString()
		{
			return ToString(type);
		}

		public static string ToString(Type type)
		{
            if (type == null) return "null";
			if (type == typeof(bool)) return "bool";
			if (type == typeof(byte)) return "byte";
			if (type == typeof(byte[])) return "byte[]";
			if (type == typeof(char)) return "char";
			if (type == typeof(DateTime)) return "datetime";
			if (type == typeof(decimal)) return "decimal";
			if (type == typeof(double)) return "double";
			if (type == typeof(Guid)) return "guid";
			if (type == typeof(short)) return "short";
			if (type == typeof(int)) return "int";
			if (type == typeof(long)) return "long";
			if (type == typeof(sbyte)) return "sbyte";
			if (type == typeof(float)) return "float";
			if (type == typeof(string)) return "string";
			if (type == typeof(TimeSpan)) return "timespan";
			if (type == typeof(ushort)) return "ushort";
			if (type == typeof(uint)) return "uint";
			if (type == typeof(ulong)) return "ulong";            
			if (type == typeof(Image)) return "image";
            
            if (type == typeof(bool?)) return "bool (Nullable)";
            if (type == typeof(byte?)) return "byte (Nullable)";
            if (type == typeof(char?)) return "char (Nullable)";
            if (type == typeof(DateTime?)) return "datetime (Nullable)";
            if (type == typeof(decimal?)) return "decimal (Nullable)";
            if (type == typeof(double?)) return "double (Nullable)";
			if (type == typeof(Guid?)) return "guid (Nullable)";
            if (type == typeof(short?)) return "short (Nullable)";
            if (type == typeof(int?)) return "int (Nullable)";
            if (type == typeof(long?)) return "long (Nullable)";
            if (type == typeof(sbyte?)) return "sbyte (Nullable)";
            if (type == typeof(float?)) return "float (Nullable)";
            if (type == typeof(TimeSpan?)) return "timespan (Nullable)";
            if (type == typeof(ushort?)) return "ushort (Nullable)";
            if (type == typeof(uint?)) return "uint (Nullable)";
            if (type == typeof(ulong?)) return "ulong (Nullable)";

			if (type == typeof(object)) return "object";

			return type.ToString();
		}

		
		public static List<Type> SimpleTypes = null;
        public static List<Type> SimpleBaseTypes = null;

		
		/// <summary>
		/// Gets the array of simple types.
		/// </summary>
		/// <returns>Array of simple types.</returns>
		public static StiTypeWrapper[] GetTypeWrappers()
		{
			var wrappers = new StiTypeWrapper[SimpleTypes.Count];
			
			int index = 0;
			foreach (Type type in SimpleTypes)
			{
				wrappers[index++] = new StiTypeWrapper(type);
			}
			return wrappers;
		}


		/// <summary>
		/// Creates a new instance of the StiTypeWrapper class.
		/// </summary>
		/// <param name="type">Type for which wrapper is described.</param>
		public StiTypeWrapper(Type type)
		{
			this.type = type;
		}

		static StiTypeWrapper()
        {
            #region SimpleTypes
            SimpleTypes = new List<Type>();

            SimpleTypes.Add(typeof(string));
            SimpleTypes.Add(typeof(float));
            SimpleTypes.Add(typeof(double));
            SimpleTypes.Add(typeof(decimal));
            SimpleTypes.Add(typeof(DateTime));
            SimpleTypes.Add(typeof(TimeSpan));            
            SimpleTypes.Add(typeof(sbyte));
            SimpleTypes.Add(typeof(byte));
            SimpleTypes.Add(typeof(byte[]));
            SimpleTypes.Add(typeof(short));
            SimpleTypes.Add(typeof(ushort));
            SimpleTypes.Add(typeof(int));
            SimpleTypes.Add(typeof(uint));
            SimpleTypes.Add(typeof(long));
            SimpleTypes.Add(typeof(ulong));
            SimpleTypes.Add(typeof(bool));
            SimpleTypes.Add(typeof(char));
            SimpleTypes.Add(typeof(Guid));
            SimpleTypes.Add(typeof(object));
            SimpleTypes.Add(typeof(Image));

            SimpleTypes.Add(typeof(float?));
            SimpleTypes.Add(typeof(double?));
            SimpleTypes.Add(typeof(decimal?));
            SimpleTypes.Add(typeof(DateTime?));
            SimpleTypes.Add(typeof(TimeSpan?));            
            SimpleTypes.Add(typeof(sbyte?));
            SimpleTypes.Add(typeof(byte?));
            SimpleTypes.Add(typeof(short?));
            SimpleTypes.Add(typeof(ushort?));
            SimpleTypes.Add(typeof(int?));
            SimpleTypes.Add(typeof(uint?));
            SimpleTypes.Add(typeof(long?));
            SimpleTypes.Add(typeof(ulong?));
            SimpleTypes.Add(typeof(bool?));
            SimpleTypes.Add(typeof(char?));
            SimpleTypes.Add(typeof(Guid?));
            #endregion

            #region SimpleBaseTypes
            SimpleBaseTypes = new List<Type>();

            SimpleBaseTypes.Add(typeof(string));
            SimpleBaseTypes.Add(typeof(float));
            SimpleBaseTypes.Add(typeof(double));
            SimpleBaseTypes.Add(typeof(decimal));
            SimpleBaseTypes.Add(typeof(DateTime));
            SimpleBaseTypes.Add(typeof(TimeSpan));            
            SimpleBaseTypes.Add(typeof(sbyte));
            SimpleBaseTypes.Add(typeof(byte));
            SimpleBaseTypes.Add(typeof(short));
            SimpleBaseTypes.Add(typeof(ushort));
            SimpleBaseTypes.Add(typeof(int));
            SimpleBaseTypes.Add(typeof(uint));
            SimpleBaseTypes.Add(typeof(long));
            SimpleBaseTypes.Add(typeof(ulong));
            SimpleBaseTypes.Add(typeof(bool));
            SimpleBaseTypes.Add(typeof(char));
            SimpleBaseTypes.Add(typeof(Guid));
            SimpleBaseTypes.Add(typeof(object));
            SimpleBaseTypes.Add(typeof(Image));
            #endregion
        }
	}
}
