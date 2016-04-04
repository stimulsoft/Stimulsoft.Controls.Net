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
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace Stimulsoft.Base
{
	/// <summary>
	/// Helps to create objects of the specified type.
	/// </summary>
	public sealed class StiActivator
	{
        public static bool AllowTypeNotFoundException = false;

		/// <summary>
		/// Creates the object of the specified type.
		/// </summary>
		/// <param name="type">The type of the created object.</param>
		/// <returns>The Created object.</returns>
		public static object CreateObject(Type type)
		{
			if (type == null)return null;
			return Activator.CreateInstance(type);
		}

		/// <summary>
		/// Creates the object of the specified type.
		/// </summary>
		/// <param name="typeName">The type of the created object.</param>
		/// <returns>The Created object.</returns>
		public static object CreateObject(string typeName)
		{
			return CreateObject(typeName, new object[0]);
		}

		/// <summary>
		/// Creates the object of the specified type.
		/// </summary>
		/// <param name="type">The type of the created object.</param>
		/// <returns>The Created object.</returns>
		public static object CreateObject(Type type, object[] arguments)
		{
			return CreateObject(type.ToString(), arguments);
		}
	
		/// <summary>
		/// Creates the object of the specified type.
		/// </summary>
		/// <param name="typeName">The type of the created object.</param>
		/// <returns>The Created object.</returns>
		public static object CreateObject(string typeName, object[] arguments)
		{
            Type type = StiTypeFinder.GetType(typeName);
            if (type != null)
            {
                try
                {
                    return Activator.CreateInstance(type, arguments);
                }
                catch (Exception exp)
                {
                    Exception e = exp.InnerException != null ? exp.InnerException : exp;
                    throw e;
                }
            }
            else
            {
                if (AllowTypeNotFoundException)
                    throw new Exception("The type " + typeName + " not found!");
                else
                    return null;
            }
		}		
	}
}
