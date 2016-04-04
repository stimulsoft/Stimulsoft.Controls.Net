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
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;



namespace Stimulsoft.Base.Drawing.Design
{
	/// <summary>
	/// Converts colors from one data type to another.
	/// </summary>
	public class StiColorConverter : ColorConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))return true; 
			return base.CanConvertFrom(context, sourceType); 
		} 

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string colorName = value as string;
            if (colorName != null & colorName.IndexOf(",", StringComparison.InvariantCulture) != -1)
			{
				colorName = colorName.Trim();
			
				string []strs = colorName.Split(new char[]{','});
				if (strs.Length == 4)return Color.FromArgb(
										 int.Parse(strs[0].Trim()),
										 int.Parse(strs[1].Trim()),
										 int.Parse(strs[2].Trim()),
										 int.Parse(strs[3].Trim()));

				return Color.FromArgb(
					int.Parse(strs[0].Trim()),
					int.Parse(strs[1].Trim()),
					int.Parse(strs[2].Trim()));
			}
			return base.ConvertFrom(context, culture, value); 
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, 
			Type destinationType)
		{
			if (destinationType == typeof(string))return true;
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, 
			object value, Type destinationType) 
		{
			if (destinationType == typeof(string))
			{
				Color color = (Color)value;
				string colorName = null;
				if (color.IsSystemColor)
				{
                    colorName = StiLocalization.Get("PropertySystemColors", color.Name, false);
                    if (colorName == null)
                        return color.Name;
				}
				else if (color.IsKnownColor)
				{
					colorName = StiLocalization.Get("PropertyColor", color.Name);
				}
				else 
				{
					if (color.A == 255)
						return		 
						color.R.ToString() + "," +
						color.G.ToString() + "," +
						color.B.ToString();
					else 
						return		 
						color.A.ToString() + "," +
						color.R.ToString() + "," +
						color.G.ToString() + "," +
						color.B.ToString();
				}
				return colorName;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

	}
}
