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
using System.Linq;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Class describes a brush.
	/// </summary>
	[TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.StiBrushConverter))]
	[RefreshProperties(RefreshProperties.All)]
	[Editor("Stimulsoft.Base.Drawing.Design.StiBrushEditor, Stimulsoft.Report.Design, " + StiVersion.VersionInfo, typeof(UITypeEditor))]
	[StiReferenceIgnore]
	public abstract class StiBrush : 
		ICloneable
    {
        #region ICloneable
        /// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public virtual object Clone()
		{
			return this.MemberwiseClone();
		}
		#endregion

        #region IEquatable
	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        //if (obj.GetType() != this.GetType()) return false;
	        return false;
	    }

	    public override int GetHashCode()
	    {
	        return base.GetHashCode();
	    }
	    #endregion

        #region Methods
        public static StiBrush Light(StiBrush baseBrush, byte value)
		{
			if (baseBrush is StiSolidBrush)
			{
				return new StiSolidBrush(StiColorUtils.Light(((StiSolidBrush)baseBrush).Color, value));
			}
			else if (baseBrush is StiGradientBrush)
			{
				StiGradientBrush gradientBrush = baseBrush as StiGradientBrush;
				return new StiGradientBrush(
					StiColorUtils.Light(gradientBrush.StartColor, value), 
					StiColorUtils.Light(gradientBrush.EndColor, value), 
					gradientBrush.Angle);
				
			}
			else if (baseBrush is StiHatchBrush)
			{
				StiHatchBrush hatchBrush = baseBrush as StiHatchBrush;
				return new StiHatchBrush(
					hatchBrush.Style,
					StiColorUtils.Light(hatchBrush.ForeColor, value), 
					StiColorUtils.Light(hatchBrush.BackColor, value));
			}
			else if (baseBrush is StiGlareBrush)
			{
				StiGlareBrush glareBrush = baseBrush as StiGlareBrush;
				return new StiGlareBrush(
					StiColorUtils.Light(glareBrush.StartColor, value), 
					StiColorUtils.Light(glareBrush.EndColor, value), 
					glareBrush.Angle);				
			}
            else if (baseBrush is StiGlassBrush)
            {
                StiGlassBrush glassBrush = baseBrush as StiGlassBrush;
                return new StiGlassBrush(
                    StiColorUtils.Light(glassBrush.Color, value),
                    glassBrush.DrawHatch,
                    glassBrush.Blend);
            }
            
			return baseBrush;
		}


		public static StiBrush Dark(StiBrush baseBrush, byte value)
		{
			if (baseBrush is StiSolidBrush)
			{
				return new StiSolidBrush(StiColorUtils.Dark(((StiSolidBrush)baseBrush).Color, value));
			}
			else if (baseBrush is StiGradientBrush)
			{
				StiGradientBrush gradientBrush = baseBrush as StiGradientBrush;
				return new StiGradientBrush(
					StiColorUtils.Dark(gradientBrush.StartColor, value), 
					StiColorUtils.Dark(gradientBrush.EndColor, value), 
					gradientBrush.Angle);
				
			}
			else if (baseBrush is StiHatchBrush)
			{
				StiHatchBrush hatchBrush = baseBrush as StiHatchBrush;
				return new StiHatchBrush(
					hatchBrush.Style,
					StiColorUtils.Dark(hatchBrush.ForeColor, value), 
					StiColorUtils.Dark(hatchBrush.BackColor, value));
			}
			else if (baseBrush is StiGlareBrush)
			{
				StiGlareBrush glareBrush = baseBrush as StiGlareBrush;
				return new StiGlareBrush(
					StiColorUtils.Dark(glareBrush.StartColor, value), 
					StiColorUtils.Dark(glareBrush.EndColor, value), 
					glareBrush.Angle);				
			}
            else if (baseBrush is StiGlassBrush)
            {
                StiGlassBrush glassBrush = baseBrush as StiGlassBrush;
                return new StiGlassBrush(
                    StiColorUtils.Dark(glassBrush.Color, value),
                    glassBrush.DrawHatch,
                    glassBrush.Blend);
            }
			return baseBrush;
		}

        /// <summary>
        /// Returns the gdi brush from the report brush.
        /// </summary>
        /// <param name="brush">Report brush.</param>
        /// <param name="rect">Rectangle for gradient.</param>
        /// <returns>Gdi brush.</returns>
        public static Brush GetBrush(StiBrush brush, Rectangle rect)
        {
            return GetBrush(brush, RectangleD.CreateFromRectangle(rect));
        }

		/// <summary>
		/// Returns the gdi brush from the report brush.
		/// </summary>
		/// <param name="brush">Report brush.</param>
		/// <param name="rect">Rectangle for gradient.</param>
		/// <returns>Gdi brush.</returns>
		public static Brush GetBrush(StiBrush brush, RectangleF rect)
		{
			return GetBrush(brush, RectangleD.CreateFromRectangle(rect));
		}


		/// <summary>
		/// Returns the standard brush from the report brush.
		/// </summary>
		/// <param name="brush">Report brush.</param>
		/// <param name="rect">Rectangle for gradient.</param>
		/// <returns>Gdi brush.</returns>
		public static Brush GetBrush(StiBrush brush, RectangleD rect)
		{
			if (brush is StiEmptyBrush)
			{
				return new SolidBrush(Color.Transparent);
			}
			else if (brush is StiSolidBrush)
            {
                return new SolidBrush(((StiSolidBrush)brush).Color);
            }
            else if (brush is StiGradientBrush)
            {
				RectangleF rectF = rect.ToRectangleF();
				if (rectF.Width < 1)rectF.Width = 1;
				if (rectF.Height < 1)rectF.Height = 1;

				StiGradientBrush gradientBrush = brush as StiGradientBrush;
                return new LinearGradientBrush(rectF,
                    gradientBrush.StartColor, gradientBrush.EndColor, (float)gradientBrush.Angle);
				
            }
            else if (brush is StiHatchBrush)
            {
                StiHatchBrush hatchBrush = brush as StiHatchBrush;
                return new HatchBrush(hatchBrush.Style,
                    hatchBrush.ForeColor, hatchBrush.BackColor);
            }
			else if (brush is StiGlareBrush)
			{
				RectangleF rectF = rect.ToRectangleF();
				if (rectF.Width < 1)rectF.Width = 1;
				if (rectF.Height < 1)rectF.Height = 1;

				StiGlareBrush glareBrush = brush as StiGlareBrush;
				LinearGradientBrush br = new LinearGradientBrush(rectF,
					glareBrush.StartColor, glareBrush.EndColor, (float)glareBrush.Angle);
				br.SetSigmaBellShape(glareBrush.Focus, glareBrush.Scale);
				return br;				
			}
            else if (brush is StiGlassBrush)
            {
                Bitmap bmp = new Bitmap((int)rect.Width + 1, (int)rect.Height + 1);
                using (Graphics gg = Graphics.FromImage(bmp))
                {
                    ((StiGlassBrush)brush).Draw(gg, new RectangleF(0, 0, (float)rect.Width + 1, (float)rect.Height + 1));
                }

                TextureBrush textureBrush = new TextureBrush(bmp);
                textureBrush.TranslateTransform((float)rect.X, (float)rect.Y);
                return textureBrush;
            }
            return null;
		}

        public static StiBrush LoadFromJson(JObject jObject)
        {
            var ident = jObject.Properties().FirstOrDefault(x => x.Name == "Ident");

            switch (ident.Value.ToObject<string>())
            {
                case "StiEmptyBrush":
                    return new StiEmptyBrush();

                case "StiSolidBrush":
                    var solid = new StiSolidBrush();
                    solid.LoadValuesFromJson(jObject);
                    return solid;

                case "StiGradientBrush":
                    var gradient = new StiGradientBrush();
                    gradient.LoadValuesFromJson(jObject);
                    return gradient;

                case "StiGlareBrush":
                    var glare = new StiGlareBrush();
                    glare.LoadValuesFromJson(jObject);
                    return glare;

                case "StiGlassBrush":
                    var glass = new StiGlassBrush();
                    glass.LoadValuesFromJson(jObject);
                    return glass;

                case "StiHatchBrush":
                    var hatch = new StiHatchBrush();
                    hatch.LoadValuesFromJson(jObject);
                    return hatch;
            }

            throw new Exception("Type is not supported!");
        }

		
		/// <summary>
		/// Transform a brush into a color.
		/// </summary>
		/// <param name="brush">Brush for converting.</param>
		/// <returns>Converted color.</returns>
		public static Color ToColor(StiBrush brush)
		{
			if (brush is StiEmptyBrush)return Color.Transparent;
			if (brush is StiSolidBrush)return ((StiSolidBrush)brush).Color;
			if (brush is StiGradientBrush)return ((StiGradientBrush)brush).StartColor;
			if (brush is StiGlareBrush)return ((StiGlareBrush)brush).StartColor;
            if (brush is StiGlassBrush) return ((StiGlassBrush)brush).Color;
			if (brush is StiHatchBrush) return ((StiHatchBrush)brush).ForeColor;
			return Color.Empty;
		}
		#endregion
    }
}