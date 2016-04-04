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
	/// Class realize methods for conversion image to string and string to image.
	/// </summary>
	public sealed class StiImageConverter
	{
        /// <summary>
        /// Convert Image to packed String.
        /// </summary>
        /// <param name="image">Image for converting.</param>
        /// <returns>Result string.</returns>
        public static string ImageToPackedString(Image image)
        {
            if (image == null) return string.Empty;

            byte[] bytes = ImageToBytes(image);
            return StiGZipHelper.Pack(Convert.ToBase64String(bytes));
        }

        /// <summary>
        /// Convert packed String to Image.
        /// </summary>
        /// <param name="str">String for converting.</param>
        /// <returns>Result Image.</returns>
        public static Image PackedStringToImage(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;

            byte[] bytes = Convert.FromBase64String(StiGZipHelper.Unpack(str));
            return BytesToImage(bytes);
        }
                
		/// <summary>
		/// Convert Image to String.
		/// </summary>
		/// <param name="image">Image for converting.</param>
		/// <returns>Result string.</returns>
		public static string ImageToString(Image image)
		{
			if (image == null) return string.Empty;

			//byte [] bytes = ImageToBytes(image);
            byte[] bytes = new byte[0];

            lock (image)    //fix for multitreading
            {
                TypeConverter imageConverter = TypeDescriptor.GetConverter(typeof(Image));
                try
                {
                    bytes = (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
                }
                catch
                {
                    try
                    {
                        using (Bitmap bmp = new Bitmap(image.Width, image.Height, image.PixelFormat))
                        {
                            Graphics gr = Graphics.FromImage(bmp);
                            gr.DrawImage(image, 0, 0, image.Width, image.Height);
                            gr.Dispose();
                            bytes = (byte[])imageConverter.ConvertTo(bmp, typeof(byte[]));
                        }
                    }
                    catch
                    {
                    }
                }
            }

			return Convert.ToBase64String(bytes);
		}

		/// <summary>
		/// Convert Image to Bytes.
		/// </summary>
		public static byte[] ImageToBytes(Image image)
		{
            if (image == null) return new byte[0];

            TypeConverter imageConverter = TypeDescriptor.GetConverter(typeof(Image));
            try
            {
                return (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
            }
            catch
            {
                try
                {
                    using (Bitmap bmp = new Bitmap(image.Width, image.Height, image.PixelFormat))
                    {
                        Graphics gr = Graphics.FromImage(bmp);
                        gr.DrawImage(image, 0, 0, image.Width, image.Height);
                        gr.Dispose();
                        return (byte[])imageConverter.ConvertTo(bmp, typeof(byte[]));
                    }
                }
                catch
                {
                }
            }
            return new byte[0];
		}


		/// <summary>
		/// Convert Bytes to Image.
		/// </summary>
		public static Image BytesToImage(byte[] bytes)
		{
			if (bytes == null || bytes.Length == 0)return null;

			MemoryStream stream = new MemoryStream(bytes);
			Image image = Image.FromStream(stream);			
			return image;
		}

		
		/// <summary>
		/// Convert String to Image.
		/// </summary>
		/// <param name="str">String for converting.</param>
		/// <returns>Result Image.</returns>
		public static Image StringToImage(string str)
		{
			if (string.IsNullOrWhiteSpace(str))return null;

            if (str.StartsWith("data:image"))
            {
                int pos = str.LastIndexOf(',');
                if (pos > 0)
                {
                    str = str.Substring(pos + 1);
                }
            }

			byte[] bytes = Convert.FromBase64String(str);
			return BytesToImage(bytes);
		}
	}
}
