using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.SaintsRow2.Helpers;
using Gibbed.SaintsRow2.FileFormats;
using Gibbed.SaintsRow2.XmlTable;

namespace Gibbed.SaintsRow2.Tester
{
	public class Program
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

			/*
			// Test reading of LeStrings
			Stream stream = File.OpenRead("static_US.le_strings");
			StringsFile = new LeStringsFile();
			StringsFile.Read(stream);
			stream.Close();
			Console.WriteLine(StringsFile.Strings["DROP_DESTINY".KeyCRC32()]);
			*/

			/*
			// Check if my created vpp is correct :)
			Stream stream;

			stream = File.OpenRead("patch.vpp_pc");
			PackageFile patch = new PackageFile();
			patch.Read(stream);
			long patchSize = stream.Length;
			stream.Close();

			stream = File.OpenRead("patch_test.vpp_pc");
			PackageFile test = new PackageFile();
			test.Read(stream);
			long testSize = stream.Length;
			stream.Close();

			SortedDictionary<string, long> patch_sizes = GetSizes(patch, patchSize);
			SortedDictionary<string, long> test_sizes = GetSizes(test, testSize);

			foreach (string name in patch_sizes.Keys)
			{
				if (test_sizes.ContainsKey(name) == false)
				{
					Console.WriteLine("{0} is missing", name);
					continue;
				}

				if (test_sizes[name] != patch_sizes[name])
				{
					Console.WriteLine("{0}: {1} vs {2}", name, patch_sizes[name], test_sizes[name]);
				}
			}
			*/

			/*
			// Test reading of vint_doc
			foreach (string path in Directory.GetFiles(".", "*.vint_doc"))
			{
				FileStream stream = File.OpenRead(path);
				VintFile vintFile = new VintFile();
				vintFile.Read(stream);
				stream.Close();
			}
			*/

			foreach (string path in Directory.GetFiles("extracted\\common", "*.xtbl"))
			{
				Stream stream = File.OpenRead(path);
				SerializationBuilder builder = new SerializationBuilder();
				builder.ReadXmlTable(stream);
				stream.Close();
			}
		}

		private static SortedDictionary<string, long> GetSizes(PackageFile package, long totalSize)
		{
			SortedDictionary<string, long> result = new SortedDictionary<string, long>();
			PackageEntry last = null;

			string name;

			foreach (PackageEntry entry in package.Entries)
			{
				if (last != null)
				{
					name = last.Name + "." + last.Extension;
					result[name] = entry.Offset - last.Offset;
				}

				last = entry;
			}

			name = last.Name + "." + last.Extension;
			result[name] = totalSize - last.Offset;
			return result;
		}
	}
}
