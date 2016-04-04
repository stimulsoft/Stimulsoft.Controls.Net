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
using System.CodeDom.Compiler;
using System.Drawing.Text;
using System.Drawing;
using System.Collections;
using System.Security.Policy;

namespace Stimulsoft.Base
{		
	public static class StiFontCollection
    {
        #region Fields.Static
        private static object lockObject = new object();
        private static Hashtable fontFamilyHash = new Hashtable();
        private static Hashtable fontPathHash = new Hashtable();
        #endregion

        #region Properties.Static
        private static PrivateFontCollection instance = new PrivateFontCollection();
        public static PrivateFontCollection Instance
        {
            get
            {
                if (instance == null)
                    instance = new PrivateFontCollection();

                return instance;
            }
        }
        #endregion

        #region Methods.Static
        public static void AddFontFile(string fileName)
        {
            lock (lockObject)
            {
                Instance.AddFontFile(fileName);
                fontPathHash[Instance.Families.LastOrDefault().Name] = fileName;
            }
        }

        public static List<FontFamily> GetFontFamilies()
        {
            lock (lockObject)
            {
                var fonts = FontFamily.Families.ToList();
                fonts.AddRange(instance.Families);
                return fonts.OrderBy(f => f.Name).ToList();
            }
        }

        public static FontFamily GetFontFamily(string fontName)
        {
            lock (lockObject)
            {
                var fontFamily = fontFamilyHash[fontName] as FontFamily;
                if (fontFamily != null) return fontFamily;

                fontFamily = GetFontFamilies().FirstOrDefault(f => f.Name == fontName);
                if (fontFamily == null)
                {
                    using (var font = new Font(fontName, 1f, FontStyle.Regular))
                    {
                        fontFamily = font.FontFamily;
                    }
                }

                fontFamilyHash[fontName] = fontFamily;
                return fontFamily;
            }
        }

        public static bool IsCustomFont(string fontName)
        {
            return Instance.Families.Any(f => f.Name == fontName);
        }

        public static string GetCustomFontPath(string fontName)
        {
            var path = fontPathHash[fontName] as string;
            if (path == null) return null;
            if (!path.StartsWith("file:///")) path = "file:///" + path;

            return path;
        }

        public static Font CreateFont(string fontName, float fontSize, FontStyle fontStyle)
        {
            return new Font(GetFontFamily(fontName), fontSize, fontStyle);
        }
        #endregion
    }
}
