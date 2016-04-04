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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Drawing;
using System.Net.Configuration;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Stimulsoft.Base
{
	/// <summary>
	/// Saving of the positions in text.
	/// </summary>
	public struct StiPosition
	{
		private int line;
		/// <summary>
		/// Gets or sets line in text.
		/// </summary>
		public int Line
		{
			get
			{
				return line;
			}
			set
			{
				line = value;
			}
		}
		

		private int column;
		/// <summary>
		/// Gets or sets column in text.
		/// </summary>
		public int Column
		{
			get
			{
				return column;
			}
			set
			{
				column = value;
			}
		}


		/// <summary>
		/// Creates position in text.
		/// </summary>
		/// <param name="line">Line in text.</param>
		/// <param name="column">Column in text.</param>
		public StiPosition(int line, int column)
		{
			this.line = line;
			this.column = column;
		}
	};

	/// <summary>
	/// Performs the lexical analysis.
	/// </summary>
	public sealed class StiLexer
	{
		private StringBuilder text;
		/// <summary>
		/// Gets or sets text for analys.
		/// </summary>
		public StringBuilder Text
		{
			get
			{
				return text;
			}
            set
            {
                text = value;
                baseText = value.ToString();
            }
		}

        private string baseText;
        /// <summary>
        /// Gets or sets text for analys.
        /// </summary>
        internal string BaseText
        {
            get
            {
                return baseText;
            }
            set
            {
                baseText = value;
            }
        }

		/// <summary>
		/// Start positions of token.
		/// </summary>
        private List<int> positions = new List<int>();

		private int positionInText;
		/// <summary>
		/// Gets or sets current position in text.
		/// </summary>
		public int PositionInText
		{
			get
			{
				return positionInText;
			}
            set
            {
                positionInText = value;
            }
		}


		/// <summary>
		/// Start of current token.
		/// </summary>
		private int begToken;

		/// <summary>
		/// Saves position of token in text.
		/// </summary>
		public void SavePosToken()
		{
			begToken = positionInText;
			positions.Add(positionInText);
		}


		/// <summary>
		/// Gets position of token in text.
		/// </summary>
		/// <param name="positionInText">Position in text.</param>
		/// <returns>Position of token in text.</returns>
		public StiPosition GetPosition(int positionInText)
		{
			StiPosition pos = new StiPosition(1, 1);

			for (int ps = 0; ps < positionInText; ps++)
			{
				pos.Column++;
				if (Text[ps] == '\n')
				{
					pos.Line++;
					pos.Column = 1;
				}
			}
			return pos;
		}


		/// <summary>
		/// Skips all not control symbols.
		/// </summary>
		public void Skip()
		{
			while (
				positionInText < Text.Length && 
				(Char.IsWhiteSpace(Text[positionInText]) || 
				Char.IsControl(Text[positionInText])))
			{
				positionInText++;
			}
		}

		
		/// <summary>
		/// Wait the left paren.
		/// </summary>
		public bool WaitLparen2()
		{
			StiToken token = GetToken();
			return token.Type == StiTokenType.LPar;
		}
		

		/// <summary>
		/// Wait the right bracket.
		/// </summary>
		public bool WaitComma2()
		{
			StiToken token = GetToken();
			return token.Type == StiTokenType.Comma;
		}


		/// <summary>
		/// Wait the assign.
		/// </summary>
		public bool WaitAssign2()
		{
			StiToken token = GetToken();
			return token.Type == StiTokenType.Assign;
		}


		/// <summary>
		/// Wait the right paren.
		/// </summary>
		public bool WaitRparen2()
		{
			StiToken token = GetToken();
			return token.Type == StiTokenType.RPar;
		}


		/// <summary>
		/// Wait the left brace.
		/// </summary>
		public bool WaitLbrace2()
		{
			StiToken token = GetToken();
			return token.Type == StiTokenType.LBrace;
		}


		/// <summary>
		/// Wait the semicolon.
		/// </summary>
		public bool WaitSemicolon2()
		{
			StiToken token = GetToken();
			return token.Type == StiTokenType.SemiColon;
		}

		
		/// <summary>
		/// Wait the right brace.
		/// </summary>
		public bool WaitRbrace2()
		{
			StiToken token = GetToken();
			return token.Type == StiTokenType.RBrace;
		}


		/// <summary>
		/// Scans the number.
		/// </summary>
		/// <returns>Token containing number.</returns>
		public StiToken ScanNumber()
		{
			int posStart = positionInText;
			
			bool isFloat = false;
			
			while (positionInText != Text.Length && Char.IsDigit(Text[positionInText]))
			{
				positionInText++;
			}

			if (positionInText != Text.Length && Text[positionInText] == '.' && 
				positionInText + 1 != Text.Length && Char.IsDigit(Text[positionInText + 1]))
			{
				positionInText++;
				while (positionInText != Text.Length && Char.IsDigit(Text[positionInText]))
				{
					positionInText++;
				}
				isFloat = true;
			}
      
			string nm = BaseText.Substring(posStart, positionInText - posStart);
			if (isFloat)
			{
                var nfi = new NumberFormatInfo
                {
                    CurrencyDecimalSeparator = "."
                };

			    return new StiToken(StiTokenType.Value, posStart, positionInText - posStart, 
					double.Parse(nm, nfi));
            }
			else
			{
				string valueStr = nm;
				try
				{
					ulong value = ulong.Parse(valueStr);
					return new StiToken(StiTokenType.Value, posStart, positionInText - posStart, 
						value);
				}
				catch(Exception e)				
				{
					if (e is OverflowException || e is FormatException)
					{
						return new StiToken(StiTokenType.Value, posStart, positionInText - posStart, valueStr);
					}
					else throw;
				}
			}
		}

		/// <summary>
		/// Scans the identifier.
		/// </summary>
		/// <returns>Token containing identifier.</returns>
		public StiToken ScanIdent()
		{
			int startIndex = positionInText;
			string ident = string.Empty;
			while (positionInText != Text.Length && 
				(Char.IsLetterOrDigit(Text[positionInText]) || 
				Text[positionInText] == '_'  || 
				Text[positionInText] == '№'))
			{
				ident += Text[positionInText++];
			}
			return new StiToken(StiTokenType.Ident, startIndex, positionInText - startIndex, ident);
		}


		/// <summary>
		/// Scans the string.
		/// </summary>
		/// <returns>Token containing string.</returns>
		public StiToken ScanString()
		{
			int startIndex = positionInText;
			positionInText++;
			string str = string.Empty;
			while (positionInText != Text.Length && Text[positionInText] != '"')
			{
				str += Text[positionInText++];
			}
			if (positionInText == Text.Length)
			{
				new StiToken(StiTokenType.Value, startIndex, positionInText - startIndex, str);
			}
			positionInText++;
			return new StiToken(StiTokenType.Value, startIndex, positionInText - startIndex, str);
		}


		/// <summary>
		/// Scans the symbol.
		/// </summary>
		/// <returns>Token containing symbol.</returns>
		public StiToken ScanChar()
		{
			if (++positionInText == Text.Length)
			{
				return new StiToken(StiTokenType.Value, positionInText - 3, 3, ' ');
			}
			char c = Text[positionInText++];
			if (positionInText == Text.Length || Text[positionInText] != '\'')
			{
				return new StiToken(StiTokenType.Value, positionInText - 3, 3, c);
			}
			positionInText++;
			return new StiToken(StiTokenType.Value, positionInText - 3, 3, c);
		}


		/// <summary>
		/// Returns to previous token.
		/// </summary>
		public void UngetToken()
		{
			positionInText = positions[positions.Count - 1];
			positions.RemoveAt(positions.Count - 1);
		}


		/// <summary>
		/// Gets next token.
		/// </summary>
		/// <returns>Next token.</returns>
		public StiToken GetToken()
		{
			Skip();
			
			#region End of the text
			if (Text.Length <= positionInText)
			{
				return new StiToken(StiTokenType.EOF, positionInText, 0);
			}
			#endregion

			#region Ident
			else if (Char.IsLetter(Text[positionInText]) || Text[positionInText] == '_' || Text[positionInText] == '№')
			{
				int startIndex = positionInText;
				SavePosToken();
				StiToken token = ScanIdent();
				switch((string)token.Data)
				{
					case "true":	return new StiToken(StiTokenType.Value, startIndex, 4, true);
					case "false":	return new StiToken(StiTokenType.Value, startIndex, 5, false);
				}
				return token;
			}
			#endregion
				
			#region Number
			else if (Char.IsDigit(Text[positionInText]))
			{
				SavePosToken();
				return ScanNumber();
			}
			#endregion

			#region String
			else if (Text[positionInText] == '"')
			{
				SavePosToken();
				return ScanString();
			}
			#endregion

			#region Char
			else if (Text[positionInText] == '\'')
			{
				SavePosToken();
				return ScanChar();
			}
			#endregion

			else
			{
				#region Operators
				switch (Text[positionInText])
				{
					case '€': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Euro,		positionInText - 1, 1);
					case '®': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Copyright,	positionInText - 1, 1);
					case '(': SavePosToken(); positionInText++; return new StiToken(StiTokenType.LPar,		positionInText - 1, 1);
					case ')': SavePosToken(); positionInText++; return new StiToken(StiTokenType.RPar,		positionInText - 1, 1);
					case '{': SavePosToken(); positionInText++; return new StiToken(StiTokenType.LBrace,	positionInText - 1, 1);
					case '}': SavePosToken(); positionInText++; return new StiToken(StiTokenType.RBrace,	positionInText - 1, 1);
					case ',': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Comma,		positionInText - 1, 1);
					case '.': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Dot,		positionInText - 1, 1);
					case ';': SavePosToken(); positionInText++; return new StiToken(StiTokenType.SemiColon,	positionInText - 1, 1);
					case ':': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Colon,		positionInText - 1, 1);
					case '!': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Minus,		positionInText - 1, 1);
					case '*': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Mult,		positionInText - 1, 1);
					case '^': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Not,		positionInText - 1, 1);
					case '/': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Div,		positionInText - 1, 1);
					case '\\': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Splash,	positionInText - 1, 1);
					case '%': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Percent,	positionInText - 1, 1);
					case '#': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Sharp,		positionInText - 1, 1);
					case '$': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Dollar,	positionInText - 1, 1);
					case '@': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Ampersand,	positionInText - 1, 1);
					case '[': SavePosToken(); positionInText++; return new StiToken(StiTokenType.LBracket,	positionInText - 1, 1);
					case ']': SavePosToken(); positionInText++; return new StiToken(StiTokenType.RBracket,	positionInText - 1, 1);
					case '?': SavePosToken(); positionInText++; return new StiToken(StiTokenType.Question,	positionInText - 1, 1);

					#region Symbol "|"
					case '|': 
						SavePosToken();
						positionInText++;
						if (positionInText != Text.Length && Text[positionInText] == '|')
						{
							positionInText++;
							return new StiToken(StiTokenType.DoubleOr, positionInText - 2, 2);
						}
						return new StiToken(StiTokenType.Or, positionInText - 1, 1);
					#endregion

					#region Symbol "&"
					case '&': 
						SavePosToken();
						positionInText++;
						if (positionInText != Text.Length && Text[positionInText] == '&')
						{
							positionInText++;
							return new StiToken(StiTokenType.DoubleAnd, positionInText - 2, 2);
						}
						return new StiToken(StiTokenType.And, positionInText - 1, 1);
					#endregion

					#region Symbol "+"
					case '+': 
						SavePosToken();
						positionInText++;
						if (positionInText != Text.Length && Text[positionInText] == '+')
						{
							positionInText++;
							return new StiToken(StiTokenType.DoublePlus, positionInText - 2, 2);
						}
						return new StiToken(StiTokenType.Plus, positionInText - 1, 1);
					#endregion

					#region Symbol "-"
					case '-': 
						SavePosToken();
						positionInText++;
						if (positionInText != Text.Length && Text[positionInText] == '-')
						{
							positionInText++;
							return new StiToken(StiTokenType.DoubleMinus, positionInText - 2, 2);
						}
						return new StiToken(StiTokenType.Minus, positionInText - 1, 1);
					#endregion

					#region Symbol "="
					case '=':
						SavePosToken();
						positionInText++;
						if (positionInText != Text.Length && Text[positionInText] == '=')
						{
							positionInText++;
							return new StiToken(StiTokenType.Equal, positionInText - 2, 2);
						}
						return new StiToken(StiTokenType.Assign, positionInText - 1, 1);
					#endregion
					
					#region Symbol "<"
					case '<':
						SavePosToken();
						positionInText++;
						if (positionInText != Text.Length && Text[positionInText] == '=')
						{
							positionInText++;
							return new StiToken(StiTokenType.LeftEqual, positionInText - 2, 2);
						}
						if (positionInText != Text.Length && Text[positionInText] == '<')
						{
							positionInText++;
							return new StiToken(StiTokenType.Shl, positionInText - 2, 2);
						}
						return new StiToken(StiTokenType.Left, positionInText - 1, 1);
					#endregion
					
					#region Symbol ">"
					case '>':
						SavePosToken();
						positionInText++;
						if (positionInText != Text.Length && Text[positionInText] == '=')
						{
							positionInText++;
							return new StiToken(StiTokenType.RightEqual, positionInText - 2, 2);
						}
						if (positionInText != Text.Length && Text[positionInText] == '>')
						{
							positionInText++;
							return new StiToken(StiTokenType.Shr, positionInText - 2, 2);
						}
						return new StiToken(StiTokenType.Right, positionInText - 1, 1);
					#endregion

					default:
					{
						SavePosToken(); positionInText++; return new StiToken(StiTokenType.Unknown,	positionInText - 1, 1);
					}
				}
                #endregion
            }
		
		}


		/// <summary>
		/// Reset state.
		/// </summary>
		public void Reset()
		{
			positions.Clear();
            positionInText = 0;
		}

		
        ///// <summary>
        ///// Replaces all occurrences of a specified string, with another specified string.
        ///// Replacing is produced with provision for tokens.
        ///// </summary>
        ///// <param name="textValue">Text for replace.</param>
        ///// <param name="oldValue">A String to be replaced.</param>
        ///// <param name="newValue">A String to replace all occurrences of oldValue.</param>
        ///// <returns>Replaced string.</returns>
        //public static string Replace(string textValue, string oldValue, string newValue)
        //{
        //    StringBuilder sb = new StringBuilder(textValue);
        //    StiLexer lexer = new StiLexer(textValue);
			
        //    StiToken token = null;
        //    do
        //    {
        //        token = lexer.GetToken();
        //        if (token.Type == StiTokenType.Ident && ((string)token.Data) == oldValue)
        //        {
        //            sb = sb.Replace(oldValue, newValue, token.Index, token.Length);
        //            lexer.positionInText += newValue.Length;
        //        }
				
        //    }
        //    while (token.Type != StiTokenType.EOF);

        //    return sb.ToString();
        //}


		/// <summary>
		/// Replaces all occurrences of a specified String, with another specified String.
		/// Before oldValue must follow specified prefix - token.
		/// Replacing is produced with provision for tokens.
		/// </summary>
		/// <param name="textValue">Text for replace.</param>
		/// <param name="prefix">Prefix - token.</param>
		/// <param name="oldValue">A String to be replaced.</param>
		/// <param name="newValue">A String to replace all occurrences of oldValue.</param>
		/// <returns>Replaced string.</returns>
		public static string ReplaceWithPrefix(string textValue, string prefix, string oldValue, string newValue)
		{
			StringBuilder sb = new StringBuilder(textValue);
			StiLexer lexer = new StiLexer(textValue);
			
			StiToken prefixToken = lexer.GetToken();
			if (prefixToken.Type == StiTokenType.EOF)return textValue;

			StiToken token = null;
			do
			{
				token = lexer.GetToken();
				if (token.Type == StiTokenType.Ident && 
					prefixToken.Type == StiTokenType.Ident && 
					((string)prefixToken.Data) == prefix &&
					((string)token.Data) == oldValue)
				{
					sb = sb.Replace(oldValue, newValue, token.Index, token.Length);
					lexer.positionInText += newValue.Length;
				}
				prefixToken = token;
				
			}
			while (token.Type != StiTokenType.EOF);

			return sb.ToString();
		}

	
		/// <summary>
		/// Replaces all occurrences of a specified String, with another specified string.
		/// Before oldValue must follow specified prefix - string.
		/// Replacing is produced with provision for tokens.
		/// </summary>
		/// <param name="prefix">Prefix - string.</param>
		/// <param name="oldValue">A String to be replaced.</param>
		/// <param name="newValue">A String to replace all occurrences of oldValue.</param>
		/// <returns>Replaced string.</returns>
		public void ReplaceWithPrefix(string prefix, string oldValue, string newValue)
		{
			this.Reset();
			
			StiToken prefixToken = this.GetToken();
			if (prefixToken.Type == StiTokenType.EOF)return;
			StiToken token = null;
			do
			{
				token = this.GetToken();
				if (token.Type == StiTokenType.Ident && 
					prefixToken.Type == StiTokenType.Ident && 
					((string)prefixToken.Data) == prefix &&
					((string)token.Data) == oldValue)
				{
					text = text.Replace(oldValue, newValue, token.Index, token.Length);
					this.positionInText += newValue.Length;
				}
				prefixToken = token;
				
			}while (token.Type != StiTokenType.EOF);
            baseText = text.ToString();
        }

		
		/// <summary>
		/// Replaces all occurrences of a specified String, with another specified string.
		/// Before oldValue must not follow specified prefix - string.
		/// </summary>
		/// <param name="prefix">Prefix - string.</param>
		/// <param name="oldValue">A String to be replaced.</param>
		/// <param name="newValue">A String to replace all occurrences of oldValue.</param>
		/// <returns>Replaced string.</returns>
		public void ReplaceWithNotEqualPrefix(StiTokenType prefix, string oldValue, string newValue)
		{
			this.Reset();
			
			StiToken prefixToken = this.GetToken();
			if (prefixToken.Type == StiTokenType.EOF)return;
			StiToken token = null;
			do
			{
				token = this.GetToken();
				if (token.Type == StiTokenType.Ident && 
					prefixToken.Type != prefix &&
					((string)token.Data) == oldValue)
				{
					text = text.Replace(oldValue, newValue, token.Index, token.Length);
					this.positionInText += newValue.Length;
				}
				prefixToken = token;
				
			}
            while (token.Type != StiTokenType.EOF);
            baseText = text.ToString();
        }

	    public static bool IdentExists(string str, string name, bool caseSensitive)
	    {
	        var lexer = new StiLexer(str);
	        while (true)
	        {
	            var token = lexer.GetToken();
	            if (token == null || token.Type == StiTokenType.EOF) return false;
	            if (token.Type == StiTokenType.Ident && token.Data != null)
	            {
	                if (caseSensitive && token.Data.ToString() == name) return true;
                    if (!caseSensitive && token.Data.ToString().ToLowerInvariant() == name.ToLowerInvariant()) return true;
	            }
	        }
	    }

        public static List<StiToken> GetAllTokens(string str)
        {
            var tokens = new List<StiToken>();
            var lexer = new StiLexer(str);
            
            while (true)
            {
                var token = lexer.GetToken();
                if (token == null || token.Type == StiTokenType.EOF) return tokens;
                tokens.Add(token);
            }
        }

		
        ///// <summary>
        ///// Replaces all occurrences of a specified String, with another specified string.
        ///// Replacing is produced with provision for tokens.
        ///// </summary>
        ///// <param name="oldValue">A String to be replaced.</param>
        ///// <param name="newValue">A String to replace all occurrences of oldValue.</param>
        //public void Replace(string oldValue, string newValue)
        //{
        //    this.Reset();

        //    StiToken token = null;
        //    do
        //    {
        //        token = this.GetToken();
        //        if (token.Type == StiTokenType.Ident && ((string)token.Data) == oldValue)
        //        {
        //            text = text.Replace(oldValue, newValue, token.Index, token.Length);
        //            this.positionInText += newValue.Length;
        //        }
				
        //    }while (token.Type != StiTokenType.EOF);
        //}

		
		/// <summary>
		/// Creates a new instance of the StiLexer class.
		/// </summary>
		/// <param name="textValue">The Text for lexical analysis.</param>
		public StiLexer(string textValue)
		{
            this.baseText = textValue;
			this.text = new StringBuilder(textValue);
			positionInText = 0;
		}

	}
}
