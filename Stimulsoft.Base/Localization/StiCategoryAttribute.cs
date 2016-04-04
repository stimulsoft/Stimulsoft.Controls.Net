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
using System.ComponentModel;
using Stimulsoft.Base.Services;

namespace Stimulsoft.Base.Localization
{
	/// <summary>
	/// Specifies the name of the category in which to group the property or event when displayed in a StiPropertyGrid control.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = true)]
	public sealed class StiCategoryAttribute : Attribute
	{
		private string category;
		/// <summary>
		/// Gets or sets the name of the category for the property or event that this attribute is applied to.
		/// </summary>
		public string Category
		{
			get
			{
				return category;
			}
			set
			{
				category = value; 
			}
		}


		/// <summary>
		/// Initializes a new instance of the StiCategoryAttribute class using the category name Default.
		/// </summary>
		/// <param name="category">The name of the category for the property or event that this attribute is applied to</param>
		public StiCategoryAttribute(string category)
		{
			this.category = category;
		}
	}
}
