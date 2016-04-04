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

namespace Stimulsoft.Base.Serializing
{
	/// <summary>
	/// Class for delayed processing references (references which allude to the unprocessed objects).
	/// </summary>
	public class StiReference
	{
		private StiPropertyInfo propInfo;
		/// <summary>
		/// Gets or sets the adapted property.
		/// </summary>
		public StiPropertyInfo PropInfo
		{
			get
			{
				return propInfo;
			}
			set
			{
				propInfo = value;
			}
		}
		
	
		private object obj;
		/// <summary>
		/// Gets or sets the object where a property is located.
		/// </summary>
		public object Object
		{
			get
			{
				return obj;
			}
			set
			{
				obj = value;
			}
		}

		
		private PropertyInfo propertyInfo;
		/// <summary>
		/// Gets or sets standard property.
		/// </summary>
		public PropertyInfo PropertyInfo
		{
			get
			{
				return propertyInfo;
			}
			set
			{
				propertyInfo = value;
			}
		}

		
		/// <summary>
		/// Creates a new instance of the StiReference class.
		/// </summary>
		/// <param name="propInfo">Adapted property.</param>
		public StiReference(StiPropertyInfo propInfo) : this(propInfo, null, null)
		{
		}

		/// <summary>
		/// Creates a new instance of the StiReference class.
		/// </summary>
		/// <param name="propInfo">Adapted property.</param>
		/// <param name="obj">The object where a property is located.</param>
		/// <param name="propertyInfo">Standard property.</param>
		public StiReference(StiPropertyInfo propInfo, object obj, PropertyInfo propertyInfo)
		{
			this.propInfo = propInfo;
			this.obj = obj;
			this.propertyInfo = propertyInfo;
		}
	}
}
