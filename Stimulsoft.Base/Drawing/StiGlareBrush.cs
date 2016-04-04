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
	/// Class describes a glare gradient brush.
	/// </summary>
	[RefreshProperties(RefreshProperties.All)]
	public class StiGlareBrush : StiBrush
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
		[Description("Gets or sets the angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line.")]
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


		private float focus = 0.5f;
		/// <summary>
		/// Gets or sets value from 0 through 1 that specifies the center of the gradient (the point where the gradient is composed of only the ending color).
		/// </summary>
		[StiSerializable]
		[DefaultValue(0.5f)]
		[Description("Gets or sets value from 0 through 1 that specifies the center of the gradient (the point where the gradient is composed of only the ending color).")]
		[RefreshProperties(RefreshProperties.All)]
		[StiOrder(400)]
		public float Focus
		{
			get
			{
				return focus;
			}
			set
			{
				if (focus != value)
				{
					if (value > 1f || value < 0)
						throw new ArgumentException("Focus must be in range between 0 and 1!");
					
					focus = value;
				}
			}
		}


		private float scale = 1f;
		/// <summary>
		/// Gets or sets value from 0 through 1 that specifies how fast the colors falloff from the focus. 
		/// </summary>
		[StiSerializable]
		[Description("Gets or sets value from 0 through 1 that specifies how fast the colors falloff from the focus.")]
		[RefreshProperties(RefreshProperties.All)]
		[DefaultValue(1f)]
		[StiOrder(500)]
        [StiGuiMode(StiGuiMode.Gdi)]
		public float Scale
		{
			get
			{
				return scale;
			}
			set
			{
				if (scale != value)
				{
					if (value > 1f || value < 0)
						throw new ArgumentException("Scale must be in range between 0 and 1!");
					scale = value;
				}
			}
		}
		#endregion

        #region IEquatable
	    protected bool Equals(StiGlareBrush other)
	    {
	        return startColor.Equals(other.startColor) && endColor.Equals(other.endColor) && angle.Equals(other.angle) && focus.Equals(other.focus) && scale.Equals(other.scale);
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        if (obj.GetType() != this.GetType()) return false;
	        return Equals((StiGlareBrush) obj);
	    }

	    public override int GetHashCode()
	    {
	        unchecked
	        {
                int hashCode = defaultHashCode;
	            hashCode = (hashCode*397) ^ startColor.GetHashCode();
	            hashCode = (hashCode*397) ^ endColor.GetHashCode();
	            hashCode = (hashCode*397) ^ angle.GetHashCode();
	            hashCode = (hashCode*397) ^ focus.GetHashCode();
	            hashCode = (hashCode*397) ^ scale.GetHashCode();
	            return hashCode;
	        }
	    }
        private static int defaultHashCode = "StiGlareBrush".GetHashCode();
	    #endregion

		#region Methods
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        //public override bool Equals(Object obj) 
        //{
        //    StiGlareBrush brush = obj as StiGlareBrush;
        //    if (brush == null)return false;

        //    if (this.angle != brush.angle)return false;
        //    if (this.endColor != brush.endColor)return false;
        //    if (this.startColor != brush.startColor)return false;
        //    if (this.focus != brush.focus)return false;
        //    if (this.scale != brush.scale)return false;

        //    return true;
        //}

		private bool ShouldSerializeStartColor()
		{
			return startColor != Color.Black;
		}

		private bool ShouldSerializeEndColor()
		{
			return endColor != Color.White;
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

                    case "Focus":
                        this.Focus = property.Value.ToObject<float>();
                        break;

                    case "Scale":
                        this.Scale = property.Value.ToObject<float>();
                        break;
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of the StiGlareBrush class.
        /// </summary>
        public StiGlareBrush() : this(Color.Black, Color.White, 0)
		{
		}


		/// <summary>
		/// Creates a new instance of the StiGlareBrush class.
		/// </summary>
		/// <param name="startColor">the starting color for the gradient.</param>
		/// <param name="endColor">The ending color for the gradient.</param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line.</param>
		public StiGlareBrush(Color startColor, Color endColor, double angle) : this(startColor, endColor, angle, 0.5f, 1f)
		{
		}

		/// <summary>
		/// Creates a new instance of the StiGlareBrush class.
		/// </summary>
		/// <param name="startColor">the starting color for the gradient.</param>
		/// <param name="endColor">The ending color for the gradient.</param>
		/// <param name="angle">The angle, measured in degrees clockwise from the x-axis, of the gradient's orientation line.</param>
		/// <param name="focus">The value from 0 through 1 that specifies the center of the gradient (the point where the gradient is composed of only the ending color).</param>
		/// <param name="scale">The value from 0 through 1 that specifies how fast the colors falloff from the focus. </param>
		public StiGlareBrush(Color startColor, Color endColor, double angle, float focus, float scale)
		{
			this.startColor = startColor;
			this.endColor = endColor;
			this.angle = angle;

			this.focus = focus;
			this.scale = scale;
		}
		#endregion
	}
}
