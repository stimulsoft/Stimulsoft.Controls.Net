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
using System.Collections.Generic;
using System.CodeDom.Compiler;

namespace Stimulsoft.Base
{		
	/// <summary>
	/// Helps to compile scripts.
	/// </summary>
	public sealed class StiCompiler
    {
        #region Fields.Static
        /// <summary>
        /// An string of the version which will be used for compiled assembly. Example: 3.2.0.0
        /// </summary>
        public static string AssemblyVersion = null;
        #endregion

        #region LanguageType
        public enum LanguageType
		{
			/// <summary>
			/// CSharp language
			/// </summary>
			CSharp,

			/// <summary>
			/// Vb.Net language
			/// </summary>
			VB
		}
		#endregion

        /// <summary>
		/// Compile text to code.
		/// </summary>
		/// <param name="textToCompile">Text for compiling.</param>
		/// <param name="outputAssembly">Path where place the result of compiling, if way is not specified result place in memories.</param>
		/// <param name="outputType">Type of the result to compiling.</param>
		/// <param name="referencedAssemblies">Referenced assemblies</param>
		/// <returns>Result of compiling.</returns>
        public static CompilerResults Compile(string textToCompile, string outputAssembly, LanguageType languageType,
            StiOutputType outputType, string[] referencedAssemblies)
        {
            return Compile(textToCompile, outputAssembly, languageType, outputType, referencedAssemblies, null);
        }

		/// <summary>
		/// Compile text to code.
		/// </summary>
		/// <param name="textToCompile">Text for compiling.</param>
		/// <param name="outputAssembly">Path where place the result of compiling, if way is not specified result place in memories.</param>
		/// <param name="outputType">Type of the result to compiling.</param>
		/// <param name="referencedAssemblies">Referenced assemblies.</param>
        /// <param name="resources">Linked resources.</param>
		/// <returns>Result of compiling.</returns>
		public static CompilerResults Compile(string textToCompile, string outputAssembly, LanguageType languageType, 
			StiOutputType outputType, string[] referencedAssemblies, List<string> resources)
		{
			CompilerParameters parms = new CompilerParameters();

            #region Add Resources
            if (resources != null)
            {
                foreach (string resource in resources)
                {
                    parms.EmbeddedResources.Add(resource);
                }
            }
            #endregion

            parms.ReferencedAssemblies.AddRange(referencedAssemblies);

			parms.OutputAssembly = outputAssembly;
			if (outputAssembly.Length == 0)
			{
				parms.GenerateInMemory = true;
			}
			else 
			{
				parms.GenerateInMemory = false;
			}

			switch(outputType)
			{
				case StiOutputType.ClassLibrary:
					parms.CompilerOptions = parms.CompilerOptions + " /target:library";
					parms.GenerateExecutable = false;
					break;

				case StiOutputType.ConsoleApplication:
					parms.CompilerOptions = parms.CompilerOptions + " /target:exe";
					parms.GenerateExecutable = true;
					break;

				case StiOutputType.WindowsApplication:
					parms.CompilerOptions = parms.CompilerOptions + " /target:winexe";
					parms.GenerateExecutable = true;
					break;
			}
            
			CodeDomProvider provider = null;
			
			if (languageType == LanguageType.CSharp)provider = new Microsoft.CSharp.CSharpCodeProvider();
			else provider = new Microsoft.VisualBasic.VBCodeProvider();

			CompilerResults results = null;

            if (AssemblyVersion != null)
            {
                int namespaceIndex = textToCompile.IndexOf("namespace");

                textToCompile = textToCompile.Insert(namespaceIndex, "[assembly: System.Reflection.AssemblyVersion(\"" + AssemblyVersion + "\")]\n");
            }

            results = provider.CompileAssemblyFromSource(parms, textToCompile);
			provider.Dispose();
            
			return results;
		}
	}
}
