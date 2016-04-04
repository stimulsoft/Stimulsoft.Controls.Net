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
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.Collections;

namespace Stimulsoft.Base.Design
{	
	/// <summary>
	/// Attribute with serialization parameters.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class StiBrowsableAttribute : Attribute
	{
		private bool browsable = false;
		public bool Browsable
		{
			get
			{
				return browsable;
			}
			set
			{
				browsable = value;
			}

		}


		/// <summary>
		/// Creates a new object of the type StiBrowsableAttribute. 
		/// </summary>
		public StiBrowsableAttribute() : this(true)
		{
		}
		
		
		/// <summary>
		/// Creates a new object of the type StiBrowsableAttribute. 
		/// </summary>
		public StiBrowsableAttribute(bool browsable) 
		{
			this.browsable = browsable;
		}
	}

}
