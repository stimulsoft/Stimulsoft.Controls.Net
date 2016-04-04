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
	/// Class realize methods for saving metafile.
	/// </summary>
	public sealed class StiMetafileSaver
	{
		public static void Save(Stream stream, Metafile metafile)
		{
			Metafile mf = null;
			using (Bitmap bmp = new Bitmap(1, 1))
			using (Graphics grfx = Graphics.FromImage(bmp))
			{
				IntPtr ipHdc = grfx.GetHdc();
                GraphicsUnit unit = GraphicsUnit.Pixel;
				mf = new Metafile(stream, ipHdc, metafile.GetBounds(ref unit),  MetafileFrameUnit.Pixel);
				grfx.ReleaseHdc(ipHdc);
			}

			using (Graphics grfx = Graphics.FromImage(mf))
			{
				//grfx.DrawImage(metafile, 0, 0, metafile.Width, metafile.Height);
                grfx.DrawImage(
                    metafile,
                    metafile.GetMetafileHeader().Bounds.Left,
                    metafile.GetMetafileHeader().Bounds.Top,
                    metafile.Width,
                    metafile.Height);                
			}
		}

		public static void Save(string fileName, Metafile metafile)
		{
			StiFileUtils.ProcessReadOnly(fileName);
			FileStream stream = new FileStream(fileName, FileMode.Create);
			Save(stream, metafile);
			stream.Flush();
			stream.Close();
		}
		
	}
}
