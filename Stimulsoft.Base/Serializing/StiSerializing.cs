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
using System.Drawing.Imaging;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Collections;
using System.Globalization;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Design;


namespace Stimulsoft.Base.Serializing
{
	public delegate void StiTypeNotFoundEventHandler(object sender, StiTypeNotFoundEventArgs e);

	#region StiTypeNotFoundEventArgs
	public class StiTypeNotFoundEventArgs : EventArgs
	{
		private string typeName = string.Empty;
		public string TypeName
		{
			get
			{
				return typeName;
			}
		}


		private object createdObject = null;
		public object CreatedObject
		{
			get
			{
				return createdObject;
			}
			set
			{
				createdObject = value;
			}
		}


		public StiTypeNotFoundEventArgs(string typeName)
		{
			this.typeName = typeName;
		}
	}
	#endregion

	public delegate void StiPropertyNotFoundEventHandlers(object sender, StiPropertyNotFoundEventArgs e);

	#region StiPropertyNotFoundEventArgs
	public class StiPropertyNotFoundEventArgs : EventArgs
	{
		private string propertyName = string.Empty;
		public string PropertyName
		{
			get
			{
				return propertyName;
			}
			set
			{
				propertyName = value;
			}
		}

		private Type propertyType = null;
		public Type PropertyType
		{
			get
			{
				return propertyType;
			}
		}


		public StiPropertyNotFoundEventArgs(string propertyName, Type propertyType)
		{
			this.propertyName = propertyName;
			this.propertyType = propertyType;
		}
	}
	#endregion

	/// <summary>
	/// Class contains methods of serialization and deserialization.
	/// </summary>
	public class StiSerializing
	{
		#region Fields
		/// <summary>
		/// Converter that is used for convertation of objects into the line and back.
		/// </summary>
		private StiObjectStringConverter converter;

		/// <summary>
		/// Hashtable that is used for check acceleration whether this is a content.
		/// </summary>
		private Hashtable contentHashtable = new Hashtable();

		/// <summary>
		/// Collection for serialization/deserialization.
		/// </summary>
		public StiGraphs Graphs;

		private int ItemIndex = 1;

        /// <summary>
        /// A collection which used for conversation of the name of property to correct name of property.
        /// </summary>
        private Hashtable propertyNameHashtable = new Hashtable();
		
		/// <summary>
		/// Collection of references for delayed serialization/deserialization.
		/// </summary>
		public StiReferenceCollection References;
		#endregion

		#region Methods
		/// <summary>
		/// Adds the string representation of type for type conversation.
		/// </summary>
		public static void AddSourceTypeToDestinationType(string typeDestination, string typeSource)
		{
			SourceTypeToDestinationType[typeSource] = typeDestination;
		}

		private static string ConvertSourceTypeToDestinationType(string sourceType)
		{
			string destinationType = SourceTypeToDestinationType[sourceType] as string;
			return destinationType == null ? sourceType : destinationType;
		}


		/// <summary>
		/// Adds the type and its string substitution.
		/// </summary>
		public static void AddStringType(Type type, string str)
		{
			StringToType[str] = type;
			TypeToString[type] = str;
		}


		/// <summary>
		/// Adds the name of the property and its string substitution.
		/// </summary>
		public void AddStringProperty(string property, string str)
		{
			StringToProperty[str] = property;
			PropertyToString[property] = str;
		}

		private Type GetType(string typeStr)
		{
			Type type = StringToType[ConvertSourceTypeToDestinationType(typeStr)] as Type;
			return type;
		}

		private object GetObjectFromType(string typeStr)
		{
			Type type = GetType(typeStr);
			if (type == null)
			{
				return StiActivator.CreateObject(ConvertSourceTypeToDestinationType(typeStr));
			}
			else
			{
				return StiActivator.CreateObject(type);
			}
		}


		/// <summary>
		/// Clears the hashtable of strings-types.
		/// </summary>
		public static void ClearStringType()
		{
			StringToType.Clear();
			TypeToString.Clear();
		}


		/// <summary>
		/// Clears the hashtable of strings-properties.
		/// </summary>
		public void ClearPropertyString()
		{
			StringToProperty.Clear();
			PropertyToString.Clear();
		}


		/// <summary>
		/// Returns the string substitution for a property name.
		/// </summary>
		public string GetStringFromProperty(string propertyName)
		{
			string str = PropertyToString[propertyName] as string;
			if (str == null)return propertyName;
			return str;
		}


		/// <summary>
		/// Returns the property name from string-substitution.
		/// </summary>
		public string GetPropertyFromString(string str)
		{
			string propertyName = StringToProperty[str] as string;
			if (propertyName == null)return str;
			return propertyName;
		}


		/// <summary>
		/// Sets delayed references for serialization.
		/// </summary>
		public void SetReferenceSerializing()
		{
			foreach (StiReference reference in References)
			{
				if (reference.PropInfo.Value != null)
				{
					int code = Graphs[reference.PropInfo.Value];
					if (code == -1)
					{
						reference.PropInfo.IsReference = false;
						reference.PropInfo.Value = null;
					}
				
					else reference.PropInfo.ReferenceCode = code;
				}
			}
		}

		
		/// <summary>
		/// Sets delayed references for serialization.
		/// </summary>
		public void SetReferenceDeserializing()
		{
			int index = 0;
			foreach (StiReference reference in References)
			{
				object val = Graphs[reference.PropInfo.ReferenceCode];
				if (val != null)SetProperty(reference.PropertyInfo, reference.Object, val);
				index++;
			}
		}

		
		/// <summary>
		/// Returns the type of elements in the array or collection.
		/// </summary>
		/// <param name="array">Array or collection.</param>
		/// <returns>Element type.</returns>
		public static Type GetTypeOfArrayElement(object array)
		{
			if (array != null)
			{
				Type type = array.GetType();
				if (type.GetElementType() != null)return  type.GetElementType();
				MethodInfo[] methods = type.GetMethods();
			
				foreach (MethodInfo methodInfo in methods)
					if (methodInfo.Name == "get_Item")
					{
						return methodInfo.ReturnType;
					}	

				string typeName = array.GetType().FullName;
				return StiTypeFinder.GetType(typeName.Substring(0, typeName.Length - 2));
			}
			return typeof(object);
		}


		/// <summary>
		/// Returns the value to default for property. 
		/// If the value is not assigned by default then null returns.
		/// </summary>
		/// <param name="prop">Descriptor.</param>
		/// <returns>Default.</returns>
		public object GetDefaultValue(MemberDescriptor prop)
		{
			DefaultValueAttribute defaultValue = 
				prop.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;
			if (defaultValue == null)return null;
			return defaultValue.Value;
		}

		
		/// <summary>
		/// Returns true if the object is marked as a content.
		/// </summary>
		/// <param name="obj">Object for check.</param>
		/// <returns>Result of check.</returns>
		public bool IsContent(object obj)
		{
            if (obj == null) return false;
			System.Type type = obj.GetType();
			object result = contentHashtable[type];
			if (result != null)
			{
				return (bool)result;
			}
			Object[] attrs = type.GetCustomAttributes(typeof(StiSerializableAttribute), true);
			if (attrs.Length > 0 && 
				((StiSerializableAttribute)attrs[0]).Visibility == StiSerializationVisibility.Content)
			{
				contentHashtable.Add(type, true);
				return true;
			}
			contentHashtable.Add(type, false);
			return false;
		}

	
		/// <summary>
		/// Serilize an object into the type List into the list.
		/// </summary>
		/// <param name="props">List for record.</param>
		/// <param name="list">Serialized object.</param>
		/// <param name="serializeType">Serialization type.</param>
		/// <returns>Number of serialized objects.</returns>
		private int SerializeList(StiPropertyInfoCollection props, object list, StiSerializeTypes serializeType)
		{
			if (list == null) return 0;
		
			int count = 0;
			
			foreach (object item in (IList)list)
			{
				if (item == null) continue;
				InvokeSerializing();
				
				if (item.GetType().IsPrimitive || item is string || IsContent(item))
				{
					props.Add(new StiPropertyInfo("value", item, null, false, false, false, null));
				}				
				else
				{
					int graphCode = Graphs[item];

					#region Object earlier does not serialized
					if (graphCode == -1)
					{
                        Graphs.Add(item);

						StiPropertyInfo itemProp = null;

						if (CheckSerializable && item is IStiSerializable && (!(item is IStiNonSerialized)) && 
							((!IgnoreSerializableForContainer) ||
                            (IgnoreSerializableForContainer && item.GetType().ToString().IndexOf("Container", StringComparison.InvariantCulture) == -1)))
						{
							itemProp = new StiPropertyInfo(
								"item" + count.ToString(), item, null, true, false, false, item.GetType().ToString());
							itemProp.IsSerializable = true;
						}
						else
						{
                            #region IStiSerializableCustomControl
                            if (item is IStiSerializableCustomControl &&
                                ((IStiSerializableCustomControl)item).Control != null)
                            {
                                Control itemControl = ((IStiSerializableCustomControl)item).Control as Control;
                                Graphs.Add(itemControl);
                                StiPropertyInfo itemControlProp = null;

                                string nameControl = "Item" + (ItemIndex++).ToString();

                                PropertyInfo piControl = itemControl.GetType().GetProperty("Name");
                                if (piControl != null) nameControl = (string)piControl.GetValue(itemControl, null);
                                itemControlProp = new StiPropertyInfo(nameControl, itemControl, null, true, false, false, itemControl.GetType().ToString());
                                itemControlProp.Properties.AddRange(SerializeControl(itemControl, serializeType));

                                itemControlProp.ReferenceCode = Graphs[itemControl];
                                props.Add(itemControlProp);
                            }
                            #endregion

							string name = "Item" + (ItemIndex ++).ToString();

							PropertyInfo pi = item.GetType().GetProperty("Name");
							if (pi != null)name = (string)pi.GetValue(item, null);
							itemProp = new StiPropertyInfo(name, item, null, true, false, false, item.GetType().ToString());
							itemProp.Properties.AddRange(SerializeObject(item, serializeType));
                        }
						itemProp.ReferenceCode = Graphs[item];
						props.Add(itemProp);
					}
					#endregion

					#region Object earlier serialized - serializing reference
					else
					{
						string name = "Item" + (ItemIndex ++).ToString();

						StiPropertyInfo itemProp = new StiPropertyInfo(
							name, item, null, true, true, false, item.GetType().ToString());
						itemProp.ReferenceCode = graphCode;
						props.Add(itemProp);
					}
					#endregion
				}
				count++;
			}
			
			return count;
		}

        public StiPropertyInfoCollection SerializeControl(Control obj, StiSerializeTypes serializeType)
        {
            StiPropertyInfoCollection propList = new StiPropertyInfoCollection();

            Attribute[] attr = { new SerializableAttribute(), new DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Visible)};
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj, attr);
            foreach (PropertyDescriptor p in properties)
            {
                if (!p.ShouldSerializeValue(obj))
                {
                    continue;
                }
                object defaultValue = GetDefaultValue(p);
                StiPropertyInfo propInfo = new StiPropertyInfo(p.Name, p.GetValue(obj), defaultValue, false, false, false, p.PropertyType.ToString());
                if (propInfo != null) propList.Add(propInfo);
            }

            return propList;
        }

		/// <summary>
		/// Serilizes an object into the list.
		/// </summary>
		/// <param name="obj">Object for serialization.</param>
		/// <param name="serializeType">Serialization type.</param>
		/// <returns>List of serialized objects.</returns>
		public StiPropertyInfoCollection SerializeObject(object obj, StiSerializeTypes serializeType) 
		{			
			if (IsContent(obj))
			{
				StiPropertyInfoCollection pl = new StiPropertyInfoCollection();
				pl.Add(new StiPropertyInfo(string.Empty, obj, null, false, false, false, null));
				return pl;
			}

			if (obj == null) return null;
			if (Graphs[obj] == -1)Graphs.Add(obj);

			StiPropertyInfoCollection propList = new StiPropertyInfoCollection();
            
			PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
			if (props == null || props.Count == 0) return null;
			if (SortProperties)props = props.Sort();
			int count = props.Count;

			for (int index = 0; index < count; index++)
			{
				InvokeSerializing();
				PropertyDescriptor prop = props[index] as PropertyDescriptor;	
								
				StiSerializableAttribute ac = 
					prop.Attributes[typeof(StiSerializableAttribute)] as StiSerializableAttribute;

				if (ac != null)
				{
					bool contSer = false;
					if (((serializeType & StiSerializeTypes.SerializeToCode) > 0) && 
						((ac.SerializeType & StiSerializeTypes.SerializeToCode) > 0))contSer = true;

					else if (((serializeType & StiSerializeTypes.SerializeToDesigner) > 0) && 
						((ac.SerializeType & StiSerializeTypes.SerializeToDesigner) > 0))contSer = true;

					else if (((serializeType & StiSerializeTypes.SerializeToSaveLoad) > 0) && 
						((ac.SerializeType & StiSerializeTypes.SerializeToSaveLoad) > 0))contSer = true;

					else if (((serializeType & StiSerializeTypes.SerializeToDocument) > 0) && 
						((ac.SerializeType & StiSerializeTypes.SerializeToDocument) > 0))contSer = true;

					if (!contSer)continue;							

					//If serialized
					if (ac != null && 
						ac.Visibility != StiSerializationVisibility.None && 
						prop.Attributes[typeof(StiNonSerializedAttribute)] == null)
					{
						StiSerializationVisibility visibility = ((StiSerializableAttribute)ac).Visibility;
						object defaultValue = GetDefaultValue(prop);

						object valueObject = prop.GetValue(obj);
						StiPropertyInfo propInfo = null;
						
						#region Value is null
						if (valueObject == null)
						{
							propInfo = new StiPropertyInfo(
								prop.Name, null, defaultValue, true, false, false, prop.PropertyType.ToString());
						}
						#endregion

						#region Is Reference
						else if (visibility == StiSerializationVisibility.Reference)
						{
							propInfo = new StiPropertyInfo(
								prop.Name, valueObject, defaultValue, true, true, false, prop.PropertyType.ToString());
							References.Add(propInfo);
						}
						#endregion

						#region Is Simple types
						else if (visibility == StiSerializationVisibility.Content)
						{
							if (!(valueObject is IStiDefault && ((IStiDefault)valueObject).IsDefault))
							{
								propInfo = new StiPropertyInfo(
									prop.Name, valueObject, defaultValue, false, false, false, prop.PropertyType.ToString());
							}
						}
						#endregion

						#region Is List
						else if (visibility == StiSerializationVisibility.List)
						{
							if (valueObject is IList)
							{
								propInfo = new StiPropertyInfo(
									prop.Name, valueObject, defaultValue, false, false, true, prop.PropertyType.ToString());
								propInfo.Count = SerializeList(propInfo.Properties, valueObject, serializeType);
							}
							else throw new Exception(valueObject.ToString() + " is not list");
						}
						#endregion

						#region Is Class
						else if (visibility == StiSerializationVisibility.Class)
						{
							if (valueObject.GetType().IsClass)
							{
								#region Stimulsoft Reports specified condition!!
								bool allow = prop.Name == "Interaction" && serializeType == StiSerializeTypes.SerializeToCode;
								#endregion

								if (!(valueObject is IStiDefault && ((IStiDefault)valueObject).IsDefault && (!allow)))
								{
									#region Check object on subject of ignore references
									StiReferenceIgnoreAttribute[] attrs =
										(StiReferenceIgnoreAttribute[])prop.PropertyType.GetCustomAttributes(
										typeof(StiReferenceIgnoreAttribute), false);
									#endregion

									int graphCode = Graphs[valueObject];

                                    #region Stimulsoft Reports specified condition!!
                                    //TextFormat property всегда сериализуетс€ в код как value тип, поэтому на него нельз€ сослатьс€ по reference,
                                    //и все одинаковые TextFormat надо всегда сериализовать заново.
                                    //метод Graphs.Add() не добавл€ет одинаковый объект в хэш-таблицу и соответственно не создает новый reference,
                                    //поэтому добавлена еще одна проверка в классе StiCodeDomSerializator дл€ избежани€ ошибок
                                    //при повторном добавлении с одинаковым reference
                                    if (prop.Name == "TextFormat" && serializeType == StiSerializeTypes.SerializeToCode)
                                    {
                                        graphCode = -1;
                                    }
                                    #endregion

                                    #region If object earlier does not serialized or it serialized without Graphs
                                    if (graphCode == -1 || attrs.Length > 0)
									{
										Graphs.Add(valueObject);
										int refCode = Graphs[valueObject];

										if (CheckSerializable && valueObject is IStiSerializable && (!(valueObject is IStiNonSerialized)))
										{
											propInfo = new StiPropertyInfo(
												prop.Name, valueObject, defaultValue, true, false, false, prop.PropertyType.ToString());
											propInfo.IsSerializable = true;
										}
										else
										{
											propInfo = new StiPropertyInfo(
												prop.Name, valueObject, defaultValue, true, false, false, prop.PropertyType.ToString());

											StiPropertyInfoCollection properties = SerializeObject(valueObject, serializeType);
											if (properties != null) propInfo.Properties.AddRange(properties);
										}
										propInfo.ReferenceCode = refCode;
									}
									#endregion

									#region Object earlier serialized - serialize reference
									else
									{
										propInfo = new StiPropertyInfo(
											prop.Name, valueObject, defaultValue, true, true, false, prop.PropertyType.ToString());
										propInfo.ReferenceCode = graphCode;
									}
									#endregion
								}
									
							}
							else throw new Exception(valueObject.ToString() + " is not class");
						}
						#endregion

                        #region Is Control
                        else if (visibility == StiSerializationVisibility.Control)
                        {
                            int graphCode = Graphs[valueObject];
                            propInfo = new StiPropertyInfo(
                                prop.Name, valueObject, defaultValue, true, true, false, prop.PropertyType.ToString());
                            propInfo.ReferenceCode = graphCode;
                        }
                        #endregion

                        if (propInfo != null)propList.Add(propInfo);
						
					}
				}
			}
			return propList;
		}

        		
		/// <summary>
		/// Saves object into XML.
		/// </summary>
		/// <param name="tw">Object to save into XML.</param>
		/// <param name="prop">Serialized objects list.</param>
		private void SerializeProperty(XmlTextWriter tw, StiPropertyInfo prop)
        {
            #region Get correct property name
            string propertyName = propertyNameHashtable[prop.Name] as string;
            if (propertyName == null)
            {
                propertyName = XmlConvert.EncodeName(GetStringFromProperty(prop.Name));
                StringBuilder sb = new StringBuilder();
                for (int index = 0; index < propertyName.Length; index++)
                {
                    if (Char.IsPunctuation(propertyName[index])) sb.Append("_");
                    else sb.Append(propertyName[index]);
                }
                propertyName = sb.ToString();
                propertyNameHashtable[prop.Name] = propertyName;
            }
            #endregion

            if (prop.Value == null)
			{
				if (prop.DefaultValue != null)
				{
					tw.WriteStartElement(propertyName);
					tw.WriteAttributeString("isNull", "true");
					tw.WriteEndElement();
				}
			}
			else if (prop.IsKey)
			{
				if (prop.IsReference)
				{
					tw.WriteStartElement(propertyName);
					tw.WriteAttributeString("isRef", prop.ReferenceCode.ToString());
					tw.WriteEndElement();
				}
				else
				{
					tw.WriteStartElement(propertyName);
					if (prop.ReferenceCode != -1)
					{
						tw.WriteAttributeString("Ref", prop.ReferenceCode.ToString());
					}
					
					if (prop.Value != null)
					{
						string typeName = TypeToString[prop.Value.GetType()] as string;
						if (typeName == null)typeName = prop.Value.GetType().FullName;
						tw.WriteAttributeString("type", typeName);
					}
					if (prop.IsSerializable)
					{
						tw.WriteAttributeString("isSer", "true");
						IStiSerializable serializable = prop.Value as IStiSerializable;
						serializable.Serialize(converter, tw);
						Graphs.Add(serializable, prop.ReferenceCode);
					}
					else
					{
						tw.WriteAttributeString("isKey", "true");
						SerializeObject(tw, prop.Properties);
					}
					tw.WriteEndElement();					
				}
			}
			else if (prop.IsList)
			{
				tw.WriteStartElement(propertyName);
				tw.WriteAttributeString("isList", "true");
				if (prop.Value != null)
				{
					tw.WriteAttributeString("count", prop.Count.ToString());
				}
				SerializeObject(tw, prop.Properties);
				tw.WriteEndElement();
			}
			else 
			{
				if (prop.Value is Metafile)
				{					
					tw.WriteStartElement(propertyName);					
					tw.WriteString(StiMetafileConverter.MetafileToString(prop.Value as Metafile));
					tw.WriteEndElement();
				}
				else if (prop.Value is Image)
				{
					tw.WriteStartElement(propertyName);
					tw.WriteString(StiImageConverter.ImageToString(prop.Value as Image));
					tw.WriteEndElement();
				}
				else
				{
					string valueString = converter.ObjectToString(prop.Value);
					if (valueString != null)
					{
						tw.WriteStartElement(propertyName);
						tw.WriteString(valueString);
						tw.WriteEndElement();
					}
				}
			}
		}


		/// <summary>
		/// Saves the list serialized objects into XML.
		/// </summary>
		/// <param name="tw">Object to save into XML</param>
		/// <param name="props">Serialized objects list.</param>
		public void SerializeObject(XmlTextWriter tw, StiPropertyInfoCollection props) 
		{
			if (props == null) return;
			foreach (StiPropertyInfo prop in props)
			{
				InvokeSerializing();
				if (prop.DefaultValue == null || !object.Equals(prop.DefaultValue, prop.Value))
				{
					SerializeProperty(tw, prop);
				}
			}
		}

		
		/// <summary>
		/// Serialize an object into the list.
		/// </summary>
		/// <param name="obj">Object for serialization.</param>
		/// <param name="serializeType">Serialization type.</param>
		/// <returns>Serialized objects list.</returns>
		public StiPropertyInfoCollection Serialize(object obj, StiSerializeTypes serializeType)
		{			
			ItemIndex = 1;
			Graphs = new StiGraphs();
			References = new StiReferenceCollection();
			StiPropertyInfoCollection props = SerializeObject(obj, serializeType);
			SetReferenceSerializing();
			return props;
		}


		/// <summary>
		/// Serializes an object into the stream.
		/// </summary>
		/// <param name="obj">Object for serialization.</param>
		/// <param name="stream">Stream in which serialization will be generated.</param>
		/// <param name="application">Application that generates serialization.</param>
		public void Serialize(object obj, Stream stream, string application)
		{	
			Serialize(obj, stream, application, StiSerializeTypes.SerializeToSaveLoad);
		}

		
		/// <summary>
		/// Serializes object into the stream.
		/// </summary>
		/// <param name="obj">Object for serialization.</param>
		/// <param name="stream">Stream in which serialization will be generated.</param>
		/// <param name="application">Application that generates serialization.</param>
		/// <param name="serializeType">Serialization type.</param>
		public void Serialize(object obj, Stream stream, string application, StiSerializeTypes serializeType)
		{
			CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
			try
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);

				XmlTextWriter tw = new XmlTextWriter(stream, System.Text.Encoding.UTF8);
				tw.Formatting = Formatting.Indented;

				tw.WriteStartDocument(true);
				tw.WriteStartElement("StiSerializer");
				tw.WriteAttributeString("version", StiFileVersions.ReportFile);
                tw.WriteAttributeString("type", "Net");
				tw.WriteAttributeString("application", application);
            
				Graphs = new StiGraphs();
				References = new StiReferenceCollection();
			
				StiPropertyInfoCollection props = SerializeObject(obj, serializeType);
				SetReferenceSerializing();
				SerializeObject(tw, props);

				tw.WriteEndElement();
				tw.Flush();
			}				
			finally
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}

		
		/// <summary>
		/// Seializes object using textWriter.
		/// </summary>
		/// <param name="obj">Object for serialization.</param>
		/// <param name="textWriter">TextWriter in which serialization will be generated.</param>
		/// <param name="application">Application that generates serialization.</param>
		/// <param name="serializeType">Serialization type.</param>
		public void Serialize(object obj, TextWriter textWriter, string application, StiSerializeTypes serializeType)
		{
			CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
			try
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);

				XmlTextWriter tw = new XmlTextWriter(textWriter);

				tw.WriteStartDocument(true);
				tw.WriteStartElement("StiSerializer");
                tw.WriteAttributeString("version", StiFileVersions.ReportFile);
                tw.WriteAttributeString("type", "Net");
				tw.WriteAttributeString("application", application);
			            
				Graphs = new StiGraphs();
				References = new StiReferenceCollection();

				StiPropertyInfoCollection props = SerializeObject(obj, serializeType);
				SetReferenceSerializing();
				SerializeObject(tw, props);

				tw.WriteEndElement();
				tw.Flush();
			}
			finally
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}


		/// <summary>
		/// Serializes object into the file.
		/// </summary>
		/// <param name="obj">Object for serialization.</param>
		/// <param name="path">File in which serialization will be generated.</param>
		/// <param name="application">Application that generates serialization.</param>
		public void Serialize(object obj, string path, string application)
		{
			StiFileUtils.ProcessReadOnly(path);
			FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
			Serialize(obj, stream, application);
			stream.Flush();
			stream.Close();
		}

	
		/// <summary>
		/// Sets property p in object obj value.
		/// </summary>
		/// <param name="p">Property for set.</param>
		/// <param name="obj">Object in which the property is situated.</param>
		/// <param name="value">Value for setup.</param>
		public void SetProperty(PropertyInfo p, object obj, object value)
		{
			if (p != null && p.CanWrite)p.SetValue(obj, value, null);
		}


		/// <summary>
		/// Deserializes object from the list.
		/// </summary>
		/// <param name="obj">Object for deserialization.</param>
		/// <param name="props">List contains objects.</param>
		private void DeserializeObject(object obj, StiPropertyInfoCollection props) 
		{
			foreach (StiPropertyInfo prop in props)
			{
				InvokeDeserializing();
				PropertyInfo p = null;

				if (obj != null)
				{
					Type type = obj.GetType();
					p = type.GetProperty(prop.Name);
				}
				#region Property Not Found
				if (p == null && obj != null)
				{
					StiPropertyNotFoundEventArgs e = new StiPropertyNotFoundEventArgs(
						prop.Name, obj.GetType());
					InvokePropertyNotFound(this, e);

					if (e.PropertyName != prop.Name)
					{
						prop.Name = e.PropertyName;
						p = obj.GetType().GetProperty(prop.Name);
					}
				}
				if (p == null)continue;
				#endregion
                    
				if (!prop.IsReference && prop.ReferenceCode != -1)
				{
					Graphs.Add(prop.Value, prop.ReferenceCode);
				}

				//Is Class
				if (prop.IsKey)
				{	
					SetProperty(p, obj, prop.Value);

					if (p != null)DeserializeObject(p.GetValue(obj, null), prop.Properties);
					else DeserializeObject(null, prop.Properties);
					
					converter.SetProperty(p, obj, prop.Value);
				}
					//Is List
				else if (prop.IsList)
				{
					int count = (int)prop.Count;
						
					IList list = null;
					object item = null;
					if (p != null)
					{
						list = p.GetValue(obj, null) as IList;

						if (list != null)
						{
							Type elementType = GetTypeOfArrayElement(list);
							if (list is Array)
							{
								list = Array.CreateInstance(elementType, count);
								SetProperty(p, obj, list);
							}
							else list.Clear();

							string listType = list.GetType().ToString();
										
							int index = 0;
							foreach (StiPropertyInfo property in prop.Properties)
							{
								#region Invoke StiTypeNotFoundEvent
								if (property.Value == null)
								{
									StiTypeNotFoundEventArgs e = new StiTypeNotFoundEventArgs(property.TypeName);
									InvokeTypeNotFound(this, e);
									if (e.CreatedObject != null)
									{
										property.Value = e.CreatedObject;
									}
								}
								#endregion

								#region Create Undefined Types
								if (property.Value == null)
								{
                                    if (listType.EndsWith("StiComponentsCollection", StringComparison.InvariantCulture))
									{
										property.Value = StiActivator.CreateObject("Stimulsoft.Report.Components.StiUndefinedComponent");
									}
                                    else if (listType.EndsWith("StiPagesCollection", StringComparison.InvariantCulture))
									{
										property.Value = StiActivator.CreateObject("Stimulsoft.Report.Components.StiPage");
									}
                                    else if (listType.EndsWith("StiDatabaseCollection", StringComparison.InvariantCulture))
									{
										property.Value = StiActivator.CreateObject("Stimulsoft.Report.Dictionary.StiUndefinedDatabase");
									}
                                    else if (listType.EndsWith("StiDataSourcesCollection", StringComparison.InvariantCulture))
									{
										property.Value = StiActivator.CreateObject("Stimulsoft.Report.Dictionary.StiUndefinedDataSource");
									}
								}
								#endregion

								InvokeDeserializing();
								if (property.IsReference && property.ReferenceCode != -1)
								{
									property.Value = Graphs[property.ReferenceCode];
									item = property.Value;
								}
								else
								{
									if (elementType == typeof(string) || (!(property.Value is string)))
									{
										item = property.Value;
									}
									else 
									{
										try
										{
											item = converter.StringToObject((string)property.Value, elementType);
										}
										catch
										{
											item = (string)property.Value;
										}
									}
								}

								if (!property.IsReference && property.ReferenceCode != -1)
								{
									if (property.Value != null)Graphs.Add(property.Value, property.ReferenceCode);
								}

								if (list is Array)
								{
									object objs = null;
									if (elementType == item.GetType())objs = item;
									else converter.StringToObject((string)item, elementType);
									((Array)list).SetValue(objs, index++);
								}
								else 
								{
									if (item != null)
									{
										list.Add(item);
										if (!property.IsReference && (!property.IsSerializable))DeserializeObject(item, property.Properties);
									}
								}
							}
						}
					}
				}
					//Is Reference
				else if (prop.IsReference)
				{

					object val = Graphs[prop.ReferenceCode];
					SetProperty(p, obj, val);
					if (val == null)References.Add(prop, obj, p);
				}
					//Is Null
				else if (prop.Value == null)
				{
					SetProperty(p, obj, null);
				}
				else if (p != null)
				{
					object valueObj = prop.Value;
					if (prop.Value is string && p.PropertyType != typeof(object))
					{
						valueObj = converter.StringToObject((string)prop.Value, p.PropertyType);
					}
					SetProperty(p, obj, valueObj);
				}
			}
		}


		/// <summary>
		/// Deserilizes object from XML.
		/// </summary>
		/// <param name="tr">Object for to read XML.</param>
		/// <returns>List contains objects.</returns>
		private StiPropertyInfoCollection DeserializeObject(XmlTextReader tr, string parentPropName) 
		{
            var propList = new StiPropertyInfoCollection();
			tr.Read();
			while (!tr.EOF) 
			{
				if (tr.IsStartElement()) 
				{
					InvokeDeserializing();

                    var prop = new StiPropertyInfo(
						XmlConvert.DecodeName(GetPropertyFromString(tr.Name)), null, 
						tr.GetAttribute("isKey") == "true",
						tr.MoveToAttribute("isRef"),
						tr.MoveToAttribute("isList"), null);
				
					bool isNull = tr.GetAttribute("isNull") == "true";

					if (isNull)
					{
						prop.Value = null;
					}
					else if (tr.GetAttribute("isSer") != null)
					{
						string s = tr.GetAttribute("Ref");
						if (s != null)prop.ReferenceCode = int.Parse(s);

						string typeName = tr.GetAttribute("type");

						prop.TypeName = typeName;

						prop.Value = GetObjectFromType(typeName);
						prop.IsSerializable = true;
                        var serializable = prop.Value as IStiSerializable;
						serializable.Deserialize(converter, tr);
						Graphs.Add(serializable, prop.ReferenceCode);
					}
					else if (prop.IsReference)
					{
						prop.ReferenceCode = int.Parse(tr.GetAttribute("isRef"));
					}
					else if (prop.IsList)
					{
						prop.Count = int.Parse(tr.GetAttribute("count"));
						if (prop.Count > 0)
						{
							if (!tr.IsEmptyElement)prop.Properties = DeserializeObject(tr, prop.Name);
                        }
					}
					else if (!prop.IsKey)
					{
						if (tr.GetAttribute("isImage") == "true")
						{
							prop.Value = StiImageConverter.StringToImage(tr.ReadString());
							prop.TypeName = typeof(Image).ToString();
						}
						else if (tr.GetAttribute("isEmfImage") == "true")
						{
							prop.Value = StiMetafileConverter.StringToMetafile(tr.ReadString());
							prop.TypeName = typeof(Image).ToString();
						}
						else
						{
							prop.Value = tr.ReadString();
						}
						
					}
					else if (prop.IsKey)
					{
						string s = tr.GetAttribute("Ref");
						if (s != null)prop.ReferenceCode = int.Parse(s);						
						
						string typeName = tr.GetAttribute("type");
						prop.TypeName = typeName;

						prop.Value = GetObjectFromType(typeName);
						if (!tr.IsEmptyElement)
						{
							prop.Properties = DeserializeObject(tr, prop.Name);
                        }

                        #region AllowFixOldChartTitle
                        if (AllowFixOldChartTitle)
                        {
                            if ((parentPropName == "YAxis" || parentPropName == "YRightAxis") && prop.Name == "Title")
                            {
                                #region Exists ?
                                bool exists = false;
                                foreach (StiPropertyInfo pr in prop.Properties)
                                {
                                    if (pr.Name == "Direction")
                                    {
                                        exists = true;
                                        break;
                                    }
                                }
                                #endregion

                                if (!exists)
                                {
                                    var propInfo = prop.Value.GetType().GetProperty("Direction");

                                    if (parentPropName == "YAxis")
                                        propInfo.SetValue(prop.Value, 3, new object[0]);//BottomToTop = 3

                                    if (parentPropName == "YRightAxis")
                                        propInfo.SetValue(prop.Value, 2, new object[0]);//TopToBottom = 2
                                }
                            }
                        }
                        #endregion
                    }
					propList.Add(prop);
				}
				else if (tr.NodeType == XmlNodeType.EndElement)break;
				
				tr.Read();
			}
			return propList;
		}
	

		/// <summary>
		/// Deserializes object from the stream.
		/// </summary>
		/// <param name="obj">Object for deserialization.</param>
		/// <param name="stream">Stream from which deserialization is generated.</param>
		/// <param name="application">Application that generates deserialization.</param>
		public void Deserialize(object obj, Stream stream, string application)
		{
			var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
			try
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);

				var tr = new XmlTextReader(stream);
				tr.Read();
			
				tr.Read();
				if (tr.IsStartElement())
				{
					if (tr.Name == "StiSerializer")
					{
						Graphs = new StiGraphs();
						References = new StiReferenceCollection();
						Graphs.Add(obj);
						string ver = tr.GetAttribute("version");
						string app = tr.GetAttribute("application");
						string enc = tr.GetAttribute("encoding");
					
						if (app == application)
						{
							DeserializeObject(obj, DeserializeObject(tr, null));
							SetReferenceDeserializing();
						}
					}
				}
			}
			finally
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}


		/// <summary>
		/// Deserializes object using textReader.
		/// </summary>
		/// <param name="obj">Object for deserialization.</param>
		/// <param name="textReader">TextReader from which deserialization will be generated.</param>
		/// <param name="application">Application that generates deserialization.</param>
		public void Deserialize(object obj, TextReader textReader, string application)
		{
			var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
			try
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);

				var tr = new XmlTextReader(textReader);
				tr.Read();

				tr.Read();
				if (tr.IsStartElement())
				{
					if (tr.Name == "StiSerializer")
					{
						Graphs = new StiGraphs();
						References = new StiReferenceCollection();
						Graphs.Add(obj);
						string ver = tr.GetAttribute("version");
						string app = tr.GetAttribute("application");
                        if (app == application)
                        {
                            DeserializeObject(obj, DeserializeObject(tr, null));
                            SetReferenceDeserializing();
                        }
					}
				}
			}
			finally
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}


		/// <summary>
		/// Deserializes object from the file.
		/// </summary>
		/// <param name="obj">Object for deserialization.</param>
		/// <param name="path">File from which deserialization will be generated.</param>
		/// <param name="application">Application that generates deserialization.</param>
		public void Deserialize(object obj, string path, string application)
		{
			var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
			Deserialize(obj, stream, application);
			stream.Close();
		}

		#endregion
		
		#region Properties
        private static bool allowFixOldChartTitle = false;
        /// <summary>
        /// Gets or sets value which indicates that bug with loading old charts (without Direction property) will be fixed during loading of report.
        /// </summary>
        public static bool AllowFixOldChartTitle
        {
            get
            {
                return allowFixOldChartTitle;
            }
            set
            {
                allowFixOldChartTitle = value;
            }
        }

		private bool sortProperties = true;
		/// <summary>
		/// Gets or sets value indicates should properties be sorted alphabetically or not.
		/// </summary>
		public bool SortProperties
		{
			get
			{
				return sortProperties;
			}
			set
			{
				sortProperties = value;
			}
		}


		private bool checkSerializable = false;
		/// <summary>
		/// Gets or sets value, indicates should objects, when serialization, be checked in their capability or not to realize IStiSerializable.
		/// </summary>
		public bool CheckSerializable
		{
			get
			{
				return checkSerializable;
			}
			set
			{
				checkSerializable = value;
			}
		}


		private bool ignoreSerializableForContainer = false;
		/// <summary>
		/// Internal use only.
		/// </summary>
		public bool IgnoreSerializableForContainer
		{
			get
			{
				return ignoreSerializableForContainer;
			}
			set
			{
				ignoreSerializableForContainer = value;
			}
		}


		private static Hashtable typeToString = new Hashtable();
		/// <summary>
		/// Gets or sets table for transformation of a type into the string.
		/// </summary>
		public static Hashtable TypeToString
		{
			get
			{
				return typeToString;
			}
			set
			{
				typeToString = value;
			}
		}


		private static Hashtable stringToType = new Hashtable();
		/// <summary>
		/// Gets or sets table for transformation a string into the type.
		/// </summary>
		public static Hashtable StringToType
		{
			get
			{
				return stringToType;
			}
			set
			{
				stringToType = value;
			}
		}

		private static Hashtable sourceTypeToDestinationType = new Hashtable();
		public static Hashtable SourceTypeToDestinationType
		{
			get
			{
				return sourceTypeToDestinationType;
			}
			set
			{
				sourceTypeToDestinationType = value;
			}
		}


		private static Hashtable propertyToString = new Hashtable();
        private static Hashtable propertyToString2 = new Hashtable();
        /// <summary>
		/// Gets or sets table for transformation the property name into the string.
		/// </summary>
		public Hashtable PropertyToString
		{
			get
			{
                if (IsDocument)
                    return propertyToString2;
                else
                    return propertyToString;
			}
			set
			{
                if (IsDocument)
                    propertyToString2 = value;
                else
                    propertyToString = value;
            }
		}


		private static Hashtable stringToProperty = new Hashtable();
        private static Hashtable stringToProperty2 = new Hashtable();
		/// <summary>
		/// Gets or sets table for transformation the string into the property name.
		/// </summary>
		public Hashtable StringToProperty
		{
			get
			{
                if (IsDocument)
                    return stringToProperty2;
                else
				    return stringToProperty;
			}
			set
			{
                if (IsDocument)
                    stringToProperty2 = value;
                else
                    stringToProperty = value;
            }
		}


        private bool isDocument = false;
        /// <summary>
        /// Gets or sets a value indicates Document mode.
        /// </summary>
        public bool IsDocument
        {
            get
            {
                return isDocument;
            }
            set
            {
                isDocument = value;
            }
        }
		#endregion

		#region Events.Static
		#region TypeNotFound
		public static event StiTypeNotFoundEventHandler TypeNotFound;

		internal static void InvokeTypeNotFound(StiSerializing serializing, StiTypeNotFoundEventArgs e)
		{
			if (TypeNotFound != null)TypeNotFound(serializing, e);
		}
		#endregion

		#region PropertyNotFound
		public static event StiPropertyNotFoundEventHandlers PropertyNotFound;

		internal static void InvokePropertyNotFound(StiSerializing serializing, StiPropertyNotFoundEventArgs e)
		{
			if (PropertyNotFound != null)PropertyNotFound(serializing, e);
		}
		#endregion

        #region Serializing
        public static event EventHandler GlobalSerializing;

        public static void InvokeGlobalSerializing(object sender, EventArgs e)
        {
            if (GlobalSerializing != null) GlobalSerializing(sender, e);
        }
        #endregion

        #region GlobalDeserializing
        public static event EventHandler GlobalDeserializing;

        public static void InvokeGlobalDeserializing(object sender, EventArgs e)
        {
            if (GlobalDeserializing != null) GlobalDeserializing(sender, e);
        }
        #endregion
		#endregion

		#region Events
		#region Serializing
		/// <summary>
		/// Event occurs when serializing of one element.
		/// </summary>
		public event EventHandler Serializing;

		protected virtual void OnSerializing(EventArgs e)
		{
            InvokeGlobalSerializing(this, e);
		}


		/// <summary>
		/// Raises the Serializing event for this control.
		/// </summary>
		public void InvokeSerializing()
		{
			OnSerializing(EventArgs.Empty);
			if (this.Serializing != null)this.Serializing(null, EventArgs.Empty);
		}
		#endregion

		#region Deserializing
		/// <summary>
		/// Event occurs when deserializing of one element.
		/// </summary>
		public event EventHandler Deserializing;

		protected virtual void OnDeserializing(EventArgs e)
		{
            InvokeGlobalDeserializing(this, e);
		}


		/// <summary>
		/// Raises the Deserializing event for this control.
		/// </summary>
		public void InvokeDeserializing()
		{
			OnDeserializing(EventArgs.Empty);
			if (this.Deserializing != null)this.Deserializing(null, EventArgs.Empty);
		}
		#endregion
		#endregion

		#region Constructor
		/// <summary>
		/// Creates a new instance of the StiSerializing class.
		/// </summary>
		public StiSerializing() : this(new StiObjectStringConverter())
		{
		}


		/// <summary>
		/// Creates a new instance of the StiSerializing class.
		/// </summary>
		/// <param name="converter">Converter for tranformation of objects into the string and back.</param>
		public StiSerializing(StiObjectStringConverter converter)
		{
			this.converter = converter;
		}
		#endregion
	}
}