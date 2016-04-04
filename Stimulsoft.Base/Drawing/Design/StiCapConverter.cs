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
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing.Design
{
    /// <summary>
    /// Converts a StiCap object from one data type to another.
    /// </summary>
    public class StiCapConverter : TypeConverter
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


        public override bool CanConvertTo(ITypeDescriptorContext context,
            Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor)) return true;
            if (destinationType == typeof(string)) return true;
            return base.CanConvertTo(context, destinationType);
        }


        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,
            object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value == null) return "null";
                else
                {
                    StiCap cap = value as StiCap;
                    return StiLocalization.Get("PropertyEnum", typeof(StiCapStyle).Name + Enum.GetName(typeof(StiCapStyle), cap.Style), false);
                }
            }

            if (destinationType == typeof(InstanceDescriptor) && value != null)
            {
                #region StiCap
                StiCap cap = (StiCap)value;


                Type[] types = new Type[]{	
										typeof(int),
										typeof(StiCapStyle),
										typeof(int),
                                        typeof(bool),
                                        typeof(Color)
										};

                ConstructorInfo info = typeof(StiCap).GetConstructor(types);
                if (info != null)
                {
                    object[] objs = new object[]	{	
													cap.Width,
													cap.Style, 
													cap.Height,
                                                    cap.Fill,
                                                    cap.Color
													};

                    return CreateNewInstanceDescriptor(info, objs);
                }
                #endregion
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        private object CreateNewInstanceDescriptor(ConstructorInfo info, object[] objs)
        {
            return new InstanceDescriptor(info, objs);
        }
    }
}
