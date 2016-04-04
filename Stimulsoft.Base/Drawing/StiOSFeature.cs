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
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Summary description for StiOSFeature.
	/// </summary>
	[System.Security.SuppressUnmanagedCodeSecurity]
	public sealed class StiOSFeature
	{
		#region Properties
		private static bool isLayeredWindows = true;
		public static bool IsLayeredWindows
		{
			get
			{
				return isLayeredWindows;
			}
		}

		private static bool isThemes = true;
		public static bool IsThemes
		{
			get
			{
				return isThemes;
			}
		}
		#endregion


		static StiOSFeature()
		{
			isLayeredWindows = OSFeature.Feature.GetVersionPresent(OSFeature.LayeredWindows) != null;
			isThemes = OSFeature.Feature.GetVersionPresent(OSFeature.Themes) != null;
		}
	}
}
