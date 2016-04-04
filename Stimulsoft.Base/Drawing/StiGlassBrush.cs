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
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Class describes GlassBrush.
	/// </summary>
	[RefreshProperties(RefreshProperties.All)]
	public class StiGlassBrush : StiBrush
	{
		#region Properties
		private Color color = Color.Silver;
		/// <summary>
        /// Gets or sets the color of this StiGlassBrush object.
		/// </summary>
		[StiSerializable]
		[TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.StiColorConverter))]
		[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, " + StiVersion.VersionInfo, typeof(UITypeEditor))]
		[Description("Gets or sets the color of this StiGlassBrush object.")]
		[RefreshProperties(RefreshProperties.All)]
		public Color Color
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


        private bool drawHatch = true;
        /// <summary>
        /// Gets or sets value which indicates draw hatch at background or not.
        /// </summary>
        [StiSerializable]
        [DefaultValue(true)]
        [TypeConverter(typeof(StiBoolConverter))]
        [Description("Gets or sets value which indicates draw hatch at background or not.")]
        [RefreshProperties(RefreshProperties.All)]
        [StiOrder(190)]
        public virtual bool DrawHatch
        {
            get
            {
                return drawHatch;
            }
            set
            {
                drawHatch = value;
            }
        }


        private float blend = 0.2f;
        /// <summary>
        /// Gets or sets blend factor.
        /// </summary>
        [StiSerializable]
        [DefaultValue(0.2f)]
        [Description("Gets or sets blend factor.")]
        [RefreshProperties(RefreshProperties.All)]
        [StiOrder(190)]
        public virtual float Blend
        {
            get
            {
                return blend;
            }
            set
            {
                if (blend != value)
                {
                    if (value >= 0 && value <= 1)
                        blend = value;
                    else throw new ArgumentOutOfRangeException("Value must be in range between 0 and 1.");
                }
            }
        }
		#endregion

        #region IEquatable
	    protected bool Equals(StiGlassBrush other)
	    {
	        return color.Equals(other.color) && drawHatch.Equals(other.drawHatch) && blend.Equals(other.blend);
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        if (obj.GetType() != this.GetType()) return false;
	        return Equals((StiGlassBrush) obj);
	    }

	    public override int GetHashCode()
	    {
	        unchecked
	        {
                int hashCode = defaultHashCode;
	            hashCode = (hashCode*397) ^ color.GetHashCode();
	            hashCode = (hashCode*397) ^ drawHatch.GetHashCode();
	            hashCode = (hashCode*397) ^ blend.GetHashCode();
	            return hashCode;
	        }
	    }
        private static int defaultHashCode = "StiGlassBrush".GetHashCode();
	    #endregion

		#region Methods
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        //public override bool Equals(Object obj) 
        //{
        //    StiGlassBrush brush = obj as StiGlassBrush;

        //    if (brush == null) return false;

        //    if (this.color != brush.color) return false;
        //    if (this.drawHatch != brush.drawHatch) return false;
        //    if (this.blend != brush.blend) return false;

        //    return true;
        //}
        
        public Color GetTopColor()
        {
            return StiColorUtils.Light(this.Color, (byte)((float)64 * this.Blend));
        }

        public Color GetTopColorLight()
        {
            return StiColorUtils.Light(StiColorUtils.Light(this.Color, (byte)((float)64 * this.Blend)), 5);
        }

        public Color GetBottomColor()
        {
            return this.Color;
        }

        public Color GetBottomColorLight()
        {
            return StiColorUtils.Light(GetBottomColor(), 2);
        }

        public Brush GetTopBrush()
        {
            if (this.DrawHatch)
                return new HatchBrush(HatchStyle.DarkDownwardDiagonal, GetTopColor(), GetTopColorLight());
            else
                return new SolidBrush(GetTopColor());
        }

        public Brush GetBottomBrush()
        {
            if (this.DrawHatch)
                return new HatchBrush(HatchStyle.DarkDownwardDiagonal, GetBottomColor(), GetBottomColorLight());
            else
                return new SolidBrush(GetBottomColor());
        }

        public RectangleF GetTopRectangle(RectangleF rect)
        {
            RectangleF rect1 = rect;

            rect1.Height /= 2;

            if (rect1.Height * 2 < rect.Height) rect1.Height++;

            return rect1;
        }

        public RectangleF GetBottomRectangle(RectangleF rect)
        {
            RectangleF rect1 = GetTopRectangle(rect);
            RectangleF rect2 = rect;

            rect2.Height = rect.Height - rect1.Height;

            rect2.Y = rect1.Bottom;

            return rect2;
        }

        public void Draw(Graphics g, RectangleF rect)
        {
            RectangleF rect1 = GetTopRectangle(rect);
            RectangleF rect2 = GetBottomRectangle(rect);

            using (Brush brush1 = GetTopBrush())
            using (Brush brush2 = GetBottomBrush())
            {
                g.FillRectangle(brush1, rect1);
                g.FillRectangle(brush2, rect2);
            }
        }

        public void LoadValuesFromJson(JObject jObject)
        {
            foreach (var property in jObject.Properties())
            {
                switch (property.Name)
                {
                    case "Color":
                        this.Color = StiJsonReportObjectHelper.Deserialize.Color(property.Value.ToObject<string>());
                        break;

                    case "DrawHatch":
                        this.drawHatch = property.Value.ToObject<bool>();
                        break;

                    case "Blend":
                        this.blend = property.Value.ToObject<float>();
                        break;
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of the StiGlassBrush class.
        /// </summary>
        public StiGlassBrush()
		{
			this.color = Color.Silver;
            this.drawHatch = true;
            this.blend = 0.2f;

		}

		/// <summary>
        /// Creates a new instance of the StiGlassBrush class.
		/// </summary>
        /// <param name="color">The color of this StiGlassBrush object.</param>
        public StiGlassBrush(Color color, bool drawHatch, float blend)
		{
			this.color = color;
            this.drawHatch = drawHatch;
            this.blend = blend;
		}
		#endregion
	}
}
