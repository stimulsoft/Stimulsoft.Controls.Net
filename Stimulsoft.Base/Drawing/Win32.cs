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
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Stimulsoft.Base.Drawing
{	
	/// <summary>
	/// Access to Win32 functions.
	/// </summary>
	[System.Security.SuppressUnmanagedCodeSecurity]
	public sealed class Win32
	{
		#region Drawing Options
		/// <summary>
		/// Specifies values for WM_PRINT.
		/// </summary>
		public enum DrawingOptions
		{
			/// <summary>
			/// Draws the window only if it is visible.
			/// </summary>			
			PRF_CHECKVISIBLE,
			/// <summary>
			/// Draws all visible children windows.
			/// </summary>
			PRF_CHILDREN,
			/// <summary>
			/// Draws the client area of the window.
			/// </summary>
			PRF_CLIENT,
			/// <summary>
			/// Erases the background before drawing the window.
			/// </summary>
			PRF_ERASEBKGND,
			/// <summary>
			/// Draws the nonclient area of the window.
			/// </summary>
			PRF_NONCLIENT,
			PRF_OWNED
		}
		#endregion

		#region WindowExStyles
		/// <summary>
		/// Specifies values from WindowExStyles enumeration.
		/// </summary>
		public enum WindowExStyles
		{                		
			/// <summary>
			/// Specified WS_EX_DLGMODALFRAME enumeration value.
			/// </summary>
			WS_EX_DLGMODALFRAME     = 0x00000001,
                        		
			/// <summary>
			/// Specified WS_EX_NOPARENTNOTIFY enumeration value.
			/// </summary>
			WS_EX_NOPARENTNOTIFY    = 0x00000004,
                        		
			/// <summary>
			/// Specified WS_EX_TOPMOST enumeration value.
			/// </summary>
			WS_EX_TOPMOST           = 0x00000008,
                        		
			/// <summary>
			/// Specified WS_EX_ACCEPTFILES enumeration value.
			/// </summary>
			WS_EX_ACCEPTFILES       = 0x00000010,
                        		
			/// <summary>
			/// Specified WS_EX_TRANSPARENT enumeration value.
			/// </summary>
			WS_EX_TRANSPARENT       = 0x00000020,
                        		
			/// <summary>
			/// Specified WS_EX_MDICHILD enumeration value.
			/// </summary>
			WS_EX_MDICHILD          = 0x00000040,
                        		
			/// <summary>
			/// Specified WS_EX_TOOLWINDOW enumeration value.
			/// </summary>
			WS_EX_TOOLWINDOW        = 0x00000080,
                        		
			/// <summary>
			/// Specified WS_EX_WINDOWEDGE enumeration value.
			/// </summary>
			WS_EX_WINDOWEDGE        = 0x00000100,
                        		
			/// <summary>
			/// Specified WS_EX_CLIENTEDGE enumeration value.
			/// </summary>
			WS_EX_CLIENTEDGE        = 0x00000200,
                        		
			/// <summary>
			/// Specified WS_EX_CONTEXTHELP enumeration value.
			/// </summary>
			WS_EX_CONTEXTHELP       = 0x00000400,
                        		
			/// <summary>
			/// Specified WS_EX_RIGHT enumeration value.
			/// </summary>
			WS_EX_RIGHT             = 0x00001000,
                        		
			/// <summary>
			/// Specified WS_EX_LEFT enumeration value.
			/// </summary>
			WS_EX_LEFT              = 0x00000000,
                        		
			/// <summary>
			/// Specified WS_EX_RTLREADING enumeration value.
			/// </summary>
			WS_EX_RTLREADING        = 0x00002000,
                        		
			/// <summary>
			/// Specified WS_EX_LTRREADING enumeration value.
			/// </summary>
			WS_EX_LTRREADING        = 0x00000000,
                        		
			/// <summary>
			/// Specified WS_EX_LEFTSCROLLBAR enumeration value.
			/// </summary>
			WS_EX_LEFTSCROLLBAR     = 0x00004000,
                        		
			/// <summary>
			/// Specified WS_EX_RIGHTSCROLLBAR enumeration value.
			/// </summary>
			WS_EX_RIGHTSCROLLBAR    = 0x00000000,
                        		
			/// <summary>
			/// Specified WS_EX_CONTROLPARENT enumeration value.
			/// </summary>
			WS_EX_CONTROLPARENT     = 0x00010000,
                        		
			/// <summary>
			/// Specified WS_EX_STATICEDGE enumeration value.
			/// </summary>
			WS_EX_STATICEDGE        = 0x00020000,
                        		
			/// <summary>
			/// Specified WS_EX_APPWINDOW enumeration value.
			/// </summary>
			WS_EX_APPWINDOW         = 0x00040000,
                        		
			/// <summary>
			/// Specified WS_EX_OVERLAPPEDWINDOW enumeration value.
			/// </summary>
			WS_EX_OVERLAPPEDWINDOW  = 0x00000300,
                        		
			/// <summary>
			/// Specified WS_EX_PALETTEWINDOW enumeration value.
			/// </summary>
			WS_EX_PALETTEWINDOW     = 0x00000188,
                        		
			/// <summary>
			/// Specified WS_EX_LAYERED enumeration value.
			/// </summary>
			WS_EX_LAYERED			= 0x00080000
		}
		#endregion

		#region MousePosition
		/// <summary>
		/// Specifies values from MousePosition enumeration.
		/// </summary>
		public enum MousePosition
		{
			/// <summary>
			/// Specified HTERROR enumeration value.
			/// </summary>
			HTERROR =			-2,
			/// <summary>
			/// Specified HTTRANSPARENT enumeration value.
			/// </summary>
			HTTRANSPARENT =		-1,
			/// <summary>
			/// Specified HTNOWHERE enumeration value.
			/// </summary>
			HTNOWHERE =			0,
			/// <summary>
			/// Specified HTCLIENT enumeration value.
			/// </summary>
			HTCLIENT =			1,
			/// <summary>
			/// Specified HTCAPTION enumeration value.
			/// </summary>
			HTCAPTION =			2,
			/// <summary>
			/// Specified HTSYSMENU enumeration value.
			/// </summary>
			HTSYSMENU =			3,
			/// <summary>
			/// Specified HTGROWBOX enumeration value.
			/// </summary>
			HTGROWBOX =			4,
			/// <summary>
			/// Specified HTSIZE enumeration value.
			/// </summary>
			HTSIZE =			4,
			/// <summary>
			/// Specified HTMENU enumeration value.
			/// </summary>
			HTMENU =			5,
			/// <summary>
			/// Specified HTHSCROLL enumeration value.
			/// </summary>
			HTHSCROLL =			6,
			/// <summary>
			/// Specified HTVSCROLL enumeration value.
			/// </summary>
			HTVSCROLL =			7,
			/// <summary>
			/// Specified HTMINBUTTON enumeration value.
			/// </summary>
			HTMINBUTTON =		8,
			/// <summary>
			/// Specified HTMAXBUTTON enumeration value.
			/// </summary>
			HTMAXBUTTON =		9,
			/// <summary>
			/// Specified HTLEFT enumeration value.
			/// </summary>
			HTLEFT =			10,
			/// <summary>
			/// Specified HTRIGHT enumeration value.
			/// </summary>
			HTRIGHT =			11,
			/// <summary>
			/// Specified HTTOP enumeration value.
			/// </summary>
			HTTOP =				12,
			/// <summary>
			/// Specified HTTOPLEFT enumeration value.
			/// </summary>
			HTTOPLEFT =			13,
			/// <summary>
			/// Specified HTTOPRIGHT enumeration value.
			/// </summary>
			HTTOPRIGHT =		14,
			/// <summary>
			/// Specified HTBOTTOM enumeration value.
			/// </summary>
			HTBOTTOM =			15,
			/// <summary>
			/// Specified HTBOTTOMLEFT enumeration value.
			/// </summary>
			HTBOTTOMLEFT =		16,
			/// <summary>
			/// Specified HTBOTTOMRIGHT enumeration value.
			/// </summary>
			HTBOTTOMRIGHT =		17,
			/// <summary>
			/// Specified HTBORDER enumeration value.
			/// </summary>
			HTBORDER =			18,
			/// <summary>
			/// Specified HTOBJECT enumeration value.
			/// </summary>
			HTOBJECT =			19,
			/// <summary>
			/// Specified HTCLOSE enumeration value.
			/// </summary>
			HTCLOSE =			20,
			/// <summary>
			/// Specified HTHELP enumeration value.
			/// </summary>
			HTHELP =			21
		}
		#endregion
		
		#region Virtual Key
		/// <summary>
		/// Specifies values from VirtualKey enumeration.
		/// </summary>
		public enum VirtualKey
		{
            /// <summary>
            /// Specified VK_F1 enumeration value.
            /// </summary>
            VK_F1 = 0x70,
            /// <summary>
            /// Specified VK_F2 enumeration value.
            /// </summary>
            VK_F2 = 0x71,
            /// <summary>
            /// Specified VK_F3 enumeration value.
            /// </summary>
            VK_F3 = 0x72,
            /// <summary>
            /// Specified VK_F4 enumeration value.
            /// </summary>
            VK_F4 = 0x73,
            /// <summary>
            /// Specified VK_F5 enumeration value.
            /// </summary>
            VK_F5 = 0x74,
            /// <summary>
            /// Specified VK_F6 enumeration value.
            /// </summary>
            VK_F6 = 0x75,
            /// <summary>
            /// Specified VK_F7 enumeration value.
            /// </summary>
            VK_F7 = 0x76,
            /// <summary>
            /// Specified VK_F8 enumeration value.
            /// </summary>
            VK_F8 = 0x77,
            /// <summary>
            /// Specified VK_F9 enumeration value.
            /// </summary>
            VK_F9 = 0x78,
            /// <summary>
            /// Specified VK_F10 enumeration value.
            /// </summary>
            VK_F10 = 0x79,
            /// <summary>
            /// Specified VK_F11 enumeration value.
            /// </summary>
            VK_F11 = 0x7A,
            /// <summary>
            /// Specified VK_F12 enumeration value.
            /// </summary>
            VK_F12 =0x7B,
			/// <summary>
			/// Specified VK_LBUTTON enumeration value.
			/// </summary>
			VK_LBUTTON		= 0x01,
			/// <summary>
			/// Specified VK_CANCEL enumeration value.
			/// </summary>
			VK_CANCEL		= 0x03,
			/// <summary>
			/// Specified VK_BACK enumeration value.
			/// </summary>
			VK_BACK			= 0x08,
			/// <summary>
			/// Specified VK_TAB enumeration value.
			/// </summary>
			VK_TAB			= 0x09,
			/// <summary>
			/// Specified VK_CLEAR enumeration value.
			/// </summary>
			VK_CLEAR		= 0x0C,
			/// <summary>
			/// Specified VK_RETURN enumeration value.
			/// </summary>
			VK_RETURN		= 0x0D,
			/// <summary>
			/// Specified VK_SHIFT enumeration value.
			/// </summary>
			VK_SHIFT		= 0x10,
			/// <summary>
			/// Specified VK_CONTROL enumeration value.
			/// </summary>
			VK_CONTROL		= 0x11,
			/// <summary>
			/// Specified VK_MENU enumeration value.
			/// </summary>
			VK_MENU			= 0x12,
			/// <summary>
			/// Specified VK_CAPITAL enumeration value.
			/// </summary>
			VK_CAPITAL		= 0x14,
			/// <summary>
			/// Specified VK_ESCAPE enumeration value.
			/// </summary>
			VK_ESCAPE		= 0x1B,
			/// <summary>
			/// Specified VK_SPACE enumeration value.
			/// </summary>
			VK_SPACE		= 0x20,
			/// <summary>
			/// Specified VK_PRIOR enumeration value.
			/// </summary>
			VK_PRIOR		= 0x21,
			/// <summary>
			/// Specified VK_NEXT enumeration value.
			/// </summary>
			VK_NEXT			= 0x22,
			/// <summary>
			/// Specified VK_END enumeration value.
			/// </summary>
			VK_END			= 0x23,
			/// <summary>
			/// Specified VK_HOME enumeration value.
			/// </summary>
			VK_HOME			= 0x24,
			/// <summary>
			/// Specified VK_LEFT enumeration value.
			/// </summary>
			VK_LEFT			= 0x25,
			/// <summary>
			/// Specified VK_UP enumeration value.
			/// </summary>
			VK_UP			= 0x26,
			/// <summary>
			/// Specified VK_RIGHT enumeration value.
			/// </summary>
			VK_RIGHT		= 0x27,
			/// <summary>
			/// Specified VK_DOWN enumeration value.
			/// </summary>
			VK_DOWN			= 0x28,
			/// <summary>
			/// Specified VK_SELECT enumeration value.
			/// </summary>
			VK_SELECT		= 0x29,
			/// <summary>
			/// Specified VK_EXECUTE enumeration value.
			/// </summary>
			VK_EXECUTE		= 0x2B,
			/// <summary>
			/// Specified VK_SNAPSHOT enumeration value.
			/// </summary>
			VK_SNAPSHOT		= 0x2C,
			/// <summary>
			/// Specified VK_HELP enumeration value.
			/// </summary>
			VK_HELP			= 0x2F,
			/// <summary>
			/// Specified VK_0 enumeration value.
			/// </summary>
			VK_0			= 0x30,
			/// <summary>
			/// Specified VK_1 enumeration value.
			/// </summary>
			VK_1			= 0x31,
			/// <summary>
			/// Specified VK_2 enumeration value.
			/// </summary>
			VK_2			= 0x32,
			/// <summary>
			/// Specified VK_3 enumeration value.
			/// </summary>
			VK_3			= 0x33,
			/// <summary>
			/// Specified VK_4 enumeration value.
			/// </summary>
			VK_4			= 0x34,
			/// <summary>
			/// Specified VK_5 enumeration value.
			/// </summary>
			VK_5			= 0x35,
			/// <summary>
			/// Specified VK_6 enumeration value.
			/// </summary>
			VK_6			= 0x36,
			/// <summary>
			/// Specified VK_7 enumeration value.
			/// </summary>
			VK_7			= 0x37,
			/// <summary>
			/// Specified VK_8 enumeration value.
			/// </summary>
			VK_8			= 0x38,
			/// <summary>
			/// Specified VK_9 enumeration value.
			/// </summary>
			VK_9			= 0x39,
			/// <summary>
			/// Specified VK_A enumeration value.
			/// </summary>
			VK_A			= 0x41,
			/// <summary>
			/// Specified VK_B enumeration value.
			/// </summary>
			VK_B			= 0x42,
			/// <summary>
			/// Specified VK_C enumeration value.
			/// </summary>
			VK_C			= 0x43,
			/// <summary>
			/// Specified VK_D enumeration value.
			/// </summary>
			VK_D			= 0x44,
			/// <summary>
			/// Specified HTERROR enumeration value.
			/// </summary>
			VK_E			= 0x45,
			/// <summary>
			/// Specified VK_F enumeration value.
			/// </summary>
			VK_F			= 0x46,
			/// <summary>
			/// Specified VK_G enumeration value.
			/// </summary>
			VK_G			= 0x47,
			/// <summary>
			/// Specified VK_H enumeration value.
			/// </summary>
			VK_H			= 0x48,
			/// <summary>
			/// Specified VK_I enumeration value.
			/// </summary>
			VK_I			= 0x49,
			/// <summary>
			/// Specified VK_J enumeration value.
			/// </summary>
			VK_J			= 0x4A,
			/// <summary>
			/// Specified VK_K enumeration value.
			/// </summary>
			VK_K			= 0x4B,
			/// <summary>
			/// Specified VK_L enumeration value.
			/// </summary>
			VK_L			= 0x4C,
			/// <summary>
			/// Specified VK_M enumeration value.
			/// </summary>
			VK_M			= 0x4D,
			/// <summary>
			/// Specified VK_N enumeration value.
			/// </summary>
			VK_N			= 0x4E,
			/// <summary>
			/// Specified VK_O enumeration value.
			/// </summary>
			VK_O			= 0x4F,
			/// <summary>
			/// Specified VK_P enumeration value.
			/// </summary>
			VK_P			= 0x50,
			/// <summary>
			/// Specified VK_Q enumeration value.
			/// </summary>
			VK_Q			= 0x51,
			/// <summary>
			/// Specified VK_R enumeration value.
			/// </summary>
			VK_R			= 0x52,
			/// <summary>
			/// Specified VK_S enumeration value.
			/// </summary>
			VK_S			= 0x53,
			/// <summary>
			/// Specified VK_T enumeration value.
			/// </summary>
			VK_T			= 0x54,
			/// <summary>
			/// Specified VK_U enumeration value.
			/// </summary>
			VK_U			= 0x55,
			/// <summary>
			/// Specified VK_V enumeration value.
			/// </summary>
			VK_V			= 0x56,
			/// <summary>
			/// Specified VK_W enumeration value.
			/// </summary>
			VK_W			= 0x57,
			/// <summary>
			/// Specified VK_X enumeration value.
			/// </summary>
			VK_X			= 0x58,
			/// <summary>
			/// Specified VK_Y enumeration value.
			/// </summary>
			VK_Y			= 0x59,
			/// <summary>
			/// Specified VK_Z enumeration value.
			/// </summary>
			VK_Z			= 0x5A,
			/// <summary>
			/// Specified VK_NUMPAD0 enumeration value.
			/// </summary>
			VK_NUMPAD0		= 0x60,
			/// <summary>
			/// Specified VK_NUMPAD1 enumeration value.
			/// </summary>
			VK_NUMPAD1		= 0x61,
			/// <summary>
			/// Specified VK_NUMPAD2 enumeration value.
			/// </summary>
			VK_NUMPAD2		= 0x62,
			/// <summary>
			/// Specified VK_NUMPAD3 enumeration value.
			/// </summary>
			VK_NUMPAD3		= 0x63,
			/// <summary>
			/// Specified VK_NUMPAD4 enumeration value.
			/// </summary>
			VK_NUMPAD4		= 0x64,
			/// <summary>
			/// Specified VK_NUMPAD5 enumeration value.
			/// </summary>
			VK_NUMPAD5		= 0x65,
			/// <summary>
			/// Specified VK_NUMPAD6 enumeration value.
			/// </summary>
			VK_NUMPAD6		= 0x66,
			/// <summary>
			/// Specified VK_NUMPAD7 enumeration value.
			/// </summary>
			VK_NUMPAD7		= 0x67,
			/// <summary>
			/// Specified VK_NUMPAD8 enumeration value.
			/// </summary>
			VK_NUMPAD8		= 0x68,
			/// <summary>
			/// Specified VK_NUMPAD9 enumeration value.
			/// </summary>
			VK_NUMPAD9		= 0x69,
			/// <summary>
			/// Specified VK_MULTIPLY enumeration value.
			/// </summary>
			VK_MULTIPLY		= 0x6A,
			/// <summary>
			/// Specified VK_ADD enumeration value.
			/// </summary>
			VK_ADD			= 0x6B,
			/// <summary>
			/// Specified VK_SEPARATOR enumeration value.
			/// </summary>
			VK_SEPARATOR	= 0x6C,
			/// <summary>
			/// Specified VK_SUBTRACT enumeration value.
			/// </summary>
			VK_SUBTRACT		= 0x6D,
			/// <summary>
			/// Specified VK_DECIMAL enumeration value.
			/// </summary>
			VK_DECIMAL		= 0x6E,
			/// <summary>
			/// Specified VK_DIVIDE enumeration value.
			/// </summary>
			VK_DIVIDE		= 0x6F,
			/// <summary>
			/// Specified VK_ATTN enumeration value.
			/// </summary>
			VK_ATTN			= 0xF6,
			/// <summary>
			/// Specified VK_CRSEL enumeration value.
			/// </summary>
			VK_CRSEL		= 0xF7,
			/// <summary>
			/// Specified VK_EXSEL enumeration value.
			/// </summary>
			VK_EXSEL		= 0xF8,
			/// <summary>
			/// Specified VK_EREOF enumeration value.
			/// </summary>
			VK_EREOF		= 0xF9,
			/// <summary>
			/// Specified VK_PLAY enumeration value.
			/// </summary>
			VK_PLAY			= 0xFA, 
			/// <summary>
			/// Specified VK_ZOOM enumeration value.
			/// </summary>
			VK_ZOOM			= 0xFB,
			/// <summary>
			/// Specified VK_NONAME enumeration value.
			/// </summary>
			VK_NONAME		= 0xFC,
			/// <summary>
			/// Specified VK_PA1 enumeration value.
			/// </summary>
			VK_PA1			= 0xFD,
			/// <summary>
			/// Specified VK_OEM_CLEAR enumeration value.
			/// </summary>
			VK_OEM_CLEAR	= 0xFE,
			/// <summary>
			/// Specified VK_LWIN enumeration value.
			/// </summary>
			VK_LWIN			= 0x5B,
			/// <summary>
			/// Specified VK_RWIN enumeration value.
			/// </summary>
			VK_RWIN			= 0x5C,
			/// <summary>
			/// Specified VK_APPS enumeration value.
			/// </summary>
			VK_APPS			= 0x5D,
			/// <summary>
			/// Specified VK_LSHIFT enumeration value.
			/// </summary>
			VK_LSHIFT		= 0xA0, 
			/// <summary>
			/// Specified VK_RSHIFT enumeration value.
			/// </summary>
			VK_RSHIFT		= 0xA1,  
			/// <summary>
			/// Specified VK_LCONTROL enumeration value.
			/// </summary>
			VK_LCONTROL		= 0xA2, 
			/// <summary>
			/// Specified VK_RCONTROL enumeration value.
			/// </summary>
			VK_RCONTROL		= 0xA3,
			/// <summary>
			/// Specified VK_LMENU enumeration value.
			/// </summary>
			VK_LMENU		= 0xA4, 
			/// <summary>
			/// Specified VK_RMENU enumeration value.
			/// </summary>
			VK_RMENU		= 0xA5
		}
		#endregion

		#region ActivateFlag
		/// <summary>
		/// Specifies values from ActivateState enumeration.
		/// </summary>
		public enum ActivateState
		{
			/// <summary>
			/// Specified WA_INACTIVE enumeration value.
			/// </summary>
			WA_INACTIVE     = 0,
			/// <summary>
			/// Specified WA_ACTIVE enumeration value.
			/// </summary>
			WA_ACTIVE       = 1,
			/// <summary>
			/// Specified WA_CLICKACTIVE enumeration value.
			/// </summary>
			WA_CLICKACTIVE  = 2
		}
		#endregion

		#region MouseActivateFlags
		public enum MouseActivateFlags
		{
			/// <summary>
			/// Specified MA_ACTIVATE enumeration value.
			/// </summary>
			MA_ACTIVATE			= 1,
			/// <summary>
			/// Specified MA_ACTIVATEANDEAT enumeration value.
			/// </summary>
			MA_ACTIVATEANDEAT   = 2,
			/// <summary>
			/// Specified MA_NOACTIVATE enumeration value.
			/// </summary>
			MA_NOACTIVATE       = 3,
			/// <summary>
			/// Specified MA_NOACTIVATEANDEAT enumeration value.
			/// </summary>
			MA_NOACTIVATEANDEAT = 4
		}
		#endregion

		#region Windows Message
		/// <summary>
		/// Specifies values from Message enumeration.
		/// </summary>
		public enum Msg
		{
			/// <summary>
			/// Specified WM_NULL enumeration value.
			/// </summary>
			WM_NULL                   = 0x0000,
			/// <summary>
			/// Specified WM_CREATE enumeration value.
			/// </summary>
			WM_CREATE                 = 0x0001,
			/// <summary>
			/// Specified WM_DESTROY enumeration value.
			/// </summary>
			WM_DESTROY                = 0x0002,
			/// <summary>
			/// Specified WM_MOVE enumeration value.
			/// </summary>
			WM_MOVE                   = 0x0003,
			/// <summary>
			/// Specified WM_SIZE enumeration value.
			/// </summary>
			WM_SIZE                   = 0x0005,
			/// <summary>
			/// Specified WM_ACTIVATE enumeration value.
			/// </summary>
			WM_ACTIVATE               = 0x0006,
			/// <summary>
			/// Specified WM_SETFOCUS enumeration value.
			/// </summary>
			WM_SETFOCUS               = 0x0007,
			/// <summary>
			/// Specified WM_KILLFOCUS enumeration value.
			/// </summary>
			WM_KILLFOCUS              = 0x0008,
			/// <summary>
			/// Specified WM_ENABLE enumeration value.
			/// </summary>
			WM_ENABLE                 = 0x000A,
			/// <summary>
			/// Specified WM_SETREDRAW enumeration value.
			/// </summary>
			WM_SETREDRAW              = 0x000B,
			/// <summary>
			/// Specified WM_SETTEXT enumeration value.
			/// </summary>
			WM_SETTEXT                = 0x000C,
			/// <summary>
			/// Specified WM_GETTEXT enumeration value.
			/// </summary>
			WM_GETTEXT                = 0x000D,
			/// <summary>
			/// Specified WM_GETTEXTLENGTH enumeration value.
			/// </summary>
			WM_GETTEXTLENGTH          = 0x000E,
			/// <summary>
			/// Specified WM_PAINT enumeration value.
			/// </summary>
			WM_PAINT                  = 0x000F,
			/// <summary>
			/// Specified WM_CLOSE enumeration value.
			/// </summary>
			WM_CLOSE                  = 0x0010,
			/// <summary>
			/// Specified WM_QUERYENDSESSION enumeration value.
			/// </summary>
			WM_QUERYENDSESSION        = 0x0011,
			/// <summary>
			/// Specified WM_QUIT enumeration value.
			/// </summary>
			WM_QUIT                   = 0x0012,
			/// <summary>
			/// Specified WM_QUERYOPEN enumeration value.
			/// </summary>
			WM_QUERYOPEN              = 0x0013,
			/// <summary>
			/// Specified WM_ERASEBKGND enumeration value.
			/// </summary>
			WM_ERASEBKGND             = 0x0014,
			/// <summary>
			/// Specified WM_SYSCOLORCHANGE enumeration value.
			/// </summary>
			WM_SYSCOLORCHANGE         = 0x0015,
			/// <summary>
			/// Specified WM_ENDSESSION enumeration value.
			/// </summary>
			WM_ENDSESSION             = 0x0016,
			/// <summary>
			/// Specified WM_SHOWWINDOW enumeration value.
			/// </summary>
			WM_SHOWWINDOW             = 0x0018,
			/// <summary>
			/// Specified WM_CTLCOLOR enumeration value.
			/// </summary>
			WM_CTLCOLOR               = 0x0019,
			/// <summary>
			/// Specified WM_WININICHANGE enumeration value.
			/// </summary>
			WM_WININICHANGE           = 0x001A,
			/// <summary>
			/// Specified WM_SETTINGCHANGE enumeration value.
			/// </summary>
			WM_SETTINGCHANGE          = 0x001A,
			/// <summary>
			/// Specified WM_DEVMODECHANGE enumeration value.
			/// </summary>
			WM_DEVMODECHANGE          = 0x001B,
			/// <summary>
			/// Specified WM_ACTIVATEAPP enumeration value.
			/// </summary>
			WM_ACTIVATEAPP            = 0x001C,
			/// <summary>
			/// Specified WM_FONTCHANGE enumeration value.
			/// </summary>
			WM_FONTCHANGE             = 0x001D,
			/// <summary>
			/// Specified WM_TIMECHANGE enumeration value.
			/// </summary>
			WM_TIMECHANGE             = 0x001E,
			/// <summary>
			/// Specified WM_CANCELMODE enumeration value.
			/// </summary>
			WM_CANCELMODE             = 0x001F,
			/// <summary>
			/// Specified WM_SETCURSOR enumeration value.
			/// </summary>
			WM_SETCURSOR              = 0x0020,
			/// <summary>
			/// Specified WM_MOUSEACTIVATE enumeration value.
			/// </summary>
			WM_MOUSEACTIVATE          = 0x0021,
			/// <summary>
			/// Specified WM_CHILDACTIVATE enumeration value.
			/// </summary>
			WM_CHILDACTIVATE          = 0x0022,
			/// <summary>
			/// Specified WM_QUEUESYNC enumeration value.
			/// </summary>
			WM_QUEUESYNC              = 0x0023,
			/// <summary>
			/// Specified WM_GETMINMAXINFO enumeration value.
			/// </summary>
			WM_GETMINMAXINFO          = 0x0024,
			/// <summary>
			/// Specified WM_PAINTICON enumeration value.
			/// </summary>
			WM_PAINTICON              = 0x0026,
			/// <summary>
			/// Specified WM_ICONERASEBKGND enumeration value.
			/// </summary>
			WM_ICONERASEBKGND         = 0x0027,
			/// <summary>
			/// Specified WM_NEXTDLGCTL enumeration value.
			/// </summary>
			WM_NEXTDLGCTL             = 0x0028,
			/// <summary>
			/// Specified WM_SPOOLERSTATUS enumeration value.
			/// </summary>
			WM_SPOOLERSTATUS          = 0x002A,
			/// <summary>
			/// Specified WM_DRAWITEM enumeration value.
			/// </summary>
			WM_DRAWITEM               = 0x002B,
			/// <summary>
			/// Specified WM_MEASUREITEM enumeration value.
			/// </summary>
			WM_MEASUREITEM            = 0x002C,
			/// <summary>
			/// Specified WM_DELETEITEM enumeration value.
			/// </summary>
			WM_DELETEITEM             = 0x002D,
			/// <summary>
			/// Specified WM_VKEYTOITEM enumeration value.
			/// </summary>
			WM_VKEYTOITEM             = 0x002E,
			/// <summary>
			/// Specified WM_CHARTOITEM enumeration value.
			/// </summary>
			WM_CHARTOITEM             = 0x002F,
			/// <summary>
			/// Specified WM_SETFONT enumeration value.
			/// </summary>
			WM_SETFONT                = 0x0030,
			/// <summary>
			/// Specified WM_GETFONT enumeration value.
			/// </summary>
			WM_GETFONT                = 0x0031,
			/// <summary>
			/// Specified WM_SETHOTKEY enumeration value.
			/// </summary>
			WM_SETHOTKEY              = 0x0032,
			/// <summary>
			/// Specified WM_GETHOTKEY enumeration value.
			/// </summary>
			WM_GETHOTKEY              = 0x0033,
			/// <summary>
			/// Specified WM_QUERYDRAGICON enumeration value.
			/// </summary>
			WM_QUERYDRAGICON          = 0x0037,
			/// <summary>
			/// Specified WM_COMPAREITEM enumeration value.
			/// </summary>
			WM_COMPAREITEM            = 0x0039,
			/// <summary>
			/// Specified WM_GETOBJECT enumeration value.
			/// </summary>
			WM_GETOBJECT              = 0x003D,
			/// <summary>
			/// Specified WM_COMPACTING enumeration value.
			/// </summary>
			WM_COMPACTING             = 0x0041,
			/// <summary>
			/// Specified WM_COMMNOTIFY enumeration value.
			/// </summary>
			WM_COMMNOTIFY             = 0x0044,
			/// <summary>
			/// Specified WM_WINDOWPOSCHANGING enumeration value.
			/// </summary>
			WM_WINDOWPOSCHANGING      = 0x0046,
			/// <summary>
			/// Specified WM_WINDOWPOSCHANGED enumeration value.
			/// </summary>
			WM_WINDOWPOSCHANGED       = 0x0047,
			/// <summary>
			/// Specified WM_POWER enumeration value.
			/// </summary>
			WM_POWER                  = 0x0048,
			/// <summary>
			/// Specified WM_COPYDATA enumeration value.
			/// </summary>
			WM_COPYDATA               = 0x004A,
			/// <summary>
			/// Specified WM_CANCELJOURNAL enumeration value.
			/// </summary>
			WM_CANCELJOURNAL          = 0x004B,
			/// <summary>
			/// Specified WM_NOTIFY enumeration value.
			/// </summary>
			WM_NOTIFY                 = 0x004E,
			/// <summary>
			/// Specified WM_INPUTLANGCHANGEREQUEST enumeration value.
			/// </summary>
			WM_INPUTLANGCHANGEREQUEST = 0x0050,
			/// <summary>
			/// Specified WM_INPUTLANGCHANGE enumeration value.
			/// </summary>
			WM_INPUTLANGCHANGE        = 0x0051,
			/// <summary>
			/// Specified WM_TCARD enumeration value.
			/// </summary>
			WM_TCARD                  = 0x0052,
			/// <summary>
			/// Specified WM_HELP enumeration value.
			/// </summary>
			WM_HELP                   = 0x0053,
			/// <summary>
			/// Specified WM_USERCHANGED enumeration value.
			/// </summary>
			WM_USERCHANGED            = 0x0054,
			/// <summary>
			/// Specified WM_NOTIFYFORMAT enumeration value.
			/// </summary>
			WM_NOTIFYFORMAT           = 0x0055,
			/// <summary>
			/// Specified WM_CONTEXTMENU enumeration value.
			/// </summary>
			WM_CONTEXTMENU            = 0x007B,
			/// <summary>
			/// Specified WM_STYLECHANGING enumeration value.
			/// </summary>
			WM_STYLECHANGING          = 0x007C,
			/// <summary>
			/// Specified WM_STYLECHANGED enumeration value.
			/// </summary>
			WM_STYLECHANGED           = 0x007D,
			/// <summary>
			/// Specified WM_DISPLAYCHANGE enumeration value.
			/// </summary>
			WM_DISPLAYCHANGE          = 0x007E,
			/// <summary>
			/// Specified WM_GETICON enumeration value.
			/// </summary>
			WM_GETICON                = 0x007F,
			/// <summary>
			/// Specified WM_SETICON enumeration value.
			/// </summary>
			WM_SETICON                = 0x0080,
			/// <summary>
			/// Specified WM_NCCREATE enumeration value.
			/// </summary>
			WM_NCCREATE               = 0x0081,
			/// <summary>
			/// Specified WM_NCDESTROY enumeration value.
			/// </summary>
			WM_NCDESTROY              = 0x0082,
			/// <summary>
			/// Specified WM_NCCALCSIZE enumeration value.
			/// </summary>
			WM_NCCALCSIZE             = 0x0083,
			/// <summary>
			/// Specified WM_NCHITTEST enumeration value.
			/// </summary>
			WM_NCHITTEST              = 0x0084,
			/// <summary>
			/// Specified WM_NCPAINT enumeration value.
			/// </summary>
			WM_NCPAINT                = 0x0085,
			/// <summary>
			/// Specified WM_NCACTIVATE enumeration value.
			/// </summary>
			WM_NCACTIVATE             = 0x0086,
			/// <summary>
			/// Specified WM_GETDLGCODE enumeration value.
			/// </summary>
			WM_GETDLGCODE             = 0x0087,
			/// <summary>
			/// Specified WM_SYNCPAINT enumeration value.
			/// </summary>
			WM_SYNCPAINT              = 0x0088,
			/// <summary>
			/// Specified WM_NCMOUSEMOVE enumeration value.
			/// </summary>
			WM_NCMOUSEMOVE            = 0x00A0,
			/// <summary>
			/// Specified WM_NCLBUTTONDOWN enumeration value.
			/// </summary>
			WM_NCLBUTTONDOWN          = 0x00A1,
			/// <summary>
			/// Specified WM_NCLBUTTONUP enumeration value.
			/// </summary>
			WM_NCLBUTTONUP            = 0x00A2,
			/// <summary>
			/// Specified WM_NCLBUTTONDBLCLK enumeration value.
			/// </summary>
			WM_NCLBUTTONDBLCLK        = 0x00A3,
			/// <summary>
			/// Specified WM_NCRBUTTONDOWN enumeration value.
			/// </summary>
			WM_NCRBUTTONDOWN          = 0x00A4,
			/// <summary>
			/// Specified WM_NCRBUTTONUP enumeration value.
			/// </summary>
			WM_NCRBUTTONUP            = 0x00A5,
			/// <summary>
			/// Specified WM_NCRBUTTONDBLCLK enumeration value.
			/// </summary>
			WM_NCRBUTTONDBLCLK        = 0x00A6,
			/// <summary>
			/// Specified WM_NCMBUTTONDOWN enumeration value.
			/// </summary>
			WM_NCMBUTTONDOWN          = 0x00A7,
			/// <summary>
			/// Specified WM_NCMBUTTONUP enumeration value.
			/// </summary>
			WM_NCMBUTTONUP            = 0x00A8,
			/// <summary>
			/// Specified WM_NCMBUTTONDBLCLK enumeration value.
			/// </summary>
			WM_NCMBUTTONDBLCLK        = 0x00A9,
			/// <summary>
			/// Specified WM_KEYDOWN enumeration value.
			/// </summary>
			WM_KEYDOWN                = 0x0100,
			/// <summary>
			/// Specified WM_KEYUP enumeration value.
			/// </summary>
			WM_KEYUP                  = 0x0101,
			/// <summary>
			/// Specified WM_CHAR enumeration value.
			/// </summary>
			WM_CHAR                   = 0x0102,
			/// <summary>
			/// Specified WM_DEADCHAR enumeration value.
			/// </summary>
			WM_DEADCHAR               = 0x0103,
			/// <summary>
			/// Specified WM_SYSKEYDOWN enumeration value.
			/// </summary>
			WM_SYSKEYDOWN             = 0x0104,
			/// <summary>
			/// Specified WM_SYSKEYUP enumeration value.
			/// </summary>
			WM_SYSKEYUP               = 0x0105,
			/// <summary>
			/// Specified WM_SYSCHAR enumeration value.
			/// </summary>
			WM_SYSCHAR                = 0x0106,
			/// <summary>
			/// Specified WM_SYSDEADCHAR enumeration value.
			/// </summary>
			WM_SYSDEADCHAR            = 0x0107,
			/// <summary>
			/// Specified WM_KEYLAST enumeration value.
			/// </summary>
			WM_KEYLAST                = 0x0108,
			/// <summary>
			/// Specified WM_IME_STARTCOMPOSITION enumeration value.
			/// </summary>
			WM_IME_STARTCOMPOSITION   = 0x010D,
			/// <summary>
			/// Specified WM_IME_ENDCOMPOSITION enumeration value.
			/// </summary>
			WM_IME_ENDCOMPOSITION     = 0x010E,
			/// <summary>
			/// Specified WM_IME_COMPOSITION enumeration value.
			/// </summary>
			WM_IME_COMPOSITION        = 0x010F,
			/// <summary>
			/// Specified WM_IME_KEYLAST enumeration value.
			/// </summary>
			WM_IME_KEYLAST            = 0x010F,
			/// <summary>
			/// Specified WM_INITDIALOG enumeration value.
			/// </summary>
			WM_INITDIALOG             = 0x0110,
			/// <summary>
			/// Specified WM_COMMAND enumeration value.
			/// </summary>
			WM_COMMAND                = 0x0111,
			/// <summary>
			/// Specified WM_SYSCOMMAND enumeration value.
			/// </summary>
			WM_SYSCOMMAND             = 0x0112,
			/// <summary>
			/// Specified WM_TIMER enumeration value.
			/// </summary>
			WM_TIMER                  = 0x0113,
			/// <summary>
			/// Specified WM_HSCROLL enumeration value.
			/// </summary>
			WM_HSCROLL                = 0x0114,
			/// <summary>
			/// Specified WM_VSCROLL enumeration value.
			/// </summary>
			WM_VSCROLL                = 0x0115,
			/// <summary>
			/// Specified WM_INITMENU enumeration value.
			/// </summary>
			WM_INITMENU               = 0x0116,
			/// <summary>
			/// Specified WM_INITMENUPOPUP enumeration value.
			/// </summary>
			WM_INITMENUPOPUP          = 0x0117,
			/// <summary>
			/// Specified WM_MENUSELECT enumeration value.
			/// </summary>
			WM_MENUSELECT             = 0x011F,
			/// <summary>
			/// Specified WM_MENUCHAR enumeration value.
			/// </summary>
			WM_MENUCHAR               = 0x0120,
			/// <summary>
			/// Specified WM_ENTERIDLE enumeration value.
			/// </summary>
			WM_ENTERIDLE              = 0x0121,
			/// <summary>
			/// Specified WM_MENURBUTTONUP enumeration value.
			/// </summary>
			WM_MENURBUTTONUP          = 0x0122,
			/// <summary>
			/// Specified WM_MENUDRAG enumeration value.
			/// </summary>
			WM_MENUDRAG               = 0x0123,
			/// <summary>
			/// Specified WM_MENUGETOBJECT enumeration value.
			/// </summary>
			WM_MENUGETOBJECT          = 0x0124,
			/// <summary>
			/// Specified WM_UNINITMENUPOPUP enumeration value.
			/// </summary>
			WM_UNINITMENUPOPUP        = 0x0125,
			/// <summary>
			/// Specified WM_MENUCOMMAND enumeration value.
			/// </summary>
			WM_MENUCOMMAND            = 0x0126,
			/// <summary>
			/// Specified WM_CTLCOLORMSGBOX enumeration value.
			/// </summary>
			WM_CTLCOLORMSGBOX         = 0x0132,
			/// <summary>
			/// Specified WM_CTLCOLOREDIT enumeration value.
			/// </summary>
			WM_CTLCOLOREDIT           = 0x0133,
			/// <summary>
			/// Specified WM_CTLCOLORLISTBOX enumeration value.
			/// </summary>
			WM_CTLCOLORLISTBOX        = 0x0134,
			/// <summary>
			/// Specified WM_CTLCOLORBTN enumeration value.
			/// </summary>
			WM_CTLCOLORBTN            = 0x0135,
			/// <summary>
			/// Specified WM_CTLCOLORDLG enumeration value.
			/// </summary>
			WM_CTLCOLORDLG            = 0x0136,
			/// <summary>
			/// Specified WM_CTLCOLORSCROLLBAR enumeration value.
			/// </summary>
			WM_CTLCOLORSCROLLBAR      = 0x0137,
			/// <summary>
			/// Specified WM_CTLCOLORSTATIC enumeration value.
			/// </summary>
			WM_CTLCOLORSTATIC         = 0x0138,		
			/// <summary>
			/// Specified WM_MOUSEMOVE enumeration value.
			/// </summary>
			WM_MOUSEMOVE              = 0x0200,
			/// <summary>
			/// Specified WM_LBUTTONDOWN enumeration value.
			/// </summary>
			WM_LBUTTONDOWN            = 0x0201,
			/// <summary>
			/// Specified WM_LBUTTONUP enumeration value.
			/// </summary>
			WM_LBUTTONUP              = 0x0202,
			/// <summary>
			/// Specified WM_LBUTTONDBLCLK enumeration value.
			/// </summary>
			WM_LBUTTONDBLCLK          = 0x0203,
			/// <summary>
			/// Specified WM_RBUTTONDOWN enumeration value.
			/// </summary>
			WM_RBUTTONDOWN            = 0x0204,
			/// <summary>
			/// Specified WM_RBUTTONUP enumeration value.
			/// </summary>
			WM_RBUTTONUP              = 0x0205,
			/// <summary>
			/// Specified WM_RBUTTONDBLCLK enumeration value.
			/// </summary>
			WM_RBUTTONDBLCLK          = 0x0206,
			/// <summary>
			/// Specified WM_MBUTTONDOWN enumeration value.
			/// </summary>
			WM_MBUTTONDOWN            = 0x0207,
			/// <summary>
			/// Specified WM_MBUTTONUP enumeration value.
			/// </summary>
			WM_MBUTTONUP              = 0x0208,
			/// <summary>
			/// Specified WM_MBUTTONDBLCLK enumeration value.
			/// </summary>
			WM_MBUTTONDBLCLK          = 0x0209,
			/// <summary>
			/// Specified WM_MOUSEWHEEL enumeration value.
			/// </summary>
			WM_MOUSEWHEEL             = 0x020A,
			/// <summary>
			/// Specified WM_PARENTNOTIFY enumeration value.
			/// </summary>
			WM_PARENTNOTIFY           = 0x0210,
			/// <summary>
			/// Specified WM_ENTERMENULOOP enumeration value.
			/// </summary>
			WM_ENTERMENULOOP          = 0x0211,
			/// <summary>
			/// Specified WM_EXITMENULOOP enumeration value.
			/// </summary>
			WM_EXITMENULOOP           = 0x0212,
			/// <summary>
			/// Specified WM_NEXTMENU enumeration value.
			/// </summary>
			WM_NEXTMENU               = 0x0213,
			/// <summary>
			/// Specified WM_SIZING enumeration value.
			/// </summary>
			WM_SIZING                 = 0x0214,
			/// <summary>
			/// Specified WM_CAPTURECHANGED enumeration value.
			/// </summary>
			WM_CAPTURECHANGED         = 0x0215,
			/// <summary>
			/// Specified WM_MOVING enumeration value.
			/// </summary>
			WM_MOVING                 = 0x0216,
			/// <summary>
			/// Specified WM_DEVICECHANGE enumeration value.
			/// </summary>
			WM_DEVICECHANGE           = 0x0219,
			/// <summary>
			/// Specified WM_MDICREATE enumeration value.
			/// </summary>
			WM_MDICREATE              = 0x0220,
			/// <summary>
			/// Specified WM_MDIDESTROY enumeration value.
			/// </summary>
			WM_MDIDESTROY             = 0x0221,
			/// <summary>
			/// Specified WM_MDIACTIVATE enumeration value.
			/// </summary>
			WM_MDIACTIVATE            = 0x0222,
			/// <summary>
			/// Specified WM_MDIRESTORE enumeration value.
			/// </summary>
			WM_MDIRESTORE             = 0x0223,
			/// <summary>
			/// Specified WM_MDINEXT enumeration value.
			/// </summary>
			WM_MDINEXT                = 0x0224,
			/// <summary>
			/// Specified WM_MDIMAXIMIZE enumeration value.
			/// </summary>
			WM_MDIMAXIMIZE            = 0x0225,
			/// <summary>
			/// Specified WM_MDITILE enumeration value.
			/// </summary>
			WM_MDITILE                = 0x0226,
			/// <summary>
			/// Specified WM_MDICASCADE enumeration value.
			/// </summary>
			WM_MDICASCADE             = 0x0227,
			/// <summary>
			/// Specified WM_MDIICONARRANGE enumeration value.
			/// </summary>
			WM_MDIICONARRANGE         = 0x0228,
			/// <summary>
			/// Specified WM_MDIGETACTIVE enumeration value.
			/// </summary>
			WM_MDIGETACTIVE           = 0x0229,
			/// <summary>
			/// Specified WM_MDISETMENU enumeration value.
			/// </summary>
			WM_MDISETMENU             = 0x0230,
			/// <summary>
			/// Specified WM_ENTERSIZEMOVE enumeration value.
			/// </summary>
			WM_ENTERSIZEMOVE          = 0x0231,
			/// <summary>
			/// Specified WM_EXITSIZEMOVE enumeration value.
			/// </summary>
			WM_EXITSIZEMOVE           = 0x0232,
			/// <summary>
			/// Specified WM_DROPFILES enumeration value.
			/// </summary>
			WM_DROPFILES              = 0x0233,
			/// <summary>
			/// Specified WM_MDIREFRESHMENU enumeration value.
			/// </summary>
			WM_MDIREFRESHMENU         = 0x0234,
			/// <summary>
			/// Specified WM_IME_SETCONTEXT enumeration value.
			/// </summary>
			WM_IME_SETCONTEXT         = 0x0281,
			/// <summary>
			/// Specified WM_IME_NOTIFY enumeration value.
			/// </summary>
			WM_IME_NOTIFY             = 0x0282,
			/// <summary>
			/// Specified WM_IME_CONTROL enumeration value.
			/// </summary>
			WM_IME_CONTROL            = 0x0283,
			/// <summary>
			/// Specified WM_IME_COMPOSITIONFULL enumeration value.
			/// </summary>
			WM_IME_COMPOSITIONFULL    = 0x0284,
			/// <summary>
			/// Specified WM_IME_SELECT enumeration value.
			/// </summary>
			WM_IME_SELECT             = 0x0285,
			/// <summary>
			/// Specified WM_IME_CHAR enumeration value.
			/// </summary>
			WM_IME_CHAR               = 0x0286,
			/// <summary>
			/// Specified WM_IME_REQUEST enumeration value.
			/// </summary>
			WM_IME_REQUEST            = 0x0288,
			/// <summary>
			/// Specified WM_IME_KEYDOWN enumeration value.
			/// </summary>
			WM_IME_KEYDOWN            = 0x0290,
			/// <summary>
			/// Specified WM_IME_KEYUP enumeration value.
			/// </summary>
			WM_IME_KEYUP              = 0x0291,
			/// <summary>
			/// Specified WM_MOUSEHOVER enumeration value.
			/// </summary>
			WM_MOUSEHOVER             = 0x02A1,
			/// <summary>
			/// Specified WM_MOUSELEAVE enumeration value.
			/// </summary>
			WM_MOUSELEAVE             = 0x02A3,
			/// <summary>
			/// Specified WM_XBUTTONDOWN enumeration value.
			/// </summary>
			WM_XBUTTONDOWN            = 0x020B,
			/// <summary>
			/// Specified WM_XBUTTONUP enumeration value.
			/// </summary>
			WM_XBUTTONUP              = 0x020C,
			/// <summary>
			/// Specified WM_XBUTTONDBLCLK enumeration value.
			/// </summary>
			WM_XBUTTONDBLCLK          = 0x020D,
			/// <summary>
			/// Specified WM_NCXBUTTONDOWN enumeration value.
			/// </summary>
			WM_NCXBUTTONDOWN          = 0x00AB,		                
			/// <summary>
			/// Specified WM_NCXBUTTONUP enumeration value.
			/// </summary>
			WM_NCXBUTTONUP            = 0x00AC,
			/// <summary>
			/// Specified WM_CUT enumeration value.
			/// </summary>
			WM_CUT                    = 0x0300,
			/// <summary>
			/// Specified WM_COPY enumeration value.
			/// </summary>
			WM_COPY                   = 0x0301,
			/// <summary>
			/// Specified WM_PASTE enumeration value.
			/// </summary>
			WM_PASTE                  = 0x0302,
			/// <summary>
			/// Specified WM_CLEAR enumeration value.
			/// </summary>
			WM_CLEAR                  = 0x0303,
			/// <summary>
			/// Specified WM_UNDO enumeration value.
			/// </summary>
			WM_UNDO                   = 0x0304,
			/// <summary>
			/// Specified WM_PaintFORMAT enumeration value.
			/// </summary>
			WM_PaintFORMAT			  = 0x0305,
			/// <summary>
			/// Specified WM_PaintALLFORMATS enumeration value.
			/// </summary>
			WM_PaintALLFORMATS        = 0x0306,
			/// <summary>
			/// Specified WM_DESTROYCLIPBOARD enumeration value.
			/// </summary>
			WM_DESTROYCLIPBOARD       = 0x0307,
			/// <summary>
			/// Specified WM_DRAWCLIPBOARD enumeration value.
			/// </summary>
			WM_DRAWCLIPBOARD          = 0x0308,
			/// <summary>
			/// Specified WM_PAINTCLIPBOARD enumeration value.
			/// </summary>
			WM_PAINTCLIPBOARD         = 0x0309,
			/// <summary>
			/// Specified WM_VSCROLLCLIPBOARD enumeration value.
			/// </summary>
			WM_VSCROLLCLIPBOARD       = 0x030A,
			/// <summary>
			/// Specified WM_SIZECLIPBOARD enumeration value.
			/// </summary>
			WM_SIZECLIPBOARD          = 0x030B,
			/// <summary>
			/// Specified WM_ASKCBFORMATNAME enumeration value.
			/// </summary>
			WM_ASKCBFORMATNAME        = 0x030C,
			/// <summary>
			/// Specified WM_CHANGECBCHAIN enumeration value.
			/// </summary>
			WM_CHANGECBCHAIN          = 0x030D,
			/// <summary>
			/// Specified WM_HSCROLLCLIPBOARD enumeration value.
			/// </summary>
			WM_HSCROLLCLIPBOARD       = 0x030E,
			/// <summary>
			/// Specified WM_QUERYNEWPALETTE enumeration value.
			/// </summary>
			WM_QUERYNEWPALETTE        = 0x030F,
			/// <summary>
			/// Specified WM_PALETTEISCHANGING enumeration value.
			/// </summary>
			WM_PALETTEISCHANGING      = 0x0310,
			/// <summary>
			/// Specified WM_PALETTECHANGED enumeration value.
			/// </summary>
			WM_PALETTECHANGED         = 0x0311,
			/// <summary>
			/// Specified WM_HOTKEY enumeration value.
			/// </summary>
			WM_HOTKEY                 = 0x0312,
			/// <summary>
			/// Specified WM_PRINT enumeration value.
			/// </summary>
			WM_PRINT                  = 0x0317,
			/// <summary>
			/// Specified WM_PRINTCLIENT enumeration value.
			/// </summary>
			WM_PRINTCLIENT            = 0x0318,
			/// <summary>
			/// Specified WM_HANDHELDFIRST enumeration value.
			/// </summary>
			WM_HANDHELDFIRST          = 0x0358,
			/// <summary>
			/// Specified WM_HANDHELDLAST enumeration value.
			/// </summary>
			WM_HANDHELDLAST           = 0x035F,
			/// <summary>
			/// Specified WM_AFXFIRST enumeration value.
			/// </summary>
			WM_AFXFIRST               = 0x0360,
			/// <summary>
			/// Specified WM_AFXLAST enumeration value.
			/// </summary>
			WM_AFXLAST                = 0x037F,
			/// <summary>
			/// Specified WM_PENWINFIRST enumeration value.
			/// </summary>
			WM_PENWINFIRST            = 0x0380,
			/// <summary>
			/// Specified WM_PENWINLAST enumeration value.
			/// </summary>
			WM_PENWINLAST             = 0x038F,
			/// <summary>
			/// Specified WM_APP enumeration value.
			/// </summary>
			WM_APP                    = 0x8000,
			/// <summary>
			/// Specified WM_USER enumeration value.
			/// </summary>
			WM_USER                   = 0x0400,
			/// <summary>
			/// Specified WM_REFLECT enumeration value.
			/// </summary>
			WM_REFLECT                = WM_USER + 0x1c00,
			/// <summary>
			/// Specified EM_FORMATRANGE enumeration value.
			/// </summary>
			EM_FORMATRANGE			  = WM_USER+57,
			/// <summary>
			/// Specified EM_GETCHARFORMAT enumeration value.
			/// </summary>
			EM_GETCHARFORMAT		  = WM_USER+58,
			/// <summary>
			/// Specified EM_SETCHARFORMAT enumeration value.
			/// </summary>
			EM_SETCHARFORMAT		  = WM_USER+68
		}
		#endregion

		#region SetWindowPosZ
		/// <summary>
		/// Specifies values from SetWindowPosZ enumeration.
		/// </summary>
		public enum SetWindowPosZ
		{
			/// <summary>
			/// Specified HWND_TOP enumeration value.
			/// </summary>
			HWND_TOP        = 0,
			/// <summary>
			/// Specified HWND_BOTTOM enumeration value.
			/// </summary>
			HWND_BOTTOM     = 1,
			/// <summary>
			/// Specified HWND_TOPMOST enumeration value.
			/// </summary>
			HWND_TOPMOST    = -1,
			/// <summary>
			/// Specified HWND_NOTOPMOST enumeration value.
			/// </summary>
			HWND_NOTOPMOST  = -2
		}
		#endregion

		#region SetWindowPosFlags
		/// <summary>
		/// Specifies values from SetWindowPosFlags enumeration.
		/// </summary>
		[Flags]
			public enum SetWindowPosFlags
		{
			/// <summary>
			/// Specified SWP_NOSIZE enumeration value.
			/// </summary>
			SWP_NOSIZE          = 0x0001,
			/// <summary>
			/// Specified SWP_NOMOVE enumeration value.
			/// </summary>
			SWP_NOMOVE          = 0x0002,
			/// <summary>
			/// Specified SWP_NOZORDER enumeration value.
			/// </summary>
			SWP_NOZORDER        = 0x0004,
			/// <summary>
			/// Specified SWP_NOREDRAW enumeration value.
			/// </summary>
			SWP_NOREDRAW        = 0x0008,
			/// <summary>
			/// Specified SWP_NOACTIVATE enumeration value.
			/// </summary>
			SWP_NOACTIVATE      = 0x0010,
			/// <summary>
			/// Specified SWP_FRAMECHANGED enumeration value.
			/// </summary>
			SWP_FRAMECHANGED    = 0x0020,
			/// <summary>
			/// Specified SWP_SHOWWINDOW enumeration value.
			/// </summary>
			SWP_SHOWWINDOW      = 0x0040,
			/// <summary>
			/// Specified SWP_HIDEWINDOW enumeration value.
			/// </summary>
			SWP_HIDEWINDOW      = 0x0080,
			/// <summary>
			/// Specified SWP_NOCOPYBITS enumeration value.
			/// </summary>
			SWP_NOCOPYBITS      = 0x0100,
			/// <summary>
			/// Specified SWP_NOOWNERZORDER enumeration value.
			/// </summary>
			SWP_NOOWNERZORDER   = 0x0200,
			/// <summary>
			/// Specified SWP_NOSENDCHANGING enumeration value.
			/// </summary>
			SWP_NOSENDCHANGING  = 0x0400,
			/// <summary>
			/// Specified SWP_DRAWFRAME enumeration value.
			/// </summary>
			SWP_DRAWFRAME       = 0x0020,
			/// <summary>
			/// Specified SWP_NOREPOSITION enumeration value.
			/// </summary>
			SWP_NOREPOSITION    = 0x0200,
			/// <summary>
			/// Specified SWP_DEFERERASE enumeration value.
			/// </summary>
			SWP_DEFERERASE      = 0x2000,
			/// <summary>
			/// Specified SWP_ASYNCWINDOWPOS enumeration value.
			/// </summary>
			SWP_ASYNCWINDOWPOS  = 0x4000
		}
		#endregion

		#region PenStyle
		/// <summary>
		/// Specifies values from PenStyle enumeration.
		/// </summary>
		public enum PenStyle
		{
			/// <summary>
			/// Specified PS_SOLID enumeration value.
			/// </summary>
			PS_SOLID = 0,
			/// <summary>
			/// Specified PS_DASH enumeration value.
			/// </summary>
			PS_DASH = 1,
			/// <summary>
			/// Specified PS_DOT enumeration value.
			/// </summary>
			PS_DOT = 2,
			/// <summary>
			/// Specified PS_DASHDOT enumeration value.
			/// </summary>
			PS_DASHDOT = 3,
			/// <summary>
			/// Specified PS_DASHDOTDOT enumeration value.
			/// </summary>
			PS_DASHDOTDOT = 4,
			/// <summary>
			/// Specified PS_NULL enumeration value.
			/// </summary>
			PS_NULL = 5,
			/// <summary>
			/// Specified PS_INSIDEFRAME enumeration value.
			/// </summary>
			PS_INSIDEFRAME = 6,
			/// <summary>
			/// Specified PS_USERSTYLE enumeration value.
			/// </summary>
			PS_USERSTYLE = 7,
			/// <summary>
			/// Specified PS_ALTERNATE enumeration value.
			/// </summary>
			PS_ALTERNATE = 8
		}
		#endregion

		#region DrawMode
		/// <summary>
		/// Specifies values from DrawMode enumeration.
		/// </summary>
		public enum DrawMode
		{
			/// <summary>
			/// Specified R2_BLACK enumeration value.
			/// </summary>
			R2_BLACK = 1,
			/// <summary>
			/// Specified R2_NOTMERGEPEN enumeration value.
			/// </summary>
			R2_NOTMERGEPEN = 2,
			/// <summary>
			/// Specified R2_MASKNOTPEN enumeration value.
			/// </summary>
			R2_MASKNOTPEN = 3,
			/// <summary>
			/// Specified R2_NOTCOPYPEN enumeration value.
			/// </summary>
			R2_NOTCOPYPEN = 4,
			/// <summary>
			/// Specified R2_MASKPENNOT enumeration value.
			/// </summary>
			R2_MASKPENNOT = 5,
			/// <summary>
			/// Specified R2_NOT enumeration value.
			/// </summary>
			R2_NOT = 6,
			/// <summary>
			/// Specified R2_XORPEN enumeration value.
			/// </summary>
			R2_XORPEN = 7,
			/// <summary>
			/// Specified R2_NOTMASKPEN enumeration value.
			/// </summary>
			R2_NOTMASKPEN = 8,
			/// <summary>
			/// Specified R2_MASKPEN enumeration value.
			/// </summary>
			R2_MASKPEN = 9,
			/// <summary>
			/// Specified R2_NOTXORPEN enumeration value.
			/// </summary>
			R2_NOTXORPEN = 10,
			/// <summary>
			/// Specified R2_NOP enumeration value.
			/// </summary>
			R2_NOP = 11,
			/// <summary>
			/// Specified R2_MERGENOTPEN enumeration value.
			/// </summary>
			R2_MERGENOTPEN = 12,
			/// <summary>
			/// Specified R2_COPYPEN enumeration value.
			/// </summary>
			R2_COPYPEN = 13,
			/// <summary>
			/// Specified R2_MERGEPENNOT enumeration value.
			/// </summary>
			R2_MERGEPENNOT = 14,
			/// <summary>
			/// Specified R2_MERGEPEN enumeration value.
			/// </summary>
			R2_MERGEPEN = 15,
			/// <summary>
			/// Specified R2_WHITE enumeration value.
			/// </summary>
			R2_WHITE = 16
		}
		#endregion

		#region StockObject
		/// <summary>
		/// Specifies values from StockObject enumeration.
		/// </summary>
		public enum StockObject
		{
			/// <summary>
			/// Specified WHITE_BRUSH enumeration value.
			/// </summary>
			WHITE_BRUSH = 0,
			/// <summary>
			/// Specified LTGRAY_BRUSH enumeration value.
			/// </summary>
			LTGRAY_BRUSH = 1,
			/// <summary>
			/// Specified GRAY_BRUSH enumeration value.
			/// </summary>
			GRAY_BRUSH = 2,
			/// <summary>
			/// Specified DKGRAY_BRUSH enumeration value.
			/// </summary>
			DKGRAY_BRUSH = 3,
			/// <summary>
			/// Specified BLACK_BRUSH enumeration value.
			/// </summary>
			BLACK_BRUSH = 4,
			/// <summary>
			/// Specified NULL_BRUSH enumeration value.
			/// </summary>
			NULL_BRUSH = 5,
			/// <summary>
			/// Specified HOLLOW_BRUSH enumeration value.
			/// </summary>
			HOLLOW_BRUSH = 5,
			/// <summary>
			/// Specified WHITE_PEN enumeration value.
			/// </summary>
			WHITE_PEN = 6,
			/// <summary>
			/// Specified BLACK_PEN enumeration value.
			/// </summary>
			BLACK_PEN = 7,
			/// <summary>
			/// Specified NULL_PEN enumeration value.
			/// </summary>
			NULL_PEN = 8,
			/// <summary>
			/// Specified OEM_FIXED_FONT enumeration value.
			/// </summary>
			OEM_FIXED_FONT = 10,
			/// <summary>
			/// Specified ANSI_FIXED_FONT enumeration value.
			/// </summary>
			ANSI_FIXED_FONT = 11,
			/// <summary>
			/// Specified ANSI_VAR_FONT enumeration value.
			/// </summary>
			ANSI_VAR_FONT = 12,
			/// <summary>
			/// Specified SYSTEM_FONT enumeration value.
			/// </summary>
			SYSTEM_FONT = 13,
			/// <summary>
			/// Specified DEVICE_DEFAULT_FONT enumeration value.
			/// </summary>
			DEVICE_DEFAULT_FONT = 14,
			/// <summary>
			/// Specified DEFAULT_PALETTE enumeration value.
			/// </summary>
			DEFAULT_PALETTE = 15,
			/// <summary>
			/// Specified SYSTEM_FIXED_FONT enumeration value.
			/// </summary>
			SYSTEM_FIXED_FONT = 16
		}
		#endregion

		#region Color Type
		/// <summary>
		/// Specifies values from ColorType enumeration.
		/// </summary>
		public enum ColorType
		{
			/// <summary>
			/// Specified COLOR_SCROLLBAR enumeration value.
			/// </summary>
			COLOR_SCROLLBAR =					0,
			/// <summary>
			/// Specified COLOR_BACKGROUND enumeration value.
			/// </summary>
			COLOR_BACKGROUND =					1,
			/// <summary>
			/// Specified COLOR_ACTIVECAPTION enumeration value.
			/// </summary>
			COLOR_ACTIVECAPTION =				2,
			/// <summary>
			/// Specified COLOR_INACTIVECAPTION enumeration value.
			/// </summary>
			COLOR_INACTIVECAPTION =				3,
			/// <summary>
			/// Specified COLOR_MENU enumeration value.
			/// </summary>
			COLOR_MENU =						4,
			/// <summary>
			/// Specified COLOR_WINDOW enumeration value.
			/// </summary>
			COLOR_WINDOW = 						5,
			/// <summary>
			/// Specified COLOR_WINDOWFRAME enumeration value.
			/// </summary>
			COLOR_WINDOWFRAME =					6,
			/// <summary>
			/// Specified COLOR_MENUTEXT enumeration value.
			/// </summary>
			COLOR_MENUTEXT =					7,
			/// <summary>
			/// Specified COLOR_WINDOWTEXT enumeration value.
			/// </summary>
			COLOR_WINDOWTEXT =					8,
			/// <summary>
			/// Specified COLOR_CAPTIONTEXT enumeration value.
			/// </summary>
			COLOR_CAPTIONTEXT =					9,
			/// <summary>
			/// Specified COLOR_ACTIVEBORDER enumeration value.
			/// </summary>
			COLOR_ACTIVEBORDER =				10,
			/// <summary>
			/// Specified COLOR_INACTIVEBORDER enumeration value.
			/// </summary>
			COLOR_INACTIVEBORDER =				11,
			/// <summary>
			/// Specified COLOR_APPWORKSPACE enumeration value.
			/// </summary>
			COLOR_APPWORKSPACE =				12,
			/// <summary>
			/// Specified COLOR_HIGHLIGHT enumeration value.
			/// </summary>
			COLOR_HIGHLIGHT =					13,
			/// <summary>
			/// Specified COLOR_HIGHLIGHTTEXT enumeration value.
			/// </summary>
			COLOR_HIGHLIGHTTEXT =				14,
			/// <summary>
			/// Specified COLOR_BTNFACE enumeration value.
			/// </summary>
			COLOR_BTNFACE =						15,
			/// <summary>
			/// Specified COLOR_BTNSHADOW enumeration value.
			/// </summary>
			COLOR_BTNSHADOW =					16,
			/// <summary>
			/// Specified COLOR_GRAYTEXT enumeration value.
			/// </summary>
			COLOR_GRAYTEXT =					17,
			/// <summary>
			/// Specified COLOR_BTNTEXT enumeration value.
			/// </summary>
			COLOR_BTNTEXT =						18,
			/// <summary>
			/// Specified COLOR_INACTIVECAPTIONTEXT enumeration value.
			/// </summary>
			COLOR_INACTIVECAPTIONTEXT =			19,
			/// <summary>
			/// Specified COLOR_BTNHIGHLIGHT enumeration value.
			/// </summary>
			COLOR_BTNHIGHLIGHT =				20,
			/// <summary>
			/// Specified COLOR_3DDKSHADOW enumeration value.
			/// </summary>
			COLOR_3DDKSHADOW =					21,
			/// <summary>
			/// Specified COLOR_3DLIGHT enumeration value.
			/// </summary>
			COLOR_3DLIGHT =						22,
			/// <summary>
			/// Specified COLOR_INFOTEXT enumeration value.
			/// </summary>
			COLOR_INFOTEXT =					23,
			/// <summary>
			/// Specified COLOR_INFOBK enumeration value.
			/// </summary>
			COLOR_INFOBK =						24,
			/// <summary>
			/// Specified COLOR_HOTLIGHT enumeration value.
			/// </summary>
			COLOR_HOTLIGHT =					26,
			/// <summary>
			/// Specified COLOR_GRADIENTACTIVECAPTION enumeration value.
			/// </summary>
			COLOR_GRADIENTACTIVECAPTION =		27,
			/// <summary>
			/// Specified COLOR_GRADIENTINACTIVECAPTION enumeration value.
			/// </summary>
			COLOR_GRADIENTINACTIVECAPTION  =	28,
			/// <summary>
			/// Specified COLOR_MENUHILIGHT enumeration value.
			/// </summary>
			COLOR_MENUHILIGHT =					29,
			/// <summary>
			/// Specified COLOR_MENUBAR enumeration value.
			/// </summary>
			COLOR_MENUBAR =						30
		}
		#endregion

		#region ShowWindowCommand
		/// <summary>
		/// Specifies values from ShowWindowCommand enumeration.
		/// </summary>
		public enum ShowWindowCommand
		{
			/// <summary>
			/// Specified SW_HIDE enumeration value.
			/// </summary>
			SW_HIDE =				0,
			/// <summary>
			/// Specified SW_SHOWNORMAL enumeration value.
			/// </summary>
			SW_SHOWNORMAL =			1,
			/// <summary>
			/// Specified SW_NORMAL enumeration value.
			/// </summary>
			SW_NORMAL =				1,
			/// <summary>
			/// Specified SW_SHOWMINIMIZED enumeration value.
			/// </summary>
			SW_SHOWMINIMIZED =		2,
			/// <summary>
			/// Specified SW_SHOWMAXIMIZED enumeration value.
			/// </summary>
			SW_SHOWMAXIMIZED =		3,
			/// <summary>
			/// Specified SW_MAXIMIZE enumeration value.
			/// </summary>
			SW_MAXIMIZE =			3,
			/// <summary>
			/// Specified SW_SHOWNOACTIVATE enumeration value.
			/// </summary>
			SW_SHOWNOACTIVATE =		4,
			/// <summary>
			/// Specified SW_SHOW enumeration value.
			/// </summary>
			SW_SHOW =				5,
			/// <summary>
			/// Specified SW_MINIMIZE enumeration value.
			/// </summary>
			SW_MINIMIZE =			6,
			/// <summary>
			/// Specified SW_SHOWMINNOACTIVE enumeration value.
			/// </summary>
			SW_SHOWMINNOACTIVE =	7,
			/// <summary>
			/// Specified SW_SHOWNA enumeration value.
			/// </summary>
			SW_SHOWNA =				8,
			/// <summary>
			/// Specified SW_RESTORE enumeration value.
			/// </summary>
			SW_RESTORE =			9,
			/// <summary>
			/// Specified SW_SHOWDEFAULT enumeration value.
			/// </summary>
			SW_SHOWDEFAULT =		10,
			/// <summary>
			/// Specified SW_FORCEMINIMIZE enumeration value.
			/// </summary>
			SW_FORCEMINIMIZE =		11,
			/// <summary>
			/// Specified SW_MAX enumeration value.
			/// </summary>
			SW_MAX =				11
		}
		#endregion

		#region RECT
		/// <summary>
		/// Win32 RECT structure.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public struct RECT 
		{
			/// <summary>
			/// The left field of the structure.
			/// </summary>
			public Int32 left;
			/// <summary>
			/// The top field of the structure.
			/// </summary>
			public Int32 top;
			/// <summary>
			/// The right field of the structure.
			/// </summary>
			public Int32 right;
			/// <summary>
			/// The bottom field of the structure.
			/// </summary>
			public Int32 bottom;

			/// <summary>
			/// Creates RECT structure.
			/// </summary>
			/// <param name="left">The left field of the structure.</param>
			/// <param name="top">The top field of the structure.</param>
			/// <param name="right">The right field of the structure.</param>
			/// <param name="bottom">The bottom field of the structure.</param>
			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}
		

			/// <summary>
			/// Creates RECT structure.
			/// </summary>
			/// <param name="x">The left field of the structure</param>
			/// <param name="y">The top field of the structure.</param>
			/// <param name="width">The width field of the structure.</param>
			/// <param name="height">The height field of the structure.</param>
			/// <returns>Created RECT structure.</returns>
			public static RECT FromXYWH(int x, int y, int width, int height)
			{
				return new RECT(x, y, x + width, y + height); 
			}
		}
		#endregion

		#region POINT
		/// <summary>
		/// Win32 POINT structure.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public class POINT
		{
			public int x;
			public int y;
			public POINT()
			{
			}

			public POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
 
			public Point ToPoint()
			{
				return new Point(this.x, this.y);
			}
		}
		#endregion

		#region CWPSTRUCT
		/// <summary>
		/// CWPSTRUCT structure.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public struct CWPSTRUCT
		{
			public IntPtr lparam;
			public IntPtr wparam;
			public int message;
			public IntPtr hwnd;
		}
		#endregion

		#region WINDOWPOS
		/// <summary>
		/// WINDOWPOS structure.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			internal struct WINDOWPOS
		{
			public IntPtr hwnd;
			public IntPtr hwndAfter;
			public int x;
			public int y;
			public int cx;
			public int cy;
			public uint flags;
		}
		#endregion

		#region NCCALCSIZE_PARAMS
		/// <summary>
		/// NCCALCSIZE_PARAMS structure.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			internal struct NCCALCSIZE_PARAMS
		{
			public RECT rgc;
			public WINDOWPOS wndpos;
		}
		#endregion

		#region SIZE
		/// <summary>
		/// Win32 SIZE structure.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
			public class SIZE
		{
			public int cx;
			public int cy;

			public SIZE()
			{
			}

			public SIZE(int cx, int cy)
			{
				this.cx = cx;
				this.cy = cy;
			}

			public Size ToSize()
			{
				return new Size(this.cx, this.cy);
			}

		}
		#endregion 

		#region user32.dll
		/// <summary>
		/// GetSysColor function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern int GetSysColor(int color);

		/// <summary>
		/// LockWindowUpdate function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern bool LockWindowUpdate(IntPtr hWnd);

		/// <summary>
		/// GetDC function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern IntPtr GetDC(IntPtr hwnd);

		/// <summary>
		/// GetWindow function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern IntPtr GetWindow(IntPtr hwnd, int wCmd);

		/// <summary>
		/// GetWindowRect function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern bool GetWindowRect(IntPtr hwnd, ref RECT lpRect);

		/// <summary>
		/// SetWindowPos function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, 
			int x, int y, int cx, int cy, int uFlags);

		/// <summary>
		/// PtInRect function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern bool PtInRect(ref RECT lpRect, int x, int y);
		
		/// <summary>
		/// ReleaseDC function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern bool ReleaseDC(IntPtr hwnd, IntPtr hdc);

		/// <summary>
		/// SetWindowText function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern bool SetWindowText(IntPtr hwnd, ref string lpString);

		/// <summary>
		/// ShowWindow function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern int ShowWindow(IntPtr hWnd, short cmdShow);

		/// <summary>
		/// SendMessage function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern int PostMessage(IntPtr hwnd, int msg, int wParam, IntPtr lParam);

		/// <summary>
		/// SendMessage function of USER32.
		/// </summary>
		[DllImport("USER32.DLL", EntryPoint= "GetCaretBlinkTime")]
		public static extern int GetCaretBlinkTime();

		public static Bitmap CaptureWindow(Control control)
		{
			Bitmap bitmap = new Bitmap(control.Width, control.Height);
			using (Graphics g = Graphics.FromImage(bitmap))
			{
				IntPtr hdc = g.GetHdc();
				SendMessage(control.Handle, (int)Msg.WM_PRINT, (int)hdc, new System.IntPtr(12));

				g.ReleaseHdc(hdc);
			}
			return bitmap;
		}




		/// <summary>
		/// SendMessage function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

		/// <summary>
		/// ScrollWindow function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern bool ScrollWindow(IntPtr hWnd,	int xAmount, int yAmount, 
			ref RECT rectScrollRegion, ref RECT rectClip);

		/// <summary>
		/// ScrollWindow function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern bool ScrollWindow(IntPtr hWnd,	int xAmount, int yAmount, 
			IntPtr rectScrollRegion, IntPtr rectClip);

		/// <summary>
		/// ScrollWindowEx function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern bool ScrollWindowEx(IntPtr hWnd, int nXAmount, int nYAmount, 
			IntPtr rectScrollRegion, IntPtr rectClip, IntPtr hrgnUpdate, IntPtr prcUpdate, int flags);

		/// <summary>
		/// GetKeyState function of USER32.
		/// </summary>
		[DllImport("user32.dll", CharSet=CharSet.Auto)]
		public static extern short GetKeyState(int nVirtKey);	
	
		/// <summary>
		/// GetWindowDC function of USER32.
		/// </summary>
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetWindowDC(IntPtr handle);

		/// <summary>
		/// CreateCompatibleDC function of USER32.
		/// </summary>
		[DllImport("Gdi32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		/// <summary>
		/// CallWindowProc function of USER32.
		/// </summary>
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern int CallWindowProc(IntPtr wndProc, IntPtr hwnd, int msg, IntPtr wparam,	IntPtr lparam);

		/// <summary>
		/// UnhookWindowsHookEx function of USER32.
		/// </summary>
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern bool UnhookWindowsHookEx(IntPtr hookHandle);

		/// <summary>
		/// GetWindowThreadProcessId function of USER32.
		/// </summary>
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern int GetWindowThreadProcessId(IntPtr hwnd, int ID);

		/// <summary>
		/// GetClassName function of USER32.
		/// </summary>
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern int GetClassName(IntPtr hwnd, char[] className, int maxCount);

		/// <summary>
		/// CallNextHookEx function of USER32.
		/// </summary>
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern int CallNextHookEx(IntPtr hookHandle, int code, IntPtr wparam, ref CWPSTRUCT cwp);
		#endregion

		#region gdi32.dll
		/// <summary>
		/// CreatePen function of GDI32.
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
		public static extern IntPtr CreatePen(int nStyle, int nWidth, int crColor);

		/// <summary>
		/// MoveToEx function of GDI32.
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
		public static extern bool MoveToEx(IntPtr hdc, int x, int y, IntPtr pt);

		/// <summary>
		/// LineTo function of GDI32.
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
		public static extern bool LineTo(IntPtr hdc, int x, int y); 

		/// <summary>
		/// SetROP2 function of GDI32.
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
		public static extern int SetROP2(IntPtr hDC, int nDrawMode);

		/// <summary>
		/// SelectObject function of GDI32.
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
		public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject); 

		/// <summary>
		/// GetStockObject function of GDI32.
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
		public static extern IntPtr GetStockObject(int nIndex);
		
		/// <summary>
		/// Rectangle function of GDI32.
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
		public static extern bool Rectangle(IntPtr hdc, int left, int top, int right, int bottom); 

		/// <summary>
		/// DeleteObject function of GDI32.
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
		public static extern IntPtr DeleteObject(IntPtr hDC); 

		/// <summary>
		/// SetBkColor function of GDI32.
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
		public static extern int SetBkColor(IntPtr hDC, int clr); 

		/// <summary>
		/// CreateSolidBrush function of GDI32.
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
		public static extern IntPtr CreateSolidBrush(int crColor);

		/// <summary>
		/// PatBlt function of GDI32.
		/// </summary>
		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
		public static extern bool PatBlt(IntPtr hdc, int left, int top, int width, int height, int rop);
		#endregion		 

		#region Constructors
		private Win32()
		{
		}
		#endregion
	}
}
