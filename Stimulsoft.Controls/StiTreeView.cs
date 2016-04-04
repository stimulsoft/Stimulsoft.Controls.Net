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
using System.ComponentModel;
using System.Runtime.InteropServices;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	public enum StiTreeViewDrawMode
	{
		Normal = 0,
		OwnerDraw = 1,
	}

	public delegate void StiDrawTreeNodeEventHandler(object sender, StiDrawTreeNodeEventArgs e);

	/// <summary>
	/// Summary description for StiDrawTreeNodeEventArgs.
	/// </summary>
	public class StiDrawTreeNodeEventArgs
	{
		private readonly Rectangle bounds;
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}


		private readonly Graphics graphics;
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}


		private readonly TreeNode node;
		public TreeNode Node
		{
			get
			{
				return this.node;
			}
		}
 

		public StiDrawTreeNodeEventArgs(Graphics graphics, TreeNode node, Rectangle bounds)
		{
			this.graphics = graphics;
			this.node = node;
			this.bounds = bounds;
		}
	}

	#region TVITEMEX
	[StructLayout(LayoutKind.Sequential)]
	internal struct TVITEMEX 
	{ 
 		public int mask; 
 		public IntPtr hItem; 
 		public int state; 
 		public int stateMask; 
 		public string pszText; 
 		public int cchTextMax; 
 		public int iImage; 
 		public int iSelectedImage; 
 		public int cChildren; 
 		public IntPtr lParam; 
 		public int iIntegral; 
	}
	#endregion

	/// <summary>
	/// Displays an hierarchical collection of labeled items, with some impovements.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiTreeView), "Toolbox.StiTreeView.bmp")]
	public class StiTreeView : TreeView
	{
		#region Structs
		[StructLayout(LayoutKind.Sequential)]
			private struct RECT
		{
			internal int left;
			internal int top;
			internal int right;
			internal int bottom;
		}


		[StructLayout(LayoutKind.Sequential)]
			private struct NMHDR
		{
			internal IntPtr hwndFrom;
			internal IntPtr idFrom;
			internal int code;
		}
	

		[StructLayout(LayoutKind.Sequential)]
			private struct NMCUSTOMDRAW
		{
			internal NMHDR hdr;
			internal int dwDrawStage;
			internal IntPtr hdc;
			internal RECT rc; 
			internal IntPtr dwItemSpec;
			internal int uItemState;
			internal IntPtr lItemlParam;
		}


		[StructLayout(LayoutKind.Sequential)]
			private struct NMTVCUSTOMDRAW
		{
			internal NMCUSTOMDRAW nmcd;
			internal int clrText;
			internal int clrTextBk;
			internal int iLevel;
		}

		#endregion

		#region Const
		private const int NM_FIRST = 0;
		private const int NM_CUSTOMDRAW = (NM_FIRST - 12);

		private const int CDDS_PREPAINT = 0x1;
		private const int CDDS_POSTPAINT = 0x2;
		private const int CDDS_PREERASE = 0x3;
		private const int CDDS_POSTERASE = 0x4;
		private const int CDDS_ITEM = 0x10000;
		private const int CDDS_ITEMPREPAINT = (CDDS_ITEM | CDDS_PREPAINT);
		private const int CDDS_ITEMPOSTPAINT = (CDDS_ITEM | CDDS_POSTPAINT);
		private const int CDDS_ITEMPREERASE = (CDDS_ITEM | CDDS_PREERASE);
		private const int CDDS_ITEMPOSTERASE = (CDDS_ITEM | CDDS_POSTERASE);
		private const int CDDS_SUBITEM = 0x20000;

		private const int CDRF_DODEFAULT = 0x0;
		private const int CDRF_NEWFONT = 0x2;
		private const int CDRF_SKIPDEFAULT = 0x4;
		private const int CDRF_NOTIFYPOSTPAINT = 0x10;
		private const int CDRF_NOTIFYITEMDRAW = 0x20;
		private const int CDRF_NOTIFYSUBITEMDRAW = 0x20;
		private const int CDRF_NOTIFYPOSTERASE = 0x40;
		private const int WM_NOTIFY = 0x4E;
		#endregion		
		
		#region Properties
		private StiTreeViewDrawMode drawMode = StiTreeViewDrawMode.Normal;
		[DefaultValue(StiTreeViewDrawMode.Normal)]
		[Category("Behavior")]
        public new StiTreeViewDrawMode DrawMode
		{
			get
			{
				return this.drawMode;
			}
			set
			{
				if (this.drawMode != value)
				{
					this.drawMode = value;
					this.Invalidate();
				}
			}
		}
		#endregion

		#region Fields
		private const int TV_FIRST = 0x1100;
		private const int TVM_SETITEMW = TV_FIRST + 63;
		private const int TVIF_HANDLE = 0x0010;
		private const int TVIF_STATE = 0x0008;
		private const int TVIS_BOLD = 0x0010;
		#endregion

		#region Handlers
		protected override void WndProc(ref Message msg)
		{
			if (DrawMode == StiTreeViewDrawMode.OwnerDraw)
			{
				if (msg.Msg == (0x2000 | WM_NOTIFY))
				{
					if (msg.WParam.Equals(this.Handle))
					{
						msg.Result = new IntPtr(HandleNotify(ref msg));
						return;
					}
				}
			}
			base.WndProc(ref msg);
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			Point pos = PointToClient(Cursor.Position);
			TreeNode clickedNode = GetNodeAt(pos);
			SelectedNode = clickedNode;
		}

		#endregion

		#region Methods

        private int HandleNotify(ref Message m)
		{
			if (!m.LParam.Equals(IntPtr.Zero))
			{
				object tempObject = m.GetLParam(typeof(NMHDR));
				if (tempObject is NMHDR)
				{
					if (((NMHDR)tempObject).code == NM_CUSTOMDRAW)
					{
						tempObject = m.GetLParam(typeof(NMTVCUSTOMDRAW));
						if (tempObject is NMTVCUSTOMDRAW)
						{
							switch (((NMTVCUSTOMDRAW)tempObject).nmcd.dwDrawStage)
							{
								case CDDS_PREPAINT:
									return CDRF_NOTIFYITEMDRAW;

								case CDDS_ITEMPREPAINT:
									return CDRF_NOTIFYPOSTPAINT;

								case CDDS_ITEMPOSTPAINT:

									TreeNode treeNode = TreeNode.FromHandle(this, ((NMTVCUSTOMDRAW)tempObject).nmcd.dwItemSpec);
									Graphics g = Graphics.FromHdc(((NMTVCUSTOMDRAW)tempObject).nmcd.hdc);
									OnDrawNode(new StiDrawTreeNodeEventArgs(g, treeNode, treeNode.Bounds));
									g.Dispose();

									return CDRF_DODEFAULT;
							}
						}
					}
				}
			}
			return 0;
		}


		/// <summary>
		/// Sets specified TreeNode Bold mode.
		/// </summary>
		/// <param name="node">Specified TreeNode.</param>
		/// <param name="isBold">Bold mode.</param>
		public void SetBold(TreeNode node, bool isBold)
		{
			SetBold(this, node, isBold);
		}

		/// <summary>
		/// Sets specified TreeNode Bold mode.
		/// </summary>
		/// <param name="node">Specified TreeNode.</param>
		/// <param name="isBold">Bold mode.</param>
		public static void SetBold(TreeView treeView, TreeNode node, bool isBold)
		{
			bool is64Bit = IntPtr.Size == 8;
			if (is64Bit) return;

			try
			{
				if (node.Handle != IntPtr.Zero)
				{
 					TVITEMEX tvItemEx = new TVITEMEX(); 
 			
					tvItemEx.mask = TVIF_HANDLE | TVIF_STATE; 
 			
					tvItemEx.hItem = node.Handle; 
 					tvItemEx.state = isBold ? TVIS_BOLD : 0; 
 					tvItemEx.stateMask = TVIS_BOLD;

					IntPtr pointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TVITEMEX))); 
 					Marshal.StructureToPtr(tvItemEx, pointer, true); 
 					Win32.SendMessage(treeView.Handle, TVM_SETITEMW, 0, pointer); 
 					Marshal.FreeHGlobal(pointer);
				}
			}
			catch
			{
			}
		}
		

		/// <summary>
		/// Disables any updating of the tree view.
		/// </summary>
		public void Lock()
		{
			Win32.LockWindowUpdate(this.Handle);
		}

		
		/// <summary>
		/// Enables the updating of the tree view.
		/// </summary>
		public void Unlock()
		{
			Win32.LockWindowUpdate(IntPtr.Zero);
		}
		#endregion

		#region Events
		[Category("Behavior")]
        public new event StiDrawTreeNodeEventHandler DrawNode;

        protected virtual void OnDrawNode(StiDrawTreeNodeEventArgs e)
		{
			if (this.DrawNode != null)this.DrawNode(this, e);
		}
		#endregion
	}
}
