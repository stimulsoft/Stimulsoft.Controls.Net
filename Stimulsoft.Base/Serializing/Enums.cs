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
	#region StiSerializeTypes
	/// <summary>
	/// Type of serialization.
	/// </summary>
	[Flags]
	public enum StiSerializeTypes 
	{
		/// <summary>
		/// Serialize for all variations.
		/// </summary>
		SerializeToAll = 15,
		/// <summary>
		/// Serialize in the code only.
		/// </summary>
		SerializeToCode = 1, 
		/// <summary>
		/// Serialize for the designer only.
		/// </summary>
		SerializeToDesigner = 2, 
		/// <summary>
		/// Serialize for save or load only.
		/// </summary>
		SerializeToSaveLoad = 4,
		/// <summary>
		/// Serialize for document only.
		/// </summary>
		SerializeToDocument = 8
	}
	#endregion

	#region StiSerializationVisibility
	/// <summary>
	/// Serialization of visibility.
	/// </summary>
	public enum StiSerializationVisibility 
	{
		/// <summary>
		/// Do not serialize. 
		/// </summary>
		None, 
		/// <summary>
		/// Serialize with the line.
		/// </summary>
		Content, 
		/// <summary>
		/// Serialize as a collection or array.
		/// </summary>
		List, 
		/// <summary>
		/// Serialize as a class.
		/// </summary>
		Class,
		/// <summary>
		/// Serialize as a reference to a class.
		/// </summary>
		Reference,
        Control
	}
	#endregion
}
