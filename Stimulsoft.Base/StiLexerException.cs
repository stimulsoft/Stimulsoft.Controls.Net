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
	/// The exception that is thrown when a lexical analysis error occurs.
	/// </summary>
	public class StiLexerException : Exception
	{
		private StiLexerError lexerError;
		/// <summary>
		/// Gets or sets type of lexical analysis error.
		/// </summary>
		public StiLexerError LexerError
		{
			get
			{
				return lexerError;
			}
			set
			{
				lexerError = value;
			}
		}

		/// <summary>
		/// Create a new instance of the StiLexerException class.
		/// </summary>
		/// <param name="error">Type of lexical analysis error.</param>
		public StiLexerException(StiLexerError error)
		{
			this.lexerError = error;
		}
	}
}
