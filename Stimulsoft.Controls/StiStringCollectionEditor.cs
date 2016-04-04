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
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls
{
    #if !Profile
	public class StiStringCollectionEditor : CollectionEditor
	{
		public StiStringCollectionEditor(Type type) : base(type)
		{
		}

		protected override CollectionForm CreateCollectionForm()
		{
			return new StringCollectionForm(this);
		}

		protected override object[] GetItems(object editValue)
		{
			if ((editValue == null) || !(editValue is ICollection))
			{
				return new object[0];
			}
			var list1 = new ArrayList();
			ICollection collection1 = (ICollection) editValue;
			foreach (object obj1 in collection1)
			{
				list1.Add(obj1);
			}
			object[] objArray1 = new object[list1.Count];
			list1.CopyTo(objArray1, 0);
			return objArray1;
		}

 


		
		#region Constructors
		private class StringCollectionForm : CollectionForm
		{
			public StringCollectionForm(CollectionEditor editor) : base(editor)
			{
				this.instruction = new Label();
				this.textEntry = new StiTextBox();
				this.btOk = new StiButton();
				this.btCancel = new StiButton();
				this.editor = null;
				this.editor = ((StiStringCollectionEditor) editor);
				this.InitializeComponent();
				this.textEntry.Focus();
			}


			private void InitializeComponent()
			{
				this.instruction.Location = new Point(4, 7);
				this.instruction.Size = new Size(422, 14);
				this.instruction.TabIndex = 0;
				this.instruction.TabStop = false;
				this.instruction.Text = "Enter the strings in the collection (one per line):";

				this.textEntry.Location = new Point(4, 22);
				this.textEntry.Size = new Size(422, 244);
				this.textEntry.TabIndex = 0;
				this.textEntry.Text = "";
				this.textEntry.AcceptsTab = true;
				this.textEntry.AcceptsReturn = true;
				this.textEntry.AutoSize = false;
				this.textEntry.Multiline = true;
				this.textEntry.WordWrap = true;

				this.btOk.Location = new Point(260, 274);
				this.btOk.Size = new Size(75, 23);
				this.btOk.TabIndex = 1;
				this.btOk.Text = "Ok";
				this.btOk.DialogResult = DialogResult.OK;
				this.btOk.Click += new EventHandler(this.btOk_click);
				
				this.btCancel.Location = new Point(339, 274);
				this.btCancel.Size = new Size(75, 23);
				this.btCancel.TabIndex = 2;
				this.btCancel.Text = "Cancel";
				this.btCancel.DialogResult = DialogResult.Cancel;

				base.Location = new Point(7, 7);
				base.AcceptButton = this.btOk;
				base.AutoScaleBaseSize = new Size(5, 13);
				base.CancelButton = this.btCancel;
				base.ClientSize = new Size(429, 307);
				base.MaximizeBox = false;
				base.MinimizeBox = false;
				base.ShowInTaskbar = false;
				base.FormBorderStyle = FormBorderStyle.FixedDialog;
				base.StartPosition = FormStartPosition.CenterScreen;
				base.MinimumSize = new Size(300, 200);
				base.Text = "String Collection Editor";
				base.Controls.Clear();
				
				base.Controls.Add(textEntry);
				base.Controls.Add(btOk);
				base.Controls.Add(btCancel);
				base.Controls.Add(instruction);
 			}


			private void btOk_click(object sender, EventArgs e)
			{
				if (this.textEntry.Text.Length != 0)
				{
					string[] strs = this.textEntry.Text.Split(new char[]{(Char)13});

					object[] items = (object[])Array.CreateInstance(typeof(object), strs.Length);

					int index = 0;
					foreach (string str in strs)
					{
						items[index++] = str.Trim(new char[]{(Char)10});
					}
					int length = strs.Length;
					for (int ind = strs.Length - 1; ind >= 0; ind --)
					{
						if (strs[ind].Trim().Length == 0)length --;
						else break;
					}
					if (length == strs.Length)base.Items = items;
					else Array.Copy(items, 0, base.Items, 0, length);
				}
				else base.Items = new object[0];
			}

			protected override void OnEditValueChanged()
			{
				string text = string.Empty;

				foreach (object obj in base.Items)
				{
					string str = obj.ToString();
					text += str + (Char)13 + (Char)10;
				}
				this.textEntry.Text = text;
 
			}

			
			private StiStringCollectionEditor editor;
			private Label instruction;
			private StiButton btOk;
			private StiButton btCancel;
			private StiTextBox textEntry;
		}
		#endregion 
	}
    #endif
}