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

namespace Stimulsoft.Base
{	
	public class StiRepositoryItems : ICloneable
	{
		#region ICloneable
		public object Clone()
		{	
			StiRepositoryItems repository = new StiRepositoryItems();
			if (items != null)repository.items = items.Clone() as Hashtable;
			return repository;
		}
		#endregion

        #region Equals
        public bool Equals(StiRepositoryItems obj)
        {
            if (obj == null) return false;
            bool empty1 = items == null || items.Count == 0;
            bool empty2 = obj.items == null || obj.items.Count == 0;
            if (empty1 && empty2) return true;
            if (empty1 && !empty2 || !empty1 && empty2) return false;
            if (items.Count != obj.items.Count) return false;
            foreach (DictionaryEntry de in items)
            {
                if (!obj.items.ContainsKey(de.Key)) return false;
                object obj2 = obj.items[de.Key];
                if (de.Value == null)
                {
                    if (obj2 == null) return true;
                    else return false;
                }
                if (!de.Value.Equals(obj2)) return false;
                //if (de.Value != obj.items[de.Key]) return false;
            }
            return true;
        }
        #endregion

        #region IntStorage
        private class IntStorage
		{
			public int value;

			public IntStorage(int value)
			{
				this.value = value;
			}
		}
		#endregion

		#region FloatStorage
		private class FloatStorage
		{
			public float value;

			public FloatStorage(float value)
			{
				this.value = value;
			}
		}
		#endregion

		#region Fields
		private Hashtable items = null;

		private static object ValueBoolFalse = new object();
		private static object ValueBoolTrue = new object();
		#endregion

		#region Methods
		public void SetInt(object key, int value, int defaultValue)
		{
			if (value == defaultValue)
			{
				if (items == null)return;
                if (items[key] != null) items.Remove(key);
			}
			else
			{
				if (items == null)items = new Hashtable();
                items[key] = new IntStorage(value);
			}
		}


        public int GetInt(object key, int defaultValue)
		{
			if (items == null)return defaultValue;

            IntStorage value = items[key] as IntStorage;

			if (value == null)return defaultValue;
			else return value.value;
		}


        public void SetFloat(object key, float value, float defaultValue)
		{
			if (value == defaultValue)
			{
				if (items == null)return;
                if (items[key] != null) items.Remove(key);
			}
			else
			{
				if (items == null)items = new Hashtable();
                items[key] = new FloatStorage(value);
			}
		}


        public float GetFloat(object key, float defaultValue)
		{
			if (items == null)return defaultValue;

            FloatStorage value = items[key] as FloatStorage;

			if (value == null)return defaultValue;
			else return value.value;
		}


        public void SetBool(object key, bool value, bool defaultValue)
		{
			if (value == defaultValue)
			{
				if (items == null)return;
                if (items[key] != null) items.Remove(key);
			}
			else
			{
				if (items == null)items = new Hashtable();
                if (value) items[key] = ValueBoolTrue;
                else items[key] = ValueBoolFalse;
			}
		}


        public bool GetBool(object key, bool defaultValue)
		{
			if (items == null)return defaultValue;

            object value = items[key];
			if (value == ValueBoolFalse)return false;
			if (value == ValueBoolTrue)return true;
			return defaultValue;
		}


        public void Set(object key, object value, object defaultValue)
		{
			if (items == null)items = new Hashtable();
			if (value == null || value == defaultValue)
			{
                if (items[key] != null) items.Remove(key);
				return;
			}
            items[key] = value;
		}


        public object Get(object key, object defaultValue)
		{
			if (items == null)return defaultValue;
            object value = items[key];
			if (value == null)return defaultValue;
			return value;			
		}

        public bool IsPresent(object key)
        {
            if (items == null) return false;
            return items.ContainsKey(key);
        }
		#endregion
	}
}
