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
	/// Class keep an object properties information.
	/// </summary>
	public class StiPropertyInfo 
	{
		private StiPropertyInfo parent;
		/// <summary>
		/// Gets or sets an object property that is the main for this object property.
		/// </summary>
		public StiPropertyInfo Parent
		{
			get
			{
				return parent;
			}
			set
			{
				parent = value;
			}
		}


		private string name;
		/// <summary>
		/// Gets or sets the name of an object property.
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}


		private object valueObj;
		/// <summary>
		/// Gets or sets the value of an object property.
		/// </summary>
		public object Value
		{
			get
			{
				return valueObj;
			}
			set
			{
				valueObj = value;
			}
		}


		private object defaultValue;
		/// <summary>
		/// Gets or sets the value of an object property by default.
		/// </summary>
		public object DefaultValue
		{
			get
			{
				return defaultValue;
			}
			set
			{
				defaultValue = value;
			}
		}


		private bool isKey;		
		/// <summary>
		/// Gets or sets value indicates that this property describes  an object.
		/// </summary>
		public bool IsKey
		{
			get
			{
				return isKey;
			}
			set
			{
				isKey = value;
			}
		}


		private bool isReference;
		/// <summary>
		/// Gets or sets value indicates that this is a reference to an object.
		/// </summary>
		public bool IsReference
		{
			get
			{
				return isReference;
			}
			set
			{
				isReference = value;
			}
		}

		
		private bool isSerializable = false;
		/// <summary>
		/// Gets or sets value indicates that this object realizes the interface IStiSerializable.
		/// </summary>
		public bool IsSerializable
		{
			get
			{
				return isSerializable;
			}
			set
			{
				isSerializable = value;
			}
		}


		private bool isList = false;
		/// <summary>
		/// Gets or sets value indicates that this is a collection.
		/// </summary>
		public bool IsList
		{
			get
			{
				return isList;
			}
			set
			{
				isList = value;
			}
		}


		private int count;
		/// <summary>
		/// Gets or sets the number of elements in the collection.
		/// </summary>
		public int Count
		{
			get
			{
				return count;
			}
			set
			{
				count = value;
			}
		}


		private int referenceCode;
		/// <summary>
		/// Gets or sets the reference code.
		/// </summary>
		public int ReferenceCode
		{
			get
			{
				return referenceCode;
			}
			set
			{
				referenceCode = value;
			}
		}


		private string typeName;
		public string TypeName
		{
			get
			{
				return typeName;
			}
			set
			{
				typeName = value;
			}
		}


		/// <summary>
		/// Gets an object type.
		/// </summary>
		public Type Type
		{
			get
			{
				if (Value != null)return Value.GetType();
                return typeof(object);
			}
		}


		private StiPropertyInfoCollection properties;
		/// <summary>
		/// Gets or sets the collection of subordinated properties.
		/// </summary>
		public StiPropertyInfoCollection Properties
		{
			get
			{
				return properties;
			}
			set
			{
				properties = value;
			}
		}


		/// <summary>
		/// Creates a new instance of the StiPropertyInfo class.
		/// </summary>
		/// <param name="name">Name of an object properties</param>
		/// <param name="value">Value of an object property.</param>
		/// <param name="defaultValue">Value of an object property by default.</param>
		/// <param name="isKey">Value indicates that this property describes an object.</param>
		/// <param name="isReference">Value indicates that this is a reference to an object.</param>
		/// <param name="isList">Value indicates that this is a collection.</param>
		public StiPropertyInfo(string name, object value, object defaultValue, 
			bool isKey, bool isReference, bool isList, string typeName)
		{
			Properties = new StiPropertyInfoCollection(this);

			this.name = name;
			this.valueObj = value;
			this.defaultValue = defaultValue;
			this.isKey = isKey;
			this.isReference = isReference;
			this.isList = isList;
			this.referenceCode = -1;
			this.typeName = typeName;
		}


		/// <summary>
		/// Creates a new instance of the StiPropertyInfo class.
		/// </summary>
		/// <param name="name">Name of an object properties</param>
		/// <param name="value">Value of an object property.</param>
		/// <param name="isKey">Value indicates that this property describes an object.</param>
		/// <param name="isReference">Value indicates that this is a reference to an object.</param>
		/// <param name="isList">Value indicates that this is a collection.</param>
		public StiPropertyInfo(string name, object value, bool isKey, bool isReference, bool isList, string typeName) :
			this(name, value, null, isKey, isReference, isList, typeName)
		{
		}
	}
}
