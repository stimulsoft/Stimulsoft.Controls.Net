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
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// A text edit with buttons.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiTabControl), "Toolbox.StiButtonEdit.bmp")]
	public class StiButtonEdit : StiButtonEditBase
	{
		#region Properties
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override StiButtonAlign ButtonAlign
		{
			get
			{
				return base.ButtonAlign;
			}
			set
			{
				base.ButtonAlign = value;
			}
		}

		
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override int ButtonWidth
		{
			get
			{
				return base.ButtonWidth;
			}
			set
			{
				base.ButtonWidth = value;
			}
		}


		private Icon buttonIcon;
		/// <summary>
		/// An Icon that represents the icon for the button.
		/// </summary>
		[Category("Behavior")]
		public Icon ButtonIcon
		{ 
			get
			{
				return buttonIcon;
			}
			set
			{
				buttonIcon = value;
				if (value == null)this.ButtonBitmap = null;
                else this.ButtonBitmap = buttonIcon.ToBitmap();
				Invalidate();
			}
		}


		private Image image = null;
		[DefaultValue(null)]
		[Category("Appearance")]
		public Image Image
		{
			get
			{
				return this.image;
			}
			set
			{
				if (this.Image != value)
				{
					this.image = value;
					if (this.image != null)
					{
						if (image is Bitmap)
						{
							StiImageUtils.MakeImageBackgroundAlphaZero(this.image as Bitmap);
						}
					}
					this.ButtonBitmap = image;
					base.Invalidate();
				}
			}
		}
		#endregion

		#region Handlers
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			this.Focus();
		}
		#endregion
	}
}
