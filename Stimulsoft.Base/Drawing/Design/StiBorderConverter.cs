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
	/// Converts a StiBorder object from one data type to another.
	/// </summary>
	public class StiBorderConverter : TypeConverter
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
			if (destinationType == typeof(string) && value is StiBorder)
			{
				if (value == null)return "null";
				else 
				{
					StiEnumConverter enc = new StiEnumConverter(typeof(StiBorderSides));					

					return enc.ConvertTo(context, culture, ((StiBorder)value).Side, typeof(string)) as string;
				}
			}

			if (destinationType == typeof(InstanceDescriptor) && value != null)
            {
                #region StiAdvancedBorder
                if (value is StiAdvancedBorder)
                {
                    StiAdvancedBorder border = (StiAdvancedBorder)value;


                    Type[] types = new Type[]{	
											typeof(Color),
											typeof(double),
											typeof(StiPenStyle),

                                            typeof(Color),
											typeof(double),
											typeof(StiPenStyle),

                                            typeof(Color),
											typeof(double),
											typeof(StiPenStyle),

                                            typeof(Color),
											typeof(double),
											typeof(StiPenStyle),

											typeof(bool),
											typeof(double),
											typeof(StiBrush),
                                            typeof(bool)
											};

                    ConstructorInfo info = typeof(StiAdvancedBorder).GetConstructor(types);
                    if (info != null)
                    {
                        object[] objs = new object[]	{	
														border.TopSide.Color,
														border.TopSide.Size, 
														border.TopSide.Style,
                                                        
                                                        border.BottomSide.Color,
														border.BottomSide.Size, 
														border.BottomSide.Style,

                                                        border.LeftSide.Color,
														border.LeftSide.Size, 
														border.LeftSide.Style,

                                                        border.RightSide.Color,
														border.RightSide.Size, 
														border.RightSide.Style,

														border.DropShadow,
														border.ShadowSize,
														border.ShadowBrush,
                                                        border.Topmost
														};

                        return CreateNewInstanceDescriptor(info, objs);
                    }
                }
                #endregion

                #region StiBorder
                else
                {
                    StiBorder border = (StiBorder)value;


                    Type[] types = new Type[]{	
											    typeof(StiBorderSides),
											    typeof(Color),
											    typeof(double),
											    typeof(StiPenStyle),
											    typeof(bool),
											    typeof(double),
											    typeof(StiBrush),
                                                typeof(bool)
											};

                    ConstructorInfo info = typeof(StiBorder).GetConstructor(types);
                    if (info != null)
                    {
                        object[] objs = new object[]	{	
														    border.Side,
														    border.Color, 
														    border.Size,
														    border.Style,
														    border.DropShadow,
														    border.ShadowSize,
														    border.ShadowBrush,
                                                            border.Topmost
														};

                        return CreateNewInstanceDescriptor(info, objs);
                    }
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
