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
	public class StiCursorUtils
	{
		/// <summary>
		/// Gets the Cursor object associated with Type.
		/// </summary>
		/// <param name="type">The type with which the Cursor object is associated.</param>
		/// <param name="cursorName">The name of the cursor file to look for.</param>
		/// <returns>The Cursor object.</returns>
		public static Cursor GetCursor(Type type, string cursorName)
		{
			return GetCursor(type.Module.Assembly, cursorName);
		}


		/// <summary>
		/// Gets the Cursor object placed in assembly.
		/// </summary>
		/// <param name="assemblyName">The name of assembly in which the Cursor object is placed.</param>
		/// <param name="cursorName">The name of the cursor file to look for.</param>
		/// <returns>The Cursor object.</returns>
		public static Cursor GetCursor(string assemblyName, string cursorName)
		{
			Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly a in assemblys)
			{
				if (a.GetName().Name == assemblyName)return GetCursor(a, cursorName);
			}

			throw new Exception(string.Format("Can't find assembly '{0}'", assemblyName));
		}


		/// <summary>
		/// Gets the Cursor object placed in assembly.
		/// </summary>
		/// <param name="cursorAssembly">Assembly in which the Cursor object is placed.</param>
		/// <param name="cursorName">The name of the cursor file to look for.</param>
		/// <returns>The Cursor object.</returns>
		public static Cursor GetCursor(Assembly cursorAssembly, string cursorName)
		{
			Stream stream = cursorAssembly.GetManifestResourceStream(cursorName);
			if (stream != null)
			{
				Cursor cursor = new Cursor(stream);
				return cursor;
			}
			else throw new Exception(string.Format("Can't find cursor '{0}' in resources", cursorName));
		}
	}
}
