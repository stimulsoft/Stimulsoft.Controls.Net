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
using System.Drawing;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	public enum StiRtfSelectionAlign
	{
		/// <summary>
		/// The text is aligned to the left.
		/// </summary>
		Left = 1,
    
		/// <summary>
		/// The text is aligned to the right.
		/// </summary>
		Right = 2,
    
		/// <summary>
		/// The text is aligned in the center.
		/// </summary>
		Center = 3,
    
		/// <summary>
		/// The text is justified.
		/// </summary>
		Justify = 4
	}

	/// <summary>
	/// Represents a Windows rich text box control, with some impovements.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiRichTextBox), "Toolbox.StiRichTextBox.bmp")]
	[System.Security.SuppressUnmanagedCodeSecurity]
	public class StiRichTextBox : RichTextBox
	{	
		#region PARAFORMAT
		[StructLayout( LayoutKind.Sequential )]
		private struct PARAFORMAT
		{
			public int cbSize;
			public uint dwMask;
			public short wNumbering;
			public short wReserved;
			public int dxStartIndent;
			public int dxRightIndent;
			public int dxOffset;
			public short wAlignment;
			public short cTabCount;
			[MarshalAs( UnmanagedType.ByValArray, SizeConst = 32 )]
			public int[] rgxTabs;
        
			// PARAFORMAT2 from here onwards.
			public int dySpaceBefore;
			public int dySpaceAfter;
			public int dyLineSpacing;
			public short sStyle;
			public byte bLineSpacingRule;
			public byte bOutlineLevel;
			public short wShadingWeight;
			public short wShadingStyle;
			public short wNumberingStart;
			public short wNumberingStyle;
			public short wNumberingTab;
			public short wBorderSpace;
			public short wBorderWidth;
			public short wBorders;
		}

		#endregion

		#region CHARFORMATSTRUCT
		[StructLayout( LayoutKind.Sequential)]
			private struct CHARFORMATSTRUCT
		{
			public int      cbSize; 
			public UInt32   dwMask; 
			public UInt32   dwEffects; 
			public Int32    yHeight; 
			public Int32    yOffset; 
			public Int32	crTextColor; 
			public byte     bCharSet; 
			public byte     bPitchAndFamily; 
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=32)]
			public char[]   szFaceName; 
		}
		#endregion

		#region Handlers
		protected override void OnHandleCreated( EventArgs e )
		{
			base.OnHandleCreated( e );
        
			// Enable support for justification.
			SendMessage( new HandleRef( this, Handle ),
				EM_SETTYPOGRAPHYOPTIONS,
				TO_ADVANCEDTYPOGRAPHY,
				TO_ADVANCEDTYPOGRAPHY );
		}

		#endregion

		#region Properties
		public new StiRtfSelectionAlign SelectionAlignment
		{
			get
			{
				PARAFORMAT fmt = new PARAFORMAT();
				fmt.cbSize = Marshal.SizeOf( fmt );
            
				// Get the alignment.
				SendMessage( new HandleRef( this, Handle ),
					EM_GETPARAFORMAT,
					SCF_SELECTION, ref fmt );
            
				// Default to Left align.
				if ( ( fmt.dwMask & PFM_ALIGNMENT ) == 0 )
					return StiRtfSelectionAlign.Left;
            
				return ( StiRtfSelectionAlign )fmt.wAlignment;
			}
        
			set
			{
				PARAFORMAT fmt = new PARAFORMAT();
				fmt.cbSize = Marshal.SizeOf( fmt );
				fmt.dwMask = PFM_ALIGNMENT;
				fmt.wAlignment = ( short )value;
            
				// Set the alignment.
				SendMessage( new HandleRef( this, Handle ),
					EM_SETPARAFORMAT,
					SCF_SELECTION, ref fmt );
			}
		}
		#endregion

		#region Fields
		private const int EM_SETEVENTMASK = 1073;
		private const int EM_GETPARAFORMAT = 1085;
		private const int EM_SETPARAFORMAT = 1095;
		private const int EM_SETTYPOGRAPHYOPTIONS = 1226;
		private const int WM_SETREDRAW = 11;
		private const int TO_ADVANCEDTYPOGRAPHY = 1;
		private const int PFM_ALIGNMENT = 8;
		private const int SCF_SELECTION = 1;

		private const Int32 SCF_WORD		= 0x0002;
		private const Int32 SCF_ALL			= 0x0004;
		
		private const UInt32 CFM_BOLD		= 0x00000001;
		private const UInt32 CFM_ITALIC		= 0x00000002;
		private const UInt32 CFM_UNDERLINE	= 0x00000004;
		private const UInt32 CFM_STRIKEOUT	= 0x00000008;
		private const UInt32 CFM_PROTECTED	= 0x00000010;
		private const UInt32 CFM_LINK		= 0x00000020;
		private const UInt32 CFM_SIZE		= 0x80000000;
		private const UInt32 CFM_COLOR		= 0x40000000;
		private const UInt32 CFM_FACE		= 0x20000000;
		private const UInt32 CFM_OFFSET		= 0x10000000;
		private const UInt32 CFM_CHARSET	= 0x08000000;

		private const UInt32 CFE_BOLD		= 0x00000001;
		private const UInt32 CFE_ITALIC		= 0x00000002;
		private const UInt32 CFE_UNDERLINE	= 0x00000004;
		private const UInt32 CFE_STRIKEOUT	= 0x00000008;
		private const UInt32 CFE_PROTECTED	= 0x00000010;
		private const UInt32 CFE_LINK		= 0x00000020;
		private const UInt32 CFE_AUTOCOLOR	= 0x40000000;
		#endregion

		#region Methods
		[DllImport( "user32", CharSet = CharSet.Auto )]
		private static extern int SendMessage( HandleRef hWnd,
			int msg,
			int wParam,
			int lParam );
    
		[DllImport( "user32", CharSet = CharSet.Auto )]
		private static extern int SendMessage( HandleRef hWnd,
			int msg,
			int wParam,
			ref PARAFORMAT lp );

		[DllImport( "user32", CharSet = CharSet.Auto )]
		private static extern int SendMessage( HandleRef hWnd,
			int msg,
			int wParam,
			ref CHARFORMATSTRUCT lp );


		public bool SetSelectionFont(string face)
		{
			CHARFORMATSTRUCT cf = new CHARFORMATSTRUCT();
			cf.cbSize = Marshal.SizeOf(cf);
			cf.szFaceName = new char[32];
			cf.dwMask = CFM_FACE;
			face.CopyTo(0, cf.szFaceName, 0, Math.Min(31, face.Length));

			IntPtr lParam = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf)); 
			Marshal.StructureToPtr(cf, lParam, false);

			int res = Win32.SendMessage(Handle, (int)Win32.Msg.EM_SETCHARFORMAT, SCF_SELECTION, lParam);
			return (res==0);
		}

		public bool SetSelectionSize(int size)
		{
			CHARFORMATSTRUCT cf = new CHARFORMATSTRUCT();
			cf.cbSize = Marshal.SizeOf(cf);
			cf.dwMask = CFM_SIZE;

			cf.yHeight = size*20;

			IntPtr lParam = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf)); 
			Marshal.StructureToPtr(cf, lParam, false);

			int res = Win32.SendMessage(Handle, (int)Win32.Msg.EM_SETCHARFORMAT, SCF_SELECTION, lParam);
			return (res==0);
		}

		public bool SetSelectionBold(bool bold)
		{
			return SetSelectionStyle(CFM_BOLD, bold ? CFE_BOLD : 0);
		}

		public bool SetSelectionItalic(bool italic)
		{
			return SetSelectionStyle(CFM_ITALIC, italic ? CFE_ITALIC : 0);
		}

		public bool SetSelectionUnderlined(bool underlined)
		{
			return SetSelectionStyle(CFM_UNDERLINE, underlined ? CFE_UNDERLINE : 0);
		}

		private bool SetSelectionStyle(UInt32 mask, UInt32 effect)
		{			
			CHARFORMATSTRUCT cf = new CHARFORMATSTRUCT();
			cf.cbSize = Marshal.SizeOf(cf);
			cf.dwMask = mask;
			cf.dwEffects = effect;

			IntPtr lParam = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf)); 
			Marshal.StructureToPtr(cf, lParam, false);

			int res = Win32.SendMessage(Handle, (int)Win32.Msg.EM_SETCHARFORMAT, SCF_SELECTION, lParam);
			return (res==0);
		}
		#endregion
	}
}
