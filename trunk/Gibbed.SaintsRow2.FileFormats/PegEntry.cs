using System;
using System.Runtime.InteropServices;

namespace Gibbed.SaintsRow2.FileFormats
{
	public enum PegFormat
	{
		Unknown = 0,
		DXT1 = 400,
		DXT3 = 401,
		DXT5 = 402,
		R5G6B5 = 403,
		A1R5G5B5 = 404,
		A4R4G4B4 = 405,
		R8G8B8 = 406,
		A8R8G8B8 = 407,
		V8U8 = 408,
		CxV8U8 = 409,
		A8 = 410,
	}

	public class PegEntry
	{
		public PegEntryData Data;
		public string Name;

		public PegEntry()
		{
			this.Data = new PegEntryData();
		}

		public PegEntry(PegEntryData data)
		{
			this.Data = data;
		}

		public UInt32 Offset
		{
			get { return this.Data.Offset; }
			set { this.Data.Offset = value; }
		}

		public UInt16 Width
		{
			get { return this.Data.Width; }
			set { this.Data.Width = value; }
		}

		public UInt16 Height
		{
			get { return this.Data.Height; }
			set { this.Data.Height = value; }
		}

		public PegFormat Format
		{
			get { return (PegFormat)this.Data.Format; }
			set { this.Data.Format = (uint)value; }
		}

		public UInt32 Size
		{
			get { return this.Data.Size; }
			set { this.Data.Size = value; }
		}
	}

	[StructLayout(LayoutKind.Explicit, Size = 0x30)]
	public struct PegEntryData
	{
		[FieldOffset(0x00)]
		public UInt32 Offset;

		[FieldOffset(0x04)]
		public UInt16 Width;

		[FieldOffset(0x06)]
		public UInt16 Height;

		/* The format value does not directly correspond with the
		 * D3D format types, but they are mapped as such:
		 * 
		 * 400		D3DFMT_DXT1
		 * 401		D3DFMT_DXT3
		 * 402		D3DFMT_DXT5
		 * 403		D3DFMT_R5G6B5
		 * 404		D3DFMT_A1R5G5B5
		 * 405		D3DFMT_A4R4G4B4
		 * 406		D3DFMT_R8G8B8
		 * 407		D3DFMT_A8R8G8B8
		 * 408		D3DFMT_V8U8
		 * 409		D3DFMT_CxV8U8
		 * 410		D3DFMT_A8
		 */
		[FieldOffset(0x08)]
		public UInt32 Format;

		[FieldOffset(0x0C)]
		public UInt32 Unknown0C;

		[FieldOffset(0x10)]
		public UInt32 Unknown10;

		[FieldOffset(0x14)]
		public UInt32 Unknown14; // used for a pointer to the string by game

		[FieldOffset(0x18)]
		public UInt32 Unknown18;

		[FieldOffset(0x1C)]
		public UInt32 Size;

		[FieldOffset(0x20)]
		public UInt32 Unknown20;

		[FieldOffset(0x24)]
		public UInt32 Unknown24;

		[FieldOffset(0x28)]
		public UInt32 Unknown28;

		[FieldOffset(0x2C)]
		public UInt32 Unknown2C;
	}
}
