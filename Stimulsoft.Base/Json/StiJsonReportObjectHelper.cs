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

using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using System;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Base
{
    public static class StiJsonReportObjectHelper
    {
        #region Serialize
        public static class Serialize
        {
            public static string FontDefault(Font font)
            {
                return Font(font, "Arial", 8, FontStyle.Regular, GraphicsUnit.Point);
            }

            public static string Font(Font font, string defaultFamily, float defaultEmSize)
            {
                return Font(font, defaultFamily, defaultEmSize, FontStyle.Regular, GraphicsUnit.Point);
            }

            public static string Font(Font font, string defaultFamily, float defaultEmSize, FontStyle fontStyle)
            {
                return Font(font, defaultFamily, defaultEmSize, fontStyle, GraphicsUnit.Point);
            }

            public static string Font(Font font, string defaultFamily, float defaultEmSize, FontStyle defaultStyle, GraphicsUnit defaultUnit)
            {
                string fontFamily = null;
                string size = null;
                string style = null;
                string unit = null;

                int count = 0;
                if (font.FontFamily.Name != defaultFamily)
                {
                    count++;
                    fontFamily = font.FontFamily.Name;
                }
                if ((font.Size != defaultEmSize))
                {
                    count++;
                    size = font.Size.ToString();
                }
                if (font.Style != defaultStyle)
                {
                    count++;
                    style = font.Style.ToString();
                }
                if (font.Unit != defaultUnit)
                {
                    count++;
                    unit = font.Unit.ToString();
                }

                if (count == 0)
                    return null;

                return fontFamily + ";" + size + ";" + style + ";" + unit;
            }

            public static string RectangleD(RectangleD rect)
            {
                return rect.X + "," + rect.Y + "," + rect.Width + "," + rect.Height;
            }

            public static string SizeD(SizeD size)
            {
                return size.Width + "," + size.Height;
            }

            public static string JColor(Color color, Color defColor)
            {
                if (color == defColor)
                    return null;

                return JColor(color);
            }

            public static string JColor(Color color)
            {
                if (color.IsNamedColor)
                {
                    return color.Name;
                }
                else
                {
                    if (color.A == 255)
                        return
                        color.R.ToString() + "," +
                        color.G.ToString() + "," +
                        color.B.ToString();
                    else
                        return
                        color.A.ToString() + "," +
                        color.R.ToString() + "," +
                        color.G.ToString() + "," +
                        color.B.ToString();
                }
            }

            public static JObject ColorArray(Color[] array)
            {
                var jObject = new JObject();

                for (int index = 0; index < array.Length; index++)
                {
                    var color = array[index];
                    string colorStr = JColor(color);
                    jObject.AddPropertyString(index.ToString(), colorStr);
                }

                return jObject;
            }

            public static JObject StringArray(string[] array)
            {
                if (array == null || array.Length == 0)
                    return null;

                var jObject = new JObject();

                for (int index = 0; index < array.Length; index++ )
                {
                    jObject.AddPropertyString(index.ToString(), array[index]);
                }

                return jObject;
            }

            public static JObject IntArray(int[] array)
            {
                if (array == null || array.Length == 0)
                    return null;

                var jObject = new JObject();

                for (int index = 0; index < array.Length; index++)
                {
                    jObject.AddPropertyStringNullOfEmpty(index.ToString(), array[index].ToString());
                }

                return jObject;
            }

            public static JObject Size(Size size)
            {
                var jObject = new JObject();

                jObject.AddPropertyInt("Width", size.Width);
                jObject.AddPropertyInt("Height", size.Height);

                return jObject;
            }

            public static JObject Point(Point pos)
            {
                var jObject = new JObject();

                jObject.AddPropertyStringNullOfEmpty("X", pos.X.ToString());
                jObject.AddPropertyStringNullOfEmpty("Y", pos.Y.ToString());

                return jObject;
            }

            public static JObject PointF(PointF pos)
            {
                var jObject = new JObject();

                jObject.AddPropertyStringNullOfEmpty("X", pos.X.ToString());
                jObject.AddPropertyStringNullOfEmpty("Y", pos.Y.ToString());

                return jObject;
            }

            public static string JCap(StiCap cap)
            {
                var builder = new StringBuilder();

                // Width
                if (cap.Width != 10)
                    builder.Append(cap.Width);
                builder.Append(";");

                // Height
                if (cap.Height != 10)
                    builder.Append(cap.Height);
                builder.Append(";");

                // Style
                if (cap.Style != StiCapStyle.None)
                    builder.Append(cap.Style.ToString());
                builder.Append(";");

                // Color
                builder.Append(JColor(cap.Color, Color.Black));

                return builder.ToString();
            }

            public static string JBrush(StiBrush brush, StiBrush defaultBrush = null)
            {
                var builder = new StringBuilder();

                #region StiSolidBrush
                if (brush is StiSolidBrush)
                {
                    var solid = (StiSolidBrush)brush;

                    builder.Append("solid:");

                    // Color
                    builder.Append(JColor(solid.Color, Color.Transparent));
                }
                #endregion

                #region StiEmptyBrush
                else if (brush is StiEmptyBrush)
                {
                    builder.Append("empty");
                }
                #endregion

                #region StiGlareBrush
                else if (brush is StiGlareBrush)
                {
                    var glare = (StiGlareBrush)brush;

                    builder.Append("glare:");

                    // StartColor
                    builder.Append(JColor(glare.StartColor, Color.Black));
                    builder.Append(":");

                    // EndColor
                    builder.Append(JColor(glare.EndColor, Color.White));
                    builder.Append(":");

                    // Angle
                    if (glare.Angle != 0.0d)
                        builder.Append(glare.Angle);
                    builder.Append(":");

                    // Focus
                    if (glare.Focus != 0.5f)
                        builder.Append(glare.Focus);
                    builder.Append(":");

                    // Scale
                    if (glare.Scale != 1.0f)
                        builder.Append(glare.Scale);
                }
                #endregion

                #region StiGlassBrush
                else if (brush is StiGlassBrush)
                {
                    var glass = (StiGlassBrush)brush;

                    builder.Append("glass:");

                    // Color
                    builder.Append(JColor(glass.Color, Color.Silver));
                    builder.Append(":");

                    // Angle
                    if (glass.DrawHatch)
                        builder.Append(glass.DrawHatch);
                    builder.Append(":");

                    // Focus
                    if (glass.Blend != 0.2f)
                        builder.Append(glass.Blend);
                }
                #endregion

                #region StiGradientBrush
                else if (brush is StiGradientBrush)
                {
                    var gradient = (StiGradientBrush)brush;

                    builder.Append("gradient:");

                    // StartColor
                    builder.Append(JColor(gradient.StartColor, Color.Black));
                    builder.Append(":");

                    // EndColor
                    builder.Append(JColor(gradient.EndColor, Color.White));
                    builder.Append(":");

                    // Angle
                    if (gradient.Angle != 0.0d)
                        builder.Append(gradient.Angle);
                }
                #endregion

                #region StiHatchBrush
                else if (brush is StiHatchBrush)
                {
                    var hatch = (StiHatchBrush)brush;

                    builder.Append("hatch:");

                    // BackColor
                    builder.Append(JColor(hatch.BackColor, Color.Black));
                    builder.Append(":");

                    // ForeColor
                    builder.Append(JColor(hatch.ForeColor, Color.White));
                    builder.Append(":");

                    // BackwardDiagonal
                    if (hatch.Style != HatchStyle.BackwardDiagonal)
                        builder.Append(hatch.Style);
                }
                #endregion

                return builder.ToString();
            }

            public static string JBorderSide(StiBorderSide side)
            {
                string color = JColor(side.Color, Color.Black);

                string size = string.Empty;
                if (side.Size != 1d)
                    size = side.Size.ToString();

                string style = string.Empty;
                if (side.Style != StiPenStyle.None)
                    style = side.Style.ToString();

                return color + ":" + size + ":" + style;
            }

            public static string JBorder(StiBorder border)
            {
                var builder = new StringBuilder();

                var advancedBorder = border as StiAdvancedBorder;
                if (advancedBorder != null)
                {
                    // TopSide
                    builder.Append(JBorderSide(advancedBorder.TopSide));
                    builder.Append(";");

                    // BottomSide
                    builder.Append(JBorderSide(advancedBorder.BottomSide));
                    builder.Append(";");

                    // LeftSide
                    builder.Append(JBorderSide(advancedBorder.LeftSide));
                    builder.Append(";");

                    // RightSide
                    builder.Append(JBorderSide(advancedBorder.RightSide));
                    builder.Append(";");

                    // DropShadow
                    if (border.DropShadow)
                        builder.Append(border.DropShadow);
                    builder.Append(";");

                    // ShadowSize
                    if (border.ShadowSize != 4d)
                        builder.Append(border.ShadowSize);
                    builder.Append(";");

                    // ShadowBrush
                    builder.Append(JBrush(border.ShadowBrush, new StiSolidBrush(Color.Black)));

                }
                else
                {
                    // Side
                    if (border.Side != StiBorderSides.None)
                        builder.Append(border.Side);
                    builder.Append(";");

                    // Color
                    if (border.Color != Color.Black)
                        builder.Append(JColor(border.Color));
                    builder.Append(";");

                    // Size
                    if (border.Size != 1d)
                        builder.Append(border.Size);
                    builder.Append(";");

                    // Style
                    if (border.Style != StiPenStyle.Solid)
                        builder.Append(border.Style);
                    builder.Append(";");

                    // ShadowSize
                    if (border.ShadowSize != 4d)
                        builder.Append(border.ShadowSize);
                    builder.Append(";");

                    // DropShadow
                    if (border.DropShadow)
                        builder.Append(border.DropShadow);
                    builder.Append(";");

                    // Topmost
                    if (border.Topmost)
                        builder.Append(border.Topmost);
                    builder.Append(";");

                    // ShadowBrush
                    builder.Append(JBrush(border.ShadowBrush, new StiSolidBrush(Color.Black)));
                }

                return builder.ToString();
            }
        }
        #endregion

        #region Deserialize
        public static class Deserialize
        {
            public static string[] StringArray(JObject jObject)
            {
                var result = new string[jObject.Count];
                
                int index = 0;
                foreach (var prop in jObject.Properties())
                {
                    result[index] = prop.Value.ToObject<string>();
                    index++;
                }

                return result;
            }

            public static int[] IntArray(JObject jObject)
            {
                var result = new int[jObject.Count];

                int index = 0;
                foreach (var prop in jObject.Properties())
                {
                    result[index] = prop.Value.ToObject<int>();
                    index++;
                }

                return result;
            }

            public static Font Font(JProperty prop, Font defaultFont)
            {
                if (prop.Value is JValue)
                    return Font((string)((JValue)prop.Value).Value, defaultFont);
                else
                    return Font((JObject)prop.Value, defaultFont);
            }

            private static Font Font(string text, Font defaultFont)
            {
                var values = text.Split(';');
                if (values.Length != 4)
                    throw new Exception("Parsing Error");

                string defaultFamily = defaultFont.FontFamily.Name;
                float defaultEmSize = defaultFont.Size;
                FontStyle defaultStyle = defaultFont.Style;
                GraphicsUnit defaultUnit = defaultFont.Unit;

                if (!string.IsNullOrEmpty(values[0]))
                    defaultFamily = values[0];
                if (!string.IsNullOrEmpty(values[1]))
                    defaultEmSize = float.Parse(values[1]);
                if (!string.IsNullOrEmpty(values[2]))
                    defaultStyle = (FontStyle)Enum.Parse(typeof(FontStyle), values[2]);
                if (!string.IsNullOrEmpty(values[3]))
                    defaultUnit = (GraphicsUnit)Enum.Parse(typeof(GraphicsUnit), values[3]);

                return new Font(StiFontCollection.GetFontFamily(defaultFamily), defaultEmSize, defaultStyle, defaultUnit);
            }

            private static Font Font(JObject jObject, Font defaultFont)
            {
                string familyName = defaultFont.FontFamily.Name;
                float emSize = defaultFont.Size;
                FontStyle style = defaultFont.Style;
                GraphicsUnit unit = defaultFont.Unit;

                foreach (var property in jObject.Properties())
                {
                    switch (property.Name)
                    {
                        case "FamilyName":
                            familyName = property.Value.ToObject<string>();
                            break;

                        case "Size":
                            emSize = property.Value.ToObject<float>();
                            break;

                        case "Style":
                            style = (FontStyle)Enum.Parse(typeof(FontStyle), property.Value.ToObject<string>());
                            break;

                        case "Unit":
                            unit = (GraphicsUnit)Enum.Parse(typeof(GraphicsUnit), property.Value.ToObject<string>());
                            break;
                    }
                }

                return new Font(familyName, emSize, style, unit);
            }

            public static StiBorderSide JBorderSide(string text)
            {
                var values = text.Split(':');
                var side = new StiBorderSide();

                if (!string.IsNullOrEmpty(values[0]))
                    side.Color = Color(values[0]);

                if (!string.IsNullOrEmpty(values[1]))
                    side.Size = double.Parse(values[1]);

                if (!string.IsNullOrEmpty(values[2]))
                    side.Style = (StiPenStyle)Enum.Parse(typeof(StiPenStyle), values[2]);

                return side;
            }

            public static StiCap JCap(string text)
            {
                var values = text.Split(';');
                var cap = new StiCap();

                if (values.Length != 4)
                    throw new Exception("Parsing Error");

                if (!string.IsNullOrEmpty(values[0]))
                    cap.Width = int.Parse(values[0]);

                if (!string.IsNullOrEmpty(values[1]))
                    cap.Height = int.Parse(values[1]);

                if (!string.IsNullOrEmpty(values[2]))
                    cap.Style = (StiCapStyle)Enum.Parse(typeof(StiCapStyle), values[2]);

                if (!string.IsNullOrEmpty(values[3]))
                    cap.Color = Color(values[3]);

                return cap;
            }

            public static StiBorder Border(JProperty prop)
            {
                if (prop.Value is JValue)
                {
                    return Border((string)((JValue)prop.Value).Value);
                }
                else
                {
                    var border = new StiBorder();
                    border.LoadFromJson((JObject)prop.Value);
                    return border;
                }
            }

            private static StiBorder Border(string text)
            {
                var values = text.Split(';');
                if (values.Length == 7)
                {
                    bool dropShadow = false;
                    double shadowSize = 4d;
                    var shadowBrush = new StiSolidBrush(System.Drawing.Color.Black);

                    return new StiAdvancedBorder(JBorderSide(values[0]), JBorderSide(values[1]),
                        JBorderSide(values[2]), JBorderSide(values[3]), dropShadow, shadowSize, shadowBrush);
                }
                else
                {
                    var border = new StiBorder();

                    // Side
                    if (!string.IsNullOrEmpty(values[0]))
                        border.Side = (StiBorderSides)Enum.Parse(typeof(StiBorderSides), values[0]);

                    // Color
                    if (!string.IsNullOrEmpty(values[1]))
                        border.Color = Color(values[1]);

                    // Size
                    if (!string.IsNullOrEmpty(values[2]))
                        border.Size = double.Parse(values[2]);

                    // Style
                    if (!string.IsNullOrEmpty(values[3]))
                        border.Style = (StiPenStyle)Enum.Parse(typeof(StiPenStyle), values[3]);

                    // ShadowSize
                    if (!string.IsNullOrEmpty(values[4]))
                        border.ShadowSize = double.Parse(values[4]);

                    // DropShadow
                    if (!string.IsNullOrEmpty(values[5]))
                        border.DropShadow = true;

                    // Topmost
                    if (!string.IsNullOrEmpty(values[6]))
                        border.Topmost = true;

                    // ShadowBrush
                    if (!string.IsNullOrEmpty(values[7]))
                        border.ShadowBrush = Brush(values[7]);

                    return border;
                }
            }

            public static Color Color(string value)
            {
                if (value.IndexOf(",", StringComparison.InvariantCulture) != -1)
                {
                    string[] strs = value.Split(new char[] { ',' });
                    if (strs.Length == 4) return System.Drawing.Color.FromArgb(
                                              int.Parse(strs[0].Trim()),
                                              int.Parse(strs[1].Trim()),
                                              int.Parse(strs[2].Trim()),
                                              int.Parse(strs[3].Trim()));

                    return System.Drawing.Color.FromArgb(
                        int.Parse(strs[0].Trim()),
                        int.Parse(strs[1].Trim()),
                        int.Parse(strs[2].Trim()));
                }
                else if (value.StartsWith("#"))
                {
                    return ColorTranslator.FromHtml(value);
                }
                else
                {
                    return System.Drawing.Color.FromName(value);
                }
            }

            public static StiBrush Brush(JProperty prop)
            {
                if (prop.Value is JValue)
                {
                    return Brush((string)((JValue)prop.Value).Value);
                }
                else
                {
                    return StiBrush.LoadFromJson((JObject)prop.Value);
                }
            }

            private static StiBrush Brush(string text)
            {
                var values = text.Split(':');

                switch (values[0])
                {
                    #region StiEmptyBrush
                    case "empty":
                        return new StiEmptyBrush();
                    #endregion

                    #region StiGlassBrush
                    case "glass":
                        {
                            var glass = new StiGlassBrush();

                            if (!string.IsNullOrEmpty(values[1]))
                                glass.Color = Color(values[1]);

                            if (!string.IsNullOrEmpty(values[2]))
                                glass.DrawHatch = true;

                            if (!string.IsNullOrEmpty(values[3]))
                                glass.Blend = float.Parse(values[3]);

                            return glass;
                        }
                    #endregion

                    #region StiGlareBrush
                    case "glare":
                        {
                            var glare = new StiGlareBrush();

                            if (!string.IsNullOrEmpty(values[1]))
                                glare.StartColor = Color(values[1]);

                            if (!string.IsNullOrEmpty(values[2]))
                                glare.EndColor = Color(values[2]);

                            if (!string.IsNullOrEmpty(values[3]))
                                glare.Angle = double.Parse(values[3]);

                            if (!string.IsNullOrEmpty(values[4]))
                                glare.Focus = float.Parse(values[4]);

                            if (!string.IsNullOrEmpty(values[5]))
                                glare.Scale = float.Parse(values[5]);

                            return glare;
                        }
                    #endregion

                    #region StiHatchBrush
                    case "hatch":
                        {
                            var hatch = new StiHatchBrush();

                            if (!string.IsNullOrEmpty(values[1]))
                                hatch.BackColor = Color(values[1]);

                            if (!string.IsNullOrEmpty(values[2]))
                                hatch.ForeColor = Color(values[2]);

                            if (!string.IsNullOrEmpty(values[3]))
                                hatch.Style = (HatchStyle)Enum.Parse(typeof(HatchStyle), values[3]);

                            return hatch;
                        }
                    #endregion

                    #region StiGradientBrush
                    case "gradient":
                        {
                            var gradient = new StiGradientBrush();

                            if (!string.IsNullOrEmpty(values[1]))
                                gradient.StartColor = Color(values[1]);

                            if (!string.IsNullOrEmpty(values[2]))
                                gradient.EndColor = Color(values[2]);

                            if (!string.IsNullOrEmpty(values[3]))
                                gradient.Angle = double.Parse(values[3]);

                            return gradient;
                        }
                    #endregion

                    #region StiSolidBrush
                    case "solid":
                        {
                            var solid = new StiSolidBrush();

                            if (!string.IsNullOrEmpty(values[1]))
                                solid.Color = Color(values[1]);

                            return solid;
                        }
                    #endregion
                }

                return null;
            }

            public static Color[] ColorArray(JObject jObject)
            {
                var result = new Color[jObject.Count];

                int index = 0;
                foreach (var prop in jObject.Properties())
                {
                    string value = prop.Value.ToObject<string>();
                    //byte a, r, g, b;
                    //if (value.Length == 9)
                    //{
                    //    a = byte.Parse(value.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                    //    r = byte.Parse(value.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                    //    g = byte.Parse(value.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
                    //    b = byte.Parse(value.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
                    //}
                    //else
                    //{
                    //    a = 255;
                    //    r = byte.Parse(value.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                    //    g = byte.Parse(value.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                    //    b = byte.Parse(value.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
                    //}

                    //result[index] = System.Drawing.Color.FromArgb(a, r, g, b);

                    if (value.IndexOf(",", StringComparison.InvariantCulture) != -1)
                    {
                        string[] strs = value.Split(new char[] { ',' });
                        if (strs.Length == 4)
                        {
                            result[index] = System.Drawing.Color.FromArgb(
                                                  int.Parse(strs[0].Trim()),
                                                  int.Parse(strs[1].Trim()),
                                                  int.Parse(strs[2].Trim()),
                                                  int.Parse(strs[3].Trim()));
                        }
                        else
                        {
                            result[index] = System.Drawing.Color.FromArgb(
                                int.Parse(strs[0].Trim()),
                                int.Parse(strs[1].Trim()),
                                int.Parse(strs[2].Trim()));
                        }
                    }
                    else
                    {
                        result[index] = System.Drawing.Color.FromName(value);
                    }

                    index++;
                }

                return result;
            }

            public static Size Size(JObject jObject)
            {
                Size size = new Size();

                foreach (var prop in jObject.Properties())
                {
                    switch(prop.Name)
                    {
                        case "Width":
                            size.Width = prop.Value.ToObject<int>();
                            break;

                        case "Height":
                            size.Height = prop.Value.ToObject<int>();
                            break;
                    }
                }

                return size;
            }

            public static DateTime DateTime(JProperty prop)
            {
                var obj = ((JValue)prop.Value).Value;

                if (obj is System.DateTime)
                    return prop.Value.ToObject<System.DateTime>();
                else if (obj is long)
                    return new System.DateTime(prop.Value.ToObject<long>());

                return System.DateTime.Now;
            }

            public static RectangleD RectangleD(JProperty prop)
            {
                if (prop.Value is JValue)
                {
                    string text = (string)((JValue)prop.Value).Value;
                    var values = text.Split(',');
                    if (values.Length != 4)
                        throw new Exception("Parsing Error");

                    return new RectangleD(double.Parse(values[0]), double.Parse(values[1]),
                        double.Parse(values[2]), double.Parse(values[3]));
                }
                else
                {
                    var rect = new RectangleD();
                    rect.LoadFromJson((JObject)prop.Value);
                    return rect;
                }
            }

            public static SizeD SizeD(JProperty prop)
            {
                if (prop.Value is JValue)
                {
                    string text = (string)((JValue)prop.Value).Value;
                    var values = text.Split(',');
                    if (values.Length != 2)
                        throw new Exception("Parsing Error");

                    return new SizeD(double.Parse(values[0]), double.Parse(values[1]));
                }
                else
                {
                    var size = new SizeD();
                    foreach (var pr in ((JObject)prop.Value).Properties())
                    {
                        switch (prop.Name)
                        {
                            case "Width":
                                size.Width = pr.Value.ToObject<int>();
                                break;

                            case "Height":
                                size.Height = pr.Value.ToObject<int>();
                                break;
                        }
                    }
                    return size;
                }
            }

            public static System.Drawing.Point Point(JObject jObject)
            {
                var point = new System.Drawing.Point();

                foreach (var property in jObject.Properties())
                {
                    switch (property.Name)
                    {
                        case "X":
                            point.X = property.Value.ToObject<int>();
                            break;

                        case "Y":
                            point.Y = property.Value.ToObject<int>();
                            break;
                    }
                }

                return point;
            }

            public static PointF PointF(JObject jObject)
            {
                var point = new PointF();

                foreach (var property in jObject.Properties())
                {
                    switch (property.Name)
                    {
                        case "X":
                            point.X = property.Value.ToObject<float>();
                            break;

                        case "Y":
                            point.Y = property.Value.ToObject<float>();
                            break;
                    }
                }

                return point;
            }
        }
        #endregion
    }
}