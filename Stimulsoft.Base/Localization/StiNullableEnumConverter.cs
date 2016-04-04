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
using System.ComponentModel.Design.Serialization;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;
using Stimulsoft.Base.Design;

namespace Stimulsoft.Base.Localization
{
	/// <summary>
	/// Provides a type converter to convert Enum objects to and from various other representations.
	/// </summary>
	public class StiNullableEnumConverter : TypeConverter
    {
        #region Methods
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))return true; 
			return base.CanConvertFrom(context, sourceType); 
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))return true; 
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{	
			if ((value as string) != null)
			{
                Type _type = Nullable.GetUnderlyingType(this.type);
				try
				{
					string strValue = ((string) value);
                    if (strValue.Length == 0) return null;

					if (StiPropertyGridOptions.Localizable)
					{
                        string[] strs = Enum.GetNames(_type);
						foreach (string str in strs)
						{
                            string locName = StiLocalization.Get("PropertyEnum", _type.Name + str, false);
							if (locName != null && locName == strValue)
							{
								strValue = str;
							}
						}
					}

                    if (strValue.IndexOf(',') != -1)
					{
						long num = 0;
						string [] strings = strValue.Split(new char[]{','});
						for (int index = 0; (index < strings.Length); index++)
						{
                            num = (num | Convert.ToInt64(((Enum)Enum.Parse(_type, strings[index], true))));
 						}
                        return Enum.ToObject(_type, num);
 
					}

                    return Enum.Parse(_type, strValue, true);
				}
				catch
				{
					throw new Exception("ConvertInvalidPrimitive");
				}
			}

			return base.ConvertFrom(context, culture, value); 
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, 
			object value, Type destinationType)
		{

			if (destinationType == null)throw new ArgumentNullException("destinationType");
			if (value == null) return "null";

			if ((destinationType == typeof(string)) && (value != null))
			{
                if (value is string && (string)value == "null") 
                    return null;
                Type _type = Nullable.GetUnderlyingType(this.type);

                Type underlyingType = Enum.GetUnderlyingType(_type);
				if (((value as IConvertible) != null) && (value.GetType() != underlyingType))
				{
					value = ((IConvertible)value).ToType(underlyingType, culture);
 				}
                if (!this.type.IsDefined(typeof(FlagsAttribute), false) && !Enum.IsDefined(_type, value))
					throw new Exception("EnumConverterInvalidValue");

                string name = Enum.Format(_type, value, "G");
				if (StiPropertyGridOptions.Localizable)
				{
                    if (name.IndexOf(',') == -1)
					{
                        string locName = StiLocalization.Get("PropertyEnum", _type.Name + name, false);
						if (locName == null)return name;
						return locName;
					}
					else
					{
						string [] names = name.Split(new char[]{','});
                        string itog = string.Empty;

						foreach (string nm in names)
						{
							string str = string.Empty;
                            if (itog != string.Empty) str += ", ";
                            string locName = StiLocalization.Get("PropertyEnum", _type.Name + nm.Trim(), false);
							if (locName != null)itog += str + locName;
							else itog += str + nm.Trim();
						}
						return itog;
					}
				}
				return name;
			}

			if ((destinationType == typeof(InstanceDescriptor)) && (value != null))
			{
                Type _type = Nullable.GetUnderlyingType(this.type);

				string text1 = base.ConvertToInvariantString(context, value);
                if (_type.IsDefined(typeof(FlagsAttribute), false) && (text1.IndexOf(',') != -1))
				{
                    Type underlyingType = Enum.GetUnderlyingType(_type);

					if ((value as IConvertible) == null)return base.ConvertTo(context, culture, value, destinationType); 

					object obj = ((IConvertible) value).ToType(underlyingType, culture);
					Type []types = new Type[]{
												typeof(Type),
												underlyingType
											 };
					MethodInfo methodInfo = typeof(Enum).GetMethod("ToObject", types);

					if (methodInfo == null)return base.ConvertTo(context, culture, value, destinationType);

                    return new InstanceDescriptor(methodInfo, new object[] { _type, obj }); 
				}

                FieldInfo fieldInfo = _type.GetField(text1);
				if (fieldInfo != null)return new InstanceDescriptor(fieldInfo, null); 
			}

			return base.ConvertTo(context, culture, value, destinationType); 
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (this.values == null)
			{
                var values = Enum.GetValues(System.Nullable.GetUnderlyingType(this.type));
                var list = new ArrayList(values);
                list.Add("null");
                this.values = new StandardValuesCollection(list);
 			}
			return this.values;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return !this.type.IsDefined(typeof(FlagsAttribute), false);
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true; 
		}

		public override bool IsValid(ITypeDescriptorContext context, object value)
		{
			return Enum.IsDefined(this.type, value);
        }
        #endregion

        #region Properties
        protected virtual IComparer Comparer 
		{ 
			get
			{
				return InvariantComparer.Default;
			}
		}

		protected Type EnumType 
		{ 
			get
			{
				return this.type;
			}
				
		}

		protected StandardValuesCollection Values 
		{ 
			get
			{
				return this.values; 
			}
			set
			{
				this.values = value;

			}
		}
        #endregion

        #region Fields
        private Type type;
		private StandardValuesCollection values;
        #endregion

        public StiNullableEnumConverter(Type type)
        {
            this.type = type;
        }

        public StiNullableEnumConverter()
        {

        }
    }
}