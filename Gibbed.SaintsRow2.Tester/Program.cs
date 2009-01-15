using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gibbed.SaintsRow2.Package;
using Gibbed.SaintsRow2.FileFormats;
using Gibbed.SaintsRow2.Helpers;
using System.Xml;

namespace Gibbed.SaintsRow2.Tester
{
	class Program
	{
		static public LeStringsFile StringsFile;

		static void Main(string[] args)
		{
			/*
			// Test reading of VPP
			Stream stream = File.OpenRead("patch.vpp_pc");
			PackageFile package = new PackageFile();
			package.Read(stream);
			stream.Close();
			*/

			// Test reading of LeStrings
			Stream stream = File.OpenRead("static_US.le_strings");
			StringsFile = new LeStringsFile();
			StringsFile.Read(stream);
			stream.Close();
			Console.WriteLine(StringsFile.Strings["WRATH_OF_GOD".KeyCRC32()]);
		}
	}
}
