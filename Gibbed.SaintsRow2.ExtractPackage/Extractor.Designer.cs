namespace Gibbed.SaintsRow2.ExtractPackage
{
	partial class Extractor
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
			this.logText = new System.Windows.Forms.TextBox();
			this.extractButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.savePathDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.SuspendLayout();
			// 
			// logText
			// 
			this.logText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.logText.BackColor = System.Drawing.SystemColors.Window;
			this.logText.Location = new System.Drawing.Point(12, 12);
			this.logText.Multiline = true;
			this.logText.Name = "logText";
			this.logText.ReadOnly = true;
			this.logText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.logText.Size = new System.Drawing.Size(608, 130);
			this.logText.TabIndex = 0;
			// 
			// extractButton
			// 
			this.extractButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.extractButton.Location = new System.Drawing.Point(545, 177);
			this.extractButton.Name = "extractButton";
			this.extractButton.Size = new System.Drawing.Size(75, 23);
			this.extractButton.TabIndex = 1;
			this.extractButton.Text = "&Extract";
			this.extractButton.UseVisualStyleBackColor = true;
			this.extractButton.Click += new System.EventHandler(this.OnOpen);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.Enabled = false;
			this.cancelButton.Location = new System.Drawing.Point(464, 177);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.OnCancel);
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.Location = new System.Drawing.Point(12, 148);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(608, 23);
			this.progressBar.TabIndex = 3;
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "vpp_pc";
			this.openFileDialog.Filter = "Package Files (*.vpp_*)|*.vpp_*|PC Package Files (*.vpp_pc)|*.vpp_pc|XBOX 360 Pac" +
				"kage Files (*.vpp_xbox2)|*.vpp_xbox2|All Files (*.*)|*.*";
			// 
			// savePathDialog
			// 
			this.savePathDialog.Description = "Select a directory to extract files to. Don\'t chose the root game directory unles" +
				"s you really want a huge mess.";
			// 
			// Extractor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(632, 212);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.extractButton);
			this.Controls.Add(this.logText);
			this.Name = "Extractor";
			this.Text = "Gibbed\'s Saints Row 2 VPP Extractor";
			this.Load += new System.EventHandler(this.OnLoad);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox logText;
		private System.Windows.Forms.Button extractButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.FolderBrowserDialog savePathDialog;

	}
}

