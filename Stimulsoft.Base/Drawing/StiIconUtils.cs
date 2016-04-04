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
using System.IO;
using System.Reflection;
using System.Resources;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Stimulsoft.Base.Drawing
{
	public class StiIconUtils
	{
		/// <summary>
		/// Gets the Icon object associated with Type.
		/// </summary>
		/// <param name="type">The type with which the Icon object is associated.</param>
		/// <param name="iconName">The name of the icon file to look for.</param>
		/// <returns>The Icon object.</returns>
		public static Icon GetIcon(Type type, string iconName)
		{
			return GetIcon(type.Module.Assembly, iconName);
		}


		/// <summary>
		/// Gets the Icon object placed in assembly.
		/// </summary>
		/// <param name="assemblyName">The name of assembly in which the Icon object is placed.</param>
		/// <param name="iconName">The name of the Icon file to look for.</param>
		/// <returns>The Icon object.</returns>
		public static Icon GetIcon(string assemblyName, string iconName)
		{
			Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly a in assemblys)
			{
				if (a.GetName().Name == assemblyName)return GetIcon(a, iconName);
			}

			throw new Exception(string.Format("Can't find assembly '{0}'", assemblyName));
		}


		/// <summary>
		/// Gets the Icon object placed in assembly.
		/// </summary>
		/// <param name="cursorAssembly">Assembly in which the Icon object is placed.</param>
		/// <param name="iconName">The name of the Icon file to look for.</param>
		/// <returns>The Icon object.</returns>
		public static Icon GetIcon(Assembly cursorAssembly, string iconName)
		{
			Stream stream = cursorAssembly.GetManifestResourceStream(iconName);
			if (stream != null)
			{
				Icon icon = new Icon(stream);
				return icon;
			}
			else throw new Exception(string.Format("Can't find icon '{0}' in resources", iconName));
		}
	}
}
