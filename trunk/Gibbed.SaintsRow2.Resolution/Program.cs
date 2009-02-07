using System;
using System.IO;
using System.Windows.Forms;

namespace Gibbed.SaintsRow2.Resolution
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			path = Path.Combine(path, "THQ");
			path = Path.Combine(path, "Saints Row 2");
			path = Path.Combine(path, "settings.dat");

			if (File.Exists(path) == false)
			{
				MessageBox.Show(
					path + " does not exist." +
					Environment.NewLine +
					Environment.NewLine +
					"Please run Saints Row 2 at least once to create this file.",

					"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return;
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Editor(path));
		}
	}
}
