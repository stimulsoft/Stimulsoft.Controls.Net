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

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Stimulsoft.Controls
{
	internal class StiImageIndexEditor : UITypeEditor 
	{
		private ImageList imageList;
		private UITypeEditor imageEditor;

		private void SetImageList(ITypeDescriptorContext context)
		{
			if (context != null && context.Container != null)
			{
				foreach (object comp in context.Container.Components)
				{
					if (comp is StiMenuProvider)
					{
						this.imageList = ((StiMenuProvider)comp).GetImageList((MenuItem)context.Instance);
						return;
					}
				}
			}
		}
		
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			SetImageList(context);
			return base.GetEditStyle(context);
		} 

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return this.imageEditor.GetPaintValueSupported(context); 
		} 
		public override void PaintValue(PaintValueEventArgs e)
		{
			SetImageList(e.Context);
			if (this.imageList == null || this.imageList.Images.Count == 0)return; 
			if (this.imageEditor != null && e.Value is int)
			{
				int index = (int)e.Value;
				if (index >= 0 && index <= this.imageList.Images.Count)
					this.imageEditor.PaintValue(
						new PaintValueEventArgs(e.Context, this.imageList.Images[index], e.Graphics, e.Bounds));
 
			}
		} 


		#region Constructors
		public StiImageIndexEditor()
		{
			this.imageEditor = ((UITypeEditor)TypeDescriptor.GetEditor(typeof(Image), typeof(UITypeEditor)));
		} 
		#endregion
	}
}
