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
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Drawing.Text;
using System.Drawing;
using System.Reflection;

namespace Stimulsoft.Base.Drawing.Design
{
	/// <summary>
	/// Converts a StiTextOptions object from one data type to another.
	/// </summary>
	public class StiTextOptionsConverter : TypeConverter
	{
		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, 
			object value, Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(value, attributes); 
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

					StiTextOptions op = (StiTextOptions)value;
					//return StiTextOptionsHelper.ConvertTextOptionsToString(op, ch);
                    return StiTextOptionsHelper.ConvertTextOptionsToLocalizedString(op, ch, false);
				}
			}
			if (destinationType == typeof(InstanceDescriptor) && value != null)
			{
				StiTextOptions options = (StiTextOptions)value;

				if (options.FirstTabOffset == 40f && options.DistanceBetweenTabs == 20f)
				{

					Type[] types = new Type[]{	
												 typeof(bool),
												 typeof(bool),
												 typeof(bool),
												 typeof(float),
												 typeof(HotkeyPrefix),
												 typeof(StringTrimming)};

					ConstructorInfo info = typeof(StiTextOptions).GetConstructor(types);
					if (info != null)
					{
						object[] objs = new object[]	{	options.RightToLeft,
															options.LineLimit, 
															options.WordWrap,
															options.Angle,
															options.HotkeyPrefix,
															options.Trimming
														};

                        return CreateNewInstanceDescriptor(info, objs);
					}
				}
				else
				{
					Type[] types = new Type[]{	
												 typeof(bool),
												 typeof(bool),
												 typeof(bool),
												 typeof(float),
												 typeof(HotkeyPrefix),
												 typeof(StringTrimming),
												 typeof(float),
												 typeof(float)};

					ConstructorInfo info = typeof(StiTextOptions).GetConstructor(types);
					if (info != null)
					{
						object[] objs = new object[]	{	options.RightToLeft,
															options.LineLimit, 
															options.WordWrap,
															options.Angle,
															options.HotkeyPrefix,
															options.Trimming,
															options.FirstTabOffset,
															options.DistanceBetweenTabs
														};

                        return CreateNewInstanceDescriptor(info, objs);
					}
				}
					
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))return true; 
			return base.CanConvertFrom(context, sourceType); 
		} 


		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string)) return true; 
			if (destinationType == typeof(InstanceDescriptor)) return true; 
			return base.CanConvertTo(context, destinationType); 
		}

		
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string text = value as string;
				char ch;
				if (culture == null)ch = CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];
				else ch = culture.TextInfo.ListSeparator[0];

				return StiTextOptionsHelper.ConvertStringToTextOptions(text, ch);
			}
			return base.ConvertFrom(context, culture, value); 
		}

        private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
        {
            return new InstanceDescriptor(info, objs);
        }
	}
}
