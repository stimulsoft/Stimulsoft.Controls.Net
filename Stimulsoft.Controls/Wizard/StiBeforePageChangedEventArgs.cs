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

namespace Stimulsoft.Controls
{
	public delegate void StiBeforePageChangedHandler(object sender, StiBeforePageChangedEventArgs e);

	public class StiBeforePageChangedEventArgs
	{
		private bool cancel = false;
		public bool Cancel
		{
			get
			{
				return cancel;
			}
			set
			{
				cancel = value;
			}
		}

		private StiWizardPage oldWizardPage = null;
		public StiWizardPage OldWizardPage
		{
			get
			{
				return oldWizardPage;
			}
		}

		private StiWizardPage newWizardPage = null;
		public StiWizardPage NewWizardPage
		{
			get
			{
				return newWizardPage;
			}
		}

		public StiBeforePageChangedEventArgs(StiWizardPage oldWizardPage, StiWizardPage newWizardPage)
		{
			this.oldWizardPage = oldWizardPage;
			this.newWizardPage = newWizardPage;
		}
	}
}