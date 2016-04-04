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
	public class StiImageUtils
	{
		public static Bitmap GetImage(Type type, string imageName)
		{
			return GetImage(type, imageName, true);
		}


		/// <summary>
		/// Gets the Image object associated with Type.
		/// </summary>
		/// <param name="type">The type with which Image object is associated.</param>
		/// <param name="imageName">The name of the image file to look for.</param>
		/// <returns>The Image object.</returns>
		public static Bitmap GetImage(Type type, string imageName, bool makeTransparent)
		{
			return GetImage(type.Module.Assembly, imageName, makeTransparent);
		}


		public static Bitmap GetImage(string assemblyName, string imageName)
		{
			return GetImage(assemblyName, imageName, true);
		}

		
		/// <summary>
		/// Gets the Image object placed in assembly.
		/// </summary>
		/// <param name="assemblyName">The name of assembly in which the Cursor object is placed.</param>
		/// <param name="imageName">The name of the image file to look for.</param>
		/// <returns>The Image object.</returns>
		public static Bitmap GetImage(string assemblyName, string imageName, bool makeTransparent)
		{			
			Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly a in assemblys)
			{
                if (StiBaseOptions.FullTrust)
                {
                    string str = a.GetName().Name;
                    if (str == assemblyName) return GetImage(a, imageName, makeTransparent);
                }
                else
                {
                    string str = a.FullName;
                    int pos = str.IndexOf(',');
                    if (pos > -1) str = str.Substring(0, pos);
                    if (str == assemblyName) return GetImage(a, imageName, makeTransparent);
                }
			}
			
			throw new Exception(string.Format("Can't find assembly '{0}'", assemblyName));
		}

		
		public static Bitmap GetImage(Assembly imageAssembly, string imageName)
		{
			return GetImage(imageAssembly, imageName, true);
		}


		/// <summary>
		/// Gets the Image object placed in assembly.
		/// </summary>
		/// <param name="imageAssembly">Assembly in which is the Image object is placed.</param>
		/// <param name="imageName">The name of the image file to look for.</param>
		/// <returns>The Image object.</returns>
		public static Bitmap GetImage(Assembly imageAssembly, string imageName, bool makeTransparent)
		{
			Stream stream = imageAssembly.GetManifestResourceStream(imageName);
			if (stream != null)
			{
				Bitmap image = new Bitmap(stream);
				if (makeTransparent)StiImageUtils.MakeImageBackgroundAlphaZero(image);
				return image;			
			}
			else throw new Exception(
					 string.Format("Can't find image '{0}' in resources of '{1}' assembly", 
					 imageName, imageAssembly.ToString()));
		}


		public static void MakeImageBackgroundAlphaZero(Bitmap image)
		{
			Color color1 = image.GetPixel(0, (image.Height - 1));
			image.MakeTransparent();
			Color color2 = Color.FromArgb(0, color1);
			image.SetPixel(0, (image.Height - 1), color2);
		}

		
		public static Bitmap ReplaceImageColor(Bitmap bmp, Color colorForReplace, Color replacedColor)
		{
			Bitmap bufferImage = new Bitmap(bmp.Width, bmp.Height);
			Graphics g = Graphics.FromImage(bufferImage);

			ImageAttributes imageAttr = new ImageAttributes();
			ColorMap map = new ColorMap();
			map.OldColor = replacedColor;
			map.NewColor = colorForReplace;
			imageAttr.SetRemapTable(new ColorMap[]{map});

			g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 
				0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttr);

			g.Dispose();
			return bufferImage;
		}

		
		public static Bitmap ConvertToDisabled(Bitmap bmp)
		{
			Bitmap bufferImage = new Bitmap(bmp.Width, bmp.Height);
			Graphics g = Graphics.FromImage(bufferImage);

			ImageAttributes imageAttr = new ImageAttributes();
        
			ColorMatrix disableMatrix = new ColorMatrix(
				new float[][]{
								 new float[]{0.3f,0.3f,0.3f,0,0},
								 new float[]{0.59f,0.59f,0.59f,0,0},
								 new float[]{0.11f,0.11f,0.11f,0,0},
								 new float[]{0,0,0,0.4f,0,0},
								 new float[]{0,0,0,0,0.4f,0},
								 new float[]{0,0,0,0,0,0.4f}
							 });

			imageAttr.SetColorMatrix(disableMatrix);

			g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 
				0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttr);

			g.Dispose();
			return bufferImage;
		}

		
		public static Bitmap ConvertToGrayscale(Bitmap bmp)
		{
			Bitmap bufferImage = new Bitmap(bmp.Width, bmp.Height);
			Graphics g = Graphics.FromImage(bufferImage);

			ColorMatrix grayscaleMatrix = new ColorMatrix(
				new float[][]{
								 new float[]{0.3f,0.3f,0.3f,0,0},
								 new float[]{0.59f,0.59f,0.59f,0,0},
								 new float[]{0.11f,0.11f,0.11f,0,0},
								 new float[]{0,0,0,1,0,0},
								 new float[]{0,0,0,0,1,0},
								 new float[]{0,0,0,0,0,1}
							 });			


			ImageAttributes imageAttr = new ImageAttributes();
			imageAttr.SetColorMatrix(grayscaleMatrix);
			g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 
				0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttr);

			g.Dispose();
			return bufferImage;
		}
	}
}
