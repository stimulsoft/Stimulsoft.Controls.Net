#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports												}
{	                         										}
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
using System.Windows.Forms;
using System.Collections;

namespace Stimulsoft.Controls
{
	public class StiWizardPageCollection : CollectionBase
	{
		#region Collection
		public void Add(StiWizardPage page)
		{
			List.Add(page);
		}

		public void AddRange(StiWizardPage[] pages)
		{
			foreach (StiWizardPage page in pages)Add(page);
		}

		public void AddRange(StiWizardPageCollection pages)
		{
			foreach (StiWizardPage page in pages)Add(page);
		}

		public bool Contains(StiWizardPage page)
		{
			return List.Contains(page);
		}
		
		public int IndexOf(StiWizardPage page)
		{
			return List.IndexOf(page);
		}

		public void Insert(int index, StiWizardPage page)
		{
			List.Insert(index, page);
		}

		public void Remove(StiWizardPage page)
		{
			List.Remove(page);
		}

		public StiWizardPage this[int index]
		{
			get
			{
				return (StiWizardPage)List[index];
			}
			set
			{
				List[index] = value;
			}
		}
		#endregion

		#region Handlers
		protected override void OnInsert(int index, object value)
		{
			if (LockActions)return;
			parent.Controls.Add(value as Control);
		}

		protected override void OnRemove(int index, object value)
		{
			if (LockActions)return;
			parent.Controls.Remove(value as Control);
		}
 
		protected override void OnClear()
		{
			if (LockActions)return;

			int index = 0;
			while (index < parent.Controls.Count)
			{
				Control control = parent.Controls[index];
				if (control is StiWizardPage)
					parent.Controls.Remove(control);
				else 
					index++;
			}
		}
		#endregion

		private StiWizard parent;
		internal bool LockActions = false;

		public StiWizardPageCollection() : this(null)
		{
		}

		public StiWizardPageCollection(StiWizard parent)
		{
			this.parent = parent;
		}
	}
}
