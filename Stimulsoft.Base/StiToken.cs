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

namespace Stimulsoft.Base
{	
	/// <summary>
	/// Class describes Token.
	/// </summary>
	public class StiToken
	{
		private int index;
		/// <summary>
		/// Gets or sets value indicates the beginning of token in text.
		/// </summary>
		public int Index
		{
			get
			{
				return index;
			}
			set
			{
				index = value;
			}
		}

		
		private int length;
		/// <summary>
		/// Gets or sets value indicates the length of token.
		/// </summary>
		public int Length
		{
			get
			{
				return length;
			}
			set
			{
				length = value;
			}
		}

		
		private StiTokenType type;
		/// <summary>
		/// Gets or sets value indicates the type of token.
		/// </summary>
		public StiTokenType	Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		}

		
		private object data;	
		/// <summary>
		/// Gets or sets Value of the identifier.
		/// </summary>
		public object Data
		{
			get
			{
				return data;
			}
			set
			{
				data = value;
			}
		}


		/// <summary>
		/// Create a new instance StiToken.
		/// </summary>
		/// <param name="type">Type Token.</param>
		public StiToken(StiTokenType type):this(type, 0, 0)
		{
		}
		

		/// <summary>
		/// Creates a new object of the type StiToken.
		/// </summary>
		/// <param name="type">Type Token.</param>
		/// <param name="index">The Beginning Token in text.</param>
		/// <param name="length">The Length Token.</param>
		public StiToken(StiTokenType type, int index, int length)
		{
			this.type = type;
			this.index = index;
			this.length = length;
		}
		

		/// <summary>
		/// Creates a new object of the type StiToken.
		/// </summary>
		/// <param name="type">Type Token.</param>
		/// <param name="index">The Beginning Token in text.</param>
		/// <param name="length">The Length Token.</param>
		/// <param name="charValue">Char for initializing</param>
		public StiToken(StiTokenType type, int index, int length, char charValue) : this(type, index, length)
		{
			this.Data = charValue;
		}

		
		/// <summary>
		/// Creates an object of the type StiToken that contains the value of the string.
		/// </summary>
		/// <param name="type">Type Token.</param>
		/// <param name="index">The Beginning Token in text.</param>
		/// <param name="length">The Length Token.</param>
		/// <param name="stringValue">String for initializing.</param>
		public StiToken(StiTokenType type, int index, int length, string stringValue) : this(type, index, length)
		{
			this.Data = stringValue;
		}


		/// <summary>
		/// Creates an object of the type StiToken that contains an object.
		/// </summary>
		/// <param name="type">Type Token</param>
		/// <param name="index">The Beginning Token in text.</param>
		/// <param name="length">The Length Token.</param>
		/// <param name="obj">Object for initializing.</param>
		public StiToken(StiTokenType type, int index, int length, object obj) : this(type, index, length)
		{
			this.Data = obj;
		}


		public override string ToString()
		{
			switch(Type)
			{
				case StiTokenType.Value:
					return Type.ToString() + "=" + Data.ToString();

				case StiTokenType.Ident:
					return Type.ToString() + "(" + Data.ToString() + ")";

				default:
					return Type.ToString();
			}
		}
	}
}
