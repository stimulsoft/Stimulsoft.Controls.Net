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

using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
    public class StiTabulatorStyleOffice2013Blue : IStiTabulatorStyle
    {
        #region Methods
		private void DrawPageTitleLeftHorizontal(Graphics g, Pen penLight, Pen penDark, StiTabulatorPage page, 
			Rectangle rect, Rectangle contentRect)
		{
			if (page != tabulator.SelectedTab)
			{
				int size = 4;
				rect.X += size;
				rect.Width -= size;
				contentRect.X += size;
				contentRect.Width -= size;
			}

			g.DrawLine(penDark, rect.X + 1, rect.Y, rect.Right, rect.Y);
			g.DrawLine(penDark, rect.X, rect.Y + 1, rect.X, rect.Bottom - 1);
			g.DrawLine(penDark, rect.X + 1, rect.Bottom, rect.Right, rect.Bottom);

			if (page != tabulator.SelectedTab)
			{
				if (page.IsMouseOver)
				{
					using (var br = new SolidBrush(StiColorUtils.Dark(StiColors.Content, 0)))
					{
						g.FillRectangle(br, contentRect);
					}
				}
				else
				{
					using (var br = new SolidBrush(StiColorUtils.Dark(StiColors.Content, 10)))
					{
						g.FillRectangle(br, contentRect);
					}
				}
			}
			else
			{
				using (var br = new SolidBrush(tabulator.BackColor))
				{
					g.FillRectangle(br, contentRect);
				}	
		
				g.DrawLine(penLight, rect.X + 2, rect.Y + 1, rect.Right, rect.Y + 1);
				g.DrawLine(penLight, rect.X + 1, rect.Y + 1, rect.X + 1, rect.Bottom - 1);
			}	
		
			tabulator.Mode.DrawTitleText(g, rect, page);
			tabulator.Mode.DrawTitleImage(g, rect, page);
			
		}

		
		private void DrawPageTitleTopHorizontal(Graphics g, Pen penLight, Pen penDark, StiTabulatorPage page, 
			Rectangle rect, Rectangle contentRect)
		{
			Point [] pts = new Point[]{
										  new Point(rect.X, rect.Y),
										  new Point(rect.X, rect.Bottom),
										  new Point(rect.Right + 5, rect.Bottom + 1),
										  new Point(rect.Right - 5, rect.Y)};

			using (var path = new GraphicsPath(FillMode.Alternate))
			{
				path.AddPolygon(pts);				

				#region Draw page header
				if (tabulator.SelectedTab == page)
				{
					using (var br = new SolidBrush(tabulator.BackColor))
					{
						g.FillPath(br, path);
					}
				}
				else 
				{
					if (page.IsMouseOver)
					{
						using (var br = new SolidBrush(StiColorUtils.Dark(StiColors.Content, 0)))
						{
							g.FillPath(br, path);
						}
					}
					else
					{
						using (var br = new SolidBrush(StiColorUtils.Dark(StiColors.Content, 10)))
						{
							g.FillPath(br, path);
						}
					}
				}
				#endregion

				#region Draw header lines
				if (tabulator.SelectedTab == page)
				{
					g.DrawLine(penDark, pts[0], pts[1]);
					g.DrawLine(penLight, pts[0].X + 1, pts[0].Y + 1, pts[1].X + 1, pts[1].Y + 1);
				
					g.DrawLine(penDark, pts[2], pts[3]);

					g.DrawLine(penDark, pts[0], pts[3]);
					g.DrawLine(penLight, pts[0].X + 1, pts[0].Y + 1, pts[3].X - 1, pts[3].Y + 1);
				}
				else
				{
					g.DrawLine(penDark, pts[0], pts[1]);
					g.DrawLine(penDark, pts[0], pts[3]);
					g.DrawLine(penDark, pts[2], pts[3]);
				}
				#endregion
			}
			tabulator.Mode.DrawTitleText(g, rect, page);
			tabulator.Mode.DrawTitleImage(g, rect, page);
				
		}

        private void DrawPageTitleRightHorizontal(Graphics g, Pen penLight, Pen penDark, StiTabulatorPage page,
            Rectangle rect, Rectangle contentRect)
        {
            if (page != tabulator.SelectedTab)
            {
                if (page.IsMouseOver)
                {
                    using (var br = new SolidBrush(StiStyleOffice2013Blue.ColorButtonButtonMouseOver))
                    {
                        g.FillRectangle(br, contentRect);
                    }
                }
                else
                {
                    using (var br = new SolidBrush(Color.White))
                    {
                        g.FillRectangle(br, contentRect);
                    }
                }
            }
            else
            {
                using (var br = new SolidBrush(StiStyleOffice2013Blue.ColorButtonChecked))
                {
                    g.FillRectangle(br, contentRect);
                }
            }

            tabulator.Mode.DrawTitleText(g, rect, page);
            tabulator.Mode.DrawTitleImage(g, rect, page);

        }
		
		public void DrawPageTitle(Graphics g, StiTabTitlePosition position, StiTabulatorPage page)
		{
			var rect = tabulator.GetTitlePageRectangle(g, page);
            Rectangle contentRect;

			if (rect.Width != 0 && rect.Height != 0)
			{			
				using (Pen penLight = new Pen(StiColorUtils.Light(SystemColors.Control, 30)), 
						   penDark = new Pen(StiColorUtils.Dark(SystemColors.Control, 50)))
				{					
					switch (position)
					{
						case StiTabTitlePosition.LeftHorizontal:
                            contentRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 1, rect.Height - 1);
							DrawPageTitleLeftHorizontal(g, penLight, penDark, page, rect, contentRect);
							break;

						case StiTabTitlePosition.TopHorizontal:
                            contentRect = new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 1, rect.Height - 1);
							DrawPageTitleTopHorizontal(g, penLight, penDark, page, rect, contentRect);
							break;

                        case StiTabTitlePosition.RightHorizontal:
                            contentRect = new Rectangle(rect.X, rect.Y + 3, rect.Width, rect.Height);
                            DrawPageTitleRightHorizontal(g, penLight, penDark, page, rect, contentRect);
                            break;
					}
				}				
			}
		}
        
		private void DrawPageLeftHorizontal(Graphics g, StiTabulatorPage page, Pen penLight, Pen penDark, 
			Rectangle rect, Rectangle pageRect)
		{
			g.DrawLine(penDark, rect.X + 1, rect.Y, rect.Right - 1, rect.Y);
			g.DrawLine(penLight, rect.X + 1, rect.Y + 1, rect.Right - 2, rect.Y + 1);
					
			g.DrawLine(penDark, rect.Right, rect.Top + 1, rect.Right, rect.Bottom - 1);
			g.DrawLine(penDark, rect.X + 1, rect.Bottom, rect.Right - 1, rect.Bottom);

			if (((StiTabulator)page.Parent).ShowTitle)
			{
				g.DrawLine(penLight, rect.X + 1, rect.Y + 1, rect.X + 1, pageRect.Y - 1);
				g.DrawLine(penDark, rect.X, rect.Y + 1, rect.X, pageRect.Y);

				g.DrawLine(penLight, rect.X + 1, pageRect.Y + pageRect.Height, rect.X + 1, rect.Bottom - 2);
				g.DrawLine(penDark, rect.X, pageRect.Y + pageRect.Height, rect.X, rect.Bottom - 1);
			}
			else
			{
				g.DrawLine(penLight, rect.X + 1, rect.Y + 1, rect.X + 1, rect.Bottom - 2);
				g.DrawLine(penDark, rect.X, rect.Y + 1, rect.X, rect.Bottom - 1);
			}
		}
		
		private void DrawPageTopHorizontal(Graphics g, StiTabulatorPage page, Pen penLight, Pen penDark, 
			Rectangle rect, Rectangle pageRect)
		{
			g.DrawLine(penDark, rect.X, rect.Y + 1, rect.X, rect.Bottom - 1);
			g.DrawLine(penLight, rect.X + 1, rect.Y + 1, rect.X + 1, rect.Bottom - 2);
					
			g.DrawLine(penDark, rect.X + 1, rect.Bottom, rect.Right - 1, rect.Bottom);
			g.DrawLine(penDark, rect.Right, rect.Y + 1, rect.Right, rect.Bottom - 1);

			if (((StiTabulator)page.Parent).ShowTitle)
			{
				g.DrawLine(penLight, rect.X + 1, rect.Y + 1, pageRect.X - 1, rect.Y + 1);
				g.DrawLine(penDark, rect.X + 1, rect.Y, pageRect.X, rect.Y);

				g.DrawLine(penLight, pageRect.X + pageRect.Width + 5, rect.Y + 1, rect.Right - 2, rect.Y + 1);
				g.DrawLine(penDark, pageRect.X + pageRect.Width + 5, rect.Y, rect.Right - 1, rect.Y);
			}
			else
			{
				g.DrawLine(penLight, rect.X + 1, rect.Y + 1, rect.Right - 2, rect.Y + 1);
				g.DrawLine(penDark, rect.X + 1, rect.Y, rect.Right - 1, rect.Y);
			}
		}

        private void DrawPageRightHorizontal(Graphics g, StiTabulatorPage page, Pen penLight, Pen penDark,
            Rectangle rect, Rectangle pageRect)
        {
            if (((StiTabulator)page.Parent).ShowTitle)
            {
                penDark.Color = Color.FromArgb(198, 198, 198);
                penDark.DashPattern = new float[] { 1f, 2f};
                g.DrawLine(penDark, rect.Right, rect.Top, rect.Right, rect.Bottom);
            }
        }

		
		public void DrawPage(Graphics g, StiTabTitlePosition position, StiTabulatorPage page)
		{
            var rect = new Rectangle(0, 0, page.Width - 1, page.Height - 1);
            var pageRect = ((StiTabulator)page.Parent).GetTitlePageRectangle(g, page);

            using (Pen penLight = new Pen(StiColorUtils.Light(SystemColors.Control, 30)),
                       penDark = new Pen(StiColorUtils.Dark(SystemColors.Control, 50)))
            {
                DrawDot(g, 0, 0);
                DrawDot(g, rect.Right, 0);
                DrawDot(g, rect.Right, rect.Bottom);
                DrawDot(g, 0, rect.Bottom);


                switch (position)
                {
                    case StiTabTitlePosition.LeftHorizontal:
                        DrawPageLeftHorizontal(g, page, penLight, penDark, rect, pageRect);
                        break;

                    case StiTabTitlePosition.TopHorizontal:
                        DrawPageTopHorizontal(g, page, penLight, penDark, rect, pageRect);
                        break;

                    case StiTabTitlePosition.RightHorizontal:
                        DrawPageRightHorizontal(g, page, penLight, penDark, rect, pageRect);
                        break;
                }
            }
		}


		internal void DrawDot(Graphics g, int x, int y)
		{
			g.FillRectangle(SystemBrushes.Control, x, y, 1, 1);
		}

		#endregion

		#region Fields
		protected StiTabulator tabulator = null;
		#endregion

        public StiTabulatorStyleOffice2013Blue(StiTabulator tabulator)
		{
			this.tabulator = tabulator;
		}
    }
}
