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

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Allows users to increment and decrement numeric values. 
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiNumericUpDown), "Toolbox.StiNumericUpDown.bmp")]
	public class StiNumericUpDown : StiNumericUpDownBase
	{
		#region Properties (Browsable(false))
		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override string Text
		{
			get
			{
				return this.TextBox.Text;
			}
			set
			{
				this.TextBox.Text = value;
			}
		}

		#endregion
		
		#region Properties
		private int minimum = 0; 
		/// <summary>
		/// Gets or sets the minimum allowed value for the up-down control.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(0)]
		public int Minimum
		{
			get
			{
				return minimum;
			}
			set
			{
				minimum = value;
			}
		}


		/// <summary>
		/// Gets or sets the value assigned to the up-down control.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(0)]
		public int Value
		{
			get
			{
				try
				{
					int value = int.Parse(this.TextBox.Text);
					if (value > Maximum)return Maximum;
					if (value < Minimum)return Minimum;
					return value;
				}
				catch
				{
				}
				return Minimum;
			}
			set
			{
				if (this.TextBox.Text != value.ToString())
				{
					this.TextBox.Text = value.ToString();
					InvokeValueChanged(EventArgs.Empty);
				}
				
			}
		}


		private int maximum = 100; 
		/// <summary>
		/// Gets or sets the maximum value for the up-down control.
		/// </summary>
		[Category("Behavior")]
		[DefaultValue(100)]
		public int Maximum
		{
			get
			{
				return maximum;
			}
			set
			{
				maximum = value;
			}
		}

		
		private int increment = 1; 
		/// <summary>
		/// Gets or sets the value to increment or decrement the up-down control when the up or down buttons are clicked.
		/// </summary>
		[Category("Behavior")]
		public int Increment
		{
			get
			{
				return increment;
			}
			set
			{
				increment = value;
			}
		}





		#endregion
		
		#region Methods
		protected override void DoUp()
		{
			int value = this.Value;
			value += increment;
			if (value > this.Maximum)value = Maximum;
			this.Value = value;
		}

		protected override void DoDown()
		{
			int value = this.Value;
			value -= increment;
			if (value < this.Minimum)value = Minimum;
			this.Value = value;
		}

		#endregion

		#region Events
		#region ValueChanged
		/// <summary>
		/// Occurs when the Value property has been changed in some way.
		/// </summary>
		[Category("Behavior")]
		public event EventHandler ValueChanged;

		protected virtual void OnValueChanged(System.EventArgs e)
		{
			if (ValueChanged != null)ValueChanged(this, e);
		}


		/// <summary>
		/// Raises the ValueChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data. </param>
		public void InvokeValueChanged(EventArgs e)
		{
			OnValueChanged(e);
		}

		#endregion
		#endregion

		#region Handlers
		private void OnTextChanged(object sender, EventArgs e)
		{
			InvokeValueChanged(e);
		}

		#endregion

		#region Constructors
		public StiNumericUpDown()
		{
            this.Value = 1;
			this.TextBox.TextChanged += new EventHandler(OnTextChanged);
		}
		#endregion
	}
}
