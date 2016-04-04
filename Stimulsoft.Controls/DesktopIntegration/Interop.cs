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
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Stimulsoft.Controls.DesktopIntegration
{
    #region TBPFLAG
    internal enum TBPFLAG
    {
        TBPF_NOPROGRESS = 0,
        TBPF_INDETERMINATE = 0x1,
        TBPF_NORMAL = 0x2,
        TBPF_ERROR = 0x4,
        TBPF_PAUSED = 0x8
    }
    #endregion

    #region TBATFLAG
    internal enum TBATFLAG
    {
        TBATF_USEMDITHUMBNAIL = 0x1,
        TBATF_USEMDILIVEPREVIEW = 0x2
    }
    #endregion

    #region THUMBBUTTON
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct THUMBBUTTON
    {
        [MarshalAs(UnmanagedType.U4)]
        public THBMASK dwMask;
        public uint iId;
        public uint iBitmap;
        public IntPtr hIcon;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szTip;
        [MarshalAs(UnmanagedType.U4)]
        public THBFLAGS dwFlags;
    }
    #endregion

    #region THBMASK
    internal enum THBMASK
    {
        THB_BITMAP = 0x1,
        THB_ICON = 0x2,
        THB_TOOLTIP = 0x4,
        THB_FLAGS = 0x8
    }
    #endregion

    #region THBFLAGS
    internal enum THBFLAGS
    {
        THBF_ENABLED = 0,
        THBF_DISABLED = 0x1,
        THBF_DISMISSONCLICK = 0x2,
        THBF_NOBACKGROUND = 0x4,
        THBF_HIDDEN = 0x8
    }
    #endregion

    #region RECT
    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }
    }
    #endregion

    #region POINT
    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        internal int X;
        internal int Y;

        internal POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
    #endregion

    #region UnsafeNativeMethods
    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
        private static uint _uTBBCMsg;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern uint RegisterWindowMessage(string lpString);
        public static uint WM_TaskbarButtonCreated
        {
            get
            {
                if (_uTBBCMsg == 0)
                {
                    _uTBBCMsg = RegisterWindowMessage("TaskbarButtonCreated");
                }
                return _uTBBCMsg;
            }
        }

        [DllImport("user32.dll")]
        public static extern int GetWindowText(
            IntPtr hwnd, StringBuilder str, int maxCount);

        [DllImport("DwmApi.dll")]
        internal static extern int DwmSetWindowAttribute(
            IntPtr hwnd,
            uint dwAttributeToSet,
            IntPtr pvAttributeValue,
            uint cbAttribute);

        [DllImport("dwmapi.dll")]
        public static extern int DwmInvalidateIconicBitmaps(IntPtr hwnd);

        [DllImport("user32.dll",
        CharSet = CharSet.Auto,
        SetLastError = true)]
        internal static extern IntPtr SendMessage(
            IntPtr hWnd,
            uint msg,
            IntPtr wParam,
            IntPtr lParam
        );

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetClientRect(IntPtr hwnd, ref RECT rect);

        public static bool GetClientSize(IntPtr hwnd, out System.Drawing.Size size)
        {
            RECT rect = new RECT();
            if (!GetClientRect(hwnd, ref rect))
            {
                size = new System.Drawing.Size(-1, -1);
                return false;
            }
            size = new System.Drawing.Size(rect.right, rect.bottom);
            return true;
        }

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetIconicThumbnail(
            IntPtr hwnd, IntPtr hbitmap, uint flags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ClientToScreen(
            IntPtr hwnd, ref POINT point);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetIconicLivePreviewBitmap(
            IntPtr hwnd, IntPtr hbitmap,
            ref POINT ptClient, uint flags);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetIconicLivePreviewBitmap(
            IntPtr hwnd, IntPtr hbitmap, IntPtr ptClient, uint flags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool StretchBlt(
            IntPtr hDestDC, int destX, int destY, int destWidth, int destHeight,
            IntPtr hSrcDC, int srcX, int srcY, int srcWidth, int srcHeight,
            uint operation);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hwnd, ref RECT rect);
    }
    #endregion

    #region SafeNativeMethods
    [SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods
    {
        public const uint THBN_CLICKED = 0x1800;

        public const int DWM_SIT_DISPLAYFRAME = 0x00000001;

        public const int WM_COMMAND = 0x111;
        public const int WM_SYSCOMMAND = 0x112;
        public const int WM_ACTIVATE = 0x0006;
        public const int WM_CLOSE = 0x0010;
        public const int WM_DWMSENDICONICTHUMBNAIL = 0x0323;
        public const int WM_DWMSENDICONICLIVEPREVIEWBITMAP = 0x0326;

        public const int SC_CLOSE = 0xF060;

        public const int WA_ACTIVE = 1;
        public const int WA_CLICKACTIVE = 2;
        
        public const int DWMWA_FORCE_ICONIC_REPRESENTATION = 7;
        public const int DWMWA_HAS_ICONIC_BITMAP = 10;
    }
    #endregion
}