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
using System.Collections;
using System.Reflection;

namespace Stimulsoft.Base
{
	sealed public class StiFileUtils
	{
		public static bool ProcessReadOnly(string path)
		{
			if (File.Exists(path))
			{
				FileAttributes attr = File.GetAttributes(path);
				if ((attr & FileAttributes.ReadOnly) > 0)
				{
					File.SetAttributes(path, (FileAttributes)(attr - FileAttributes.ReadOnly));
					return true;
				}
			}
			return false;
		}

		private StiFileUtils()
		{
		}
	}
}
