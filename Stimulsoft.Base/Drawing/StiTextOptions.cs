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
using System.Reflection;
using System.Drawing;
using System.Drawing.Text;
using System.ComponentModel;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Class describes all text option.
	/// </summary>
	[TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.StiTextOptionsConverter))]
	[RefreshProperties(RefreshProperties.All)]
	public sealed class StiTextOptions : 
        ICloneable,
        IStiJsonReportObject
    {
        #region IStiJsonReportObject.override
        public JObject SaveToJsonObject(StiJsonSaveMode mode)
        {
            var jObject = new JObject();

            // bits
            jObject.AddPropertyBool("RightToLeft", RightToLeft);
            jObject.AddPropertyBool("LineLimit", LineLimit);
            jObject.AddPropertyFloat("Angle", Angle, 0f);
            jObject.AddPropertyFloat("FirstTabOffset", FirstTabOffset, 40f);
            jObject.AddPropertyFloat("DistanceBetweenTabs", DistanceBetweenTabs, 20f);
            jObject.AddPropertyEnum("HotkeyPrefix", HotkeyPrefix, HotkeyPrefix.None);
            jObject.AddPropertyEnum("Trimming", Trimming, StringTrimming.None);
            jObject.AddPropertyBool("WordWrap", WordWrap);

            if (jObject.Count > 0)
                return jObject;

            return null;
        }

        public void LoadFromJsonObject(JObject jObject)
        {
            foreach (var property in jObject.Properties())
            {
                switch (property.Name)
                {
                    case "RightToLeft":
                        this.RightToLeft = property.Value.ToObject<bool>();
                        break;

                    case "LineLimit":
                        this.LineLimit = property.Value.ToObject<bool>();
                        break;

                    case "Angle":
                        this.Angle = property.Value.ToObject<float>();
                        break;

                    case "FirstTabOffset":
                        this.FirstTabOffset = property.Value.ToObject<float>();
                        break;

                    case "DistanceBetweenTabs":
                        this.DistanceBetweenTabs = property.Value.ToObject<float>();
                        break;

                    case "HotkeyPrefix":
                        this.HotkeyPrefix = (HotkeyPrefix)Enum.Parse(typeof(HotkeyPrefix), property.Value.ToObject<string>());
                        break;

                    case "Trimming":
                        this.Trimming = (StringTrimming)Enum.Parse(typeof(StringTrimming), property.Value.ToObject<string>());
                        break;

                    case "WordWrap":
                        this.WordWrap = property.Value.ToObject<bool>();
                        break;
                }
            }
        }
        #endregion

        #region bitsTextOptions
        private class bitsTextOptions : ICloneable
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
            protected bool Equals(bitsTextOptions other)
            {
                return RightToLeft == other.RightToLeft && LineLimit == other.LineLimit && Angle.Equals(other.Angle) && FirstTabOffset.Equals(other.FirstTabOffset) &&
                    DistanceBetweenTabs.Equals(other.DistanceBetweenTabs) && HotkeyPrefix == other.HotkeyPrefix && Trimming == other.Trimming;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((bitsTextOptions) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = RightToLeft ? 1231 : 1237;
                    hashCode = (hashCode * 397) ^ (LineLimit ? 1231 : 1237);
                    hashCode = (hashCode*397) ^ Angle.GetHashCode();
                    hashCode = (hashCode*397) ^ FirstTabOffset.GetHashCode();
                    hashCode = (hashCode*397) ^ DistanceBetweenTabs.GetHashCode();
                    hashCode = (hashCode*397) ^ (int) HotkeyPrefix;
                    hashCode = (hashCode*397) ^ (int) Trimming;
                    return hashCode;
                }
            }
            #endregion

            public bool RightToLeft = false;
            public bool LineLimit = false;
            public float Angle = 0f;
            public float FirstTabOffset = 40f;
            public float DistanceBetweenTabs = 20f;
            public HotkeyPrefix HotkeyPrefix = HotkeyPrefix.None;
            public StringTrimming Trimming = StringTrimming.None;

            public bitsTextOptions(bool rightToLeft, bool lineLimit, float angle, float firstTabOffset, float distanceBetweenTabs,
                HotkeyPrefix hotkeyPrefix, StringTrimming trimming)
            {
                this.RightToLeft = rightToLeft;
                this.LineLimit = lineLimit;
                this.Angle = angle;
                this.FirstTabOffset = firstTabOffset;
                this.DistanceBetweenTabs = distanceBetweenTabs;
                this.HotkeyPrefix = hotkeyPrefix;
                this.Trimming = trimming;
            }
        }
        #endregion

        #region Fields
        private bitsTextOptions bits = null;
        #endregion

		#region ICloneable
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
            StiTextOptions textOptions = new StiTextOptions();

            if (this.bits != null)
                textOptions.bits = this.bits.Clone() as bitsTextOptions;
            textOptions.wordWrap = this.wordWrap;

            return textOptions;
		}
		#endregion

        #region IEquatable
	    private bool Equals(StiTextOptions other)
	    {
	        return Equals(bits, other.bits) && wordWrap.Equals(other.wordWrap);
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        return obj is StiTextOptions && Equals((StiTextOptions) obj);
	    }

	    public override int GetHashCode()
	    {
	        unchecked
	        {
	            return (((defaultHashCode * 397) ^ (bits != null ? bits.GetHashCode() : 0)) *397) ^ (wordWrap ? 1231 : 1237);
	        }
	    }
        private static int defaultHashCode = "StiTextOptions".GetHashCode();

	    //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        //public override bool Equals(Object obj)
        //{
        //    StiTextOptions options = obj as StiTextOptions;

        //    if (options == null) return false;

        //    if (options.bits == null && this.bits == null) return true;
        //    if (options.bits != null && this.bits == null) return false;
        //    if (options.bits == null && this.bits != null) return false;

        //    if (!this.bits.RightToLeft.Equals(options.bits.RightToLeft)) return false;
        //    if (this.bits.LineLimit != options.bits.LineLimit) return false;
        //    if (this.wordWrap != options.wordWrap) return false;
        //    if (this.bits.Angle != options.bits.Angle) return false;
        //    if (this.bits.FirstTabOffset != options.bits.FirstTabOffset) return false;
        //    if (this.bits.DistanceBetweenTabs != options.bits.DistanceBetweenTabs) return false;
        //    if (!this.bits.HotkeyPrefix.Equals(options.bits.HotkeyPrefix)) return false;
        //    if (!this.bits.Trimming.Equals(options.bits.Trimming)) return false;

        //    return true;
        //}
        #endregion

        #region Methods
        public StringFormat GetStringFormat()
		{
			return GetStringFormat(false, 1);
		}

		public StringFormat GetStringFormat(bool antialiasing, float zoom)
		{
			StringFormat stringFormat = null;
			
			if (antialiasing)stringFormat = new StringFormat(StringFormat.GenericTypographic);
			else stringFormat = new StringFormat();

			stringFormat.FormatFlags = 0;
			if (!WordWrap)stringFormat.FormatFlags = StringFormatFlags.NoWrap;
			if (RightToLeft)stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
			if (LineLimit)stringFormat.FormatFlags |= StringFormatFlags.LineLimit;						
			
			stringFormat.Trimming = Trimming;
			stringFormat.HotkeyPrefix = HotkeyPrefix;

			stringFormat.SetTabStops(this.FirstTabOffset * zoom, new float[]{this.DistanceBetweenTabs * zoom});
			
			return stringFormat;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets horizontal output direction.
		/// </summary>
		[DefaultValue(false)]
		[TypeConverter(typeof(StiBoolConverter))]
		[Description("Gets or sets horizontal output direction.")]
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		public bool RightToLeft
		{
            get
            {
                if (bits == null) return false;
                return bits.RightToLeft;
            }
            set
            {
                if (value == false && bits == null)
                    return;
                if (bits != null)
                    bits.RightToLeft = value;
                else
                    bits = new bitsTextOptions(value, this.LineLimit, this.Angle, 
                        this.FirstTabOffset, this.DistanceBetweenTabs, this.HotkeyPrefix, this.Trimming);
            }
		}


		/// <summary>
		/// Gets or sets value, that show compleleted lines only.
		/// </summary>
		[DefaultValue(false)]
		[TypeConverter(typeof(StiBoolConverter))]
		[Description("Gets or sets value, that show compleleted lines only.")]
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		public bool LineLimit
		{
            get
            {
                if (bits == null) return false;
                return bits.LineLimit;
            }
            set
            {
                if (value == false && bits == null)
                    return;
                if (bits != null)
                    bits.LineLimit = value;
                else
                    bits = new bitsTextOptions(this.RightToLeft, value, this.Angle, 
                        this.FirstTabOffset, this.DistanceBetweenTabs, this.HotkeyPrefix, this.Trimming);
            }
		}

        private bool wordWrap = false;
		/// <summary>
		/// Gets or sets word wrap.
		/// </summary>
		[DefaultValue(false)]
		[TypeConverter(typeof(StiBoolConverter))]
		[Description("Gets or sets word wrap.")]
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		[Browsable(false)]
		public bool WordWrap
		{
			get 
			{
				return wordWrap;
			}
			set 
			{
				wordWrap = value;
			}
		}

		
		/// <summary>
		/// Gets or sets angle of a text rotation.
		/// </summary>
		[DefaultValue(0f)]
		[Description("Gets or sets angle of a text rotation.")]
		[Browsable(false)]
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		public float Angle
		{
            get
            {
                if (bits == null) return 0f;
                return bits.Angle;
            }
            set
            {
                if (value == 0f && bits == null)
                    return;
                if (bits != null)
                    bits.Angle = value;
                else
                    bits = new bitsTextOptions(this.RightToLeft, this.LineLimit, value,
                        this.FirstTabOffset, this.DistanceBetweenTabs, this.HotkeyPrefix, this.Trimming);
            }
		}

		
		/// <summary>
		/// Gets or sets first tab offset.
		/// </summary>
		[DefaultValue(40f)]
		[Description("Gets or sets first tab offset.")]
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		public float FirstTabOffset
		{
            get
            {
                if (bits == null) return 40f;
                return bits.FirstTabOffset;
            }
            set
            {
                if (value < 0) return;
                if (value == 40f && bits == null)
                    return;
                if (bits != null)
                    bits.FirstTabOffset = value;
                else
                    bits = new bitsTextOptions(this.RightToLeft, this.LineLimit, this.Angle,
                        value, this.DistanceBetweenTabs, this.HotkeyPrefix, this.Trimming);
            }
		}
		
		/// <summary>
		/// Gets or sets distance between tabs.
		/// </summary>
		[DefaultValue(20f)]
		[Description("Gets or sets distance between tabs.")]
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		public float DistanceBetweenTabs
		{
            get
            {
                if (bits == null) return 20f;
                return bits.DistanceBetweenTabs;
            }
            set
            {
                if (value < 0) return;
                if (value == 20f && bits == null)
                    return;
                if (bits != null)
                    bits.DistanceBetweenTabs = value;
                else
                    bits = new bitsTextOptions(this.RightToLeft, this.LineLimit, this.Angle,
                        this.FirstTabOffset, value, this.HotkeyPrefix, this.Trimming);
            }
		}
		
		/// <summary>
		/// Gets or sets type of drawing hot keys.
		/// </summary>
		[DefaultValue(HotkeyPrefix.None)]
		[TypeConverter(typeof(Stimulsoft.Base.Localization.StiEnumConverter))]
		[Description("Gets or sets type of drawing hot keys.")]
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		public HotkeyPrefix HotkeyPrefix
		{
            get
            {
                if (bits == null) return HotkeyPrefix.None;
                return bits.HotkeyPrefix;
            }
            set
            {
                if (value == HotkeyPrefix.None && bits == null)
                    return;
                if (bits != null)
                    bits.HotkeyPrefix = value;
                else
                    bits = new bitsTextOptions(this.RightToLeft, this.LineLimit, this.Angle,
                        this.FirstTabOffset, this.DistanceBetweenTabs, value, this.Trimming);
            }
		}

		
		/// <summary>
		/// Gets or sets type to trim the end of a line.
		/// </summary>
		[DefaultValue(StringTrimming.None)]
		[TypeConverter(typeof(Stimulsoft.Base.Localization.StiEnumConverter))]
		[Description("Gets or sets type to trim the end of a line.")]
		[NotifyParentProperty(true)]
		[RefreshProperties(RefreshProperties.All)]
		public StringTrimming Trimming
		{
            get
            {
                if (bits == null) return StringTrimming.None;
                return bits.Trimming;
            }
            set
            {
                if (value == StringTrimming.None && bits == null)
                    return;
                if (bits != null)
                    bits.Trimming = value;
                else
                    bits = new bitsTextOptions(this.RightToLeft, this.LineLimit, this.Angle,
                        this.FirstTabOffset, this.DistanceBetweenTabs, this.HotkeyPrefix, value);
            }
		}


		[Browsable(false)]
		public bool IsDefault
		{
			get
			{
				return (!wordWrap) && bits == null;
			}
		}

		#endregion

		/// <summary>
		/// Creates a new object of the type StiTextOptions.
		/// </summary>
		public StiTextOptions() : 
			this(false, false, false, 0, HotkeyPrefix.None, StringTrimming.None)
		{
		}

		/// <summary>
		/// Creates a new object of the type StiTextOptions.
		/// </summary>
		/// <param name="rightToLeft">Horizontal output direction.</param>
		/// <param name="lineLimit">Show completed lines only.</param>
		/// <param name="wordWrap">Word wrap.</param>
		/// <param name="angle">Angle of a text rotation.</param>
		/// <param name="hotkeyPrefix">Type to draw hot keys.</param>
		/// <param name="trimming">Type to trim the end of a line.</param>
		public StiTextOptions(bool rightToLeft, bool lineLimit, bool wordWrap,
			float angle, HotkeyPrefix hotkeyPrefix, StringTrimming trimming) : 
			this(rightToLeft, lineLimit, wordWrap, angle, hotkeyPrefix, trimming,
			40f, 20f)
		{
		}

		/// <summary>
		/// Creates a new object of the type StiTextOptions.
		/// </summary>
		/// <param name="rightToLeft">Horizontal output direction.</param>
		/// <param name="lineLimit">Show completed lines only.</param>
		/// <param name="wordWrap">Word wrap.</param>
		/// <param name="angle">Angle of a text rotation.</param>
		/// <param name="hotkeyPrefix">Type to draw hot keys.</param>
		/// <param name="trimming">Type to trim the end of a line.</param>
		/// <param name="firstTabOffset">First tab offset.</param>
		/// <param name="distanceBetweenTabs">Distance between tabs.</param>
		public StiTextOptions(bool rightToLeft, bool lineLimit, bool wordWrap,
			float angle, HotkeyPrefix hotkeyPrefix, StringTrimming trimming,
			float firstTabOffset, float distanceBetweenTabs)
		{
            this.wordWrap = wordWrap;

            if (rightToLeft == false &&
                lineLimit == false &&
                angle == 0f &&
                hotkeyPrefix == HotkeyPrefix.None &&
                trimming == StringTrimming.None &&
                firstTabOffset == 40f &&
                distanceBetweenTabs == 20f)
            {
                this.bits = null;
            }
            else
            {
                this.bits = new bitsTextOptions(rightToLeft, lineLimit, angle, firstTabOffset, distanceBetweenTabs, hotkeyPrefix, trimming);
            }
		}
    }
}
