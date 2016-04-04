#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports												}
{	                         										}
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
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Stimulsoft.Controls
{
	[ToolboxItem(false)]
	public class StiProgressBar : Control
	{
		#region Const
		private const int BarLength = 50;
		private const int BarStep = 10;
		#endregion

		#region Methods
		public void StartMarquee()
		{
			timer = new Timer();
			timer.Interval = 50;
			timer.Tick += new EventHandler(OnTimerTick);
			timer.Enabled = true;
		}

		public void StopMarquee()
		{
			if (timer != null)
			{
				timer.Enabled = false;
			}
		}

		internal void UpdateMarguee()
		{
			Value += BarStep;
			if (Value > this.Width)Value = 0;
			
			using (Graphics g = this.CreateGraphics())
			{
				Draw(g);
			}
		}

		private void DrawBar(Graphics g, Rectangle rect)
		{
			int pos = rect.X;
			while (pos < rect.Right)
			{
				Rectangle rectBar = new Rectangle(pos, rect.Y, BarStep - 2, rect.Height);
				g.FillRectangle(SystemBrushes.Highlight, rectBar);
				pos += BarStep;
			}
		}


		private void Draw()
		{
			using (Graphics g = this.CreateGraphics())
			{
				Draw(g);
			}
		}

		private void Draw(Graphics gg)
		{
			var rect = this.ClientRectangle;
			using (var bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb))
			{
				using(var g = Graphics.FromImage(bmp))
				{						
					using (var brush = new SolidBrush(this.BackColor))
					{
						g.FillRectangle(brush, rect);
					}

					ControlPaint.DrawBorder3D(g, rect, Border3DStyle.SunkenOuter);

					rect.Inflate(-2, -2);
					g.SetClip(rect);

					int value = this.Value;

					float step = (float)rect.Width / (float)Maximum;
					int pos = (int)(value * step);

					if (timer != null && timer.Enabled)
					{
						var rectProgress = new Rectangle(Value, rect.Y, BarLength, rect.Height);
						DrawBar(g, rectProgress);
						int dist = rect.Right - rectProgress.Right;
						if (dist < 0)
						{
							rectProgress.X = - (rectProgress.Width + dist);
							DrawBar(g, rectProgress);
						}
					}
					else
					{
						DrawBar(g, new Rectangle(rect.X, rect.Y, pos, rect.Height));
					}
				}
				gg.DrawImageUnscaled(bmp, this.ClientRectangle);
			}
		}

		#endregion

		#region Handlers
		protected override void OnPaint(PaintEventArgs e)
		{
				
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{		
			Draw(e.Graphics);
		}


		private void OnTimerTick(object sender, EventArgs e)
		{
			UpdateMarguee();
		}

		#endregion

		#region Fields
		private Timer timer = null;
		#endregion

		#region Properties
		private int maximum = 100;
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


		private int minimum = 0;
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


		private int _value = 0;
		public int Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				Invalidate();
			}
		}

		#endregion
			
		public StiProgressBar()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
		}
	}

}
