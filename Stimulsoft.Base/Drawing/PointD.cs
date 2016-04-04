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

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Represents an ordered pair of double x- and y-coordinates that defines a point in a two-dimensional plane.
	/// </summary>
	[TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.PointDConverter))]
	public struct PointD
	{
		private double x;
		/// <summary>
		/// Gets or sets the x-coordinate of this PointD.
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
		/// Gets or sets the y-coordinate of this PointD.
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

		/// <summary>
		/// Gets a value indicating whether this PointD is empty.
		/// </summary>
		[Browsable(false)]
		public bool IsEmpty
		{
			get
			{
				return this.x == 0 && this.y == 0;
			}
		}

		/// <summary>
		/// Represents a null PointD.
		/// </summary>
		public static PointD Empty = new PointD();

		/// <summary>
		/// Converts this PointD to a human readable string.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("{{X={0}, Y={1}}}", this.x, this.y); 
		}

		/// <summary>
		/// Specifies whether this PointD contains the same coordinates as the specified Object.
		/// </summary>
		/// <param name="obj">The Object to test.</param>
		/// <returns>This method returns true if obj is a PointD and has the same coordinates as this Point.</returns>
		public override bool Equals(object obj)
		{
			PointD point = (PointD)obj;
			if ((point.X == this.X) && (point.Y == this.Y))return true;
			return false;
		}

		/// <summary>
		/// Returns a hash code for this PointD structure.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this PointD structure.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode(); 
		}

		public PointF ToPointF()
		{
			return new PointF((float)x, (float)y);
		}


		public PointF ToPoint()
		{
			return new Point((int)x, (int)y);
		}

		/// <summary>
		/// Initializes a new instance of the PointD class with the specified coordinates.
		/// </summary>
		/// <param name="x">The horizontal position of the point.</param>
		/// <param name="y">The vertical position of the point.</param>
		public PointD(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

	}
}
