#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports												}
{	                         										}
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
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls
{
	[ToolboxItem(false)]
	public class StiWebBrowser : AxHost
	{
		#region Fields
		private object ocx;
		#endregion 

		#region Properties
		public virtual string LocationURL
		{
			get
			{
				try
				{
					Type type = this.ocx.GetType();
					return type.InvokeMember("LocationURL", BindingFlags.GetProperty, null, this.ocx,
						new object[] { }) as string;
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		#endregion

		#region Methods
		public void DoBack()
		{
			try
			{
				if (this.ocx != null)
				{
					Type type = this.ocx.GetType();
					type.InvokeMember("GoBack", BindingFlags.InvokeMethod, null, this.ocx,
						new object[] { });
				}
			}
			catch
			{
			}
		}

		
		public void DoForward()
		{
			try
			{
				if (this.ocx != null)
				{
					Type type = this.ocx.GetType();
					type.InvokeMember("GoForward", BindingFlags.InvokeMethod, null, this.ocx,
						new object[] { });
				}
			}
			catch
			{
			}
		}

		
		public void DoRefresh()
		{
			try
			{
				if (this.ocx != null)
				{
					Type type = this.ocx.GetType();
					type.InvokeMember("Refresh", BindingFlags.InvokeMethod, null, this.ocx,
						new object[] { });
				}
			}
			catch
			{
			}
		}

		
		public void DoStop()
		{
			try
			{
				if (this.ocx != null)
				{
					Type type = this.ocx.GetType();
					type.InvokeMember("Stop", BindingFlags.InvokeMethod, null, this.ocx,
						new object[] { });
				}
			}
			catch
			{
			}
		}


		protected override void AttachInterfaces()
		{
			try
			{
				this.ocx = base.GetOcx();
			}
			catch
			{
			}
		}


		public void DoNavigate(string url)
		{
			if (this.ocx != null)
			{
				object obj = null;
				this.ocx.GetType().InvokeMember("Navigate2", BindingFlags.InvokeMethod, null, this.ocx, 
					new object[] { url, obj, obj, obj, obj });
			}
		}
		#endregion

		public StiWebBrowser () : base("8856f961-340a-11d0-a96b-00c04fd705a2")
		{
		}
	}
}
