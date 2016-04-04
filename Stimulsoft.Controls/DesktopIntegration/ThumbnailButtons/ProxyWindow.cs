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
using System.Windows.Forms;
using System.Text;
using System.Drawing;

namespace Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons
{
    internal sealed class ProxyWindow : Form
    {
        private CustomWindowsManager windowsManager;
        public CustomWindowsManager WindowsManager
        {
            private get
            {
                return windowsManager;
            }
            set
            {
                windowsManager = value;
            }
        }

        private IntPtr _proxyingFor;
        public IntPtr RealWindow
        {
            get 
            { 
                return _proxyingFor; 
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (WindowsManager != null)
            {
                WindowsManager.DispatchMessage(ref m);
            }
            base.WndProc(ref m);
        }

        public ProxyWindow(IntPtr proxyingFor)
        {
            _proxyingFor = proxyingFor;
            Size = new Size(1, 1);

            StringBuilder text = new StringBuilder(256);
            UnsafeNativeMethods.GetWindowText(_proxyingFor, text, text.Capacity);
            Text = text.ToString();
        }
    }
}