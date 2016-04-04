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
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Summary description for StiColorBoxForm.
	/// </summary>
	public class StiColorBoxPopupForm : StiPopupForm
	{
		#region Fields
		public Color SelectedColor;
		private Panel[] panels;
		private Control colorBox = null;

		public StiTabControl TabControl = null;
		private StiTabPage tbSystem = null;
		private StiTabPage tbCustom = null;
		private StiTabPage tbWeb = null;
		private ListBox lbSystem = null;
		private ListBox lbWeb = null;
		private StiButton btOther = null;

		private IWindowsFormsEditorService edSrv = null;		
		#endregion		

		#region Methods
		private void Localize()
		{
			tbWeb.Text =	StiLocalization.Get("FormColorBoxPopup", "Web");
			tbSystem.Text = StiLocalization.Get("FormColorBoxPopup", "System");
			tbCustom.Text = StiLocalization.Get("FormColorBoxPopup", "Custom");
			btOther.Text =	StiLocalization.Get("FormColorBoxPopup", "Others");
		}

		protected override bool UseCreateParams
		{
			get
			{
				return edSrv == null;
			}
		}

		public override void SetLocation(int x, int y, int width, int height)
		{
			base.SetLocation(x, y, width, height);

			this.DockPadding.Left += 2;
			this.DockPadding.Top += 2;
			this.DockPadding.Right += 2;
			this.DockPadding.Bottom += 2;
		}

		public override bool PreFilterMessage(ref System.Windows.Forms.Message m)
		{
			if (edSrv == null) return base.PreFilterMessage(ref m);
			return false;
		}
		
		
		public override void ClosePopupForm()
		{			
			base.ClosePopupForm();
			if (edSrv != null)edSrv.CloseDropDown();
		}

		private void InitColors()
		{
			panels = new Panel[StiColors.CustomColors.Length];

			lbWeb.BeginUpdate();
			lbSystem.BeginUpdate();

			lbWeb.Items.Clear();
			foreach (Color color in StiColors.Colors)
			{
				lbWeb.Items.Add(color);
			}
			
			lbWeb.MouseUp += new MouseEventHandler(lbWeb_MouseUp);

			lbSystem.Items.Clear();
			foreach (Color color in StiColors.SystemColors)
				lbSystem.Items.Add(color);

			lbWeb.EndUpdate();
			lbSystem.EndUpdate();
			lbSystem.MouseUp += new MouseEventHandler(lbSystem_MouseUp);
			

			#region Init custom colors
			int startX = 6;
			int startY = 6;
			int x = startX;
			int y = startY;	
		
			for (int a = 0; a < StiColors.CustomColors.Length; a++)
			{
				panels[a] = new Panel();
				panels[a].Left = x;
				panels[a].Top = y;
				panels[a].Height = 24;
				panels[a].Width = 24;
				panels[a].BackColor = StiColors.CustomColors[a];
				panels[a].MouseEnter += new EventHandler(OnMouseEnterPanel);
				panels[a].MouseLeave += new EventHandler(OnMouseLeavePanel);
				panels[a].MouseDown += new MouseEventHandler(OnMouseDownPanel);
				panels[a].MouseUp += new MouseEventHandler(OnMouseUpPanel);
				panels[a].Paint += new PaintEventHandler(OnPanelPaint);
					
				this.tbCustom.Controls.Add(panels[a]);
			
				x += 24;
				if (x > (tbCustom.Width - 5))
				{
					x = startX;
					y += 26;
				}
			}
			#endregion
			
		}

		#endregion
		
		#region Paint item
		private void lbSystem_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			DrawItem(false, sender, e);
		}

		private void lbWeb_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			DrawItem(true, sender, e);
		}

		private void DrawItem(bool web, object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			if (e.Index != -1)
			{
				Color color = Color.Empty;
				string colorName = null;

				if (web)
				{
					color = StiColors.Colors[e.Index];
					colorName = StiLocalization.Get("PropertyColor", color.Name);
				}
				else
				{
					color = StiColors.SystemColors[e.Index];
					colorName = StiLocalization.Get("PropertySystemColors", color.Name);
				}

				var g = e.Graphics;
				var rect = e.Bounds; 
				if ((e.State & DrawItemState.Selected) > 0)rect.Width--;

				var colorRect = new Rectangle(rect.X + 2, rect.Y + 2, 22, rect.Height - 5);
				var textRect = new Rectangle(colorRect.Right + 2, rect.Y, rect.Width - colorRect.X, rect.Height);

				StiControlPaint.DrawItem(g, rect, e.State, SystemColors.Window, SystemColors.ControlText);

				using (var brush = new SolidBrush(color))
				{
					g.FillRectangle(brush, colorRect);
				}
				g.DrawRectangle(Pens.Black, colorRect);

				using (var sf = new StringFormat())
				{
					if (this.RightToLeft == RightToLeft.Yes)
						sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

					g.DrawString(colorName, Font, Brushes.Black, textRect, sf);
				}
			}
		}

		#endregion

		#region Handlers
		private void lbWeb_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ListBox listBox = sender as ListBox;
			Point pos = listBox.PointToClient(Cursor.Position);
			for (int index = 0; index < listBox.Items.Count; index++)
			{
				Rectangle rect = listBox.GetItemRectangle(index);
				if (rect.Contains(pos))
				{
					listBox.SelectedIndex = index;
					break;
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Rectangle rect = ClientRectangle;
			e.Graphics.FillRectangle(StiBrushes.ContentDark, rect.X, rect.Y, rect.Width, rect.Height);
			e.Graphics.DrawRectangle(SystemPens.ControlDark, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
		}

		private void lbWeb_MouseUp(object sender, MouseEventArgs e)
		{
			if (e == null || (e.Button & MouseButtons.Left) > 0)
			{
				if (lbWeb.SelectedItem != null)SelectedColor = ((Color)lbWeb.SelectedItem);
				ClosePopupForm();
			}
		}

		private void lbSystem_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e == null || (e.Button & MouseButtons.Left) > 0)
			{
				if (lbSystem.SelectedItem != null)SelectedColor = ((Color)lbSystem.SelectedItem);
				ClosePopupForm();
			}
		}

		private void TabControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaint(e);
			if (TabControl.SelectedIndex == 1 && lbWeb.SelectedItem != null)lbWeb.Focus();
			else if (TabControl.SelectedIndex == 2 && lbSystem.SelectedItem != null)lbSystem.Focus();
		}

		private void btOther_Click(object sender, System.EventArgs e)
		{
            var clDialog = new ColorDialog();
			clDialog.FullOpen = true;
			if (clDialog.ShowDialog() == DialogResult.OK)    	
			{
				SelectedColor = clDialog.Color;
				if (colorBox is StiColorBox)
				{
					((StiColorBox)colorBox).SelectedColor = clDialog.Color;
				}
				ClosePopupForm();

				foreach (Color color in StiColors.Colors)
					if (color.ToArgb() == SelectedColor.ToArgb())
					{
						SelectedColor = color;
						break;
					}
			}		
		}
		#endregion

		#region Custom panels events
		private void OnMouseEnterPanel(object sender, EventArgs e)
		{			
			DrawPanel(sender, true);
		}
	
		private void OnMouseLeavePanel(object sender, EventArgs e)
		{			
			DrawPanel(sender, false);
		}	
	
		private void OnMouseDownPanel(object sender, MouseEventArgs e)
		{				
			if ((e.Button & MouseButtons.Left) > 0)
			{
				DrawPanel(sender, true);
			}
		}

		private void OnMouseUpPanel(object sender, MouseEventArgs e)
		{			
			if (e == null || (e.Button & MouseButtons.Left) > 0)
			{
				Panel panel = (Panel)sender;
			
				SelectedColor = panel.BackColor;

				foreach (Color color in StiColors.Colors)
					if (color.ToArgb() == SelectedColor.ToArgb())
					{
						this.SelectedColor = color;
						break;
					}
				
				ClosePopupForm();
			}
		}
	
		private void OnPanelPaint(Object sender, PaintEventArgs e)
		{					
			DrawPanel(sender, false);
		} 

		private void DrawPanel(object sender, bool isMouseOver)
		{		
			var panel = sender as Panel;
			using (var g = Graphics.FromHwnd(panel.Handle))
			{
				var rect = panel.ClientRectangle;
		
				if (isMouseOver)
				{
					g.FillRectangle(StiBrushes.Focus, rect);
					g.DrawRectangle(StiPens.SelectedText, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
				}
				else g.FillRectangle(SystemBrushes.Control, rect);
            
				rect.X += 2;
				rect.Y += 2;

				rect.Width -= 5;
				rect.Height -= 5;

			    using (var brush = new SolidBrush(panel.BackColor))
			    {
			        g.FillRectangle(brush, rect);
			    }
				g.DrawRectangle(SystemPens.ControlDark, rect);
			}
		}
	
		#endregion

		public StiColorBoxPopupForm(Color selectedColor, IWindowsFormsEditorService edSrv) : 
			this(null, selectedColor, edSrv)
		{
			TopLevel = false;
			SetLocation(0, 0, 210, 270);
			//ColorBoxPopupForm.TabControl.SelectedIndex = tabIndex;
		}

		public StiColorBoxPopupForm() : this(null, Color.Transparent)
		{
		}

		public StiColorBoxPopupForm(Control colorBox, Color selectedColor) : this(colorBox, selectedColor, null)
		{
		}

		public StiColorBoxPopupForm(Control colorBox, Color selectedColor, IWindowsFormsEditorService edSrv) : 
			base(colorBox)
		{
			try
			{		
				SuspendLayout();

				#region InitializeComponent
				TabControl = new StiTabControl();
				TabControl.SuspendLayout();
				TabControl.Dock = DockStyle.Fill;
				TabControl.Paint += new System.Windows.Forms.PaintEventHandler(TabControl_Paint);
				this.Controls.Add(TabControl);

				tbCustom = new StiTabPage();
				tbCustom.SuspendLayout();
				tbCustom.Text = "Custom";
				tbCustom.DockPadding.All = 4;
				TabControl.Controls.Add(tbCustom);

				tbWeb = new StiTabPage();
				tbWeb.SuspendLayout();
				tbWeb.Text = "Web";
				tbWeb.DockPadding.All = 4;
				TabControl.Controls.Add(tbWeb);

				tbSystem = new StiTabPage();
				tbSystem.SuspendLayout();
				tbSystem.DockPadding.All = 4;
				tbSystem.Text = "System";
				TabControl.Controls.Add(tbSystem);

				lbWeb = new ListBox();
				lbWeb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
				lbWeb.Dock = DockStyle.Fill;
				lbWeb.IntegralHeight = false;
				lbWeb.TabStop = false;
				lbWeb.MouseMove += new System.Windows.Forms.MouseEventHandler(lbWeb_MouseMove);
				lbWeb.DrawItem += new System.Windows.Forms.DrawItemEventHandler(lbWeb_DrawItem);
			
				tbWeb.Controls.Add(lbWeb);

				lbSystem = new ListBox();
				lbSystem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
				lbSystem.Dock = DockStyle.Fill;
				lbSystem.IntegralHeight = false;
				lbSystem.TabStop = false;
				lbSystem.MouseMove += new System.Windows.Forms.MouseEventHandler(lbWeb_MouseMove);
				lbSystem.DrawItem += new System.Windows.Forms.DrawItemEventHandler(lbSystem_DrawItem);
				tbSystem.Controls.Add(lbSystem);

				btOther = new StiButton();
				btOther.Location = new System.Drawing.Point(58, 216);
				btOther.Name = "Other...";
				btOther.Size = new System.Drawing.Size(80, 23);
				btOther.Click += new System.EventHandler(btOther_Click);
				tbCustom.Controls.Add(btOther);

				TabControl.ResumeLayout(false);
				tbCustom.ResumeLayout(false);
				tbWeb.ResumeLayout(false);
				tbSystem.ResumeLayout(false);
				ResumeLayout(false);
				#endregion

				this.edSrv = edSrv;
				this.colorBox = colorBox;
	
				#region InitColors
				this.SelectedColor = selectedColor;
			
				InitColors();

				this.lbSystem.SelectedIndex = -1;
				this.lbWeb.SelectedIndex = -1;

				bool stop = false;
				int index = 0;
			
				string colorName = selectedColor.ToString();
				foreach (Color color in StiColors.SystemColors)
				{
					if (color.ToString() == colorName)
					{
						this.TabControl.SelectedIndex = 2;
						this.lbSystem.SelectedIndex = index;
						stop = true;
						break;
					}
					index++;
				}

				if (!stop)
				{
					index = 0;
					int sc = selectedColor.ToArgb();
					foreach (Color color in StiColors.Colors)
					{
						if (color.ToArgb() == sc)
						{
							this.TabControl.SelectedIndex = 1;
							this.lbWeb.SelectedIndex = index;
							stop = true;
							break;
						}
						index++;
					}
				}

				if (!stop)
				{
					this.TabControl.SelectedIndex = 0;
				}
				#endregion

				if (!this.DesignMode)Localize();
			}
			catch
			{
			}
		}

		
	}
}
