#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports  											}
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
{	TRADE SECRETS OF STIMULSOFT										}
{																	}
{	CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON		}
{	ADDITIONAL RESTRICTIONS.										}
{																	}
{*******************************************************************}
*/
#endregion Copyright (C) 2003-2016 Stimulsoft

using System;
using System.Xml.Linq;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Base
{
    public abstract class StiObjectSchema
    {
        #region Properties
        /// <summary>
        /// A name of the schema object.
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Methods.SaveLoad
        /// <summary>
        /// Loads element from string.
        /// </summary>
        /// <param name="str">String representation which contain item description.</param>
        public virtual void LoadFromString(string str)
        {
            JsonConvert.PopulateObject(str, this);
        }

        /// <summary>
        /// Loads element from byte array.
        /// </summary>
        public virtual void LoadFromBytes(byte[] bytes)
        {
            var str = StiPacker.UnpackToString(bytes);
            if (str == null) return;

            LoadFromString(str);
        }


        /// <summary>
        /// Saves element to string.
        /// </summary>
        /// <returns>String representation which contains schema.</returns>
        public virtual string SaveToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, StiJsonHelper.DefaultSerializerSettings);
        }

        /// <summary>
        /// Saves element to byte array.
        /// </summary>
        /// <returns>String representation which contain item description.</returns>
        public virtual byte[] SaveToBytes()
        {
            return StiPacker.PackToBytes(SaveToString());
        }


        /// <summary>
        /// Saves specified schema to XDocument object.
        /// </summary>
        /// <returns>XElement object which contains xml description of saved schema.</returns>
        public virtual XDocument SaveToXml()
        {
            return JsonConvert.DeserializeXNode(this.SaveToString());
        }
        #endregion
    }
}
