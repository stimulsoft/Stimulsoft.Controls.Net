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
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Drawing;
using Stimulsoft.Base;

namespace Stimulsoft.Base.Drawing.Design
{
	/// <summary>
	/// Converts a PointD object from one data type to another.
	/// </summary>
	public class PointDConverter : TypeConverter
	{
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, 
			object value, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(value, attributes); 
		} 


		public override bool CanConvertTo(ITypeDescriptorContext context, 
			Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))return true;
			if (destinationType == typeof(string))return true;
			return base.CanConvertTo(context, destinationType);
		}

	
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}


		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, 
			object value, Type destinationType) 
		{
			if (destinationType == typeof(string))
			{
				if (value == null)return "null";
				else 
				{
					char ch;
					if (culture == null)ch = CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];
					else ch = culture.TextInfo.ListSeparator[0];

					PointD point = (PointD)value;
					return point.X.ToString() + ch + point.Y.ToString();
				}
			}

			if (destinationType == typeof(InstanceDescriptor) && value != null)
			{
				PointD point = (PointD)value;
				

				Type[] types = new Type[]{	
											 typeof(double),
											 typeof(double)
										 };

				ConstructorInfo info = typeof(PointD).GetConstructor(types);
				if (info != null)
				{
					object[] objs = new object[]	{	
														point.X,
														point.Y
													};
					
					return CreateNewInstanceDescriptor(info, objs);
				}
					
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))return true; 
			return base.CanConvertFrom(context, sourceType); 
		} 

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string str = value as string;
				str = str.Trim().ToLower(System.Globalization.CultureInfo.InvariantCulture);
				char ch;
				if (culture == null)ch = CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];
				else ch = culture.TextInfo.ListSeparator[0];

				string[] words = str.Split(new char[]{ch});

                return new PointD(double.Parse(words[0]), double.Parse(words[1]));
            }
			return base.ConvertFrom(context, culture, value); 
		}

        private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
        {
            return new InstanceDescriptor(info, objs);
        }
	}
}
