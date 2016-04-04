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
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Collections;
using Stimulsoft.Base;

namespace Stimulsoft.Base.Services
{
	/// <summary>
	/// Describes class for access to container of services.
	/// </summary>
	public class StiServiceContainer : CollectionBase, ICloneable
	{
		#region ICloneable
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			lock(this)return this.MemberwiseClone();
		}
		#endregion

		#region Fields
		private static Hashtable assemlyToSkip = new Hashtable();
		private Hashtable keysHashtable = new Hashtable();
		private Hashtable typesHashtable = new Hashtable();

		private bool noTypes = false;
		private static Hashtable ServiceToProperties = new Hashtable();
		private static Hashtable AssemblyKeyToString = new Hashtable();
		private static Hashtable AssemblyToTypes = new Hashtable();
		#endregion

		#region Collection
		/// <summary>
		/// Clears all collection from service container.
		/// </summary>
		public new void Clear()
		{
			this.InnerList.Clear();
			keysHashtable = new Hashtable();
			typesHashtable = new Hashtable();

		}

        /// <summary>
		/// Adds service to the container.
		/// </summary>
		/// <param name="service">Service.</param>
        public void Add(StiService service)
        {
            Add(service, true);
        }

		/// <summary>
		/// Adds service to the container.
		/// </summary>
		/// <param name="service">Service.</param>
		public void Add(StiService service, bool callBeforeGetService)
		{
			if (service.ServiceType != null)
			{
			    if (callBeforeGetService && BeforeGetService != null)
			    {
                    var args = new StiServiceActionEventArgs(StiServiceActionType.Add)
			        {
			            Services = new List<StiService> { service }
			        };
			        BeforeGetService(service.ServiceType, args);
			        if (args.Processed) return;
			    }

			    service.PackService();

				lock(List)
				{
					lock(keysHashtable)
					{
						string key = GetStringFromService(service);
						if (keysHashtable[key] == null)
						{
							keysHashtable.Add(key, service); 
							List.Add(service);

							if (!noTypes)
							{
								lock(typesHashtable)
								{
									StiServiceContainer services = typesHashtable[service.ServiceType] as StiServiceContainer;
								
									if (services == null)
									{
										services = new StiServiceContainer(true);
										typesHashtable.Add(service.ServiceType, services);
									}
                                    lock (services)
                                    {
                                        services.Add(service);
                                    }
								}
							}
						}
					}
				}
			}
		}

        public List<T> ToList<T>()
        {
            List<T> list = new List<T>();
            foreach (T item in this.List)
            {
                list.Add(item);
            }

            return list;
        }

	    public List<StiService> ToList()
	    {
	        return ToList<StiService>() as List<StiService>;
	    }


		/// <summary>
		/// Adds services to the container.
		/// </summary>
		/// <param name="services">Services.</param>
		public void AddRange(StiServiceContainer services)
		{
			foreach (StiService service in services)Add(service);
		}


		public bool Contains(StiService service)
		{
			string str = GetStringFromService(service);
			return keysHashtable[str] != null;
		}


		public bool Contains(string service)
		{
			return keysHashtable[service] != null;
		}
		

		public int IndexOf(StiService service)
		{
			return List.IndexOf(service);
		}


		public void Insert(int index, StiService service)
		{
			lock(List)
			{
				service.PackService();
				List.Insert(index, service);

				lock(keysHashtable)
				{
					string key = GetStringFromService(service);
					if (keysHashtable[key] == null)
					{
						keysHashtable.Add(key, service); 

						if (!noTypes)
						{
							lock(typesHashtable)
							{
								StiServiceContainer services = typesHashtable[service.ServiceType] as StiServiceContainer;
								
								if (services == null)
								{
									services = new StiServiceContainer(true);
									typesHashtable.Add(service.ServiceType, services);
								}
                                lock (services)
                                {
                                    services.Add(service);
                                }
							}
						}
					}
				}
			}
		}


		/// <summary>
		/// Removes services of the type from the container.
		/// </summary>
		/// <param name="serviceType">Service type.</param>
		public void Remove(Type serviceType)
		{
			lock(List)
			{
				lock(keysHashtable)
				{
					StiServiceContainer cont = GetServices(serviceType);
					foreach (StiService service in cont)
					{
						List.Remove(service);
						keysHashtable.Remove(GetStringFromService(service));
					}

					if (!noTypes)
					{
						lock(typesHashtable)
						{
							typesHashtable.Remove(serviceType);
						}
					}
				}
			}
		}


		/// <summary>
		/// Removes service from the container.
		/// </summary>
		/// <param name="service">Service.</param>
		public void Remove(StiService service)
		{
			lock(List)
			{
				List.Remove(service);
				keysHashtable.Remove(GetStringFromService(service));

				if (!noTypes)
				{
					lock(typesHashtable)
					{
						StiServiceContainer services = typesHashtable[service.ServiceType] as StiServiceContainer;
                        lock (services)
                        {
                            services.Remove(service);
                        }
					}
				}
			}
		}


		/// <summary>
		/// Removes service from the container.
		/// </summary>
		/// <param name="services">Services.</param>
		public void Remove(StiServiceContainer services)
		{
			foreach (StiService service in services)Remove(service);
		}

		
		public StiService this[int index]
		{
			get
			{
				return (StiService)List[index];
			}
			set
			{
				lock(List)
				{
					List[index] = value;
					BuildTypes();
				}
			}
		}


		private void BuildTypes()
		{
			if (!noTypes)
			{
				lock(typesHashtable)
				{
					typesHashtable.Clear();
					foreach (StiService service in List)
					{
						StiServiceContainer services = typesHashtable[service.ServiceType] as StiServiceContainer;
								
						if (services == null)
						{
							services = new StiServiceContainer(true);
							typesHashtable.Add(service.ServiceType, services);
						}
                        lock (services)
                        {
                            services.Add(service);
                        }
					}
				}
			}
		}
		#endregion
		
		#region Properties
		private static string[] standardAssemblies = 
        {
			"Stimulsoft.Base.dll",
			"Stimulsoft.Report.dll"
		};
		public static string[] StandardAssemblies
		{
			get
			{
				return standardAssemblies;
			}
			set
			{
				standardAssemblies = value;
			}
		}

		#endregion

		#region Methods
		/// <summary>
		/// Gets a service container that contains all standard services.
		/// </summary>
		/// <returns>Service container that contains all standard services.</returns>
		public static StiServiceContainer GetStandardServices()
		{
			var services = new StiServiceContainer();
			foreach (string assemblyName in standardAssemblies)
			{
				try
				{
					var srv = GetServicesFromAssembly(assemblyName);
					if (srv != null)services.AddRange(srv);
				}
				catch
				{
				}
			}
			return services;
		}


		public static ArrayList GetStandardServicesNames()
		{
			var list = new ArrayList();
			foreach (string assemblyName in standardAssemblies)
			{
				var srv = GetServicesNamesFromAssembly(StiAssemblyFinder.GetAssembly(assemblyName));
				list.Add(srv);
			}

			return list;
		}
	

		/// <summary>
		/// Gets array of services, that contains in this service container.
		/// </summary>
		/// <returns></returns>
		public StiService[] ToArray()
		{
			var services = new StiService[Count];
			int index = 0;
			foreach (StiService service in this)services[index++] = service;
			return services;
		}

		/// <summary>
		/// Gets a service type.
		/// </summary>
		public StiService GetService(Type serviceType)
		{
			return GetService(serviceType, false);
		}

		public StiService GetService(Type serviceType, bool getEnabled)
		{
			return GetService(serviceType, getEnabled, true);
		}

		/// <summary>
		/// Gets first service of the type from the container.
		/// </summary>
		/// <param name="serviceType">Service type.</param>
		/// <returns>Service.</returns>
		public StiService GetService(Type serviceType, bool getEnabled, bool callBeforeGetService)
		{
		    if (callBeforeGetService && BeforeGetService != null)
		    {
		        var args = new StiServiceActionEventArgs(StiServiceActionType.Get);
		        BeforeGetService(serviceType, args);
		        if (args.Processed) return args.Services != null ? args.Services.FirstOrDefault() : null;
		    }
			if (!noTypes)
			{
				StiServiceContainer services = typesHashtable[serviceType] as StiServiceContainer;
				if (services != null && services.Count > 0)
				{
					foreach (StiService service in services)
					{
						if (service.ServiceEnabled || (!getEnabled))return service;
					}
					return null;
				}
				return null;
			}
			else
			{
				foreach (StiService service in List)
				{
					if (service.ServiceType == serviceType && (service.ServiceEnabled || (!getEnabled)))
						return service;
				}
				return null;
			}
		}


		/// <summary>
		/// Gets all services of the type from the container.
		/// </summary>
		/// <param name="serviceType">Type of the returnable services.</param>
		/// <returns>Collection of services.</returns>
		public StiServiceContainer GetServices(Type serviceType)
		{
			return GetServices(serviceType, true);
		}

		public StiServiceContainer GetServices(Type serviceType, bool getEnabled)
		{
			return GetServices(serviceType, getEnabled, true);
		}

		/// <summary>
		/// Returns all services of the type from the container.
		/// </summary>
		/// <param name="serviceType">Type of the returnable services.</param>
		/// <param name="getEnabled">If true then returns only enabled services.</param>
		/// <returns>Collection of services.</returns>
		public StiServiceContainer GetServices(Type serviceType, bool getEnabled, bool callBeforeGetService)
		{
		    if (callBeforeGetService && BeforeGetService != null)
		    {
		        var args = new StiServiceActionEventArgs(StiServiceActionType.Get);
		        BeforeGetService(serviceType, args);
		        if (args.Processed)
		        {
                    var serviceContainer = new StiServiceContainer();
		            if (args.Services != null)
		            {
		                foreach (var service in args.Services)
		                {
                            serviceContainer.Add(service);
		                }
		            }
		            return serviceContainer;
		        }
		    }

			if (!noTypes)
			{
				StiServiceContainer services = typesHashtable[serviceType] as StiServiceContainer;
				if (services != null)
				{
					StiServiceContainer serv = new StiServiceContainer();
                    lock (services)
                    {
                        foreach (StiService service in services)
                        {
                            if (service.ServiceEnabled || (!getEnabled))
                            {
                                serv.Add(service);
                            }
                        }
                    }
					return serv;
				}
				else return new StiServiceContainer();
			}
			else
			{
				StiServiceContainer services = new StiServiceContainer(true);

				foreach (StiService service in List)
				{
					if (service.ServiceType == serviceType && (service.ServiceEnabled || (!getEnabled)))
						services.Add(service);
				}
				return services;
			}
		}


		/// <summary>
		/// Returns string presentation of the service.
		/// </summary>
		/// <param name="service">Service.</param>
		/// <returns>String.</returns>
		public static string GetStringFromService(StiService service)
		{
			return GetStringFromService(service.GetType());
		}


		public static string GetStringFromService(Type serviceType)
		{
			Assembly a = serviceType.Assembly;
			string ext = null;
            if (serviceType.ToString().StartsWith("Stimulsoft.", StringComparison.InvariantCulture)) ext = ".dll";
			else ext = Path.GetExtension(a.CodeBase).ToLower(System.Globalization.CultureInfo.InvariantCulture);

			return string.Format("{0}{1},{2}", a.GetName().Name, ext, serviceType);
		}

		/// <summary>
		/// Returns services which are in the specified assembly.
		/// </summary>
		/// <param name="assemblyName">Assembly.</param>
		/// <returns>Services.</returns>
		public static StiServiceContainer GetServicesFromAssembly(string assemblyName)
		{
			Assembly a = StiAssemblyFinder.GetAssembly(assemblyName);
			if (a == null)return new StiServiceContainer();
			return GetServicesFromAssembly(a);
			
		}


	    private static Type[] GetTypesFromAssembly(Assembly a)
		{
			lock (AssemblyToTypes)
			{
				Type [] typesInAssembly = AssemblyToTypes[a] as Type [];
					if (typesInAssembly == null)
					{
					    try
					    {
                            typesInAssembly = a.GetTypes();
					    }
                        catch (ReflectionTypeLoadException exception)
					    {
                            typesInAssembly = exception.Types;
					    }
						
						if (AssemblyToTypes[a] == null)AssemblyToTypes[a] = typesInAssembly;
					}
				return typesInAssembly;
			}
		}


		/// <summary>
		/// Returns services which are in the specified assembly.
		/// </summary>
		/// <param name="a">Assembly.</param>
		/// <returns>Services.</returns>
		public static StiServiceContainer GetServicesFromAssembly(Assembly a)
		{
			var serviceContainer = new StiServiceContainer();

			if (a != null)
			{
				var types = GetTypesFromAssembly(a);
				Type typeService = typeof(StiService);
				foreach (Type type in types)
				{	
					if (StiTypeFinder.FindType(type, typeService) && (!type.IsAbstract))
					{
						try
						{
							var service = StiActivator.CreateObject(type) as StiService;
							if (service != null && service.ServiceType != null)
								serviceContainer.Add(service);
						}
						catch
						{
						}
					}
				}
			}
			return serviceContainer;
		}


		public static ArrayList GetServicesNamesFromAssembly(Assembly a)
		{
			var serviceContainer = new ArrayList();

			if (a != null)
			{
				try
				{
					var types = GetTypesFromAssembly(a);
					var typeService = typeof(StiService);
					foreach (var type in types)
					{	
						try
						{
							if (StiTypeFinder.FindType(type, typeService) && (!type.IsAbstract))
							{
								serviceContainer.Add(GetStringFromService(type));
							}
						}
						catch
						{
						}
					}
				}
				catch
				{
				}
			}

			return serviceContainer;
		}

		
		/// <summary>
		/// Creates service.
		/// </summary>
		/// <param name="assemblyName">Assembly where the service is.</param>
		/// <param name="type">Service type.</param>
		/// <returns>Created service.</returns>
		public static StiService CreateService(string assemblyName, string type)
		{
			Assembly a = StiAssemblyFinder.GetAssembly(assemblyName);

			if (a != null)
			{			
				lock (AssemblyToTypes)
				{
					Type [] typesInAssembly = GetTypesFromAssembly(a);

					lock (StiTypeFinder.TypeToString)
					{
						Type tp = StiTypeFinder.TypeToString[type] as Type;
	
						if (tp == null)
						{
							foreach (Type fnTp in typesInAssembly)
							{
								string fnStr = fnTp.ToString();
								if (fnStr == type)
								{
									tp = fnTp;
									StiTypeFinder.TypeToString[type] = fnTp;
									break;
								}
							}
						}
						return (StiService)StiActivator.CreateObject(tp);
					}
				}
			}
			return null;
		}
	
		#endregion

		#region Save Load
		/// <summary>
		/// Saves container of services.
		/// </summary>
		/// <param name="tw">XmlWriter.</param>
		public void Save(XmlWriter tw)
		{
			StiObjectStringConverter converter = new StiObjectStringConverter();

			tw.WriteStartElement("Services");

			foreach (StiService service in this)
			{
				tw.WriteStartElement("service");
				
				Assembly a = service.GetType().Assembly;
				string path = Path.GetExtension(a.Location);

				tw.WriteAttributeString("assembly", a.GetName().Name + path);
				tw.WriteAttributeString("type", service.GetType().ToString());

				#region Serialize service parameters
				PropertyDescriptorCollection props = TypeDescriptor.GetProperties(service);

				if (props != null && props.Count != 0)
				{
					props = props.Sort();

					foreach (PropertyDescriptor prop in props)
					{
						StiServiceParamAttribute param = 
							prop.Attributes[typeof(StiServiceParamAttribute)] as StiServiceParamAttribute;
							
						if (param != null)
						{
							DefaultValueAttribute defaultAttr = 
								prop.Attributes[typeof(DefaultValueAttribute)] as DefaultValueAttribute;

							object value = prop.GetValue(service);

							if (defaultAttr == null || (!defaultAttr.Value.Equals(value)))
							{
								tw.WriteStartElement("property");
							
								#region Null
								if (value == null)
								{
									tw.WriteAttributeString("isNull", "True");
								}
								#endregion
								
								#region isList
								else if (value is Array)
								{
									tw.WriteAttributeString("name", prop.Name);
									tw.WriteAttributeString("isList", "True");
									tw.WriteAttributeString("count", ((Array)value).Length.ToString());
									tw.WriteAttributeString("type", value.GetType().GetElementType().ToString());

									Array array = value as Array;
									foreach (object val in array)
									{
										tw.WriteStartElement("item");
										tw.WriteString(converter.ObjectToString(val));
										tw.WriteEndElement();
									}
								}
								#endregion

								#region Simple
								else 
								{
									tw.WriteAttributeString("name", prop.Name);
									tw.WriteAttributeString("type", value.GetType().ToString());
									tw.WriteAttributeString("value", converter.ObjectToString(value));
								}
								#endregion

								tw.WriteEndElement();
							}
						}
						param = null;
					}
				}
				#endregion				

				tw.WriteEndElement();
			}
            
			tw.WriteEndElement();
		}

		
		/// <summary>
		/// Saves container of services.
		/// </summary>
		/// <param name="stream">Stream.</param>
		public void Save(Stream stream)
		{
			XmlTextWriter tw = new XmlTextWriter(stream, Encoding.UTF8);
			tw.Formatting = Formatting.Indented;
			Save(tw);
		}


		/// <summary>
		/// Saves container of services.
		/// </summary>
		/// <param name="fileName">File.</param>
		public void Save(string fileName)
		{
			CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
			try
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);

				XmlTextWriter tw = new XmlTextWriter(fileName, Encoding.UTF8);
				tw.Formatting = Formatting.Indented;
				tw.WriteStartDocument(true);
				Save(tw);
				tw.Flush();
				tw.Close();
			}
			finally
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}


		/// <summary>
		/// Loads container of services.
		/// </summary>
		/// <param name="tr">XmlReader.</param>
		public void Load(XmlReader tr)
		{
            var converter = new StiObjectStringConverter();
			StiService service = null;			
			
			tr.Read();
			while (tr.Read())
			{
				if (tr.IsStartElement()) 
				{
					#region service
					if (tr.Name == "service")
					{
						string assembly = tr.GetAttribute("assembly");
						string serviceType = tr.GetAttribute("type");

						if (assemlyToSkip[assembly.ToLower(System.Globalization.CultureInfo.InvariantCulture)] == null)
						{

							try
							{
								service = CreateService(assembly, serviceType);
								if (service != null)Add(service, false);
							}
							catch
							{
							}
						}
					}
					#endregion

					#region property
					else if (tr.Name == "property" && service != null)
					{
						string propName = tr.GetAttribute("name");
						string typeStr = tr.GetAttribute("type");
						var propInfo = service.GetType().GetProperty(propName);

						if (propInfo != null)
						{
							if (tr.GetAttribute("isNull") != null)propInfo.SetValue(service, null, null);
							else if (tr.GetAttribute("isList") != null)
							{
								int count = int.Parse(tr.GetAttribute("count"));
								Type elementType = StiTypeFinder.GetType(typeStr);
								Array list = Array.CreateInstance(elementType, count);

								int index = 0;
								while (tr.Read())
								{
									if (tr.IsStartElement())
									{
										string nm = tr.Name;
								
										string valueStr = tr.ReadString();
										object value = converter.StringToObject(valueStr, elementType);
										if (value != null)list.SetValue(value, new int[]{index++});
										if (index >= count)break;
									}
								}
								propInfo.SetValue(service, list, null);
							}
							else 
							{
								string valueStr = tr.GetAttribute("value");
								object value = converter.StringToObject(valueStr, StiTypeFinder.GetType(typeStr));
								if (value != null)propInfo.SetValue(service, value, null);
							}
						}
					}
					#endregion
				}
			}
		}


		/// <summary>
		/// Loads container of services.
		/// </summary>
		/// <param name="stream">Stream.</param>
		public void Load(Stream stream)
		{
			this.Clear();
				
			XmlTextReader tr = new XmlTextReader(stream);

			Load(tr);
			tr.Close();
		}


		/// <summary>
		/// Loads container of services.
		/// </summary>
		/// <param name="fileName">File.</param>
		public void Load(string fileName)
		{
			CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
			try
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
				this.Clear();

				XmlTextReader tr = new XmlTextReader(fileName);

				tr.Read();
				tr.Read();

				Load(tr);
				tr.Close();
			}
			finally
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}

		#endregion

		#region Events
        public event StiServiceActionHandler BeforeGetService;
		#endregion

		/// <summary>
		/// Creates a new object of the type StiServiceContainer.
		/// </summary>
		public StiServiceContainer() : this(false)
		{
		}


		/// <summary>
		/// Creates a new object of the type StiServiceContainer.
		/// </summary>
		private StiServiceContainer(bool noTypes)
		{
			this.noTypes = noTypes;
		}


		static StiServiceContainer()
		{
			var notNull = "NotNull";
			assemlyToSkip["stimulsoft.report.export.images.dll"] =			notNull;
			assemlyToSkip["stimulsoft.report.export.htmlexport.dll"] =		notNull;
			assemlyToSkip["stimulsoft.report.export.txtexport.dll"] =		notNull;
			assemlyToSkip["stimulsoft.report.export.excelxmlexport.dll"] =	notNull;
			assemlyToSkip["stimulsoft.report.export.excelhtmlexport.dll"] = notNull;
			assemlyToSkip["stimulsoft.report.export.xmlexport.dll"] =		notNull;
			assemlyToSkip["stimulsoft.report.export.csvexport.dll"] =		notNull;
			assemlyToSkip["stimulsoft.report.export.rtfexport.dll"] =		notNull;
			assemlyToSkip["stimulsoft.report.export.pdfexport.dll"] =		notNull;
		}
	}
}