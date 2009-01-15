using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Gibbed.SaintsRow2.Helpers;

namespace Gibbed.SaintsRow2.Package
{
	public class PackageFileException : Exception
	{
	}

	public class NotAPackageException : PackageFileException
	{
	}

	public class UnsupportedPackageVersionException : PackageFileException
	{
	}

	public class PackageEntry
	{
		public string Name;
		public string Extension;
		public long Offset;
		public int Size;
		public uint Unknown08;
		public uint Unknown14;
		public uint Unknown1C;
	}

	public class PackageFile
	{
		public UInt32 Version;
		public List<PackageEntry> Entries = new List<PackageEntry>();

		public void Read(Stream stream)
		{
			this.Entries.Clear();

			byte[] headerBuffer = new byte[384];
			stream.Read2048(headerBuffer, 0, 384);

			// Magic
			if (BitConverter.ToUInt32(headerBuffer, 0) != 0x51890ACE)
			{
				throw new NotAPackageException();
			}

			this.Version = BitConverter.ToUInt32(headerBuffer, 4);
			if (this.Version != 4)
			{
				throw new UnsupportedPackageVersionException();
			}

			int indexCount = BitConverter.ToInt32(headerBuffer, 0x154);
			int bufferSize;

			// File Index
			byte[] indexBuffer;
			bufferSize = BitConverter.ToInt32(headerBuffer, 0x15C);
			indexBuffer = new byte[bufferSize];
			stream.Read2048(indexBuffer, 0, bufferSize);

			// Names
			byte[] namesBuffer;
			bufferSize = BitConverter.ToInt32(headerBuffer, 0x160);
			namesBuffer = new byte[bufferSize];
			stream.Read2048(namesBuffer, 0, bufferSize);

			// Extensions
			byte[] extsBuffer;
			bufferSize = BitConverter.ToInt32(headerBuffer, 0x164);
			extsBuffer = new byte[bufferSize];
			stream.Read2048(extsBuffer, 0, bufferSize);

			long baseOffset = stream.Position;

			for (int i = 0; i < indexCount; i++)
			{
				PackageEntry entry = new PackageEntry();

				int offset = i * 28; // Each entry is 28 bytes long
				byte[] index = new byte[28];

				int nameOffset		= BitConverter.ToInt32 (indexBuffer, offset + 0x00);
				int extOffset		= BitConverter.ToInt32 (indexBuffer, offset + 0x04);
				entry.Unknown08		= BitConverter.ToUInt32(indexBuffer, offset + 0x08);
				entry.Offset		= BitConverter.ToInt32 (indexBuffer, offset + 0x0C) + baseOffset;
				entry.Size			= BitConverter.ToInt32 (indexBuffer, offset + 0x10);
				entry.Unknown14		= BitConverter.ToUInt32(indexBuffer, offset + 0x14);
				entry.Unknown1C		= BitConverter.ToUInt32(indexBuffer, offset + 0x18);

				entry.Name = namesBuffer.GetASCIIZ(nameOffset);
				entry.Extension = extsBuffer.GetASCIIZ(extOffset);

				if (entry.Unknown08 != 0 || entry.Unknown14 != 0xFFFFFFFF || entry.Unknown1C != 0)
				{
					throw new Exception();
				}

				this.Entries.Add(entry);
			}
		}
	}
}
