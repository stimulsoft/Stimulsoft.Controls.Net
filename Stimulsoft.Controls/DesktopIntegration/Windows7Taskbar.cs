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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using Stimulsoft.Base;

namespace Stimulsoft.Controls.DesktopIntegration
{
    public static class Windows7Taskbar
    {
        #region Properties
        private static bool isWindowsVistaOrGreater = false;
        public static bool IsWindowsVistaOrGreater
        {
            get
            {
                return isWindowsVistaOrGreater;
            }
        }
        
        [CLSCompliantAttribute(false)]
        public static uint TaskbarButtonCreatedMessage
        {
            get 
            { 
                return UnsafeNativeMethods.WM_TaskbarButtonCreated; 
            }
        }

        private static ITaskbarList3 _taskbarList;
        internal static ITaskbarList3 TaskbarList
        {
            get
            {
                if (_taskbarList == null)
                {
                    lock (typeof(Windows7Taskbar))
                    {
                        if (_taskbarList == null)
                        {
                            _taskbarList = (ITaskbarList3)new CTaskbarList();
                            _taskbarList.HrInit();
                        }
                    }
                }
                return _taskbarList;
            }
        }

        private static bool disableTaskbarProgress = false;
        public static bool DisableTaskbarProgress
        {
            get
            {
                return disableTaskbarProgress;
            }
            set
            {
                disableTaskbarProgress = value;
            }
        }
        #endregion

        #region Taskbar Progress Bar
        #region Enum ThumbnailProgressState
        public enum ThumbnailProgressState
        {
            NoProgress = 0,
            Indeterminate = 0x1,
            Normal = 0x2,
            // red
            Error = 0x4,
            // yellow
            Paused = 0x8
        }
        #endregion

        #region Fields
        private static IntPtr LastHwnd = IntPtr.Zero;
        #endregion

        public static void ShowTaskbarProgress(object form, ThumbnailProgressState state)
        {
            ShowTaskbarProgress((form is Form) ? ((Form)form).Handle : IntPtr.Zero, state);
        }

        public static void ShowTaskbarProgress(IntPtr hwnd, ThumbnailProgressState state)
        {
            if (disableTaskbarProgress || !isWindowsVistaOrGreater) return;

            HideTaskbarProgress();

            if (hwnd == IntPtr.Zero)
            {
                hwnd = GetHandleOfTheMainWindow();
            }

            if (hwnd != IntPtr.Zero)
            {
                LastHwnd = hwnd;
                TaskbarList.SetProgressState(LastHwnd, (TBPFLAG)state);
            }
        }

        public static void HideTaskbarProgress()
        {
            if (disableTaskbarProgress || !isWindowsVistaOrGreater) return;

            if (LastHwnd != IntPtr.Zero)
            {
                TaskbarList.SetProgressState(LastHwnd, TBPFLAG.TBPF_NOPROGRESS);
                LastHwnd = IntPtr.Zero;
            }
        }

        private static IntPtr GetHandleOfTheMainWindow()
        {
            if (Application.OpenForms.Count > 0)
            {
                var forms = Application.OpenForms;
                int index = Application.OpenForms.Count - 1;
                while (index >= 0)
                {
                    var form = forms[index];
                    if (!(form is IStiThreadForm))
                    {
                        if (form.ShowInTaskbar)
                            return form.Handle;
                    }

                    index--;
                }
            }

            return IntPtr.Zero;
        }
        #endregion

        #region Infrastructure
        private static void InternalEnableCustomWindowPreview(IntPtr hwnd, bool enable)
        {
            IntPtr t = Marshal.AllocHGlobal(4);
            Marshal.WriteInt32(t, enable ? 1 : 0);

            try
            {
                int rc;
                rc = UnsafeNativeMethods.DwmSetWindowAttribute(
                    hwnd, SafeNativeMethods.DWMWA_HAS_ICONIC_BITMAP, t, 4);
                if (rc != 0)
                    throw Marshal.GetExceptionForHR(rc);

                rc = UnsafeNativeMethods.DwmSetWindowAttribute(
                    hwnd, SafeNativeMethods.DWMWA_FORCE_ICONIC_REPRESENTATION, t, 4);
                if (rc != 0)
                    throw Marshal.GetExceptionForHR(rc);
            }
            finally
            {
                Marshal.FreeHGlobal(t);
            }
        }
        #endregion

        #region DWM Iconic Thumbnail and Peek Bitmap
        public static void EnableCustomWindowPreview(IntPtr hwnd)
        {
            InternalEnableCustomWindowPreview(hwnd, true);
        }

        public static void DisableCustomWindowPreview(IntPtr hwnd)
        {
            InternalEnableCustomWindowPreview(hwnd, false);
        }

        public static void SetIconicThumbnail(IntPtr hwnd, Bitmap bitmap)
        {
            int rc = UnsafeNativeMethods.DwmSetIconicThumbnail(
                hwnd,
                bitmap.GetHbitmap(),
                SafeNativeMethods.DWM_SIT_DISPLAYFRAME);
            if (rc != 0)
                throw Marshal.GetExceptionForHR(rc);
        }

        public static void SetPeekBitmap(IntPtr hwnd, Bitmap bitmap, bool displayFrame)
        {
            int rc = UnsafeNativeMethods.DwmSetIconicLivePreviewBitmap(
                hwnd, bitmap.GetHbitmap(),
                IntPtr.Zero, displayFrame ? SafeNativeMethods.DWM_SIT_DISPLAYFRAME : (uint)0);

            if (rc != 0)
                throw Marshal.GetExceptionForHR(rc);
        }

        public static void SetPeekBitmap(IntPtr hwnd, Bitmap bitmap, Point offset, bool displayFrame)
        {
            POINT nativePoint = new POINT(offset.X, offset.Y);
            int rc = UnsafeNativeMethods.DwmSetIconicLivePreviewBitmap(
                hwnd,
                bitmap.GetHbitmap(),
                ref nativePoint,
                displayFrame ? SafeNativeMethods.DWM_SIT_DISPLAYFRAME : (uint)0);

            if (rc != 0)
                throw Marshal.GetExceptionForHR(rc);
        }
        #endregion

        #region Taskbar Thumbnails
        public static void SetThumbnailTooltip(IntPtr hWnd, string tooltip)
        {
            if (!isWindowsVistaOrGreater || hWnd != IntPtr.Zero)
            {
                TaskbarList.SetThumbnailTooltip(hWnd, tooltip);
            }
        }
        #endregion

        #region Miscellaneous
        /*public static void AllowTaskbarWindowMessagesThroughUIPI()
        {
            if (isWindowsVistaOrGreater)
            {
                UnsafeNativeMethods.ChangeWindowMessageFilter(
                    UnsafeNativeMethods.WM_TaskbarButtonCreated,
                    SafeNativeMethods.MSGFLT_ADD);
                UnsafeNativeMethods.ChangeWindowMessageFilter(
                    SafeNativeMethods.WM_DWMSENDICONICTHUMBNAIL,
                    SafeNativeMethods.MSGFLT_ADD);
                UnsafeNativeMethods.ChangeWindowMessageFilter(
                    SafeNativeMethods.WM_DWMSENDICONICLIVEPREVIEWBITMAP,
                    SafeNativeMethods.MSGFLT_ADD);
                UnsafeNativeMethods.ChangeWindowMessageFilter(
                    SafeNativeMethods.WM_COMMAND,
                    SafeNativeMethods.MSGFLT_ADD);
                UnsafeNativeMethods.ChangeWindowMessageFilter(
                    SafeNativeMethods.WM_SYSCOMMAND,
                    SafeNativeMethods.MSGFLT_ADD);
                UnsafeNativeMethods.ChangeWindowMessageFilter(
                    SafeNativeMethods.WM_ACTIVATE,
                    SafeNativeMethods.MSGFLT_ADD);
            }
        }*/
        #endregion

        #region Application Id
        /*public static void SetCurrentProcessAppId(string appId)
        {
            if (isWindowsVistaOrGreater)
            {
                UnsafeNativeMethods.SetCurrentProcessExplicitAppUserModelID(appId);
            }
        }

        public static string GetCurrentProcessAppId()
        {
            if (isWindowsVistaOrGreater)
            {
                string appId;
                UnsafeNativeMethods.GetCurrentProcessExplicitAppUserModelID(out appId);
                return appId;
            }
            else
            {
                return string.Empty;
            }
        }*/
        #endregion

        static Windows7Taskbar()
        {
            isWindowsVistaOrGreater = (System.Environment.OSVersion.Version.Major >= 6 && System.Environment.OSVersion.Version.Minor >= 1);
        }
    }
}