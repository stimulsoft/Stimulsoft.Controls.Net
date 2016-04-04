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

namespace Stimulsoft.Base
{
    public class StiTestConnectionResult
    {        
        #region Properties
        /// <summary>
        /// A value which indicates the result of connection testing. True if connection tested successfully.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// A message which describes the result of connection testing.
        /// </summary>
        public string Notice { get; set; }
        #endregion 

        #region Methods.Static
        public static StiTestConnectionResult MakeWrong(string notice)
        {
            return new StiTestConnectionResult
            {
                Success = false,
                Notice = notice
            };
        }


        public static StiTestConnectionResult MakeWrong(Exception exception)
        {
            return new StiTestConnectionResult
            {
                Success = false,
                Notice = exception.Message
            };
        }

        public static StiTestConnectionResult MakeWrong()
        {
            return new StiTestConnectionResult
            {
                Success = false
            };
        }

        public static StiTestConnectionResult MakeFine()
        {
            return new StiTestConnectionResult
            {
                Success = true
            };
        }
        #endregion
    }
}
