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
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Drawing;
using Stimulsoft.Base;

namespace Stimulsoft.Base
{
    public static class DecimalHelper
	{
		public static decimal Parse(string value)
		{
            if (value == "0")
                return 0m;
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
                return ((decimal)long.Parse(after.ToString())) / dec;
            else if (before.Length != 0 && after.Length != 0)
                return (decimal)long.Parse(before.ToString()) + ((decimal)long.Parse(after.ToString())) / dec;
            else if (before.Length != 0 && after.Length == 0)
                return (decimal)long.Parse(before.ToString());
            else
                return 0m;
		}
    }
}
