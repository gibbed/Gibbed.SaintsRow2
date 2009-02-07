using System;
using System.IO;
using System.Windows.Forms;

namespace Gibbed.SaintsRow2.Resolution
{
	public partial class Editor : Form
	{
		private string SettingsPath;

		private void ReadResolution()
		{
			BinaryReader reader = new BinaryReader(File.OpenRead(this.SettingsPath));
			reader.BaseStream.Seek(0x3C, SeekOrigin.Begin);
			this.widthText.Text = reader.ReadInt32().ToString();
			reader.BaseStream.Seek(0x40, SeekOrigin.Begin);
			this.heightText.Text = reader.ReadInt32().ToString();
			reader.Close();
		}

		private void WriteResolution()
		{
			BinaryWriter writer = new BinaryWriter(File.OpenWrite(this.SettingsPath));
			writer.BaseStream.Seek(0x3C, SeekOrigin.Begin);
			writer.Write(int.Parse(this.widthText.Text));
			writer.BaseStream.Seek(0x40, SeekOrigin.Begin);
			writer.Write(int.Parse(this.heightText.Text));
			writer.Close();
		}

		public Editor(string path)
		{
			this.InitializeComponent();
			this.SettingsPath = path;
			this.ReadResolution();
		}

		private void OnSave(object sender, EventArgs e)
		{
			try
			{
				int.Parse(this.widthText.Text);
				int.Parse(this.heightText.Text);
			}
			catch (FormatException)
			{
				MessageBox.Show(this, "Invalid input for width or height.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			this.WriteResolution();
		}
	}
}
