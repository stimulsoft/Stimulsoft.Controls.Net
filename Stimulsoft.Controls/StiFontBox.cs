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

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base;

namespace Stimulsoft.Controls
{	
	/// <summary>
	/// Allows the user to select a font.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiFontBox), "Toolbox.StiFontBox.bmp")]
	public class StiFontBox : StiComboBox
	{
		#region Fields
		private bool drawCombo = false;
		#endregion

		#region Browsable(false)
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool UseFirstImage
		{
			get
			{
				return base.UseFirstImage;
			}
			set
			{
			}
		}

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string DisplayMember
		{
			get
			{
				return base.DisplayMember;
			}
			set
			{
				base.DisplayMember = value;
			}
		}

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new object DataSource
		{
			get
			{
				return base.DataSource;
			}
			set
			{
				base.DataSource = value;
			}
		}

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new string ValueMember
		{
			get
			{
				return base.ValueMember;
			}
			set
			{
				base.ValueMember = value;
			}
		}

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ComboBoxStyle DropDownStyle
		{
			get
			{
				return base.DropDownStyle;
			}
			set
			{
				base.DropDownStyle = value;
			}
		}


		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override ImageList ImageList
		{
			get
			{
				return base.ImageList;
			}
			set
			{
			}
		}

		

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ComboBox.ObjectCollection Items
		{
			get
			{
				return base.Items;
			}
		}

		#endregion

		#region Properties
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}


		/// <summary>
		/// Gets or sets a Selected Font.
		/// </summary>
		[Category("Behavior")]
		public virtual Font SelectedFont
		{
			get
			{
				return new Font(this.Text, 8);
			}
			set
			{
				//this.Selected = value;
				this.Text = value.Name;
				
				Invalidate();
			}
		}
		#endregion

		#region Methods
		protected override void Draw(bool fillContent)
		{
			base.Draw(fillContent);
			using (var g = Graphics.FromHwnd(this.Handle))
			{	
				Rectangle rect = StiControlPaint.GetContentRect(new Rectangle(0, 0, Width - 1, Height - 1), 
					true, RightToLeft);
				rect.Inflate(-2, -2);
				//rect.Width--;
				rect.Height++;
				
				var args = new DrawItemEventArgs(g, Font, rect, -1, 
					Focused ? DrawItemState.Selected : DrawItemState.Default);

				drawCombo = true;
				OnDrawItem(args);
				drawCombo = false;
			}
		}

		private void Fill()
		{
			try
			{
                var items = new List<string>();

				int index = 0;
				int selIndex = -1;
                foreach (var fontFamily in StiFontCollection.GetFontFamilies())
				{
					if (fontFamily.IsStyleAvailable(FontStyle.Regular))
					{
						items.Add(fontFamily.Name);
						if (this.Text == fontFamily.Name)selIndex = index;
						index++;
					}
				}			
				base.Items.Clear();
				base.Items.AddRange(items.ToArray());
				this.SelectedIndex = selIndex;
			}
			catch
			{
			}
		}
		
		#endregion
		
		#region Handlers
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			base.OnMeasureItem(e);
			Graphics g = e.Graphics;
			string str = Items[e.Index].ToString();
			e.ItemWidth = (int)g.MeasureString(str, this.Font).Width + 
				SystemInformation.HorizontalScrollBarThumbWidth + 35;
		}
		protected override void OnDrawItem(DrawItemEventArgs e)
		{	
			if ((e.Index != -1 || drawCombo) && Enabled)
			{
				string fontName = SelectedFont.Name;

				if (e.Index != -1)fontName = Items[e.Index].ToString();

				Rectangle itemRect = e.Bounds;			
				if ((e.State & DrawItemState.Selected) > 0)itemRect.Width--;

				StiControlPaint.DrawItem(e.Graphics, itemRect, e.State, fontName,
					null, e.Index, Font, BackColor, ForeColor, 30, base.RightToLeft);

				var rect = new Rectangle(e.Bounds.X, e.Bounds.Y, 28, e.Bounds.Height - 1);
				var g = e.Graphics;

				if (((e.State & DrawItemState.Focus) > 0) || ((e.State & DrawItemState.Selected) > 0))
				{
					g.DrawRectangle(StiPens.SelectedText, rect);
				}

				if (Enabled)
				{
					#region Sample draw
					using (var sf = new StringFormat())
					{
						sf.LineAlignment = StringAlignment.Center;
						sf.Alignment = StringAlignment.Center;
						sf.FormatFlags = StringFormatFlags.NoWrap;

						if (this.RightToLeft == RightToLeft.Yes)
							sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

						using (var fnt = new Font(fontName, 10))
						{
							g.DrawString("ab", fnt, StiBrushes.SelectedText, rect, sf);
						}
					}
					#endregion
				}
			}
		}

		#endregion

		#region Constructors
		public StiFontBox()
		{	
			DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;

			this.Text = "Arial";
			ItemHeight = 14;
			DropDownStyle = ComboBoxStyle.DropDownList;
			Fill();
		}
		#endregion
	}

}
