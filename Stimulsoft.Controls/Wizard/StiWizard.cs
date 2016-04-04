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
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace Stimulsoft.Controls
{

    [ToolboxItem(true)]
    //[ToolboxBitmap(typeof(StiWizard), "Toolbox.StiWizard.bmp")]
#if !Profile
	[Designer(typeof(StiWizardDesigner))]
#endif
    public class StiWizard : ContainerControl
    {
        #region Fields
        internal StringFormat sfHeaderTitle;
        internal StringFormat sfHeaderDescription;
        private int step = 14;
        #endregion

        #region Control Added, removed
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (e.Control is StiWizardPage)
            {
                e.Control.Dock = DockStyle.Fill;
                e.Control.CreateGraphics();

                if (this.Pages.Count == 1) this.SelectedPage = this.Pages[0];

                this.Pages.LockActions = true;
                this.Pages.Add(e.Control as StiWizardPage);
                this.Pages.LockActions = false;

                Invalidate();
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);

            if (e.Control is StiWizardPage)
            {
                this.Pages.LockActions = true;
                this.Pages.Remove(e.Control as StiWizardPage);
                this.Pages.LockActions = false;

                try
                {
                    foreach (StiWizardPage page in this.Pages)
                    {
                        if (page == SelectedPage) return;
                    }

                    if (this.Pages.Count > 0)
                    {
                        SelectedPage = this.Pages[0] as StiWizardPage;
                        return;
                    }
                    SelectedPage = null;
                }
                finally
                {
                    Invalidate();
                }
            }
        }
        #endregion

        #region Browsable(false)
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
        private StiWizardPageCollection pages;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StiWizardPageCollection Pages
        {
            get
            {
                return pages;
            }
        }


        private Button backButton;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button BackButton
        {
            get
            {
                return backButton;
            }
        }


        private Button nextButton;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button NextButton
        {
            get
            {
                return nextButton;
            }
        }


        private Button cancelButton;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button CancelButton
        {
            get
            {
                return cancelButton;
            }
        }


        private Button helpButton;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button HelpButton
        {
            get
            {
                return helpButton;
            }
        }


        private Button finishButton;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Button FinishButton
        {
            get
            {
                return finishButton;
            }
        }


        [Browsable(false)]
        internal bool IsDesignMode
        {
            get
            {
                return DesignMode;
            }
        }


        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }


        private StiWizardPage selectedPage = null;
        /// <summary>
        /// Gets or sets the currently-selected wizard page.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StiWizardPage SelectedPage
        {
            get
            {
                return selectedPage;
            }
            set
            {
                foreach (StiWizardPage page in this.Pages)
                {
                    if (page == value)
                    {
                        page.Dock = DockStyle.Fill;
                        page.Show();
                    }
                }

                foreach (StiWizardPage page in this.Pages)
                {
                    if (page != value) page.Hide();
                }

                selectedPage = value;
                SetLayout();
                this.Invalidate();
                InvokeSelectedIndexChanged(this, EventArgs.Empty);

            }
        }


        /// <summary>
        /// Gets or sets the tab order of the control within its container.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get
            {
                return this.Pages.IndexOf(SelectedPage);
            }
            set
            {
                if (value >= 0 && value < this.Pages.Count)
                    SelectedPage = this.Pages[value] as StiWizardPage;
            }
        }


        [DefaultValue("< &Back")]
        public string BackText
        {
            get
            {
                return BackButton.Text;
            }
            set
            {
                BackButton.Text = value;
            }
        }


        [DefaultValue("&Next >")]
        public string NextText
        {
            get
            {
                return NextButton.Text;
            }
            set
            {
                NextButton.Text = value;
            }
        }


        [DefaultValue("&Help")]
        public string HelpText
        {
            get
            {
                return HelpButton.Text;
            }
            set
            {
                HelpButton.Text = value;
            }
        }


        [DefaultValue("Cancel")]
        public string CancelText
        {
            get
            {
                return CancelButton.Text;
            }
            set
            {
                CancelButton.Text = value;
            }
        }


        [DefaultValue("&Finish")]
        public string FinishText
        {
            get
            {
                return FinishButton.Text;
            }
            set
            {
                FinishButton.Text = value;
            }
        }


        public bool HelpButtonVisible
        {
            get
            {
                return HelpButton.Visible;
            }
            set
            {
                HelpButton.Visible = value;
            }
        }


        private bool allowPageSwitch = false;
        [DefaultValue(false)]
        [Category("Appearance")]
        public bool AllowPageSwitch
        {
            get
            {
                return allowPageSwitch;
            }
            set
            {
                allowPageSwitch = value;
            }
        }

        private bool drawHeaderBorder = false;
        [DefaultValue(false)]
        [Category("Appearance")]
        public bool DrawHeaderBorder
        {
            get
            {
                return drawHeaderBorder;
            }
            set
            {
                drawHeaderBorder = value;
                Invalidate();
            }
        }


        private bool fillAcceptCancelProps = true;
        [DefaultValue(true)]
        public bool FillAcceptCancelProps
        {
            get
            {
                return fillAcceptCancelProps;
            }
            set
            {
                if (fillAcceptCancelProps != value)
                {
                    fillAcceptCancelProps = value;
                    SetLayout();
                }
            }
        }


        private bool showProgressTree = true;
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool ShowProgressTree
        {
            get
            {
                return showProgressTree;
            }
            set
            {
                showProgressTree = value;
                SetLayout();
                Invalidate();
            }
        }


        private bool progressTreeOneLevel = false;
        [DefaultValue(false)]
        [Category("Appearance")]
        public bool ProgressTreeOneLevel
        {
            get
            {
                return progressTreeOneLevel;
            }
            set
            {
                progressTreeOneLevel = value;
                Invalidate();
            }
        }


        private bool markLastPage = true;
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool MarkLastPage
        {
            get
            {
                return markLastPage;
            }
            set
            {
                markLastPage = value;
                Invalidate();
            }
        }

        private int treeProgressWidth = 200;
        [DefaultValue(200)]
        [Category("Appearance")]
        public int TreeProgressWidth
        {
            get
            {
                return treeProgressWidth;
            }
            set
            {
                treeProgressWidth = value;
                SetLayout();
                Invalidate();
            }
        }

        private int headerHeight = 66;
        [DefaultValue(66)]
        [Category("Appearance")]
        public int HeaderHeight
        {
            get
            {
                return headerHeight;
            }
            set
            {
                headerHeight = value;
                SetLayout();
                Invalidate();
            }
        }

        private int dockPaddingFromLeft = 0;
        [DefaultValue(0)]
        [Category("Appearance")]
        public int DockPaddingFromLeft
        {
            get
            {
                return dockPaddingFromLeft;
            }
            set
            {
                dockPaddingFromLeft = value;
                SetLayout();
                Invalidate();
            }
        }

        private int dockPaddingFromRight = 0;
        [DefaultValue(0)]
        [Category("Appearance")]
        public int DockPaddingFromRight
        {
            get
            {
                return dockPaddingFromRight;
            }
            set
            {
                dockPaddingFromRight = value;
                SetLayout();
                Invalidate();
            }
        }


        private int dockPaddingFromBottom = 0;
        [DefaultValue(0)]
        [Category("Appearance")]
        public int DockPaddingFromBottom
        {
            get
            {
                return dockPaddingFromBottom;
            }
            set
            {
                dockPaddingFromBottom = value;
                SetLayout();
                Invalidate();
            }
        }

        private bool showProgressLine = true;
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool ShowProgressLine
        {
            get
            {
                return showProgressLine;
            }
            set
            {
                showProgressLine = value;
                Invalidate();
            }
        }

        private bool showHeader = true;
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool ShowHeader
        {
            get
            {
                return showHeader;
            }
            set
            {
                showHeader = value;
                SetLayout();
                Invalidate();
            }
        }

        private bool showHeaderText = true;
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool ShowHeaderText
        {
            get
            {
                return showHeaderText;
            }
            set
            {
                showHeaderText = value;
                SetLayout();
                Invalidate();
            }
        }

        private bool showHeaderImageAtRight = true;
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool ShowHeaderImageAtRight
        {
            get
            {
                return showHeaderImageAtRight;
            }
            set
            {
                showHeaderImageAtRight = value;
                SetLayout();
                Invalidate();
            }
        }

        private bool headerOnAll = true;
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool HeaderOnAll
        {
            get
            {
                return headerOnAll;
            }
            set
            {
                headerOnAll = value;
                SetLayout();
                Invalidate();
            }
        }

        private bool drawPageBorders = true;
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool DrawPageBorders
        {
            get
            {
                return drawPageBorders;
            }
            set
            {
                drawPageBorders = value;
                SetLayout();
                Invalidate();
            }
        }

        private Font headerTitleFont;
        public Font HeaderTitleFont
        {
            get
            {
                return headerTitleFont;
            }
            set
            {
                headerTitleFont = value;
                Invalidate();
            }
        }

        private Font headerDescriptionFont;
        public Font HeaderDescriptionFont
        {
            get
            {
                return headerDescriptionFont;
            }
            set
            {
                headerDescriptionFont = value;
                Invalidate();
            }
        }

        private Font treeProgressFont;
        public Font TreeProgressFont
        {
            get
            {
                return treeProgressFont;
            }
            set
            {
                treeProgressFont = value;
                Invalidate();
            }
        }

        private Color headerColor = Color.White;
        [Category("Appearance")]
        public Color HeaderColor
        {
            get
            {
                return headerColor;
            }
            set
            {
                headerColor = value;
                Invalidate();
            }
        }

        private Color headerTextColor = Color.Empty;
        [Category("Appearance")]
        public Color HeaderTextColor
        {
            get
            {
                return headerTextColor;
            }
            set
            {
                headerTextColor = value;
                Invalidate();
            }
        }

        private Color contentColor = Color.Empty;
        [Category("Appearance")]
        public Color ContentColor
        {
            get
            {
                return contentColor;
            }
            set
            {
                contentColor = value;
                Invalidate();
            }
        }

        private Image headerBackgroundImage = null;
        [Category("Appearance")]
        public Image HeaderBackgroundImage
        {
            get
            {
                return headerBackgroundImage;
            }
            set
            {
                headerBackgroundImage = value;
                Invalidate();
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
            }
        }
        #endregion

        #region Methods
        internal Rectangle GetBackButtonRect()
        {
            Rectangle rect = GetControlRect();
            rect.Y += 14;
            rect.Height = 24;

            rect.Location = new Point(rect.Right - 249 - 73, rect.Y);
            rect.Size = new Size(73, 23);
            return rect;
        }

        internal Rectangle GetNextButtonRect()
        {
            Rectangle rect = GetControlRect();
            rect.Y += 14;
            rect.Height = 24;

            rect.Location = new Point(rect.Right - 174 - 73, rect.Y);
            rect.Size = new Size(73, 23);
            return rect;
        }

        internal Rectangle GetCancelButtonRect()
        {
            Rectangle rect = GetControlRect();
            rect.Y += 14;
            rect.Height = 24;

            rect.Location = new Point(rect.Right - 87, rect.Y);
            rect.Size = new Size(73, 23);
            return rect;
        }

        internal Rectangle GetFinishButtonRect()
        {
            Rectangle rect = GetControlRect();
            rect.Y += 14;
            rect.Height = 24;

            rect.Location = new Point(rect.Right - 174, rect.Y);
            rect.Size = new Size(73, 23);
            return rect;
        }

        internal Rectangle GetHelpButtonRect()
        {
            Rectangle rect = GetControlRect();
            rect.Y += 14;
            rect.Height = 24;

            rect.Location = new Point(rect.X + 14, rect.Y);
            rect.Size = new Size(73, 23);
            return rect;
        }

        internal StiWizardPage GetWizardPageAtPoint(Point pos)
        {
            int index = 0;
            while (index < this.Pages.Count)
            {
                Rectangle rect = this.GetBoxRect(index);
                if (rect.Contains(pos)) return this.Pages[index];

                rect = this.GetBoxTextRect(index);
                if (rect.Contains(pos)) return this.Pages[index];
                index++;
            }
            return null;
        }

        private Rectangle GetTreeProgressRect()
        {
            if (ShowProgressTree)
            {
                return new Rectangle(0, (HeaderOnAll && ShowHeader) ? HeaderHeight : 0,
                    200, Height - 50 - (HeaderOnAll && ShowHeader ? HeaderHeight : 0));
            }
            else return Rectangle.Empty;
        }

        private Rectangle GetHeaderRect()
        {
            if (ShowHeader)
            {
                int startPos = (HeaderOnAll || (!ShowProgressTree)) ? 0 : TreeProgressWidth;

                Rectangle rect = new Rectangle(startPos, 0, Width - startPos, HeaderHeight);
                if (DrawHeaderBorder)
                {
                    rect.Inflate(-2, 0);
                    rect.Y += 2;
                    rect.Height -= 2;
                }
                return rect;
            }
            else return Rectangle.Empty;
        }

        private Rectangle GetHeaderTitleRect()
        {
            if (!ShowHeaderText) return Rectangle.Empty;

            Rectangle rect = GetHeaderRect();
            Rectangle imageRect = GetHeaderImageRect();
            if (ShowHeaderImageAtRight)
                return new Rectangle(rect.X + 10, rect.Y + 10, Width - 20 - imageRect.Width, 20);
            else
                return new Rectangle(imageRect.Right + 10, rect.Y + 10, Width - 10, 20);
        }

        private Rectangle GetHeaderDescriptionRect()
        {
            Rectangle rect = GetHeaderTitleRect();
            if (rect.IsEmpty) rect = GetHeaderRect();
            Rectangle imageRect = GetHeaderImageRect();

            if (ShowHeaderText)
            {
                if (ShowHeaderImageAtRight)
                    return new Rectangle(rect.X + 10, rect.Y + 20, rect.Width - 20 - imageRect.Width, rect.Height - rect.Y + 20);
                else
                    return new Rectangle(imageRect.Right + 10, rect.Y + 20, rect.Width - 10, rect.Height - rect.Y + 20);
            }
            else
            {
                if (ShowHeaderImageAtRight)
                    return new Rectangle(rect.X + 10, rect.Y, rect.Width - 20 - imageRect.Width, rect.Height);
                else
                    return new Rectangle(imageRect.Right + 10, rect.Y, rect.Width - 10, rect.Height);
            }
        }

        private Rectangle GetHeaderImageRect()
        {
            if (SelectedPage.Image == null) return Rectangle.Empty;
            Rectangle rect = GetHeaderRect();

            if (this.ShowHeaderImageAtRight)
                rect.X = Width - SelectedPage.Image.Width - 10;
            else
                rect.X = 10;

            rect.Y = (rect.Height - SelectedPage.Image.Height) / 2;

            rect.Width = SelectedPage.Image.Width;
            rect.Height = SelectedPage.Image.Height;
            return rect;
        }

        private Rectangle GetControlRect()
        {
            return new Rectangle(0, Height - 50, Width, 50);
        }

        private Rectangle GetBoxTextRect(int index)
        {
            Rectangle rect = GetBoxRect(index);
            rect.X += step + 2;
            rect.Y -= step >> 1;
            rect.Width = 200 - rect.X - 5;
            rect.Height += step;

            if (index == 0 || (this.Pages.Count - 1) == index)
            {
                if (!ProgressTreeOneLevel) rect.X += 6;
            }

            return rect;
        }

        internal Rectangle GetBoxRect(int index)
        {
            Rectangle rectTree = GetTreeProgressRect();

            Rectangle rect = new Rectangle(8, rectTree.Top + 16 + index * step * 2, step, step);
            if ((!ProgressTreeOneLevel) && index > 0 && index < this.Pages.Count - 1)
            {
                rect.X += step;
            }
            return rect;
        }

        private void DrawBox(Rectangle fullRect, Graphics g, int index)
        {
            Rectangle textRect = GetBoxTextRect(index);
            textRect.X -= 16;
            textRect.Y += 1;

            FontStyle fs = 0;
            Brush foreground;
            if (index == SelectedIndex)
            {
                foreground = Brushes.White;
                fs = FontStyle.Bold;

                var selRect = new Rectangle(fullRect.X, textRect.Top, fullRect.Width, textRect.Height);
                using (var fillBrush = new SolidBrush(Color.FromArgb(23, 97, 197)))
                {
                    g.FillRectangle(fillBrush, selRect);
                }
                var pen = new Pen(Color.FromArgb(5, 57, 139));
                g.DrawRectangle(pen, selRect);

                var rectTemp = new Rectangle(selRect.X + 1, selRect.Y + 1, selRect.Width - 1, 3);
                using (var brush = new LinearGradientBrush(rectTemp, Color.FromArgb(16, 80, 175), Color.FromArgb(23, 97, 197), 90f))
                {
                    g.FillRectangle(brush, rectTemp);
                }
                rectTemp = new Rectangle(selRect.X + 1, selRect.Bottom - 3, selRect.Width - 1, 3);
                using (var brush = new LinearGradientBrush(rectTemp, Color.FromArgb(16, 80, 175), Color.FromArgb(23, 97, 197), 270f))
                {
                    g.FillRectangle(brush, rectTemp);
                }

                int height = (selRect.Height % 2 == 0) ? 12 : 11;
                rectTemp = new Rectangle(fullRect.Right - 6, selRect.Top + ((selRect.Height - height) / 2), 6, height);
                using (var path = new GraphicsPath())
                {
                    path.AddLines(new[]
                    {
                        new Point(rectTemp.Right, rectTemp.Top),
                        new Point(rectTemp.Left, rectTemp.Top + rectTemp.Height / 2),
                        new Point(rectTemp.Right, rectTemp.Bottom)
                    });

                    var mode = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    g.FillPath(Brushes.White, path);
                    g.DrawPath(pen, path);

                    g.SmoothingMode = mode;

                    using (var pen1 = new Pen(Brushes.White))
                    {
                        g.DrawLine(pen1, rectTemp.Right, rectTemp.Top + 1, rectTemp.Right, rectTemp.Bottom - 1);
                    }
                }

                pen.Dispose();
            }
            else
            {
                foreground = Brushes.Black;
            }

            using (var font = new Font(TreeProgressFont.FontFamily.Name, TreeProgressFont.Size, fs))
            using (var sf = new StringFormat())
            {
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Near;
                string text = this.Pages[index].Text;

                g.DrawString(text, font, foreground, textRect, sf);
            }
        }

        private void SetLayout()
        {
            DockPadding.Top = ShowHeader ? HeaderHeight : 0;
            DockPadding.Left = ShowProgressTree ? TreeProgressWidth : DockPaddingFromLeft;
            DockPadding.Right = DockPaddingFromRight;
            DockPadding.Bottom = 50 + DockPaddingFromBottom;

            BackButton.Bounds = GetBackButtonRect();
            NextButton.Bounds = GetNextButtonRect();
            CancelButton.Bounds = GetCancelButtonRect();
            FinishButton.Bounds = GetFinishButtonRect();
            HelpButton.Bounds = GetHelpButtonRect();

            UpdateWizardState();

            if (FillAcceptCancelProps)
            {
                Form form = this.FindForm();
                if (form != null)
                {
                    if (NextButton.Visible) form.AcceptButton = NextButton;
                    else form.AcceptButton = FinishButton;
                    form.CancelButton = CancelButton;
                }
            }
        }

        public void UpdateWizardState()
        {

            if (SelectedIndex == 0) BackButton.Enabled = false;
            else BackButton.Enabled = true;

            if (SelectedIndex == this.Pages.Count - 1)
            {
                NextButton.Enabled = false;
                FinishButton.Enabled = true;
                FinishButton.Focus();
            }
            else
            {
                StiCanNextEventArgs e = new StiCanNextEventArgs();
                InvokeOnCanNext(e);
                NextButton.Enabled = e.CanNext;

                StiCanFinishEventArgs ee = new StiCanFinishEventArgs();
                InvokeOnCanFinish(ee);
                FinishButton.Enabled = ee.CanFinish;
            }
        }
        #endregion

        #region Handlers
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (AllowPageSwitch && ShowProgressTree)
            {
                Point pos = PointToClient(Cursor.Position);
                StiWizardPage page = GetWizardPageAtPoint(pos);
                if (page != null)
                {
                    SelectedPage = page;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs p)
        {
            var g = p.Graphics;
            var rect2 = GetControlRect();

            if (!ContentColor.IsEmpty)
            {
                using (var brush = new SolidBrush(ContentColor))
                {
                    g.FillRectangle(brush, new Rectangle(0, 0, this.Width, rect2.Y));
                }
            }

            if (ShowProgressTree)
            {
                var rect = GetTreeProgressRect();
                g.SetClip(rect);

                using (var brush = new SolidBrush(Color.White))//Color.FromArgb(236, 244, 251)))
                {
                    g.FillRectangle(brush, rect);
                }

                #region Draw Progress
                if (this.Pages.Count > 1)
                {
                    var rectStart = GetBoxRect(0);
                    var rectEnd = GetBoxRect(pages.Count - 1);

                    if (this.Pages.Count > 2)
                    {
                        var rectPr = new Rectangle(
                            rectStart.X + (step >> 1),
                            rectStart.Y + (step >> 1),
                            step,
                            rectEnd.Bottom - rectStart.Bottom);

                        if (ProgressTreeOneLevel)
                        {
                            if (ShowProgressLine)
                                g.DrawLine(Pens.White,
                                    rectPr.X, rectPr.Y,
                                    rectPr.X, rectPr.Bottom);
                        }
                        else
                        {
                            if (ShowProgressLine) g.DrawRectangle(Pens.White, rectPr);
                        }
                    }
                    else
                    {
                        if (ShowProgressLine)
                        {
                            g.DrawLine(Pens.White,
                                rectStart.X + (step >> 1),
                                rectStart.Y + (step >> 1),
                                rectEnd.X + (step >> 1),
                                rectEnd.Y + (step >> 1));
                        }
                    }
                }

                if (this.ControlStyle == StiControlStyle.Office2013Blue)
                {
                    using (var pen = new Pen(Color.FromArgb(198, 198, 198)) { DashPattern = new float[] { 1f, 2f } })
                    {
                        g.DrawLine(pen, rect.Right - 1, rect.Y, rect.Right - 1, rect.Bottom);
                    }
                }
                else
                {
                    g.DrawLine(SystemPens.ControlDark, rect.Right - 1, rect.Y, rect.Right - 1, rect.Bottom);
                }

                

                rect.X += 1;
                rect.Width -= 3;
                for (int index = 0; index < this.Pages.Count; index++)
                {
                    DrawBox(rect, p.Graphics, index);
                }
                #endregion

                g.ResetClip();
            }

            if (this.ControlStyle == StiControlStyle.Office2013Blue)
            {
                using (Pen pen = new Pen(Color.FromArgb(198, 198, 198)) { DashPattern = new float[] { 1f, 2f } })
                {
                    g.DrawLine(pen, rect2.X, rect2.Y, rect2.Right, rect2.Y);
                }
            }
            else
            {
                g.DrawLine(SystemPens.ControlDark, rect2.X, rect2.Y, rect2.Right, rect2.Y);
                g.DrawLine(SystemPens.ControlLightLight, rect2.X, rect2.Y + 1, rect2.Right, rect2.Y + 1);
            }

            #region Draw Header
            if (ShowHeader && SelectedPage != null)
            {
                var rect = GetHeaderRect();
                if (HeaderBackgroundImage != null)
                {
                    g.DrawImage(HeaderBackgroundImage, rect);
                }
                else
                {
                    using (var brush = new SolidBrush(HeaderColor))
                    {
                        g.FillRectangle(brush, rect);
                    }
                }

                if (DrawHeaderBorder)
                {
                    var headerBorder = rect;
                    headerBorder.Width--;
                    headerBorder.Height--;
                    g.DrawRectangle(SystemPens.ControlDark, headerBorder);
                }

                if (ShowHeaderText)
                {
                    g.DrawString(SelectedPage.Text, HeaderTitleFont,
                        SystemBrushes.ControlText, GetHeaderTitleRect(),
                        sfHeaderTitle);
                }

                g.SetClip(GetHeaderDescriptionRect());

                if (!HeaderTextColor.IsEmpty)
                {
                    using (var brush = new SolidBrush(HeaderTextColor))
                    {
                        g.DrawString(SelectedPage.Description, HeaderDescriptionFont,
                            brush, GetHeaderDescriptionRect(),
                            sfHeaderDescription);
                    }
                }
                else
                {
                    g.DrawString(SelectedPage.Description, HeaderDescriptionFont,
                        SystemBrushes.ControlText, GetHeaderDescriptionRect(),
                        sfHeaderDescription);
                }
                
                g.ResetClip();

                if (SelectedPage.Image != null) g.DrawImage(SelectedPage.Image, GetHeaderImageRect());

                if (DrawPageBorders)
                {
                    if (this.ControlStyle == StiControlStyle.Office2013Blue)
                    {
                        using (var pen = new Pen(Color.FromArgb(198, 198, 198)) { DashPattern = new float[] { 1f, 2f } })
                        {
                            g.DrawLine(pen, rect.X, rect.Bottom - 1, rect.Right, rect.Bottom - 1);
                        }
                    }
                    else
                    {
                        g.DrawLine(SystemPens.ControlLightLight, rect.X, rect.Bottom - 1, rect.Right, rect.Bottom - 1);
                        g.DrawLine(SystemPens.ControlDark, rect.X, rect.Bottom - 2, rect.Right, rect.Bottom - 2);
                    }
                }
            }
            #endregion
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            SetLayout();
        }


        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            SetLayout();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetLayout();
        }
        #endregion

        #region Events
        #region TitleClick
        /// <summary>
        /// Occurs when the control title is clicked.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler TitleClick;

        protected virtual void OnTitleClick(EventArgs e)
        {

        }

        /// <summary>
        /// Raises the TitleClick event for the specified control.
        /// </summary>
        /// <param name="sender">The Control to assign the TitleClick event to. </param>
        /// <param name="e">An EventArgs that contains the event data. </param>
        public virtual void InvokeTitleClick(object sender, EventArgs e)
        {
            try
            {
                OnTitleClick(EventArgs.Empty);
                if (this.TitleClick != null) this.TitleClick(null, EventArgs.Empty);
            }
            catch
            {
            }
        }

        #endregion

        #region SelectedIndexChanged
        /// <summary>
        /// Occurs when the SelectedIndex property is changed.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler SelectedIndexChanged;

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {

        }

        /// <summary>
        /// Raises the SelectedIndexChanged event for the specified control.
        /// </summary>
        /// <param name="sender">The Control to assign the SelectedIndexChanged event to. </param>
        /// <param name="e">An EventArgs that contains the event data. </param>
        public virtual void InvokeSelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedIndexChanged(EventArgs.Empty);
            if (SelectedIndexChanged != null) SelectedIndexChanged(sender, e);
        }
        #endregion

        #region Cancel
        public void InvokeOnCancelClick(EventArgs e)
        {
            OnCancelClick(e);
        }


        protected void OnCancelClick(EventArgs e)
        {
            OnCancelClick(this, e);
        }


        private void OnCancelClick(object sender, EventArgs e)
        {
            if (CancelClick != null) CancelClick(sender, e);
        }


        public event EventHandler CancelClick;
        #endregion

        #region Back
        public void InvokeOnBackClick(EventArgs e)
        {
            OnBackClick(e);
        }


        protected void OnBackClick(EventArgs e)
        {
            OnBackClick(this, e);
        }


        private void OnBackClick(object sender, EventArgs e)
        {
            if (SelectedIndex > 0)
            {
                StiBeforePageChangedEventArgs ee = new StiBeforePageChangedEventArgs(
                    this.Pages[this.SelectedIndex] as StiWizardPage,
                    this.Pages[this.SelectedIndex - 1] as StiWizardPage);
                InvokeOnBeforeBackClick(ee);

                if (!ee.Cancel)
                {
                    SelectedIndex--;
                    UpdateWizardState();

                    if (BackClick != null) BackClick(sender, e);
                }
            }
        }


        public event EventHandler BackClick;
        #endregion

        #region Next
        public void InvokeOnNextClick(EventArgs e)
        {
            OnNextClick(e);
        }


        protected void OnNextClick(EventArgs e)
        {
            OnNextClick(this, e);
        }


        private void OnNextClick(object sender, EventArgs e)
        {
            if (SelectedIndex < this.Pages.Count - 1)
            {
                StiBeforePageChangedEventArgs ee = new StiBeforePageChangedEventArgs(
                    this.Pages[this.SelectedIndex] as StiWizardPage,
                    this.Pages[this.SelectedIndex + 1] as StiWizardPage);
                InvokeOnBeforeNextClick(ee);

                if (!ee.Cancel)
                {
                    SelectedIndex++;
                    UpdateWizardState();

                    if (NextClick != null) NextClick(sender, e);
                }
            }
        }


        public event EventHandler NextClick;

        #endregion

        #region BeforeBack
        public void InvokeOnBeforeBackClick(StiBeforePageChangedEventArgs e)
        {
            OnBeforeBackClick(e);
        }


        protected void OnBeforeBackClick(StiBeforePageChangedEventArgs e)
        {
            OnBeforeBackClick(this, e);
        }


        private void OnBeforeBackClick(object sender, StiBeforePageChangedEventArgs e)
        {
            if (BeforeBackClick != null) BeforeBackClick(sender, e);
        }


        public event StiBeforePageChangedHandler BeforeBackClick;
        #endregion

        #region BeforeNext
        public void InvokeOnBeforeNextClick(StiBeforePageChangedEventArgs e)
        {
            OnBeforeNextClick(e);
        }


        protected void OnBeforeNextClick(StiBeforePageChangedEventArgs e)
        {
            OnBeforeNextClick(this, e);
        }


        private void OnBeforeNextClick(object sender, StiBeforePageChangedEventArgs e)
        {
            if (BeforeNextClick != null) BeforeNextClick(sender, e);
        }


        public event StiBeforePageChangedHandler BeforeNextClick;
        #endregion

        #region BeforeFinish
        public void InvokeOnBeforeFinishClick(StiBeforeFinishEventArgs e)
        {
            OnBeforeFinishClick(e);
        }


        protected void OnBeforeFinishClick(StiBeforeFinishEventArgs e)
        {
            OnBeforeFinishClick(this, e);
        }


        private void OnBeforeFinishClick(object sender, StiBeforeFinishEventArgs e)
        {
            if (BeforeFinishClick != null) BeforeFinishClick(sender, e);
        }


        public event StiBeforeFinishHandler BeforeFinishClick;
        #endregion

        #region Finish
        public void InvokeOnFinishClick(EventArgs e)
        {
            OnFinishClick(e);
        }


        protected void OnFinishClick(EventArgs e)
        {
            StiBeforeFinishEventArgs ee = new StiBeforeFinishEventArgs();
            InvokeOnBeforeFinishClick(ee);

            if (!ee.Cancel)
            {
                OnFinishClick(this, e);
            }
        }


        private void OnFinishClick(object sender, EventArgs e)
        {
            if (FinishClick != null) FinishClick(sender, e);
        }


        public event EventHandler FinishClick;
        #endregion

        #region Help
        public void InvokeOnHelpClick(EventArgs e)
        {
            OnHelpClick(e);
        }


        private void OnHelpClick(object sender, EventArgs e)
        {
            if (HelpClick != null) HelpClick(sender, e);
        }


        protected void OnHelpClick(EventArgs e)
        {
            OnHelpClick(this, e);
        }


        public event EventHandler HelpClick;
        #endregion

        #region CanNext
        public void InvokeOnCanNext(StiCanNextEventArgs e)
        {
            OnCanNext(e);
        }


        private void OnCanNext(object sender, StiCanNextEventArgs e)
        {
            if (CanNext != null) CanNext(sender, e);
        }


        protected void OnCanNext(StiCanNextEventArgs e)
        {
            OnCanNext(this, e);
        }


        public event StiCanNextHandler CanNext;
        #endregion

        #region CanFinish
        public void InvokeOnCanFinish(StiCanFinishEventArgs e)
        {
            OnCanFinish(e);
        }


        private void OnCanFinish(object sender, StiCanFinishEventArgs e)
        {
            if (CanFinish != null) CanFinish(sender, e);
        }


        protected void OnCanFinish(StiCanFinishEventArgs e)
        {
            OnCanFinish(this, e);
        }


        public event StiCanFinishHandler CanFinish;
        #endregion
        #endregion

        #region Control Added, removed
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                sfHeaderTitle.Dispose();
                sfHeaderDescription.Dispose();

                headerTitleFont.Dispose();
                headerDescriptionFont.Dispose();
            }
            base.Dispose(disposing);

        }

        #endregion

        #region Constructors
        public StiWizard()
        {
            this.pages = new StiWizardPageCollection(this);

            #region Buttons
            this.nextButton = new Button();
            this.NextButton.TabIndex = 1000;
            this.NextButton.Click += new EventHandler(OnNextClick);

            this.finishButton = new Button();
            this.FinishButton.DialogResult = DialogResult.OK;
            this.FinishButton.TabIndex = 1001;
            this.FinishButton.Click += new EventHandler(OnFinishClick);

            this.cancelButton = new Button();
            this.CancelButton.DialogResult = DialogResult.Cancel;
            this.CancelButton.TabIndex = 1002;
            this.CancelButton.Click += new EventHandler(OnCancelClick);

            this.helpButton = new Button();
            this.HelpButton.TabIndex = 1003;
            this.HelpButton.Click += new EventHandler(OnHelpClick);

            this.backButton = new Button();
            this.BackButton.TabIndex = 1004;
            this.BackButton.Enabled = false;
            this.BackButton.Click += new EventHandler(OnBackClick);

            this.Controls.Add(BackButton);
            this.Controls.Add(HelpButton);
            this.Controls.Add(CancelButton);
            this.Controls.Add(FinishButton);
            this.Controls.Add(NextButton);
            #endregion

            #region Button Text
            this.BackText = "< &Back";
            this.NextText = "&Next >";
            this.CancelText = "Cancel";
            this.FinishText = "&Finish";
            this.HelpText = "&Help";
            #endregion

            #region sfHeaderTitle
            sfHeaderTitle = new StringFormat
            {
                HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show,
                LineAlignment = StringAlignment.Near,
                Alignment = StringAlignment.Near,
                FormatFlags = StringFormatFlags.NoWrap,
                Trimming = StringTrimming.EllipsisCharacter
            };
            if (this.RightToLeft == RightToLeft.Yes)
                sfHeaderTitle.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            #endregion

            #region sfHeaderDescription
            sfHeaderDescription = new StringFormat
            {
                HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show,
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near,
                Trimming = StringTrimming.EllipsisCharacter
            };
            if (this.RightToLeft == RightToLeft.Yes)
                sfHeaderDescription.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            #endregion

            #region Fonts
            headerTitleFont = new Font(Font.FontFamily.Name, 10, FontStyle.Bold);
            headerDescriptionFont = new Font(Font.FontFamily.Name, 8);
            treeProgressFont = new Font(Font.FontFamily.Name, 8);
            #endregion

            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            SetLayout();
        }
        #endregion
    }
}