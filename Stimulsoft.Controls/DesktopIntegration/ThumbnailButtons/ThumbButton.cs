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

namespace Stimulsoft.Controls.DesktopIntegration.ThumbnailButtons
{
    public sealed class ThumbButton
    {
        #region Fields
        private ThumbButtonManager _manager;
        public event EventHandler Clicked;
        #endregion

        #region Properties
        private uint id;
        [CLSCompliantAttribute(false)]
        public uint Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        private Icon icon;
        public Icon Icon
        {
            get
            {
                return icon;
            }
            set
            {
                icon = value;
            }
        }

        private string tooltip;
        public string Tooltip
        {
            get
            {
                return tooltip;
            }
            set
            {
                tooltip = value;
            }
        }

        private THBFLAGS flags;
        internal THBFLAGS Flags 
        {
            get
            {
                return flags;
            }
            set
            {
                flags = value;
            }
        }

        internal THUMBBUTTON Win32ThumbButton
        {
            get
            {
                THUMBBUTTON win32ThumbButton = new THUMBBUTTON();
                win32ThumbButton.iId = Id;
                win32ThumbButton.szTip = Tooltip;
                win32ThumbButton.hIcon = Icon.Handle;
                win32ThumbButton.dwFlags = Flags;

                win32ThumbButton.dwMask = THBMASK.THB_FLAGS;
                if (!string.IsNullOrEmpty(tooltip))
                    win32ThumbButton.dwMask |= THBMASK.THB_TOOLTIP;
                if (icon != null)
                    win32ThumbButton.dwMask |= THBMASK.THB_ICON;

                return win32ThumbButton;
            }
        }

        public bool Visible
        {
            get
            {
                return (flags & THBFLAGS.THBF_HIDDEN) == 0;
            }
            set
            {
                if (value)
                {
                    flags &= ~(THBFLAGS.THBF_HIDDEN);
                }
                else
                {
                    flags |= THBFLAGS.THBF_HIDDEN;
                }
                _manager.RefreshThumbButtons();
            }
        }

        public bool Enabled
        {
            get
            {
                return (flags & THBFLAGS.THBF_DISABLED) == 0;
            }
            set
            {
                if (value)
                {
                    flags &= ~(THBFLAGS.THBF_DISABLED);
                }
                else
                {
                    flags |= THBFLAGS.THBF_DISABLED;
                }
                _manager.RefreshThumbButtons();
            }
        }
        #endregion

        #region Methods
        internal void FireClick()
        {
            if (Clicked != null)
                Clicked(this, EventArgs.Empty);
        }
        #endregion

        internal ThumbButton(ThumbButtonManager manager, uint id, Icon icon, string tooltip)
        {
            _manager = manager;

            this.id = id;
            this.icon = icon;
            this.tooltip = tooltip;
        }
    }
}