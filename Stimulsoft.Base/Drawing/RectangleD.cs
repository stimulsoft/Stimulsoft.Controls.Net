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
using System.ComponentModel;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Stores a set of four double-point numbers that represent the location and size of a rectangle.
	/// </summary>
	[TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.RectangleDConverter))]
	public struct RectangleD
    {
        #region Properties
        private double x;
		/// <summary>
		/// Gets or sets the x-coordinate of the upper-left corner of this RectangleD structure.
		/// </summary>
		public double X
		{
			get
			{
				return x;
			}
			set
			{
				x = value;
			}
		}


		private double y;
		/// <summary>
		/// Gets or sets the y-coordinate of the upper-left corner of this RectangleD structure.
		/// </summary>
		public double Y
		{
			get
			{
				return y;
			}
			set
			{
				y = value;
			}
		}


		private double width;
		/// <summary>
		/// Gets or sets the width of this RectangleD structure.
		/// </summary>
		public double Width
		{
			get
			{
				return width;
			}
			set
			{
				width = value;
			}
		}
		

		private double height;
		/// <summary>
		/// Gets or sets the height of this RectangleD structure.
		/// </summary>
		public double Height
		{
			get
			{
				return height;
			}
			set
			{
				height = value;
			}
		}


		/// <summary>
		/// Gets or sets the x-coordinate of the upper-left corner of this RectangleD structure.
		/// </summary>
		public double Left
		{
			get
			{
				return x;
			}
		}


		/// <summary>
		/// Gets or sets the y-coordinate of the upper-left corner of this RectangleD structure.
		/// </summary>
		public double Top
		{
			get
			{
				return y;
			}
		}


		/// <summary>
		/// Gets the x-coordinate of the right edge of this RectangleD structure.
		/// </summary>
		public double Right
		{
			get
			{
				return x + width;
			}
		}


		/// <summary>
		/// Gets the y-coordinate of the bottom edge of this RectangleD structure.
		/// </summary>
		public double Bottom
		{
			get
			{
				return y + height;
			}
		}


		/// <summary>
		/// Tests whether all numeric properties of this RectangleD have values of zero.
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return width == 0 && height == 0 && x == 0 && y == 0;
			}
		}


		/// <summary>
		/// Gets or sets the coordinates of the upper-left corner of this RectangleD structure.
		/// </summary>
		public PointD Location
		{
			get
			{
				return new PointD(x, y);
			}
		}


		/// <summary>
		/// Gets or sets the size of this RectangleD.
		/// </summary>
		public SizeD Size
		{
			get
			{
				return new SizeD(width, height);
			}
		}

        #endregion

        #region Fields
        /// <summary>
        /// Represents an instance of the RectangleD class with its members uninitialized.
        /// </summary>
        public static RectangleD Empty = new RectangleD(0, 0, 0, 0);
        #endregion

        #region Methods
        public void Inflate(double width, double height)
		{
			this.X -= width;
			this.Y -= height;
			this.Width += 2 * width;
			this.Height += 2 * height;
		}


		/// <summary>
		/// Converts the specified RectangleD to a Rectangle.
		/// </summary>
		public Rectangle ToRectangle()
		{
			return new Rectangle((int)this.X, (int)this.Y, (int)this.Width, (int)this.Height);
		}


		/// <summary>
		/// Converts the specified RectangleD to a RectangleF.
		/// </summary>
		public RectangleF ToRectangleF()
		{
			return new RectangleF((float)this.X, (float)this.Y, (float)this.Width, (float)this.Height);
		}


		/// <summary>
		/// Normalizes (convert all negative values) rectangle.
		/// </summary>
		/// <returns>Normalized rectangle.</returns>
		public RectangleD Normalize()
		{
			RectangleD rect = this;
			if (rect.Width < 0)
			{
				rect.X += rect.Width;
				rect.Width = - rect.Width;
			}

			if (rect.Height < 0)
			{
				rect.Y += rect.Height;
				rect.Height = - rect.Height;
			}
			return rect;
		}


		/// <summary>
		/// Multiplies the rectangle on number.
		/// </summary>
		/// <param name="multipleFactor">Number.</param>
		/// <returns>Multiplied rectangle.</returns>
		public RectangleD Multiply(double multipleFactor)
		{
			return new RectangleD(
				this.X * multipleFactor, 
				this.Y * multipleFactor, 
				this.Width * multipleFactor, 
				this.Height * multipleFactor);
		}


		/// <summary>
		/// Divides rectangle on number.
		/// </summary>
		/// <param name="divideFactor">Number.</param>
		/// <returns>Divided rectangle.</returns>
		public RectangleD Divide(double divideFactor)
		{
			return new RectangleD(
				this.X / divideFactor, 
				this.Y / divideFactor, 
				this.Width / divideFactor, 
				this.Height / divideFactor);
		}


		/// <summary>
		/// Creates the specified RectangleD from a Rectangle.
		/// </summary>
		public static RectangleD CreateFromRectangle(Rectangle rect)
		{
			return new RectangleD(rect.X, rect.Y, rect.Width, rect.Height);
		}


		/// <summary>
		/// Creates the specified RectangleD from a RectangleF.
		/// </summary>
		public static RectangleD CreateFromRectangle(RectangleF rect)
		{
			return new RectangleD(rect.X, rect.Y, rect.Width, rect.Height);
		}


		/// <summary>
		/// Changes the sizes of the rectangle.
		/// </summary>
		/// <param name="offsettingRectangle">Data for change the size.</param>
		/// <returns>Changed rectangle.</returns>
		public RectangleD OffsetSize(RectangleD offsettingRectangle)
		{
			return new RectangleD(
				x + offsettingRectangle.width,
				y + offsettingRectangle.height,
				width,
				height);
		}


		/// <summary>
		/// Changes the sizes of the rectangle.
		/// </summary>
		/// <param name="offsettingRectangle">Data for change the size.</param>
		/// <returns>Changed rectangle.</returns>
		public RectangleD OffsetRect(RectangleD offsettingRectangle)
		{
			return new RectangleD(
				x - offsettingRectangle.x,
				y - offsettingRectangle.y,
				width + offsettingRectangle.width,
				height + offsettingRectangle.height);
		}


		/// <summary>
		/// Determines if this rectangle intersects with rect.
		/// </summary>
		public bool IntersectsWith(RectangleD rect)
		{
			decimal rectX = Math.Round((decimal)rect.X, 2);
			decimal rectY = Math.Round((decimal)rect.Y, 2);
			decimal rectRight = Math.Round((decimal)rect.Right, 2);
			decimal rectBottom = Math.Round((decimal)rect.Bottom, 2);
			decimal thisX = Math.Round((decimal)this.X, 2);
			decimal thisY = Math.Round((decimal)this.Y, 2);
			decimal thisRight = Math.Round((decimal)this.Right, 2);
			decimal thisBottom = Math.Round((decimal)this.Bottom, 2);
			

			return
				rectX < thisRight && 
				rectY < thisBottom &&
				rectRight > thisX && 
				rectBottom > thisY;
		}

		
		/// <summary>
		/// Align the rectangle to grid.
		/// </summary>
		/// <param name="gridSize">Grid size.</param>
		/// <param name="aligningToGrid">Align or no.</param>
		/// <returns>Aligned rectangle.</returns>
		public RectangleD AlignToGrid(double gridSize, bool aligningToGrid)
		{
			if (aligningToGrid)
				return new RectangleD(
					Math.Round((this.X / gridSize)) * gridSize,
					Math.Round((this.Y / gridSize)) * gridSize,
					Math.Round((this.Width / gridSize)) * gridSize,
					Math.Round((this.Height / gridSize)) * gridSize);
			else return new RectangleD(this.Left, this.Top, this.Width, this.Height);
		}

	
		/// <summary>
		/// Fit rectangle to rectangle.
		/// </summary>
		/// <param name="rectangle">Rectangle, which will be fited.</param>
		/// <returns>Result rectangle.</returns>
		public RectangleD FitToRectangle(RectangleD rectangle)
		{
			if (this.IsEmpty)return rectangle;
			if (rectangle.IsEmpty)return this;

			RectangleD rectangle2 = this;
			if (rectangle2.Left > rectangle.Left)
			{
				rectangle2.Width += rectangle2.Left - rectangle.Left;
				rectangle2.X = rectangle.Left;
			}
			if (rectangle2.Top > rectangle.Top)
			{
				rectangle2.Height += rectangle2.Top - rectangle.Top;
				rectangle2.Y = rectangle.Top;
			}
			if (rectangle2.Right < rectangle.Right)
			{
				rectangle2.Width += rectangle.Right - rectangle2.Right;
			}
			
			if (rectangle2.Bottom < rectangle.Bottom)
			{
				rectangle2.Height += rectangle.Bottom - rectangle2.Bottom;
			}
			
			return rectangle2;
		}


		/// <summary>
		/// Determines if the specified point is contained within this RectangleD structure.
		/// </summary>
		/// <param name="pt">The PointD to test.</param>
		/// <returns>This method returns true if the point represented by the pt parameter is contained within this RectangleD structure; otherwise false.</returns>
		public bool Contains(PointD pt)
		{
			return this.Contains(pt.X, pt.Y); 
		} 


		/// <summary>
		/// Determines if the rectangular region represented by rect is entirely contained within this RectangleD structure.
		/// </summary>
		/// <param name="rect">The RectangleD to test.</param>
		/// <returns>This method returns true if the rectangular region represented by rect is entirely contained within the rectangular region represented by this RectangleD; otherwise false.</returns>
		public bool Contains(RectangleD rect)
		{
			if (((this.X < rect.X) && ((rect.X + rect.Width) < (this.X + this.Width))) && (this.Y < rect.Y))
			{
				return !((rect.Y + rect.Height) > (this.Y + this.Height)); 
			}
			return false; 
		} 


		/// <summary>
		/// Determines if the specified point is contained within this RectangleD structure.
		/// </summary>
		/// <param name="x">The x-coordinate of the point to test.</param>
		/// <param name="y">The y-coordinate of the point to test.</param>
		/// <returns>This method returns true if the point defined by x and y is contained within this RectangleD structure; otherwise false.</returns>
		public bool Contains(double x, double y)
		{
			if (((this.X <= x) && (x < (this.X + this.Width))) && (this.Y <= y))
			{
				return (y < (this.Y + this.Height)); 
			}
			return false; 
		}


		public static RectangleD Intersect(RectangleD a, RectangleD b)
		{
			double num1 = Math.Max(a.X, b.X);
			double num2 = Math.Min(a.X + a.Width, b.X + b.Width);
			double num3 = Math.Max(a.Y, b.Y);
			double num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
			
			if ((num2 >= num1) && (num4 >= num3))
			{
				return new RectangleD(num1, num3, num2 - num1, num4 - num3);
			}
			return RectangleD.Empty;
		}

		public void Intersect(RectangleD rect)
		{
			rect = RectangleD.Intersect(rect, this);
			this.X = rect.X;
			this.Y = rect.Y;
			this.Width = rect.Width;
			this.Height = rect.Height;
		}

		/// <summary>
		/// Tests whether obj is a RectangleF with the same location and size of this RectangleD.
		/// </summary>
		/// <param name="obj">The Object to test.</param>
		/// <returns>This method returns true if obj is a RectangleD and its X, Y, Width, and Height properties are equal to the corresponding properties of this RectangleD; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			RectangleD rect = (RectangleD)obj;
			if (((rect.X == this.X) && (rect.Y == this.Y)) && 
				(rect.Width == this.Width) && (rect.Height == this.Height))return true; 
			return false;
		}


		/// <summary>
		/// Returns a hash code for this RectangleD structure.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this RectangleD structure.</returns>
		public override int GetHashCode()
		{
			return (((((int) this.X) ^ ((((int) this.Y) << 13) | 
				(((int) this.Y) >> 19))) ^ ((((int) this.Width) << 26) | 
				(((int) this.Width) >> 6))) ^ 
				((((int) this.Height) << 7) | (((int) this.Height) >> 25))); 
		}

        public override string ToString()
        {
            return string.Format("X={0} Y={1} W={2} H={3}", (decimal)this.x, (decimal)this.y, (decimal)this.width, (decimal)this.height);
        }
        #endregion

        #region Methods.json

        public void LoadFromJson(JObject jObject)
        {
            foreach (var property in jObject.Properties())
            {
                switch (property.Name)
                {
                    case "X":
                        this.x = property.Value.ToObject<double>();
                        break;

                    case "Y":
                        this.y = property.Value.ToObject<double>();
                        break;

                    case "Width":
                        this.width = property.Value.ToObject<double>();
                        break;

                    case "Height":
                        this.height = property.Value.ToObject<double>();
                        break;
                }
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the RectangleD class with the specified location and size.
        /// </summary>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public RectangleD(double x, double y, double width, double height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
 		} 


		/// <summary>
		/// Initializes a new instance of the RectangleD class with the specified location and size.
		/// </summary>
		/// <param name="location">A PointD that represents the upper-left corner of the rectangular region.</param>
		/// <param name="size">A SizeD that represents the width and height of the rectangular region.</param>
		public RectangleD(PointD location, SizeD size)
		{
			this.x = location.X;
			this.y = location.Y;
			this.width = size.Width;
			this.height = size.Height;
		} 

	}
}
