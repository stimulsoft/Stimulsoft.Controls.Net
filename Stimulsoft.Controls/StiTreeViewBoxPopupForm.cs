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
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	internal class StiTreeViewBoxPopupForm : StiPopupForm
	{
		#region Fields
		internal int SelectedIndex = -1;

		internal StiTreeView tvNodes;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tvNodes = new StiTreeView();
			this.SuspendLayout();
			// 
			// tvNodes
			// 
			this.tvNodes.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tvNodes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvNodes.HideSelection = false;
			this.tvNodes.ImageIndex = -1;
			this.tvNodes.Location = new System.Drawing.Point(2, 2);
			this.tvNodes.Name = "tvNodes";
			this.tvNodes.SelectedImageIndex = -1;
			this.tvNodes.Size = new System.Drawing.Size(296, 296);
			this.tvNodes.TabIndex = 0;
			this.tvNodes.DoubleClick += new System.EventHandler(this.tvNodes_DoubleClick);
			// 
			// StiTreeViewBoxPopupForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(300, 300);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.tvNodes});
			this.Name = "StiTreeViewBoxPopupForm";
			this.Text = "StiComboBoxPopupForm";
			this.ResumeLayout(false);

		}
		#endregion

		#region Handlers
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Rectangle rect = ClientRectangle;
			e.Graphics.FillRectangle(StiBrushes.Window, rect.X, rect.Y, rect.Width, rect.Height);
			e.Graphics.DrawRectangle(SystemPens.ControlDark, rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
		}

		private void tvNodes_DoubleClick(object sender, System.EventArgs e)
		{
			TreeNode treeNode = tvNodes.SelectedNode;

			if (treeNode != null)
			{
				if (treeNode.Tag != null || (!((StiTreeViewBox)ParentControl).SelectOnlyTagNotNull))
				{
					((StiTreeViewBox)ParentControl).SelectedItem = treeNode.Tag;
						
					StiValueChangedEventArgs ea = new 
						StiValueChangedEventArgs(tvNodes, treeNode.Text);
					
					((StiTreeViewBox)ParentControl).InvokeValueChanged(ea);


					((StiTreeViewBox)ParentControl).Text = ea.Text;
				}
			}
			else
			{
				if (!((StiTreeViewBox)ParentControl).SelectOnlyTagNotNull)
				{
					StiValueChangedEventArgs ea = new 
						StiValueChangedEventArgs(tvNodes, string.Empty);
					((StiTreeViewBox)ParentControl).InvokeValueChanged(ea);

					((StiTreeViewBox)ParentControl).SelectedItem = null;
					((StiTreeViewBox)ParentControl).Text = ea.Text;
				}

			}

			ClosePopupForm();
		}

		#endregion

		#region Methods
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				tvNodes.ImageList = null;
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		public override void SetLocation(int x, int y, int width, int height)
		{
			base.SetLocation(x, y, width, height);

			this.DockPadding.Left += 2;
			this.DockPadding.Top += 2;
			this.DockPadding.Right += 2;
			this.DockPadding.Bottom += 2;
		}
		#endregion

		public StiTreeViewBoxPopupForm() : this(null)
		{
		}
		
		public StiTreeViewBoxPopupForm(Control parentControl) : base(parentControl)
		{
			InitializeComponent();

		}

	}
}