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

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Collections;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// The StiMenuProvider allows to perfect standard the MainMenu and the ContexMenu.
	/// </summary>
	[ProvideProperty("ImageIndex", typeof(MenuItem))]
	[ProvideProperty("ImageList", typeof(MenuItem))]
	[ProvideProperty("Draw", typeof(MenuItem))]
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(StiMenuProvider), "Toolbox.StiMenuProvider.bmp")]
	public class StiMenuProvider : Component, IExtenderProvider
	{
		#region Methods
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (font != null)font.Dispose();
				if (imageIndexes != null)
				{
					imageIndexes.Clear();
					imageIndexes = null;
				}

				if (imageLists != null)
				{					
					foreach (ImageList list in imageLists.Values)
					{
						if (list != null)list.Dispose();
					}
					imageLists.Clear();
					imageLists = null;
				}
			}
			base.Dispose(disposing);
 
		}


		public void Clear()
		{
			imageLists.Clear();
			imageIndexes.Clear();
		}


		private bool FindRightToLeft(MenuItem menuItem)
		{
			MainMenu menu = menuItem.GetMainMenu();
			if (menu != null)return menu.RightToLeft == RightToLeft.Yes;

			ContextMenu contextMenu = menuItem.GetContextMenu();
			if (contextMenu != null)return contextMenu.RightToLeft == RightToLeft.Yes;

			return false;
		}

		public virtual void DrawItem(object sender, DrawItemEventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;

			using (Bitmap bmp = new Bitmap(e.Bounds.Width, e.Bounds.Height, PixelFormat.Format32bppArgb))
			{
				using(Graphics g = Graphics.FromImage(bmp))
				{
					bool rightToLeft = FindRightToLeft(menuItem);

					if (menuItem.Text == "-")
					{
						DrawMenuItemSeparator(g, e, menuItem, rightToLeft);
					}
					else if (
						menuItem.Text.Length >= 4 && 
						menuItem.Text.Substring(0, 2) == "--" && 
						menuItem.Text.Substring(menuItem.Text.Length - 2) == "--")
					{
						DrawMenuItemTitle(g, e, menuItem, rightToLeft);
					}			
					else
					{
						DrawMenuItemBackground(g, e, menuItem, rightToLeft);
						DrawMenuItemText(g, e, menuItem, rightToLeft);
						DrawMenuItemShortcut(g, e, menuItem, rightToLeft);
						DrawMenuItemCheck(g, e, menuItem, rightToLeft);
						DrawMenuItemImage(g, e, menuItem, rightToLeft);
					}

					e.Graphics.DrawImageUnscaled(bmp, e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);					
				}
			}
		}

		private void DrawMenuItemTitle(Graphics g, DrawItemEventArgs e, MenuItem menuItem, bool rightToLeft)
		{
			Rectangle rect = e.Bounds;
			rect.Location = new Point(0, 0);

			DrawMenuItemBar(g, e, menuItem, rightToLeft);

			if (!rightToLeft)
			{
				rect.X += MenuBarWidth;
			}
			rect.Width -= MenuBarWidth;
			
			rect.Width --;
			rect.Height --;

			using (var brush = new SolidBrush(StiColorUtils.Dark(SystemColors.Control, 20)))
			{
				g.FillRectangle(brush, rect);
			}

			var textRect = new Rectangle(rect.Left + 5, rect.Top, rect.Width - 5, rect.Height);	

			using (var sf = new StringFormat())
			{
				if (rightToLeft)
                    sf.Alignment = StringAlignment.Far;
				else 
                    sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Center;
				sf.FormatFlags = StringFormatFlags.NoWrap;
				sf.Trimming = StringTrimming.EllipsisCharacter;

				sf.HotkeyPrefix = HotkeyPrefix.Hide;

				g.DrawString(menuItem.Text.Substring(2, menuItem.Text.Length - 4), Font, SystemBrushes.ControlText, textRect, sf);
			}

			using (var pen = new Pen(StiColors.Content))
			{
				if (rightToLeft)g.DrawLine(pen, rect.Right, rect.Y, rect.Right, rect.Bottom);
				else g.DrawLine(pen, rect.X, rect.Y, rect.X, rect.Bottom);
			}
		}

		private void DrawMenuItemBar(Graphics g, DrawItemEventArgs e, Rectangle rect, bool rightToLeft)
		{
			using (var brush = StiBrushes.GetControlBrush(rect, 0f))
			{
				g.FillRectangle(brush, rect);
			}
		}

		private void DrawMenuItemCheck(Graphics g, DrawItemEventArgs e, MenuItem item, bool rightToLeft)
		{
			var rect = e.Bounds;
			rect.Location = new Point(0, 0);
			if (item.Checked)
			{
				rect.X ++;
				rect.Y ++;

				if (rightToLeft)rect.X = rect.Right - (MenuBarWidth - 1);
				rect.Width = (MenuBarWidth - 4);
				rect.Height -= 3;

				g.FillRectangle(StiBrushes.Selected, rect);
				g.DrawRectangle(StiPens.SelectedText, rect);

				var imageList = GetImageList(item);
				int imageIndex = GetImageIndex(item);

				if (!(imageList != null && imageIndex != -1 && imageIndex < imageList.Images.Count))
				{
					if (rightToLeft)
                        StiControlPaint.DrawCheck(g, e.Bounds.Right - MenuBarWidth + ImageSize / 2, rect.Y + ImageSize / 2, true);
					else 
                        StiControlPaint.DrawCheck(g, ImageSize / 2, rect.Y + ImageSize / 2, true);
				}
			}
		}

		private void DrawMenuItemShortcut(Graphics g, DrawItemEventArgs e, MenuItem item, bool rightToLeft)
		{
			var rect = e.Bounds;
			rect.Location = new Point(0, 0);
			rect.Width -= 15;

			if (item.ShowShortcut && (!(item.Parent is MainMenu)))
			{
				using (var sf  = new StringFormat())
				{
					if (rightToLeft)
					{
						rect.X += 10;
						sf.Alignment = StringAlignment.Near;
					}
					else sf.Alignment = StringAlignment.Far;
					sf.LineAlignment = StringAlignment.Center;
					string shortcutText = ConvertShortcut(item.Shortcut);
					
					if (item.Enabled)
					{
						g.DrawString(shortcutText, Font, SystemBrushes.ControlText, rect, sf);
					}
					else
					{
						using (var brush = new SolidBrush(StiColorUtils.Light(SystemColors.ControlText, 150)))
						{
							g.DrawString(shortcutText, Font, brush, rect, sf);
						}
					}
				}
			}
		}

		private void DrawMenuItemText(Graphics g, DrawItemEventArgs e, MenuItem item, bool rightToLeft)
		{
			Rectangle rect = e.Bounds;
			rect.Location = new Point(0, 0);
			if (!(item.Parent is MainMenu))
			{
				if (rightToLeft)rect = new Rectangle(rect.Left + 20, rect.Top, rect.Width - MenuBarWidth - 25, rect.Height);
				else rect = new Rectangle(rect.Left + MenuBarWidth + 5, rect.Top, rect.Width, rect.Height);

				using (var sf = new StringFormat())
				{
					sf.Alignment = rightToLeft ? StringAlignment.Far : StringAlignment.Near;

					sf.LineAlignment = StringAlignment.Center;
					sf.FormatFlags |= StringFormatFlags.NoWrap;
					sf.HotkeyPrefix = HotkeyPrefix.Show; 

					if (item.Enabled) 
					{
						if (item.DefaultItem)
						{
							using (var defaultFont = new Font(Font, FontStyle.Bold))
							{
								g.DrawString(item.Text, defaultFont, SystemBrushes.ControlText, rect, sf);
							}
						}
						else g.DrawString(item.Text, Font, SystemBrushes.ControlText, rect, sf);
					}
					else
					{
						using (var brush = new SolidBrush(StiColorUtils.Light(SystemColors.ControlText, 150)))
						{
							g.DrawString(item.Text, Font, brush, rect, sf);
						}
					}
				}
			}
		}

		private void DrawMenuItemImage(Graphics g, DrawItemEventArgs e, MenuItem item, bool rightToLeft)
		{
			Rectangle rect = e.Bounds;
			rect.Location = new Point(0, 0);
			Image image = null;

			var imageList = GetImageList(item);
			int imageIndex = GetImageIndex(item);

			if (imageList != null && imageIndex != -1 && imageIndex < imageList.Images.Count)
			{
				image = imageList.Images[imageIndex];
			}
                
			if (image != null)
			{
				if (rightToLeft)
				{
					rect = new Rectangle(
						rect.Right - MenuBarWidth + (MenuBarWidth - imageList.Images[imageIndex].Width) / 2,
						rect.Y + (ImageSize - imageList.Images[imageIndex].Height) / 2,
						image.Width,
						image.Height);
				}
				else
				{
					rect = new Rectangle(
						(MenuBarWidth - imageList.Images[imageIndex].Width) / 2,
						rect.Y + (ImageSize - imageList.Images[imageIndex].Height) / 2,
						image.Width,
						image.Height);
				}
			}

			if (image != null)
			{
				if (item.Enabled)imageList.Draw(g, rect.X, rect.Y, rect.Width, rect.Height, imageIndex);
				else StiControlPaint.DrawImageDisabled(g, image as Bitmap, rect.X, rect.Y);
			}
		}

		private void DrawMenuItemBackground(Graphics g, DrawItemEventArgs e, MenuItem item, bool rightToLeft)
		{
			Rectangle rect = e.Bounds;
			rect.Location = new Point(0, 0);

			if (!(item.Parent is MainMenu))
			{
				if ((((e.State & DrawItemState.Selected) > 0) || ((e.State & DrawItemState.HotLight) > 0))&& item.Enabled)
				{
					rect.Width --;
					rect.Height --;
				
					g.FillRectangle(StiBrushes.Focus, rect);
					g.DrawRectangle(StiPens.SelectedText, rect);
				}
				else 
				{
					g.FillRectangle(StiBrushes.Content, rect);
					DrawMenuItemBar(g, e, item, rightToLeft);				
				}
			}
		}

		private void DrawMenuItemBar(Graphics g, DrawItemEventArgs e, MenuItem item, bool rightToLeft)
		{
			Rectangle rect = e.Bounds;
			rect.Location = new Point(0, 0);

			if (rightToLeft)rect.X = rect.Right - MenuBarWidth;
			rect.Width = MenuBarWidth;
			DrawMenuItemBar(g, e, rect, rightToLeft);
		}

		private void DrawMenuItemSeparator(Graphics g, DrawItemEventArgs e, MenuItem item, bool rightToLeft)
		{
			DrawMenuItemBackground(g, e, item, rightToLeft);
			DrawMenuItemBar(g, e, item, rightToLeft);

			Rectangle rect = e.Bounds;
			rect.Location = new Point(0, 0);

			if (!rightToLeft)
			{
				rect.X += MenuBarWidth + 8;
			}
			rect.Width -= MenuBarWidth + 8;
			int py = rect.Y + rect.Height / 2;
			
			g.DrawLine(SystemPens.ControlDark, rect.X, py, rect.Right, py);
		}


		private string ConvertShortcut(Shortcut shortcut)
		{
			if (shortcut == Shortcut.None)
                return string.Empty;
			var keys = (Keys)shortcut;
			return TypeDescriptor.GetConverter(typeof(Keys)).ConvertToString(keys); 
		}
		

		public virtual void MeasureItem(object sender, MeasureItemEventArgs e)
		{
			var menuItem = (MenuItem)sender;
			bool rightToLeft = FindRightToLeft(menuItem);
			
			var g = e.Graphics;

			if (menuItem.Text.Length >= 4 && menuItem.Text.Substring(0, 2) == "--" && 
				menuItem.Text.Substring(menuItem.Text.Length - 2) == "--")
			{
				e.ItemWidth = (int)g.MeasureString(menuItem.Text.Substring(2, menuItem.Text.Length - 4), font).Width + MenuBarWidth;
				e.ItemHeight = 25;
			}
			else if (menuItem.Text != "-")
			{
				int wd = 0;
				if (menuItem.ShowShortcut)
					wd = (int)g.MeasureString(ConvertShortcut(menuItem.Shortcut), font).Width;
				if (wd != 0)wd += 15;

				if (rightToLeft)wd += 10;

				e.ItemHeight = (int)font.GetHeight() + 6;
				if (e.ItemHeight < ImageSize)e.ItemHeight = ImageSize;
				e.ItemWidth = ((int)g.MeasureString(menuItem.Text, font).Width) + wd;
				if (!(menuItem.Parent is MainMenu))e.ItemWidth += ImageSize * 2;
				
			}
			else
			{
				e.ItemWidth = 20;
				e.ItemHeight = 3;
			}
		}


		/// <summary>
		/// Adds the MenuItem to the MenuProvider.
		/// </summary>
		/// <param name="menuItem">Added MenuItem.</param>
		/// <param name="provider">The MenuProvider to which the MenuItem is added.</param>
		public static void AddMenuProviderToMenuItem(MenuItem menuItem, StiMenuProvider provider)
		{
			AddMenuProviderToMenuItem(menuItem, provider, null, -1);
			
		}


		/// <summary>
		/// Adds the MenuItem to the MenuProvider.
		/// </summary>
		/// <param name="menuItem">Added MenuItem.</param>
		/// <param name="provider">The MenuProvider to which the MenuItem is added.</param>
		/// <param name="imageList">Collection of images available to the MenuItem.</param>
		/// <param name="imageIndex">A zero-based index, which represents the image position in an ImageList.</param>
		public static void AddMenuProviderToMenuItem(MenuItem menuItem, StiMenuProvider provider, 
			ImageList imageList, int imageIndex)
		{
			if (!(menuItem.Parent is MainMenu))
			{
				menuItem.OwnerDraw = true;
				menuItem.DrawItem += provider.diEventHandler;
				menuItem.MeasureItem += provider.miEventHandler;
				provider.SetImageList(menuItem, imageList);
				provider.SetImageIndex(menuItem, imageIndex);
			}
		}


		/// <summary>
		/// Adds the Menu to the MenuProvider.
		/// </summary>
		/// <param name="menu">Added Menu.</param>
		/// <param name="provider">The MenuProvider to which the MenuItem is added.</param>
		public static void AddMenuProviderToMenu(Menu menu, StiMenuProvider provider)
		{
			foreach (MenuItem menuItem in menu.MenuItems)
			{
				if (!(menuItem.Parent is MainMenu))
				{
					menuItem.OwnerDraw = true;
					menuItem.DrawItem += new DrawItemEventHandler(provider.DrawItem);
					menuItem.MeasureItem += new MeasureItemEventHandler(provider.MeasureItem);
				}
			}
		}

		#endregion

		#region Fields
		private DrawItemEventHandler diEventHandler = null;
		private MeasureItemEventHandler miEventHandler = null;

		private const int MenuBarWidth = 24;
		private const int ImageSize = 22;
		private Hashtable imageIndexes;
		private Hashtable imageLists;
		#endregion
	
		#region IExtenderProvider
		public bool CanExtend(object e)
		{
			return e is MenuItem;
		}
		#endregion

		#region ImageIndex
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor(typeof(StiImageIndexEditor), typeof(UITypeEditor))]
		public void SetImageIndex(MenuItem control, int value)
		{
			if (!this.imageIndexes.Contains(control))this.imageIndexes.Add(control, value);
			else this.imageIndexes[control] = value;
		} 
		
		
		[TypeConverter(typeof(ImageIndexConverter))]
		[Editor(typeof(StiImageIndexEditor), typeof(UITypeEditor))]
		public int GetImageIndex(MenuItem control)
		{
			if (this.imageIndexes[control] == null)return -1; 
			return (int)this.imageIndexes[control];
		}

		#endregion

		#region ImageList
		public ImageList GetImageList(MenuItem control)
		{
			if (this.imageLists[control] == null)return null; 
			return (ImageList)this.imageLists[control];
		}

		public void SetImageList(MenuItem control, ImageList value)
		{
			if (!this.imageLists.Contains(control))this.imageLists.Add(control, value);
			else this.imageLists[control] = value;
		} 
		#endregion ImageList

		#region Paint
		public void SetDraw(MenuItem control, bool value)
		{
			if (!base.DesignMode)
			{
				if (value)
				{
					if (!(control.Parent is MainMenu))
					{
						control.MeasureItem += new MeasureItemEventHandler(this.MeasureItem);
						control.DrawItem += new DrawItemEventHandler(this.DrawItem);
						control.OwnerDraw = true;
					}
				}
				else
				{
					
					control.MeasureItem -= new MeasureItemEventHandler(this.MeasureItem);
					control.DrawItem -= new DrawItemEventHandler(this.DrawItem);
					control.OwnerDraw = false;
                        					
					if (this.imageIndexes.Contains(control))this.imageIndexes.Remove(control);
					
				}
			}
		} 
	
		[Browsable(false)]
		public bool GetDraw(MenuItem control)
		{
			return true; 
		}

		#endregion

		#region Properties
		private Font font;
		/// <summary>
		/// Gets or sets the font of the text to draw a menu item.
		/// </summary>
		[Category("Behavior")]
		public Font Font
		{
			get
			{
				return font;
			}
			set
			{
				font = value;
			}
			
		}

		#endregion

		#region Constructors
		public StiMenuProvider()
		{
			diEventHandler = new DrawItemEventHandler(DrawItem);
			miEventHandler = new MeasureItemEventHandler(MeasureItem);

			this.font = new Font("Arial", 8);
			this.imageIndexes = new Hashtable();
			this.imageLists = new Hashtable();
		}
		#endregion
	}
}
