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
using System.Xml;
using System.IO;
using System.Text;
using System.Collections;
using System.Drawing;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base
{
	/// <summary>
	/// This class helps with searialing/deserializing state of object to string.
	/// </summary>
	public class StiObjectStateSaver 
	{
		/// <summary>
		/// Write object state to string.
		/// </summary>
		/// <param name="obj">Object which state will be save to string.</param>
		/// <returns>String which contains string representation of object.</returns>
		public static string WriteObjectStateToString(object obj)
		{
			return WriteObjectStateToString(obj, new StiObjectStringConverter());
		}

		/// <summary>
		/// Object which state will be save to string.
		/// </summary>
		/// <param name="obj">Object which state will be save to string.</param>
		/// <param name="converter">Object converter which used for writing to string.</param>
		/// <returns>String which contain string representation of object.</returns>
		public static string WriteObjectStateToString(object obj, StiObjectStringConverter converter)
		{
			CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
			try
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);

				StringBuilder sb = new StringBuilder();
				using (StringWriter writer = new StringWriter(sb))
				{				
					StiSerializing sr = new StiSerializing(converter);
					sr.SortProperties = false;
					sr.CheckSerializable = true;
					sr.Serialize(obj, writer, "State", StiSerializeTypes.SerializeToAll);
				}
				return sb.ToString();
			}
			finally
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}


		/// <summary>
		/// Read object state from string.
		/// </summary>
		/// <param name="obj">Object for storing state.</param>
		/// <param name="str">String which contain state of object.</param>
		public static void ReadObjectStateFromString(object obj, string str)
		{
			ReadObjectStateFromString(obj, str, new StiObjectStringConverter());
		}
		
		/// <summary>
		/// Read object state from string.
		/// </summary>
		/// <param name="obj">Object for storing state.</param>
		/// <param name="str">String which contain state of object.</param>
		/// <param name="converter">Object converter which used for writing to string.</param>
		public static void ReadObjectStateFromString(object obj, string str, StiObjectStringConverter converter)
		{
			CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
			try
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);

				using (StringReader reader = new StringReader(str))
				{				
					StiSerializing sr = new StiSerializing(converter);
					sr.Deserialize(obj, reader, "State");
				}
			}
			finally
			{
                System.Threading.Thread.CurrentThread.CurrentCulture = currentCulture;
			}
		}
	}
}
