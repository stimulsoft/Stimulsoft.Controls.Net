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
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Services
{
	/// <summary>
	/// Class describes attribute which is used for assinging bitmap to service category.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
	public sealed class StiServiceCategoryBitmapAttribute: Attribute
	{
        /// <summary>
        /// Returns a path to image according to its type.
        /// </summary>
        /// <param name="type">Service type.</param>
        /// <returns>Path to Image.</returns>
        public static string GetImagePath(Type type)
        {
            StiServiceCategoryBitmapAttribute[] attributes = (StiServiceCategoryBitmapAttribute[])
                type.GetCustomAttributes(typeof(StiServiceCategoryBitmapAttribute), true);

            if (attributes.Length > 0)
            {
                StiServiceCategoryBitmapAttribute serviceBitmapAttribute =
                    (StiServiceCategoryBitmapAttribute)attributes[0];

                return serviceBitmapAttribute.BitmapName;
            }
            else return null;
        }

		/// <summary>
		/// Returns a bitmap of service category according to its type.
		/// </summary>
		/// <param name="type">Service type.</param>
		/// <returns>Image.</returns>
		public static Bitmap GetImage(Type type)
		{
			StiServiceCategoryBitmapAttribute[] attributes = (StiServiceCategoryBitmapAttribute[])
				type.GetCustomAttributes(typeof(StiServiceCategoryBitmapAttribute), true);
			
			if (attributes.Length > 0)
			{
				StiServiceCategoryBitmapAttribute serviceBitmapAttribute = 
					(StiServiceCategoryBitmapAttribute)attributes[0];
					
				return serviceBitmapAttribute.GetImage();
			}
			else return null;
		}


		/// <summary>
		/// Returns bitmap of this attribute.
		/// </summary>
		/// <returns>Image.</returns>
		public Bitmap GetImage()
		{
			return StiImageUtils.GetImage(type, bitmapName);
		}


		private string bitmapName;
		/// <summary>
		/// Gets or sets a path to a bitmap in resources.
		/// </summary>
		public string BitmapName
		{
			get
			{
				return bitmapName;
			}
			set
			{
				bitmapName = value;
			}
		}
		

		private Type type;
		/// <summary>
		/// Gets or sets service type to which a bitmap is assign to.
		/// </summary>
		public Type Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		}


		/// <summary>
		/// Creates a new attribute of the type StiServiceCategoryBitmapAttribute.
		/// </summary>
		/// <param name="type">Service type which categories assigning a bitmap.</param>
		/// <param name="bitmapName">Path to a bitmap in resources.</param>
		public StiServiceCategoryBitmapAttribute(Type type, string bitmapName)
		{
			this.type = type;
			this.bitmapName = bitmapName;
		}
	}
}
