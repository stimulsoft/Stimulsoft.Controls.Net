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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Represents a Windows toolbar.
	/// </summary>
	[ToolboxItem(true)]
    #if !Profile
	[Designer(typeof(StiToolBarDesigner))]
    #endif
	[ToolboxBitmap(typeof(StiToolBar), "Toolbox.StiToolBar.bmp")]
	public class StiToolBar : Panel
    {
        #region enum.StiToolBarLineStyle
        public enum StiToolBarLineStyle
	    {
            All,
	        Bottom,
            TopBottom
	    }
        #endregion

        #region Fields
        /// <summary>
		/// Width of the field to draw points.
		/// </summary>
		protected const int DotWidth = 12;
		protected const int DockPaddingTitle = 12;
		protected const int DockPaddingNormal = 3;	

		protected bool IsDragging = false;
		
		protected bool ChangePos = true;
		#endregion

		#region Properties
		private bool allowMove = true;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual bool AllowMove
		{
			get
			{
				return allowMove;
			}
			set
			{
				allowMove = value;
			}
		}

        private StiToolBarLineStyle lineStyle = StiToolBarLineStyle.Bottom;
        public StiToolBarLineStyle LineStyle
        {
            get
            {
                return lineStyle;
            }
            set
            {
                lineStyle = value;
                this.Invalidate();
            }
        }

		protected virtual bool IsOverTitle
		{
			get
			{
				if (!AllowMove)return false;
				Point point = this.PointToClient(Cursor.Position);
				if (point.X >= 0 && point.X <= 6 && point.Y >= 0 && point.Y <= Height)return true;
				return false;
			}
		}


		private Point lastPos;
		protected Point LastPos
		{
			get
			{
				return lastPos;
			}
			set
			{
				lastPos = value;
			}
		}

	
		private Point movePos;
		protected Point MovePos
		{
			get
			{
				return movePos;
			}
			set
			{
				movePos = value;
			}
		}

        private StiControlStyle controlStyle = StiControlStyle.Flat;
        [DefaultValue(StiControlStyle.Flat)]
        public StiControlStyle ControlStyle
        {
            get
            {
                return controlStyle;
            }
            set
            {
                controlStyle = value;

                if (controlStyle == StiControlStyle.Office2013Blue)
                    this.Padding = new Padding(6,3,3,4);
            }
        }
		#endregion				

		#region Methods
		public void CheckSeparators()
		{
			SuspendLayout();

			try
			{
				#region Remove duplicates separators
                var controls = new List<Control>();
				foreach (Control control in this.Controls)
				{
					if (control is StiToolButton && ((StiToolButton)control).Style == ToolBarButtonStyle.Separator)
					{
						control.Visible = true;
					}
					if (control.Visible)controls.Add(control);
				}

				foreach (var control in controls)
				{
					if (control is StiToolButton && 
						((StiToolButton)control).Style == ToolBarButtonStyle.Separator && 
						control.Visible)
					{
						int index = controls.IndexOf(control) + 1;
						for (int pos = index; pos < controls.Count; pos ++)
						{
							var btn = controls[pos] as StiToolButton;
							var ctl = controls[pos];
							if (btn != null && btn.Style != ToolBarButtonStyle.Separator)
							{
								break;
							}
							else if (btn == null)break;
							else ctl.Visible = false;
						}
					}
				}
            
				for (int index = 0; index < this.Controls.Count; index++)
				{
					var control = this.Controls[index];
					if (control.Visible)
					{
						if (control is StiToolButton && 
							((StiToolButton)control).Style == ToolBarButtonStyle.Separator)
						{
							control.Visible = false;
						}
						break;
					}
				}

				for (int index = this.Controls.Count - 1; index >= 0; index--)
				{
					var control = this.Controls[index];
					if (control.Visible)
					{
						if (control is StiToolButton && 
							((StiToolButton)control).Style == ToolBarButtonStyle.Separator)
						{
							control.Visible = false;
						}
						break;
					}
				}

				if (Controls.Count > 0)
				{
					for (int index = 0; index < Controls.Count; index ++)
					{
						var control = Controls[index];
						if (control is StiToolButton && ((StiToolButton)control).Style == ToolBarButtonStyle.Separator)
						{
							control.Visible = false;
						}
						else break;
					}
					
					for (int index = Controls.Count - 1; index >= 0; index --)
					{
						var control = Controls[index];
						if (control is StiToolButton && ((StiToolButton)control).Style == ToolBarButtonStyle.Separator)
						{
							control.Visible = false;
						}
						else break;
					}
				}
				#endregion
			}
			catch
			{
			}

			ResumeLayout();
		}

		private void DrawDot(Graphics graphics, int x, int y)
		{
			int width = 2;
			graphics.FillRectangle(SystemBrushes.ControlLightLight, x + 1, y + 1, width, width);
			graphics.FillRectangle(SystemBrushes.ControlDarkDark, x, y, width, width);
		}

		private int GetMaxVerticalPos()
		{
			int maxBottom = 0;

		    foreach (Control control in Parent.Controls)
		    {
		        if (control != this)
		        {
		            maxBottom = Math.Max(control.Bottom, maxBottom);
		        }
		    }
		    return maxBottom;
		}

		private Point CheckLocation(Point location)
		{
			location.X = Math.Max(0, location.X);
			location.Y = Math.Max(0, location.Y);

			int pos = GetFreePos(location.X, location.Y);			
			location.X = Math.Max(pos, location.X);			
			location.Y = Math.Min(CorrectY(location.Y), GetMaxVerticalPos());
			if (location.X + Width > Parent.Width)location.X = Parent.Width - Width;
			return location;
		}
		
		private void SetLayoutTitle()
		{
			if (this.RightToLeft == RightToLeft.Yes)
			{
				this.DockPadding.Right = DockPaddingTitle;
				this.DockPadding.Left = DockPaddingNormal;
			}
			else 
			{
				this.DockPadding.Left = DockPaddingTitle;
				this.DockPadding.Right = DockPaddingNormal;				
			}

			this.DockPadding.Top = DockPaddingNormal;			
			this.DockPadding.Bottom = DockPaddingNormal;
		}

		private void SetLayoutNormal()
		{
			this.DockPadding.Left = DockPaddingNormal;
			this.DockPadding.Top = DockPaddingNormal;
			this.DockPadding.Bottom = DockPaddingNormal;
			this.DockPadding.Right = DockPaddingNormal;
		}

		private int CorrectY(int y)
		{
			int pos = (int)(Math.Round(((double)y / Height))) * Height;
			return Math.Max(0, pos);
		}

		private int GetFreePos(int x, int y)
		{
			int pos = x;
			if (Parent != null)
			{
				foreach (Control cl in Parent.Controls)
				{
					if (cl != this)
					{
						StiToolBar tb = cl as StiToolBar;

						if (tb != null && tb.Visible && tb.Top == y && x >= tb.Left && pos < tb.Right)
							pos = tb.Right;
					}
				}
			}
				
			return pos;
		}

		private int GetHeight()
		{
			int height = 0;
			if (Parent != null)
				foreach (Control cl in Parent.Controls)
				{
					StiToolBar tb = cl as StiToolBar;

					if (tb != null && tb.Visible && tb.Bottom > height)height = tb.Bottom;
				}
				
			return height;
		}

		public void DoAutoSize()
		{
			int maxRight = 0;

			foreach (Control control in Controls)
			{
				maxRight = Math.Max(control.Right, maxRight);
			}
			this.Width = maxRight + 4;
		}

        /// <summary>
        /// Check state of all ToolBar.
        /// </summary>
		public void CheckState()
		{
			if (this.Dock == DockStyle.None && Parent != null && 
				(!(Parent is Form)) && (!DesignMode))
				Parent.ClientSize = new Size(Parent.ClientSize.Width, GetHeight());
		}

		public void PlaceOnControl()
		{
			this.Top = 0;
			
			Control parent = this.Parent;
			
			int posY = 0;
				
			while (true)
			{
				int posX = 0;

				foreach (Control control in parent.Controls)
				{
					if (control.Top == posY && control != this)
					{
						posX = Math.Max(control.Right, posX);
					}
				}

				if (parent.Width > (posX + this.Width) || posX == 0)
				{
					this.Left = posX;
					this.Top = posY;
					return;
				}
				posY += this.Height;
			}
		}

		#endregion
		
		#region Handlers
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			SetLayoutTitle();
		}

		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			StiColors.InitColors();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if ((e.Button & MouseButtons.Left) > 0)
			{
				if (IsOverTitle)
				{
					lastPos = Cursor.Position;
					IsDragging = true;
					movePos = Location;
				}
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			IsDragging = false;
			base.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (IsDragging)
			{
				if (Dock == DockStyle.None)
				{
					movePos.Offset(Cursor.Position.X - lastPos.X, Cursor.Position.Y - lastPos.Y);
					if (ChangePos)
						this.Location = CheckLocation(new Point(movePos.X, CorrectY(movePos.Y)));
				
					lastPos = Cursor.Position;

				}
				CheckState();
			}

			if (Dock == DockStyle.None)
			{
				if (IsOverTitle)Cursor.Current = Cursors.SizeAll;
				else Cursor.Current = Cursors.Default;
			}
		}

		protected override void OnPaintBackground(PaintEventArgs p)
		{
		}

		protected override void OnPaint(PaintEventArgs p)
		{
			var g = p.Graphics;
			var rect = new Rectangle(0, 0, Width, Height);

			if (BackgroundImage == null)
			{
                if (ControlStyle == StiControlStyle.Office2013Blue)
                {
                    using (var brush = new SolidBrush(Color.White))
                    {
                        g.FillRectangle(brush, rect);
                    }
                }
                else if (ControlStyle == StiControlStyle.Office2010)
                {
                    using (var brush = new SolidBrush(Color.FromArgb(245, 245, 245)))
                    {
                        g.FillRectangle(brush, rect);
                    }
                }
                else
                {
                    using (var brush = StiBrushes.GetControlBrush(rect, 90))
                    {
                        g.FillRectangle(brush, rect);
                    }
                }
			}
			else
			{
				StiControlPaint.DrawImageBackground(g, BackgroundImage, rect);
			}

			using (var penDark = new Pen(StiColorUtils.Dark(SystemColors.Control, 30)))
			{
                if (ControlStyle == StiControlStyle.Office2013Blue || ControlStyle == StiControlStyle.Office2010)
                {
                    penDark.Color = Color.FromArgb(198, 198, 198);
                    penDark.DashPattern = new float[] { 1f, 2f };

                    if (lineStyle == StiToolBarLineStyle.Bottom)
                    {
                        g.DrawLine(penDark, rect.X, rect.Bottom - 1, rect.Right - 1, rect.Bottom - 1);
                    }
                    else if (lineStyle == StiToolBarLineStyle.All)
                    {
                        g.DrawRectangle(penDark, rect.X, rect.Y, rect.Right - 1, rect.Bottom - 1);
                    }
                    else
                    {
                        g.DrawLine(penDark, rect.X, rect.Top, rect.Right - 1, rect.Top);
                        g.DrawLine(penDark, rect.X, rect.Bottom - 1, rect.Right - 1, rect.Bottom - 1);
                    }
                }
                else
                {
                    g.DrawLine(penDark, rect.X, rect.Bottom - 1, rect.Right - 1, rect.Bottom - 1);
                    g.DrawLine(penDark, rect.Right - 1, rect.Bottom - 1, rect.Right - 1, rect.Y);
                }
			}

            if (ControlStyle == StiControlStyle.Flat)
            {
                int y = (Height - 16) / 2;
                int x = 4;
                if (this.RightToLeft == RightToLeft.Yes) x = this.Width - 6;
                DrawDot(g, x, y);
                DrawDot(g, x, y + 4);
                DrawDot(g, x, y + 8);
                DrawDot(g, x, y + 12);
            }
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			CheckState();
		}
		
		#endregion

		#region Constructors
		public StiToolBar()
		{
			this.SetStyle(ControlStyles.ResizeRedraw, true);

			SetLayoutTitle();

			this.Height = 28;			
		}
		#endregion
	}
}