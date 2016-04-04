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
using System.Reflection;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base
{
	/// <summary>
	/// Class contains statistic methods to find types.
	/// </summary>
	public sealed class StiTypeFinder
	{
		#region Fields
		internal static Hashtable TypeToString = new Hashtable();
		internal static Hashtable StringToType = new Hashtable();
		private static Hashtable FindTypes = new Hashtable();
		#endregion

		#region Events
		public static event StiTypeNotFoundEventHandler TypeNotFound;
        public static event EventHandler<StiTypeEventArgs> TypeResolve;
		#endregion

		#region Methods
		/// <summary>
		/// Returns the type from its string representation.
		/// </summary>
		public static Type GetType(string typeName)
		{	
			lock (TypeToString)
			{
                if (TypeResolve != null)
                {
                    var args = new StiTypeEventArgs(typeName);
                    TypeResolve(null, args);
                    if (args.Type != null)
                        return args.Type;
                }

				if (!string.IsNullOrEmpty(typeName))
				{
					Type type = StringToType[typeName] as Type;
					if (type != null)return type;

					type = Type.GetType(typeName);
					if (type == null)
					{
						var assemblys = AppDomain.CurrentDomain.GetAssemblies();
						foreach (Assembly assembly in assemblys)
						{
							type = assembly.GetType(typeName);
							if (type != null)
							{
								if (StringToType[typeName] == null)StringToType[typeName] = type;
								return type;
							}
						}
					}
					else 
					{
						if (StringToType[typeName] == null)
						{
							lock (StringToType)StringToType[typeName] = type;
						}
						return type;
					}
				}

				var e = new StiTypeNotFoundEventArgs(typeName);
				if (TypeNotFound != null)TypeNotFound(typeName, e);
				return e.CreatedObject as Type;
			}
		}


	    private static void AddTypeFF(Type exType, Type typeForFinding, bool result)
	    {
	        lock (FindTypes)
	        {
	            if (exType == null || typeForFinding == null) return;

	            Hashtable ff = FindTypes[exType] as Hashtable;
	            if (ff == null)
	            {
	                ff = new Hashtable();
	                FindTypes[exType] = ff;
	            }
	            if (!ff.ContainsKey(typeForFinding)) ff[typeForFinding] = result;
	        }
	    }


		private static object GetTypeFF(Type exType, Type typeForFinding)
		{
			if (exType == null)return null;
			Hashtable ff = FindTypes[exType] as Hashtable;
			if (ff == null)return null;
			return ff[typeForFinding];
		}

		
		/// <summary>
		/// Finds in the type exType the type findType.
		/// </summary>
		/// <param name="exType">Examined type.</param>
		/// <param name="typeForFinding">Type for finding.</param>
		/// <returns>true, if type is not found.</returns>
		public static bool FindType(Type exType, Type typeForFinding)
		{
			if (exType == null)return false;
			if (typeForFinding == typeof(object))return true;

			object result = GetTypeFF(exType, typeForFinding);
			if (result != null)return (bool)result;

			while (exType != typeof(object))
			{
				if (exType == typeForFinding)
				{
					AddTypeFF(exType, typeForFinding, true);
					return true;
				}
				if (exType.BaseType == null)
				{
					AddTypeFF(exType, typeForFinding, false);
					return false;
				}
				exType = exType.BaseType;
			}
			AddTypeFF(exType, typeForFinding, false);
			return false;
		}

		
		/// <summary>
		/// Finds in the type exType interface findType.
		/// </summary>
		/// <param name="exType">Examined type.</param>
		/// <param name="interfaceForFinding">Interface for finding.</param>
		/// <returns>true, if interface is not found.</returns>
		public static bool FindInterface(Type exType, Type interfaceForFinding)
		{

			if (exType == null)return false;

			object result = GetTypeFF(exType, interfaceForFinding);
			if (result != null)return (bool)result;

			while (exType != typeof(object))
			{
				if (exType == null)
				{
					AddTypeFF(exType, interfaceForFinding, false);
					return false;
				}
				Type[] interfaces = exType.GetInterfaces();
				foreach (Type inter in interfaces)
				{
					if (inter == interfaceForFinding)
					{
						AddTypeFF(exType, interfaceForFinding, true);
						return true;
					}
				}
				exType = exType.BaseType;
			}
			AddTypeFF(exType, interfaceForFinding, false);
			return false;
		}
		#endregion
	}
}
