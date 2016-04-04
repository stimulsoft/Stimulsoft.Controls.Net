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
using System.ComponentModel;
using System.IO;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Services;


namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Class realize methods for conversion Metafile to string and string to Metafile.
	/// </summary>
	public sealed class StiMetafileConverter
	{
		/// <summary>
		/// Convert Metafile to String.
		/// </summary>
		/// <param name="metafile">Metafile for converting.</param>
		/// <returns>Result string.</returns>
		public static string MetafileToString(Metafile metafile)
		{
			byte [] bytes = MetafileToBytes(metafile);
			return Convert.ToBase64String(bytes);
		}

		/// <summary>
		/// Convert Metafile to Bytes.
		/// </summary>
		public static byte[] MetafileToBytes(Metafile metafile)
		{
			MemoryStream ms = new MemoryStream();
			
			StiMetafileSaver.Save(ms, metafile);		
			ms.Flush();
			byte[] bytes = ms.ToArray();
			ms.Close();
			return bytes;
		}


		/// <summary>
		/// Convert Bytes to Metafile.
		/// </summary>
		public static Metafile BytesToMetafile(byte[] bytes)
		{
			MemoryStream stream = new MemoryStream(bytes);
			
			Metafile mf = Metafile.FromStream(stream) as Metafile;
			
			return mf;
		}
		
		
		/// <summary>
		/// Convert String to Metafile.
		/// </summary>
		public static Metafile StringToMetafile(string str)
		{
			byte[] bytes = Convert.FromBase64String(str);
			return BytesToMetafile(bytes);
		}
	}
}
