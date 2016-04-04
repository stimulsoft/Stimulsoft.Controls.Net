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
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Class describes SolidBrush.
	/// </summary>
	[RefreshProperties(RefreshProperties.All)]
	public class StiSolidBrush : StiBrush
	{
		#region Properties
		private Color color;
		/// <summary>
		/// Gets or sets the color of this StiSolidBrush object.
		/// </summary>
		[StiSerializable]
		[TypeConverter(typeof(Stimulsoft.Base.Drawing.Design.StiColorConverter))]
		[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, " + StiVersion.VersionInfo, typeof(UITypeEditor))]
		[Description("Gets or sets the color of this StiSolidBrush object.")]
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
		#endregion

        #region IEquatable
        protected bool Equals(StiSolidBrush other)
        {
            return color.Equals(other.color);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StiSolidBrush)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (defaultHashCode * 397) ^ color.GetHashCode();
            }
        }
        private static int defaultHashCode = "StiSolidBrush".GetHashCode();
        #endregion

		#region Methods
	    //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}

        //public override bool Equals(Object obj) 
        //{
        //    StiSolidBrush brush = obj as StiSolidBrush;

        //    if (brush == null)return false;

        //    if (this.color != brush.color)return false;

        //    return true;
        //}

		private bool ShouldSerializeColor()
		{
			return color != Color.Transparent;
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
                }
            }
        }
		#endregion

		#region Constructors
		/// <summary>
		/// Creates a new instance of the StiSolidBrush class.
		/// </summary>
		public StiSolidBrush()
		{
			color = Color.Transparent;
		}

		/// <summary>
		/// Creates a new instance of the StiSolidBrush class.
		/// </summary>
		/// <param name="color">The color of this StiSolidBrush object.</param>
		public StiSolidBrush(Color color)
		{
			this.color = color;
		}
		#endregion
	}
}
