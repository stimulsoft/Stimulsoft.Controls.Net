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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Drawing
{
	/// <summary>
	/// Class contains methods for text drawing.
	/// </summary>
	public class StiTextDrawing
	{
		#region StiRange
		/// <summary>
		/// Structure describes the range.
		/// Range describes the word or chain symbol in text.
		/// </summary>
		private struct StiRange
		{
			/// <summary>
			/// Contents of range.
			/// </summary>
			public string Text;
			
			/// <summary>
			/// Position of range.
			/// </summary>
			public PointD Pos;

			/// <summary>
			/// Size of range.
			/// </summary>
			public SizeD Size;
			
			/// <summary>
			/// Line on which is placed range.
			/// </summary>
			public int Line;
			
			/// <summary>
			/// Is start range.
			/// </summary>
			public bool IsStart;

			/// <summary>
			/// Is end range.
			/// </summary>
			public bool IsEnd;

			/// <summary>
			/// After this range, new line is starting.
			/// </summary>
			public bool NewLineForm;
			
			/// <summary>
			/// Initializes a new instance of the StiRange class with the specified location and size.
			/// </summary>
			public StiRange(string text, SizeD size, bool newLineForm)
			{
				this.Pos = new PointD(0, 0);
				this.Text = text;
				this.Size = size;
				this.Line = 0;
				this.IsStart = false;
				this.IsEnd = false;
				this.NewLineForm = newLineForm;
			}
		}
		#endregion

		private struct StiTxt
		{
			public string Text;
			public bool IsEnter;

			/// <summary>
			/// Returns the collection of the words.
			/// </summary>
			public static StiTxt[] GetTexts(string str)
			{
                var al = new List<StiTxt>();
				
				int pos = 0;
				bool start = true;
				string s = "";
				while (pos < str.Length)
				{
					if (str[pos] == '\n')
					{
						al.Add(new StiTxt(s, true));
						s = "";
						start = true;
					}
					else if (!start && str[pos] == ' ')
					{
						al.Add(new StiTxt(s, false));
						s = "";
						start = true;
					}
					else 
					{
						s += str[pos];
						start = false;
					}
					pos++;
				}

				if (pos == str.Length)
                    al.Add(new StiTxt(s, false));

				return al.ToArray();
			}

			public StiTxt(string text, bool isEnter)
			{
				Text = text;
				IsEnter = isEnter;
			}
		}


		/// <summary>
		/// Structure describes the line.
		/// </summary>
		private struct Line
		{
			public int Start;
			public int Count;
			public bool IsEnter;

			public Line(int start, int count, bool isEnter)
			{
				Start = start;
				Count = count;
				IsEnter = isEnter;
			}
		}


		/// <summary>
		/// Corrects the Range size according to a described rectangle.
		/// </summary>
		private static void CorrectSize(ref StiRange range, RectangleD rect)
		{
			if (range.Pos.X + range.Size.Width > rect.Right)
				range.Size.Width = rect.Right - range.Pos.X;
						
			if (range.Pos.Y + range.Size.Height > rect.Bottom)
				range.Size.Height = rect.Bottom - range.Pos.Y;
		}


		/// <summary>
		/// Returns ranges array.
		/// </summary>
		/// <param name="g">Graphics to measure sizes.</param>
		/// <param name="rect">Describes a rectangle.</param>
		/// <param name="text">Text for burst into arrays.</param>
		/// <param name="font">Font of the text.</param>
		/// <param name="sf">Text format.</param>
		/// <returns>Ranges.</returns>
		private static StiRange[] GetRange(Graphics g, RectangleD rect, string text, Font font, StringFormat sf)
		{
            var rn = new List<StiRange>();
			var txt = StiTxt.GetTexts(text);
            var ln = new List<Line>();
			double heightFont = font.Height;
			
			//Form words
			for (int k = 0; k < txt.Length; k++)
			{
				var size = g.MeasureString(txt[k].Text, font);
				rn.Add(new StiRange(txt[k].Text, new SizeD(size.Width, size.Height), txt[k].IsEnter));
			}

			#region Divide into lines
			double posX = rect.Left;
			double posY = rect.Top;
			int line = 1;
			int wordCount = 0;
			int start = 0;
			bool forceNewLine = false;

			var ranges = rn.ToArray();

			float spaceSize = g.MeasureString(" ", font, 100000, sf).Width / 2;

			for (int k = 0; k < ranges.Length; k++)
			{
				posX += ranges[k].Size.Width - spaceSize;
				if (forceNewLine|| (posX >= rect.Right && wordCount >= 1))
				{
					posX = rect.Left + ranges[k].Size.Width;
					line++;
					posY += heightFont;
					ln.Add(new Line(start, wordCount, forceNewLine));
					start = k;
					wordCount = 0;
				}
				wordCount++;

				ranges[k].Line = line;
				ranges[k].Pos.Y = posY;
				forceNewLine = ranges[k].NewLineForm;
			}
			ln.Add(new Line(start, wordCount, true));
			#endregion

			var lines = ln.ToArray();

			#region Examine lines and sets horizontal coordinates
			for (int h = 0; h < lines.Length; h++)
			{
				int startPos = lines[h].Start;
				int endPos = lines[h].Start + lines[h].Count - 1;

				ranges[startPos].IsStart = true;
				ranges[endPos].IsEnd = true;

				#region Last line or line finishing Enter
				if (lines[h].IsEnter)
				{
					posX = rect.Left;
					for (int f = startPos; f <= endPos; f++)
					{
						ranges[f].Pos.X = posX;
						posX += ranges[f].Size.Width;
					}
				}
				#endregion
				else
				{
					#region One word
					if (lines[h].Count == 1)ranges[startPos].Pos.X = rect.Left; 
					#endregion
					
					#region Much words
					else 
					{
						ranges[startPos].Pos.X = rect.Left;
						ranges[endPos].Pos.X = rect.Right - ranges[endPos].Size.Width;

						double space = 0;
						if (lines[h].Count > 2)
						{
							double wx = 0;
							for (int a = startPos + 1; a < endPos; a++)
								wx += ranges[a].Size.Width;

							space = ((rect.Width - 
								ranges[startPos].Size.Width -
								ranges[endPos].Size.Width -
								wx) / (lines[h].Count - 1));
						}

						posX = ranges[startPos].Size.Width + rect.Left + space;
						for (int f = startPos + 1; f < endPos; f++)
						{
							ranges[f].Pos.X = posX;
							posX += space  + ranges[f].Size.Width;
						}
					}
					#endregion
				}
			}
			#endregion

			#region Aligning the text
			if (sf.LineAlignment != StringAlignment.Near)
			{
				double allHeight = heightFont * lines.Length;
				double dist = 0;

				if (sf.LineAlignment == StringAlignment.Far)
					dist = rect.Height - allHeight;
				if (sf.LineAlignment == StringAlignment.Center)
					dist = (rect.Height - allHeight) / 2;

				for (int k = 0; k < ranges.Length; k++)
					ranges[k].Pos.Y += dist;
			}
			#endregion

			#region Correct of the text (LineLimit & Trimming)
			for (int k = 0; k < ranges.Length; k++)CorrectSize(ref ranges[k], rect);
			#endregion
			
			return ranges;
		}


		/// <summary>
		/// Draws the text aligned to width.
		/// </summary>
		/// <param name="g">Graphics to draw on.</param>
		/// <param name="text">Text to draw on.</param>
		/// <param name="font">Font to draw on.</param>
		/// <param name="brush">Brush to draw on.</param>
		/// <param name="rect">Rectangle to draw on.</param>
		/// <param name="stringFormat">Text format.</param>
		public static void DrawStringWidth(Graphics g, string text, Font font, Brush brush, 
			RectangleD rect, StringFormat stringFormat)
		{
			Region svClip = g.Clip;
			g.SetClip(rect.ToRectangleF(), CombineMode.Intersect);
			if (!string.IsNullOrEmpty(text))
			{
				if ((stringFormat.FormatFlags & StringFormatFlags.NoWrap) > 0)
					stringFormat.FormatFlags ^= StringFormatFlags.NoWrap;

				var ranges = GetRange(g, rect, text, font, stringFormat);
				
				stringFormat.LineAlignment = StringAlignment.Near;
				
				stringFormat.FormatFlags |= StringFormatFlags.LineLimit;

				for (int k = 0; k < ranges.Length; k++)
				{
					if (ranges[k].IsStart)stringFormat.Alignment = StringAlignment.Near;
					else if (ranges[k].IsEnd)stringFormat.Alignment = StringAlignment.Far;
					else stringFormat.Alignment = StringAlignment.Center;
					
                    g.DrawString(ranges[k].Text, font, brush, 
						(new RectangleD(ranges[k].Pos, ranges[k].Size)).ToRectangleF(), stringFormat);
					/*
					g.DrawRectangle(Pens.Red, 
						(float)ranges[k].Pos.X,
						(float)ranges[k].Pos.Y,
						(float)ranges[k].Size.Width,
						(float)ranges[k].Size.Height);
					*/
				}
			}
			g.SetClip(svClip, CombineMode.Replace);
		}


		/// <summary>
		/// Draws aligned to width text on the angle.
		/// </summary>
		/// <param name="g">Graphics to draw on.</param>
		/// <param name="text">Text to draw on.</param>
		/// <param name="font">Font to draw on.</param>
		/// <param name="brush">Brush to draw.</param>
		/// <param name="rect">Rectangle to draw.</param>
		/// <param name="stringFormat">Text format.</param>
		/// <param name="angle">Show text at an angle.</param>
		public static void DrawStringWidth(Graphics g, string text, Font font, Brush brush, 
			RectangleD rect, StringFormat stringFormat, float angle)
		{
			if (angle != 0)
			{
				var svClip = g.Clip;
				g.SetClip(rect.ToRectangleF(), CombineMode.Intersect);
				var gs = g.Save();
				g.TranslateTransform((float)(rect.Left + rect.Width / 2),(float)(rect.Top + rect.Height / 2));
				g.RotateTransform((float)angle);
				rect.X =  - rect.Width / 2;
				rect.Y =  - rect.Height / 2;
				var drawRect = new RectangleD(rect.X, rect.Y, rect.Width, rect.Height);
				
				if ((angle > 45 && angle < 135) || (angle > 225 && angle < 315))
					drawRect = new RectangleD(rect.Y, rect.X, rect.Height, rect.Width);

				DrawStringWidth(g, text, font, brush, drawRect, stringFormat);
				g.Restore(gs);
				g.SetClip(svClip, CombineMode.Replace);
			}
			else DrawStringWidth(g, text, font, brush, rect, stringFormat);
		}


		public static void DrawString(Graphics g, string text, Font font, Brush brush, 
			RectangleF rect, StringFormat stringFormat, float angle)
		{
			DrawString(g, text, font, brush, RectangleD.CreateFromRectangle(rect), stringFormat, angle);
		}

		/// <summary>
		/// Draws text at an angle.
		/// </summary>
		/// <param name="g">Graphics to draw on.</param>
		/// <param name="text">Text to draw on.</param>
		/// <param name="font">Font to draw on.</param>
		/// <param name="brush">Brush to draw.</param>
		/// <param name="rect">Rectangle to draw.</param>
		/// <param name="stringFormat">Text format.</param>
		/// <param name="angle">Show text at an angle.</param>
		public static void DrawString(Graphics g, string text, Font font, Brush brush, 
			RectangleD rect, StringFormat stringFormat, float angle)
		{
			try
			{
				if (angle != 0)
				{
					var svClip = g.Clip;
					g.SetClip(rect.ToRectangleF(), CombineMode.Intersect);
					var gs = g.Save();
					g.TranslateTransform((float)(rect.Left + rect.Width / 2), (float)(rect.Top + rect.Height / 2));
					g.RotateTransform(-(float)angle);
					rect.X =  - rect.Width / 2;
					rect.Y =  - rect.Height / 2;
				
					var drawRect = new RectangleD(rect.X, rect.Y, rect.Width, rect.Height);
				
					if ((angle > 45 && angle < 135) || (angle > 225 && angle < 315))
						drawRect = new RectangleD(rect.Y, rect.X, rect.Height, rect.Width);

					if (angle == 0 || angle == 90 || angle == 180 || angle == 270)
					{
						g.DrawString(text, font, brush, drawRect.ToRectangleF(), stringFormat);
					}
					else
					{
						stringFormat.SetTabStops(20f, new float[]{30f, 30f, 30f});
						stringFormat.Alignment = StringAlignment.Center;
						stringFormat.LineAlignment = StringAlignment.Center;
						
						g.DrawString(text, font, brush, 
							(float)(drawRect.X + drawRect.Width / 2), (float)(drawRect.Y + drawRect.Height / 2), stringFormat);
					}
					g.Restore(gs);
					g.SetClip(svClip, CombineMode.Replace);
				}
				else 
				{
					g.DrawString(text, font, brush, rect.ToRectangleF(), stringFormat);
				}
			}
			catch
			{
			}
		}


		/// <summary>
		/// Draws text at an angle.
		/// </summary>
		/// <param name="g">Graphics to draw on.</param>
		/// <param name="text">Text to draw on.</param>
		/// <param name="font">Font to draw on.</param>
		/// <param name="brush">Brush to draw.</param>
		/// <param name="rect">Rectangle to draw.</param>
		/// <param name="stringFormat">Text format.</param>
		public static void DrawString(Graphics g, string text, Font font, Brush brush, 
			RectangleD rect, StringFormat stringFormat)
		{
			DrawString(g, text, font, brush, rect, stringFormat, 0);
		}


		/// <summary>
		/// Draws text.
		/// </summary>
		/// <param name="g">Graphics to draw on.</param>
		/// <param name="text">Text to draw on.</param>
		/// <param name="font">Font to draw on.</param>
		/// <param name="brush">Brush to draw.</param>
		/// <param name="rect">Rectangle to draw.</param>
		/// <param name="textOptions">Options to show text.</param>
		/// <param name="ha">Horizontal alignment.</param>
		/// <param name="va">Vertical alignment.</param>
		public static void DrawString(Graphics g, string text, Font font, Brush brush, 
			RectangleD rect, StiTextOptions textOptions, StiTextHorAlignment ha, StiVertAlignment va)
		{

			DrawString(g, text, font, brush, rect, textOptions, ha, va, false, 1);
			
		}

	
		/// <summary>
		/// Draws text.
		/// </summary>
		/// <param name="g">Graphics to draw on.</param>
		/// <param name="text">Text to draw on.</param>
		/// <param name="font">Font to draw on.</param>
		/// <param name="brush">Brush to draw.</param>
		/// <param name="rect">Rectangle to draw.</param>
		/// <param name="textOptions">Options to show text.</param>
		/// <param name="ha">Horizontal alignment.</param>
		/// <param name="va">Vertical alignment.</param>
		public static void DrawString(Graphics g, string text, Font font, Brush brush, 
			RectangleD rect, StiTextOptions textOptions, 
			StiTextHorAlignment ha, StiVertAlignment va, bool antialiasing,
			float zoom)
		{
			using (StringFormat stringFormat = GetStringFormat(textOptions, ha, va, antialiasing, zoom))
			{
				if (ha == StiTextHorAlignment.Width)
					DrawStringWidth(g, text, font, brush, rect, stringFormat, textOptions.Angle);
				else DrawString(g, text, font, brush, rect, stringFormat, textOptions.Angle);
			}
		}


		public static SizeF MeasureString(Graphics g, string text, Font font, 
			StringAlignment horizontalAligment, StringAlignment vertiacalAligment, float angle)
		{

			StiTextHorAlignment ha;

			switch (horizontalAligment)
			{
				case StringAlignment.Center:
					ha = StiTextHorAlignment.Center;
					break;

				case StringAlignment.Far:
					ha = StiTextHorAlignment.Right;
					break;

				default:
					ha = StiTextHorAlignment.Left;
					break;
			}

			StiVertAlignment va;

			switch (vertiacalAligment)
			{
				case StringAlignment.Center:
					va = StiVertAlignment.Center;
					break;

				case StringAlignment.Far:
					va = StiVertAlignment.Bottom;
					break;

				default:
					va = StiVertAlignment.Top;
					break;
			}

			return MeasureString(g, text, font, ha, va, angle).ToSizeF();
		}


		public static SizeD MeasureString(Graphics g, string text, Font font, 
			StiTextHorAlignment ha, StiVertAlignment va, float angle)
		{
            return MeasureString(g, text, font, ha, va, false, angle);
		}

		
		public static SizeD MeasureString(Graphics g, string text, Font font, 
			StiTextHorAlignment ha, StiVertAlignment va, bool antialiasing, float angle)
		{
			StiTextOptions options = new StiTextOptions();
			options.Angle = angle;
			return MeasureString(g, text, font, 0, options, ha, va, antialiasing);
		}
		

		public static SizeD MeasureString(Graphics g, string text, Font font, 
			double width, StiTextOptions textOptions, StiTextHorAlignment ha, StiVertAlignment va, 
			bool antialiasing)
		{
            #region Cache for stringFormat - about 4% speed optimization
            int hashCode;
            unchecked
            {
                hashCode = textOptions.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)ha;
                hashCode = (hashCode * 397) ^ (int)va;
                hashCode = (hashCode * 397) ^ (antialiasing ? 1231 : 1237);
            }
            var stringFormat = (StringFormat)hashStringFormats[hashCode];
            if (stringFormat == null)
            {
                stringFormat = GetStringFormat(textOptions, ha, va, antialiasing, 1);
                lock (hashStringFormats)
                {
                    hashStringFormats[hashCode] = stringFormat;
                }
            }
            #endregion

            //using (StringFormat stringFormat = GetStringFormat(textOptions, ha, va, antialiasing, 1))
            //{
				if (!textOptions.WordWrap)width = 0;

                SizeF size = new SizeF((float)width, 0);
                try
                {
                    //size = g.MeasureString(text, font, (int)width, stringFormat);
                    size = g.MeasureString(text, font, new SizeF((float)width, 999999f), stringFormat);
                }
                catch
                {
                    size = new SizeF((float)width, 0);
                }

				if (textOptions.Angle == 90 || textOptions.Angle == 270)
				{
					float rw = size.Width;
					size.Width = size.Height;
					size.Height = rw;
				}
				else if (textOptions.Angle != 0f)
				{
					float sx = (float)size.Width / 2;
					float sy = (float)size.Height / 2;

				    var points = new PointF[]
				    {
				        new PointF(-sx, -sy),
				        new PointF(sx, -sy),
				        new PointF(sx, sy),
				        new PointF(-sx, sy)
				    };

					var m = new Matrix();
					m.Rotate(-(float)textOptions.Angle);
					m.TransformPoints(points);

					int index = 0;
					foreach (PointF point in points)
					{								
						double px = point.X;
						double py = point.Y;

						points[index++] = new PointF((float)(px + size.Width / 2), (float)(py + size.Height / 2));
					}

					using (var path = new GraphicsPath())
					{
						path.AddPolygon(points);
						var rect2 = path.GetBounds();
						return new SizeD(rect2.Width, rect2.Height);
					}
				}
				return new SizeD(size.Width, size.Height);
            //}
		}

        private static Hashtable hashStringFormats = new Hashtable();

		public static StringFormat GetStringFormat(
			StiTextOptions textOptions, StiTextHorAlignment ha, StiVertAlignment va, float zoom)
		{
			return GetStringFormat(textOptions, ha, va, false, zoom);
		}

		
		public static StringAlignment GetAlignment(StiTextHorAlignment alignment)
		{
			switch (alignment)
			{
				case StiTextHorAlignment.Center:
				case StiTextHorAlignment.Width:
					return StringAlignment.Center;

				case StiTextHorAlignment.Right:
					return StringAlignment.Far;

				default:
					return StringAlignment.Near;
			}
		}


		public static StringAlignment GetAlignment(StiVertAlignment alignment)
		{
			switch(alignment)
			{
				case StiVertAlignment.Center:
					return StringAlignment.Center;

				case StiVertAlignment.Bottom:
					return StringAlignment.Far;

				default:
					return StringAlignment.Near;
			}
		}


		
		public static StringFormat GetStringFormat(
			StiTextOptions textOptions, StiTextHorAlignment ha, StiVertAlignment va, bool antialiasing, float zoom)
		{
			var stringFormat = textOptions.GetStringFormat(antialiasing, zoom);
			
			stringFormat.Alignment = GetAlignment(ha);
			stringFormat.LineAlignment = GetAlignment(va);
			if (MeasureTrailingSpaces)
				stringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

			return stringFormat;
		}

		private static bool measureTrailingSpaces = false;
        /// <summary>
        /// Gets or sets value which indicates that text drawing engine will be measure text string including trailing spaces.
        /// </summary>
		public static bool MeasureTrailingSpaces
		{
			get
			{
				return measureTrailingSpaces;
			}
			set
			{
				measureTrailingSpaces = value;
			}
		}

	}
}
