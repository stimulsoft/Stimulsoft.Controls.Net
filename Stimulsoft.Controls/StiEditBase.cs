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
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Defines the base edit control.
	/// </summary>
	[ToolboxItem(false)]
	public class StiEditBase : StiControlBase
	{
		[ToolboxItem(false)]
		public class InternalTextBox : TextBox
		{
			protected override bool ProcessDialogKey(Keys keyData)
			{
				StiEditBase editor = Parent as StiEditBase;

				if ((keyData & Keys.KeyCode) == Keys.Tab && (Control.ModifierKeys & Keys.Shift) > 0)
				{
					Form form = editor.FindForm();
					if (form != null)
					{
						form.SelectNextControl(editor, false, true, true, true);
						return true;
					}
				}
				return base.ProcessDialogKey(keyData);
			}

			public override ISite Site
			{
				get
				{
					if ((!DesignMode) && ParentControl != null)
					{
						this.Name = ParentControl.Name;
						this.Tag = ParentControl.Tag;
					}
					return base.Site;
				}
			}


			private StiEditBase parentControl = null;
			public StiEditBase ParentControl
			{
				get
				{
					return parentControl;
				}
			}


			public InternalTextBox(StiEditBase ParentControl)
			{
				this.parentControl = ParentControl;
			}
		}

		#region Fields
		protected TextBox TextBox;
		#endregion

		#region Handlers
		protected override void OnBackColorChanged(System.EventArgs e)
		{
			base.OnBackColorChanged(e);
			this.TextBox.BackColor = this.BackColor;
			
		}

		protected override void OnTextChanged(System.EventArgs e)
		{
			base.OnTextChanged(e);
			this.TextBox.Text = this.Text;
			
		}

		protected override void OnEnabledChanged(System.EventArgs e)
		{
			base.OnEnabledChanged(e);
			TextBox.Visible = this.Enabled;
			Invalidate();
		}

		protected override void OnGotFocus(System.EventArgs e)
		{
			this.TextBox.Focus();
			base.OnGotFocus(e);
		}

		protected override void OnSizeChanged(System.EventArgs e)
		{
			SetPosTextBox();
			base.OnSizeChanged(e);
		}

		protected override void OnPaint(PaintEventArgs p)
		{
			Graphics g = p.Graphics;

			Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

			if (this.ReadOnly)IsMouseOver = false;

			if ((!this.Enabled) || this.ReadOnly)
			{
				g.FillRectangle(SystemBrushes.Window, rect);
			}

            if (VisualStyleInformation.IsEnabledByUser && VisualStyleInformation.IsSupportedByOS)
            {
                try
                {
                    VisualStyleElement element = null;

                    if (!this.Enabled) element = VisualStyleElement.TextBox.TextEdit.Disabled;
                    else if (this.Focused) element = VisualStyleElement.TextBox.TextEdit.Focused;
                    else if (IsMouseOver) element = VisualStyleElement.TextBox.TextEdit.Hot;
                    else element = VisualStyleElement.TextBox.TextEdit.Normal;

                    if (VisualStyleRenderer.IsElementDefined(element))
                    {
                        VisualStyleRenderer renderer = new VisualStyleRenderer(element);
                        renderer.DrawBackground(g, rect);
                        return;
                    }
                }
                catch
                {
                }
            }
			if (this.AllowDrawBorder)
				StiControlPaint.DrawBorder(g, rect, (IsMouseOver | Focused) & HotFocus, Flat);

		}
		#endregion

		#region Events
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
		}

		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
		}

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
		}
		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick(e);
		}

		
		private void InvokeMouseUp(object sender, MouseEventArgs e)
		{
			OnMouseUp(e);
		}

		private void InvokeMouseDown(object sender, MouseEventArgs e)
		{
			OnMouseDown(e);
		}

		private void InvokeMouseEnter(object sender, EventArgs e)
		{
			OnMouseEnter(e);
		}

		private void InvokeMouseLeave(object sender, EventArgs e)
		{
			OnMouseLeave(e);
		}

		private void InvokeMouseHover(object sender, EventArgs e)
		{
			OnMouseHover(e);
		}

		private void InvokeMouseMove(object sender, MouseEventArgs e)
		{
			OnMouseMove(e);
		}

		private void InvokeKeyDown(object sender, KeyEventArgs e)
		{
			OnKeyDown(e);
		}

		private void InvokeKeyUp(object sender, KeyEventArgs e)
		{
			OnKeyUp(e);
		}

		private void InvokeKeyPress(object sender, KeyPressEventArgs e)
		{
			OnKeyPress(e);
		}

		private void InvokeClick(object sender, EventArgs e)
		{
			OnClick(e);
		}

		private void InvokeDoubleClick(object sender, EventArgs e)
		{
			OnDoubleClick(e);
		}
		#endregion

		#region TextBox event
		private void TextBox_TextChanged(object sender, System.EventArgs e)
		{
			this.Text = this.TextBox.Text;
			
		}

		private void TextBox_GotFocus(object sender, System.EventArgs e)
		{
			base.OnGotFocus(e);
			TextBox.Name = this.Name;
			TextBox.Tag = this.Tag;
			Invalidate();
		}
		
		private void TextBox_LostFocus(object sender, System.EventArgs e)
		{
			base.OnLostFocus(e);
			Invalidate();
		}
		private void TextBox_SizeChanged(object sender, System.EventArgs e)
		{
			SetPosTextBox();
			Invalidate();
		}

		private void TextBox_MouseEnter(object sender, System.EventArgs e)
		{
			IsMouseOver = true;
			this.Invalidate();
		}
		private void TextBox_MouseLeave(object sender, System.EventArgs e)
		{
			IsMouseOver = this.ClientRectangle.Contains(this.PointToClient(Cursor.Position));
			this.Invalidate();
		}
		#endregion

		#region Browsable(false)
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
			}
		}
		#endregion

		#region Properties
		private bool allowDrawBorder = true;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool AllowDrawBorder
		{
			get
			{
				return allowDrawBorder;
			}
			set
			{
				allowDrawBorder = value;
			}
		}
        
		/// <summary>
		/// Gets or sets the starting point of the text selected in the text box.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int SelectionStart 
		{
			get
			{
				return TextBox.SelectionStart;
			}
			set
			{
				TextBox.SelectionStart = value;
			}
		}

		
		/// <summary>
		/// Gets or sets the number of characters selected in the text box.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual int SelectionLength
		{
			get
			{
				return TextBox.SelectionLength;
			}
			set
			{
				TextBox.SelectionLength = value;
			}
		}

		
		/// <summary>
		/// Gets or sets a value indicating the currently selected text in the control.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual string SelectionText
		{
			get
			{
				return TextBox.SelectedText;
			}
			set
			{
				TextBox.SelectedText = value;
			}
		}

		
		/// <summary>
		/// Gets or sets a value indicating whether the selected text in the text box control remains highlighted when the control loses focus.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool HideSelection
		{
			get
			{
				return TextBox.HideSelection;
			}
			set
			{
				TextBox.HideSelection = value;
			}
		}


		[DefaultValue(32767)]
		[Category("Behavior")]
		public virtual int MaxLength
		{
			get
			{
				return TextBox.MaxLength;
			}
			set
			{
				TextBox.MaxLength = value;
			}
		}

		
		/// <summary>
		/// Gets a value indicating whether the control has input focus.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override bool Focused
		{
			get
			{
				return base.Focused || TextBox.Focused;
			}
		}

		
		/// <summary>
		/// Gets or sets a value indicating whether a text in the text box is read-only.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(false)]
		public virtual bool ReadOnly
		{
			get
			{
				return TextBox.ReadOnly;
			}
			set
			{
				TextBox.ReadOnly = value;
				if (value)TextBox.BackColor = SystemColors.Control;
				else TextBox.BackColor = this.BackColor;
				Invalidate();
			}
		}

		#endregion

		#region Methods
		protected virtual Rectangle GetContentRect()
		{
			return new Rectangle(2, 2, Width - 4, Height - 4);
		}

		protected virtual void SetPosTextBox()
		{
			try
			{
				Rectangle contentRect = GetContentRect();
			
				TextBox.Left = contentRect.Left + 1;
				TextBox.Width = contentRect.Width - 2;

				if (TextBox.AutoSize)
				{
					TextBox.Top = (Height - TextBox.Height) / 2;
					TextBox.Height = contentRect.Height;
				}
				else TextBox.Top = contentRect.Top + (contentRect.Height - TextBox.Height) / 2;
			}
			catch
			{
			}
		}

		#endregion

		#region Constructors
		public StiEditBase()
		{
			TextBox = new InternalTextBox(this);

			this.Width = 100;
			this.Height = 20;

			this.BackColor = SystemColors.Window;

			#region EditBox Init
			TextBox.BorderStyle = BorderStyle.None;
			TextBox.MouseEnter += new System.EventHandler(TextBox_MouseEnter);
			TextBox.MouseLeave += new System.EventHandler(TextBox_MouseLeave);
			TextBox.GotFocus += new System.EventHandler(TextBox_GotFocus);
			TextBox.LostFocus += new System.EventHandler(TextBox_LostFocus);
			TextBox.SizeChanged += new System.EventHandler(TextBox_SizeChanged);
			TextBox.TextChanged += new System.EventHandler(TextBox_TextChanged);

			TextBox.MouseUp += new MouseEventHandler(this.InvokeMouseUp);
			TextBox.MouseDown += new MouseEventHandler(this.InvokeMouseDown);
			TextBox.MouseEnter += new EventHandler(this.InvokeMouseEnter);
			TextBox.MouseHover += new EventHandler(this.InvokeMouseHover);
			TextBox.MouseLeave += new EventHandler(this.InvokeMouseLeave);
			TextBox.MouseMove += new MouseEventHandler(this.InvokeMouseMove);
			TextBox.KeyDown += new KeyEventHandler(this.InvokeKeyDown);
			TextBox.KeyPress += new KeyPressEventHandler(this.InvokeKeyPress);
			TextBox.KeyUp += new KeyEventHandler(this.InvokeKeyUp);
			TextBox.Click += new EventHandler(this.InvokeClick);
			TextBox.DoubleClick += new EventHandler(this.InvokeDoubleClick);

			this.Controls.Add(TextBox);
			SetPosTextBox();
			#endregion
		}
		#endregion
	}
}
