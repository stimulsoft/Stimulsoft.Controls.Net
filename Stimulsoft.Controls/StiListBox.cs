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
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Represents a Windows list box control.
	/// </summary>
	[ToolboxItem(false)]
	public class StiListBox : ListBox
	{
		#region Browsable(false)
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new System.Windows.Forms.DrawMode DrawMode
		{
			get
			{
				return base.DrawMode;
			}
			set
			{
				base.DrawMode = value;
			}
		}
		#endregion

		#region Properties
		private DrawItemEventHandler GetDrawItemHandler()
		{
			FieldInfo info = typeof(ComboBox).GetField("EVENT_DRAWITEM", 
				System.Reflection.BindingFlags.Static | 
				System.Reflection.BindingFlags.NonPublic | 
				System.Reflection.BindingFlags.GetField);
            
			DrawItemEventHandler handler = (DrawItemEventHandler)base.Events[info.GetValue(null)];

			return handler;
		}

		private bool useFirstImage = false;
		/// <summary>
		/// Use always first Image from ImageList to Draw item.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]		
		public bool UseFirstImage
		{
			get
			{
				return useFirstImage;
			}
			set
			{
				useFirstImage = value;
				Invalidate();
			}
		}


		private bool hotTrack = false;
		/// <summary>
		/// Use always first Image from ImageList to Draw item.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]
		public bool HotTrack
		{
			get
			{
				return hotTrack;
			}
			set
			{
				hotTrack = value;
				Invalidate();
			}
		}


		private ImageList imageList = null;
		/// <summary>
		/// Gets or sets the collection of images available to the StiListBoxBase items.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(null)]
		public ImageList ImageList
		{
			get
			{
				return imageList;
			}
			set
			{
				imageList = value;
				Invalidate();
			}
		}

		#endregion

		#region Handlers
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			StiColors.InitColors();
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			DrawItemEventHandler handler = GetDrawItemHandler();
			if (handler == null)
			{
				Graphics g = e.Graphics;
				Rectangle itemRect = e.Bounds;
				if ((e.State & DrawItemState.Selected) > 0)itemRect.Width--;
				DrawItemState state = e.State;

				if (e.Index != -1 && Items.Count > 0)
				{
					int imageIndex = UseFirstImage ? 0 : e.Index;
					StiControlPaint.DrawItem(g, itemRect, state, this.GetItemText(Items[e.Index]), 
						ImageList, imageIndex, Font, BackColor, ForeColor, RightToLeft);							
				}
			}
			else
			{
				handler(this, e);
			}
		}
	
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (HotTrack)
			{
				Point pos = PointToClient(Cursor.Position);
				for (int index = 0; index < Items.Count; index++)
				{
					Rectangle rect = GetItemRectangle(index);
					if (rect.Contains(pos))
					{
						SelectedIndex = index;
						break;
					}
				}
			}
		}
		#endregion

		#region Constructors
		public StiListBox()
		{
            DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;          			
		}
		#endregion
	}
}