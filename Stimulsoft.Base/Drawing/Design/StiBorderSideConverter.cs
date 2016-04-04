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
	/// Converts a StiBorderSide object from one data type to another.
	/// </summary>
	public class StiBorderSideConverter : TypeConverter
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
			if (destinationType == typeof(string) && value is StiBorderSide)
			{
				if (value == null)return "null";
				else 
				{
                    StiBorderSide side = value as StiBorderSide;
                    if (side.side == StiBorderSides.Top)
                        return "(" + StiLocalization.Get("PropertyMain", "TopSide") + ")";
                    if (side.side == StiBorderSides.Bottom)
                        return "(" + StiLocalization.Get("PropertyMain", "BottomSide") + ")";
                    if (side.side == StiBorderSides.Left)
                        return "(" + StiLocalization.Get("PropertyMain", "LeftSide") + ")";
                    if (side.side == StiBorderSides.Right)
                        return "(" + StiLocalization.Get("PropertyMain", "RightSide") + ")";
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
