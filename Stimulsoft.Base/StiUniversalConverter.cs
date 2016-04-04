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
using System.Windows.Forms;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base
{
	public class StiUniversalConverter : StiOrderConverter
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
			#region Find constructor
			ConstructorInfo currentInfo = null;
			if (value != null)
			{
				ConstructorInfo[] cnInfos = value.GetType().GetConstructors();
								
				foreach (ConstructorInfo cnInfo in cnInfos)
				{
					if (cnInfo.GetCustomAttributes(typeof(StiUniversalConstructorAttribute), false).Length > 0)
					{
						currentInfo = cnInfo;
						break;
					}
				}
				if (currentInfo == null)
				{
					if (destinationType == typeof(string))return string.Empty;
					return base.ConvertTo(context, culture, value, destinationType);
				}
			}
			#endregion

            if (destinationType == typeof(string) && currentInfo != null)
			{
				object [] attrs = 
					currentInfo.GetCustomAttributes(typeof(StiUniversalConstructorAttribute), false);

				string name = ((StiUniversalConstructorAttribute)attrs[0]).Name;

				string displayName = StiLocalization.Get("PropertyMain", name, false);
				if (string.IsNullOrEmpty(displayName))return string.Format("({0})", name);;
				return string.Format("({0})", displayName);
			}

            if (destinationType == typeof(InstanceDescriptor) && value != null && currentInfo != null)
			{
				CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
				try
				{
                    System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);

					#region Create parameters list
					ParameterInfo[] pars = currentInfo.GetParameters();

					Type[] types = new Type[pars.Length];
					object[] objs = new object[pars.Length];

					for (int index = 0; index < pars.Length; index++)
					{
						types[index] = pars[index].ParameterType;
						string propertyName = pars[index].Name.Substring(0, 1).ToUpper(System.Globalization.CultureInfo.InvariantCulture) + 
							pars[index].Name.Substring(1);

						PropertyInfo prop = value.GetType().GetProperty(propertyName);
						if (prop == null)throw new ArgumentException(
											 string.Format("Property '{0}' not present in '{1}'",
											 propertyName, value.GetType().ToString()));
						objs[index] = prop.GetValue(value, null);
					}
					#endregion
		
					return new InstanceDescriptor(currentInfo, objs);
				}
				finally
				{
                    System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}	
	}
}
