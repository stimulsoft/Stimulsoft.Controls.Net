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
	/// Helps a converts and works with objects.
	/// </summary>
	public class StiObjectConverter 
	{
		/// <summary>
		/// Convert object to Decimal.
		/// </summary>
		/// <param name="value">Object for converting.</param>
		/// <returns>Converted Decimal value.</returns>
		public static Decimal ConvertToDecimal(object value)
		{
			try
			{
				if (value == null)
				{
					return 0M;
				}
				else if (value is string)
				{
                    if (((string)value).Length == 0)
                        return 0M;
					return Decimal.Parse(((string)value).Replace(".", ",").Replace(",", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator));
				}
				else 
				{
					return (Decimal)Convert.ChangeType(value, typeof(Decimal));
				}
			}
			catch
			{
				return 0;
			}
		}


		/// <summary>
		/// Convert object to Double.
		/// </summary>
		/// <param name="value">Object for converting.</param>
		/// <returns>Converted Double value.</returns>
		public static Double ConvertToDouble(object value)
		{
			try
			{
				if (value == null)
				{
					return 0d;
				}
				else if (value is string)
				{
                    if (string.IsNullOrWhiteSpace((string)value)) return 0;
                    return Double.Parse(((string)value).Replace(".", ",").Replace(",", System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                }
				else 
				{
					return (Double)Convert.ChangeType(value, typeof(Double));
				}
			}
			catch
			{
				return 0;
			}
		}


		/// <summary>
		/// Convert object to Int64.
		/// </summary>
		/// <param name="value">Object for converting.</param>
		/// <returns>Converted Int64 value.</returns>
		public static Int64 ConvertToInt64(object value)
		{
			try
			{
				if (value == null)
				{
					return 0;
				}
				else if (value is string)
				{
					return Int64.Parse((string)value);
				}
				else 
				{
					return (Int64)Convert.ChangeType(value, typeof(Int64));
				}
			}
			catch
			{
				return 0;
			}
		}		


		/// <summary>
		/// Convert array of bytes to string.
		/// </summary>
        /// <param name="bytes">Array of bytes for converting.</param>
		/// <returns>Converted string.</returns>
		public static string ConvertToString(byte[] bytes)
		{
			if (bytes != null)
			{
				StringBuilder sb = new StringBuilder();

				foreach (byte b in bytes)
				{
					sb.Append(b.ToString("x2"));
				}
				return sb.ToString();
			}

			return string.Empty;
		}		
	}
}