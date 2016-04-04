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
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Class describes a gradient brush.
	/// </summary>
	[RefreshProperties(RefreshProperties.All)]
	public class StiGradientBrush : StiBrush
	{
		#region Properties
		private Color startColor;
		/// <summary>
		/// Gets or sets the starting color for the gradient.
		/// </summary>
		[StiSerializable]
		[TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.StiColorConverter))]
		[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, " + StiVersion.VersionInfo, typeof(UITypeEditor))]
		[Description("Gets or sets the starting color for the gradient.")]
		[RefreshProperties(RefreshProperties.All)]
		[StiOrder(200)]
		public Color StartColor
		{
			get
			{
				return startColor;
			}
			set
			{
				startColor = value;
			}
		}


		private Color endColor;
		/// <summary>
		/// Gets or sets the ending color for the gradient.
		/// </summary>
		[StiSerializable]
		[TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.StiColorConverter))]
		[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, " + StiVersion.VersionInfo, typeof(UITypeEditor))]
		[Description("Gets or sets the ending color for the gradient.")]
		[RefreshProperties(RefreshProperties.All)]
		[StiOrder(300)]
		public Color EndColor
		{
			get
			{
				return endColor;
			}
			set
			{
				endColor = value;
			}
		}


		private double angle;
		/// <summary>
		/// Gets or sets the angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line. 
		/// </summary>
		[StiSerializable]
		[Description("Gets or sets the angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line. ")]
		[RefreshProperties(RefreshProperties.All)]
		[StiOrder(100)]
		public double Angle
		{
			get
			{
				return angle;
			}
			set
			{
				angle = value;
			}
		}

		#endregion

        #region IEquatable
	    protected bool Equals(StiGradientBrush other)
	    {
	        return startColor.Equals(other.startColor) && endColor.Equals(other.endColor) && angle.Equals(other.angle);
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        if (obj.GetType() != this.GetType()) return false;
	        return Equals((StiGradientBrush) obj);
	    }

	    public override int GetHashCode()
	    {
	        unchecked
	        {
                int hashCode = defaultHashCode;
	            hashCode = (hashCode*397) ^ startColor.GetHashCode();
	            hashCode = (hashCode*397) ^ endColor.GetHashCode();
	            hashCode = (hashCode*397) ^ angle.GetHashCode();
	            return hashCode;
	        }
	    }
        private static int defaultHashCode = "StiGradientBrush".GetHashCode();
	    #endregion

		#region Methods
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        //public override bool Equals(Object obj) 
        //{
        //    StiGradientBrush brush = obj as StiGradientBrush;
        //    if (brush == null)return false;

        //    if (this.angle != brush.angle)return false;
        //    if (this.endColor != brush.endColor)return false;
        //    if (this.startColor != brush.startColor)return false;

        //    return true;
        //}

		private bool ShouldSerializeStartColor()
		{
			return startColor != Color.White;
		}

		private bool ShouldSerializeEndColor()
		{
			return endColor != Color.Black;
		}
        
		private bool ShouldSerializeAngle()
		{
			return angle != 0;
		}

        public void LoadValuesFromJson(JObject jObject)
        {
            foreach (var property in jObject.Properties())
            {
                switch (property.Name)
                {
                    case "StartColor":
                        this.StartColor = StiJsonReportObjectHelper.Deserialize.Color(property.Value.ToObject<string>());
                        break;

                    case "EndColor":
                        this.EndColor = StiJsonReportObjectHelper.Deserialize.Color(property.Value.ToObject<string>());
                        break;

                    case "Angle":
                        this.Angle = property.Value.ToObject<double>();
                        break;
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of the StiGradientBrush class.
        /// </summary>
        public StiGradientBrush()
		{
			this.startColor = Color.Black;
			this.endColor = Color.White;
			this.angle = 0;
		}

		/// <summary>
		/// Creates a new instance of the StiGradientBrush class.
		/// </summary>
		/// <param name="startColor">the starting color for the gradient.</param>
		/// <param name="endColor">The ending color for the gradient.</param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line.</param>
		public StiGradientBrush(Color startColor, Color endColor, double angle)
		{
			this.startColor = startColor;
			this.endColor = endColor;
			this.angle = angle;
		}
		#endregion
	}
}
