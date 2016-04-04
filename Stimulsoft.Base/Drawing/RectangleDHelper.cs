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
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Drawing;
using Stimulsoft.Base;

namespace Stimulsoft.Base.Drawing
{
	public class RectangleDHelper : TypeConverter
	{
		public static string ConvertRectangleDToString(RectangleD rectangle)
		{
			return
                string.Format("{0},{1},{2},{3}",
				rectangle.X,
				rectangle.Y,
				rectangle.Width,
				rectangle.Height);
		}

        public static string ConvertRectangleDToString(RectangleD rectangle, char separator)
        {
            return
                string.Format("{0}{1}{2}{3}{4}{5}{6}",
                rectangle.X,
                separator,
                rectangle.Y,
                separator,
                rectangle.Width,
                separator,
                rectangle.Height);
        }

		
		public static RectangleD ConvertStringToRectangleD(string str, char separator)
		{
			string[] words = str.Split(new char[]{separator});
            
            return new RectangleD(
				double.Parse(words[0]),
				double.Parse(words[1]),
				double.Parse(words[2]),
				double.Parse(words[3]));
		}
	}
}
