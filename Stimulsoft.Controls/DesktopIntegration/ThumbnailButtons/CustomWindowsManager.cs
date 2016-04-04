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
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons
{
    public class BitmapRequestedEventArgs : EventArgs
    {
        internal BitmapRequestedEventArgs(int width, int height, bool defaultDisplayFrame)
        {
            _width = width;
            _height = height;
            DisplayFrameAroundBitmap = defaultDisplayFrame;
        }

        private int _width;
        public int Width
        {
            get 
            { 
                return _width; 
            }
        }

        private int _height;
        public int Height
        {
            get 
            { 
                return _height; 
            }
        }

        private bool useWindowScreenshot;
        public bool UseWindowScreenshot
        {
            internal get
            {
                return useWindowScreenshot;
            }
            set
            {
                useWindowScreenshot = value;
            }
        }

        private bool doNotMirrorBitmap;
        public bool DoNotMirrorBitmap
        {
            internal get
            {
                return doNotMirrorBitmap;
            }
            set
            {
                doNotMirrorBitmap = value;
            }
        }

        private bool displayFrameAroundBitmap;
        public bool DisplayFrameAroundBitmap
        {
            internal get
            {
                return displayFrameAroundBitmap;
            }
            set
            {
                displayFrameAroundBitmap = value;
            }
        }

        private Bitmap bitmap;
        public Bitmap Bitmap
        {
            internal get
            {
                return bitmap;
            }
            set
            {
                bitmap = value;
            }
        }
    }

    public sealed class CustomWindowsManager
    {
        public static CustomWindowsManager CreateWindowsManager(IntPtr hwnd)
        {
            return CreateWindowsManager(hwnd, IntPtr.Zero);
        }

        public static CustomWindowsManager CreateWindowsManager(IntPtr hwnd, IntPtr parentHwnd)
        {
            if (parentHwnd == IntPtr.Zero)
                return new CustomWindowsManager(hwnd);

            ProxyWindow proxy = new ProxyWindow(hwnd);

            Windows7Taskbar.TaskbarList.UnregisterTab(parentHwnd);

            Windows7Taskbar.TaskbarList.RegisterTab(proxy.Handle, parentHwnd);
            Windows7Taskbar.TaskbarList.SetTabOrder(proxy.Handle, IntPtr.Zero);
            Windows7Taskbar.TaskbarList.ActivateTab(proxy.Handle);

            return new CustomWindowsManager(proxy, parentHwnd);
        }

        private IntPtr _hwnd;
        private IntPtr _hwndParent;
        private ProxyWindow _proxyWindow;


        internal CustomWindowsManager(IntPtr hwnd)
        {
            _hwnd = hwnd;
            Windows7Taskbar.EnableCustomWindowPreview(hwnd);
        }

        internal CustomWindowsManager(ProxyWindow proxy, IntPtr hwndParent)
        {
            _hwnd = proxy.RealWindow;
            _hwndParent = hwndParent;
            _proxyWindow = proxy;//Just keep it alive
            _proxyWindow.WindowsManager = this;

            Windows7Taskbar.EnableCustomWindowPreview(WindowToTellDwmAbout);
        }

        public event EventHandler<BitmapRequestedEventArgs> PeekRequested;
        public event EventHandler<BitmapRequestedEventArgs> ThumbnailRequested;

        private IntPtr WindowToTellDwmAbout
        {
            get
            {
                if (_proxyWindow == null)
                    return _hwnd;
                else
                    return _proxyWindow.Handle;
            }
        }

        public void InvalidatePreviews()
        {
            UnsafeNativeMethods.DwmInvalidateIconicBitmaps(WindowToTellDwmAbout);
        }

        public void DisablePreview()
        {
            Windows7Taskbar.DisableCustomWindowPreview(WindowToTellDwmAbout);
        }

        public void EnablePreview()
        {
            Windows7Taskbar.EnableCustomWindowPreview(WindowToTellDwmAbout);
        }

        public void WindowClosed()
        {
            Windows7Taskbar.TaskbarList.UnregisterTab(_hwnd);
            _proxyWindow.Close();
        }

        public void DispatchMessage(ref Message m)
        {
            if (m.Msg == SafeNativeMethods.WM_ACTIVATE && _hwndParent != IntPtr.Zero)
            {
                if (((int)m.WParam) == SafeNativeMethods.WA_ACTIVE ||
                    ((int)m.WParam) == SafeNativeMethods.WA_CLICKACTIVE)
                {
                    UnsafeNativeMethods.SendMessage(
                        _hwnd, (uint)m.Msg, m.WParam, m.LParam);

                    //TODO: Technically, we should also test if the child
                    //isn't visible.  If it is, no need to send the message.
                }
            }
            if (m.Msg == SafeNativeMethods.WM_SYSCOMMAND && _hwndParent != IntPtr.Zero)
            {
                if (((int)m.WParam) == SafeNativeMethods.SC_CLOSE)
                {
                    UnsafeNativeMethods.SendMessage(
                        _hwnd, SafeNativeMethods.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    WindowClosed();
                }
            }
            if (m.Msg == SafeNativeMethods.WM_DWMSENDICONICTHUMBNAIL)
            {
                int width = (int)(((long)m.LParam) >> 16);
                int height = (int)(((long)m.LParam) & (0xFFFF));

                BitmapRequestedEventArgs b = new BitmapRequestedEventArgs(width, height, true);
                ThumbnailRequested(this, b);

                if (b.UseWindowScreenshot)
                {
                    Size clientSize;
                    UnsafeNativeMethods.GetClientSize(_hwnd, out clientSize);

                    float thumbnailAspect = ((float)width) / height;
                    float windowAspect = ((float)clientSize.Width) / clientSize.Height;

                    if (windowAspect > thumbnailAspect)
                    {
                        //Wider than the thumbnail, make the thumbnail height smaller:
                        height = (int)(height * (thumbnailAspect / windowAspect));
                    }
                    if (windowAspect < thumbnailAspect)
                    {
                        //The thumbnail is wider, make the width smaller:
                        width = (int)(width * (windowAspect / thumbnailAspect));
                    }

                    b.Bitmap = ScreenCapture.GrabWindowBitmap(_hwnd, new Size(width, height));
                }
                else if (!b.DoNotMirrorBitmap)
                {
                    b.Bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }

                Windows7Taskbar.SetIconicThumbnail(WindowToTellDwmAbout, b.Bitmap);
                b.Bitmap.Dispose(); //TODO: Is it our responsibility?
            }
            else if (m.Msg == SafeNativeMethods.WM_DWMSENDICONICLIVEPREVIEWBITMAP)
            {
                Size clientSize;
                if (!UnsafeNativeMethods.GetClientSize(_hwnd, out clientSize))
                {
                    clientSize = new Size(50, 50);//Best guess
                }

                BitmapRequestedEventArgs b = new BitmapRequestedEventArgs(
                    clientSize.Width, clientSize.Height, _hwndParent == IntPtr.Zero);
                PeekRequested(this, b);

                if (b.UseWindowScreenshot)
                {
                    b.Bitmap = ScreenCapture.GrabWindowBitmap(_hwnd, clientSize);
                }
                else if (!b.DoNotMirrorBitmap)
                {
                    b.Bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }

                if (_hwndParent != IntPtr.Zero)
                {
                    Point offset = WindowUtilities.GetParentOffsetOfChild(_hwnd, _hwndParent);
                    Windows7Taskbar.SetPeekBitmap(WindowToTellDwmAbout, b.Bitmap, offset, b.DisplayFrameAroundBitmap);
                }
                else
                {
                    Windows7Taskbar.SetPeekBitmap(WindowToTellDwmAbout, b.Bitmap, b.DisplayFrameAroundBitmap);
                }
                b.Bitmap.Dispose();
            }
        }
    }

    public static class ScreenCapture
    {
        public static Bitmap GrabWindowBitmap(IntPtr hwnd, Size bitmapSize)
        {
            IntPtr windowDC = IntPtr.Zero;
            try
            {
                Size realWindowSize;
                UnsafeNativeMethods.GetClientSize(hwnd, out realWindowSize);

                windowDC = UnsafeNativeMethods.GetWindowDC(hwnd);

                var targetBitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);

                using (var targetGr = Graphics.FromImage(targetBitmap))
                {
                    var targetDC = targetGr.GetHdc();

                    uint operation = 0x00CC0020 /*SRCCOPY*/
                                   | 0x40000000 /*CAPTUREBLT*/;

                    Size ncArea = WindowUtilities.GetNonClientArea(hwnd);

                    UnsafeNativeMethods.StretchBlt(
                        targetDC, 0, 0, targetBitmap.Width, targetBitmap.Height,
                        windowDC, ncArea.Width, realWindowSize.Height + ncArea.Height - 1, realWindowSize.Width, -realWindowSize.Height,
                        operation);

                    targetGr.ReleaseHdc();

                    targetGr.DrawString("Windows 7 Bridge",
                        new Font(FontFamily.GenericMonospace, 12),
                        new SolidBrush(Color.Red),
                        new PointF(10, 10));

                    return targetBitmap;
                }
            }
            finally
            {
                if (windowDC != IntPtr.Zero)
                {
                    UnsafeNativeMethods.ReleaseDC(hwnd, windowDC);
                }
            }
        }
    }

    internal static class WindowUtilities
    {
        public static Point GetParentOffsetOfChild(IntPtr hwnd, IntPtr hwndParent)
        {
            POINT childScreenCoord = new POINT(0, 0);
            UnsafeNativeMethods.ClientToScreen(hwnd, ref childScreenCoord);

            POINT parentScreenCoord = new POINT(0, 0);
            UnsafeNativeMethods.ClientToScreen(hwndParent, ref parentScreenCoord);

            Point offset = new Point(
                childScreenCoord.X - parentScreenCoord.X,
                childScreenCoord.Y - parentScreenCoord.Y);

            return offset;
        }

        public static Size GetNonClientArea(IntPtr hwnd)
        {
            POINT c = new POINT(0, 0);
            UnsafeNativeMethods.ClientToScreen(hwnd, ref c);

            RECT r = new RECT();
            UnsafeNativeMethods.GetWindowRect(hwnd, ref r);

            return new Size(c.X - r.left, c.Y - r.top);
        }
    }
}