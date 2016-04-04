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

using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Single tab page in a StiWizard.
	/// </summary>
	[ToolboxItem(false)]
    #if !Profile
	[Designer(typeof(StiWizardPageDesigner))]
    #endif
	public class StiWizardPage : ContainerControl
	{
		#region Browsable(false)
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		public new bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				if (Parent != null && this == ((StiWizard)Parent).SelectedPage)
				{
					base.Visible = true;
					Dock = DockStyle.Fill;
				}
				else base.Visible = value;
			}
		}

		
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		public new DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = value;
			}
		}


		/// <summary>
		/// Gets or sets the text associated with this control.
		/// </summary>
		[Browsable(true)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				Invalidate();
				if (Parent != null)((StiWizard)Parent).Invalidate();
			}
		}

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ScrollableControl.DockPaddingEdges DockPadding
		{
			get
			{
				return base.DockPadding;
			}
		}
		#endregion

		#region Properties
		[Browsable(false)]
		public int PageIndex
		{
			get
			{
				if (Parent != null)return Parent.Controls.IndexOf(this);
				return -1;
			}
		}


		private Image image = null;
		[DefaultValue(null)]
		[Category("Appearance")]
		public virtual Image Image
		{
			get
			{
				return image;
			}
			set
			{
				image = value;
				if (value is Bitmap)((Bitmap)value).MakeTransparent();
				Invalidate();
				if (Parent != null)Parent.Invalidate();
			}
		}

		private string description = string.Empty;
		[DefaultValue("")]
		[Category("Appearance")]
		public string Description
		{
			get
			{
				return description;
			}
			set
			{	
				description = value;

				Invalidate();
				if (Parent != null)Parent.Invalidate();
			}
		}
		#endregion

		#region Constructors
		public StiWizardPage() : this("TabulatorPage")
		{
		}

		public StiWizardPage(string text)
		{
			this.DockPadding.All = 2;
			this.Text = text;

			this.Visible = false;

			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
		}
		#endregion
	}
}
