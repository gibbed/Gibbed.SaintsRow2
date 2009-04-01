using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
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

		[FieldOffset(0x14C)]
		public uint Flags;

		[FieldOffset(0x154)]
		public int IndexCount;

		[FieldOffset(0x158)]
		public int PackageSize;

		[FieldOffset(0x15C)]
		public int IndexSize;
		
		[FieldOffset(0x160)]
		public int NamesSize;
		
		[FieldOffset(0x164)]
		public int ExtensionsSize;

		[FieldOffset(0x168)]
		public int UncompressedDataSize;

		[FieldOffset(0x16C)]
		public uint CompressedDataSize;
	}

	public class PackageEntry
	{
		public string Name;
		public string Extension;
		public long Offset;
		public int UncompressedSize;
		public uint Unknown08;
		public int CompressedSize;
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
			if (stream.ReadAligned(headerBuffer, 0, 384, 2048) != 384)
			{
				throw new NotAPackageFileException();
			}

			PackageHeader header = (PackageHeader)headerBuffer.BytesToStructure(typeof(PackageHeader));

			bool bigEndian;

			// Magic
			if (header.Magic == 0x51890ACE)
			{
				bigEndian = false;
			}
			else if (header.Magic == 0xCE0A8951)
			{
				bigEndian = true;

				header.Version = header.Version.Swap();
				header.Flags = header.Flags.Swap();
				header.IndexCount = header.IndexCount.Swap();
				header.PackageSize = header.PackageSize.Swap();
				header.IndexSize = header.IndexSize.Swap();
				header.NamesSize = header.NamesSize.Swap();
				header.ExtensionsSize = header.ExtensionsSize.Swap();
				header.UncompressedDataSize = header.UncompressedDataSize.Swap();
				header.CompressedDataSize = header.CompressedDataSize.Swap();
			}
			else
			{
				throw new NotAPackageFileException();
			}

			if (header.Version != 4)
			{
				throw new UnsupportedPackageFileVersionException();
			}

			this.Version = header.Version;

			// File Index
			byte[] indexBuffer;
			indexBuffer = new byte[header.IndexSize];
			if (stream.ReadAligned(indexBuffer, 0, header.IndexSize, 2048) != header.IndexSize)
			{
				throw new PackageFileException();
			}

			// Names
			byte[] namesBuffer;
			namesBuffer = new byte[header.NamesSize];
			if (stream.ReadAligned(namesBuffer, 0, header.NamesSize, 2048) != header.NamesSize)
			{
				throw new PackageFileException();
			}

			// Extensions
			byte[] extensionsBuffer;
			extensionsBuffer = new byte[header.ExtensionsSize];
			if (stream.ReadAligned(extensionsBuffer, 0, header.ExtensionsSize, 2048) != header.ExtensionsSize)
			{
				throw new PackageFileException();
			}

			long baseOffset = stream.Position;

			for (int i = 0; i < header.IndexCount; i++)
			{
				PackageEntry entry = new PackageEntry();

				int offset = i * 28; // Each index entry is 28 bytes long
				byte[] index = new byte[28];

				int nameOffset;
				int extensionOffset;

				if (bigEndian == false)
				{
					nameOffset				= BitConverter.ToInt32 (indexBuffer, offset + 0x00);
					extensionOffset			= BitConverter.ToInt32 (indexBuffer, offset + 0x04);
					entry.Unknown08			= BitConverter.ToUInt32(indexBuffer, offset + 0x08);
					entry.Offset			= BitConverter.ToInt32 (indexBuffer, offset + 0x0C);// + baseOffset;
					entry.UncompressedSize	= BitConverter.ToInt32 (indexBuffer, offset + 0x10);
					entry.CompressedSize	= BitConverter.ToInt32 (indexBuffer, offset + 0x14);
					entry.Unknown1C			= BitConverter.ToUInt32(indexBuffer, offset + 0x18);
				}
				else
				{
					nameOffset				= BitConverter.ToInt32 (indexBuffer, offset + 0x00).Swap();
					extensionOffset			= BitConverter.ToInt32 (indexBuffer, offset + 0x04).Swap();
					entry.Unknown08			= BitConverter.ToUInt32(indexBuffer, offset + 0x08).Swap();
					entry.Offset			= BitConverter.ToInt32 (indexBuffer, offset + 0x0C).Swap();
					entry.UncompressedSize	= BitConverter.ToInt32 (indexBuffer, offset + 0x10).Swap();
					entry.CompressedSize	= BitConverter.ToInt32 (indexBuffer, offset + 0x14).Swap();
					entry.Unknown1C			= BitConverter.ToUInt32(indexBuffer, offset + 0x18).Swap();
				}

				// package is compressed with zlib, offsets are not correct, fix 'em
				// compression occurs in the 360 version of Saints Row 2 packages
				// this should work (I hope)
				if ((header.Flags & 1) == 1)
				{
					entry.Offset = baseOffset;
					baseOffset += entry.CompressedSize.Align(2048);
				}
				else
				{
					entry.Offset += baseOffset;
				}

				entry.Name = namesBuffer.GetASCIIZ(nameOffset);
				entry.Extension = extensionsBuffer.GetASCIIZ(extensionOffset);

				if (entry.Unknown08 != 0 || entry.Unknown1C != 0)
				{
					throw new Exception();
				}

				if ((header.Flags & 1) != 1 && entry.CompressedSize != -1)
				{
					throw new Exception();
				}

				this.Entries.Add(entry);
			}
		}

		public void Write(Stream stream)
		{
			MemoryStream memory;

			PackageHeader header = new PackageHeader();
			header.Magic = 0x51890ACE;
			header.Version = 4; // this.Version
			header.CompressedDataSize = 0xFFFFFFFF;
			header.Flags = 2; // I think this flag means 'preload' or something of that sort. Patch has it set at least.

			List<string> names = new List<string>();
			List<string> extensions = new List<string>();
			Dictionary<string, int> nameOffsets = new Dictionary<string, int>();
			Dictionary<string, int> extensionOffsets = new Dictionary<string, int>();

			foreach (PackageEntry entry in this.Entries)
			{
				if (names.Contains(entry.Name) == false)
				{
					names.Add(entry.Name);
				}

				if (extensions.Contains(entry.Extension) == false)
				{
					extensions.Add(entry.Extension);
				}
			}

			names.Sort();
			extensions.Sort();

			memory = new MemoryStream();
			foreach (string name in names)
			{
				nameOffsets[name] = (int)memory.Position;
				memory.WriteASCIIZ(name);
			}
			header.NamesSize = (int)memory.Length;
			byte[] namesBuffer = memory.GetBuffer();

			memory = new MemoryStream();
			foreach (string extension in extensions)
			{
				extensionOffsets[extension] = (int)memory.Position;
				memory.WriteASCIIZ(extension);
			}
			header.ExtensionsSize = (int)memory.Length;
			byte[] extensionsBuffer = memory.GetBuffer();

			int totalSize = 0;
			memory = new MemoryStream();
			foreach (PackageEntry entry in this.Entries)
			{
				memory.WriteS32(nameOffsets[entry.Name]);
				memory.WriteS32(extensionOffsets[entry.Extension]);
				memory.WriteU32(entry.Unknown08);
				memory.WriteU32((uint)entry.Offset);
				memory.WriteS32(entry.UncompressedSize);
				memory.WriteS32(entry.CompressedSize);
				memory.WriteU32(entry.Unknown1C);
				totalSize += (int)entry.UncompressedSize.Align(16);
			}
			header.IndexSize = (int)memory.Length;
			byte[] indexBuffer = memory.GetBuffer();

			header.IndexCount = this.Entries.Count;

			header.UncompressedDataSize = totalSize;

			totalSize += 2048; // header
			totalSize += header.IndexSize.Align(2048); // index
			totalSize += header.NamesSize.Align(2048); // names
			totalSize += header.ExtensionsSize.Align(2048); // extensions

			header.PackageSize = totalSize;

			int headerSize = Marshal.SizeOf(typeof(PackageHeader));
			byte[] headerBuffer = new byte[headerSize];
			GCHandle headerHandle = GCHandle.Alloc(headerBuffer, GCHandleType.Pinned);
			Marshal.StructureToPtr(header, headerHandle.AddrOfPinnedObject(), false);
			headerHandle.Free();

			stream.WriteAligned(headerBuffer, 0, headerBuffer.Length, 2048);
			stream.WriteAligned(indexBuffer, 0, header.IndexSize, 2048);
			stream.WriteAligned(namesBuffer, 0, header.NamesSize, 2048);
			stream.WriteAligned(extensionsBuffer, 0, header.ExtensionsSize, 2048);
		}
	}
}
