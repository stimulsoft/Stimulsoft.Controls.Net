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
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Stimulsoft.Base
{
    public partial class StiExceptionForm : Form, IComparer <AssemblyName>
    {
        public int Compare(AssemblyName x, AssemblyName y)
        {
            return x.Name.CompareTo(y.Name);
        }


        public StiExceptionForm(Exception exception)
        {
            InitializeComponent();

            Assembly assembly = Assembly.GetAssembly(this.GetType());
            Version version = assembly.GetName().Version;

            this.textBoxMessage.Text = exception.Message;
            this.textBoxApplication.Text = Application.ProductName;
            this.textBoxFramework.Text = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
            this.textBoxVersion.Text = string.Format("Version: {0}.{1}.{2} from {3:D}",
                version.Major, version.Minor, version.Build, StiVersion.CreationDate);

            this.textBoxMessage2.Text = exception.Message;
            this.textBoxSource.Text = exception.Source;
            this.textBoxStackTrace.Text = exception.StackTrace;

            #region Create Assemblies
            listViewAssemblies.Clear();
            listViewAssemblies.Columns.Add("Name", 320, HorizontalAlignment.Left);
            listViewAssemblies.Columns.Add("Version", 150, HorizontalAlignment.Left);

            var assemblyEntry = Assembly.GetEntryAssembly();
            if (assemblyEntry == null) assemblyEntry = Assembly.GetExecutingAssembly();

            var assemblies = assemblyEntry.GetReferencedAssemblies();
            Array.Sort(assemblies, this);
            
            foreach (AssemblyName assemblyName in assemblies)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = assemblyName.Name;                
                listViewItem.SubItems.Add(assemblyName.Version.ToString());
                listViewAssemblies.Items.Add(listViewItem);
            }
            #endregion
        }

        private string GetExceptionString()
        {            
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("----------------------------");

            sb.AppendLine("[Customer Explanation]")
                .AppendLine()
                .AppendLine(textBoxInformation.Text)
                .AppendLine();

            sb.AppendLine("----------------------------");
           
            sb.AppendLine("[General Info]")
                .AppendLine()
                .AppendLine("Application: " + this.textBoxApplication.Text)
                .AppendLine("Framework:   " + this.textBoxFramework.Text)
                .AppendLine("Version:     " + this.textBoxVersion.Text)
                .AppendLine("MachineName: " + Environment.MachineName)
                .AppendLine("OSVersion:   " + Environment.OSVersion.VersionString)
                .AppendLine("UserName:    " + Environment.UserName)
                .AppendLine();

            sb.AppendLine("----------------------------");

            sb.AppendLine("[Exception Info]")
                .AppendLine("Message:     " + this.textBoxMessage.Text)
                .AppendLine()
                .AppendLine("Source:      " + this.textBoxSource.Text)
                .AppendLine()
                .AppendLine("StackTrace:").AppendLine(this.textBoxStackTrace.Text)
                .AppendLine();

            sb.AppendLine("----------------------------");

            sb.AppendLine("[Assemblies]");

            var assemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            Array.Sort(assemblies, this);

            foreach (var assemblyName in assemblies)
            {
                sb.AppendLine(string.Format("{0}, Version = {1}", assemblyName.Name, assemblyName.Version));
            }

            return sb.ToString();
        }

        private void buttonSaveToFile_Click(object sender, EventArgs e)
        {
            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
                saveDialog.FilterIndex = 1;
                saveDialog.FileName = "Exception.txt";
                saveDialog.RestoreDirectory = true;

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var writer = new StreamWriter(saveDialog.FileName))
                    {
                        writer.Write(GetExceptionString());
                        writer.Flush();
                        writer.Close();
                    }
                }
            }
        }

        private void buttonClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(GetExceptionString(), true);
        }
    }
}