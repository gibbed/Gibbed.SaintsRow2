namespace Gibbed.SaintsRow2.Resolution
{
	partial class Editor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
			this.resolutionGroup = new System.Windows.Forms.GroupBox();
			this.heightText = new System.Windows.Forms.TextBox();
			this.widthText = new System.Windows.Forms.TextBox();
			this.xLabel = new System.Windows.Forms.Label();
			this.saveButton = new System.Windows.Forms.Button();
			this.resolutionGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// resolutionGroup
			// 
			this.resolutionGroup.Controls.Add(this.heightText);
			this.resolutionGroup.Controls.Add(this.widthText);
			this.resolutionGroup.Controls.Add(this.xLabel);
			this.resolutionGroup.Location = new System.Drawing.Point(12, 12);
			this.resolutionGroup.Name = "resolutionGroup";
			this.resolutionGroup.Size = new System.Drawing.Size(144, 51);
			this.resolutionGroup.TabIndex = 0;
			this.resolutionGroup.TabStop = false;
			this.resolutionGroup.Text = "Resolution";
			// 
			// heightText
			// 
			this.heightText.Location = new System.Drawing.Point(88, 19);
			this.heightText.Name = "heightText";
			this.heightText.Size = new System.Drawing.Size(48, 20);
			this.heightText.TabIndex = 1;
			// 
			// widthText
			// 
			this.widthText.Location = new System.Drawing.Point(6, 19);
			this.widthText.Name = "widthText";
			this.widthText.Size = new System.Drawing.Size(48, 20);
			this.widthText.TabIndex = 0;
			// 
			// xLabel
			// 
			this.xLabel.Location = new System.Drawing.Point(60, 18);
			this.xLabel.Name = "xLabel";
			this.xLabel.Size = new System.Drawing.Size(22, 21);
			this.xLabel.TabIndex = 1;
			this.xLabel.Text = "x";
			this.xLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(81, 75);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(75, 23);
			this.saveButton.TabIndex = 2;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.OnSave);
			// 
			// Editor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(168, 110);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.resolutionGroup);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Editor";
			this.Text = "Edit Settings";
			this.resolutionGroup.ResumeLayout(false);
			this.resolutionGroup.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox resolutionGroup;
		private System.Windows.Forms.Label xLabel;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.TextBox widthText;
		private System.Windows.Forms.TextBox heightText;
	}
}

