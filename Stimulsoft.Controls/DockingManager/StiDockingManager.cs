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
using System.Xml;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls
{
	/// <summary>
	/// Manage docking panels.
	/// </summary>
    #if !Profile
	[Designer(typeof(StiDockingManagerDesigner))]
    #endif
	[ToolboxBitmap(typeof(StiDockingManager), "Toolbox.StiDockingManager.bmp")]
	[ToolboxItem(true)]
	public class StiDockingManager : Component
	{
		#region Fields
		private Hashtable guidChecker = new Hashtable();
		private bool autoHide = false;
		private DockStyle dockStyle = DockStyle.Left;
		
		private int left = 0;
		private int top = 0;
		private int width = 200;
		private int height = 300;

		private int controlIndex = -1;
		private int selectedIndex = 0;

		private StiFloatingForm floatingForm = null;
		private bool isFloating = false;
		private bool isClosing = false;
		#endregion 

		#region Methods
		public void ShowUndockingPanels()
		{
			if (!ShowingUndocked)
			{
				foreach (var panel in UndockedPanels)
				{
					if (!panel.FloatingForm.Visible)
					{
						ParentForm.AddOwnedForm(panel.FloatingForm);
						panel.FloatingForm.Show();
					}				
				}
				ShowingUndocked = true;
			}
		}


		/// <summary>
		/// Saves a configuration of docking manager.
		/// </summary>
		/// <param name="file">The name and location of the configuration to save.</param>
		public void SaveConfig(string file)
		{
			var stream = new FileStream(file, FileMode.Create);
			SaveConfig(stream);
			stream.Flush();
			stream.Close();
		}


		/// <summary>
		/// Saves a configuration of docking manager.
		/// </summary>
		/// <param name="stream">The data stream to save a configuration.</param>
		public void SaveConfig(Stream stream)
		{
			var tw = new XmlTextWriter(stream, System.Text.Encoding.UTF8);
			tw.Formatting = Formatting.Indented;

			tw.WriteStartDocument(true);
			
			SaveConfig(tw);
			
			tw.WriteEndDocument();
			tw.Flush();
		}


		/// <summary>
		/// Saves a configuration of docking manager.
		/// </summary>
		/// <param name="tw">The XmlTextWriter to save configuration.</param>
		public void SaveConfig(XmlTextWriter tw)
		{			
			tw.WriteStartElement("StiDockingManagerConfig");

			var al = DockedPanels;

			foreach (var panel in al)
			{
				if (panel.Controls.Count > 0)
				{
					tw.WriteStartElement("Panel",				panel.Text);
					tw.WriteAttributeString("Floating",			"False");
					tw.WriteAttributeString("Closing",			"False");
					tw.WriteAttributeString("AutoHide",			panel.AutoHide.ToString());
					tw.WriteAttributeString("Dock",				panel.Dock.ToString());
					
					if (panel.Collapsed)
					{
						tw.WriteAttributeString("Width",			panel.CollapsedSize.Width.ToString());
						tw.WriteAttributeString("Height",			panel.CollapsedSize.Height.ToString());
					}
					else
					{
						tw.WriteAttributeString("Width",			panel.Width.ToString());
						tw.WriteAttributeString("Height",			panel.Height.ToString());
					}

					tw.WriteAttributeString("SelectedIndex",	panel.Controls.IndexOf(panel.SelectedControl).ToString());

					tw.WriteStartElement("Controls", panel.Text);
					foreach (StiDockingControl control in panel.Controls)
					{					
						tw.WriteElementString("Guid", XmlConvert.EncodeName(control.Guid));
					}
					tw.WriteEndElement();
					tw.WriteEndElement();
				}
			}

			foreach (var panel in UndockedPanels)
			{
				if (panel.FloatingForm != null && panel.Controls.Count > 0)
				{
					tw.WriteStartElement("Panel", panel.Text);
					tw.WriteAttributeString("Floating",	"True");
					tw.WriteAttributeString("Closing",	"False");
					tw.WriteAttributeString("Left",		panel.FloatingForm.Left.ToString());
					tw.WriteAttributeString("Top",		panel.FloatingForm.Top.ToString());
					tw.WriteAttributeString("Width",	panel.FloatingForm.Width.ToString());
					tw.WriteAttributeString("Height",	panel.FloatingForm.Height.ToString());
					tw.WriteAttributeString("SelectedIndex",	panel.Controls.IndexOf(panel.SelectedControl).ToString());

					tw.WriteStartElement("Controls", panel.Text);
					foreach (StiDockingControl control in panel.Controls)
					{					
						tw.WriteElementString("Guid", XmlConvert.EncodeName(control.Guid));
					}
					tw.WriteEndElement();

					tw.WriteEndElement();
				}
			}

			if (ClosedControls.Count > 0)
			{
				tw.WriteStartElement("Panel");
				tw.WriteAttributeString("Floating",	"False");
				tw.WriteAttributeString("Closing",	"True");

				var hash = new Hashtable();

				foreach (var control in ClosedControls)
				{
					if (hash[control.Guid] == null)
					{
						hash[control.Guid] = control.Guid;
						tw.WriteElementString("Guid", XmlConvert.EncodeName(control.Guid));
					}
				}

				tw.WriteEndElement();
			}

			tw.WriteEndElement();

		}


		/// <summary>
		/// Loads a configuration of docking manager.
		/// </summary>
		/// <param name="file">The name and location of the configuration to load.</param>
		public void LoadConfig(string file)
		{
			if (File.Exists(file))
			{
				var stream = new FileStream(file, FileMode.Open);
				LoadConfig(stream);
				stream.Flush();
				stream.Close();
			}
		}


		/// <summary>
		/// Loads a configuration of docking manager.
		/// </summary>
		/// <param name="stream">The data stream that contains the configuration to load.</param>
		public void LoadConfig(Stream stream)
		{
			LockLayout = true;

			var tr = new XmlTextReader(stream);		

			LoadConfig(tr);

			LockLayout = false;
			SetLayoutAllPanels();
		}


		/// <summary>
		/// Loads a configuration of docking manager.
		/// </summary>
		/// <param name="tr">The XmlTextReader that contains the configuration to load.</param>
		public void LoadConfig(XmlTextReader tr)
		{		
			LockLayout = true;
			if (tr.IsStartElement())
			{
				if (tr.Name == "StiDockingManagerConfig")
				{
					autoHide = false;
					dockStyle = DockStyle.Left;
					width = 200;
					height = 300;

					guidChecker.Clear();
                    LoadNodes(tr);
				}
			}			
			LockLayout = false;
			SetLayoutAllPanels();
		}


		private void LoadNodes(XmlTextReader tr)
		{
			while (tr.Read())
			{
				if (!tr.IsStartElement())return;

				#region Panel
				if (tr.Name == "Panel")
				{
					controlIndex = -1;
					isFloating = tr.GetAttribute("Floating") == "True";
					isClosing = tr.GetAttribute("Closing") == "True";
                    
					if (!isClosing)
					{
						width = int.Parse(tr.GetAttribute("Width"));
						height = int.Parse(tr.GetAttribute("Height"));
						selectedIndex = int.Parse(tr.GetAttribute("SelectedIndex"));

						if (!isFloating)
						{
							autoHide = tr.GetAttribute("AutoHide") == "True";
							dockStyle = (DockStyle)Enum.Parse(typeof(DockStyle), tr.GetAttribute("Dock"));
						}
						else
						{
							left = int.Parse(tr.GetAttribute("Left"));
							top = int.Parse(tr.GetAttribute("Top"));
						}
					}

					this.floatingForm = null;
				}
				#endregion
						
				#region Guid
				if (tr.Name == "Guid")
				{
					controlIndex++;
					string guid = XmlConvert.DecodeName(tr.ReadElementString());

					if (guidChecker[guid] == null)
					{
						guidChecker[guid] = guid;
							
						var control = GetDockingControl(guid);
							
						if (control != null)
						{
							var dockingPanel = control.Parent as StiDockingPanel;

							if (!isClosing)
							{
								#region !isFloating
								if (!isFloating)
								{
									if (control.Parent.Dock != dockStyle)
									{
										dockingPanel.DockControlToForm(control, ParentForm, dockStyle);
									
										dockingPanel = GetDockingPanel(dockStyle);

										if (dockingPanel.Controls.Count == 0)
                                            UndockedPanels.Remove(dockingPanel);
									}
							
									if (control.Parent.Controls.IndexOf(control) != controlIndex)
									{
										control.Parent.Controls.SetChildIndex(control, controlIndex);
									}							

									if (!autoHide)
									{									
										dockingPanel.Width = width;
										dockingPanel.Height = height;
									}
									else 
									{
										dockingPanel.AutoHide = autoHide;
										dockingPanel.Collapsed = true;
										dockingPanel.CollapsedSize = new Size(width, height);
									}
								}
								#endregion

								#region isFloating
								else 
								{
									if (floatingForm == null)
									{
										dockingPanel.UndockControl(control, new Rectangle(left, top, width, height), true, dockingPanel.Parent);

										floatingForm = ((StiDockingPanel)control.Parent).FloatingForm;
										floatingForm.Width = width;
										floatingForm.Height = height;

										if (dockingPanel.Controls.Count == 0)
                                            UndockedPanels.Remove(dockingPanel);
									}
									else
									{
										dockingPanel.DockControlToPanel((StiDockingPanel)floatingForm.Controls[0]);

										if (dockingPanel.Controls.Count == 0)
                                            UndockedPanels.Remove(dockingPanel);
									
									}
								}
								#endregion

								if (controlIndex == selectedIndex && 
									((StiDockingPanel)control.Parent).SelectedControl != control)
								{
									((StiDockingPanel)control.Parent).SelectedControl = control;
								}
							}
							else
							{
								dockingPanel.DoClose(control);
							}
						}
					}
				}
				#endregion

				LoadNodes(tr);
			}
		}


		internal void SetLayoutAllPanels()
		{
			if (ParentForm != null)
			{
				foreach (Control control in ParentForm.Controls)
				{
					if (control is StiDockingPanel)
					{
						((StiDockingPanel)control).SetLayoutAllPanels();
						return;
					}
				}
			}

			foreach (var panel in UndockedPanels)
			{
				panel.SetLayoutAllPanels();
				return;
			}
		}

		
		internal void AddUndockingPanels(StiDockingPanel dockingPanel)
		{
			UndockedPanels.Add(dockingPanel);
            Form = (dockingPanel.FloatingForm != null) 
                ? dockingPanel.LastDockForm 
                : dockingPanel.Parent as Form;
		}

		internal void RemoveUndockingPanels(StiDockingPanel dockingPanel)
		{
			UndockedPanels.Remove(dockingPanel);
		}


		internal StiDockingPanel GetDockingPanel(DockStyle dockStyle)
		{
			foreach (Control control in ParentForm.Controls)
			{
				if (control is StiDockingPanel && control.Dock == dockStyle)
				{
					return control as StiDockingPanel;
				}
			}
			return null;
		}

		internal StiDockingControl GetDockingControl(string guid)
		{
			var al = DockedPanels;
			
			foreach (var panel in al)
			{
				foreach (StiDockingControl control in panel.Controls)
				{
					if (control.Guid == guid)return control;
				}
			}

			foreach (var panel in UndockedPanels)
			{
				foreach (StiDockingControl control in panel.Controls)
				{
					if (control.Guid == guid)return control;
				}
			}

			foreach (var control in ClosedControls)
			{
				var panel = new StiDockingPanel(this);
				panel.LastDockForm = ParentForm;
				panel.Controls.Add(control);
				
				if (control.Guid == guid)return control;
			}
			return null;
		}
		#endregion

		#region Properties
		private bool lockDockingPanels = false;
		[DefaultValue(false)]
		[Browsable(false)]
		public bool LockDockingPanels
		{
			get
			{
				return lockDockingPanels;
			}
			set
			{
				lockDockingPanels = value;
			}
		}


        private List<StiDockingPanel> undockedPanels = new List<StiDockingPanel>();
		/// <summary>
		/// Gets or sets a collection of undocked panels.
		/// </summary>
		[Browsable(false)]
        public List<StiDockingPanel> UndockedPanels
		{
			get
			{
				return undockedPanels;
			}
			set
			{
				undockedPanels = value;
			}
		}


        private List<StiDockingControl> closedControls = new List<StiDockingControl>();
		/// <summary>
		/// Gets or sets a collection of closed controls.
		/// </summary>
		[Browsable(false)]
        public List<StiDockingControl> ClosedControls
		{
			get
			{
				return closedControls;
			}
			set
			{
				closedControls = value;
			}
		}


		private bool showingUndocked = true;
		/// <summary>
		/// Gets or sets a value indicates showing undocking panel when a configuration loading.
		/// </summary>
		[DefaultValue(true)]
		[Browsable(false)]
		public bool ShowingUndocked
		{
			get
			{
				return showingUndocked;
			}
			set
			{
				showingUndocked = value;
			}
		}


		/// <summary>
		/// Gets a collection of docked panels.
		/// </summary>
		[Browsable(false)]
        public List<StiDockingPanel> DockedPanels
		{
			get
			{
                var al = new List<StiDockingPanel>();
				foreach (Control control in ParentForm.Controls)
				{
				    var docking = control as StiDockingPanel;
					if (docking != null)
					{
                        al.Add(docking);
					}
				}
				return al;
			}
		}


		private Form parentForm = null;
		/// <summary>
		/// Gets or sets the parent form of this docking manager.
		/// </summary>
		[Browsable(false)]
		public Form ParentForm
		{
			get
			{
				return parentForm;
			}
			set
			{
				parentForm = value;
			}
		}


		private Form form = null;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		internal Form Form
		{
			get
			{
				return form;
			}
			set
			{
				if (form == null && form != value && value != null)
				{
					form = value;
					form.Closed += new EventHandler(OnClosedUndockingPanels);
				}
			}
		}


		private bool lockLayout = false;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		internal bool LockLayout
		{
			get
			{
				return lockLayout;
			}
			set
			{
				lockLayout = value;
			}
		}
		#endregion

		#region Handlers
		private void OnClosedUndockingPanels(object sender, EventArgs e)
		{
			foreach (var panel in UndockedPanels)
			{
				var form = panel.Parent as Form;
				if (form != null)
				{
					form.Close();
					form.Dispose();
				}
			}
			UndockedPanels.Clear();
		}
		
		#endregion
		
		#region Constructors
		public StiDockingManager()
		{
			var cont =  base.Container;
		}
		#endregion
	}
}
