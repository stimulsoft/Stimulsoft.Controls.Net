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
    public static class StiExceptionProvider
    {
        private static IStiCustomExceptionProvider customExceptionProvider;
        public static IStiCustomExceptionProvider CustomExceptionProvider
        {
            get
            {
                return customExceptionProvider;
            }
            set
            {
                customExceptionProvider = value;
            }
        }

        private static bool hideMessages = false;
        /// <summary>
        /// Gets or sets a value indicating whether not to show the message from engine of the message.
        /// </summary>
        public static bool HideMessages
        {
            get
            {
                return hideMessages;
            }
            set
            {
                hideMessages = value;
            }
        }

        private static bool hideExceptions = false;
        /// <summary>
        /// Gets or sets the value, which indicates not to show the exception from engine of the exception.
        /// </summary>
        public static bool HideExceptions
        {
            get
            {
                return hideExceptions;
            }
            set
            {
                hideExceptions = value;
            }
        }

        public static void Show(Exception exception)
        {
            if (StiExceptionProvider.CustomExceptionProvider != null)
                StiExceptionProvider.CustomExceptionProvider.Show(exception);
            else
            {
                if (!HideMessages)
                {
                    using (var form = new StiExceptionForm(exception))
                    {
                        form.ShowDialog();
                    }
                }
                else
                {
                    if (!HideExceptions) throw exception;
                }
            }
        }
    }
}
