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
using System.Collections.Generic;

namespace Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons
{
    public sealed class ThumbButtonManager
    {
        public ThumbButtonManager(IntPtr hwnd)
        {
            _hwnd = hwnd;
        }

        [CLSCompliantAttribute(false)]
        public ThumbButton CreateThumbButton(uint id, Icon icon, string tooltip)
        {
            return new ThumbButton(this, id, icon, tooltip);
        }

        public void DispatchMessage(ref Message message)
        {
            UInt64 wparam = (UInt64)message.WParam.ToInt64();
            UInt32 wparam32 = (UInt32)(wparam & 0xffffffff);   //Clear top 32 bits

            if (message.Msg == SafeNativeMethods.WM_COMMAND &&
                (wparam32 >> 16 == SafeNativeMethods.THBN_CLICKED))
            {
                uint id = wparam32 & 0xffff;    //Bottom 16 bits
                _thumbButtons[id].FireClick();
            }
        }

        public void AddThumbButtons(params ThumbButton[] buttons)
        {
            //Array.ForEach(buttons, b => _thumbButtons.Add(b.Id, b));

            int index = 0;
            while (index < buttons.Length)
            {
                _thumbButtons.Add(buttons[index].Id, buttons[index]);
                index++;
            }

            RefreshThumbButtons();
        }

        [CLSCompliantAttribute(false)]
        public ThumbButton this[uint id]
        {
            get
            {
                return _thumbButtons[id];
            }
        }

        #region Implementation

        private bool _buttonsLoaded;
        internal void RefreshThumbButtons()
        {
            //THUMBBUTTON[] win32Buttons =
            //    (from thumbButton in _thumbButtons.Values
            //     select thumbButton.Win32ThumbButton).ToArray();

            int index = 0;
            THUMBBUTTON[] win32Buttons = new THUMBBUTTON[_thumbButtons.Count];

            foreach(ThumbButton thumbButton in _thumbButtons.Values)
            {
                win32Buttons[index] = thumbButton.Win32ThumbButton;
                index++;
            }

            if (_buttonsLoaded)
            {
                Windows7Taskbar.TaskbarList.ThumbBarUpdateButtons(
                    _hwnd, (uint)win32Buttons.Length, win32Buttons);
            }
            else //First time
            {
                Windows7Taskbar.TaskbarList.ThumbBarAddButtons(
                    _hwnd, (uint)win32Buttons.Length, win32Buttons);
                _buttonsLoaded = true;
            }
        }

        private Dictionary<uint, ThumbButton> _thumbButtons =
            new Dictionary<uint, ThumbButton>();
        private IntPtr _hwnd;

        #endregion
    }
}