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

namespace Stimulsoft.Base.Serializing
{

	/// <summary>
	/// Attribute with serialization parameters.
	/// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public sealed class StiSerializableAttribute : Attribute
	{
		private StiSerializationVisibility visibility;
		/// <summary>
		/// Gets serialization visibility.
		/// </summary>
		public StiSerializationVisibility Visibility 
		{
			get 
			{ 
				return visibility;
			} 
		}


		private StiSerializeTypes serializeType;
		/// <summary>
		/// Gets or sets serialization type.
		/// </summary>
		public  StiSerializeTypes SerializeType
		{
			get
			{
				return serializeType;
			}
			set
			{
				serializeType = value;
			}

		}


		/// <summary>
		/// Creates a new object of the type StiSerializableAttribute. 
		/// Visibility is set into the Content.
		/// </summary>
		public StiSerializableAttribute() : this(StiSerializationVisibility.Content)
		{
		}
		
		
		/// <summary>
		/// Creates a new object of the type StiSerializableAttribute. 
		/// </summary>
		/// <param name="visibility">Serialized object visibility.</param>
		public StiSerializableAttribute(StiSerializationVisibility visibility) 
			 : this(visibility, StiSerializeTypes.SerializeToAll)
		{
		}

		/// <summary>
		/// Creates a new object of the type StiSerializableAttribute. 
		/// </summary>
		/// <param name="serializeType">Serializaion type of a serialized object.</param>
		public StiSerializableAttribute(StiSerializeTypes serializeType) :
			this(StiSerializationVisibility.Content, serializeType)
		{
			
		}
		
				
		/// <summary>
		/// Creates a new object of the type StiSerializableAttribute. 
		/// </summary>
		/// <param name="visibility">Serialized object visibility.</param>
		/// <param name="serializeType">Serializaion type of a serialized object.</param>
		public StiSerializableAttribute(StiSerializationVisibility visibility, StiSerializeTypes serializeType) 
		{
			this.serializeType = serializeType;
			this.visibility = visibility;
		}
		
	}

}
