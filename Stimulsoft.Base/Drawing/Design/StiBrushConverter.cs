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
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing.Design
{
	/// <summary>
	/// Converts a StiBrush object from one data type to another.
	/// </summary>
	public class StiBrushConverter : StiOrderConverter
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
			if (destinationType == typeof(InstanceDescriptor))return true;
			if (destinationType == typeof(string))return true;
			return base.CanConvertTo(context, destinationType);
		}


		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, 
			object value, Type destinationType) 
		{
			if (destinationType == typeof(string))
			{
				if (value == null)return "null";
				else if (value is StiEmptyBrush)	return StiLocalization.Get("Report", "StiEmptyBrush");
				else if (value is StiSolidBrush)	return StiLocalization.Get("Report", "StiSolidBrush");
				else if (value is StiGradientBrush)	return StiLocalization.Get("Report", "StiGradientBrush");
				else if (value is StiHatchBrush)	return StiLocalization.Get("Report", "StiHatchBrush");
				else if (value is StiGlareBrush)	return StiLocalization.Get("Report", "StiGlareBrush");
                else if (value is StiGlassBrush)    return StiLocalization.Get("Report", "StiGlassBrush");
			}
			if (destinationType == typeof(InstanceDescriptor) && value != null)
			{
				Type[] types;
				ConstructorInfo info;
				object[] objs;

				#region StiSolidBrush
				if (value is StiEmptyBrush)
				{
					types = new Type[0];

					info = typeof(StiEmptyBrush).GetConstructor(types);
					if (info != null)
					{
						objs = new object[0];

                        return CreateNewInstanceDescriptor(info, objs);
					}
				}
				#endregion

				#region StiGradientBrush
				else if (value is StiGradientBrush)
				{
					StiGradientBrush brush = (StiGradientBrush)value;
				
					types = new Type[]{
											typeof(Color),
											typeof(Color),
											typeof(double)
									  };

					info = typeof(StiGradientBrush).GetConstructor(types);
					if (info != null)
					{
						objs = new object[]	{	brush.StartColor,
												brush.EndColor, 
												brush.Angle};

                        return CreateNewInstanceDescriptor(info, objs);
					}
				}
				#endregion

				#region StiHatchBrush
				else if (value is StiHatchBrush)
				{
					StiHatchBrush brush = (StiHatchBrush)value;
				
					types = new Type[]{	typeof(HatchStyle),
										typeof(Color),
										typeof(Color)};

					info = typeof(StiHatchBrush).GetConstructor(types);
					if (info != null)
					{
						objs = new object[]	{	brush.Style,
												brush.ForeColor, 
												brush.BackColor};

                        return CreateNewInstanceDescriptor(info, objs);
					}
				}
				#endregion

				#region StiSolidBrush
				else if (value is StiSolidBrush)
				{
					StiSolidBrush brush = (StiSolidBrush)value;
				
					types = new Type[]{	typeof(Color)};

					info = typeof(StiSolidBrush).GetConstructor(types);
					if (info != null)
					{
						objs = new object[]{brush.Color};

                        return CreateNewInstanceDescriptor(info, objs);
					}
				}
				#endregion

				#region StiGlareBrush
				else if (value is StiGlareBrush)
				{
					StiGlareBrush brush = (StiGlareBrush)value;
				
					types = new Type[]{
										  typeof(Color),
										  typeof(Color),
										  typeof(double),
										  typeof(float),
										  typeof(float)
									  };

					info = typeof(StiGlareBrush).GetConstructor(types);
					if (info != null)
					{
						objs = new object[]	{	brush.StartColor,
												brush.EndColor, 
												brush.Angle,
												brush.Focus,
												brush.Scale};

                        return CreateNewInstanceDescriptor(info, objs);
					}
				}
				#endregion

                #region StiGlassBrush
                else if (value is StiGlassBrush)
                {
                    StiGlassBrush brush = (StiGlassBrush)value;

                    types = new Type[]{
										  typeof(Color),
										  typeof(bool),
										  typeof(float)
									  };

                    info = typeof(StiGlassBrush).GetConstructor(types);
                    if (info != null)
                    {
                        objs = new object[]	{	brush.Color,
												brush.DrawHatch, 
												brush.Blend};

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
