using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Gibbed.SaintsRow2.FileFormats;
using Gibbed.SaintsRow2.Helpers;

namespace Gibbed.SaintsRow2.BuildPackage
{
	public class MyPackageEntry : PackageEntry
	{
		public Stream FileStream;
	}

	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 2)
			{
				Console.WriteLine("{0} <package.vpp_pc> <directory> [<directory>[, <directory>[, ...]]]", Path.GetFileName(Application.ExecutablePath));
				return;
			}

			Dictionary<string, MyPackageEntry> files = new Dictionary<string, MyPackageEntry>();
			Stream stream = File.Create(args[0]);

			SortedDictionary<string, string> paths = new SortedDictionary<string, string>();

			long offset = 0;
			for (int i = 1; i < args.Length; i++)
			{
				string directory = args[i];

				foreach (string path in Directory.GetFiles(directory, "*"))
				{
					string fullPath = Path.GetFullPath(path);

					string name = Path.GetFileName(fullPath);

					if (Path.HasExtension(name) == false)
					{
						continue;
					}
					else if (Path.GetExtension(name) == ".vpp_pc")
					{
						continue;
					}
					else if (paths.ContainsKey(name))
					{
						continue;
					}

					paths[name] = fullPath;
					Console.WriteLine(fullPath);
				}
			}

			foreach (KeyValuePair<string, string> value in paths)
			{
				string name = value.Key;
				string path = value.Value;

				MyPackageEntry entry = new MyPackageEntry();
				entry.FileStream = File.OpenRead(path);

				entry.Name = Path.GetFileNameWithoutExtension(name);
				entry.Extension = Path.GetExtension(name).Substring(1);

				entry.Offset = offset;
				entry.UncompressedSize = (int)entry.FileStream.Length;
				entry.Unknown08 = 0;
				entry.CompressedSize = -1;
				entry.Unknown1C = 0;

				files[name] = entry;

				offset += entry.FileStream.Length.Align(16);
			}

			PackageFile package = new PackageFile();

			foreach (MyPackageEntry entry in files.Values)
			{
				package.Entries.Add(entry);
			}

			package.Write(stream);

			foreach (MyPackageEntry entry in package.Entries)
			{
				long size = entry.UncompressedSize;
				while (size > 0)
				{
					byte[] block = new byte[2048];
					int read = entry.FileStream.Read(block, 0, 2048);

					if (read == 0)
					{
						break;
					}

					stream.Write(block, 0, read);
					size -= read;
				}

				long align = entry.UncompressedSize.Align(16) - entry.UncompressedSize;
				if (align > 0)
				{
					byte[] block = new byte[align];
					stream.Write(block, 0, (int)align);
				}
			}

			stream.Close();

			foreach (MyPackageEntry entry in files.Values)
			{
				entry.FileStream.Close();
			}
		}
	}
}
