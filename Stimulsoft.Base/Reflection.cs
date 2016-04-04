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
using System.Reflection;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text;

namespace Stimulsoft.Base
{
    public class Reflection
    {
        public static object Create(Assembly assembly, string type, params object[] args)
        {
            return Activator.CreateInstance(assembly.GetType(type), args);
        }

        public static T InvokeMethod<T>(object value, string methodName, params object[] args)
        {
            return (T) InvokeMethod(value, methodName, args);
        }

        public static object InvokeMethod(object value, string methodName, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                var types = args.Select(a => a.GetType()).ToArray();

                return value.GetType().GetMethod(methodName, types).Invoke(value, args);
            }
            return value.GetType().GetMethod(methodName).Invoke(value, args);
        }

        public static T GetPropertyValue<T>(object value, string propertyName)
        {
            return (T)GetPropertyValue(value, propertyName);
        }

        public static object GetPropertyValue(object value, string propertyName)
        {
            var prop = value.GetType().GetProperty(propertyName);

            return prop != null ? prop.GetValue(value, null) : null;
        }

        public static void SetPropertyValue(object value, string propertyName, object propValue)
        {
            value.GetType().GetProperty(propertyName).SetValue(value, propValue, null);
        }
    }
}
