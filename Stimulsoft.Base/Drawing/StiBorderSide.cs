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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Drawing
{
    public class StiBorderSide : 
        ICloneable
    {
        #region ICloneable
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone()
        {
            StiBorderSide border = base.MemberwiseClone() as StiBorderSide;
            border.style = this.style;
            return border;
        }
        #endregion

        #region IEquatable
        protected bool Equals(StiBorderSide other)
        {
            return size.Equals(other.size) && color.Equals(other.color) && side == other.side && style == other.style;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as StiBorderSide;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = size.GetHashCode();
                hashCode = (hashCode*397) ^ color.GetHashCode();
                hashCode = (hashCode*397) ^ (int) side;
                hashCode = (hashCode*397) ^ (int) style;
                return hashCode;
            }
        }

        public static bool operator ==(StiBorderSide left, StiBorderSide right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(StiBorderSide left, StiBorderSide right)
        {
            return !Equals(left, right);
        }


        //public override bool Equals(Object obj)
        //{
        //    StiBorderSide borderSide = obj as StiBorderSide;

        //    if (borderSide == null) return false;

        //    if (!this.side.Equals(borderSide.side)) return false;
        //    if (this.color != borderSide.color) return false;
        //    if (this.size != borderSide.size) return false;
        //    if (!this.style.Equals(borderSide.style)) return false;

        //    return true;
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
        #endregion

        #region Methods
        public float GetSizeOffset()
        {
            if (this.Style == StiPenStyle.None) return 0f;
            if (this.Style == StiPenStyle.Double) return 1f;
            if (this.Style == StiPenStyle.Solid) return Math.Max(1, (int)this.Size);
            
            return Math.Max(1, (int) (this.Size * 0.5f));
        }

        internal void Draw(Graphics g, RectangleF rect, float zoom, Color emptyColor, StiAdvancedBorder border)
        {
            if (this.Style == StiPenStyle.None) return;
            else if (this.Style == StiPenStyle.Double)DrawDoubleInternal(g, rect, zoom, emptyColor, border);
            else if (this.Style == StiPenStyle.Solid && Size >= 1) DrawSolidInternal(g, rect, zoom, border);
            else DrawInternal(g, rect, zoom, border);
        }

        internal void DrawInternal(Graphics g, RectangleF rect, float zoom, StiAdvancedBorder border)
        {
            using (var pen = new Pen(this.Color))
            {
                pen.DashStyle = StiPenUtils.GetPenStyle(style);
                pen.Width = (int)(this.Size * zoom);
                pen.StartCap = LineCap.NoAnchor;
                pen.EndCap = LineCap.NoAnchor;

                rect =  GetAdvancedBorderRectangle(g, rect, zoom, border);

                switch (side)
                {
                    case StiBorderSides.Top:
                        if (border.IsTopBorderSidePresent)
                            g.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);
                        break;

                    case StiBorderSides.Bottom:
                        if (border.IsBottomBorderSidePresent)
                            g.DrawLine(pen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                        break;

                    case StiBorderSides.Left:
                        if (border.IsLeftBorderSidePresent)
                            g.DrawLine(pen, rect.Left, rect.Top, rect.Left, rect.Bottom);
                        break;

                    case StiBorderSides.Right:
                        if (border.IsRightBorderSidePresent)
                            g.DrawLine(pen, rect.Right, rect.Top, rect.Right, rect.Bottom);
                        break;
                }
            }
        }

        internal void DrawSolidInternal(Graphics g, RectangleF rect, float zoom, StiAdvancedBorder border)
        {
            using (var brush = new SolidBrush(this.Color))
            {
                var size = Math.Max(1, (int)(this.Size * zoom));

                rect = GetAdvancedBorderRectangle(g, rect, zoom, border);

                switch (side)
                {
                    case StiBorderSides.Top:
                        if (border.IsTopBorderSidePresent)
                            g.FillRectangle(brush, rect.Left, rect.Top, rect.Width, size);
                        break;

                    case StiBorderSides.Bottom:
                        if (border.IsBottomBorderSidePresent)
                            g.FillRectangle(brush, rect.Left, rect.Bottom - size, rect.Width, size);
                        break;

                    case StiBorderSides.Left:
                        if (border.IsLeftBorderSidePresent)
                            g.FillRectangle(brush, rect.Left, rect.Top, size, rect.Height);
                        break;

                    case StiBorderSides.Right:
                        if (border.IsRightBorderSidePresent)
                            g.FillRectangle(brush, rect.Right - size, rect.Top, size, rect.Height);
                        break;
                }
            }
        }

        private void DrawDoubleInternal(Graphics g, RectangleF rect, float zoom, Color emptyColor, StiAdvancedBorder border)
        {
            using (var emptyPen = new Pen(emptyColor))
            using (var pen = new Pen(this.Color, 1))
            {
                pen.DashStyle = StiPenUtils.GetPenStyle(style);
                pen.StartCap = LineCap.Square;
                pen.EndCap = LineCap.Square;

                var rectIn = rect;
                var rectOut = rect;

                rectIn.Inflate(-1, -1);
                rectOut.Inflate(1, 1);

                var left = 0f;
                var right = 0f;
                var top = 0f;
                var bottom = 0f;

                switch (side)
                {
                    #region Top
                    case StiBorderSides.Top:
                        if (border.IsTopBorderSidePresent)
                        {
                            left = rectIn.Left;
                            right = rectIn.Right;

                            if (!border.IsLeftBorderSidePresent) left = rectOut.Left;
                            if (!border.IsRightBorderSidePresent) right = rectOut.Right;

                            g.DrawLine(emptyPen, rect.Left, rect.Top, rect.Right, rect.Top);
                            g.DrawLine(pen, left, rectIn.Top, right, rectIn.Top);
                            g.DrawLine(pen, rectOut.Left, rectOut.Top, rectOut.Right, rectOut.Top);
                        }
                        break;
                    #endregion

                    #region Bottom
                    case StiBorderSides.Bottom:
                        if (border.IsBottomBorderSidePresent)
                        {
                            left = rectIn.Left;
                            right = rectIn.Right;

                            if (!border.IsLeftBorderSidePresent) left = rectOut.Left;
                            if (!border.IsRightBorderSidePresent) right = rectOut.Right;

                            g.DrawLine(emptyPen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                            g.DrawLine(pen, left, rectIn.Bottom, right, rectIn.Bottom);
                            g.DrawLine(pen, rectOut.Left, rectOut.Bottom, rectOut.Right, rectOut.Bottom);
                        }
                        break;
                    #endregion

                    #region Left
                    case StiBorderSides.Left:
                        if (border.IsLeftBorderSidePresent)
                        {
                            top = rectIn.Top;
                            bottom = rectIn.Bottom;

                            if (!border.IsTopBorderSidePresent) top = rectOut.Top;
                            if (!border.IsBottomBorderSidePresent) bottom = rectOut.Bottom;

                            g.DrawLine(emptyPen, rect.Left, rect.Top, rect.Left, rect.Bottom);
                            g.DrawLine(pen, rectIn.Left, top, rectIn.Left, bottom);
                            g.DrawLine(pen, rectOut.Left, rectOut.Top, rectOut.Left, rectOut.Bottom);
                        }
                        break;
                    #endregion

                    #region Right
                    case StiBorderSides.Right:
                        if (border.IsRightBorderSidePresent)
                        {
                            top = rectIn.Top;
                            bottom = rectIn.Bottom;

                            if (!border.IsTopBorderSidePresent) top = rectOut.Top;
                            if (!border.IsBottomBorderSidePresent) bottom = rectOut.Bottom;

                            g.DrawLine(emptyPen, rect.Right, rect.Top, rect.Right, rect.Bottom);
                            g.DrawLine(pen, rectIn.Right, top, rectIn.Right, bottom);
                            g.DrawLine(pen, rectOut.Right, rectOut.Top, rectOut.Right, rectOut.Bottom);
                        }
                        break;
                    #endregion
                }
            }
        }

        private RectangleF GetAdvancedBorderRectangle(Graphics g, RectangleF rect, float zoom, StiAdvancedBorder border)
        {
            if (border.IsTopBorderSidePresent)
            {
                var size = (int)(border.TopSide.Size * zoom * .5);
                rect.Y -= size;
                rect.Height += size;
            }

            if (border.IsBottomBorderSidePresent)
            {
                var size = (int)(border.BottomSide.Size * zoom * .5);
                rect.Height = rect.Height + size;
            }

            if (border.IsLeftBorderSidePresent)
            {
                var size = (int)(border.LeftSide.Size * zoom * .5);
                rect.X -= size;
                rect.Width += size;
            }

            if (border.IsRightBorderSidePresent)
            {
                var size = (int)(border.RightSide.Size * zoom * .5);
                rect.Width = rect.Width + size;
            }

            return rect;
        }
        #endregion

        #region Fields
        internal StiBorderSides side = StiBorderSides.None;
        #endregion

        #region Properties
        private Color color = Color.Black;
        /// <summary>
        /// Gets or sets a border color.
        /// </summary>
        [StiSerializable]
        [TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.StiColorConverter))]
        [Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, " + StiVersion.VersionInfo, typeof(UITypeEditor))]
        [Description("Gets or sets a border color.")]
        [RefreshProperties(RefreshProperties.All)]
        public virtual Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }


        private double size = 1d;
        /// <summary>
        /// Gets or sets a border size.
        /// </summary>
        [StiSerializable]
        [DefaultValue(1d)]
        [Description("Gets or sets a border size.")]
        [RefreshProperties(RefreshProperties.All)]
        public virtual double Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }


        private StiPenStyle style = StiPenStyle.None;
        /// <summary>
        /// Gets or sets a border style.
        /// </summary>
        [Editor("Stimulsoft.Base.Drawing.Design.StiPenStyleEditor, Stimulsoft.Report.Design, " + StiVersion.VersionInfo, typeof(UITypeEditor))]
        [StiSerializable]
        [DefaultValue(StiPenStyle.None)]
        [TypeConverter(typeof(Stimulsoft.Base.Localization.StiEnumConverter))]
        [Description("Gets or sets a border style.")]
        [RefreshProperties(RefreshProperties.All)]
        public virtual StiPenStyle Style
        {
            get
            {
                return style;
            }
            set
            {
                style = value;
            }
        }

        /// <summary>
        /// Gets value indicates, that this object-frame is by default.
        /// </summary>
        [Browsable(false)]
        public bool IsDefault
        {
            get
            {
                return 
                    Color == Color.Black &&                    
                    Size == 1d &&
                    Style == StiPenStyle.None;
            }
        }
        #endregion

        /// <summary>
        /// Creates a new instance of the StiBorderSide class.
		/// </summary>
        public StiBorderSide() :
			this(Color.Black, 1d, StiPenStyle.None)
		{
		}

        /// <summary>
        /// Creates a new instance of the StiBorderSide class.
        /// </summary>
        /// <param name="color">Border color.</param>
        /// <param name="size">Border size.</param>
        /// <param name="style">Border style.</param>
        public StiBorderSide(Color color, double size, StiPenStyle style)
        {
            this.color = color;
            this.size = size;
            this.style = style;
        }
    }
}
