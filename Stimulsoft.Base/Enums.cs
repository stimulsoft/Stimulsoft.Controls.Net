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

namespace Stimulsoft.Base
{

    #region StiAnimationType
    public enum StiAnimationType
    {
        Opacity,
        Scale,
        Translation
    }
    #endregion
    
	#region StiOutputType
	/// <summary>
	/// Types of the result to compiling.
	/// </summary>
	public enum StiOutputType 
	{
		/// <summary>
		/// Class library.
		/// </summary>
		ClassLibrary, 

		/// <summary>
		/// Console application.
		/// </summary>
		ConsoleApplication, 

		/// <summary>
		/// Windows application.
		/// </summary>
		WindowsApplication
	}
	#endregion

	#region StiLexerError
	/// <summary>
	/// Defines identifiers that indicate the type of lexical analysis error.
	/// </summary>
	public enum StiLexerError
	{
		/// <summary>
		/// Left paren not found.
		/// </summary>
		LParenNotFound,
		/// <summary>
		/// Comma not found.
		/// </summary>
		CommaNotFound,
		/// <summary>
		/// Assing not found.
		/// </summary>
		AssignNotFound,
		/// <summary>
		/// Right paren not found.
		/// </summary>
		RParenNotFound,
		/// <summary>
		/// Left brace not found.
		/// </summary>
		LBraceNotFound,
		/// <summary>
		/// Semicolon not found.
		/// </summary>
		SemicolonNotFound,
		/// <summary>
		/// Right brace not found.
		/// </summary>
		RBraceNotFound
	}
	#endregion

	#region StiTokenType
	/// <summary>
	/// Types of token.
	/// </summary>
	public enum StiTokenType
	{
		/// <summary>
		/// None token.
		/// </summary>
		None = 0,
		/// <summary>
		/// .
		/// </summary>
		Dot, 
		/// <summary>
		/// ,
		/// </summary>
		Comma, 
		/// <summary>
		/// :
		/// </summary>
		Colon, 
		/// <summary>
		/// ;
		/// </summary>
		SemiColon,
		/// <summary>
		/// Shift to the left Token.
		/// </summary>
		Shl, 
		/// <summary>
		/// Shift to the right Token.
		/// </summary>
		Shr,
		/// <summary>
		/// Assign Token.
		/// </summary>
		Assign,
		/// <summary>
		/// Equal Token.
		/// </summary>
		Equal,
		/// <summary>
		/// NotEqual Token.
		/// </summary>
		NotEqual, 
		/// <summary>
		/// LeftEqual Token.
		/// </summary>
		LeftEqual, 
		/// <summary>
		/// Left Token.
		/// </summary>
		Left,
		/// <summary>
		/// RightEqual Token.
		/// </summary>
		RightEqual, 
		/// <summary>
		/// Right Token.
		/// </summary>
		Right,
		/// <summary>
		/// Logical OR Token.
		/// </summary>
		Or, 
		/// <summary>
		/// Logical AND Token.
		/// </summary>
		And, 
		/// <summary>
		/// Logical NOT Token.
		/// </summary>
		Not, 
		/// <summary>
		/// Double logical OR Token.
		/// </summary>
		DoubleOr, 
		/// <summary>
		/// Double logical AND Token.
		/// </summary>
		DoubleAnd,
		/// <summary>
		/// Copyright
		/// </summary>
		Copyright,
		/// <summary>
		/// ?
		/// </summary>
		Question,
		/// <summary>
		/// +
		/// </summary>
		Plus,
		/// <summary>
		/// -
		/// </summary>
		Minus, 
		/// <summary>
		/// *
		/// </summary>
		Mult,
		/// <summary>
		/// /
		/// </summary>
		Div, 
		/// <summary>
		/// \
		/// </summary>
		Splash, 
		/// <summary>
		/// %
		/// </summary>
		Percent, 
		/// <summary>
		/// @
		/// </summary>
		Ampersand, 
		/// <summary>
		/// #
		/// </summary>
		Sharp,
		/// <summary>
		/// $
		/// </summary>
		Dollar,
		/// <summary>
		/// ˆ
		/// </summary>
		Euro,
		/// <summary>
		/// ++
		/// </summary>
		DoublePlus, 
		/// <summary>
		/// --
		/// </summary>
		DoubleMinus,
		/// <summary>
		/// (
		/// </summary>
		LPar,
		/// <summary>
		/// )
		/// </summary>
		RPar,
		/// <summary>
		/// {
		/// </summary>
		LBrace, 
		/// <summary>
		/// }
		/// </summary>
		RBrace, 
		/// <summary>
		/// [
		/// </summary>
		LBracket, 
		/// <summary>
		/// ]
		/// </summary>
		RBracket,
		/// <summary>
		/// Token contains value.
		/// </summary>
		Value, 
		/// <summary>
		/// Token contains identifier.
		/// </summary>
		Ident,
		/// <summary>
		/// 
		/// </summary>
		Unknown,
		/// <summary>
		/// EOF Token.
		/// </summary>
		EOF
	}
	#endregion

	#region StiSqlParserType
	public enum StiSqlParserType
	{
		Number,
		Date,
		String
	}
    #endregion

    #region StiGuiMode
    public enum StiGuiMode
    {
        Gdi,
        Wpf,
        Xbap,
        Cloud
    }
    #endregion

    #region StiLevel
    /// <summary>
    /// Enums provides levels of access to property.
    /// </summary>
    public enum StiLevel
    {
        /// <summary>
        /// Minimal level of properties access. Only principal properties of components.
        /// </summary>
        Basic,
        /// <summary>
        /// Standard level of properties access. All properties available except rarely used properties.
        /// </summary>
        Standard,
        /// <summary>
        /// Professional level of properties access. All properties available.
        /// </summary>
        Professional
    }
    #endregion
}