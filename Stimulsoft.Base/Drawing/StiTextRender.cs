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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Stimulsoft.Base.Drawing
{
    [System.Security.SecuritySafeCritical]
    public class StiTextRenderer
    {
        #region Static properties
        private static double precisionModeFactor = 4;
        /// <summary>
        /// Obsolete
        /// </summary>
        public static double PrecisionModeFactor
        {
            get
            {
                return precisionModeFactor;
            }
            set
            {
                precisionModeFactor = value;
            }
        }

        private static bool precisionModeEnabled = false;
        /// <summary>
        /// Obsolete
        /// </summary>
        public static bool PrecisionModeEnabled
        {
            get
            {
                return precisionModeEnabled;
            }
            set
            {
                precisionModeEnabled = value;
            }
        }

        private static bool correctionEnabled = true;
        /// <summary>
        /// Enable the inter-character distance correction
        /// Can cause exception on some systems or fonts
        /// </summary>
        public static bool CorrectionEnabled
        {
            get
            {
                return correctionEnabled && !compatibility2009;
            }
            set
            {
                correctionEnabled = value;
            }
        }

        private static float maxFontSize = 1024;
        public static float MaxFontSize
        {
            get
            {
                return maxFontSize;
            }
            set
            {
                maxFontSize = value;
            }
        }

        private static bool compatibility2009 = false;
        /// <summary>
        /// Enable text scaling mode as in the 2009.1 version.
        /// For backward compatibility 
        /// </summary>
        public static bool Compatibility2009
        {
            get
            {
                return compatibility2009;
            }
            set
            {
                compatibility2009 = value;
            }
        }

        private static bool optimizeBottomMargin = true;
        /// <summary>
        /// Optimize the bottom margin of the text
        /// </summary>
        public static bool OptimizeBottomMargin
        {
            get
            {
                return optimizeBottomMargin;
            }
            set
            {
                optimizeBottomMargin = value;
            }
        }

        private static bool interpreteFontSizeInHtmlTagsAsInHtml = false;
        /// <summary>
        /// Interprete the FontSize attribute in the Html-tags as in Html
        /// </summary>
        public static bool InterpreteFontSizeInHtmlTagsAsInHtml 
        {
            get
            {
                return interpreteFontSizeInHtmlTagsAsInHtml;
            }
            set
            {
                interpreteFontSizeInHtmlTagsAsInHtml = value;
            }
        }
        #endregion

        #region Allows Html-tags
        //Allows tag:
        //+ <b> </b>
        //+ <i> </i>
        //+ <u> </u>
        //+ <s> </s>
        //+ <sub> </sub>
        //+ <sup> </sup>
        //+ <font color="#rrggbb" face="FontName" size="1..n"> </font>
		//+ <font-face="FontName"> </font-face>
		//+ <font-name="FontName"> </font-name>
		//+ <font-family="FontName"> </font-family>
		//+ <font-size="1..n"> </font-size>
		//+ <font-color="#rrggbb"> </font-color>
		//+ <color="#rrggbb"> </color>
		//+ <background-color="#rrggbb"> </background-color>
		//+ <letter-spacing="0"> </letter-spacing>
		//+ <word-spacing="0"> </word-spacing>
		//+ <line-height="1"> </line-height>
		//+ <text-align="left"> </text-align> //center, right, justify
        //+ <br> <br/>
        //+ &amp; &lt; &gt; &quot; &nbsp; &#xxxx;
        //+ <p> </p>
        //+ <strong> </strong>  //аналогично <b>
        //+ <em> </em>          //аналогично <i>
        //+ <strike> </strike>  //аналогично <s>
        //+ <ul> </ul> <li> </li>
        //+ <ol> </ol> <li> </li>
        
        //+ attribute "style"

        //+ color formats: #rrggbb #rgb rgb(r,g,b)
        //+ font formats: name name1,name2

        //! trimming not work properly for html

        private static Hashtable htmlEscapeSequence = null;
        private static Hashtable HtmlEscapeSequence
        {
            get
            {
                if (htmlEscapeSequence == null)
                {
                    htmlEscapeSequence = new Hashtable();

                    htmlEscapeSequence["&quot;"]    = (char)34;
                    htmlEscapeSequence["&amp;"]     = (char)38;
                    htmlEscapeSequence["&lt;"]      = (char)60;
                    htmlEscapeSequence["&gt;"]      = (char)62;

                    htmlEscapeSequence["&nbsp;"]    = (char)160;
                    htmlEscapeSequence["&iexcl;"]    = (char)161;
                    htmlEscapeSequence["&cent;"]    = (char)162;
                    htmlEscapeSequence["&pound;"]   = (char)163;
                    htmlEscapeSequence["&curren;"]  = (char)164;
                    htmlEscapeSequence["&yen;"]     = (char)165;
                    htmlEscapeSequence["&brvbar;"]  = (char)166;
                    htmlEscapeSequence["&sect;"]    = (char)167;
                    htmlEscapeSequence["&uml;"]     = (char)168;
                    htmlEscapeSequence["&copy;"]    = (char)169;
                    htmlEscapeSequence["&ordf;"]    = (char)170;
                    htmlEscapeSequence["&laquo;"]   = (char)171;
                    htmlEscapeSequence["&not;"]     = (char)172;
                    htmlEscapeSequence["&shy;"]     = (char)173;
                    htmlEscapeSequence["&reg;"]     = (char)174;
                    htmlEscapeSequence["&macr;"]    = (char)175;
                    htmlEscapeSequence["&deg;"]     = (char)176;
                    htmlEscapeSequence["&plusmn;"]  = (char)177;
                    htmlEscapeSequence["&sup2;"]    = (char)178;
                    htmlEscapeSequence["&sup3;"]    = (char)179;
                    htmlEscapeSequence["&acute;"]  = (char)180;
                    htmlEscapeSequence["&micro;"]   = (char)181;
                    htmlEscapeSequence["&para;"]    = (char)182;
                    htmlEscapeSequence["&middot;"]  = (char)183;
                    htmlEscapeSequence["&cedil;"]   = (char)184;
                    htmlEscapeSequence["&sup1;"]    = (char)185;
                    htmlEscapeSequence["&ordm;"]    = (char)186;
                    htmlEscapeSequence["&raquo;"]   = (char)187;
                    htmlEscapeSequence["&frac14;"]  = (char)188;
                    htmlEscapeSequence["&frac12;"]  = (char)189;
                    htmlEscapeSequence["&frac34;"]  = (char)190;
                    htmlEscapeSequence["&iquest;"]  = (char)191;
                    htmlEscapeSequence["&Agrave;"]  = (char)192;
                    htmlEscapeSequence["&Aacute;"]  = (char)193;
                    htmlEscapeSequence["&Acirc;"]   = (char)194;
                    htmlEscapeSequence["&Atilde;"]  = (char)195;
                    htmlEscapeSequence["&Auml;"]    = (char)196;
                    htmlEscapeSequence["&Aring;"]   = (char)197;
                    htmlEscapeSequence["&AElig;"]   = (char)198;
                    htmlEscapeSequence["&Ccedil;"]  = (char)199;
                    htmlEscapeSequence["&Egrave;"]  = (char)200;
                    htmlEscapeSequence["&Eacute;"]  = (char)201;
                    htmlEscapeSequence["&Ecirc;"]   = (char)202;
                    htmlEscapeSequence["&Euml;"]    = (char)203;
                    htmlEscapeSequence["&Igrave;"]  = (char)204;
                    htmlEscapeSequence["&Iacute;"]  = (char)205;
                    htmlEscapeSequence["&Icirc;"]   = (char)206;
                    htmlEscapeSequence["&Iuml;"]    = (char)207;
                    htmlEscapeSequence["&ETH;"]     = (char)208;
                    htmlEscapeSequence["&Ntilde;"]  = (char)209;
                    htmlEscapeSequence["&Ograve;"]  = (char)210;
                    htmlEscapeSequence["&Oacute;"]  = (char)211;
                    htmlEscapeSequence["&Ocirc;"]   = (char)212;
                    htmlEscapeSequence["&Otilde;"]  = (char)213;
                    htmlEscapeSequence["&Ouml;"]    = (char)214;
                    htmlEscapeSequence["&times;"]   = (char)215;
                    htmlEscapeSequence["&Oslash;"]  = (char)216;
                    htmlEscapeSequence["&Ugrave;"]  = (char)217;
                    htmlEscapeSequence["&Uacute;"]  = (char)218;
                    htmlEscapeSequence["&Ucirc;"]   = (char)219;
                    htmlEscapeSequence["&Uuml;"]    = (char)220;
                    htmlEscapeSequence["&Yacute;"]  = (char)221;
                    htmlEscapeSequence["&THORN;"]   = (char)222;
                    htmlEscapeSequence["&szlig;"]   = (char)223;
                    htmlEscapeSequence["&agrave;"]  = (char)224;
                    htmlEscapeSequence["&aacute;"]  = (char)225;
                    htmlEscapeSequence["&acirc;"]   = (char)226;
                    htmlEscapeSequence["&atilde;"]  = (char)227;
                    htmlEscapeSequence["&auml;"]    = (char)228;
                    htmlEscapeSequence["&aring;"]   = (char)229;
                    htmlEscapeSequence["&aelig;"]   = (char)230;
                    htmlEscapeSequence["&ccedil;"]  = (char)231;
                    htmlEscapeSequence["&egrave;"]  = (char)232;
                    htmlEscapeSequence["&eacute;"]  = (char)233;
                    htmlEscapeSequence["&ecirc;"]   = (char)234;
                    htmlEscapeSequence["&euml;"]    = (char)235;
                    htmlEscapeSequence["&igrave;"]  = (char)236;
                    htmlEscapeSequence["&iacute;"]  = (char)237;
                    htmlEscapeSequence["&icirc;"]   = (char)238;
                    htmlEscapeSequence["&iuml;"]    = (char)239;
                    htmlEscapeSequence["&eth;"]     = (char)240;
                    htmlEscapeSequence["&ntilde;"]  = (char)241;
                    htmlEscapeSequence["&ograve;"]  = (char)242;
                    htmlEscapeSequence["&oacute;"]  = (char)243;
                    htmlEscapeSequence["&ocirc;"]   = (char)244;
                    htmlEscapeSequence["&otilde;"]  = (char)245;
                    htmlEscapeSequence["&ouml;"]    = (char)246;
                    htmlEscapeSequence["&divide;"]  = (char)247;
                    htmlEscapeSequence["&oslash;"]  = (char)248;
                    htmlEscapeSequence["&ugrave;"]  = (char)249;
                    htmlEscapeSequence["&uacute;"]  = (char)250;
                    htmlEscapeSequence["&ucirc;"]   = (char)251;
                    htmlEscapeSequence["&uuml;"]    = (char)252;
                    htmlEscapeSequence["&yacute;"]  = (char)253;
                    htmlEscapeSequence["&thorn;"]   = (char)254;
                    htmlEscapeSequence["&yuml;"]    = (char)255;

                    htmlEscapeSequence["&OElig;"]   = (char)338;
                    htmlEscapeSequence["&oelig;"]   = (char)339;
                    htmlEscapeSequence["&Scaron;"]  = (char)352;
                    htmlEscapeSequence["&scaron;"]  = (char)353;
                    htmlEscapeSequence["&Yuml;"]    = (char)376;
                    htmlEscapeSequence["&fnof;"]    = (char)402;
                    htmlEscapeSequence["&circ;"]    = (char)710;
                    htmlEscapeSequence["&tilde;"]   = (char)732;

                    htmlEscapeSequence["&Alpha;"]   = (char)913;
                    htmlEscapeSequence["&Beta;"]    = (char)914;
                    htmlEscapeSequence["&Gamma;"]   = (char)915;
                    htmlEscapeSequence["&Delta;"]   = (char)916;
                    htmlEscapeSequence["&Epsilon;"] = (char)917;
                    htmlEscapeSequence["&Zeta;"]    = (char)918;
                    htmlEscapeSequence["&Eta;"]     = (char)919;
                    htmlEscapeSequence["&Theta;"]   = (char)920;
                    htmlEscapeSequence["&Iota;"]    = (char)921;
                    htmlEscapeSequence["&Kappa;"]   = (char)922;
                    htmlEscapeSequence["&Lambda;"]  = (char)923;
                    htmlEscapeSequence["&Mu;"]      = (char)924;
                    htmlEscapeSequence["&Nu;"]      = (char)925;
                    htmlEscapeSequence["&Xi;"]      = (char)926;
                    htmlEscapeSequence["&Omicron;"] = (char)927;
                    htmlEscapeSequence["&Pi;"]      = (char)928;
                    htmlEscapeSequence["&Rho;"]     = (char)929;
                    htmlEscapeSequence["&Sigma;"]   = (char)931;
                    htmlEscapeSequence["&Tau;"]     = (char)932;
                    htmlEscapeSequence["&Upsilon;"] = (char)933;
                    htmlEscapeSequence["&Phi;"]     = (char)934;
                    htmlEscapeSequence["&Chi;"]     = (char)935;
                    htmlEscapeSequence["&Psi;"]     = (char)936;
                    htmlEscapeSequence["&Omega;"]   = (char)937;
                    htmlEscapeSequence["&alpha;"]   = (char)945;
                    htmlEscapeSequence["&beta;"]    = (char)946;
                    htmlEscapeSequence["&gamma;"]   = (char)947;
                    htmlEscapeSequence["&delta;"]   = (char)948;
                    htmlEscapeSequence["&epsilon;"] = (char)949;
                    htmlEscapeSequence["&zeta;"]    = (char)950;
                    htmlEscapeSequence["&eta;"]     = (char)951;
                    htmlEscapeSequence["&theta;"]   = (char)952;
                    htmlEscapeSequence["&iota;"]    = (char)953;
                    htmlEscapeSequence["&kappa;"]   = (char)954;
                    htmlEscapeSequence["&lambda;"]  = (char)955;
                    htmlEscapeSequence["&mu;"]      = (char)956;
                    htmlEscapeSequence["&nu;"]      = (char)957;
                    htmlEscapeSequence["&xi;"]      = (char)958;
                    htmlEscapeSequence["&omicron;"] = (char)959;
                    htmlEscapeSequence["&pi;"]      = (char)960;
                    htmlEscapeSequence["&rho;"]     = (char)961;
                    htmlEscapeSequence["&sigmaf;"]  = (char)962;
                    htmlEscapeSequence["&sigma;"]   = (char)963;
                    htmlEscapeSequence["&tau;"]     = (char)964;
                    htmlEscapeSequence["&upsilon;"] = (char)965;
                    htmlEscapeSequence["&phi;"]     = (char)966;
                    htmlEscapeSequence["&chi;"]     = (char)967;
                    htmlEscapeSequence["&psi;"]     = (char)968;
                    htmlEscapeSequence["&omega;"]   = (char)969;
                    htmlEscapeSequence["&thetasym;"] = (char)977;
                    htmlEscapeSequence["&upsih;"]   = (char)978;
                    htmlEscapeSequence["&piv;"]     = (char)982;

                    htmlEscapeSequence["&ensp;"]    = (char)8194;
                    htmlEscapeSequence["&emsp;"]    = (char)8195;
                    htmlEscapeSequence["&thinsp;"]  = (char)8201;
                    htmlEscapeSequence["&zwnj;"]    = (char)8204;
                    htmlEscapeSequence["&zwj;"]     = (char)8205;
                    htmlEscapeSequence["&lrm;"]     = (char)8206;
                    htmlEscapeSequence["&rlm;"]     = (char)8207;
                    htmlEscapeSequence["&ndash;"]   = (char)8211;
                    htmlEscapeSequence["&mdash;"]   = (char)8212;
                    htmlEscapeSequence["&lsquo;"]   = (char)8216;
                    htmlEscapeSequence["&rsquo;"]   = (char)8217;
                    htmlEscapeSequence["&sbquo;"]   = (char)8218;
                    htmlEscapeSequence["&ldquo;"]   = (char)8220;
                    htmlEscapeSequence["&rdquo;"]   = (char)8221;
                    htmlEscapeSequence["&bdquo;"]   = (char)8222;
                    htmlEscapeSequence["&dagger;"]  = (char)8224;
                    htmlEscapeSequence["&Dagger;"]  = (char)8225;
                    htmlEscapeSequence["&bull;"]    = (char)8226;
                    htmlEscapeSequence["&hellip;"]  = (char)8230;
                    htmlEscapeSequence["&permil;"]  = (char)8240;
                    htmlEscapeSequence["&prime;"]   = (char)8242;
                    htmlEscapeSequence["&Prime;"]   = (char)8243;
                    htmlEscapeSequence["&lsaquo;"]  = (char)8249;
                    htmlEscapeSequence["&rsaquo;"]  = (char)8250;
                    htmlEscapeSequence["&oline;"]   = (char)8254;
                    htmlEscapeSequence["&frasl;"]   = (char)8260;
                    htmlEscapeSequence["&euro;"]    = (char)8364;
                    htmlEscapeSequence["&image;"]    = (char)8365;

                    htmlEscapeSequence["&weierp;"]  = (char)8472;
                    htmlEscapeSequence["&real;"]    = (char)8476;
                    htmlEscapeSequence["&trade;"]   = (char)8482;

                    htmlEscapeSequence["&alefsym;"] = (char)8501;
                    htmlEscapeSequence["&larr;"]    = (char)8592;
                    htmlEscapeSequence["&uarr;"]    = (char)8593;
                    htmlEscapeSequence["&rarr;"]    = (char)8594;
                    htmlEscapeSequence["&darr;"]    = (char)8595;
                    htmlEscapeSequence["&harr;"]    = (char)8596;
                    htmlEscapeSequence["&crarr;"]   = (char)8629;
                    htmlEscapeSequence["&lArr;"]    = (char)8656;
                    htmlEscapeSequence["&uArr;"]    = (char)8657;
                    htmlEscapeSequence["&rArr;"]    = (char)8658;
                    htmlEscapeSequence["&dArr;"]    = (char)8659;
                    htmlEscapeSequence["&hArr;"]    = (char)8660;
                    htmlEscapeSequence["&forall;"]  = (char)8704;
                    htmlEscapeSequence["&part;"]    = (char)8706;
                    htmlEscapeSequence["&exist;"]   = (char)8707;
                    htmlEscapeSequence["&empty;"]   = (char)8709;
                    htmlEscapeSequence["&nabla;"]   = (char)8711;
                    htmlEscapeSequence["&isin;"]    = (char)8712;
                    htmlEscapeSequence["&notin;"]   = (char)8713;
                    htmlEscapeSequence["&ni;"]      = (char)8715;
                    htmlEscapeSequence["&prod;"]    = (char)8719;
                    htmlEscapeSequence["&sum;"]     = (char)8721;
                    htmlEscapeSequence["&minus;"]   = (char)8722;
                    htmlEscapeSequence["&lowast;"]  = (char)8727;
                    htmlEscapeSequence["&radic;"]   = (char)8730;
                    htmlEscapeSequence["&prop;"]    = (char)8733;
                    htmlEscapeSequence["&infin;"]   = (char)8734;
                    htmlEscapeSequence["&ang;"]     = (char)8736;
                    htmlEscapeSequence["&and;"]     = (char)8743;
                    htmlEscapeSequence["&or;"]      = (char)8744;
                    htmlEscapeSequence["&cap;"]     = (char)8745;
                    htmlEscapeSequence["&cup;"]     = (char)8746;
                    htmlEscapeSequence["&int;"]     = (char)8747;
                    htmlEscapeSequence["&there4;"]  = (char)8756;
                    htmlEscapeSequence["&sim;"]     = (char)8764;
                    htmlEscapeSequence["&cong;"]    = (char)8773;
                    htmlEscapeSequence["&asymp;"]   = (char)8776;
                    htmlEscapeSequence["&ne;"]      = (char)8800;
                    htmlEscapeSequence["&equiv;"]   = (char)8801;
                    htmlEscapeSequence["&le;"]      = (char)8804;
                    htmlEscapeSequence["&ge;"]      = (char)8805;
                    htmlEscapeSequence["&sub;"]     = (char)8834;
                    htmlEscapeSequence["&sup;"]     = (char)8835;
                    htmlEscapeSequence["&nsub;"]    = (char)8836;
                    htmlEscapeSequence["&sube;"]    = (char)8838;
                    htmlEscapeSequence["&supe;"]    = (char)8839;
                    htmlEscapeSequence["&oplus;"]   = (char)8853;
                    htmlEscapeSequence["&otimes;"]  = (char)8855;
                    htmlEscapeSequence["&perp;"]    = (char)8869;
                    htmlEscapeSequence["&sdot;"]    = (char)8901;
                    htmlEscapeSequence["&lceil;"]   = (char)8968;
                    htmlEscapeSequence["&rceil;"]   = (char)8969;
                    htmlEscapeSequence["&lfloor;"]  = (char)8970;
                    htmlEscapeSequence["&rfloor;"]  = (char)8971;
                    htmlEscapeSequence["&lang;"]    = (char)9001;
                    htmlEscapeSequence["&rang;"]    = (char)9002;
                    htmlEscapeSequence["&loz;"]     = (char)9674;
                    htmlEscapeSequence["&spades;"]  = (char)9824;
                    htmlEscapeSequence["&clubs;"]   = (char)9827;
                    htmlEscapeSequence["&hearts;"]  = (char)9829;
                    htmlEscapeSequence["&diams;"]   = (char)9830;
                }
                return htmlEscapeSequence;
            }
        }
        #endregion

        //private static string StiHtmlBeginTag = "<StiHtml>";
        public static string StiForceWidthAlignTag = "<forcewidth>";
        private static Hashtable HtmlNameToColor = null;
        private static object lockHtmlNameToColor = new object();
        private static Hashtable OutlineTextMetricsCache = new Hashtable();
        private static object lockOutlineTextMetricsCache = new object();

        #region Enum StiHtmlTag
        public enum StiHtmlTag
        {
            None = 0,
            B,
            I,
            U,
            S,
            Sup,
            Sub,
            Font,
            FontName,
            FontSize,
            FontColor,
            Backcolor,
            LetterSpacing,
            WordSpacing,
            LineHeight,
            TextAlign,
            PClose,
            ListItem
        }
        #endregion

        #region Struct StiFontState
        public class StiFontState
        {
            public string FontName; //original font name
            public Font FontBase;
            public Font FontScaled;
            public int SuperOrSubscriptIndex;
            public int ParentFontIndex;
            public IntPtr hFont;
            public IntPtr hFontScaled;
            public IntPtr hScriptCache;
            public IntPtr hScriptCacheScaled;
            public double LineHeight;
            public double Ascend;
            public double Descend;
            public double ElipsisWidth = 0;
            public double EmValue;
        }
        #endregion

        #region Struct StiHtmlTagsState
        public struct StiHtmlTagsState
        {
            public bool Bold;
            public bool Italic;
            public bool Underline;
            public bool Strikeout;
            public float FontSize;
            public string FontName;
            public Color FontColor;
            public Color BackColor;
            public bool Subsript;
            public bool Superscript;
            public double LetterSpacing;
            public double WordSpacing;
            public double LineHeight;
            public StiTextHorAlignment TextAlign;
            public bool IsColorChanged;
            public bool IsBackcolorChanged;
            public StiHtmlTag Tag;
            public int Indent;

            public StiHtmlTagsState(bool bold, bool italic, bool underline, bool strikeout, float fontSize, string fontName, Color fontColor, Color backColor,
                bool superscript, bool subscript, double letterSpacing, double wordSpacing, double lineHeight, StiTextHorAlignment textAlign)
            {
                Bold = bold;
                Italic = italic;
                Underline = underline;
                Strikeout = strikeout;
                FontSize = fontSize;
                FontName = fontName;
                FontColor = fontColor;
                BackColor = backColor;
                Subsript = subscript;
                Superscript = superscript;
                LetterSpacing = letterSpacing;
                WordSpacing = wordSpacing;
                LineHeight = lineHeight;
                TextAlign = textAlign;
                IsColorChanged = false;
                IsBackcolorChanged = false;
                Tag = StiHtmlTag.None;
                Indent = 0;
            }

            public StiHtmlTagsState(StiHtmlTagsState state)
            {
                Bold = state.Bold;
                Italic = state.Italic;
                Underline = state.Underline;
                Strikeout = state.Strikeout;
                FontSize = state.FontSize;
                FontName = state.FontName;
                FontColor = state.FontColor;
                BackColor = state.BackColor;
                Subsript = state.Subsript;
                Superscript = state.Superscript;
                LetterSpacing = state.LetterSpacing;
                WordSpacing = state.WordSpacing;
                LineHeight = state.LineHeight;
                TextAlign = state.TextAlign;
                IsColorChanged = state.IsColorChanged;
                IsBackcolorChanged = state.IsBackcolorChanged;
                Tag = state.Tag;
                Indent = state.Indent;
            }
        }
        #endregion

        #region Struct StiHtmlState
        public struct StiHtmlState
        {
            public StiHtmlTagsState TS;
            public StringBuilder Text;
            public int FontIndex;
            public int PosBegin;
            internal List<StiHtmlTagsState> TagsStack;
            internal List<int> ListLevels;

            public StiHtmlState(StiHtmlTagsState ts, int posBegin)
            {
                TS = ts;
                Text = new StringBuilder();
                FontIndex = 0;
                PosBegin = posBegin;
                TagsStack = null;
                ListLevels = null;
            }

            public StiHtmlState(StiHtmlState state)
            {
                TS = new StiHtmlTagsState(state.TS);
                Text = new StringBuilder();
                FontIndex = 0;
                PosBegin = state.PosBegin;
                TagsStack = null;
                ListLevels = state.ListLevels;

                if (TS.Indent < 0)
                {
                    if (ListLevels != null)
                    {
                        TS.Indent = ListLevels.Count;
                    }
                    else
                    {
                        TS.Indent = 0;
                    }
                }
            }
        }
        #endregion

        #region Methods.ParseHtmlToStates
        public static List<StiHtmlState> ParseHtmlToStates(string inputHtml, StiHtmlState baseState)
        {
            return ParseHtmlToStates(inputHtml, baseState, false);
        }

        public static List<StiHtmlState> ParseHtmlToStates(string inputHtml, StiHtmlState baseState, bool storeStack)
        {
            var resultList = new List<StiHtmlState>();
            StiHtmlState state = baseState;
            var stack = new List<StiHtmlTagsState>();
            
            int pos = 0;
            bool lastSymIsSpace = false;
            if (inputHtml == null) inputHtml = string.Empty;
            while (pos < inputHtml.Length)
            {
                if (inputHtml[pos] != '<')
                {
                    char ch = inputHtml[pos];
                    if (char.IsWhiteSpace(ch))
                    {
                        if (!lastSymIsSpace)
                        {
                            state.Text.Append(' ');
                            lastSymIsSpace = true;
                        }
                    }
                    else
                    {
                        if (char.GetUnicodeCategory(ch) != UnicodeCategory.OtherNotAssigned)
                        {
                            state.Text.Append(ch);
                            lastSymIsSpace = false;
                        }
                    }
                    pos++;
                }
                else
                {
                    if (state.Text.Length > 0)
                    {
                        resultList.Add(state);
                        state = new StiHtmlState(state);
                        state.PosBegin = pos;
                        if (state.TS.Tag == StiHtmlTag.ListItem) state.TS.Tag = StiHtmlTag.None;
                    }
                    while ((pos < inputHtml.Length) && (inputHtml[pos] == '<'))
                    {
                        pos++;
                        int posEnd = pos;
                        while ((posEnd < inputHtml.Length) && (inputHtml[posEnd] != '>'))
                        {
                            posEnd++;
                        }
                        string tag = inputHtml.Substring(pos, posEnd - pos);
                        string tag2 = tag.Trim().ToLowerInvariant();
                        pos = posEnd;
                        pos++;
                        if (tag2 == "/p")
                        {
                            state.Text.Append('\n');
                            resultList.Add(state);
                            state = new StiHtmlState(state);
                            state.PosBegin = pos;
                            lastSymIsSpace = true;
                            state.Text.Append('\n');
                            state.TS.Tag = StiHtmlTag.PClose;
                            double storedLineHeight = state.TS.LineHeight;
                            state.TS.LineHeight = 0.8;
                            resultList.Add(state);
                            state = new StiHtmlState(state);
                            state.TS.LineHeight = storedLineHeight;
                            state.TS.Tag = StiHtmlTag.None;
                            state.PosBegin = pos;
                            if (state.TS.Indent > 0) state.Text.Append(GetIndentString(state.TS.Indent));
                        }
                        else if (tag2 == "p")
                        {
                            bool isPreviousTagPClose = (resultList.Count > 0) && ((resultList[resultList.Count - 1]).TS.Tag == StiHtmlTag.PClose);
                            if (!isPreviousTagPClose)
                            {
                                if ((resultList.Count > 1) || ((resultList.Count == 1) && ((resultList[0]).Text.ToString().Trim().Length != 0)))
                                {
                                    state.Text.Append('\n');
                                    resultList.Add(state);
                                    state = new StiHtmlState(state);
                                }
                                state.PosBegin = pos;
                                lastSymIsSpace = true;
                                state.Text.Append('\n');
                                double storedLineHeight = state.TS.LineHeight;
                                state.TS.LineHeight = 0.8;
                                resultList.Add(state);
                                state = new StiHtmlState(state);
                                state.TS.LineHeight = storedLineHeight;
                                state.PosBegin = pos;
                                if (state.TS.Indent > 0) state.Text.Append(GetIndentString(state.TS.Indent));
                            }
                        }
                        else if ((tag2 == "br") || (tag2 == "br/") || (tag2 == "br /"))
                        {
                            lastSymIsSpace = true;
                            state.Text.Append('\n');
                            resultList.Add(state);
                            state = new StiHtmlState(state);
                            state.PosBegin = pos;
                            if (state.TS.Indent > 0) state.Text.Append(GetIndentString(state.TS.Indent));
                        }
                        else if (tag2 == "li")
                        {
                            bool isPreviousTagListItem = (resultList.Count > 0) && ((resultList[resultList.Count - 1]).TS.Tag == StiHtmlTag.ListItem);
                            //if (state.TS.Tag == StiHtmlTag.ListItem)
                            //{
                            //    //after text break, if break point is inside the list
                            //    lastSymIsSpace = true;
                            //    state.PosBegin = pos;
                            //    state.Text.Append(GetIndentString(state.TS.Indent));
                            //    resultList.Add(state);
                            //    state = new StiHtmlState(state);
                            //    state.TS.Tag = StiHtmlTag.None;
                            //    state.PosBegin = pos;
                            //}
                            //else 
                            if (!isPreviousTagListItem)
                            {
                                lastSymIsSpace = true;
                                state.Text.Append('\n');
                                state.TS.Tag = StiHtmlTag.ListItem;
                                resultList.Add(state);
                                state = new StiHtmlState(state);
                                state.TS.Tag = StiHtmlTag.ListItem;
                                state.PosBegin = pos;
                                state.Text.Append(GetIndentString(state.TS.Indent));
                                resultList.Add(state);
                                state = new StiHtmlState(state);
                                state.TS.Tag = StiHtmlTag.None;
                                state.PosBegin = pos;
                            }
                            if (state.TS.Indent == 0) state.TS.Indent++;
                            if (state.ListLevels == null) state.ListLevels = new List<int>();
                            while (state.TS.Indent > state.ListLevels.Count) state.ListLevels.Add(0);
                            var state1 = resultList[resultList.Count - 1];
                            InsertMarker(state1.Text, state.ListLevels[state.TS.Indent - 1], state.TS.Indent);
                            if (state.ListLevels[state.TS.Indent - 1] > 0)
                            {
                                state.ListLevels = new List<int>(state.ListLevels);
                                state.ListLevels[state.TS.Indent - 1]++;
                            }
                            resultList[resultList.Count - 1] = state1;
                        }
                        else if (tag2 == "/li")
                        {
                            bool isPreviousTagListItem = (resultList.Count > 0) && ((resultList[resultList.Count - 1]).TS.Tag == StiHtmlTag.ListItem);
                            if (!isPreviousTagListItem)
                            {
                                lastSymIsSpace = true;
                                state.Text.Append('\n');
                                state.TS.Tag = StiHtmlTag.ListItem;
                                resultList.Add(state);
                                state = new StiHtmlState(state);
                                state.TS.Tag = StiHtmlTag.ListItem;
                                state.PosBegin = pos;
                                state.Text.Append(GetIndentString(state.TS.Indent));
                                resultList.Add(state);
                                state = new StiHtmlState(state);
                                state.TS.Tag = StiHtmlTag.None;
                                state.PosBegin = pos;
                            }
                        }
                        else if (tag2 == "ul")
                        {
                            bool isPreviousTagListItem = (resultList.Count > 0) && ((resultList[resultList.Count - 1]).TS.Tag == StiHtmlTag.ListItem);
                            if (!isPreviousTagListItem)
                            {
                                lastSymIsSpace = true;
                                state.Text.Append('\n');
                                state.TS.Tag = StiHtmlTag.ListItem;
                                resultList.Add(state);
                                state = new StiHtmlState(state);
                                state.TS.Tag = StiHtmlTag.ListItem;
                                state.PosBegin = pos;
                                state.Text.Append(GetIndentString(state.TS.Indent + 1));
                                state.TS.Indent++;
                                resultList.Add(state);
                                state = new StiHtmlState(state);
                                state.TS.Tag = StiHtmlTag.None;
                                state.PosBegin = pos;
                            }
                            else
                            {
                                state.TS.Indent++;
                                var state1 = resultList[resultList.Count - 1];
                                state1.Text.Append(GetIndentString(1));
                                state1.TS.Indent++;
                                resultList[resultList.Count - 1] = state1;
                            }
                            if (state.ListLevels == null) state.ListLevels = new List<int>();
                            while (state.ListLevels.Count < state.TS.Indent) state.ListLevels.Add(0);
                            state.ListLevels[state.TS.Indent - 1] = 1 - state.TS.Indent;
                        }
                        else if (tag2 == "/ul")
                        {
                            bool isPreviousTagListItem = (resultList.Count > 0) && ((resultList[resultList.Count - 1]).TS.Tag == StiHtmlTag.ListItem);
                            if (!isPreviousTagListItem)
                            {
                                lastSymIsSpace = true;
                                state.Text.Append('\n');
                                state.TS.Tag = StiHtmlTag.ListItem;
                                resultList.Add(state);
                                state = new StiHtmlState(state);
                                state.TS.Tag = StiHtmlTag.ListItem;
                                state.PosBegin = pos;
                                if (state.TS.Indent > 0) state.TS.Indent--;
                                state.Text.Append(GetIndentString(state.TS.Indent));
                                if (state.TS.Indent == 0) state.ListLevels = null;
                                resultList.Add(state);
                                state = new StiHtmlState(state);
                                state.TS.Tag = StiHtmlTag.None;
                                state.PosBegin = pos;
                            }
                            else
                            {
                                if (state.TS.Indent > 0) state.TS.Indent--; 
                                var state1 = resultList[resultList.Count - 1];
                                if (state1.TS.Indent > 0) state1.TS.Indent--;
                                state1.Text = new StringBuilder(GetIndentString(state1.TS.Indent));
                                if (state.TS.Indent == 0)
                                {
                                    state.ListLevels = null;
                                    state1.ListLevels = null;
                                }
                                resultList[resultList.Count - 1] = state1;
                            }                            
                        }
                        else if (tag2 == "ol")
                        {
                            bool isPreviousTagListItem = (resultList.Count > 0) && ((resultList[resultList.Count - 1]).TS.Tag == StiHtmlTag.ListItem);
                            if (!isPreviousTagListItem)
                            {
                                lastSymIsSpace = true;
                                state.Text.Append('\n');
                                state.TS.Tag = StiHtmlTag.ListItem;
                                resultList.Add(state);
                                state = new StiHtmlState(state);
                                state.TS.Tag = StiHtmlTag.ListItem;
                                state.PosBegin = pos;
                                state.Text.Append(GetIndentString(state.TS.Indent + 1));
                                state.TS.Indent++;
                                resultList.Add(state);
                                state = new StiHtmlState(state);
                                state.TS.Tag = StiHtmlTag.None;
                                state.PosBegin = pos;
                            }
                            else
                            {
                                state.TS.Indent++;
                                var state1 = resultList[resultList.Count - 1];
                                state1.Text.Append(GetIndentString(1));
                                state1.TS.Indent++;
                                resultList[resultList.Count - 1] = state1;
                            }
                            if (state.ListLevels == null) state.ListLevels = new List<int>();
                            while (state.ListLevels.Count < state.TS.Indent) state.ListLevels.Add(1);
                            state.ListLevels[state.TS.Indent - 1] = 1;

                            var state2 = resultList[resultList.Count - 1];
                            state2.ListLevels = state.ListLevels;
                            resultList[resultList.Count - 1] = state2;
                        }
                        else if (tag2 == "/ol")
                        {
                            bool isPreviousTagListItem = (resultList.Count > 0) && ((resultList[resultList.Count - 1]).TS.Tag == StiHtmlTag.ListItem);
                            if (!isPreviousTagListItem)
                            {
                                lastSymIsSpace = true;
                                state.Text.Append('\n');
                                state.TS.Tag = StiHtmlTag.ListItem;
                                resultList.Add(state);
                                state = new StiHtmlState(state);
                                state.TS.Tag = StiHtmlTag.ListItem;
                                state.PosBegin = pos;
                                if (state.TS.Indent > 0) state.TS.Indent--;
                                state.Text.Append(GetIndentString(state.TS.Indent));
                                if (state.TS.Indent == 0) state.ListLevels = null;
                                resultList.Add(state);
                                state = new StiHtmlState(state);
                                state.TS.Tag = StiHtmlTag.None;
                                state.PosBegin = pos;
                            }
                            else
                            {
                                if (state.TS.Indent > 0) state.TS.Indent--;
                                var state1 = resultList[resultList.Count - 1];
                                if (state1.TS.Indent > 0) state1.TS.Indent--;
                                state1.Text = new StringBuilder(GetIndentString(state1.TS.Indent));
                                if (state.TS.Indent == 0)
                                {
                                    state.ListLevels = null;
                                    state1.ListLevels = null;
                                }
                                resultList[resultList.Count - 1] = state1;
                            }
                        }
                        else
                        {
                            ParseHtmlTag(tag, ref state, stack, baseState);
                            if (storeStack)
                            {
                                state.TagsStack = new List<StiHtmlTagsState>();
                                foreach (StiHtmlTagsState tagState in stack)
                                {
                                    state.TagsStack.Add(new StiHtmlTagsState(tagState));
                                }
                            }
                        }
                    }
                }
            }
            if (state.Text.Length > 0) resultList.Add(state);
            if (resultList.Count == 0) resultList.Add(state);
            return resultList;
        }
        #endregion

        #region Methods.ParseHtmlTag
        private static void ParseHtmlTag(string tag, ref StiHtmlState state, List<StiHtmlTagsState> stack, StiHtmlState baseState)
        {
            //state.Tag.Append(tag);

            #region parse tag into pairs
            var attr = new List<DictionaryEntry>();
            int pos = 0;
            while ((pos < tag.Length) && (tag[pos] == ' ')) pos++;
            while (pos < tag.Length)
            {
                int pos2 = pos;
                var pair = new DictionaryEntry();
                while ((pos2 < tag.Length) && (tag[pos2] != ' ') && (tag[pos2] != '=')) pos2++;
                pair.Key = tag.Substring(pos, pos2 - pos).ToLowerInvariant();
                pos = pos2;
                while ((pos < tag.Length) && (tag[pos] == ' ')) pos++;
                if ((pos < tag.Length) && (tag[pos] == '='))
                {
                    pos++;
                    while ((pos < tag.Length) && (tag[pos] == ' ')) pos++;
                    if (pos < tag.Length)
                    {
                        if (tag[pos] == '"')
                        {
                            pos++;
                            pos2 = pos;
                            while ((pos2 < tag.Length) && (tag[pos2] != '"')) pos2++;
                            pair.Value = tag.Substring(pos, pos2 - pos);
                            pos = pos2;
                            pos++;
                        }
                        else
                        {
                            pos2 = pos;
                            while ((pos2 < tag.Length) && (tag[pos2] != ' ')) pos2++;
                            pair.Value = tag.Substring(pos, pos2 - pos);
                            pos = pos2;
                        }
                    }
                }
                while ((pos < tag.Length) && (tag[pos] == ' ')) pos++;
                attr.Add(pair);
            }
            #endregion

            string delimiter = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var htmlTag = StiHtmlTag.None;
            bool closeTag = false;

            #region parse pairs
            if (attr.Count > 0)
            {
                var tsLast = new StiHtmlTagsState(state.TS);
                DictionaryEntry de = attr[0];
                switch ((string)de.Key)
                {
                    case "b":
                    case "strong":
                        state.TS.Bold = true;
                        htmlTag = StiHtmlTag.B;
                        break;
                    case "/b":
                    case "/strong":
                        state.TS.Bold = false;
                        htmlTag = StiHtmlTag.B;
                        closeTag = true;
                        break;

                    case "i":
                    case "em":
                        state.TS.Italic = true;
                        htmlTag = StiHtmlTag.I;
                        break;
                    case "/i":
                    case "/em":
                        state.TS.Italic = false;
                        htmlTag = StiHtmlTag.I;
                        closeTag = true;
                        break;

                    case "u":
                        state.TS.Underline = true;
                        htmlTag = StiHtmlTag.U;
                        break;
                    case "/u":
                        state.TS.Underline = false;
                        htmlTag = StiHtmlTag.U;
                        closeTag = true;
                        break;

                    case "s":
                    case "strike":
                        state.TS.Strikeout = true;
                        htmlTag = StiHtmlTag.S;
                        break;
                    case "/s":
                    case "/strike":
                        state.TS.Strikeout = false;
                        htmlTag = StiHtmlTag.S;
                        closeTag = true;
                        break;

                    case "sup":
                        state.TS.Superscript = true;
                        state.TS.Subsript = false;
                        htmlTag = StiHtmlTag.Sup;
                        break;
                    case "/sup":
                        state.TS.Superscript = false;
                        htmlTag = StiHtmlTag.Sup;
                        closeTag = true;
                        break;

                    case "sub":
                        state.TS.Subsript = true;
                        state.TS.Superscript = false;
                        htmlTag = StiHtmlTag.Sub;
                        //closeTag = true;
                        break;
                    case "/sub":
                        state.TS.Subsript = false;
                        htmlTag = StiHtmlTag.Sub;
                        closeTag = true;
                        break;

                    case "letter-spacing":
                        double letterSpacing = 0;

                        if ((string) de.Value != "normal")
                        {
                            if (!double.TryParse(((string) de.Value).Replace(',', '.').Replace(".", delimiter), out letterSpacing))
                            {
                                letterSpacing = 0;
                            }
                        }

                        state.TS.LetterSpacing = letterSpacing;
                        htmlTag = StiHtmlTag.LetterSpacing;
                        //closeTag = true;
                        break;
                    case "/letter-spacing":
                        htmlTag = StiHtmlTag.LetterSpacing;
                        closeTag = true;
                        break;

                    case "word-spacing":
                        double wordSpacing = 0;
                        if ((string) de.Value != "normal")
                        {
                            if (!double.TryParse(((string) de.Value).Replace(',', '.').Replace(".", delimiter), out wordSpacing))
                            {
                                wordSpacing = 0;
                            }
                        }

                        state.TS.WordSpacing = wordSpacing;
                        htmlTag = StiHtmlTag.WordSpacing;
                        break;
                    case "/word-spacing":
                        htmlTag = StiHtmlTag.WordSpacing;
                        closeTag = true;
                        break;

                    case "line-height":
                        double lineHeight = 1;
                        if ((string) de.Value != "normal")
                        {
                            if (!double.TryParse(((string) de.Value).Replace(',', '.').Replace(".", delimiter), out lineHeight))
                            {
                                lineHeight = 1;
                            }
                        }

                        if (lineHeight <= 0) lineHeight = 1;
                        state.TS.LineHeight = lineHeight;
                        htmlTag = StiHtmlTag.LineHeight;
                        break;
                    case "/line-height":
                        htmlTag = StiHtmlTag.LineHeight;
                        closeTag = true;
                        break;

                    case "text-align":
                        try
                        {
                            string align = ((string)de.Value).ToLowerInvariant();
                            if (align == "left") state.TS.TextAlign = StiTextHorAlignment.Left;
                            if (align == "right") state.TS.TextAlign = StiTextHorAlignment.Right;
                            if (align == "center") state.TS.TextAlign = StiTextHorAlignment.Center;
                            if (align == "justify") state.TS.TextAlign = StiTextHorAlignment.Width;
                        }
                        catch
                        {
                        }
                        htmlTag = StiHtmlTag.TextAlign;
                        break;
                    case "/text-align":
                        htmlTag = StiHtmlTag.TextAlign;
                        closeTag = true;
                        break;

                    case "font":
                        if (attr.Count > 1)
                        {
                            #region parse font attributes
                            for (int indexDE = 1; indexDE < attr.Count; indexDE++)
                            {
                                DictionaryEntry def = attr[indexDE];
                                switch ((string)def.Key)
                                {
                                    case "color":
                                        try
                                        {
                                            state.TS.FontColor = ParseColor((string)def.Value);
                                            state.TS.IsColorChanged = true;
                                        }
                                        catch
                                        {
                                        }
                                        break;

                                    case "face":
                                    case "family":
                                    case "name":
                                        try
                                        {
                                            state.TS.FontName = (string)def.Value;
                                        }
                                        catch
                                        {
                                        }
                                        break;

                                    case "size":
                                        float ffontSize = ParseFontSize((string)def.Value, delimiter);
                                        state.TS.FontSize = ffontSize;
                                        break;
                                }
                            }
                            #endregion
                        }
                        htmlTag = StiHtmlTag.Font;
                        break;

                    case "/font":
                        htmlTag = StiHtmlTag.Font;
                        closeTag = true;
                        break;

                    case "font-face":
                    case "font-family":
                    case "font-name":
                        try
                        {
                            state.TS.FontName = (string)de.Value;
                        }
                        catch
                        {
                        }
                        htmlTag = StiHtmlTag.FontName;
                        break;
                    case "/font-face":
                    case "/font-family":
                    case "/font-name":
                        htmlTag = StiHtmlTag.FontName;
                        closeTag = true;
                        break;

                    case "font-size":
                        float fontSize = ParseFontSize((string)de.Value, delimiter);
                        state.TS.FontSize = fontSize;
                        htmlTag = StiHtmlTag.FontSize;
                        break;
                    case "/font-size":
                        htmlTag = StiHtmlTag.FontSize;
                        closeTag = true;
                        break;

                    case "color":
                    case "font-color":
                        try
                        {
                            state.TS.FontColor = ParseColor((string)de.Value);
                            state.TS.IsColorChanged = true;
                        }
                        catch
                        {
                        }
                        htmlTag = StiHtmlTag.FontColor;
                        break;
                    case "/color":
                    case "/font-color":
                        htmlTag = StiHtmlTag.FontColor;
                        closeTag = true;
                        break;

                    case "background-color":
                        try
                        {
                            state.TS.BackColor = ParseColor((string)de.Value);
                            state.TS.IsBackcolorChanged = true;
                        }
                        catch
                        {
                        }
                        htmlTag = StiHtmlTag.Backcolor;
                        break;
                    case "/background-color":
                        htmlTag = StiHtmlTag.Backcolor;
                        closeTag = true;
                        break;

                    case "stihtml":
                        stack.Clear();
                        try
                        {
                            if (attr.Count > 1 && attr[1].Key != null)
                            {
                                stack.AddRange(StringToStack((string)attr[1].Key, baseState.TS));
                            }
                        }
                        catch
                        {
                        }
                        break;

                    case "stihtml2":
                        try
                        {
                            if (attr.Count > 2 && attr[2].Key != null)
                            {
                                state.ListLevels = StringToListLevels((string)attr[2].Key);
                                if (state.ListLevels != null) state.TS.Indent = state.ListLevels.Count;
                                //state.TS.Tag = StiHtmlTag.ListItem;
                                int lineInfoIndent = int.Parse(attr[1].Key.ToString());
                                if (lineInfoIndent > 0) state.TS.Indent = -lineInfoIndent;
                            }
                        }
                        catch
                        {
                        }
                        break;

                    //case "br":
                    //    state.Text.Append('\n');
                    //    break;

                }

                if (closeTag)
                {
                    if (stack.Count > 0)
                    {
                        for (int index = stack.Count - 1; index >= 0; index--)
                        {
                            StiHtmlTagsState ts = stack[index];
                            if (ts.Tag == htmlTag)
                            {
                                state.TS = ts;
                                stack.RemoveRange(index, stack.Count - index);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (htmlTag != StiHtmlTag.None)
                    {
                        tsLast.Tag = htmlTag;
                        stack.Add(tsLast);
                    }
                }

                if (!closeTag && attr.Count > 1)
                {
                    for (int indexDE = 1; indexDE < attr.Count; indexDE++)
                    {
                        var def = attr[indexDE];
                        if ((string)def.Key == "style")
                        {
                            ParseStyleAttribute((string)def.Value, ref state);
                        }
                    }
                }
            }
            #endregion
        }

        private static void ParseStyleAttribute(string style, ref StiHtmlState state)
        {
            if (string.IsNullOrEmpty(style)) return;
            string delimiter = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string[] pairs = style.Split(new char[] { ';' });
            foreach (string pair in pairs)
            {
                string[] parts = pair.Split(new char[] { ':' });
                bool hasValue = parts.Length > 1;
                string key = parts[0].Trim();
                string value = hasValue ? parts[1].Trim() : null;
                switch (key)
                {
                    case "color":
                        try
                        {
                            state.TS.FontColor = ParseColor(value);
                            state.TS.IsColorChanged = true;
                        }
                        catch
                        {
                        }
                        break;

                    case "background-color":
                        try
                        {
                            state.TS.BackColor = ParseColor(value);
                            state.TS.IsBackcolorChanged = true;
                        }
                        catch
                        {
                        }
                        break;

                    case "text-decoration":
                        if (value == "underline") state.TS.Underline = true;
                        if (value == "line-through") state.TS.Strikeout = true;
                        if (value == "none")
                        {
                            state.TS.Underline = false;
                            state.TS.Strikeout = false;
                        }
                        break;

                    case "font-weight":
                        state.TS.Bold = (value == "bold" || value == "bolder" || value == "600" || value == "700" || value == "800" || value == "900");
                        break;

                    case "font-style":
                        if (value == "normal") state.TS.Italic = false;
                        if (value == "italic" || value == "oblique") state.TS.Italic = true;
                        break;

                    case "vertical-align":
                        if (value == "baseline")
                        {
                            state.TS.Subsript = false;
                            state.TS.Superscript = false;
                        }
                        if (value == "sub")
                        {
                            state.TS.Subsript = true;
                            state.TS.Superscript = false;
                        }
                        if (value == "super")
                        {
                            state.TS.Subsript = false;
                            state.TS.Superscript = true;
                        }
                        break;

                    case "letter-spacing":
                        double letterSpacing = 0;
                        if (value != "normal")
                        {
                            if (!double.TryParse(value.Replace(',', '.').Replace(".", delimiter), out letterSpacing))
                            {
                                letterSpacing = 0;
                            }
                        }
                        
                        state.TS.LetterSpacing = letterSpacing;
                        break;

                    case "word-spacing":
                        double wordSpacing = 0;
                        if (value != "normal")
                        {
                            if (!double.TryParse(value.Replace(',', '.').Replace(".", delimiter), out wordSpacing))
                            {
                                wordSpacing = 0;
                            }
                        }
                        
                        state.TS.WordSpacing = wordSpacing;
                        break;

                    case "line-height":
                        double lineHeight = 1;
                        if (value != "normal")
                        {
                            if (!double.TryParse(value.Replace(',', '.').Replace(".", delimiter), out lineHeight))
                            {
                                lineHeight = 1;
                            }
                        }

                        if (lineHeight <= 0) lineHeight = 1;
                        state.TS.LineHeight = lineHeight;
                        break;

                    case "text-align":
                        string align = value.ToLowerInvariant();
                        if (align == "left") state.TS.TextAlign = StiTextHorAlignment.Left;
                        if (align == "right") state.TS.TextAlign = StiTextHorAlignment.Right;
                        if (align == "center") state.TS.TextAlign = StiTextHorAlignment.Center;
                        if (align == "justify") state.TS.TextAlign = StiTextHorAlignment.Width;
                        break;

                }
            }
        }
        #endregion

        #region Methods.GetFontWidth

        private static Hashtable hashFonts = null;

        private static ushort[] GetFontWidth(Font font)
        {
            string fontName = font.Name + (font.Bold ? ",bold" : "") + (font.Italic ? ",italic" : "");
            if (hashFonts == null) hashFonts = new Hashtable();
            object data = hashFonts[fontName];
            if (data == null)
            {
                try
                {
                    data = GetGdiFontWidth(font);
                }
                catch
                {
                    data = new ushort[0];
                }
                hashFonts[fontName] = data;
            }
            return (ushort[])data;
        }

        [System.Security.SecuritySafeCritical]
        private static ushort[] GetGdiFontWidth(Font font)
        {
            ushort[] Widths = new ushort[0];
            Graphics gr = Graphics.FromHwnd(IntPtr.Zero);
            try
            {
                IntPtr hdc = gr.GetHdc();
                try
                {
                    using (Font tempFont = StiFontCollection.CreateFont(font.Name, MaxFontSize, font.Style))
                    {
                        IntPtr hFont = tempFont.ToHfont();
                        try
                        {
                            IntPtr oldObj = SelectObject(hdc, hFont);

                            //Symbols
                            StringBuilder sb = new StringBuilder();
                            for (int index = 0; index < 0x10000 - 1; index++)
                            {
                                sb.Append((char) index);
                            }
                            string str = sb.ToString();
                            int count = str.Length;
                            ushort[] rtcode = new ushort[count];
                            uint res = GetGlyphIndices(hdc, str, count, rtcode, 1);
                            if (res == GDI_ERROR) ThrowError(100);

                            //Glyphs
                            int count1 = 0;
                            for (int index = 0; index < count; index++)
                            {
                                if (rtcode[index] != 0xFFFF && rtcode[index] > count1) count1 = rtcode[index];
                            }
                            ushort[] rtcode1 = new ushort[count1];
                            for (ushort index = 0; index < count1; index++)
                            {
                                rtcode1[index] = index;
                            }

                            //Widths
                            ABC[] abc = new ABC[count1];
                            bool gdiErr = GetCharABCWidthsI(hdc, 0, (uint) count1, rtcode1, abc);
                            if (gdiErr == false) ThrowError(101);

                            ushort[] tempWidths = new ushort[count1];
                            for (int index = 0; index < count1; index++)
                            {
                                //double fl = (abc[index].abcA + abc[index].abcB + abc[index].abcC) * FontFactor * GraphicsScale;
                                //double fl = (abc[index].abcA + abc[index].abcB + abc[index].abcC) * GraphicsScale;
                                double fl = (abc[index].abcA + abc[index].abcB + abc[index].abcC);
                                tempWidths[index] = (ushort) (Math.Round(fl));
                            }

                            int pos = count1 - 1;
                            while (pos > 0 && tempWidths[pos - 1] == tempWidths[count1 - 1]) pos--;
                            pos++;

                            Widths = new ushort[pos];
                            for (int index = 0; index < pos; index++)
                            {
                                Widths[index] = tempWidths[index];
                            }

                            SelectObject(hdc, oldObj);
                        }
                        finally
                        {
                            bool error = DeleteObject(hFont);
                            if (!error) ThrowError(102);
                        }
                    }
                }
                finally
                {
                    gr.ReleaseHdc(hdc);
                }
            }
            finally
            {
                gr.Dispose();
            }
            return Widths;
        }
        #endregion

        #region Dll import

        [DllImport("gdi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		internal static extern uint GetOutlineTextMetrics(IntPtr hdc, uint cbData, IntPtr lpOTM);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int DrawTextExW(IntPtr hDC, string lpszString, int nCount, ref RECT lpRect, int nFormat, [In, Out] DRAWTEXTPARAMS param);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern bool DeleteObject(IntPtr objectHandle);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		private static extern int SetTextColor(IntPtr hDC, int crColor);

		[DllImport("gdi32.dll", EntryPoint = "SetBkMode", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		private static extern int SetBkMode(IntPtr hDC, int nBkMode);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		private static extern int SetBkColor(IntPtr hDC, int clr);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		private static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		private static extern int GetClipRgn(IntPtr hDC, IntPtr hRgn);

		[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
		private static extern int SelectClipRgn(IntPtr hDC, IntPtr hRgn);

		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern bool GetWorldTransform(
			IntPtr hdc,
			out XFORM lpXform
			);

		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern bool SetWorldTransform(
			IntPtr hdc,
			ref XFORM lpXform
			);

		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern bool ModifyWorldTransform(
			IntPtr hdc,
			ref XFORM lpXform,
			UInt32 iMode
			);

		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern int SetGraphicsMode(
			IntPtr hdc,
			int iMode
			);

		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern int SetMapMode(
			IntPtr hdc,		// handle to device context
			int fnMapMode	// new mapping mode
			);

		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern bool GetWindowExtEx(
			IntPtr hdc,		// handle to device context
			out SIZE lpSize	// viewport dimensions
			);

		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern bool SetWindowExtEx(
			IntPtr hdc,		// handle to device context
			int nXExtent,	// new horizontal window extent
			int nYExtent,	// new vertical window extent
			out SIZE lpSize	// original viewport extent
			);

		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern bool GetViewportExtEx(
			IntPtr hdc,		// handle to device context
			out SIZE lpSize	// viewport dimensions
			);

		[DllImport("gdi32.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern bool SetViewportExtEx(
			IntPtr hdc,		// handle to device context
			int nXExtent,	// new horizontal viewport extent
			int nYExtent,	// new vertical viewport extent
			out SIZE lpSize	// original viewport extent
			);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetCharABCWidthsI(
            IntPtr hdc,				// handle to DC
            uint giFirst,			// first glyph index in range
            uint cgi,				// count of glyph indices in range
            [In, Out] ushort[] pgi,  // array of glyph indices
            [In, Out] ABC[] lpabc); // array of character widths

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern uint GetGlyphIndices(
            IntPtr hdc,				// handle to DC
            string lpstr,			// string to convert
            int c,					// number of characters in string
            [In, Out] ushort[] pgi,	// array of glyph indices
            uint fl);			// glyph options


		[DllImport("usp10.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern int ScriptItemize(
			string pwcInChars,
			int cInChars,
			int cMaxItems,
			ref SCRIPT_CONTROL psControls,
			ref SCRIPT_STATE psState,
			//ref SCRIPT_ITEM scriptItemList,
            IntPtr scriptItemList,
			out int scriptItemCount
			);

		//E_OUTOFMEMORY result если не хватило scriptItemList

		//HRESULT WINAPI ScriptItemize(
		//    const WCHAR *pwcInChars,
		//    int cInChars,
		//    int cMaxItems,
		//    const SCRIPT_CONTROL *psControl,
		//    const SCRIPT_STATE *psState,
		//    SCRIPT_ITEM *scriptItemList,
		//    int *scriptItemCount
		//);

		[DllImport("usp10.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern int ScriptLayout(
			int cRuns,
			byte[] pbLevel,
			int[] piVisualToLogical,
			int[] piLogicalToVisual
			);

		//HRESULT WINAPI ScriptLayout(
		//  int cRuns, 
		//  const BYTE *pbLevel, 
		//  int *piVisualToLogical, 
		//  int *piLogicalToVisual 
		//);

		[DllImport("usp10.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern int ScriptShape(
			IntPtr hdc,				// handle to DC
			ref IntPtr psc,         // SCRIPT_CACHE
			string pwcChars,
			int cChars,
			int cMaxGlyphs,
			ref SCRIPT_ANALYSIS psa,
			ushort[] pwOutGlyphs,
			ushort[] pwLogClust,
			//ref SCRIPT_VISATTR psva,
            IntPtr psva,
			out int pcGlyphs
			);

		//HRESULT WINAPI ScriptShape(
		//  HDC hdc, 
		//  SCRIPT_CACHE *psc, 
		//  const WCHAR *pwcChars, 
		//  int cChars, 
		//  int cMaxGlyphs, 
		//  SCRIPT_ANALYSIS *psa, 
		//  WORD *pwOutGlyphs, 
		//  WORD *pwLogClust, 
		//  SCRIPT_VISATTR *psva, 
		//  int *pcGlyphs 
		//);

		[DllImport("usp10.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern int ScriptPlace(
			IntPtr hdc,				// handle to DC
			ref IntPtr psc,         // SCRIPT_CACHE
			ushort[] pwGlyphs,
			int pcGlyphs,
			//ref SCRIPT_VISATTR psva,
            IntPtr psva,
			ref SCRIPT_ANALYSIS psa,
			int[] piAdvance,
			//ref GOFFSET pGoffset,
            IntPtr pGoffset,
			out ABC pABC
			);

		//HRESULT WINAPI ScriptPlace(
		//  HDC hdc, 
		//  SCRIPT_CACHE *psc, 
		//  const WORD *pwGlyphs, 
		//  int cGlyphs, 
		//  const SCRIPT_VISATTR *psva, 
		//  SCRIPT_ANALYSIS *psa, 
		//  int *piAdvance, 
		//  GOFFSET *pGoffset, 
		//  ABC *pABC 
		//);

		[DllImport("usp10.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern int ScriptTextOut(
			IntPtr hdc,				// handle to DC
			ref IntPtr psc,         // SCRIPT_CACHE
			int x,
			int y,
			uint fuOptions,
			ref RECT lprc,
			ref SCRIPT_ANALYSIS psa,
			IntPtr pwcReserved,
			int iReserved,
			ushort[] pwGlyphs,
			int cGlyphs,
			int[] piAdvance,
			IntPtr piJustify,
			//ref GOFFSET pGoffset
            IntPtr pGoffset
			);

		//HRESULT WINAPI ScriptTextOut(
		//  const HDC hdc, 
		//  SCRIPT_CACHE *psc, 
		//  int x, 
		//  int y, 
		//  UINT fuOptions, 
		//  const RECT *lprc, 
		//  const SCRIPT_ANALYSIS *psa, 
		//  const WCHAR *pwcReserved, 
		//  int iReserved, 
		//  const WORD *pwGlyphs, 
		//  int cGlyphs, 
		//  const int *piAdvance, 
		//  const int *piJustify, 
		//  const GOFFSET *pGoffset 
		//);

		[DllImport("usp10.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern int ScriptFreeCache(ref IntPtr psc);

		//HRESULT WINAPI ScriptFreeCache(
		//  SCRIPT_CACHE *psc 
		//);

		[DllImport("usp10.dll", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern int ScriptBreak(
			string pwcInChars,
			int cInChars,
			ref SCRIPT_ANALYSIS psa,
			//ref SCRIPT_LOGATTR psla
            IntPtr psla
			);

		//HRESULT WINAPI ScriptBreak( 
		//  const WCHAR *pwcChars, 
		//  int cChars, 
		//  const SCRIPT_ANALYSIS *psa, 
		//  SCRIPT_LOGATTR *psla 
		//);

        #endregion

        #region Methods.ThrowError
        private static void ThrowError(int step)
        {
            Win32Exception myEx = new Win32Exception(Marshal.GetLastWin32Error());
            throw new Exception(string.Format("TextRender error at step {0}, code #{1:X8}: {2}", step, myEx.ErrorCode, myEx.Message));
        }
        private static void ThrowError(int step, int error)
        {
            Win32Exception myEx = new Win32Exception(Marshal.GetLastWin32Error());
            throw new Exception(string.Format("TextRender error at step {0}, code #{1:X8}(#{2:X8}): {3}", step, myEx.ErrorCode, error, myEx.Message));
        }
        #endregion

        #region Structures

        private static int sizeofScriptItem = Marshal.SizeOf(typeof(SCRIPT_ITEM));
        private static int sizeofScriptVisattr = Marshal.SizeOf(typeof(SCRIPT_VISATTR));
        private static int sizeofGoffset = Marshal.SizeOf(typeof(GOFFSET));

        #region struct GOFFSET
        //typedef struct tagGOFFSET {
		//  LONG  du;
		//  LONG  dv;
		//} GOFFSET;

		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
		private struct GOFFSET
		{
            [FieldOffset(0)]
			public Int32 du;
            [FieldOffset(4)]
            public Int32 dv;
		}
        #endregion

        #region struct SCRIPT_VISATTR
		//typedef struct tag_SCRIPT_VISATTR { 
		//  WORD uJustification :4; 
		//  WORD fClusterStart :1; 
		//  WORD fDiacritic :1; 
		//  WORD fZeroWidth :1; 
		//  WORD fReserved :1; 
		//  WORD fShapeReserved :8; 
		//} SCRIPT_VISATTR;

		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
		private struct SCRIPT_VISATTR
		{
            [FieldOffset(0)]
			private UInt16 packed;

			internal ushort uJustification
			{
				get { return (ushort)(packed & 0x000F); }
			}
			internal ushort fClusterStart
			{
				get { return (ushort)((packed & 0x0010) >> 4); }
			}
			internal ushort fDiacritic
			{
				get { return (ushort)((packed & 0x0020) >> 5); }
			}
			internal ushort fZeroWidth
			{
				get { return (ushort)((packed & 0x0040) >> 6); }
			}
			internal ushort fReserved
			{
				get { return (ushort)((packed & 0x0080) >> 7); }
			}
			internal ushort fShapeReserved
			{
				get { return (ushort)((packed & 0xFF00) >> 8); }
			}
		}
        #endregion

        #region struct SCRIPT_STATE
		//typedef struct tag_SCRIPT_STATE { 
		//  WORD uBidiLevel :5; 
		//  WORD fOverrideDirection :1; 
		//  WORD fInhibitSymSwap :1; 
		//  WORD fCharShape :1; 
		//  WORD fDigitSubstitute :1; 
		//  WORD fInhibitLigate :1; 
		//  WORD fDisplayZWG :1; 
		//  WORD fArabicNumContext :1; 
		//  WORD fGcpClusters :1; 
		//  WORD fReserved :1; 
		//  WORD fEngineReserved :2; 
		//} SCRIPT_STATE;

		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
			private struct SCRIPT_STATE
		{
			//internal ushort packed;
            [FieldOffset(0)]
            internal UInt16 packed;

			internal ushort uBidiLevel
			{
				get { return (ushort)(packed & 0x001F); }
				set { packed = (ushort)((packed & 0xFFE0) | (value & 0x001F)); }
			}
			internal ushort fOverrideDirection
			{
				get { return (ushort)((packed & 0x0020) >> 5); }
			}
			internal ushort fInhibitSymSwap
			{
				get { return (ushort)((packed & 0x0040) >> 6); }
			}
			internal ushort fCharShape
			{
				get { return (ushort)((packed & 0x0080) >> 7); }
			}
			internal ushort fDigitSubstitute
			{
				get { return (ushort)((packed & 0x0100) >> 8); }
			}
			internal ushort fInhibitLigate
			{
				get { return (ushort)((packed & 0x0200) >> 9); }
			}
			internal ushort fDisplayZWG
			{
				get { return (ushort)((packed & 0x0400) >> 10); }
			}
			internal ushort fArabicNumContext
			{
				get { return (ushort)((packed & 0x0800) >> 11); }
			}
			internal ushort fGcpClusters
			{
				get { return (ushort)((packed & 0x1000) >> 12); }
			}
			internal ushort fReserved
			{
				get { return (ushort)((packed & 0x2000) >> 13); }
			}
			internal ushort fEngineReserved
			{
				get { return (ushort)((packed & 0xC000) >> 14); }
			}
		}
        #endregion

        #region struct SCRIPT_ANALYSIS
		//typedef struct tag_SCRIPT_ANALYSIS {
		//  WORD eScript      :10; 
		//  WORD fRTL          :1; 
		//  WORD fLayoutRTL    :1; 
		//  WORD fLinkBefore   :1; 
		//  WORD fLinkAfter    :1; 
		//  WORD fLogicalOrder :1; 
		//  WORD fNoGlyphIndex :1; 
		//  SCRIPT_STATE s ; 
		//} SCRIPT_ANALYSIS;

		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
		private struct SCRIPT_ANALYSIS
		{
			//internal ushort packed;
            [FieldOffset(0)]
            internal UInt16 packed;
            [FieldOffset(2)]
            public SCRIPT_STATE s;

			internal ushort eScript
			{
				get { return (ushort)(packed & 0x03FF); }
			}
			internal bool fRTL
			{
				get { return (((packed & 0x0400) >> 10) > 0 ? true : false); }
			}
			internal ushort fLayoutRTL
			{
				get { return (ushort)((packed & 0x0800) >> 11); }
			}
			internal ushort fLinkBefore
			{
				get { return (ushort)((packed & 0x1000) >> 12); }
			}
			internal ushort fLinkAfter
			{
				get { return (ushort)((packed & 0x2000) >> 13); }
			}
			internal ushort fLogicalOrder
			{
				get { return (ushort)((packed & 0x4000) >> 14); }
			}
			internal ushort fNoGlyphIndex
			{
				get { return (ushort)((packed & 0x8000) >> 15); }
			}
		}
        #endregion

        #region struct SCRIPT_ITEM
		[StructLayout(LayoutKind.Explicit, Pack=8, CharSet = CharSet.Auto)]
		private struct SCRIPT_ITEM
		{
            //internal int iCharPos;
            [FieldOffset(0)]
            internal Int32 iCharPos;
            [FieldOffset(4)]
            internal SCRIPT_ANALYSIS a;
		}
        #endregion

        #region struct SCRIPT_CONTROL
		//typedef struct tag_SCRIPT_CONTROL { 
		//  DWORD uDefaultLanguage :16; 
		//  DWORD fContextDigits :1; 
		//  DWORD fInvertPreBoundDir :1; 
		//  DWORD fInvertPostBoundDir :1; 
		//  DWORD fLinkStringBefore :1; 
		//  DWORD fLinkStringAfter :1; 
		//  DWORD fNeutralOverride :1; 
		//  DWORD fNumericOverride :1; 
		//  DWORD fLegacyBidiClass :1; 
		//  DWORD fReserved :8; 
		//} SCRIPT_CONTROL;

		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
		private struct SCRIPT_CONTROL
		{
            [FieldOffset(0)]
			internal UInt16 uDefaultLanguage;
            [FieldOffset(2)]
            private byte packed;
            [FieldOffset(3)]
            internal byte fReserved;

			SCRIPT_CONTROL(uint data)
			{
				uDefaultLanguage = (ushort)(data & 0x0000FFFF);
				packed = (byte)((data & 0x00FF0000) >> 16);
				fReserved = (byte)((data & 0xFF000000) >> 24);
			}

			internal ushort fContextDigits
			{
				get { return (ushort)(packed & 0x0001); }
			}
			internal ushort fInvertPreBoundDir
			{
				get { return (ushort)((packed & 0x0002) >> 1); }
			}
			internal ushort fInvertPostBoundDir
			{
				get { return (ushort)((packed & 0x0004) >> 2); }
			}
			internal ushort fLinkStringBefore
			{
				get { return (ushort)((packed & 0x0008) >> 3); }
			}
			internal ushort fLinkStringAfter
			{
				get { return (ushort)((packed & 0x0010) >> 4); }
			}
			internal ushort fNeutralOverride
			{
				get { return (ushort)((packed & 0x0020) >> 5); }
			}
			internal ushort fNumericOverride
			{
				get { return (ushort)((packed & 0x0040) >> 6); }
			}
			internal ushort fLegacyBidiClass
			{
				get { return (ushort)((packed & 0x0080) >> 7); }
			}
		}
        #endregion

        #region struct SCRIPT_LOGATTR
		//typedef struct tag_SCRIPT_LOGATTR { 
		//  BYTE fSoftBreak :1; 
		//  BYTE fWhiteSpace :1; 
		//  BYTE fCharStop :1; 
		//  BYTE fWordStop :1; 
		//  BYTE fInvalid :1; 
		//  BYTE fReserved :3; 
		//} SCRIPT_LOGATTR;

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct SCRIPT_LOGATTR
		{
			internal byte packed;

			internal bool fSoftBreak
			{
				get
                {
                    return ((packed & 0x0001) > 0 ? true : false);
                }
                set
                {
                    packed |= 0x0001;
                    if (!value)
                        packed ^= 0x0001;
                }
			}
			internal bool fWhiteSpace
			{
				get { return (((packed & 0x0002) >> 1) > 0 ? true : false); }
			}
			internal bool fCharStop
			{
				get { return (((packed & 0x0004) >> 2) > 0 ? true : false); }
			}
			internal bool fWordStop
			{
				get { return (((packed & 0x0008) >> 3) > 0 ? true : false); }
			}
			internal bool fInvalid
			{
				get { return (((packed & 0x0010) >> 4) > 0 ? true : false); }
			}
			internal ushort fReserved
			{
				get { return (ushort)((packed & 0x00E0) >> 5); }
			}
		}
        #endregion
        
        #region RECT
		[StructLayout(LayoutKind.Sequential)]
		private struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
			public RECT(int left, int top, int right, int bottom)
			{
				this.left = left;
				this.top = top;
				this.right = right;
				this.bottom = bottom;
			}

			public RECT(Rectangle r)
			{
				this.left = r.Left;
				this.top = r.Top;
				this.right = r.Right;
				this.bottom = r.Bottom;
			}

			public static RECT FromXYWH(int x, int y, int width, int height)
			{
				return new RECT(x, y, x + width, y + height);
			}

			public Size Size
			{
				get
				{
					return new Size(this.right - this.left, this.bottom - this.top);
				}
			}

			public Rectangle ToRectangle()
			{
				return new Rectangle(this.left, this.top, this.right - this.left, this.bottom - this.top);
			}
		}
        #endregion

        #region DRAWTEXTPARAMS
		[StructLayout(LayoutKind.Sequential)]
			internal class DRAWTEXTPARAMS
		{
			private int cbSize;
			public int iTabLength = 0;
			public int iLeftMargin = 0;
			public int iRightMargin = 0;
			public int uiLengthDrawn = 0;
			public DRAWTEXTPARAMS()
			{
				this.cbSize = Marshal.SizeOf(typeof(DRAWTEXTPARAMS));
			}
		}
        #endregion

        #region ABC
		private struct ABC
		{
			internal int abcA;
			internal uint abcB;
			internal int abcC;

			internal ABC(int abcA, uint abcB, int abcC)
			{
				this.abcA = abcA;
				this.abcB = abcB;
				this.abcC = abcC;
			}
		}
        #endregion

        #region XFORM
		[StructLayout(LayoutKind.Sequential)]
		private struct XFORM
		{
			public float eM11;
			public float eM12;
			public float eM21;
			public float eM22;
			public float eDx;
			public float eDy;

			public XFORM(float eM11, float eM12, float eM21, float eM22, float eDx, float eDy)
			{
				this.eM11 = eM11;
				this.eM12 = eM12;
				this.eM21 = eM21;
				this.eM22 = eM22;
				this.eDx = eDx;
				this.eDy = eDy;
			}
			public XFORM(double eM11, double eM12, double eM21, double eM22, double eDx, double eDy)
			{
				this.eM11 = (float)eM11;
				this.eM12 = (float)eM12;
				this.eM21 = (float)eM21;
				this.eM22 = (float)eM22;
				this.eDx = (float)eDx;
				this.eDy = (float)eDy;
			}
		}
        #endregion

        #region LineInfo
		public class LineInfo
		{
			public int Begin;
			public int Length;
			public bool NeedWidthAlign;
			public int End
			{
				get
				{
					return Begin + Length;
				}
				set
				{
					Length = value - Begin;
				}
			}
			public int Width;
			public double JustifyOffset;
			public string Text;
            public int IndexOfMaxFont;
            public double LineHeight;
            public StiTextHorAlignment TextAlignment;
            public int Indent;
		}
        #endregion

		#region SIZE
		[StructLayout(LayoutKind.Sequential)]
		private struct SIZE
		{
			public int cx;
			public int cy;

			public SIZE(int cx, int cy)
			{
				this.cx = cx;
				this.cy = cy;
			}
		}
		#endregion

		#region struct RunInfo
		public struct RunInfo
		{
			public string Text;
			public double XPos;
			public double YPos;
			public int[] Widths;
            public int[] GlyphWidths;
            public Color TextColor;
            public Color BackColor;
            public int FontIndex;
            public int[] GlyphIndexList;
            public double[] ScaleList;

            //private RunInfo(string text, double xPos, double yPos, int[] widths, int[] glyphWidths, Color textColor, Color backColor, int fontIndex,
            //    int[] glyphIndexList, double[] scaleList)
            //{
            //    this.Text = text;
            //    this.XPos = xPos;
            //    this.YPos = yPos;
            //    this.Widths = widths;
            //    this.GlyphWidths = glyphWidths;
            //    this.TextColor = textColor;
            //    this.BackColor = backColor;
            //    this.FontIndex = fontIndex;
            //    this.GlyphIndexList = glyphIndexList;
            //    this.ScaleList = scaleList;
            //}
		}
		#endregion

		#region POINT
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct POINT 
		{
			public int x;
			public int y;

			internal POINT(int x, int y)
			{
				this.x = x;
				this.y = y;
			}
		}
		#endregion

		#region PANOSE
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		private struct PANOSE 
		{
			public byte bFamilyType;
			public byte bSerifStyle;
			public byte bWeight;
			public byte bProportion;
			public byte bContrast;
			public byte bStrokeVariation;
			public byte ArmStyle;
			public byte bLetterform;
			public byte bMidline;
			public byte bXHeight;

			internal PANOSE(
				byte bFamilyType, 
				byte bSerifStyle, 
				byte bWeight, 
				byte bProportion,
				byte bContrast, 
				byte bStrokeVariation, 
				byte ArmStyle, 
				byte bLetterform, 
				byte bMidline, 
				byte bXHeight)
			{
				this.bFamilyType = bFamilyType;
				this.bSerifStyle = bSerifStyle;
				this.bWeight = bWeight;
				this.bProportion = bProportion;
				this.bContrast = bContrast;
				this.bStrokeVariation = bStrokeVariation;
				this.ArmStyle = ArmStyle;
				this.bLetterform = bLetterform;
				this.bMidline = bMidline;
				this.bXHeight = bXHeight;
			}
		}
		#endregion

		#region TEXTMETRIC
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct TEXTMETRIC 
		{
			public int tmHeight;
			public int tmAscent;
			public int tmDescent;
			public int tmInternalLeading;
			public int tmExternalLeading;
			public int tmAveCharWidth;
			public int tmMaxCharWidth;
			public int tmWeight;
			public int tmOverhang;
			public int tmDigitizedAspectX;
			public int tmDigitizedAspectY;
			public char tmFirstChar;
			public char tmLastChar;
			public char tmDefaultChar;
			public char tmBreakChar;
			public byte tmItalic;
			public byte tmUnderlined;
			public byte tmStruckOut;
			public byte tmPitchAndFamily;
			public byte tmCharSet;

			internal TEXTMETRIC(
				int tmHeight, 
				int tmAscent, 
				int tmDescent, 
				int tmInternalLeading,
				int tmExternalLeading, 
				int tmAveCharWidth, 
				int tmMaxCharWidth, 
				int tmWeight, 
				int tmOverhang, 
				int tmDigitizedAspectX, 
				int tmDigitizedAspectY, 

				char tmFirstChar, 
				char tmLastChar, 
				char tmDefaultChar, 
				char tmBreakChar,
 
				byte tmItalic, 
				byte tmUnderlined, 
				byte tmStruckOut, 
				byte tmPitchAndFamily, 
				byte tmCharSet)
			{
				this.tmHeight = tmHeight;
				this.tmAscent = tmAscent;
				this.tmDescent = tmDescent;
				this.tmInternalLeading = tmInternalLeading;
				this.tmExternalLeading = tmExternalLeading;
				this.tmAveCharWidth = tmAveCharWidth;
				this.tmMaxCharWidth = tmMaxCharWidth;
				this.tmWeight = tmWeight;
				this.tmOverhang = tmOverhang;
				this.tmDigitizedAspectX = tmDigitizedAspectX;
				this.tmDigitizedAspectY = tmDigitizedAspectY;

				this.tmFirstChar = tmFirstChar;
				this.tmLastChar = tmLastChar;
				this.tmDefaultChar = tmDefaultChar;
				this.tmBreakChar = tmBreakChar;

				this.tmItalic = tmItalic;
				this.tmUnderlined = tmUnderlined;
				this.tmStruckOut = tmStruckOut;
				this.tmPitchAndFamily = tmPitchAndFamily;
				this.tmCharSet = tmCharSet;
			}
		}
		#endregion

		#region OUTLINETEXTMETRIC
		[StructLayout(LayoutKind.Sequential)]
		private struct OUTLINETEXTMETRIC 
		{
			public uint otmSize;
			public TEXTMETRIC otmTextMetrics;
			public byte otmFiller;
			public PANOSE otmPanoseNumber;
			public uint otmfsSelection;
			public uint otmfsType;
			public int otmsCharSlopeRise;
			public int otmsCharSlopeRun;
			public int otmItalicAngle;
			public uint otmEMSquare;
			public int otmAscent;
			public int otmDescent;
			public uint otmLineGap;
			public uint otmsCapEmHeight;
			public uint otmsXHeight;
			public RECT otmrcFontBox;
			public int otmMacAscent;
			public int otmMacDescent;
			public uint otmMacLineGap;
			public uint otmusMinimumPPEM;
			public POINT otmptSubscriptSize;
			public POINT otmptSubscriptOffset;
			public POINT otmptSuperscriptSize;
			public POINT otmptSuperscriptOffset;
			public uint otmsStrikeoutSize;
			public int otmsStrikeoutPosition;
			public int otmsUnderscoreSize;
			public int otmsUnderscorePosition;
			public uint otmpFamilyName;		//string offset
			public uint otmpFaceName;		//string offset
			public uint otmpStyleName;		//string offset
			public uint otmpFullName;		//string offset
		}
		#endregion

        #endregion

        #region Constants

        #region TextFormatFlags
		/// <summary>
		/// DrawText Format Flags
		/// </summary>
		[Flags]
		internal enum TextFormatFlags
		{
			Bottom = 8,
			CalculateRectangle = 0x400,
			Default = 0,
			EndEllipsis = 0x8000,
			ExpandTabs = 0x40,
			ExternalLeading = 0x200,
			HidePrefix = 0x100000,
			HorizontalCenter = 1,
			Internal = 0x1000,
			Left = 0,
			ModifyString = 0x10000,
			NoClipping = 0x100,
			NoFullWidthCharacterBreak = 0x80000,
			NoPrefix = 0x800,
			PathEllipsis = 0x4000,
			PrefixOnly = 0x200000,
			Right = 2,
			RightToLeft = 0x20000,
			SingleLine = 0x20,
			TabStop = 0x80,
			TextBoxControl = 0x2000,
			Top = 0,
			VerticalCenter = 4,
			WordBreak = 0x10,
			WordEllipsis = 0x40000
		}
        #endregion

		/// <summary>
		/// Device Context Background Mode
		/// </summary>
		internal enum DeviceContextBackgroundMode
		{
			Opaque = 2,
			Transparent = 1
		}

		/// <summary>
		/// Script doesn't exist in font
		/// </summary>
		private const int E_SCRIPT_NOT_IN_FONT = -2147220992; //0x80040200
		/// <summary>
		/// Ran out of memory
		/// </summary>
		private const int E_OUTOFMEMORY = -2147024882; //0x8007000E

		/// <summary>
		/// XForm stuff
		/// </summary>
		private const int MWT_IDENTITY = 1;
		private const int MWT_LEFTMULTIPLY = 2;
		private const int MWT_RIGHTMULTIPLY = 3;

		/// <summary>
		/// Graphics Modes
		/// </summary>
		private const int GM_COMPATIBLE = 1;
		private const int GM_ADVANCED = 2;

		/// <summary>
		/// Mapping Modes
		/// </summary>
		private const int MM_TEXT = 1;
		private const int MM_LOMETRIC = 2;
		private const int MM_HIMETRIC = 3;
		private const int MM_LOENGLISH = 4;
		private const int MM_HIENGLISH = 5;
		private const int MM_TWIPS = 6;
		private const int MM_ISOTROPIC = 7;
		private const int MM_ANISOTROPIC = 8;

        private const uint GDI_ERROR = 0xFFFFFFFF;

        #endregion        

		#region Const
		//private const bool useGdiPlus = false;
        private const int precisionDigits = 5;
		#endregion

        #region Utils
		/// <summary>
		/// Convert Color to Win32 GDI format
		/// </summary>
		/// <param name="c">Input Color</param>
		/// <returns>Result GDI Color</returns>
		internal static int ColorToWin32(Color c)
		{
			return ((c.R | (c.G << 8)) | (c.B << 0x10));
		}

		private static int GetTabsWidth(StiTextOptions textOptions, double tabSpaceWidth, double currentPosition)
		{
       		float tDistanceBetweenTabs = 20;
    		float tFirstTabOffset = 40;
            if (textOptions != null)
            {
                tDistanceBetweenTabs = textOptions.DistanceBetweenTabs;
                tFirstTabOffset = textOptions.FirstTabOffset;
            }

			double position = currentPosition;
			double otherTab = tabSpaceWidth * tDistanceBetweenTabs;
			double firstTab = tabSpaceWidth * tFirstTabOffset + otherTab;

			if (currentPosition < firstTab)
			{
				position = firstTab;
			}
			else
			{
				if (tDistanceBetweenTabs > 0)
				{
					int kolTabs = (int)((currentPosition - firstTab) / otherTab);
					kolTabs ++;
					position = firstTab + kolTabs * otherTab;
				}
			}

			int result = (int)Math.Round((decimal)(position - currentPosition));
			return result;
		}

        private static int GetFontIndex(string fontName, float fontSize, bool bold, bool italic, bool underlined, bool strikeout,
            bool superOrSubscript, List<StiFontState> tempFontList)
        {
            int fontIndex = GetFontIndex2(fontName, fontSize, bold, italic, underlined, strikeout, tempFontList);
            if (superOrSubscript)
            {
                int fontIndex2 = (tempFontList[fontIndex]).SuperOrSubscriptIndex;
                if (fontIndex2 == -1)
                {
                    fontIndex2 = GetFontIndex2(fontName, fontSize / 1.5f, bold, italic, underlined, strikeout, tempFontList);
                    (tempFontList[fontIndex]).SuperOrSubscriptIndex = fontIndex2;
                    (tempFontList[fontIndex2]).ParentFontIndex = fontIndex;
                }
                fontIndex = fontIndex2;
            }
            return fontIndex;
        }

        private static int GetFontIndex2(string fontName, float fontSize, bool bold, bool italic, bool underlined, bool strikeout,
            List<StiFontState> tempFontList)
        {
            if (tempFontList.Count > 0)
            {
                for (int indexFont = 0; indexFont < tempFontList.Count; indexFont++)
                {
                    StiFontState tempFontState = tempFontList[indexFont];
                    if ((tempFontState.FontName == fontName) &&
                        (tempFontState.FontBase.Size == fontSize) &&
                        (tempFontState.FontBase.Bold == bold) &&
                        (tempFontState.FontBase.Italic == italic) &&
                        (tempFontState.FontBase.Underline == underlined) &&
                        (tempFontState.FontBase.Strikeout == strikeout))
                    {
                        return indexFont;
                    }
                }
            }
            FontStyle fontStyle = FontStyle.Regular;
            if (bold) fontStyle |= FontStyle.Bold;
            if (italic) fontStyle |= FontStyle.Italic;
            if (underlined) fontStyle |= FontStyle.Underline;
            if (strikeout) fontStyle |= FontStyle.Strikeout;

            Font stateFont = null;
            if (fontName.IndexOf(',') != -1)
            {
                string[] fontNames = fontName.Split(new char[] { ',' });
                foreach (string fontNamePart in fontNames)
                {
                    stateFont = StiFontCollection.CreateFont(fontNamePart, fontSize, fontStyle);
                    if (stateFont.Name.Equals(fontNamePart, StringComparison.InvariantCultureIgnoreCase)) break;
                }
            }
            else
            {
                stateFont = StiFontCollection.CreateFont(fontName, fontSize, fontStyle);
            }

            var fontState = new StiFontState();
            fontState.FontName = fontName;
            fontState.FontBase = stateFont;
            fontState.ParentFontIndex = -1;
            fontState.SuperOrSubscriptIndex = -1;
            tempFontList.Add(fontState);
            int fontIndex = tempFontList.Count - 1;
            return fontIndex;
        }

        private static Color ParseColor(string colorAttribute)
        {
            Color color = Color.Transparent;
            if (colorAttribute.Length > 1)
            {
                if (colorAttribute[0] == '#')
                {
                    #region Parse RGB value in hexadecimal notation
                    string colorSt = colorAttribute.Substring(1).ToLowerInvariant();
                    StringBuilder sbc = new StringBuilder();
                    foreach (char ch in colorSt)
                    {
                        if (ch == '0' || ch == '1' || ch == '2' || ch == '3' || ch == '4' || ch == '5' || ch == '6' || ch == '7' ||
                            ch == '8' || ch == '9' || ch == 'a' || ch == 'b' || ch == 'c' || ch == 'd' || ch == 'e' || ch == 'f') sbc.Append(ch);
                    }
                    if (sbc.Length == 3)
                    {
                        colorSt = string.Format("{0}{0}{1}{1}{2}{2}", sbc[0], sbc[1], sbc[2]);
                    }
                    else
                    {
                        colorSt = sbc.ToString();
                    }
                    if (colorSt.Length == 6)
                    {
                        int colorInt = Convert.ToInt32(colorSt, 16);
                        color = Color.FromArgb(0xFF, (colorInt >> 16) & 0xFF, (colorInt >> 8) & 0xFF, colorInt & 0xFF);
                    }
                    #endregion
                }
                else if (colorAttribute.StartsWith("rgb", StringComparison.InvariantCulture))
                {
                    #region Parse RGB function
                    string[] colors = colorAttribute.Trim().Substring(4, colorAttribute.Length - 5).Split(new char[] { ',' });
                    if (colors.Length == 3)
                    {
                        int[] colorsInt = new int[3];
                        if (colors[0].EndsWith("%", StringComparison.InvariantCulture)) colorsInt[0] = (int)Math.Round(Convert.ToInt32(colors[0].Substring(0, colors[0].Length - 1)) * 2.55);
                        else colorsInt[0] = Convert.ToInt32(colors[0]);
                        if (colors[1].EndsWith("%", StringComparison.InvariantCulture)) colorsInt[1] = (int)Math.Round(Convert.ToInt32(colors[1].Substring(0, colors[1].Length - 1)) * 2.55);
                        else colorsInt[1] = Convert.ToInt32(colors[1]);
                        if (colors[2].EndsWith("%", StringComparison.InvariantCulture)) colorsInt[2] = (int)Math.Round(Convert.ToInt32(colors[2].Substring(0, colors[2].Length - 1)) * 2.55);
                        else colorsInt[2] = Convert.ToInt32(colors[2]);
                        color = Color.FromArgb(0xFF, colorsInt[0], colorsInt[1], colorsInt[2]);
                    }
                    #endregion
                }
                else
                {
                    #region Parse color keywords
                    lock (lockHtmlNameToColor)
                    {
                        if (HtmlNameToColor == null)
                        {
                            #region Init hashtable
                            string[,] initData = {
                                {"AliceBlue",	    "#F0F8FF"},
                                {"AntiqueWhite",	"#FAEBD7"},
                                {"Aqua",	    "#00FFFF"},
                                {"Aquamarine",	"#7FFFD4"},
                                {"Azure",	    "#F0FFFF"},
                                {"Beige",	    "#F5F5DC"},
                                {"Bisque",	    "#FFE4C4"},
                                {"Black",	    "#000000"},
                                {"BlanchedAlmond",	"#FFEBCD"},
                                {"Blue",	    "#0000FF"},
                                {"BlueViolet",	"#8A2BE2"},
                                {"Brown",	    "#A52A2A"},
                                {"BurlyWood",	"#DEB887"},
                                {"CadetBlue",	"#5F9EA0"},
                                {"Chartreuse",	"#7FFF00"},
                                {"Chocolate",	"#D2691E"},
                                {"Coral",	    "#FF7F50"},
                                {"CornflowerBlue",	"#6495ED"},
                                {"Cornsilk",	"#FFF8DC"},
                                {"Crimson",	    "#DC143C"},
                                {"Cyan",	    "#00FFFF"},
                                {"DarkBlue",	"#00008B"},
                                {"DarkCyan",	"#008B8B"},
                                {"DarkGoldenRod",	"#B8860B"},
                                {"DarkGray",	"#A9A9A9"},
                                {"DarkGrey",	"#A9A9A9"},
                                {"DarkGreen",	"#006400"},
                                {"DarkKhaki",	"#BDB76B"},
                                {"DarkMagenta",	"#8B008B"},
                                {"DarkOliveGreen",	"#556B2F"},
                                {"Darkorange",	"#FF8C00"},
                                {"DarkOrchid",	"#9932CC"},
                                {"DarkRed",	    "#8B0000"},
                                {"DarkSalmon",	"#E9967A"},
                                {"DarkSeaGreen",	"#8FBC8F"},
                                {"DarkSlateBlue",	"#483D8B"},
                                {"DarkSlateGray",	"#2F4F4F"},
                                {"DarkSlateGrey",	"#2F4F4F"},
                                {"DarkTurquoise",	"#00CED1"},
                                {"DarkViolet",	"#9400D3"},
                                {"DeepPink",	"#FF1493"},
                                {"DeepSkyBlue",	"#00BFFF"},
                                {"DimGray",	    "#696969"},
                                {"DimGrey",	    "#696969"},
                                {"DodgerBlue",	"#1E90FF"},
                                {"FireBrick",	"#B22222"},
                                {"FloralWhite",	"#FFFAF0"},
                                {"ForestGreen",	"#228B22"},
                                {"Fuchsia",	    "#FF00FF"},
                                {"Gainsboro",	"#DCDCDC"},
                                {"GhostWhite",	"#F8F8FF"},
                                {"Gold",	    "#FFD700"},
                                {"GoldenRod",	"#DAA520"},
                                {"Gray",	    "#808080"},
                                {"Grey",	    "#808080"},
                                {"Green",	    "#008000"},
                                {"GreenYellow",	"#ADFF2F"},
                                {"HoneyDew",	"#F0FFF0"},
                                {"HotPink",	    "#FF69B4"},
                                {"IndianRed",	"#CD5C5C"},
                                {"Indigo",	    "#4B0082"},
                                {"Ivory",	    "#FFFFF0"},
                                {"Khaki",	    "#F0E68C"},
                                {"Lavender",	"#E6E6FA"},
                                {"LavenderBlush",	"#FFF0F5"},
                                {"LawnGreen",	"#7CFC00"},
                                {"LemonChiffon",	"#FFFACD"},
                                {"LightBlue",	"#ADD8E6"},
                                {"LightCoral",	"#F08080"},
                                {"LightCyan",	"#E0FFFF"},
                                {"LightGoldenRodYellow",	"#FAFAD2"},
                                {"LightGray",	"#D3D3D3"},
                                {"LightGrey",	"#D3D3D3"},
                                {"LightGreen",	"#90EE90"},
                                {"LightPink",	"#FFB6C1"},
                                {"LightSalmon",	"#FFA07A"},
                                {"LightSeaGreen",	"#20B2AA"},
                                {"LightSkyBlue",	"#87CEFA"},
                                {"LightSlateGray",	"#778899"},
                                {"LightSlateGrey",	"#778899"},
                                {"LightSteelBlue",	"#B0C4DE"},
                                {"LightYellow",	"#FFFFE0"},
                                {"Lime",	    "#00FF00"},
                                {"LimeGreen",	"#32CD32"},
                                {"Linen",	    "#FAF0E6"},
                                {"Magenta",	    "#FF00FF"},
                                {"Maroon",	    "#800000"},
                                {"MediumAquaMarine",	"#66CDAA"},
                                {"MediumBlue",	"#0000CD"},
                                {"MediumOrchid",	"#BA55D3"},
                                {"MediumPurple",	"#9370D8"},
                                {"MediumSeaGreen",	"#3CB371"},
                                {"MediumSlateBlue",	"#7B68EE"},
                                {"MediumSpringGreen",	"#00FA9A"},
                                {"MediumTurquoise",	"#48D1CC"},
                                {"MediumVioletRed",	"#C71585"},
                                {"MidnightBlue",	"#191970"},
                                {"MintCream",	"#F5FFFA"},
                                {"MistyRose",	"#FFE4E1"},
                                {"Moccasin",	"#FFE4B5"},
                                {"NavajoWhite",	"#FFDEAD"},
                                {"Navy",	    "#000080"},
                                {"OldLace",	    "#FDF5E6"},
                                {"Olive",	    "#808000"},
                                {"OliveDrab",	"#6B8E23"},
                                {"Orange",	    "#FFA500"},
                                {"OrangeRed",	"#FF4500"},
                                {"Orchid",	    "#DA70D6"},
                                {"PaleGoldenRod",	"#EEE8AA"},
                                {"PaleGreen",	"#98FB98"},
                                {"PaleTurquoise",	"#AFEEEE"},
                                {"PaleVioletRed",	"#D87093"},
                                {"PapayaWhip",	"#FFEFD5"},
                                {"PeachPuff",	"#FFDAB9"},
                                {"Peru",	    "#CD853F"},
                                {"Pink",	    "#FFC0CB"},
                                {"Plum",	    "#DDA0DD"},
                                {"PowderBlue",	"#B0E0E6"},
                                {"Purple",	    "#800080"},
                                {"Red",	        "#FF0000"},
                                {"RosyBrown",	"#BC8F8F"},
                                {"RoyalBlue",	"#4169E1"},
                                {"SaddleBrown",	"#8B4513"},
                                {"Salmon",	    "#FA8072"},
                                {"SandyBrown",	"#F4A460"},
                                {"SeaGreen",	"#2E8B57"},
                                {"SeaShell",	"#FFF5EE"},
                                {"Sienna",	    "#A0522D"},
                                {"Silver",	    "#C0C0C0"},
                                {"SkyBlue",	    "#87CEEB"},
                                {"SlateBlue",	"#6A5ACD"},
                                {"SlateGray",	"#708090"},
                                {"SlateGrey",	"#708090"},
                                {"Snow",	    "#FFFAFA"},
                                {"SpringGreen",	"#00FF7F"},
                                {"SteelBlue",	"#4682B4"},
                                {"Tan",	        "#D2B48C"},
                                {"Teal",	    "#008080"},
                                {"Thistle",	    "#D8BFD8"},
                                {"Tomato",	    "#FF6347"},
                                {"Turquoise",	"#40E0D0"},
                                {"Violet",	    "#EE82EE"},
                                {"Wheat",	    "#F5DEB3"},
                                {"White",	    "#FFFFFF"},
                                {"WhiteSmoke",	"#F5F5F5"},
                                {"Yellow",	    "#FFFF00"},
                                {"YellowGreen",	"#9ACD32"}};

                            HtmlNameToColor = new Hashtable();
                            for (int index = 0; index < initData.GetLength(0); index++)
                            {
                                string key = initData[index, 0].ToLowerInvariant();
                                int colorInt = Convert.ToInt32(initData[index, 1].Substring(1), 16);
                                Color value = Color.FromArgb(0xFF, (colorInt >> 16) & 0xFF, (colorInt >> 8) & 0xFF, colorInt & 0xFF);
                                HtmlNameToColor[key] = value;
                            }
                            #endregion
                        }
                    }
                    string colorSt = colorAttribute.ToLowerInvariant();
                    if (HtmlNameToColor.ContainsKey(colorSt))
                    {
                        color = (Color)HtmlNameToColor[colorSt];
                    }
                    #endregion
                }
            }
            return color;
        }

        private static string ColorToHtml(Color color)
        {
            string colorSt = string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
            if (color.A != 255) colorSt = "#ttt";
            return colorSt;
        }

        private static float ParseFontSize(string fontSizeAttribute, string delimiter)
        {
            float ffontSize = 8;
            if (!float.TryParse(fontSizeAttribute.Replace(',', '.').Replace(".", delimiter), out ffontSize))
            {
                ffontSize = 8;
            }

            if (ffontSize < 0.5) ffontSize = 0.5f;

            if (InterpreteFontSizeInHtmlTagsAsInHtml)
            {
                switch ((int)Math.Round(ffontSize))
                {
                    case 1:
                        ffontSize = 7;
                        break;

                    case 2:
                        ffontSize = 10;
                        break;

                    case 3:
                        ffontSize = 12;
                        break;

                    case 4:
                        ffontSize = 14;
                        break;

                    case 5:
                        ffontSize = 16;
                        break;

                    case 6:
                        ffontSize = 22;
                        break;

                    case 7:
                        ffontSize = 36;
                        break;
                }
            }

            return ffontSize;
        }

        private static string StateToHtml(StiHtmlState state, string text, int lineInfoIndent)
        {
            StringBuilder sbb = new StringBuilder();
            sbb.Append(string.Format("<font name=\"{0}\" size=\"{1}\">",
                state.TS.FontName,
                state.TS.FontSize));
            if (state.TS.IsColorChanged)
            {
                sbb.Append(string.Format("<font-color=\"{0}\">", ColorToHtml(state.TS.FontColor)));
            }
            if (state.TS.IsBackcolorChanged)
            {
                sbb.Append(string.Format("<background-color=\"{0}\">", ColorToHtml(state.TS.BackColor)));
            }
            sbb.Append(string.Format("<{0}b>", state.TS.Bold ? "" : "/"));
            sbb.Append(string.Format("<{0}i>", state.TS.Italic ? "" : "/"));
            sbb.Append(string.Format("<{0}u>", state.TS.Underline ? "" : "/"));
            sbb.Append(string.Format("<{0}s>", state.TS.Strikeout ? "" : "/"));
            sbb.Append(string.Format("<{0}sup>", state.TS.Superscript ? "" : "/"));
            sbb.Append(string.Format("<{0}sub>", state.TS.Subsript ? "" : "/"));
            sbb.Append(string.Format("<letter-spacing=\"{0}\">", state.TS.LetterSpacing));
            sbb.Append(string.Format("<word-spacing=\"{0}\">", state.TS.WordSpacing));
            sbb.Append(string.Format("<line-height=\"{0}\">", state.TS.LineHeight));
            string align = "left";
            if (state.TS.TextAlign == StiTextHorAlignment.Center) align = "center";
            if (state.TS.TextAlign == StiTextHorAlignment.Right) align = "right";
            if (state.TS.TextAlign == StiTextHorAlignment.Width) align = "justify";
            sbb.Append(string.Format("<text-align=\"{0}\">", align));
            sbb.Append("<StiHtml " + StackToString(state.TagsStack) + ">");
            if (state.TS.Indent > 0) sbb.Append(string.Format("<StiHtml2 {0} {1}>", lineInfoIndent, ListLevelsToString(state.ListLevels, state.TS.Indent)));

            if (text != null)
            {
                //first edition
                //sbb.Append(text.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\xA0", "&nbsp;"));

                //second edition
                //sbb.Append(text);

                //third edition
                sbb.Append(text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;"));
            }

            return sbb.ToString();
        }

        private static string GetIndentString(int indent)
        {
            StringBuilder sb = new StringBuilder();
            for (int index = 0; index < indent; index++)
            {
                sb.Append("\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0\xA0");
            }
            return sb.ToString();
        }

        private static string bulletBlack = new string('\x2022', 1);
        private static string bulletWhite = new string('\x25E6', 1);

        private static void InsertMarker(StringBuilder sb, int markerInt, int indent)
        {
            string marker = bulletBlack;
            if (markerInt > 0)
            {
                marker = markerInt.ToString() + '.';
            }
            else
            {
                int markerInt2 = (0 - markerInt) % 2;
                if (markerInt2 == 1) marker = bulletWhite;
            }

            int offsetMarker = markerInt > 0 ? 2 : 3;
            if (sb.Length > 3)
            {
                if (marker.Length >= sb.Length - offsetMarker)
                {
                    sb.Remove(0, sb.Length - offsetMarker);
                    sb.Insert(0, marker);
                }
                else
                {
                    int offset = sb.Length - offsetMarker - marker.Length;
                    for (int index = 0; index < marker.Length; index++)
                    {
                        sb[offset + index] = marker[index];
                    }
                }
            }
        }
        #endregion

		#region Methods
        [System.Security.SecuritySafeCritical]
		public static SizeD MeasureText(Graphics g, string text, Font font, RectangleD bounds,
			double lineSpacing, bool wordWrap, bool rightToLeft,
            double scale, double angle, StringTrimming trimming, bool lineLimit, bool allowHtmlTags, StiTextOptions textOptions)
		{
			SizeD measureSize = new SizeD(0, 0);
			DrawTextBase(g, ref text, font, bounds, Color.Black, Color.Black, lineSpacing,
				StiTextHorAlignment.Left, StiVertAlignment.Top,	wordWrap, rightToLeft, scale, angle,
                trimming, lineLimit, ref measureSize, false, null, null, allowHtmlTags, null, null, textOptions);
			return measureSize;
		}

        [System.Security.SecuritySafeCritical]
		public static void DrawText(Graphics g, string text, Font font, RectangleD bounds,
			Color foreColor, Color backColor, double lineSpacing,
			StiTextHorAlignment horAlign, StiVertAlignment vertAlign, bool wordWrap, bool rightToLeft,
            double scale, double angle, StringTrimming trimming, bool lineLimit, bool allowHtmlTags, StiTextOptions textOptions = null)
		{
			SizeD measureSize = new SizeD(0, 0);
			DrawTextBase(g, ref text, font, bounds, foreColor, backColor, lineSpacing, horAlign, vertAlign,
                wordWrap, rightToLeft, scale, angle, trimming, lineLimit, ref measureSize, true, null, null, allowHtmlTags, null, null, textOptions);
		}

        [System.Security.SecuritySafeCritical]
		public static string BreakText(Graphics g, ref string text, Font font, RectangleD bounds,
            Color foreColor, Color backColor, double lineSpacing, StiTextHorAlignment horAlign, bool wordWrap, bool rightToLeft,
            double scale, double angle, StringTrimming trimming, bool allowHtmlTags, StiTextOptions textOptions)
		{
			SizeD measureSize = new SizeD(0, 0);
            return DrawTextBase(g, ref text, font, bounds, foreColor, backColor, lineSpacing,
                horAlign, StiVertAlignment.Top, wordWrap, rightToLeft, scale, angle,
                trimming, true, ref measureSize, false, null, null, allowHtmlTags, null, null, textOptions);
		}

        [System.Security.SecuritySafeCritical]
        public static List<string> GetTextLines(Graphics g, ref string text, Font font, RectangleD bounds,
			double lineSpacing, bool wordWrap, bool rightToLeft,
            double scale, double angle, StringTrimming trimming, bool allowHtmlTags, StiTextOptions textOptions)
		{
			SizeD measureSize = new SizeD(0, 0);
            var textLines = new List<string>();
			DrawTextBase(g, ref text, font, bounds, Color.Black, Color.Black, lineSpacing,
				StiTextHorAlignment.Left, StiVertAlignment.Top,	wordWrap, rightToLeft, scale, angle,
                trimming, true, ref measureSize, false, textLines, null, allowHtmlTags, null, null, textOptions);
			return textLines;
		}

        [System.Security.SecuritySafeCritical]
        public static List<string> GetTextLinesAndWidths(Graphics g, ref string text, Font font, RectangleD bounds,
            double lineSpacing, bool wordWrap, bool rightToLeft,
            double scale, double angle, StringTrimming trimming, bool allowHtmlTags, ref List<string> textLines, ref List<StiTextRenderer.LineInfo> linesInfo, StiTextOptions textOptions)
        {
            SizeD measureSize = new SizeD(0, 0);
            DrawTextBase(g, ref text, font, bounds, Color.Black, Color.Black, lineSpacing,
                StiTextHorAlignment.Left, StiVertAlignment.Top, wordWrap, rightToLeft, scale, angle,
                trimming, true, ref measureSize, false, textLines, linesInfo, allowHtmlTags, null, null, textOptions);
            return textLines;
        }

        [System.Security.SecuritySafeCritical]
   		public static void DrawTextForOutput(Graphics g, string text, Font font, RectangleD bounds,
			Color foreColor, Color backColor, double lineSpacing,
			StiTextHorAlignment horAlign, StiVertAlignment vertAlign, bool wordWrap, bool rightToLeft,
            double scale, double angle, StringTrimming trimming, bool lineLimit, bool allowHtmlTags,
            List<RunInfo> outRunsList, List<StiFontState> outFontsList, StiTextOptions textOptions)
		{
			SizeD measureSize = new SizeD(0, 0);
			DrawTextBase(g, ref text, font, bounds, foreColor, backColor, lineSpacing, horAlign, vertAlign,
                wordWrap, rightToLeft, scale, angle, trimming, lineLimit, ref measureSize, true, null, null,
                allowHtmlTags, outRunsList, outFontsList, textOptions);
		}

		#region DrawTextBase private
        [System.Security.SecuritySafeCritical]
		private static string DrawTextBase(Graphics g, ref string text, Font font, RectangleD bounds,
			Color foreColor, Color backColor, double lineSpacing,
			StiTextHorAlignment horAlign, StiVertAlignment vertAlign, bool wordWrap, bool rightToLeft,
			double scale, double angle, StringTrimming trimming, bool lineLimit,
            ref SizeD measureSize, bool needDraw, List<string> textLinesArray, List<StiTextRenderer.LineInfo> textLinesInfo, bool allowHtmlTags,
            List<RunInfo> outRunsList, List<StiFontState> outFontsList, StiTextOptions textOptions)
		{
			RectangleD regionRect = new RectangleD(
                //bounds.X + g.Transform.OffsetX + 1,   //fix 02.12.2008
                bounds.X + g.Transform.OffsetX + 0,
				bounds.Y + g.Transform.OffsetY,
				bounds.Width + 1,
                bounds.Height + 1);
			RectangleD textRect = new RectangleD(
				regionRect.X + 1.5 * scale,
				regionRect.Y,
				(double)Math.Round((decimal)(bounds.Width - 3 * scale), precisionDigits),
				bounds.Height);
			SizeD textSize = new SizeD(textRect.Width, textRect.Height);	//8.81102 width

            Font baseFont = font;
            double baseScale = scale;
            if (PrecisionModeEnabled)
            {
                font = StiFontCollection.CreateFont(font.Name, (float)(font.Size * PrecisionModeFactor), font.Style);
                scale = baseScale / PrecisionModeFactor;
            }

			GraphicsUnit gUnit = g.PageUnit;
			float dpiX = g.DpiX;
			float dpiY = g.DpiY;
			RectangleF pageF = g.ClipBounds;
            float pageScale = g.PageScale;

            //backingImage == System.Drawing.Imaging.Metafile

            #region Get hidden field
            Image backingImage = null;
            Type typeFormat1 = g.GetType();
            FieldInfo infoFormat1 = typeFormat1.GetField("backingImage",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.NonPublic);
            if (infoFormat1 != null)
            {
                object obj1 = infoFormat1.GetValue(g);  //backingImage
                if (obj1 != null)
                {
                    backingImage = obj1 as Image;
                }
            }
            #endregion

            bool useGdiPlus = false;
            if ((backingImage != null) && (backingImage is System.Drawing.Imaging.Metafile))
            {
                useGdiPlus = true;
            }
            //useGdiPlus = true;
            //useGdiPlus = false;

            if (outRunsList != null) useGdiPlus = true;

            string writtenText = text;
			string breakText = string.Empty;

            var runs = new List<RunInfo>();

			if ((!string.IsNullOrEmpty(text)) && (foreColor != Color.Transparent) && (font != null) &&
				(scale > 0.00001))
			{
                if (horAlign == StiTextHorAlignment.Width) wordWrap = true;

                bool forceWidthAlign = text.EndsWith(StiForceWidthAlignTag, StringComparison.InvariantCulture);
                if (forceWidthAlign)
                {
                    text = text.Substring(0, text.Length - StiForceWidthAlignTag.Length);
                    writtenText = text;
                }

                StiFontState[] fontList = null;
                StiHtmlState[] stateList = null;
                int[] stateOrder = null;
                int currentStateIndex = 0;
                string originalText = text;

                #region Make states list
                var baseTagsState = new StiHtmlTagsState(
                    baseFont.Bold,
                    baseFont.Italic,
                    baseFont.Underline,
                    baseFont.Strikeout,
                    baseFont.SizeInPoints,
                    baseFont.Name,
                    foreColor,
                    backColor,
                    false,
                    false,
                    0,
                    0,
                    lineSpacing,
                    horAlign);
                var baseState = new StiHtmlState(
                    baseTagsState,
                    0);

                if (allowHtmlTags)
                {
                    var states = ParseHtmlToStates(text, baseState, !needDraw);
                    stateList = new StiHtmlState[states.Count];
                    var sb = new StringBuilder();
                    var orders = new List<int>();
                    for (int index = 0; index < states.Count; index++)
                    {
                        var state = states[index];
                        stateList[index] = state;

                        var sbTemp = PrepareStateText(state.Text);
                        sb.Append(sbTemp);

                        for (int indexOrder = 0; indexOrder < sbTemp.Length; indexOrder++)
                        {
                            orders.Add(index);
                        }
                        if (state.TS.TextAlign == StiTextHorAlignment.Width)
                        {
                            state.TS.WordSpacing = 0;
                        }
                    }
                    text = sb.ToString();
                    stateOrder = new int[orders.Count];
                    for (int index = 0; index < stateOrder.Length; index++)
                    {
                        stateOrder[index] = orders[index];
                    }
                }
                else
                {
                    stateList = new StiHtmlState[1];
                    stateList[0] = baseState;
                    stateOrder = new int[text.Length];
                    for (int index = 0; index < stateOrder.Length; index++)
                    {
                        stateOrder[index] = 0;
                    }
                }
                #endregion

                #region Make fonts list
                var tempFontList = new List<StiFontState>();
                for (int indexState = 0; indexState < stateList.Length; indexState++)
                {
                    stateList[indexState].FontIndex = GetFontIndex(
                        stateList[indexState].TS.FontName,
                        (float)(PrecisionModeEnabled ? stateList[indexState].TS.FontSize * PrecisionModeFactor : stateList[indexState].TS.FontSize),
                        stateList[indexState].TS.Bold,
                        stateList[indexState].TS.Italic,
                        stateList[indexState].TS.Underline,
                        stateList[indexState].TS.Strikeout,
                        stateList[indexState].TS.Superscript || stateList[indexState].TS.Subsript,
                        tempFontList);
                }
                fontList = new StiFontState[tempFontList.Count];
                for (int indexFont = 0; indexFont < fontList.Length; indexFont++)
                {
                    fontList[indexFont] = tempFontList[indexFont];
                }
                #endregion

                for (int indexFont = 0; indexFont < fontList.Length; indexFont++)
                {
                    fontList[indexFont].FontScaled = null;
                    fontList[indexFont].hFontScaled = IntPtr.Zero;
                    fontList[indexFont].hScriptCache = IntPtr.Zero;
                    fontList[indexFont].hScriptCacheScaled = IntPtr.Zero;
                    if (((scale != 1) || (PrecisionModeEnabled)) && needDraw)
                    {
                        fontList[indexFont].FontScaled = new Font(
                            fontList[indexFont].FontBase.FontFamily,
                            (float)(fontList[indexFont].FontBase.Size * scale),
                            fontList[indexFont].FontBase.Style,
                            fontList[indexFont].FontBase.Unit,
                            fontList[indexFont].FontBase.GdiCharSet,
                            fontList[indexFont].FontBase.GdiVerticalFont);
                        fontList[indexFont].hFontScaled = fontList[indexFont].FontScaled.ToHfont();
                    }
                }

                try
                {
                    IntPtr hdc = g.GetHdc();
                    try
                    {
                        #region get OUTLINETEXTMETRIC - calculate LineHeight
                        for (int indexFont = 0; indexFont < fontList.Length; indexFont++)
                        {
                            OUTLINETEXTMETRIC otm = GetOutlineTextMetricsCached(fontList[indexFont].FontName, fontList[indexFont].FontBase.Style, hdc);

                            double lineHeightTextMetric = (otm.otmTextMetrics.tmAscent + otm.otmTextMetrics.tmDescent + otm.otmTextMetrics.tmExternalLeading) / MaxFontSize * fontList[indexFont].FontBase.SizeInPoints;
                            double lineHeight = lineHeightTextMetric * scale;
                            double lineHeightWithSpacing = lineHeight * lineSpacing;
                            double ascend = (otm.otmTextMetrics.tmAscent) / MaxFontSize * fontList[indexFont].FontBase.SizeInPoints * scale;
                            double descend = (otm.otmTextMetrics.tmDescent) / MaxFontSize * fontList[indexFont].FontBase.SizeInPoints * scale;
                            //double emValue = otm.otmEMSquare / MaxFontSize * fontList[indexFont].FontBase.SizeInPoints * scale;
                            //double emValue = otm.otmEMSquare / MaxFontSize * fontList[indexFont].FontBase.SizeInPoints;
                            double emValue = fontList[indexFont].FontBase.SizeInPoints;
                            //fontList[indexFont].LineHeightTextMetric = lineHeightTextMetric;
                            fontList[indexFont].LineHeight = lineHeight;
                            //fontList[indexFont].LineHeightWithSpacing = lineHeightWithSpacing;
                            fontList[indexFont].Ascend = ascend;
                            fontList[indexFont].Descend = descend;
                            fontList[indexFont].EmValue = emValue;
                        }
                        #endregion

                        for (int indexFont = 0; indexFont < fontList.Length; indexFont++)
                        {
                            fontList[indexFont].hFont = fontList[indexFont].FontBase.ToHfont();
                        }
                        try
                        {
                            #region textSize correction
                            if (((angle > 45) && (angle < 135)) || ((angle > 225) && (angle < 315)))
                            {
                                double tempValue = textSize.Width;
                                textSize.Width = textSize.Height;
                                textSize.Height = tempValue;
                            }
                            double angleRad = -angle * Math.PI / 180;
                            #endregion

                            bool gdiError;
                            int oldGraphMode = 0;
                            int oldMapMode = 0;
                            XFORM oldXForm = new XFORM(1, 0, 0, 1, 0, 0);
                            IntPtr oldRegion = IntPtr.Zero;
                            IntPtr newRegion = IntPtr.Zero;
                            int resultGetClip = 0;

                            if (needDraw)
                            {
                                oldGraphMode = SetGraphicsMode(hdc, GM_ADVANCED);
                                if (oldGraphMode == 0) ThrowError(3);

                                if (Compatibility2009)
                                {
                                    #region set world transformation
                                    float scaleX = 1;
                                    float scaleY = 1;
                                    if ((gUnit != GraphicsUnit.Display) && (gUnit != GraphicsUnit.Pixel))
                                    {
                                        #region scale for print
                                        oldMapMode = SetMapMode(hdc, MM_ANISOTROPIC);
                                        if (oldMapMode == 0) ThrowError(4);

                                        float maxX = pageF.Width + (pageF.X > 0 ? pageF.X : 0);
                                        float maxY = pageF.Height + (pageF.Y > 0 ? pageF.Y : 0);

                                        scaleX = dpiX / 100f;
                                        scaleY = dpiY / 100f;

                                        float maxXP = maxX * scaleX;
                                        float maxYP = maxY * scaleY;

                                        SIZE windowSize = new SIZE();
                                        gdiError = SetWindowExtEx(
                                            hdc,
                                            (int)maxX,
                                            (int)maxY,
                                            out windowSize);
                                        if (!gdiError) ThrowError(5);

                                        SIZE viewportSize = new SIZE();
                                        gdiError = SetViewportExtEx(
                                            hdc,
                                            (int)maxXP,
                                            (int)maxYP,
                                            out viewportSize);
                                        if (!gdiError) ThrowError(6);
                                        #endregion
                                    }

                                    gdiError = GetWorldTransform(hdc, out oldXForm);
                                    if (!gdiError) ThrowError(7);

                                    //translate dx, dy
                                    XFORM newXForm1 = new XFORM(1, 0, 0, 1,
                                        textRect.Left + textRect.Width / 2f,
                                        textRect.Top + textRect.Height / 2f);
                                    gdiError = ModifyWorldTransform(hdc, ref newXForm1, MWT_LEFTMULTIPLY);
                                    if (!gdiError) ThrowError(8);

                                    //rotate
                                    XFORM newXForm2 = new XFORM(
                                        Math.Cos(angleRad),
                                        Math.Sin(angleRad),
                                        -Math.Sin(angleRad),
                                        Math.Cos(angleRad),
                                        0, 0);
                                    gdiError = ModifyWorldTransform(hdc, ref newXForm2, MWT_LEFTMULTIPLY);
                                    if (!gdiError) ThrowError(9);

                                    //translate dx, dy
                                    XFORM newXForm3 = new XFORM(1, 0, 0, 1,
                                        -textSize.Width / 2f,
                                        -textSize.Height / 2f);
                                    gdiError = ModifyWorldTransform(hdc, ref newXForm3, MWT_LEFTMULTIPLY);
                                    if (!gdiError) ThrowError(10);

                                    #endregion

                                    #region set drawing parameters
                                    if (!foreColor.IsEmpty)
                                    {
                                        SetTextColor(hdc, ColorToWin32(foreColor));
                                    }
                                    DeviceContextBackgroundMode newMode1 = (backColor.IsEmpty || (backColor == Color.Transparent)) ? DeviceContextBackgroundMode.Transparent : DeviceContextBackgroundMode.Opaque;
                                    SetBkMode(hdc, (int)newMode1);
                                    if (newMode1 != DeviceContextBackgroundMode.Transparent)
                                    {
                                        SetBkColor(hdc, ColorToWin32(backColor));
                                    }

                                    resultGetClip = GetClipRgn(hdc, oldRegion);
                                    newRegion = CreateRectRgn(
                                        (int)(regionRect.Left * scaleX),
                                        (int)(regionRect.Top * scaleY),
                                        (int)(regionRect.Right * scaleX),
                                        (int)(regionRect.Bottom * scaleY));
                                    SelectClipRgn(hdc, newRegion);
                                    #endregion
                                }
                            }

                            SelectObject(hdc, fontList[0].hFont);
                            RECT lpRect = new RECT(textRect.ToRectangle());

                            #region prepare text
                            var linesInfo = new List<LineInfo>();
                            //IntPtr scriptCache = IntPtr.Zero;

                            #region get lines info
                            int posCurrent = 0;
                            while (posCurrent < text.Length)
                            {
                                LineInfo pair = new LineInfo();
                                pair.Begin = posCurrent;
                                //find end line
                                while ((posCurrent < text.Length) && (text[posCurrent] != '\r') && (text[posCurrent] != '\n'))
                                {
                                    posCurrent++;
                                }
                                pair.End = posCurrent;
                                //trim spaces right
                                while ((pair.End > pair.Begin + 1) && (Char.IsWhiteSpace(text[pair.End - 1])))
                                {
                                    pair.End--;
                                }
                                linesInfo.Add(pair);
                                //find next line
                                posCurrent++;
                                if ((posCurrent < text.Length) &&
                                    ((text[posCurrent] == '\r') || (text[posCurrent] == '\n')) &&
                                    (text[posCurrent - 1] != text[posCurrent]))
                                {
                                    posCurrent++;
                                }
                            }
                            #endregion

                            if (stateList.Length > 0 && stateList[0].TS.Indent < 0 && linesInfo.Count > 0)
                            {
                                linesInfo[0].Indent = -stateList[0].TS.Indent;
                                if (stateList[0].ListLevels != null)
                                {
                                    stateList[0].TS.Indent = stateList[0].ListLevels.Count;
                                }
                            }

                            #region calculate line height, ellipsis and tabs
                            RECT lpRectForCalculate = new RECT(0, 0, Int32.MaxValue, Int32.MaxValue);
                            TextFormatFlags flagsForCalculate = TextFormatFlags.Default | TextFormatFlags.CalculateRectangle;
                            DrawTextExW(hdc, "…", 1, ref lpRectForCalculate, (int)flagsForCalculate, new DRAWTEXTPARAMS());
                            int ellipsisWidth = (int)(lpRectForCalculate.Size.Width * scale);

                            //lpRectForCalculate = new RECT(0, 0, Int32.MaxValue, Int32.MaxValue);
                            //DrawTextExW(hdc, " ", 1, ref lpRectForCalculate, (int)flagsForCalculate, new DRAWTEXTPARAMS());
                            //double tabSpaceWidth = lpRectForCalculate.Size.Width / font.SizeInPoints * 4;
                            double tabSpaceWidth = 1;

                            //double lineHeight2 = lpRectForCalculate.Size.Height * scale;
                            //double lineHeight = font.Height * scale;

                            //double lineHeight = lineHeightTextMetric * scale;
                            //double lineHeightWithSpacing = lineHeight * lineSpacing;
                            #endregion

                            // !!! переделать все fontList[0].LineHeight

                            #region LineLimit; for HtmlTags works incorrectly
                            double linesCountCalcDouble = (textSize.Height - fontList[0].LineHeight) / (fontList[0].LineHeight * lineSpacing) + 1;
                            int linesCountCalc = (int)linesCountCalcDouble;
                            if (!lineLimit)
                            {
                                if (allowHtmlTags)
                                {
                                    linesCountCalc = linesInfo.Count;
                                }
                                else
                                {
                                    linesCountCalc++;
                                }
                            }
                            int linesCountLimit = linesCountCalc;
                            #endregion

                            #region wordwrap and line length calculation
                            //if ((wordWrap) || (horAlign != StiTextHorAlignment.Left))
                            {
                                var wordWrapPoints = new List<LineInfo>();
                                int lastLineIndex = 0;
                                for (int indexLine = 0; indexLine < linesInfo.Count; indexLine++)
                                {
                                    LineInfo textLinePair = linesInfo[indexLine];
                                    string textLine = text.Substring(textLinePair.Begin, textLinePair.Length);
                                    int textLen = textLine.Length;

                                    int indentIndex = stateList[stateOrder[textLinePair.Begin]].TS.Indent;
                                    int indentCount = indentIndex * 10;
                                    int indentCalcSize = 0; 
                                    int indentSize = 0;
                                    bool isIndent = indentCount > 0;
                                    bool isWrapped = false;
                                    if (textLinePair.Indent > 0)
                                    {
                                        indentSize = textLinePair.Indent;
                                        indentCalcSize = textLinePair.Indent;
                                        isWrapped = true;
                                    }

                                    if (textLen == 0)
                                    {
                                        var pairWrapPoint = new LineInfo();
                                        pairWrapPoint.Begin += textLinePair.Begin;
                                        wordWrapPoints.Add(pairWrapPoint);
                                        lastLineIndex = wordWrapPoints.Count;
                                        continue;
                                    }

                                    if (textLen > 0)
                                    {
                                        #region scan for bracket and etc..
                                        //scan for bracket
                                        int[] nowrapList = new int[textLen];	//1 - nowrap, 2 - nowrapBegin
                                        //bool existNowrapPoints = false;
                                        int posInText = 0;
                                        while (posInText < textLen)
                                        {
                                            //find first bracket in string
                                            while (posInText < textLen)
                                            {
                                                UnicodeCategory cat = char.GetUnicodeCategory(textLine[posInText]);
                                                if ((cat == UnicodeCategory.OpenPunctuation) || (cat == UnicodeCategory.ClosePunctuation) ||
                                                    (cat == UnicodeCategory.InitialQuotePunctuation) || (cat == UnicodeCategory.FinalQuotePunctuation))
                                                    break;
                                                posInText++;
                                            }
                                            if (posInText < textLen)	//bracket founded
                                            {
                                                int posBegin = posInText;
                                                int posEnd = posInText;
                                                if (char.GetUnicodeCategory(textLine[posInText]) == UnicodeCategory.OpenPunctuation ||
                                                    char.GetUnicodeCategory(textLine[posInText]) == UnicodeCategory.InitialQuotePunctuation)
                                                {
                                                    #region OpenPunctuation - Word - ClosePunctuation
                                                    //scan left part
                                                    while (posInText < textLen)
                                                    {
                                                        UnicodeCategory cat = char.GetUnicodeCategory(textLine[posInText]);
                                                        if (!((cat == UnicodeCategory.OpenPunctuation) || (cat == UnicodeCategory.InitialQuotePunctuation) || (cat == UnicodeCategory.SpaceSeparator)))
                                                            break;
                                                        posInText++;
                                                    }
                                                    //scan center part
                                                    if ((posInText < textLen) &&
                                                        (char.GetUnicodeCategory(textLine[posInText]) != UnicodeCategory.ClosePunctuation) &&
                                                        (char.GetUnicodeCategory(textLine[posInText]) != UnicodeCategory.FinalQuotePunctuation))
                                                    {
                                                        //skip one word and spaces after him
                                                        while (posInText < textLen)
                                                        {
                                                            UnicodeCategory cat = char.GetUnicodeCategory(textLine[posInText]);
                                                            if (cat == UnicodeCategory.SpaceSeparator || isWordWrapSymbol2(textLine, posInText) || isCJKWordWrap(textLine, posInText))
                                                                break;
                                                            posInText++;
                                                        }
                                                        while (posInText < textLen)
                                                        {
                                                            UnicodeCategory cat = char.GetUnicodeCategory(textLine[posInText]);
                                                            if (cat != UnicodeCategory.SpaceSeparator)
                                                                break;
                                                            posInText++;
                                                        }
                                                        if ((posInText < textLen) && (char.GetUnicodeCategory(textLine[posInText]) == UnicodeCategory.DashPunctuation)) posInText++;
                                                    }
                                                    //scan right part
                                                    if ((posInText < textLen) &&
                                                        ((char.GetUnicodeCategory(textLine[posInText]) == UnicodeCategory.ClosePunctuation) ||
                                                        (char.GetUnicodeCategory(textLine[posInText]) == UnicodeCategory.FinalQuotePunctuation)))
                                                    {
                                                        while (posInText < textLen)
                                                        {
                                                            UnicodeCategory cat = char.GetUnicodeCategory(textLine[posInText]);
                                                            if (!((cat == UnicodeCategory.ClosePunctuation) || (cat == UnicodeCategory.FinalQuotePunctuation) ||
                                                                (cat == UnicodeCategory.OtherPunctuation) || (cat == UnicodeCategory.SpaceSeparator)))
                                                                break;
                                                            posInText++;
                                                        }
                                                    }
                                                    posEnd = posInText;
                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region Word - ClosePunctuation
                                                    //scan center part - search one word before bracket
                                                    posInText--;
                                                    //skip SpaceSeparators
                                                    while (posInText >= 0)
                                                    {
                                                        UnicodeCategory cat = char.GetUnicodeCategory(textLine[posInText]);
                                                        if (cat != UnicodeCategory.SpaceSeparator)
                                                            break;
                                                        posInText--;
                                                    }
                                                    //skip one word
                                                    while (posInText >= 0)
                                                    {
                                                        UnicodeCategory cat = char.GetUnicodeCategory(textLine[posInText]);
                                                        if (cat == UnicodeCategory.SpaceSeparator || nowrapList[posInText] != 0 || isCJKWordWrap(textLine, posInText))
                                                            break;
                                                        posInText--;
                                                    }
                                                    posBegin = posInText + (isCJKWordWrap(textLine, posInText) ? 0 : 1);

                                                    //scan right part
                                                    posInText = posEnd;
                                                    while (posInText < textLen)
                                                    {
                                                        UnicodeCategory cat = char.GetUnicodeCategory(textLine[posInText]);
                                                        if (!((cat == UnicodeCategory.ClosePunctuation) || (cat == UnicodeCategory.FinalQuotePunctuation) ||
                                                            (cat == UnicodeCategory.OtherPunctuation) || (cat == UnicodeCategory.SpaceSeparator)))
                                                            break;
                                                        posInText++;
                                                    }
                                                    posEnd = posInText;
                                                    #endregion
                                                }
                                                //remove spaces at end of wrap string
                                                while ((posEnd > posBegin) && (char.GetUnicodeCategory(textLine[posEnd - 1]) == UnicodeCategory.SpaceSeparator))
                                                {
                                                    posEnd--;
                                                }
                                                //existNowrapPoints = true;
                                                nowrapList[posBegin] = 2;
                                                for (int indexBr = posBegin + 1; indexBr < posEnd; indexBr++)
                                                {
                                                    nowrapList[indexBr] = 1;
                                                }
                                            }
                                        }

                                        //scan for NonBreakingSpace
                                        bool flag2 = false;
                                        for (int indexChar = 0; indexChar < textLen; indexChar++)
                                        {
                                            if (textLine[indexChar] == '\x2011' || textLine[indexChar] == '\xA0')
                                            {
                                                flag2 = true;
                                                nowrapList[indexChar] = 1;
                                                int indexCh2 = indexChar;
                                                bool flag3 = true;
                                                while (indexCh2 > 0 && char.IsLetterOrDigit(textLine[indexCh2 - 1]))
                                                {
                                                    indexCh2--;
                                                    if (nowrapList[indexCh2] == 0)
                                                    {
                                                        nowrapList[indexCh2] = 1;
                                                    }
                                                    else
                                                    {
                                                        flag3 = false;
                                                        break;
                                                    }
                                                }
                                                if (flag3) nowrapList[indexCh2] = 2;
                                                indexCh2 = indexChar;
                                                while (indexCh2 + 1 < textLen && (char.IsLetterOrDigit(textLine[indexCh2 + 1]) || textLine[indexCh2 + 1] == '\x2011' || textLine[indexCh2 + 1] == '\xA0'))
                                                {
                                                    indexCh2++;
                                                    nowrapList[indexCh2] = 1;
                                                }
                                                indexChar = indexCh2;
                                            }
                                        }
                                        if (flag2)
                                        {
                                            textLine = textLine.Replace('\x2011', '-');
                                        }

                                        //scan for OtherPunctuation, MathSymbol, CurrencySymbol
                                        for (int indexChar = 0; indexChar < textLen; indexChar++)
                                        {
                                            if (isNotWordWrapSymbol(textLine, indexChar))
                                            {
                                                if (indexChar > 0 && isNotWordWrapSymbol2(textLine, indexChar - 1))
                                                {
                                                    nowrapList[indexChar] = 1;

                                                    //mark symbols after this position
                                                    int indexCh2 = indexChar;
                                                    if (!isWordWrapSymbol2(textLine, indexCh2) && !isCJKSymbol(textLine, indexCh2))
                                                    {
                                                        indexCh2++;
                                                        while (indexCh2 < textLine.Length && char.IsLetterOrDigit(textLine[indexCh2]) && !isCJKSymbol(textLine, indexCh2))
                                                        {
                                                            if (nowrapList[indexCh2] == 0)
                                                            {
                                                                nowrapList[indexCh2] = 1;
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                            indexCh2++;
                                                        }
                                                    }

                                                    indexCh2 = indexChar;
                                                    bool flag3 = false;
                                                    bool flag4 = true;
                                                    while (indexCh2 > 0 && isNotWordWrapSymbol2(textLine, indexCh2 - 1) && !isWordWrapSymbol2(textLine, indexCh2 - 1) && flag4)
                                                    {
                                                        flag3 = true;
                                                        indexCh2--;
                                                        if (nowrapList[indexCh2] == 0)
                                                        {
                                                            nowrapList[indexCh2] = 1;
                                                        }
                                                        else
                                                        {
                                                            flag3 = false;
                                                            break;
                                                        }
                                                        flag4 = !isCJKSymbol(textLine, indexCh2);
                                                    }
                                                    if (flag3) nowrapList[indexCh2] = 2;
                                                }
                                                else if ((indexChar < textLine.Length - 1) && isNotWordWrapSymbol2(textLine, indexChar + 1))
                                                {
                                                    bool flag3 = nowrapList[indexChar] == 0;

                                                    //mark symbols after this position
                                                    int indexCh2 = indexChar;
                                                    if (!isWordWrapSymbol2(textLine, indexCh2))
                                                    {
                                                        indexCh2++;
                                                        while (indexCh2 < textLine.Length && char.IsLetterOrDigit(textLine[indexCh2]))
                                                        {
                                                            if (nowrapList[indexCh2] == 0)
                                                            {
                                                                nowrapList[indexCh2] = 1;
                                                            }
                                                            else
                                                            {
                                                                break;
                                                            }
                                                            indexCh2++;
                                                        }
                                                    }

                                                    if (flag3) nowrapList[indexChar] = 2;
                                                }
                                            }
                                        }
                                        #endregion

                                        int[] lineWidths = new int[textLen];

                                        #region ScriptItemize
                                        SCRIPT_ITEM[] scriptItemList = null;
                                        int scriptItemCount = 0;
                                        int error = 0;
                                        int cMaxItems = 10;
                                        do
                                        {
                                            cMaxItems *= 10;
                                            SCRIPT_CONTROL psControl = new SCRIPT_CONTROL();
                                            SCRIPT_STATE psState = new SCRIPT_STATE();
                                            //scriptItemList = new SCRIPT_ITEM[cMaxItems];
                                            psState.uBidiLevel = (ushort)(rightToLeft ? 1 : 0);

                                            IntPtr buf = Marshal.AllocHGlobal(sizeofScriptItem * (cMaxItems + 1) + 2);

                                            error = ScriptItemize(
                                                textLine,
                                                textLine.Length,
                                                cMaxItems,
                                                ref psControl,
                                                ref psState,
                                                //ref scriptItemList[0],
                                                buf,
                                                out scriptItemCount);
                                            if ((error != 0) && (error != E_OUTOFMEMORY))
                                            {
                                                Marshal.FreeHGlobal(buf);
                                                ThrowError(11, error);
                                            }

                                            if (error == 0)
                                            {
                                                scriptItemList = new SCRIPT_ITEM[scriptItemCount + 1];
                                                IntPtr offset = buf;
                                                for (int indexItem = 0; indexItem < scriptItemCount + 1; indexItem++)
                                                {
                                                    scriptItemList[indexItem] = (SCRIPT_ITEM)Marshal.PtrToStructure(offset, typeof(SCRIPT_ITEM));
                                                    offset = (IntPtr)((Int64)offset + sizeofScriptItem);
                                                }
                                            }

                                            Marshal.FreeHGlobal(buf);

                                        }
                                        while (error == E_OUTOFMEMORY);
                                        #endregion

                                        #region Check for: '-' at end of word, break between letter and digit
                                        List<SCRIPT_ITEM> tempList = new List<SCRIPT_ITEM>(scriptItemList);
                                        for (int indexScriptItem = scriptItemList.Length - 2; indexScriptItem > 0; indexScriptItem--)
                                        {
                                            int posChar = scriptItemList[indexScriptItem].iCharPos;
                                            if ((posChar == scriptItemList[indexScriptItem + 1].iCharPos - 1) && (textLine[posChar] == '-') && char.IsLetter(textLine, posChar - 1) ||
                                                char.IsDigit(textLine, posChar) && char.IsLetter(textLine, posChar - 1))
                                            {
                                                tempList.RemoveAt(indexScriptItem);
                                            }
                                        }
                                        if (tempList.Count < scriptItemList.Length)
                                        {
                                            scriptItemList = new SCRIPT_ITEM[tempList.Count];
                                            for (int indexItem = 0; indexItem < tempList.Count; indexItem++)
                                            {
                                                scriptItemList[indexItem] = tempList[indexItem];
                                            }
                                            scriptItemCount = scriptItemList.Length - 1;
                                        }
                                        tempList.Clear();
                                        #endregion

                                        #region break runs in nowrapBegin points
                                        //if (existNowrapPoints)
                                        {
                                            var newScriptItemList = new List<SCRIPT_ITEM>();
                                            newScriptItemList.Add(scriptItemList[0]);
                                            int itemIndex = 0;
                                            for (int indexChar = 0; indexChar < textLen; indexChar++)
                                            {
                                                if (indexChar == scriptItemList[itemIndex + 1].iCharPos)
                                                {
                                                    itemIndex++;
                                                    newScriptItemList.Add(scriptItemList[itemIndex]);
                                                    continue;
                                                }
                                                if ((nowrapList[indexChar] == 2) && (indexChar != 0))
                                                {
                                                    SCRIPT_ITEM si = scriptItemList[itemIndex];
                                                    si.iCharPos = indexChar;
                                                    newScriptItemList.Add(si);
                                                    continue;
                                                }
                                                if ((indexChar > 0) && (stateOrder[textLinePair.Begin + indexChar] != stateOrder[textLinePair.Begin + indexChar - 1]))
                                                {
                                                    SCRIPT_ITEM si = scriptItemList[itemIndex];
                                                    si.iCharPos = indexChar;
                                                    newScriptItemList.Add(si);
                                                    continue;
                                                }
                                            }
                                            newScriptItemList.Add(scriptItemList[scriptItemCount]);
                                            scriptItemList = new SCRIPT_ITEM[newScriptItemList.Count];
                                            for (int indexRun = 0; indexRun < newScriptItemList.Count; indexRun++)
                                            {
                                                scriptItemList[indexRun] = newScriptItemList[indexRun];
                                            }
                                            scriptItemCount = newScriptItemList.Count - 1;
                                        }
                                        #endregion

                                        int bufLen = textLen * 2;
                                        if (bufLen < 20) bufLen = 20;
                                        int sumLen = 0;
                                        int lastLineOffset = 0;

                                        int nowrapLastRunIndex = 0;
                                        int nowrapLastUsedRun = 0;

                                        // process each "run" of text 
                                        for (int indexRun = 0; indexRun < scriptItemCount; indexRun++)
                                        {
                                            #region process run
                                            ushort[] glyphIndexList = new ushort[bufLen];
                                            ushort[] glyphClusterList = new ushort[textLen];
                                            //SCRIPT_VISATTR[] scriptVisAttrList = new SCRIPT_VISATTR[bufLen];
                                            IntPtr scriptVisAttrList = Marshal.AllocHGlobal(sizeofScriptVisattr * bufLen);
                                            //GOFFSET[] goff = new GOFFSET[bufLen];
                                            int[] advanceWidthList = new int[bufLen];
                                            ABC abc;
                                            int glyphIndexCount;

                                            int runCharPos = scriptItemList[indexRun].iCharPos;
                                            int runCharLen = scriptItemList[indexRun + 1].iCharPos - runCharPos;
                                            string runText = textLine.Substring(runCharPos, runCharLen);

                                            currentStateIndex = stateOrder[textLinePair.Begin + runCharPos];
                                            StiFontState currentFontState = fontList[stateList[currentStateIndex].FontIndex];

                                            SelectObject(hdc, currentFontState.hFont);

                                            if ((nowrapList[runCharPos] == 2) && (indexRun != nowrapLastUsedRun))
                                            {
                                                nowrapLastRunIndex = indexRun;
                                            }

                                            #region ScriptShape
                                            error = ScriptShape(
                                                hdc,
                                                ref currentFontState.hScriptCache,
                                                runText,
                                                runCharLen,
                                                bufLen,
                                                ref scriptItemList[indexRun].a,
                                                glyphIndexList,
                                                glyphClusterList,
                                                //ref scriptVisAttrList[0],
                                                scriptVisAttrList,
                                                out glyphIndexCount
                                                );
                                            if (error == E_SCRIPT_NOT_IN_FONT)  //fix
                                            {
                                                error = 0;
                                                glyphIndexCount = runCharLen;
                                                for (int tIndex = 0; tIndex < glyphIndexCount; tIndex++)
                                                {
                                                    glyphClusterList[tIndex] = (ushort)tIndex;
                                                }
                                                scriptItemList[indexRun].a.packed = 0;
                                            }
                                            if ((error != 0) && (error != E_SCRIPT_NOT_IN_FONT))
                                            {
                                                Marshal.FreeHGlobal(scriptVisAttrList);
                                                ThrowError(12, error);
                                            }
                                            #endregion

                                            IntPtr goff = Marshal.AllocHGlobal(sizeofGoffset * bufLen);

                                            #region ScriptPlace
                                            error = ScriptPlace(
                                                hdc,
                                                ref currentFontState.hScriptCache,
                                                glyphIndexList,
                                                glyphIndexCount,
                                                //ref scriptVisAttrList[0],
                                                scriptVisAttrList,
                                                ref scriptItemList[indexRun].a,
                                                advanceWidthList,
                                                //ref goff[0],
                                                goff,
                                                out abc
                                                );
                                            Marshal.FreeHGlobal(goff);
                                            if (error != 0)
                                            {
                                                Marshal.FreeHGlobal(scriptVisAttrList);
                                                ThrowError(13, error);
                                            }
                                            #endregion

                                            Marshal.FreeHGlobal(scriptVisAttrList);

                                            if (CorrectionEnabled)
                                            {
                                                #region AdvanceWidthList correction
                                                ushort[] allGlyphWidth = GetFontWidth(currentFontState.FontBase);
                                                if (allGlyphWidth.Length > 0)
                                                {
                                                    //double summm1 = 0;
                                                    //for (int indexGlyph = 0; indexGlyph < glyphIndexCount; indexGlyph++)
                                                    //{
                                                    //    summm1 += advanceWidthList[indexGlyph];
                                                    //}
                                                    //double summm2 = 0;
                                                    double fontScale = MaxFontSize / currentFontState.EmValue;
                                                    for (int indexGlyph = 0; indexGlyph < glyphIndexCount; indexGlyph++)
                                                    {
                                                        int tempGlyphIndex = glyphIndexList[indexGlyph];
                                                        if (tempGlyphIndex >= allGlyphWidth.Length) tempGlyphIndex = allGlyphWidth.Length - 1;
                                                        double width = allGlyphWidth[tempGlyphIndex] / fontScale;
                                                        if (advanceWidthList[indexGlyph] < width - 0.4)
                                                        {
                                                            int newWidth = (int)Math.Round(width);
                                                            if (advanceWidthList[indexGlyph] >= newWidth) newWidth++;
                                                            advanceWidthList[indexGlyph] = newWidth;
                                                        }
                                                        else
                                                        {
                                                            double percent = advanceWidthList[indexGlyph] / width;
                                                            if (advanceWidthList[indexGlyph] - width > 1 && percent > 1.1)
                                                            {
                                                                advanceWidthList[indexGlyph] = (int)Math.Ceiling(width);
                                                            }
                                                        }
                                                        //summm2 += advanceWidthList[indexGlyph];
                                                    }
                                                }
                                                #endregion
                                            }

                                            #region Apply LetterSpacing and WordSpacing
                                            if (stateList[currentStateIndex].TS.LetterSpacing != 0)
                                            {
                                                //double addedLetterSpacing = currentFontState.EmValue * stateList[currentStateIndex].TS.LetterSpacing;
                                                double addedLetterSpacing = currentFontState.EmValue * 1.35 * stateList[currentStateIndex].TS.LetterSpacing;
                                                int ssumWidthInt = 0;
                                                double ssumWidthDouble = 0;
                                                for (int indexGlyph = 0; indexGlyph < glyphIndexCount; indexGlyph++)
                                                {
                                                    ssumWidthDouble += advanceWidthList[indexGlyph] + addedLetterSpacing;
                                                    int glyphWidthScaled = (int)ssumWidthDouble - ssumWidthInt;
                                                    ssumWidthInt += glyphWidthScaled;
                                                    advanceWidthList[indexGlyph] = glyphWidthScaled;
                                                }
                                            }
                                            if (stateList[currentStateIndex].TS.WordSpacing != 0)
                                            {
                                                double addedWordSpacing = currentFontState.EmValue * stateList[currentStateIndex].TS.WordSpacing;
                                                int ssumWidthInt = 0;
                                                double ssumWidthDouble = 0;
                                                for (int indexChar = 0; indexChar < runCharLen; indexChar++)
                                                {
                                                    if (char.IsWhiteSpace(runText[indexChar]))
                                                    {
                                                        ssumWidthDouble += advanceWidthList[glyphClusterList[indexChar]] + addedWordSpacing;
                                                        int glyphWidthScaled = (int)ssumWidthDouble - ssumWidthInt;
                                                        ssumWidthInt += glyphWidthScaled;
                                                        advanceWidthList[glyphClusterList[indexChar]] += glyphWidthScaled;
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region calculate chars widths
                                            //int runLength = (int)(abc.abcA + abc.abcB + abc.abcC);
                                            int runLength = 0;
                                            for (int indexChar = 0; indexChar < runCharLen; indexChar++)
                                            {
                                                int currentScriptLen = 0;
                                                if (runText[indexChar] == '\t')
                                                {
                                                    currentScriptLen = GetTabsWidth(textOptions, tabSpaceWidth, sumLen + runLength);
                                                }
                                                else
                                                {
                                                    int clusterNumber = glyphClusterList[indexChar]; //index of first glyph
                                                    //find last char in cluster
                                                    while ((indexChar < runCharLen - 1) && (glyphClusterList[indexChar + 1] == clusterNumber))
                                                    {
                                                        indexChar++;
                                                    }
                                                    int indexGlyphBegin = clusterNumber;
                                                    int indexGlyphEnd = clusterNumber;
                                                    if (scriptItemList[indexRun].a.fRTL)
                                                    {
                                                        indexGlyphBegin = (indexChar + 1 < runCharLen ? glyphClusterList[indexChar + 1] + 1 : 0);
                                                        indexGlyphEnd++;
                                                    }
                                                    else
                                                    {
                                                        indexGlyphEnd = (indexChar + 1 < runCharLen ? glyphClusterList[indexChar + 1] : glyphIndexCount);
                                                    }
                                                    for (int indexGlyph = indexGlyphBegin; indexGlyph < indexGlyphEnd; indexGlyph++)
                                                    {
                                                        currentScriptLen += advanceWidthList[indexGlyph];
                                                    }
                                                }
                                                runLength += currentScriptLen;
                                                lineWidths[runCharPos + indexChar] = currentScriptLen;
                                                if (isIndent && runCharPos + indexChar + 1 == indentCount && !isWrapped)
                                                {
                                                    indentCalcSize = sumLen + runLength;
                                                }
                                            }
                                            #endregion

                                            #region calculate boundsWidth
                                            double boundsWidth = Int32.MaxValue;
                                            bool needTrim = false;
                                            if (trimming != StringTrimming.None)
                                            {
                                                boundsWidth = textSize.Width;
                                                if ((!wordWrap) || (wordWrapPoints.Count + 1 == linesCountLimit))
                                                {
                                                    needTrim = true;
                                                }
                                                if ((trimming == StringTrimming.EllipsisCharacter) || (trimming == StringTrimming.EllipsisWord))
                                                {
                                                    if (needTrim) boundsWidth = textSize.Width - ellipsisWidth;
                                                }
                                            }
                                            else
                                            {
                                                if (wordWrap) boundsWidth = textSize.Width - indentSize; // - stateList[currentStateIndex].TS.Indent * 40;
                                            }
                                            #endregion

                                            if (Math.Round((decimal)((sumLen + runLength) * scale), precisionDigits) <= (decimal)boundsWidth)
                                            {
                                                //run fully in rect
                                                sumLen += runLength;
                                                //lineWidths[runCharPos] = runLength;
                                            }
                                            else
                                            {
                                                #region find wordWrap point

                                                #region ScriptBreak
                                                //SCRIPT_LOGATTR[] logAttrList = new SCRIPT_LOGATTR[runCharLen];

                                                IntPtr ptrLogAttrList = Marshal.AllocHGlobal(runCharLen + 2);

                                                error = ScriptBreak(
                                                    runText,
                                                    runCharLen,
                                                    ref scriptItemList[indexRun].a,
                                                    //ref logAttrList[0]
                                                    ptrLogAttrList
                                                    );
                                                if (error != 0)
                                                {
                                                    Marshal.FreeHGlobal(ptrLogAttrList);
                                                    ThrowError(14, error);
                                                }

                                                byte[] tempBuf = new byte[runCharLen];
                                                Marshal.Copy(ptrLogAttrList, tempBuf, 0, runCharLen);
                                                Marshal.FreeHGlobal(ptrLogAttrList);

                                                SCRIPT_LOGATTR[] logAttrList = new SCRIPT_LOGATTR[runCharLen];
                                                for (int indexChar = 0; indexChar < runCharLen; indexChar++)
                                                {
                                                    logAttrList[indexChar].packed = tempBuf[indexChar];
                                                    char ch = runText[indexChar];
                                                    //if (ch == '!' || ch == '%' || ch == ',' || ch == '.' || ch == '/' || ch == ';')
                                                    //{
                                                    //    logAttrList[indexChar].fSoftBreak = true;
                                                    //}
                                                    if (ch == '(' || ch == '{')
                                                    {
                                                        logAttrList[indexChar].fSoftBreak = true;
                                                    }
                                                    if (indexChar > 0)
                                                    {
                                                        ch = runText[indexChar - 1];
                                                        if (ch == '!' || ch == '%' || ch == ')' || ch == '}' || ch == '-' || ch == '?')
                                                        {
                                                            logAttrList[indexChar].fSoftBreak = true;
                                                        }
                                                        if (runText[indexChar] == '´' && char.IsLetterOrDigit(ch))
                                                        {
                                                            logAttrList[indexChar].fSoftBreak = false;
                                                        }
                                                    }
                                                }
                                                #endregion

                                                int scriptLen = 0;
                                                int scriptLenSp = 0;
                                                int scriptLastCharPos = 0;
                                                int lineCharOffset = 0;
                                                for (int indexChar = 0; indexChar < runCharLen; indexChar++)
                                                {
                                                    if (logAttrList[indexChar].fSoftBreak)
                                                    {
                                                        scriptLenSp = scriptLen;
                                                        scriptLastCharPos = indexChar;
                                                    }

                                                    #region calculate current script length
                                                    if (runText[indexChar] == '\t')
                                                    {
                                                        int tabWidth = GetTabsWidth(textOptions, tabSpaceWidth, sumLen + scriptLen);
                                                        lineWidths[runCharPos + indexChar] = tabWidth;
                                                    }

                                                    int clusterNumber = glyphClusterList[indexChar]; //index of first glyph
                                                    //find last char in cluster
                                                    while ((indexChar < runCharLen - 1) && (glyphClusterList[indexChar + 1] == clusterNumber))
                                                    {
                                                        indexChar++;
                                                    }
                                                    scriptLen += lineWidths[runCharPos + indexChar];
                                                    #endregion

                                                    if (!logAttrList[indexChar].fWhiteSpace) scriptLenSp = scriptLen;

                                                    #region calculate boundsWidth
                                                    boundsWidth = Int32.MaxValue;
                                                    needTrim = false;
                                                    if (trimming != StringTrimming.None)
                                                    {
                                                        boundsWidth = textSize.Width;
                                                        if ((!wordWrap) || (wordWrapPoints.Count + 1 == linesCountLimit))
                                                        {
                                                            needTrim = true;
                                                        }
                                                        if ((trimming == StringTrimming.EllipsisCharacter) || (trimming == StringTrimming.EllipsisWord))
                                                        {
                                                            if (needTrim) boundsWidth = textSize.Width - ellipsisWidth;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (wordWrap) boundsWidth = textSize.Width - indentSize; // - stateList[currentStateIndex].TS.Indent * 40;
                                                    }
                                                    #endregion

                                                    //check if script intersect rect
                                                    if (Math.Round((decimal)((sumLen + scriptLenSp) * scale), precisionDigits) > (decimal)boundsWidth)
                                                    {
                                                        #region add line
                                                        if (needTrim)
                                                        {
                                                            #region trimming
                                                            if ((trimming == StringTrimming.Character) || (trimming == StringTrimming.EllipsisCharacter))
                                                            {
                                                                textLen = runCharPos + indexChar;
                                                                if (textLen == 0) textLen++;
                                                            }
                                                            if ((trimming == StringTrimming.Word) || (trimming == StringTrimming.EllipsisWord))
                                                            {
                                                                //get last script in rect
                                                                textLen = runCharPos + scriptLastCharPos;
                                                                if (textLen == 0) textLen = runCharPos + indexChar;
                                                                if (textLen == 0) textLen++;
                                                            }
                                                            indexRun = scriptItemCount;

                                                            if (lastLineOffset < textLen)
                                                            {
                                                                var pairTrim = new LineInfo();
                                                                pairTrim.Begin = lastLineOffset;
                                                                pairTrim.End = textLen;
                                                                pairTrim.NeedWidthAlign = false;
                                                                if ((trimming == StringTrimming.EllipsisCharacter) || (trimming == StringTrimming.EllipsisWord))
                                                                {
                                                                    pairTrim.Text = textLine.Substring(pairTrim.Begin, pairTrim.Length) + "…";
                                                                }
                                                                wordWrapPoints.Add(pairTrim);
                                                                lastLineOffset = textLen;
                                                            }
                                                            #endregion

                                                            break;
                                                        }

                                                        bool needNowrap = false;
                                                        if (nowrapList[runCharPos + indexChar] == 1)
                                                        {
                                                            int indexOfBegin = runCharPos + indexChar - 1;
                                                            while (nowrapList[indexOfBegin] == 1) indexOfBegin--;
                                                            if (indexOfBegin == lastLineOffset)
                                                            {
                                                                needNowrap = true;
                                                            }
                                                        }
                                                        if (((scriptLastCharPos == lineCharOffset) && (sumLen == 0)) || needNowrap)
                                                        {
                                                            //only one word in line; break it
                                                            lineCharOffset = indexChar;
                                                            if ((lineCharOffset == 0) && (!needNowrap)) lineCharOffset++;
                                                            if (runCharPos + lineCharOffset - lastLineOffset == 0) lineCharOffset++;
                                                        }
                                                        else
                                                        {
                                                            //get last script in rect
                                                            lineCharOffset = scriptLastCharPos;
                                                        }

                                                        var pairWrapPoint = new LineInfo();
                                                        pairWrapPoint.Begin = lastLineOffset;
                                                        pairWrapPoint.End = runCharPos + lineCharOffset;
                                                        pairWrapPoint.NeedWidthAlign = true;
                                                        wordWrapPoints.Add(pairWrapPoint);
                                                        if (isWrapped) pairWrapPoint.Indent = indentCalcSize;

                                                        indentSize = indentCalcSize;
                                                        isWrapped = true;

                                                        #region trim
                                                        while ((pairWrapPoint.End > pairWrapPoint.Begin + 1) && (Char.IsWhiteSpace(textLine[pairWrapPoint.End - 1])))
                                                        {
                                                            pairWrapPoint.End--;
                                                        }
                                                        while ((lineCharOffset < runCharLen - 1) && (Char.IsWhiteSpace(textLine[runCharPos + lineCharOffset])))
                                                        {
                                                            lineCharOffset++;
                                                        }
                                                        #endregion

                                                        scriptLen = 0;
                                                        scriptLenSp = 0;
                                                        sumLen = 0;
                                                        scriptLastCharPos = lineCharOffset;
                                                        lastLineOffset = runCharPos + lineCharOffset;
                                                        indexChar = lineCharOffset - 1;

                                                        if ((lastLineOffset < textLen) && (nowrapList[lastLineOffset] == 1) && (nowrapLastRunIndex != 0))
                                                        {
                                                            lastLineOffset = scriptItemList[nowrapLastRunIndex].iCharPos;
                                                            pairWrapPoint.End = lastLineOffset;
                                                            indexRun = nowrapLastRunIndex - 1;
                                                            nowrapLastUsedRun = nowrapLastRunIndex;
                                                            nowrapLastRunIndex = 0;
                                                            //trim
                                                            while ((pairWrapPoint.End > pairWrapPoint.Begin + 1) && (Char.IsWhiteSpace(textLine[pairWrapPoint.End - 1])))
                                                            {
                                                                pairWrapPoint.End--;
                                                            }
                                                            break;
                                                        }

                                                        nowrapLastRunIndex = 0;
                                                        nowrapLastUsedRun = 0;
                                                        #endregion
                                                    }
                                                }
                                                #endregion

                                                sumLen += scriptLen;
                                            }
                                            #endregion
                                        }

                                        #region add last line if need
                                        if (lastLineOffset < textLen)
                                        {
                                            var pairWrapPoint = new LineInfo();
                                            pairWrapPoint.Begin = lastLineOffset;
                                            pairWrapPoint.End = textLen;
                                            pairWrapPoint.NeedWidthAlign = false;
                                            if (((trimming == StringTrimming.EllipsisCharacter) || (trimming == StringTrimming.EllipsisWord)) &&
                                                ((wordWrapPoints.Count + 1 == linesCountLimit) && (indexLine + 1 < linesInfo.Count)))
                                            {
                                                pairWrapPoint.Text = textLine.Substring(pairWrapPoint.Begin, pairWrapPoint.Length) + "…";
                                            }
                                            if (isWrapped) pairWrapPoint.Indent = indentCalcSize;
                                            wordWrapPoints.Add(pairWrapPoint);
                                        }
                                        #endregion

                                        #region calculate width justify and line width
                                        for (int indexWrapLine = lastLineIndex; indexWrapLine < wordWrapPoints.Count; indexWrapLine++)
                                        {
                                            LineInfo line = wordWrapPoints[indexWrapLine];
                                            int lineWidth = 0;
                                            int spaceCount = 0;
                                            for (int indexChar = line.Begin; indexChar < line.End; indexChar++)
                                            {
                                                lineWidth += lineWidths[indexChar];
                                                char ch = text[textLinePair.Begin + indexChar];
                                                if (char.IsWhiteSpace(ch) && ch != '\xA0')
                                                {
                                                    spaceCount++;
                                                }
                                            }
                                            if (line.Text != null) lineWidth += ellipsisWidth;
                                            line.Width = (int)(lineWidth * scale);
                                            line.Begin += textLinePair.Begin;
                                            //if ((line.NeedWidthAlign) && (spaceCount > 0))
                                            if (spaceCount > 0)
                                            {
                                                double lineJustifyOffset = (textSize.Width - line.Width - line.Indent) / (double)spaceCount;
                                                if (lineJustifyOffset > 0)
                                                {
                                                    line.JustifyOffset = lineJustifyOffset;
                                                }
                                                else
                                                {
                                                    line.NeedWidthAlign = false;
                                                }
                                            }
                                        }
                                        lastLineIndex = wordWrapPoints.Count;
                                        #endregion
                                    }
                                }
                                linesInfo = wordWrapPoints;
                            }
                            #endregion

                            if ((!lineLimit) && (trimming == StringTrimming.None)) linesCountLimit = linesInfo.Count;

                            #endregion

                            #region measure text
                            int maxWidth = 0;
                            double fullTextHeight = 0;
                            linesCountCalc = 0;
                            for (int indexLine = 0; indexLine < linesInfo.Count; indexLine++)
                            {
                                var lineInfo = linesInfo[indexLine];
                                if (maxWidth < lineInfo.Width) maxWidth = lineInfo.Width;
                                //calculate max font size for this line
                                int fontIndex = stateList[stateOrder[lineInfo.Begin]].FontIndex;
                                for (int indexChar = lineInfo.Begin + 1; indexChar < lineInfo.End; indexChar++)
                                {
                                    int tempFontIndex = stateList[stateOrder[indexChar]].FontIndex;
                                    if (fontList[fontIndex].FontBase.Size < fontList[tempFontIndex].FontBase.Size)
                                    {
                                        fontIndex = tempFontIndex;
                                    }
                                }
                                lineInfo.IndexOfMaxFont = fontIndex;

                                //double lineSpace = stateList[stateOrder[lineInfo.End - (lineInfo.Length > 0 ? 1 : 0)]].LineHeight;
                                double lineSpace = 1;
                                if (indexLine != linesInfo.Count - 1)
                                {
                                    var nextLineInfo = linesInfo[indexLine + 1];
                                    int stateIndex = nextLineInfo.Begin;
                                    if (stateIndex > 0) stateIndex--;
                                    lineSpace = stateList[stateOrder[stateIndex]].TS.LineHeight;
                                }
                                lineInfo.LineHeight = fontList[fontIndex].LineHeight * lineSpace;
                                fullTextHeight += lineInfo.LineHeight;
                                if ((fullTextHeight < textRect.Height) ||
                                    (fullTextHeight - lineInfo.LineHeight + fontList[fontIndex].LineHeight < textRect.Height))
                                {
                                    linesCountCalc++;
                                }
                                lineInfo.TextAlignment = stateList[stateOrder[lineInfo.End - (lineInfo.Length > 0 ? 1 : 0)]].TS.TextAlign;

                                //for test only
                                //lineInfo.Text = text.Substring(lineInfo.Begin, lineInfo.Length);
                            }
                            if (lineLimit) linesCountLimit = linesCountCalc;
                            measureSize.Width = maxWidth;
                            measureSize.Height = (int)Math.Round((decimal)fullTextHeight);
                            if (linesInfo.Count == 0) measureSize.Height = 0;
                            if (optimizeBottomMargin)
                            {
                                if (linesInfo.Count == 1) measureSize.Height += fontList[0].LineHeight * 0.07;
                                else if (linesInfo.Count == 2) measureSize.Height += fontList[0].LineHeight * 0.085;
                                else if (linesInfo.Count > 2) measureSize.Height += fontList[0].LineHeight * 0.1;
                            }
                            else
                            {
                                if (linesInfo.Count == 1) measureSize.Height += fontList[0].LineHeight * 0.1;
                                if (linesInfo.Count > 1) measureSize.Height += fontList[0].LineHeight * 0.4;
                            }
                            if (((angle > 45) && (angle < 135)) || ((angle > 225) && (angle < 315)))
                            {
                                double tempValue = measureSize.Width;
                                measureSize.Width = measureSize.Height;
                                measureSize.Height = tempValue;
                            }
                            if (measureSize.Width > textRect.Width && wordWrap) measureSize.Width = textRect.Width;
                            if (measureSize.Height > textRect.Height) measureSize.Height = textRect.Height;
                            measureSize.Width += 3 * scale;
                            #endregion

                            if (textLinesArray != null)
                            {
                                for (int index = 0; index < linesInfo.Count; index++)
                                {
                                    var lineInfo = linesInfo[index];
                                    string textLine = lineInfo.Text;
                                    if (textLine == null) textLine = text.Substring(lineInfo.Begin, lineInfo.Length);
                                    if (lineInfo.Indent > 0) textLine = GetIndentString(lineInfo.Indent) + textLine;
                                    textLinesArray.Add(textLine);
                                    if (textLinesInfo != null) textLinesInfo.Add(lineInfo);
                                }
                            }

                            if (needDraw)
                            {
                                if (!Compatibility2009)
                                {
                                    #region set world transformation
                                    float scaleX = 1;
                                    float scaleY = 1;
                                    if ((gUnit != GraphicsUnit.Display) && (gUnit != GraphicsUnit.Pixel))
                                    {
                                        #region scale for print
                                        oldMapMode = SetMapMode(hdc, MM_ANISOTROPIC);
                                        if (oldMapMode == 0) ThrowError(4);

                                        float maxX = pageF.Width + (pageF.X > 0 ? pageF.X : 0);
                                        float maxY = pageF.Height + (pageF.Y > 0 ? pageF.Y : 0);

                                        scaleX = dpiX / 100f;
                                        scaleY = dpiY / 100f;

                                        if (pageScale != 0.01)
                                        {
                                            scaleX *= pageScale / 0.01f;
                                            scaleY *= pageScale / 0.01f;
                                        }

                                        float maxXP = maxX * scaleX;
                                        float maxYP = maxY * scaleY;

                                        SIZE windowSize = new SIZE();
                                        gdiError = SetWindowExtEx(
                                            hdc,
                                            (int)maxX,
                                            (int)maxY,
                                            out windowSize);
                                        if (!gdiError) ThrowError(5);

                                        SIZE viewportSize = new SIZE();
                                        gdiError = SetViewportExtEx(
                                            hdc,
                                            (int)maxXP,
                                            (int)maxYP,
                                            out viewportSize);
                                        if (!gdiError) ThrowError(6);
                                        #endregion
                                    }

                                    gdiError = GetWorldTransform(hdc, out oldXForm);
                                    if (!gdiError) ThrowError(7);

                                    //translate dx, dy
                                    XFORM newXForm1 = new XFORM(1, 0, 0, 1,
                                        textRect.Left + textRect.Width / 2f,
                                        textRect.Top + textRect.Height / 2f);
                                    gdiError = ModifyWorldTransform(hdc, ref newXForm1, MWT_LEFTMULTIPLY);
                                    if (!gdiError) ThrowError(8);

                                    //rotate
                                    XFORM newXForm2 = new XFORM(
                                        Math.Cos(angleRad),
                                        Math.Sin(angleRad),
                                        -Math.Sin(angleRad),
                                        Math.Cos(angleRad),
                                        0, 0);
                                    gdiError = ModifyWorldTransform(hdc, ref newXForm2, MWT_LEFTMULTIPLY);
                                    if (!gdiError) ThrowError(9);

                                    //translate dx, dy
                                    XFORM newXForm3 = new XFORM(1, 0, 0, 1,
                                        -textSize.Width / 2f,
                                        -textSize.Height / 2f);
                                    gdiError = ModifyWorldTransform(hdc, ref newXForm3, MWT_LEFTMULTIPLY);
                                    if (!gdiError) ThrowError(10);

                                    #endregion

                                    #region set drawing parameters
                                    if (!foreColor.IsEmpty)
                                    {
                                        SetTextColor(hdc, ColorToWin32(foreColor));
                                    }
                                    DeviceContextBackgroundMode newMode1 = (backColor.IsEmpty || (backColor == Color.Transparent)) ? DeviceContextBackgroundMode.Transparent : DeviceContextBackgroundMode.Opaque;
                                    SetBkMode(hdc, (int)newMode1);
                                    if (newMode1 != DeviceContextBackgroundMode.Transparent)
                                    {
                                        SetBkColor(hdc, ColorToWin32(backColor));
                                    }

                                    resultGetClip = GetClipRgn(hdc, oldRegion);
                                    newRegion = CreateRectRgn(
                                        (int)(regionRect.Left * scaleX),
                                        (int)(regionRect.Top * scaleY),
                                        (int)(regionRect.Right * scaleX),
                                        (int)(regionRect.Bottom * scaleY));
                                    SelectClipRgn(hdc, newRegion);
                                    #endregion
                                }

                                int linesCount = linesInfo.Count;
                                if (linesCount > linesCountLimit) linesCount = linesCountLimit;

                                #region vertical alignment
                                double rectY = 0;
                                double textHeight = 0;
                                for (int indexLine = 0; indexLine < linesCount; indexLine++)
                                {
                                    textHeight += linesInfo[indexLine].LineHeight;
                                }
                                textHeight = (int)textHeight;
                                StiVertAlignment vertAlignment = vertAlign;
                                if ((angle != 0) && (angle != 90) && (angle != 180) && (angle != 270))
                                {
                                    vertAlignment = StiVertAlignment.Center;
                                }
                                switch (vertAlignment)
                                {
                                    case StiVertAlignment.Center:
                                        rectY += (textSize.Height - textHeight) / 2;
                                        break;

                                    case StiVertAlignment.Bottom:
                                        rectY += textSize.Height - textHeight;
                                        break;
                                }
                                #endregion

                                decimal yposDecimal = (decimal)rectY;
                                for (int indexLine = 0; indexLine < linesCount; indexLine++)
                                {
                                    var textLineInfo = linesInfo[indexLine];
                                    if (textLineInfo.Length > 0)
                                    {
                                        string textLine = text.Substring(textLineInfo.Begin, textLineInfo.Length);
                                        if (textLineInfo.Text != null) textLine = textLineInfo.Text;
                                        if (textLine.IndexOf('\x2011') != -1) textLine = textLine.Replace('\x2011', '-');
                                        //if (textLineInfo.Indent > 0) textLine = GetIndentString(textLineInfo.Indent) + textLine;

                                        #region horizontal alignment
                                        double rectX = textLineInfo.Indent * scale; // stateList[stateOrder[textLineInfo.Begin]].TS.Indent * 40;

                                        double textLineWidth = textLineInfo.Width;

                                        StiTextHorAlignment lineHorAlign = textLineInfo.TextAlignment;
                                        if (rightToLeft)
                                        {
                                            if (textLineInfo.TextAlignment == StiTextHorAlignment.Left) lineHorAlign = StiTextHorAlignment.Right;
                                            if (textLineInfo.TextAlignment == StiTextHorAlignment.Right) lineHorAlign = StiTextHorAlignment.Left;
                                        }
                                        if (forceWidthAlign && (indexLine == linesInfo.Count - 1))
                                        {
                                            textLineInfo.NeedWidthAlign = true;
                                        }
                                        if ((lineHorAlign == StiTextHorAlignment.Width) && (!textLineInfo.NeedWidthAlign))
                                        {
                                            if (rightToLeft) lineHorAlign = StiTextHorAlignment.Right;
                                            else lineHorAlign = StiTextHorAlignment.Left;
                                        }
                                        switch (lineHorAlign)
                                        {
                                            case StiTextHorAlignment.Center:
                                                rectX += (textSize.Width - textLineWidth) / 2f;
                                                break;

                                            case StiTextHorAlignment.Right:
                                                rectX += textSize.Width - textLineWidth;
                                                break;
                                        }
                                        #endregion

                                        #region out text

                                        #region ScriptItemize
                                        SCRIPT_ITEM[] scriptItemList = null;
                                        int scriptItemCount = 0;
                                        int error = 0;
                                        int cMaxItems = 10;
                                        do
                                        {
                                            cMaxItems *= 10;
                                            SCRIPT_CONTROL psControl = new SCRIPT_CONTROL();
                                            SCRIPT_STATE psState = new SCRIPT_STATE();
                                            //scriptItemList = new SCRIPT_ITEM[cMaxItems];
                                            psState.uBidiLevel = (ushort)(rightToLeft ? 1 : 0);

                                            IntPtr buf = Marshal.AllocHGlobal(sizeofScriptItem * cMaxItems + 1);

                                            error = ScriptItemize(
                                                textLine,
                                                textLine.Length,
                                                cMaxItems,
                                                ref psControl,
                                                ref psState,
                                                //ref scriptItemList[0],
                                                buf,
                                                out scriptItemCount);
                                            if ((error != 0) && (error != E_OUTOFMEMORY))
                                            {
                                                Marshal.FreeHGlobal(buf);
                                                ThrowError(15, error);
                                            }

                                            scriptItemList = new SCRIPT_ITEM[scriptItemCount + 1];
                                            IntPtr offset = buf;
                                            for (int indexItem = 0; indexItem < scriptItemCount + 1; indexItem++)
                                            {
                                                scriptItemList[indexItem] = (SCRIPT_ITEM)Marshal.PtrToStructure(offset, typeof(SCRIPT_ITEM));
                                                offset = (IntPtr)((Int64)offset + sizeofScriptItem);
                                            }
                                            Marshal.FreeHGlobal(buf);

                                        }
                                        while (error == E_OUTOFMEMORY);
                                        int store_scriptItemCount = scriptItemCount;
                                        #endregion

                                        #region break runs in nowrapBegin points
                                        //if (existNowrapPoints)
                                        {
                                            var newScriptItemList = new List<SCRIPT_ITEM>();
                                            newScriptItemList.Add(scriptItemList[0]);
                                            int itemIndex = 0;
                                            for (int indexChar = 0; indexChar < textLine.Length; indexChar++)
                                            {
                                                if (indexChar == scriptItemList[itemIndex + 1].iCharPos)
                                                {
                                                    itemIndex++;
                                                    newScriptItemList.Add(scriptItemList[itemIndex]);
                                                    continue;
                                                }
                                                if ((indexChar > 0) && (stateOrder[textLineInfo.Begin + indexChar] != stateOrder[textLineInfo.Begin + indexChar - 1]))
                                                {
                                                    SCRIPT_ITEM si = scriptItemList[itemIndex];
                                                    si.iCharPos = indexChar;
                                                    newScriptItemList.Add(si);
                                                    continue;
                                                }
                                            }
                                            newScriptItemList.Add(scriptItemList[scriptItemCount]);
                                            scriptItemList = new SCRIPT_ITEM[newScriptItemList.Count];
                                            for (int indexRun = 0; indexRun < newScriptItemList.Count; indexRun++)
                                            {
                                                scriptItemList[indexRun] = newScriptItemList[indexRun];
                                            }
                                            scriptItemCount = newScriptItemList.Count - 1;
                                        }
                                        #endregion

                                        #region ScriptLayout
                                        byte[] bidiLevel = new byte[scriptItemCount];
                                        int[] visualToLogicalList = new int[scriptItemCount];
                                        int[] logicalToVisualList = new int[scriptItemCount];

                                        for (int index = 0; index < scriptItemCount; index++)
                                        {
                                            bidiLevel[index] = (byte)scriptItemList[index].a.s.uBidiLevel;
                                        }

                                        error = ScriptLayout(scriptItemCount, bidiLevel, visualToLogicalList, logicalToVisualList);
                                        if (error != 0)
                                        {
                                            ThrowError(16, error);
                                        }
                                        #endregion

                                        int textLen = textLine.Length;
                                        int bufLen = textLen * 2;
                                        if (bufLen < 20) bufLen = 20;
                                        int sumLen = 0;
                                        double xpos = rectX;
                                        //int ypos = (int)Math.Round(yposDecimal);

                                        // process each "run" of text 
                                        for (int indexRun = 0; indexRun < scriptItemCount; indexRun++)
                                        {
                                            ushort[] glyphIndexList = new ushort[bufLen];
                                            ushort[] glyphClusterList = new ushort[textLen];
                                            //SCRIPT_VISATTR[] scriptVisAttrList = new SCRIPT_VISATTR[bufLen];
                                            IntPtr scriptVisAttrList = Marshal.AllocHGlobal(sizeofScriptVisattr * bufLen);
                                            //GOFFSET[] goff = new GOFFSET[bufLen];
                                            int[] advanceWidthList = new int[bufLen];
                                            ABC abc;
                                            int glyphIndexCount;

                                            int vidx = visualToLogicalList[indexRun];

                                            int runCharPos = scriptItemList[vidx].iCharPos;
                                            int runCharLen = scriptItemList[vidx + 1].iCharPos - runCharPos;
                                            string runText = textLine.Substring(runCharPos, runCharLen);

                                            currentStateIndex = stateOrder[textLineInfo.Begin + runCharPos];
                                            StiFontState currentFontState = fontList[stateList[currentStateIndex].FontIndex];
                                            //IntPtr hhFontScaled = fontList[stateList[currentStateIndex].FontIndex].hFontScaled;
                                            //IntPtr hhFont = fontList[stateList[currentStateIndex].FontIndex].hFont;
                                            bool useScale = currentFontState.hFontScaled != IntPtr.Zero;
                                            if (!useGdiPlus)
                                            {
                                                if (useScale)
                                                {
                                                    SelectObject(hdc, currentFontState.hFontScaled);
                                                }
                                                else
                                                {
                                                    SelectObject(hdc, currentFontState.hFont);
                                                }
                                            }
                                            double ypos = (double)yposDecimal;
                                            if (stateList[currentStateIndex].TS.Superscript || stateList[currentStateIndex].TS.Subsript)
                                            {
                                                StiFontState parentState = fontList[currentFontState.ParentFontIndex];
                                                ypos += fontList[textLineInfo.IndexOfMaxFont].Ascend - parentState.Ascend;
                                                if (stateList[currentStateIndex].TS.Subsript)
                                                {
                                                    ypos += parentState.LineHeight - (currentFontState.Ascend + currentFontState.Descend);
                                                }
                                            }
                                            else
                                            {
                                                ypos += fontList[textLineInfo.IndexOfMaxFont].Ascend - currentFontState.Ascend;
                                            }

                                            if (!useGdiPlus)
                                            {
                                                if (!stateList[currentStateIndex].TS.FontColor.IsEmpty)
                                                {
                                                    SetTextColor(hdc, ColorToWin32(stateList[currentStateIndex].TS.FontColor));
                                                }
                                                DeviceContextBackgroundMode newMode = (stateList[currentStateIndex].TS.BackColor.IsEmpty || (stateList[currentStateIndex].TS.BackColor == Color.Transparent)) ? DeviceContextBackgroundMode.Transparent : DeviceContextBackgroundMode.Opaque;
                                                SetBkMode(hdc, (int)newMode);
                                                if (newMode != DeviceContextBackgroundMode.Transparent)
                                                {
                                                    SetBkColor(hdc, ColorToWin32(stateList[currentStateIndex].TS.BackColor));
                                                }
                                            }

                                            IntPtr hScriptCache = useScale ? currentFontState.hScriptCacheScaled : currentFontState.hScriptCache;

                                            #region ScriptShape
                                            error = ScriptShape(
                                                hdc,
                                                ref hScriptCache,
                                                runText,
                                                runCharLen,
                                                textLen * 2,
                                                ref scriptItemList[vidx].a,
                                                glyphIndexList,
                                                glyphClusterList,
                                                //ref scriptVisAttrList[0],
                                                scriptVisAttrList,
                                                out glyphIndexCount
                                                );
                                            if (error == E_SCRIPT_NOT_IN_FONT)  //fix
                                            {
                                                error = 0;
                                                glyphIndexCount = runCharLen;
                                                for (int tIndex = 0; tIndex < glyphIndexCount; tIndex++)
                                                {
                                                    glyphClusterList[tIndex] = (ushort)tIndex;
                                                }
                                                scriptItemList[indexRun].a.packed = 0;
                                            }
                                            if ((error != 0) && (error != E_SCRIPT_NOT_IN_FONT))
                                            {
                                                Marshal.FreeHGlobal(scriptVisAttrList);
                                                ThrowError(17, error);
                                            }
                                            int store_error17 = error;
                                            #endregion

                                            IntPtr goff = Marshal.AllocHGlobal(sizeofGoffset * bufLen);

                                            //if (currentFontState.hFontScaled != IntPtr.Zero) SelectObject(hdc, currentFontState.hFontScaled);

                                            #region ScriptPlace
                                            int summaryWidthInt = 0;
                                            if (useScale)
                                            {
                                                SelectObject(hdc, currentFontState.hFont);
                                                error = ScriptPlace(
                                                    hdc,
                                                    ref currentFontState.hScriptCache,
                                                    glyphIndexList,
                                                    glyphIndexCount,
                                                    //ref scriptVisAttrList[0],
                                                    scriptVisAttrList,
                                                    ref scriptItemList[vidx].a,
                                                    advanceWidthList,
                                                    //ref goff[0],
                                                    goff,
                                                    out abc
                                                    );
                                                if (error != 0)
                                                {
                                                    Marshal.FreeHGlobal(goff);
                                                    Marshal.FreeHGlobal(scriptVisAttrList);
                                                    ThrowError(1801, error);
                                                }
                                                SelectObject(hdc, currentFontState.hFontScaled);

                                                if (CorrectionEnabled)
                                                {
                                                    #region AdvanceWidthList correction
                                                    ushort[] allGlyphWidth = GetFontWidth(currentFontState.FontBase);
                                                    if (allGlyphWidth.Length > 0)
                                                    {
                                                        double fontScale = MaxFontSize / currentFontState.EmValue;
                                                        for (int indexGlyph = 0; indexGlyph < glyphIndexCount; indexGlyph++)
                                                        {
                                                            int tempGlyphIndex = glyphIndexList[indexGlyph];
                                                            if (tempGlyphIndex >= allGlyphWidth.Length) tempGlyphIndex = allGlyphWidth.Length - 1;
                                                            double width = allGlyphWidth[tempGlyphIndex] / fontScale;
                                                            if (advanceWidthList[indexGlyph] < width - 0.4)
                                                            {
                                                                int newWidth = (int)Math.Round(width);
                                                                if (advanceWidthList[indexGlyph] >= newWidth) newWidth++;
                                                                advanceWidthList[indexGlyph] = newWidth;
                                                            }
                                                            else
                                                            {
                                                                double percent = advanceWidthList[indexGlyph] / width;
                                                                if (advanceWidthList[indexGlyph] - width > 1 && percent > 1.1)
                                                                {
                                                                    advanceWidthList[indexGlyph] = (int)Math.Ceiling(width);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    #endregion
                                                }

                                                for (int indexGlyph = 0; indexGlyph < glyphIndexCount; indexGlyph++)
                                                {
                                                    summaryWidthInt += advanceWidthList[indexGlyph];
                                                }
                                            }

                                            error = ScriptPlace(
                                                hdc,
                                                ref hScriptCache,
                                                glyphIndexList,
                                                glyphIndexCount,
                                                //ref scriptVisAttrList[0],
                                                scriptVisAttrList,
                                                ref scriptItemList[vidx].a,
                                                advanceWidthList,
                                                //ref goff[0],
                                                goff,
                                                out abc
                                                );
                                            if (error != 0)
                                            {
                                                Marshal.FreeHGlobal(goff);
                                                Marshal.FreeHGlobal(scriptVisAttrList);
                                                ThrowError(18, error);
                                            }

                                            Marshal.FreeHGlobal(scriptVisAttrList);
                                            #endregion

                                            if (CorrectionEnabled)
                                            {
                                                #region AdvanceWidthList correction
                                                ushort[] allGlyphWidth = GetFontWidth(currentFontState.FontBase);
                                                if (allGlyphWidth.Length > 0)
                                                {
                                                    double fontScale = MaxFontSize / currentFontState.EmValue / scale;
                                                    //double summm1 = 0;
                                                    //for (int indexGlyph = 0; indexGlyph < glyphIndexCount; indexGlyph++)
                                                    //{
                                                    //    summm1 += advanceWidthList[indexGlyph];
                                                    //}
                                                    //double summm2 = 0;
                                                    for (int indexGlyph = 0; indexGlyph < glyphIndexCount; indexGlyph++)
                                                    {
                                                        int tempGlyphIndex = glyphIndexList[indexGlyph];
                                                        if (tempGlyphIndex >= allGlyphWidth.Length) tempGlyphIndex = allGlyphWidth.Length - 1;
                                                        double width = allGlyphWidth[tempGlyphIndex] / fontScale;
                                                        if (advanceWidthList[indexGlyph] < width - 0.4)
                                                        {
                                                            int newWidth = (int)Math.Round(width);
                                                            if (advanceWidthList[indexGlyph] >= newWidth) newWidth++;
                                                            advanceWidthList[indexGlyph] = newWidth;
                                                        }
                                                        else
                                                        {
                                                            double percent = advanceWidthList[indexGlyph] / width;
                                                            if (advanceWidthList[indexGlyph] - width > 1 && percent > 1.1)
                                                            {
                                                                advanceWidthList[indexGlyph] = (int)Math.Ceiling(width);
                                                            }
                                                        }
                                                        //summm2 += advanceWidthList[indexGlyph];
                                                    }
                                                }
                                                #endregion
                                            }

                                            #region AdvanceWidthList correction for scale
                                            if (useScale)
                                            {
                                                int summaryScaledInt = 0;
                                                for (int indexGlyph = 0; indexGlyph < glyphIndexCount; indexGlyph++)
                                                {
                                                    summaryScaledInt += advanceWidthList[indexGlyph];
                                                }
                                                if (summaryScaledInt > 0)
                                                {
                                                    double correctionValue = (summaryWidthInt * scale) / summaryScaledInt;
                                                    int ssumWidthInt = 0;
                                                    double ssumWidthDouble = 0;
                                                    //double summm3 = 0;
                                                    for (int indexGlyph = 0; indexGlyph < glyphIndexCount; indexGlyph++)
                                                    {
                                                        ssumWidthDouble += advanceWidthList[indexGlyph] * correctionValue;
                                                        int glyphWidthScaled = (int)ssumWidthDouble - ssumWidthInt;
                                                        ssumWidthInt += glyphWidthScaled;
                                                        advanceWidthList[indexGlyph] = glyphWidthScaled;
                                                        //summm3 += advanceWidthList[indexGlyph];
                                                    }
                                                }
                                            }
                                            #endregion

                                            //if (currentFontState.hFontScaled != IntPtr.Zero) SelectObject(hdc, currentFontState.hFont);

                                            #region store base advanceWidthList
                                            int[] baseAdvanceWidthList = null;
                                            if (outRunsList != null)
                                            {
                                                baseAdvanceWidthList = new int[glyphIndexCount];
                                                for (int indexChar = 0; indexChar < glyphIndexCount; indexChar++)
                                                {
                                                    baseAdvanceWidthList[indexChar] = advanceWidthList[indexChar];
                                                }
                                            }
                                            #endregion

                                            #region Apply LetterSpacing and WordSpacing
                                            double additionalSpacing = 0;
                                            if (stateList[currentStateIndex].TS.LetterSpacing != 0)
                                            {
                                                //double addedLetterSpacing = currentFontState.EmValue * stateList[currentStateIndex].TS.LetterSpacing;
                                                //double addedLetterSpacing = currentFontState.EmValue * 1.35 * stateList[currentStateIndex].TS.LetterSpacing;
                                                double addedLetterSpacing = currentFontState.EmValue * 1.35 * stateList[currentStateIndex].TS.LetterSpacing * scale;
                                                int ssumWidthInt = 0;
                                                double ssumWidthDouble = 0;
                                                for (int indexGlyph = 0; indexGlyph < glyphIndexCount; indexGlyph++)
                                                {
                                                    ssumWidthDouble += advanceWidthList[indexGlyph] + addedLetterSpacing;
                                                    int glyphWidthScaled = (int)ssumWidthDouble - ssumWidthInt;
                                                    ssumWidthInt += glyphWidthScaled;
                                                    advanceWidthList[indexGlyph] = glyphWidthScaled;
                                                }
                                                additionalSpacing += addedLetterSpacing * glyphIndexCount;
                                            }
                                            if (stateList[currentStateIndex].TS.WordSpacing != 0)
                                            {
                                                //double addedWordSpacing = currentFontState.EmValue * stateList[currentStateIndex].TS.WordSpacing;
                                                double addedWordSpacing = currentFontState.EmValue * stateList[currentStateIndex].TS.WordSpacing * scale;
                                                int ssumWidthInt = 0;
                                                double ssumWidthDouble = 0;
                                                for (int indexChar = 0; indexChar < runCharLen; indexChar++)
                                                {
                                                    if (char.IsWhiteSpace(runText[indexChar]))
                                                    {
                                                        ssumWidthDouble += advanceWidthList[glyphClusterList[indexChar]] + addedWordSpacing;
                                                        int glyphWidthScaled = (int)ssumWidthDouble - ssumWidthInt;
                                                        ssumWidthInt += glyphWidthScaled;
                                                        advanceWidthList[glyphClusterList[indexChar]] += glyphWidthScaled;
                                                        additionalSpacing += addedWordSpacing;
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region calculate runLength and check for simplyRun
                                            int runLength = 0;
                                            bool simplyRun = !scriptItemList[vidx].a.fRTL;
                                            for (int indexChar = 0; indexChar < runCharLen; indexChar++)
                                            {
                                                int currentScriptLen = 0;
                                                if (runText[indexChar] == '\t')
                                                {
                                                    //currentScriptLen = GetTabsWidth(tFirstTabOffset, tDistanceBetweenTabs, tabSpaceWidth, sumLen + runLength);
                                                    int tabOffset = GetTabsWidth(textOptions, tabSpaceWidth * scale, sumLen + runLength);
                                                    currentScriptLen = tabOffset;
                                                    summaryWidthInt += (int)(tabOffset / scale);
                                                }
                                                else
                                                {
                                                    int clusterNumber = glyphClusterList[indexChar]; //index of first glyph
                                                    //find last char in cluster
                                                    while ((indexChar < runCharLen - 1) && (glyphClusterList[indexChar + 1] == clusterNumber))
                                                    {
                                                        indexChar++;
                                                        simplyRun = false;
                                                    }
                                                    int indexGlyphBegin = clusterNumber;
                                                    int indexGlyphEnd = clusterNumber;
                                                    if (scriptItemList[indexRun].a.fRTL)
                                                    {
                                                        indexGlyphBegin = (indexChar + 1 < runCharLen ? glyphClusterList[indexChar + 1] + 1 : 0);
                                                        indexGlyphEnd++;
                                                    }
                                                    else
                                                    {
                                                        indexGlyphEnd = (indexChar + 1 < runCharLen ? glyphClusterList[indexChar + 1] : glyphIndexCount);
                                                    }
                                                    for (int indexGlyph = indexGlyphBegin; indexGlyph < indexGlyphEnd; indexGlyph++)
                                                    {
                                                        currentScriptLen += advanceWidthList[indexGlyph];
                                                    }
                                                    if (indexGlyphEnd - indexGlyphBegin > 1) simplyRun = false;
                                                }
                                                runLength += currentScriptLen;
                                            }
                                            #endregion

                                            //if (angle != 0) simplyRun = false;

                                            #region scale and justify correction
                                            //int sumWidthInt = 0;
                                            //double sumWidthDouble = 0;
                                            //if (currentFontState.hFontScaled != IntPtr.Zero)
                                            //{
                                            //    for (int indexGlyph = 0; indexGlyph < glyphIndexCount; indexGlyph++)
                                            //    {
                                            //        sumWidthDouble += advanceWidthList[indexGlyph] * scale;
                                            //        int glyphWidthScaled = (int)sumWidthDouble - sumWidthInt;
                                            //        sumWidthInt += glyphWidthScaled;
                                            //        advanceWidthList[indexGlyph] = glyphWidthScaled;
                                            //    }
                                            //}

                                            int sumJustifyInt = 0;
                                            double sumJustifyDouble = 0;
                                            if (lineHorAlign == StiTextHorAlignment.Width)
                                            {
                                                for (int indexChar = 0; indexChar < runCharLen; indexChar++)
                                                {
                                                    if (char.IsWhiteSpace(runText[indexChar]) && runText[indexChar] != '\xA0')
                                                    {
                                                        sumJustifyDouble += textLineInfo.JustifyOffset;
                                                        int justifyOffset = (int)(sumJustifyDouble - sumJustifyInt);
                                                        sumJustifyInt += justifyOffset;
                                                        advanceWidthList[glyphClusterList[indexChar]] += justifyOffset;
                                                    }
                                                }
                                            }
                                            #endregion

                                            if (simplyRun && useGdiPlus || outRunsList != null)
                                            //  || ((glyphIndexCount == 0) && (runCharLen > 0)))   //added 2009.03.04 as trial fix
                                            {

                                                var runinfo = new RunInfo();
                                                runinfo.Text = runText;
                                                runinfo.XPos = xpos;
                                                runinfo.YPos = ypos;
                                                runinfo.Widths = new int[runCharLen];

                                                #region fill widths table
                                                for (int indexChar = 0; indexChar < runCharLen; indexChar++)
                                                {
                                                    int beginIndexChar = indexChar;
                                                    int clusterNumber = glyphClusterList[indexChar]; //index of first glyph
                                                    //find last char in cluster
                                                    while ((indexChar < runCharLen - 1) && (glyphClusterList[indexChar + 1] == clusterNumber))
                                                    {
                                                        indexChar++;
                                                    }
                                                    int indexGlyphBegin = clusterNumber;
                                                    int indexGlyphEnd = clusterNumber;
                                                    if (scriptItemList[indexRun].a.fRTL)
                                                    {
                                                        indexGlyphBegin = (indexChar + 1 < runCharLen ? glyphClusterList[indexChar + 1] + 1 : 0);
                                                        indexGlyphEnd++;
                                                    }
                                                    else
                                                    {
                                                        indexGlyphEnd = (indexChar + 1 < runCharLen ? glyphClusterList[indexChar + 1] : glyphIndexCount);
                                                    }

                                                    if ((beginIndexChar != indexChar) || (indexGlyphEnd - indexGlyphBegin > 1))
                                                    {
                                                        int currentScriptLen = 0;
                                                        for (int indexGlyph = indexGlyphBegin; indexGlyph < indexGlyphEnd; indexGlyph++)
                                                        {
                                                            currentScriptLen += advanceWidthList[indexGlyph];
                                                        }
                                                        if (indexChar != beginIndexChar)
                                                        {
                                                            int charsCount = indexChar - beginIndexChar + 1;
                                                            double tempWidth = currentScriptLen / (double)charsCount;
                                                            double currWidthDouble = 0;
                                                            int currWidthInt = 0;
                                                            for (int tempIndex = 0; tempIndex < charsCount - 1; tempIndex++)
                                                            {
                                                                currWidthDouble += tempWidth;
                                                                int currentValue = (int)Math.Round(currWidthDouble) - currWidthInt;
                                                                currWidthInt += currentValue;
                                                                runinfo.Widths[beginIndexChar + tempIndex] = currentValue;
                                                            }
                                                            runinfo.Widths[indexChar] = currentScriptLen - currWidthInt;
                                                        }
                                                        else
                                                        {
                                                            runinfo.Widths[indexChar] = currentScriptLen;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        runinfo.Widths[indexChar] = advanceWidthList[indexGlyphBegin];
                                                    }
                                                }
                                                #endregion

                                                if (baseAdvanceWidthList == null) baseAdvanceWidthList = new int[glyphIndexCount];

                                                runinfo.GlyphIndexList = new int[glyphIndexCount];
                                                runinfo.GlyphWidths = new int[glyphIndexCount];
                                                runinfo.ScaleList = new double[glyphIndexCount];
                                                for (int indexGlyph = 0; indexGlyph < glyphIndexCount; indexGlyph++)
                                                {
                                                    runinfo.GlyphIndexList[indexGlyph] = glyphIndexList[indexGlyph];
                                                    runinfo.GlyphWidths[indexGlyph] = advanceWidthList[indexGlyph];
                                                    runinfo.ScaleList[indexGlyph] = 1;
                                                    if (baseAdvanceWidthList[indexGlyph] != 0)
                                                    {
                                                        runinfo.ScaleList[indexGlyph] = advanceWidthList[indexGlyph] / (double)baseAdvanceWidthList[indexGlyph];
                                                    }
                                                }
                                                runinfo.TextColor = stateList[currentStateIndex].TS.FontColor;
                                                runinfo.BackColor = stateList[currentStateIndex].TS.BackColor;
                                                runinfo.FontIndex = stateList[currentStateIndex].FontIndex;
                                                runs.Add(runinfo);
                                            }
                                            else
                                            {
                                                //if (currentFontState.hFontScaled != IntPtr.Zero) SelectObject(hdc, currentFontState.hFontScaled);

                                                #region ScriptTextOut
                                                error = ScriptTextOut(
                                                    hdc,
                                                    ref hScriptCache,
                                                    (int)Math.Round((decimal)xpos),
                                                    (int)Math.Round((decimal)ypos),
                                                    0,
                                                    ref lpRect,					// no clipping
                                                    ref scriptItemList[vidx].a,
                                                    IntPtr.Zero,
                                                    0,
                                                    glyphIndexList,
                                                    glyphIndexCount,
                                                    advanceWidthList,
                                                    IntPtr.Zero,
                                                    //ref goff[0]
                                                    goff
                                                    );
                                                if (error != 0)
                                                {
                                                    Marshal.FreeHGlobal(goff);

                                                    #region throw error
                                                    StringBuilder stb = new StringBuilder();
                                                    try
                                                    {
                                                        stb.Append(string.Format(" indexLine={0}",
                                                            indexLine));
                                                        stb.Append(string.Format(" scriptItemCount={0}",
                                                            store_scriptItemCount));
                                                        stb.Append(string.Format(" indexRun={0}",
                                                            indexRun));
                                                        stb.Append(string.Format(" runCharLen={0}",
                                                            runCharLen));
                                                        stb.Append(string.Format(" runText={0}",
                                                            runText == null ? "null" : "\"" + runText + "\""));
                                                        stb.Append(string.Format(" err17={0}",
                                                            store_error17));
                                                        stb.Append(string.Format(" hdc={0:X8} hScriptCache={1:X8} x={2:X4} y={3:X4}",
                                                            hdc.ToInt64(),
                                                            currentFontState.hScriptCache.ToInt64(),
                                                            (int)Math.Round((decimal)xpos),
                                                            (int)Math.Round((decimal)ypos)));
                                                        stb.Append(string.Format(" lpRect=({0:X4},{1:X4},{2:X4},{3:X4})",
                                                            lpRect.left,
                                                            lpRect.right,
                                                            lpRect.top,
                                                            lpRect.bottom));
                                                        stb.Append(string.Format(" scriptItemList=({0:X8},{1:X8})",
                                                            scriptItemList[vidx].a.packed,
                                                            scriptItemList[vidx].a.s.packed));
                                                        if (glyphIndexList != null)
                                                        {
                                                            stb.Append(string.Format(" glyphIndexList=[{0}]",
                                                                glyphIndexList.Length));
                                                        }
                                                        else
                                                        {
                                                            stb.Append(" glyphIndexList=null");
                                                        }
                                                        stb.Append(string.Format(" glyphIndexCount={0}", glyphIndexCount));
                                                        if (advanceWidthList != null)
                                                        {
                                                            stb.Append(string.Format(" advanceWidthList=[{0}]",
                                                                advanceWidthList.Length));
                                                        }
                                                        else
                                                        {
                                                            stb.Append(" advanceWidthList=null");
                                                        }
                                                        //stb.Append(string.Format(" goff=[{0}]",
                                                        //    goff.Length));

                                                        stb.Append(".\r Parameters: ");
                                                        stb.Append(string.Format(" text={0}",
                                                            text == null ? "null" : "\"" + text + "\""));
                                                        stb.Append(string.Format(" font={0},{1}",
                                                            font.Name, font.Size));
                                                        string style = (font.Bold ? "B" : "") + (font.Italic ? "I" : "") + (font.Underline ? "U" : "");
                                                        if (style.Length > 0) stb.Append("," + style);
                                                        stb.Append(string.Format(" bounds=({0},{1},{2},{3})",
                                                            bounds.Left.ToString().Replace(',', '.'),
                                                            bounds.Right.ToString().Replace(',', '.'),
                                                            bounds.Top.ToString().Replace(',', '.'),
                                                            bounds.Bottom.ToString().Replace(',', '.')));
                                                        stb.Append(string.Format(" lineSpacing={0}",
                                                            lineSpacing));
                                                        stb.Append(string.Format(" wordwrap={0}",
                                                            wordWrap ? "true" : "false"));
                                                        stb.Append(string.Format(" rightToLeft={0}",
                                                            rightToLeft ? "true" : "false"));
                                                        stb.Append(string.Format(" scale={0}",
                                                            scale));
                                                        stb.Append(string.Format(" angle={0}",
                                                            angle));
                                                        stb.Append(string.Format(" trim={0}",
                                                            trimming.ToString()));
                                                        stb.Append(string.Format(" lineLimit={0}",
                                                            lineLimit ? "true" : "false"));
                                                        stb.Append(string.Format(" allowHtmlTags={0}",
                                                            allowHtmlTags ? "true" : "false"));
                                                    }
                                                    catch
                                                    {
                                                        ThrowError(19, error);
                                                    }
                                                    Win32Exception myEx = new Win32Exception(Marshal.GetLastWin32Error());
                                                    throw new Exception(string.Format("TextRender error at step {0}, code #{1:X8}(#{2:X8}): {3} \r{4}",
                                                        19,
                                                        myEx.ErrorCode,
                                                        error,
                                                        myEx.Message,
                                                        stb.ToString()));
                                                    #endregion
                                                }
                                                #endregion

                                                //if (currentFontState.hFontScaled != IntPtr.Zero) SelectObject(hdc, currentFontState.hFont);
                                            }
                                            Marshal.FreeHGlobal(goff);

                                            if (useScale)
                                            {
                                                if (currentFontState.hScriptCacheScaled != hScriptCache) currentFontState.hScriptCacheScaled = hScriptCache;
                                            }
                                            else
                                            {
                                                if (currentFontState.hScriptCache != hScriptCache) currentFontState.hScriptCache = hScriptCache;
                                            }

                                            //xpos += runLength * scale + sumJustifyDouble;
                                            if (useScale)
                                            {
                                                xpos += summaryWidthInt * scale + sumJustifyDouble + additionalSpacing;
                                            }
                                            else
                                            {
                                                xpos += runLength + sumJustifyDouble;
                                            }
                                            sumLen += runLength;
                                        }
                                        #endregion
                                    }
                                    yposDecimal += (decimal)textLineInfo.LineHeight;
                                }

                                #region restore clipping
                                if (resultGetClip == 1)
                                {
                                    SelectClipRgn(hdc, oldRegion);
                                }
                                if (resultGetClip == 0)
                                {
                                    SelectClipRgn(hdc, IntPtr.Zero);
                                }
                                #endregion

                                DeleteObject(newRegion);

                                #region restore world transformation
                                gdiError = SetWorldTransform(hdc, ref oldXForm);
                                if (!gdiError) ThrowError(20);

                                if (oldMapMode != 0)
                                {
                                    int tempMapMode = SetMapMode(hdc, oldMapMode);
                                    if (tempMapMode == 0) ThrowError(21);
                                }

                                int tempGraphMode = SetGraphicsMode(hdc, oldGraphMode);
                                if (tempGraphMode == 0) ThrowError(22);
                                #endregion
                            }

                            //ScriptFreeCache(ref scriptCache);

                            #region break text
                            if (!needDraw)
                            {
                                if (linesInfo.Count > linesCountLimit)
                                {
                                    forceWidthAlign = (linesCountLimit > 0) && (linesInfo[linesCountLimit - 1]).NeedWidthAlign;
                                    if (allowHtmlTags)
                                    {
                                        var lineInfo = linesInfo[linesCountLimit];
                                        int statePos = stateOrder[lineInfo.Begin];
                                        StiHtmlState state = stateList[statePos];

                                        int posLine = lineInfo.Begin;
                                        while ((posLine > 0) && (stateOrder[posLine - 1] == stateOrder[posLine])) posLine--;
                                        int offset = lineInfo.Begin - posLine;
                                        string stateText = PrepareStateText(state.Text).ToString();
                                        if (linesCountLimit > 0)
                                        {
                                            string writtenTextEnd = null;
                                            if ((statePos > 0) && (stateList[statePos - 1].TS.Tag == StiHtmlTag.ListItem) && (state.TS.Tag == StiHtmlTag.ListItem))
                                            {
                                                if (stateList[statePos - 1].ListLevels.Count == state.ListLevels.Count)
                                                {
                                                    writtenTextEnd = "<li>" + stateText.Substring(0, offset);
                                                }
                                                else
                                                {
                                                    writtenTextEnd = (state.ListLevels[state.ListLevels.Count - 1] > 0 ? "<ol>" : "<ul>") + stateText.Substring(0, offset);
                                                }
                                            }
                                            else
                                            {
                                                writtenTextEnd = StateToHtml(state, stateText.Substring(0, offset), lineInfo.Indent);
                                            }
                                            writtenText = originalText.Substring(0, state.PosBegin) +
                                                (offset > 0 ? writtenTextEnd : "") +
                                                (forceWidthAlign ? StiForceWidthAlignTag : "");
                                        }
                                        else
                                        {
                                            writtenText = string.Empty;
                                        }
                                        int stateIndex = stateOrder[lineInfo.Begin] + 1;
                                        breakText = StateToHtml(state.TS.Tag == StiHtmlTag.ListItem ? stateList[stateIndex] : state, stateText.Substring(offset), lineInfo.Indent);
                                        if (state.TS.Tag == StiHtmlTag.ListItem && stateIndex < stateList.Length)
                                        {
                                            breakText += stateList[stateIndex].Text;
                                            stateIndex++;
                                        }
                                        if (stateIndex < stateList.Length)
                                        {
                                            breakText += originalText.Substring(stateList[stateIndex].PosBegin);
                                        }
                                    }
                                    else
                                    {
                                        LineInfo lineInfo = null;
                                        if (linesCountLimit > 0)
                                        {
                                            lineInfo = linesInfo[linesCountLimit - 1];
                                            writtenText = text.Substring(0, lineInfo.End) + (forceWidthAlign ? StiForceWidthAlignTag : "");
                                        }
                                        else
                                        {
                                            writtenText = string.Empty;
                                        }
                                        lineInfo = linesInfo[linesCountLimit];
                                        breakText = text.Substring(lineInfo.Begin);
                                    }
                                }
                            }
                            #endregion

                        }
                        finally
                        {
                            for (int indexFont = 0; indexFont < fontList.Length; indexFont++)
                            {
                                bool error = DeleteObject(fontList[indexFont].hFont);
                                if (!error) ThrowError(23);
                                ScriptFreeCache(ref fontList[indexFont].hScriptCache);
                                if (fontList[indexFont].hScriptCacheScaled != IntPtr.Zero)
                                {
                                    ScriptFreeCache(ref fontList[indexFont].hScriptCacheScaled);
                                }
                            }
                        }
                    }
                    finally
                    {
                        g.ReleaseHdc(hdc);
                    }

                    if (runs.Count > 0)
                    {
                        if (outRunsList != null)
                        {
                            #region Store data for Wpf
                            outRunsList.Clear();
                            outRunsList.AddRange(runs);
                            outFontsList.Clear();
                            outFontsList.AddRange(fontList);
                            #endregion
                        }
                        else
                        {
                            #region draw with GDI+
                            var sf = new StringFormat(StringFormat.GenericTypographic);

                            Region oldRegion = g.Clip;
                            RectangleF newRegion = new RectangleF(
                                (float)(bounds.X),
                                (float)(bounds.Y),
                                (float)(bounds.Width),
                                (float)(bounds.Height));
                            g.SetClip(newRegion, System.Drawing.Drawing2D.CombineMode.Intersect);
                            TextRenderingHint defaultHint = g.TextRenderingHint;
                            //g.TextRenderingHint = TextRenderingHint.AntiAlias;

                            System.Drawing.Drawing2D.GraphicsState gstate = g.Save();

                            #region Rotate
                            g.TranslateTransform(
                                (float)(bounds.X + 1.5 * scale + textRect.Width / 2f),
                                (float)(bounds.Y + textRect.Height / 2f));
                            g.RotateTransform((float)(-angle));
                            g.TranslateTransform(
                                (float)(-textSize.Width / 2f),
                                (float)(-textSize.Height / 2f));
                            #endregion

                            #region Draw lines
                            for (int indexRun = 0; indexRun < runs.Count; indexRun++)
                            {
                                var runInfo = runs[indexRun];
                                //double baseX = runInfo.XPos + bounds.X + 1.5 * scale;
                                //double baseY = runInfo.YPos + bounds.Y;
                                double baseX = runInfo.XPos;
                                double baseY = runInfo.YPos;

                                Font fontDraw = fontList[runInfo.FontIndex].FontBase;
                                if (fontList[runInfo.FontIndex].hFontScaled != IntPtr.Zero) fontDraw = fontList[runInfo.FontIndex].FontScaled;
                                var brush = new SolidBrush(runInfo.TextColor);

                                if (runInfo.BackColor != Color.Transparent)
                                {
                                    double width = 0;
                                    for (int indexChar = 0; indexChar < runInfo.Text.Length; indexChar++)
                                    {
                                        width += runInfo.Widths[indexChar];
                                    }

                                    var rect = new RectangleF();
                                    rect.X = (float)baseX;
                                    rect.Y = (float)baseY;
                                    rect.Width = (float)width;
                                    rect.Height = (float)fontList[runInfo.FontIndex].LineHeight;
                                    g.FillRectangle(new SolidBrush(runInfo.BackColor), rect);
                                }

                                for (int indexChar = 0; indexChar < runInfo.Text.Length; indexChar++)
                                {
                                    char ch = runInfo.Text[indexChar];
                                    //if (ch == ' ') ch = '_';
                                    g.DrawString(
                                        ch.ToString(),
                                        fontDraw,
                                        brush,
                                        (float)(baseX),
                                        (float)(baseY),
                                        sf);
                                    baseX += runInfo.Widths[indexChar];
                                }

                            }
                            #endregion

                            g.Restore(gstate);

                            g.TextRenderingHint = defaultHint;
                            g.Clip = oldRegion;
                            #endregion
                        }
                    }
                }
                catch
                {

                }
				finally
				{
                    for (int indexFont = 0; indexFont < fontList.Length; indexFont++)
                    {
                        if (fontList[indexFont].hFontScaled != IntPtr.Zero)
                        {
                            bool error = DeleteObject(fontList[indexFont].hFontScaled);
                            if (!error) ThrowError(24);
                        }
                    }
				}
			}

			text = breakText;
			return writtenText;
		}

        private static OUTLINETEXTMETRIC GetOutlineTextMetricsCached(string fontName, FontStyle fontStyle, IntPtr hdc)
        {
            string st = fontName + "*" + (char)(48 + (int)fontStyle);
            object obj = OutlineTextMetricsCache[st];
            if (obj == null)
            {
                OUTLINETEXTMETRIC otm = new OUTLINETEXTMETRIC();

                lock (lockOutlineTextMetricsCache)
                {
                    using (Font tempFont = StiFontCollection.CreateFont(fontName, MaxFontSize, fontStyle))
                    {
                        IntPtr hTempFont = tempFont.ToHfont();
                        try
                        {
                            SelectObject(hdc, hTempFont);

                            uint cbSize = GetOutlineTextMetrics(hdc, 0, IntPtr.Zero);
                            if (cbSize == 0) ThrowError(1);
                            IntPtr buffer = Marshal.AllocHGlobal((int)cbSize);
                            try
                            {
                                if (GetOutlineTextMetrics(hdc, cbSize, buffer) != 0)
                                {
                                    otm = (OUTLINETEXTMETRIC)Marshal.PtrToStructure(buffer, typeof(OUTLINETEXTMETRIC));
                                }
                            }
                            finally
                            {
                                Marshal.FreeHGlobal(buffer);
                            }
                        }
                        finally
                        {
                            bool error = DeleteObject(hTempFont);
                            if (!error) ThrowError(2);
                        }
                    }

                    OutlineTextMetricsCache[st] = otm;
                }

                return otm;
            }
            return (OUTLINETEXTMETRIC)obj;
        }

        public static StringBuilder PrepareStateText(StringBuilder stateText)
        {
            //convert &#xxxx; to unicode symbols
            StringBuilder sbTemp = new StringBuilder();
            int indexChar = 0;
            while (indexChar < stateText.Length)
            {
                bool flag = false;
                if ((stateText[indexChar] == '&') && (indexChar + 3 < stateText.Length))
                {
                    int indexChar2 = indexChar + 1;
                    StringBuilder sbTemp2 = new StringBuilder();
                    if (stateText[indexChar2] == '#')
                    {
                        indexChar2++;
                        while ((indexChar2 < stateText.Length) && char.IsDigit(stateText[indexChar2]))
                        {
                            sbTemp2.Append(stateText[indexChar2]);
                            indexChar2++;
                        }
                        if ((sbTemp2.Length > 0) && (indexChar2 < stateText.Length) && (stateText[indexChar2] == ';'))
                        {
                            indexChar2++;
                            sbTemp.Append((char) (uint.Parse(sbTemp2.ToString())));
                            indexChar = indexChar2;
                            flag = true;
                        }
                    }
                    else
                    {
                        while ((indexChar2 < stateText.Length) && char.IsLetterOrDigit(stateText[indexChar2]))
                        {
                            sbTemp2.Append(stateText[indexChar2]);
                            indexChar2++;
                        }
                        if ((sbTemp2.Length > 0) && (indexChar2 < stateText.Length) && (stateText[indexChar2] == ';'))
                        {
                            object es = HtmlEscapeSequence["&" + sbTemp2 + ";"];
                            if (es != null)
                            {
                                indexChar2++;
                                sbTemp.Append((char) es);
                                indexChar = indexChar2;
                                flag = true;
                            }
                        }
                    }
                }
                if (!flag)
                {
                    sbTemp.Append(stateText[indexChar]);
                    indexChar++;
                }
            }

            sbTemp.Replace("&nbsp;", "\xA0")
                  .Replace("&lt;", "<")
                  .Replace("&gt;", ">")
                  .Replace("&quot;", "\"")
                  .Replace("&amp;", "&");
            return sbTemp;
        }

        //private static bool isWordWrapSymbol(string text, int pos)
        //{
        //    char sym1 = text[pos];
        //    if (sym1 == '(' || sym1 == '{') return true;
        //    if (pos > 0)
        //    {
        //        char sym2 = text[pos - 1];
        //        if (sym2 == '!' || sym2 == '%' || sym2 == ')' || sym2 == '}' || sym2 == '-' || sym2 == '?') return true;
        //    }
        //    return false;
        //}
        private static bool isWordWrapSymbol2(string text, int pos)
        {
            char sym1 = text[pos];
            //return (sym1 == '(' || sym1 == '{' || sym1 == '!' || sym1 == '%' || sym1 == ')' || sym1 == '}' || sym1 == '-' || sym1 == '?');
            return (sym1 == '!' || sym1 == '%' || sym1 == ')' || sym1 == '}' || sym1 == '-' || sym1 == '?' ||
                sym1 == '）' || sym1 == '：' || sym1 == '、' || sym1 == '，' || sym1 == '。');   //CJK punctuation
        }
        private static bool isNotWordWrapSymbol(string text, int pos)
        {
            UnicodeCategory uc = char.GetUnicodeCategory(text[pos]);
            bool result = uc == UnicodeCategory.OtherPunctuation || uc == UnicodeCategory.MathSymbol || uc == UnicodeCategory.CurrencySymbol;
            if (pos > 0 && isWordWrapSymbol2(text, pos - 1)) result = false;
            return result;
        }
        private static bool isNotWordWrapSymbol2(string text, int pos)
        {
            UnicodeCategory uc = char.GetUnicodeCategory(text[pos]);
            bool result = uc == UnicodeCategory.OtherPunctuation || uc == UnicodeCategory.MathSymbol || uc == UnicodeCategory.CurrencySymbol;
            return result || char.IsLetterOrDigit(text[pos]);
        }

        private static bool isCJKWordWrap(string text, int pos)
        {
            if ((pos > 0) && isCJKSymbol(text, pos))
            {
                return isCJKSymbol(text, pos - 1);
            }
            return false;
        }
        private static bool isCJKSymbol(string text, int pos)
        {
            char sym = text[pos];
            return (sym >= 0x4e00 && sym <= 0x9fcc) || (sym >= 0x3400 && sym <= 0x4db5); // || (sym >= 0x20000 && sym <= 0x2a6df); //char is only 16bit
        }

        private static string StackToString(List<StiHtmlTagsState> stack)
        {
            if (stack == null || stack.Count == 0) return string.Empty;
            var sb = new StringBuilder();
            try
            {
                for (int index = 0; index < stack.Count; index++)
                {
                    var state = stack[index];
                    var prevState = new StiHtmlTagsState();
                    bool first = index == 0;
                    if (!first) prevState = stack[index - 1];

                    if (state.IsBackcolorChanged) sb.AppendFormat("bc{0:X2}{1:X2}{2:X2}{3:X2}:", state.BackColor.A, state.BackColor.R, state.BackColor.G, state.BackColor.B);
                    if (state.Bold && (first || state.Bold != prevState.Bold)) sb.Append("bd:");
                    if (state.IsColorChanged) sb.AppendFormat("fc{0:X2}{1:X2}{2:X2}{3:X2}:", state.FontColor.A, state.FontColor.R, state.FontColor.G, state.FontColor.B);
                    if (!string.IsNullOrEmpty(state.FontName) && (first || state.FontName != prevState.FontName)) sb.AppendFormat("fn{0}:", state.FontName.Replace(' ', '_'));
                    if (first || state.FontSize != prevState.FontSize) sb.AppendFormat("fs{0}:", state.FontSize);
                    if (state.Italic && (first || state.Italic != prevState.Italic)) sb.Append("it:");
                    if (first || state.LetterSpacing != prevState.LetterSpacing) sb.AppendFormat("ls{0}:", state.LetterSpacing);
                    if (first || state.LineHeight != prevState.LineHeight) sb.AppendFormat("lh{0}:", state.LineHeight);
                    if (state.Strikeout && (first || state.Strikeout != prevState.Strikeout)) sb.Append("st:");
                    if (state.Subsript && (first || state.Subsript != prevState.Subsript)) sb.Append("sb:");
                    if (state.Superscript && (first || state.Superscript != prevState.Superscript)) sb.Append("sp:");
                    if (first || state.Tag != prevState.Tag) sb.AppendFormat("tg{0}:", (int)state.Tag);
                    if (first || state.TextAlign != prevState.TextAlign) sb.AppendFormat("ta{0}:", (int)state.TextAlign);
                    if (state.Underline && (first || state.Underline != prevState.Underline)) sb.Append("un:");
                    if (first || state.WordSpacing != prevState.WordSpacing) sb.AppendFormat("ws{0}:", state.WordSpacing);
                    if (sb[sb.Length - 1] == ':') sb.Length--;

                    if (index < stack.Count - 1) sb.Append(";");
                }
            }
            catch
            {
            }
            return sb.ToString();
        }
        private static List<StiHtmlTagsState> StringToStack(string inputString, StiHtmlTagsState baseState)
        {
            var lastState = new StiHtmlTagsState(baseState);
            var output = new List<StiHtmlTagsState>();
            try
            {
                string[] arr = inputString.Split(new char[] { ';' });
                foreach (string stState in arr)
                {
                    var state = new StiHtmlTagsState(lastState);
                    string[] arr2 = stState.Split(new char[] { ':' });
                    foreach (string stPart in arr2)
                    {
                        string stParam = stPart.Substring(2);
                        switch (stPart.Substring(0, 2))
                        {
                            case "bc":
                                state.BackColor = Color.FromArgb(
                                    int.Parse(stParam.Substring(0, 2), NumberStyles.HexNumber),
                                    int.Parse(stParam.Substring(2, 2), NumberStyles.HexNumber),
                                    int.Parse(stParam.Substring(4, 2), NumberStyles.HexNumber),
                                    int.Parse(stParam.Substring(6, 2), NumberStyles.HexNumber));
                                state.IsBackcolorChanged = true;
                                break;
                            case "bd":
                                state.Bold = true;
                                break;
                            case "fc":
                                state.FontColor = Color.FromArgb(
                                    int.Parse(stParam.Substring(0, 2), NumberStyles.HexNumber),
                                    int.Parse(stParam.Substring(2, 2), NumberStyles.HexNumber),
                                    int.Parse(stParam.Substring(4, 2), NumberStyles.HexNumber),
                                    int.Parse(stParam.Substring(6, 2), NumberStyles.HexNumber));
                                state.IsColorChanged = true;
                                break;
                            case "fn":
                                state.FontName = stParam.Replace('_', ' ');
                                break;
                            case "fs":
                                state.FontSize = float.Parse(stParam);
                                break;
                            case "it":
                                state.Italic = true;
                                break;
                            case "ls":
                                state.LetterSpacing = double.Parse(stParam);
                                break;
                            case "lh":
                                state.LineHeight = double.Parse(stParam);
                                break;
                            case "st":
                                state.Strikeout = true;
                                break;
                            case "sb":
                                state.Subsript = true;
                                break;
                            case "sp":
                                state.Superscript = true;
                                break;
                            case "tg":
                                state.Tag = (StiHtmlTag)int.Parse(stParam);
                                break;
                            case "ta":
                                state.TextAlign = (StiTextHorAlignment)int.Parse(stParam);
                                break;
                            case "un":
                                state.Underline = true;
                                break;
                            case "ws":
                                state.WordSpacing = double.Parse(stParam);
                                break;
                        }
                    }
                    output.Add(state);
                    lastState = state;
                }
            }
            catch
            {
            }
            return output;
        }

        private static string ListLevelsToString(List<int> list, int indent)
        {
            if (list == null || list.Count == 0) list = new List<int>();
            var sb = new StringBuilder();
            try
            {
                for (int index = 0; index < indent; index++)
                {
                    if (index < list.Count)
                    {
                        sb.Append(list[index].ToString());
                    }
                    else
                    {
                        sb.Append("0");
                    }
                    if (index < indent - 1) sb.Append(";");
                }
            }
            catch
            {
            }
            return sb.ToString();
        }
        private static List<int> StringToListLevels(string inputString)
        {
            var output = new List<int>();
            try
            {
                string[] arr = inputString.Split(new char[] { ';' });
                foreach (string marker in arr)
                {
                    output.Add(int.Parse(marker));
                }
            }
            catch
            {
            }
            return output;
        }
        #endregion

        #endregion
    }
}
