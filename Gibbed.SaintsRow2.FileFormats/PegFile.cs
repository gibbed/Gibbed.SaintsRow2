using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using Gibbed.SaintsRow2.Helpers;

namespace Gibbed.SaintsRow2.FileFormats
{
	public class PegFile
	{
		public List<PegEntry> Entries = new List<PegEntry>();

		public UInt32 FileSize;
		public UInt32 Unknown0C;
		public UInt32 Unknown10;
		public UInt32 Unknown16;

		public void Read(Stream stream)
		{
			this.Entries.Clear();

			if (stream.ReadU32() != 0x564B4547)
			{
				throw new NotAPegException("header must be GEKV");
			}

			if (stream.ReadU32() != 10)
			{
				throw new UnsupportedPackageVersionException("only version 10 is supported");
			}

			this.FileSize = stream.ReadU32();

			if (stream.Length != this.FileSize)
			{
				throw new PegFileException("size of file does not match size in header");
			}

			this.Unknown0C = stream.ReadU32();
			this.Unknown10 = stream.ReadU32();
			int count = stream.ReadU16();
			this.Unknown16 = stream.ReadU16();

			// Read names
			string[] names = new string[count];
			stream.Seek(0x18 + (0x30 * count), SeekOrigin.Begin);
			for (int i = 0; i < count; i++)
			{
				names[i] = stream.ReadASCIIZ();
			}

			// Read entries
			stream.Seek(0x18, SeekOrigin.Begin);
			for (int i = 0; i < count; i++)
			{
				byte[] data = new byte[0x30];
				stream.Read(data, 0, 0x30);
				PegEntry entry = new PegEntry((PegEntryData)data.BytesToStructure(typeof(PegEntryData)));
				entry.Name = names[i];
				this.Entries.Add(entry);
			}
		}
	}
}
