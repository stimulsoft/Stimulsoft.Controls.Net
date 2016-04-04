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
	/// <summary>
	/// Class describes a multi-border.
	/// </summary>
	public class StiAdvancedBorder : StiBorder
	{
		#region ICloneable
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public override object Clone()
		{
			var border = base.Clone() as StiAdvancedBorder;
            
            border.topSide = this.topSide.Clone() as StiBorderSide;
            border.bottomSide = this.bottomSide.Clone() as StiBorderSide;
            border.leftSide = this.leftSide.Clone() as StiBorderSide;
            border.rightSide = this.rightSide.Clone() as StiBorderSide;

			return border;
		}
		#endregion

        #region IEquatable
	    protected bool Equals(StiAdvancedBorder other)
	    {
	        return base.Equals(other) && Equals(bottomSide, other.bottomSide) && Equals(topSide, other.topSide) && Equals(leftSide, other.leftSide) && Equals(rightSide, other.rightSide);
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        if (obj.GetType() != this.GetType()) return false;
	        return Equals((StiAdvancedBorder) obj);
	    }

	    public override int GetHashCode()
	    {
	        unchecked
	        {
	            int hashCode = base.GetHashCode();
	            hashCode = (hashCode*397) ^ (bottomSide != null ? bottomSide.GetHashCode() : 0);
	            hashCode = (hashCode*397) ^ (topSide != null ? topSide.GetHashCode() : 0);
	            hashCode = (hashCode*397) ^ (leftSide != null ? leftSide.GetHashCode() : 0);
	            hashCode = (hashCode*397) ^ (rightSide != null ? rightSide.GetHashCode() : 0);
	            return hashCode;
	        }
	    }

	    public static bool operator ==(StiAdvancedBorder left, StiAdvancedBorder right)
	    {
	        return Equals(left, right);
	    }

	    public static bool operator !=(StiAdvancedBorder left, StiAdvancedBorder right)
	    {
	        return !Equals(left, right);
	    }

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        //public override bool Equals(Object obj) 
        //{
        //    return base.Equals(obj);
        //}
	    #endregion

        #region Methods
		/// <summary>
		/// Draws this border on the indicated Graphics.
		/// </summary>
		/// <param name="g">Graphics on which a border can be drawn.</param>
		/// <param name="rect">The rectangle that indicates an area of the border drawing.</param>
		/// <param name="zoom">The scale of a border to draw.</param>
		/// <param name="emptyColor">The color of space between double lines (used only when border style equal Double).</param>
        public override void Draw(Graphics g, RectangleF rect, float zoom, Color emptyColor, bool drawBorderFormatting, bool drawBorderSides)
		{
            if (drawBorderFormatting)
            {
                DrawBorderShadow(g, rect, zoom);
            }

            if (drawBorderSides)
            {
                if (this.IsLeftBorderSidePresent) this.LeftSide.Draw(g, rect, zoom, emptyColor, this);
                if (this.IsRightBorderSidePresent) this.RightSide.Draw(g, rect, zoom, emptyColor, this);
                if (this.IsTopBorderSidePresent) this.TopSide.Draw(g, rect, zoom, emptyColor, this);
                if (this.IsBottomBorderSidePresent) this.BottomSide.Draw(g, rect, zoom, emptyColor, this);
            }
		}		
		#endregion

		#region Properties
        private StiBorderSide leftSide;
        /// <summary>
        /// Gets or sets frame of left side.
        /// </summary>
        [StiSerializable]
        [StiOrder(10)]
        [TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.StiBorderSideConverter))]
        [Description("Gets or sets frame of left side.")]
        [RefreshProperties(RefreshProperties.All)]
        public virtual StiBorderSide LeftSide
        {
            get
            {
                return leftSide;
            }
        }

        private StiBorderSide rightSide;
        /// <summary>
        /// Gets or sets frame of right side.
        /// </summary>
        [StiSerializable]
        [StiOrder(20)]
        [TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.StiBorderSideConverter))]
        [Description("Gets or sets frame of right side.")]
        [RefreshProperties(RefreshProperties.All)]
        public virtual StiBorderSide RightSide
        {
            get
            {
                return rightSide;
            }
        }

        private StiBorderSide topSide;
        /// <summary>
        /// Gets or sets frame of top side.
        /// </summary>
        [StiSerializable]
        [StiOrder(30)]
        [TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.StiBorderSideConverter))]
        [Description("Gets or sets frame of top side.")]
        [RefreshProperties(RefreshProperties.All)]
        public virtual StiBorderSide TopSide
        {
            get
            {
                return topSide;
            }
        }

        private StiBorderSide bottomSide;
        /// <summary>
        /// Gets or sets frame of bottom side.
        /// </summary>
        [StiSerializable]
        [StiOrder(40)]
        [TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.StiBorderSideConverter))]
        [Description("Gets or sets frame of bottom side.")]
        [RefreshProperties(RefreshProperties.All)]
        public virtual StiBorderSide BottomSide
        {
            get
            {
                return bottomSide;
            }
        }

		/// <summary>
		/// Gets value which indicates that top side of border is present.
		/// </summary>
		public override bool IsTopBorderSidePresent
		{
			get
			{
				return this.TopSide.Style != StiPenStyle.None;
			}
		}


		/// <summary>
		/// Gets value which indicates that bottom side of border is present.
		/// </summary>
		public override bool IsBottomBorderSidePresent
		{
			get
			{
                return this.BottomSide.Style != StiPenStyle.None;
			}
		}


		/// <summary>
		/// Gets value which indicates that left side of border is present.
		/// </summary>
		public override bool IsLeftBorderSidePresent
		{
			get
			{
                return this.LeftSide.Style != StiPenStyle.None;
			}
		}


		/// <summary>
		/// Gets value which indicates that right side of border is present.
		/// </summary>
		public override bool IsRightBorderSidePresent
		{
			get
			{
                return this.RightSide.Style != StiPenStyle.None;
			}
		}


		/// <summary>
		/// Gets value which indicates that all sides of border is present.
		/// </summary>
		public override bool IsAllBorderSidesPresent
		{
			get
			{
				return 
                    this.IsLeftBorderSidePresent && 
                    this.IsRightBorderSidePresent &&
                    this.IsTopBorderSidePresent &&
                    this.IsBottomBorderSidePresent;
			}
		}


		/// <summary>
        /// Gets or sets frame borders. Not used in StiAdvancedBorder.
		/// </summary>
        public override StiBorderSides Side
		{
			get 
			{
                StiBorderSides side = StiBorderSides.None;
                if (this.IsLeftBorderSidePresent) side |= StiBorderSides.Left;
                if (this.IsRightBorderSidePresent) side |= StiBorderSides.Right;
                if (this.IsTopBorderSidePresent) side |= StiBorderSides.Top;
                if (this.IsBottomBorderSidePresent) side |= StiBorderSides.Bottom;

				return side;
			}
			set 
			{
                if ((value & StiBorderSides.Left) > 0)
                {
                    if (!this.IsLeftBorderSidePresent)this.LeftSide.Style = StiPenStyle.Solid;
                }
                else this.LeftSide.Style = StiPenStyle.None;

                if ((value & StiBorderSides.Right) > 0)
                {
                    if (!this.IsRightBorderSidePresent) this.RightSide.Style = StiPenStyle.Solid;
                }
                else this.RightSide.Style = StiPenStyle.None;

                if ((value & StiBorderSides.Top) > 0)
                {
                    if (!this.IsTopBorderSidePresent) this.TopSide.Style = StiPenStyle.Solid;
                }
                else this.TopSide.Style = StiPenStyle.None;

                if ((value & StiBorderSides.Bottom) > 0)
                {
                    if (!this.IsBottomBorderSidePresent) this.BottomSide.Style = StiPenStyle.Solid;
                }
                else this.BottomSide.Style = StiPenStyle.None;
			}
		}


		/// <summary>
        /// Gets or sets a border color. Not used in StiAdvancedBorder.
		/// </summary>
        [Browsable(false)]
        [StiNonSerialized]
        public override Color Color
		{
			get 
			{
			    return this.LeftSide.Color;
			}
			set 
			{
                this.LeftSide.Color = value;
                this.RightSide.Color = value;
                this.TopSide.Color = value;
                this.BottomSide.Color = value;
			}
		}


		/// <summary>
        /// Gets or sets a border size. Not used in StiAdvancedBorder.
		/// </summary>
        [Browsable(false)]
        [StiNonSerialized]
        public override double Size
		{
			get 
			{
				return this.LeftSide.Size;
			}
			set 
			{
                this.LeftSide.Size = value;
                this.RightSide.Size = value;
                this.TopSide.Size = value;
                this.BottomSide.Size = value;
			}
		}


		/// <summary>
        /// Gets or sets a border style. Not used in StiAdvancedBorder.
		/// </summary>		
        [Browsable(false)]
        [StiNonSerialized]
        public override StiPenStyle Style
		{
			get 
			{
				return this.LeftSide.Style;
			}
			set 
			{
                this.LeftSide.Style = value;
                this.RightSide.Style = value;
                this.TopSide.Style = value;
                this.BottomSide.Style = value;
			}
		}


		/// <summary>
		/// Gets value indicates, that this object-frame is by default.
		/// </summary>
        public override bool IsDefault
		{
			get
			{
				return                     
					(!DropShadow) &&
                    (!this.Topmost) &&
					ShadowSize == 4d &&					
					ShadowBrush is StiSolidBrush &&
					((StiSolidBrush)ShadowBrush).Color == Color.Black &&
                    this.LeftSide.IsDefault &&
                    this.RightSide.IsDefault &&
                    this.TopSide.IsDefault &&
                    this.BottomSide.IsDefault;
			}
		}
		#endregion	

		/// <summary>
        /// Creates a new instance of the StiAdvancedBorder class.
		/// </summary>
		public StiAdvancedBorder() :
			this(
            new StiBorderSide(),
            new StiBorderSide(),
            new StiBorderSide(),
            new StiBorderSide(), 
            false, 4d, new StiSolidBrush(Color.Black))
		{
		}
		
	
		/// <summary>
        /// Creates a new instance of the StiAdvancedBorder class.
		/// </summary>
        public StiAdvancedBorder(
                Color topSideColor, double topSideSize, StiPenStyle topSideStyle, 
                Color bottomSideColor, double bottomSideSize, StiPenStyle bottomSideStyle, 
                Color leftSideColor, double leftSideSize, StiPenStyle leftSideStyle, 
                Color rightSideColor, double rightSideSize, StiPenStyle rightSideStyle, 
                bool dropShadow, double shadowSize, StiBrush shadowBrush) : 
            this(
                new StiBorderSide(topSideColor, topSideSize, topSideStyle), 
                new StiBorderSide(bottomSideColor, bottomSideSize, bottomSideStyle), 
                new StiBorderSide(leftSideColor, leftSideSize, leftSideStyle), 
                new StiBorderSide(rightSideColor, rightSideSize, rightSideStyle), 
                dropShadow, shadowSize, shadowBrush)
        {
        }

        /// <summary>
        /// Creates a new instance of the StiAdvancedBorder class.
        /// </summary>
        public StiAdvancedBorder(
            Color topSideColor, double topSideSize, StiPenStyle topSideStyle,
            Color bottomSideColor, double bottomSideSize, StiPenStyle bottomSideStyle,
            Color leftSideColor, double leftSideSize, StiPenStyle leftSideStyle,
            Color rightSideColor, double rightSideSize, StiPenStyle rightSideStyle,
            bool dropShadow, double shadowSize, StiBrush shadowBrush, bool topmost)
            :
            this(
                new StiBorderSide(topSideColor, topSideSize, topSideStyle),
                new StiBorderSide(bottomSideColor, bottomSideSize, bottomSideStyle),
                new StiBorderSide(leftSideColor, leftSideSize, leftSideStyle),
                new StiBorderSide(rightSideColor, rightSideSize, rightSideStyle),
                dropShadow, shadowSize, shadowBrush, topmost)
        {
        }

        /// <summary>
        /// Creates a new instance of the StiAdvancedBorder class.
        /// </summary>
        /// <param name="topSide">Top side of border.</param>
        /// <param name="bottomSide">Bottom side of border.</param>
        /// <param name="leftSide">Left side of border.</param>
        /// <param name="rightSide">Right side of border.</param>
        /// <param name="dropShadow">Drop shadow or not.</param>
        /// <param name="shadowSize">Shadow siz.</param>
        /// <param name="shadowBrush">Brush for drawing shadow of border.</param>
        public StiAdvancedBorder(StiBorderSide topSide, StiBorderSide bottomSide, StiBorderSide leftSide, StiBorderSide rightSide,
            bool dropShadow, double shadowSize, StiBrush shadowBrush) : this(topSide, bottomSide, leftSide, rightSide, 
            dropShadow, shadowSize, shadowBrush, false)
        {
        }


		/// <summary>
        /// Creates a new instance of the StiAdvancedBorder class.
		/// </summary>
		/// <param name="topSide">Top side of border.</param>
        /// <param name="bottomSide">Bottom side of border.</param>
        /// <param name="leftSide">Left side of border.</param>
        /// <param name="rightSide">Right side of border.</param>
		/// <param name="dropShadow">Drop shadow or not.</param>
		/// <param name="shadowSize">Shadow siz.</param>
		/// <param name="shadowBrush">Brush for drawing shadow of border.</param>
        /// <param name="topmost">Value which indicates that border sides will be drawn on top of all components.</param>
        public StiAdvancedBorder(StiBorderSide topSide, StiBorderSide bottomSide, StiBorderSide leftSide, StiBorderSide rightSide,
            bool dropShadow, double shadowSize, StiBrush shadowBrush, bool topmost)
		{
			this.topSide = topSide;
            this.bottomSide = bottomSide;
            this.leftSide = leftSide;
            this.rightSide = rightSide;

            this.leftSide.side = StiBorderSides.Left;
            this.rightSide.side = StiBorderSides.Right;
            this.topSide.side = StiBorderSides.Top;
            this.bottomSide.side = StiBorderSides.Bottom;

			this.ShadowBrush = shadowBrush;
			this.ShadowSize = shadowSize;
			this.DropShadow = dropShadow;
            this.Topmost = topmost;
		}
	}
}
