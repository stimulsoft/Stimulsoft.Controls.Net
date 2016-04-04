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

using System.Text;

namespace Stimulsoft.Base
{
    public static class FloatHelper
	{
		public static float Parse(string value)
		{
            if (value == "0")
                return 0f;
            var before = new StringBuilder();
            var after = new StringBuilder();
            int dec = 1;

            bool isBefore = true;
            for (int index = 0; index < value.Length; index++)
            {
                char chr = value[index];
                if (chr == ',' || chr == '.')
                {
                    isBefore = false;
                }
                else
                {
                    if (isBefore)
                        before = before.Append(chr);
                    else
                    {
                        after = after.Append(chr);
                        dec *= 10;
                    }
                }
            }
            if (before.Length == 0 && after.Length != 0)
                return ((float)long.Parse(after.ToString())) / dec;
            else if (before.Length != 0 && after.Length != 0)
                return (float)long.Parse(before.ToString()) + ((float)long.Parse(after.ToString())) / dec;
            else if (before.Length != 0 && after.Length == 0)
                return (float)long.Parse(before.ToString());
            else
                return 0f;
		}
    }
}
