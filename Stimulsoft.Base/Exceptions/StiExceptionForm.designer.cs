namespace Stimulsoft.Base
{
    partial class StiExceptionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StiExceptionForm));
            this.btCancel = new System.Windows.Forms.Button();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.tabControlGeneral = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxInformation = new System.Windows.Forms.TextBox();
            this.textBoxFramework = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxVersion = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxApplication = new System.Windows.Forms.TextBox();
            this.picGeneral = new System.Windows.Forms.PictureBox();
            this.tabPageException = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxStackTrace = new System.Windows.Forms.TextBox();
            this.textBoxSource = new System.Windows.Forms.TextBox();
            this.textBoxMessage2 = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listViewAssemblies = new System.Windows.Forms.ListView();
            this.buttonClipboard = new System.Windows.Forms.Button();
            this.buttonSaveToFile = new System.Windows.Forms.Button();
            this.tabControlGeneral.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGeneral)).BeginInit();
            this.tabPageException.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            this.btCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(497, 374);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(76, 23);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "Close";
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMessage.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxMessage.Location = new System.Drawing.Point(76, 6);
            this.textBoxMessage.Multiline = true;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.ReadOnly = true;
            this.textBoxMessage.Size = new System.Drawing.Size(495, 64);
            this.textBoxMessage.TabIndex = 0;
            // 
            // tabControlGeneral
            // 
            this.tabControlGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlGeneral.Controls.Add(this.tabPage1);
            this.tabControlGeneral.Controls.Add(this.tabPageException);
            this.tabControlGeneral.Controls.Add(this.tabPage2);
            this.tabControlGeneral.Location = new System.Drawing.Point(4, 4);
            this.tabControlGeneral.Name = "tabControlGeneral";
            this.tabControlGeneral.SelectedIndex = 0;
            this.tabControlGeneral.Size = new System.Drawing.Size(585, 364);
            this.tabControlGeneral.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.textBoxInformation);
            this.tabPage1.Controls.Add(this.textBoxFramework);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.textBoxVersion);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textBoxApplication);
            this.tabPage1.Controls.Add(this.picGeneral);
            this.tabPage1.Controls.Add(this.textBoxMessage);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(577, 338);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 164);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(565, 20);
            this.label8.TabIndex = 40;
            this.label8.Text = "Please enter detailed information about events which cause this exception. ";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 20);
            this.label7.TabIndex = 40;
            this.label7.Text = "Framework";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxInformation
            // 
            this.textBoxInformation.AcceptsReturn = true;
            this.textBoxInformation.AcceptsTab = true;
            this.textBoxInformation.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxInformation.Location = new System.Drawing.Point(6, 187);
            this.textBoxInformation.Multiline = true;
            this.textBoxInformation.Name = "textBoxInformation";
            this.textBoxInformation.Size = new System.Drawing.Size(565, 145);
            this.textBoxInformation.TabIndex = 8;
            // 
            // textBoxFramework
            // 
            this.textBoxFramework.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxFramework.Location = new System.Drawing.Point(76, 132);
            this.textBoxFramework.Name = "textBoxFramework";
            this.textBoxFramework.ReadOnly = true;
            this.textBoxFramework.Size = new System.Drawing.Size(495, 20);
            this.textBoxFramework.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 20);
            this.label2.TabIndex = 30;
            this.label2.Text = "Version";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxVersion
            // 
            this.textBoxVersion.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxVersion.Location = new System.Drawing.Point(76, 103);
            this.textBoxVersion.Name = "textBoxVersion";
            this.textBoxVersion.ReadOnly = true;
            this.textBoxVersion.Size = new System.Drawing.Size(495, 20);
            this.textBoxVersion.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 20);
            this.label1.TabIndex = 28;
            this.label1.Text = "Application";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxApplication
            // 
            this.textBoxApplication.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxApplication.Location = new System.Drawing.Point(76, 77);
            this.textBoxApplication.Name = "textBoxApplication";
            this.textBoxApplication.ReadOnly = true;
            this.textBoxApplication.Size = new System.Drawing.Size(495, 20);
            this.textBoxApplication.TabIndex = 1;
            // 
            // picGeneral
            // 
            this.picGeneral.Image = ((System.Drawing.Image)(resources.GetObject("picGeneral.Image")));
            this.picGeneral.Location = new System.Drawing.Point(6, 6);
            this.picGeneral.Name = "picGeneral";
            this.picGeneral.Size = new System.Drawing.Size(64, 64);
            this.picGeneral.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picGeneral.TabIndex = 26;
            this.picGeneral.TabStop = false;
            // 
            // tabPageException
            // 
            this.tabPageException.Controls.Add(this.label5);
            this.tabPageException.Controls.Add(this.label4);
            this.tabPageException.Controls.Add(this.label3);
            this.tabPageException.Controls.Add(this.textBoxStackTrace);
            this.tabPageException.Controls.Add(this.textBoxSource);
            this.tabPageException.Controls.Add(this.textBoxMessage2);
            this.tabPageException.Location = new System.Drawing.Point(4, 22);
            this.tabPageException.Name = "tabPageException";
            this.tabPageException.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageException.Size = new System.Drawing.Size(577, 338);
            this.tabPageException.TabIndex = 1;
            this.tabPageException.Text = "Exception";
            this.tabPageException.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(559, 20);
            this.label5.TabIndex = 41;
            this.label5.Text = "Stack Trace";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(559, 20);
            this.label4.TabIndex = 41;
            this.label4.Text = "Source";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(559, 20);
            this.label3.TabIndex = 41;
            this.label3.Text = "Message";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxStackTrace
            // 
            this.textBoxStackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStackTrace.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxStackTrace.Location = new System.Drawing.Point(6, 168);
            this.textBoxStackTrace.Multiline = true;
            this.textBoxStackTrace.Name = "textBoxStackTrace";
            this.textBoxStackTrace.ReadOnly = true;
            this.textBoxStackTrace.Size = new System.Drawing.Size(565, 164);
            this.textBoxStackTrace.TabIndex = 2;
            // 
            // textBoxSource
            // 
            this.textBoxSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSource.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxSource.Location = new System.Drawing.Point(6, 119);
            this.textBoxSource.Multiline = true;
            this.textBoxSource.Name = "textBoxSource";
            this.textBoxSource.ReadOnly = true;
            this.textBoxSource.Size = new System.Drawing.Size(565, 23);
            this.textBoxSource.TabIndex = 1;
            // 
            // textBoxMessage2
            // 
            this.textBoxMessage2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMessage2.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxMessage2.Location = new System.Drawing.Point(6, 29);
            this.textBoxMessage2.Multiline = true;
            this.textBoxMessage2.Name = "textBoxMessage2";
            this.textBoxMessage2.ReadOnly = true;
            this.textBoxMessage2.Size = new System.Drawing.Size(565, 64);
            this.textBoxMessage2.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listViewAssemblies);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(577, 338);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "Assemblies";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listViewAssemblies
            // 
            this.listViewAssemblies.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listViewAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewAssemblies.FullRowSelect = true;
            this.listViewAssemblies.HotTracking = true;
            this.listViewAssemblies.HoverSelection = true;
            this.listViewAssemblies.Location = new System.Drawing.Point(3, 3);
            this.listViewAssemblies.Name = "listViewAssemblies";
            this.listViewAssemblies.Size = new System.Drawing.Size(571, 332);
            this.listViewAssemblies.TabIndex = 22;
            this.listViewAssemblies.UseCompatibleStateImageBehavior = false;
            this.listViewAssemblies.View = System.Windows.Forms.View.Details;
            // 
            // buttonClipboard
            // 
            this.buttonClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClipboard.Location = new System.Drawing.Point(383, 374);
            this.buttonClipboard.Name = "buttonClipboard";
            this.buttonClipboard.Size = new System.Drawing.Size(108, 23);
            this.buttonClipboard.TabIndex = 1;
            this.buttonClipboard.Text = "Copy to Clipboard";
            this.buttonClipboard.Click += new System.EventHandler(this.buttonClipboard_Click);
            // 
            // buttonSaveToFile
            // 
            this.buttonSaveToFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveToFile.Location = new System.Drawing.Point(269, 374);
            this.buttonSaveToFile.Name = "buttonSaveToFile";
            this.buttonSaveToFile.Size = new System.Drawing.Size(108, 23);
            this.buttonSaveToFile.TabIndex = 1;
            this.buttonSaveToFile.Text = "Save to File";
            this.buttonSaveToFile.Click += new System.EventHandler(this.buttonSaveToFile_Click);
            // 
            // StiExceptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 405);
            this.Controls.Add(this.tabControlGeneral);
            this.Controls.Add(this.buttonSaveToFile);
            this.Controls.Add(this.buttonClipboard);
            this.Controls.Add(this.btCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StiExceptionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exception Report";
            this.tabControlGeneral.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGeneral)).EndInit();
            this.tabPageException.ResumeLayout(false);
            this.tabPageException.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.TabControl tabControlGeneral;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPageException;
        private System.Windows.Forms.PictureBox picGeneral;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxInformation;
        private System.Windows.Forms.TextBox textBoxFramework;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxApplication;
        private System.Windows.Forms.TextBox textBoxStackTrace;
        private System.Windows.Forms.TextBox textBoxSource;
        private System.Windows.Forms.TextBox textBoxMessage2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView listViewAssemblies;
        private System.Windows.Forms.Button buttonClipboard;
        private System.Windows.Forms.Button buttonSaveToFile;

    }
}