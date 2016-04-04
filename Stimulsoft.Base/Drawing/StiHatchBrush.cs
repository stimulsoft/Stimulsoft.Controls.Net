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
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Class describes the HatchBrush.
	/// </summary>
	[RefreshProperties(RefreshProperties.All)]
	public class StiHatchBrush : StiBrush
	{
		#region Properties
		private Color backColor;
		/// <summary>
		/// Gets the color of spaces between the hatch lines drawn by this StiHatchBrush object.
		/// </summary>
		[StiSerializable]
		[TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.StiColorConverter))]
		[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, " + StiVersion.VersionInfo, typeof(UITypeEditor))]
		[Description("Gets the color of spaces between the hatch lines drawn by this StiHatchBrush object.")]
		[RefreshProperties(RefreshProperties.All)]
		[StiOrder(200)]
		public Color BackColor
		{
			get
			{
				return backColor;
			}
			set
			{
				backColor = value;
			}
		}

		
		private Color foreColor;
		/// <summary>
		/// Gets the color of hatch lines drawn by this StiHatchBrush object.
		/// </summary>
		[StiSerializable]
		[TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.StiColorConverter))]
		[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, " + StiVersion.VersionInfo, typeof(UITypeEditor))]
		[Description("Gets the color of hatch lines drawn by this StiHatchBrush object.")]
		[RefreshProperties(RefreshProperties.All)]
		[StiOrder(100)]
		public Color ForeColor
		{
			get
			{
				return foreColor;
			}
			set
			{
				foreColor = value;
			}
		}

		
		private HatchStyle style;
		/// <summary>
		/// Gets the hatch style of this StiHatchBrush object.
		/// </summary>
		[Editor("Stimulsoft.Base.Drawing.Design.StiHatchStyleEditor, Stimulsoft.Report.Design, " + StiVersion.VersionInfo, typeof(UITypeEditor))]
		[StiSerializable]
		[TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.StiHatchStyleConverter))]
		[Description("Gets the hatch style of this StiHatchBrush object.")]
		[RefreshProperties(RefreshProperties.All)]
		[StiOrder(300)]
		public HatchStyle Style
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
		#endregion

        #region IEquatable
        protected bool Equals(StiHatchBrush other)
	    {
	        return backColor.Equals(other.backColor) && foreColor.Equals(other.foreColor) && style == other.style;
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        if (obj.GetType() != this.GetType()) return false;
	        return Equals((StiHatchBrush) obj);
	    }

	    public override int GetHashCode()
	    {
	        unchecked
	        {
                int hashCode = defaultHashCode;
	            hashCode = (hashCode*397) ^ backColor.GetHashCode();
	            hashCode = (hashCode*397) ^ foreColor.GetHashCode();
	            hashCode = (hashCode*397) ^ (int) style;
	            return hashCode;
	        }
	    }
        private static int defaultHashCode = "StiHatchBrush".GetHashCode();
	    #endregion

		#region Methods
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        //public override bool Equals(Object obj) 
        //{
        //    StiHatchBrush brush = obj as StiHatchBrush;

        //    if (brush == null)return false;

        //    if (this.backColor != brush.backColor)return false;
        //    if (this.foreColor != brush.foreColor)return false;
        //    if (!this.style.Equals(brush.style))return false;

        //    return true;
        //}

		private bool ShouldSerializeBackColor()
		{
			return backColor != Color.White;
		}

		private bool ShouldSerializeForeColor()
		{
			return foreColor != Color.Black;
		}

		private bool ShouldSerializeStyle()
		{
			return style != HatchStyle.BackwardDiagonal;
		}

        public void LoadValuesFromJson(JObject jObject)
        {
            foreach (var property in jObject.Properties())
            {
                switch (property.Name)
                {
                    case "BackColor":
                        this.backColor = StiJsonReportObjectHelper.Deserialize.Color(property.Value.ToObject<string>());
                        break;

                    case "ForeColor":
                        this.foreColor = StiJsonReportObjectHelper.Deserialize.Color(property.Value.ToObject<string>());
                        break;

                    case "Style":
                        this.style = (HatchStyle)Enum.Parse(typeof(HatchStyle), property.Value.ToObject<string>());
                        break;
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of the StiHatchBrush class.
        /// </summary>
        public StiHatchBrush()
		{
			style = HatchStyle.BackwardDiagonal;
			backColor = Color.Black;
			foreColor = Color.White;
		}


		/// <summary>
		/// Creates a new instance of the StiHatchBrush class.
		/// </summary>
		/// <param name="style">Hatch style of this StiHatchBrush object.</param>
		/// <param name="foreColor">The color of hatch lines drawn by this StiHatchBrush object.</param>
		/// <param name="backColor">The color of spaces between the hatch lines drawn by this StiHatchBrush object.</param>
		public StiHatchBrush(HatchStyle style, Color foreColor, Color backColor)
		{
			this.style = style;
			this.foreColor = foreColor;
			this.backColor = backColor;
		}
		#endregion
	}
}
