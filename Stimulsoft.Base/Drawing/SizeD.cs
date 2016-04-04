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

using System.Drawing;
using System.ComponentModel;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using System;

namespace Stimulsoft.Base.Drawing
{

	/// <summary>
	/// Stores an ordered pair of floating-point numbers, typically the width and height of a rectangle.
	/// </summary>
	[TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.SizeDConverter))]
	public struct SizeD : 
        IStiDefault
    {
        #region IStiDefault
        [Browsable(false)]
		public bool IsDefault
		{
			get
			{
				return Width == 0 && Height == 0;
			}
		}
        #endregion

        #region Properties
        private double width;
		/// <summary>
		/// Gets or sets the horizontal component of this SizeD.
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
		/// Gets or sets the vertical component of this SizeD.
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
        /// Gets a value indicating whether this SizeD has zero width and height.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return width == 0 && height == 0;
            }
        }
        #endregion

        #region Fields
        /// <summary>
        /// Initializes a new instance of the SizeD class.
        /// </summary>
        public static SizeD Empty = new SizeD(0, 0);
        #endregion

        #region Methods.override
        /// <summary>
        /// Tests to see whether the specified object is a SizeD with the same dimensions as this SizeD.
        /// </summary>
        /// <param name="obj">The Object to test.</param>
        /// <returns>This method returns true if obj is a SizeD and has the same width and height as this SizeD; otherwise, false.</returns>
        public override bool Equals(object obj)
		{
			var size = (SizeD)obj;
			if ((size.Width == this.Width) && (size.Height == this.Height))return true;
			return false;
		}


		/// <summary>
		/// Returns a hash code for this SizeD structure.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this SizeD structure.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode(); 
		}

        public override string ToString()
		{
			return string.Format("{{Width={0}, Height={1}}}", this.width, this.height); 
		}

		public SizeF ToSizeF()
		{
			return new SizeF((float)width, (float)height);
		}
        #endregion

        /// <summary>
        /// Initializes a new instance of the SizeD class from the specified dimensions.
        /// </summary>
        /// <param name="width">The width component of the new SizeD.</param>
        /// <param name="height">The height component of the new SizeD.</param>
        public SizeD(double width, double height)
		{
			this.width = width;
			this.height = height;
		}
	}
}
