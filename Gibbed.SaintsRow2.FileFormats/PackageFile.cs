using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Gibbed.SaintsRow2.Helpers;

namespace Gibbed.SaintsRow2.FileFormats
{
	[StructLayout(LayoutKind.Explicit, Size = 384)]
	public struct PackageHeader
	{
		[FieldOffset(0)]
		public uint Magic;

		[FieldOffset(4)]
		public uint Version;

		[FieldOffset(0x154)]
		public int IndexCount;

		[FieldOffset(0x15C)]
		public int IndexSize;
		
		[FieldOffset(0x160)]
		public int NamesSize;
		
		[FieldOffset(0x164)]
		public int ExtensionsSize;
	}

	public class PackageEntry
	{
		public int NameOffset;
		public string Name;
		public int ExtensionOffset;
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
			if (stream.Read2048(headerBuffer, 0, 384) != 384)
			{
				throw new NotAPackageException();
			}

			PackageHeader header = (PackageHeader)headerBuffer.BytesToStructure(typeof(PackageHeader));

			// Magic
			if (header.Magic != 0x51890ACE)
			{
				throw new NotAPackageException();
			}

			if (header.Version != 4)
			{
				throw new UnsupportedPackageVersionException();
			}

			this.Version = header.Version;

			int bufferSize;

			// File Index
			byte[] indexBuffer;
			indexBuffer = new byte[header.IndexSize];
			if (stream.Read2048(indexBuffer, 0, header.IndexSize) != header.IndexSize)
			{
				throw new PackageFileException();
			}

			// Names
			byte[] namesBuffer;
			namesBuffer = new byte[header.NamesSize];
			if (stream.Read2048(namesBuffer, 0, header.NamesSize) != header.NamesSize)
			{
				throw new PackageFileException();
			}

			// Extensions
			byte[] extensionsBuffer;
			extensionsBuffer = new byte[header.ExtensionsSize];
			if (stream.Read2048(extensionsBuffer, 0, header.ExtensionsSize) != header.ExtensionsSize)
			{
				throw new PackageFileException();
			}

			long baseOffset = stream.Position;

			for (int i = 0; i < header.IndexCount; i++)
			{
				PackageEntry entry = new PackageEntry();

				int offset = i * 28; // Each index entry is 28 bytes long
				byte[] index = new byte[28];

				entry.NameOffset		= BitConverter.ToInt32 (indexBuffer, offset + 0x00);
				entry.ExtensionOffset	= BitConverter.ToInt32 (indexBuffer, offset + 0x04);
				entry.Unknown08			= BitConverter.ToUInt32(indexBuffer, offset + 0x08);
				entry.Offset			= BitConverter.ToInt32 (indexBuffer, offset + 0x0C) + baseOffset;
				entry.Size				= BitConverter.ToInt32 (indexBuffer, offset + 0x10);
				entry.Unknown14			= BitConverter.ToUInt32(indexBuffer, offset + 0x14);
				entry.Unknown1C			= BitConverter.ToUInt32(indexBuffer, offset + 0x18);

				entry.Name = namesBuffer.GetASCIIZ(entry.NameOffset);
				entry.Extension = extensionsBuffer.GetASCIIZ(entry.ExtensionOffset);

				if (entry.Unknown08 != 0 || entry.Unknown14 != 0xFFFFFFFF || entry.Unknown1C != 0)
				{
					throw new Exception();
				}

				this.Entries.Add(entry);
			}
		}

		public void Write(Stream stream)
		{
			throw new NotImplementedException();
		}
	}
}
