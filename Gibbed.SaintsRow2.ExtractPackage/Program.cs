using System;
using System.Windows.Forms;

namespace Gibbed.SaintsRow2.ExtractPackage
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Extractor());
		}
	}
}
