#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Linq;

namespace Stimulsoft.Base.Json.Linq
{
    public static class JsonExtensions
    {
        #region Методы для сохранения Отчета в JSON
        public static void RemoveProperty(this JObject jObject, string propertyName)
        {
            foreach (JProperty token in jObject.ChildrenTokens)
            {
                if (token.Name == propertyName)
                {
                    jObject.ChildrenTokens.Remove(token);
                    return;
                }
            }
        }

        public static void AddPropertyInt(this JObject jObject, string propertyName, int value, int defaultValue = 0)
        {
            // Ищем и удаляем свойство, если оно было уже добавлено ранее
            RemoveProperty(jObject, propertyName);

            if (value == defaultValue)
                return;

            jObject.Add(new JProperty(propertyName, value));
        }

        public static void AddPropertyIntNoDefaultValue(this JObject jObject, string propertyName, int value)
        {
            // Ищем и удаляем свойство, если оно было уже добавлено ранее
            RemoveProperty(jObject, propertyName);

            jObject.Add(new JProperty(propertyName, value));
        }

        public static void AddPropertyFloat(this JObject jObject, string propertyName, float value, float defaultValue)
        {
            // Ищем и удаляем свойство, если оно было уже добавлено ранее
            RemoveProperty(jObject, propertyName);

            if (value == defaultValue)
                return;

            jObject.Add(new JProperty(propertyName, value));
        }

        public static void AddPropertyFloatNullable(this JObject jObject, string propertyName, float? value, float? defaultValue)
        {
            // Ищем и удаляем свойство, если оно было уже добавлено ранее
            RemoveProperty(jObject, propertyName);

            if (value == defaultValue)
                return;

            jObject.Add(new JProperty(propertyName, value));
        }

        public static void AddPropertyDouble(this JObject jObject, string propertyName, double value, double defaultValue)
        {
            // Ищем и удаляем свойство, если оно было уже добавлено ранее
            RemoveProperty(jObject, propertyName);

            if (value == defaultValue)
                return;

            jObject.Add(new JProperty(propertyName, value));
        }

        public static void AddPropertyDouble(this JObject jObject, string propertyName, double value)
        {
            // Ищем и удаляем свойство, если оно было уже добавлено ранее
            RemoveProperty(jObject, propertyName);

            jObject.Add(new JProperty(propertyName, value));
        }

        public static void AddPropertyJObject(this JObject jObject, string propertyName, JObject value)
        {
            // Ищем и удаляем свойство, если оно было уже добавлено ранее
            RemoveProperty(jObject, propertyName);

            if (value == null || value.Count == 0)
                return;

            jObject.Add(new JProperty(propertyName, value));
        }

        public static void AddPropertyIdent(this JObject jObject, string propertyName, string value)
        {
            jObject.Add(new JProperty(propertyName, value));
        }

        public static void AddPropertyBool(this JObject jObject, string propertyName, bool value, bool defaultValue = false)
        {
            // Ищем и удаляем свойство, если оно было уже добавлено ранее
            RemoveProperty(jObject, propertyName);

            if (value == defaultValue)
                return;

            jObject.Add(new JProperty(propertyName, value));
        }

        // Записываем Ticks у заданной даты
        public static void AddPropertyDateTime(this JObject jObject, string propertyName, DateTime value)
        {
            // Ищем и удаляем свойство, если оно было уже добавлено ранее
            RemoveProperty(jObject, propertyName);

            string jsonMsDate = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
            }).Replace("\"", string.Empty).Replace(@"\", string.Empty);

            jObject.Add(new JProperty(propertyName, jsonMsDate));
        }

        // Добавляет заданное Enum значение, если оно не равно defaultValue(значения не должны быть равны null)
        public static void AddPropertyEnum(this JObject jObject, string propertyName, Enum value, Enum defaultValue)
        {
            // Ищем и удаляем свойство, если оно было уже добавлено ранее
            RemoveProperty(jObject, propertyName);

            if (value.Equals(defaultValue))
                return;

            jObject.Add(new JProperty(propertyName, value.ToString()));
        }

        // Добавляет заданное Enum значение
        public static void AddPropertyEnum(this JObject jObject, string propertyName, Enum value)
        {
            // Ищем и удаляем свойство, если оно было уже добавлено ранее
            RemoveProperty(jObject, propertyName);

            jObject.Add(new JProperty(propertyName, value.ToString()));
        }

        // Добавляет заданное значение, если оно не IsNullOrEmpty 
        public static void AddPropertyString(this JObject jObject, string propertyName, string value, string defaultValue)
        {
            // Ищем и удаляем свойство, если оно было уже добавлено ранее
            RemoveProperty(jObject, propertyName);

            if (value.Equals(defaultValue))
                return;

            jObject.Add(new JProperty(propertyName, value));
        }

        // Добавляет заданное значение, если оно не IsNullOrEmpty 
        public static void AddPropertyString(this JObject jObject, string propertyName, string value)
        {
            // Ищем и удаляем свойство, если оно было уже добавлено ранее
            RemoveProperty(jObject, propertyName);

            jObject.Add(new JProperty(propertyName, value));
        }

        // Добавляет заданное значение, если оно не IsNullOrEmpty 
        public static void AddPropertyStringNullOfEmpty(this JObject jObject, string propertyName, string value)
        {
            // Ищем и удаляем свойство, если оно было уже добавлено ранее
            RemoveProperty(jObject, propertyName);

            if (string.IsNullOrEmpty(value))
                return;

            jObject.Add(new JProperty(propertyName, value));
        }

        #endregion
    }
}
