using System;
using System.IO;
using System.Text;

namespace Gibbed.SaintsRow2.Helpers
{
	public static class StreamHelpers
	{
		public static bool ReadBoolean(this Stream stream)
		{
			return stream.ReadU8() > 0 ? true : false;
		}

		public static void WriteBoolean(this Stream stream, bool value)
		{
			stream.WriteU8((byte)(value == true ? 1 : 0));
		}

		public static byte ReadU8(this Stream stream)
		{
			return (byte)stream.ReadByte();
		}

		public static void WriteU8(this Stream stream, byte value)
		{
			stream.WriteByte(value);
		}

		public static char ReadS8(this Stream stream)
		{
			return (char)stream.ReadByte();
		}

		public static void WriteS8(this Stream stream, char value)
		{
			stream.WriteByte((byte)value);
		}

		public static Int16 ReadS16(this Stream stream)
		{
			byte[] data = new byte[2];
			stream.Read(data, 0, 2);
			return BitConverter.ToInt16(data, 0);
		}

		public static void WriteS16(this Stream stream, Int16 value)
		{
			byte[] data = BitConverter.GetBytes(value);
			stream.Write(data, 0, 2);
		}

		public static Int16 ReadS16BE(this Stream stream)
		{
			byte[] data = new byte[2];
			stream.Read(data, 0, 2);
			return BitConverter.ToInt16(data, 0).Swap();
		}

		public static void WriteS16BE(this Stream stream, Int16 value)
		{
			byte[] data = BitConverter.GetBytes(value.Swap());
			stream.Write(data, 0, 2);
		}

		public static UInt16 ReadU16(this Stream stream)
		{
			byte[] data = new byte[2];
			stream.Read(data, 0, 2);
			return BitConverter.ToUInt16(data, 0);
		}

		public static void WriteU16(this Stream stream, UInt16 value)
		{
			byte[] data = BitConverter.GetBytes(value);
			stream.Write(data, 0, 2);
		}

		public static UInt16 ReadU16BE(this Stream stream)
		{
			byte[] data = new byte[2];
			stream.Read(data, 0, 2);
			return BitConverter.ToUInt16(data, 0).Swap();
		}

		public static void WriteU16BE(this Stream stream, UInt16 value)
		{
			byte[] data = BitConverter.GetBytes(value.Swap());
			stream.Write(data, 0, 2);
		}

		public static UInt32 ReadU24BE(this Stream stream)
		{
			byte[] data = new byte[4];
			stream.Read(data, 1, 3);
			return BitConverter.ToUInt32(data, 0).Swap();
		}
		
		public static void WriteU24BE(this Stream stream, UInt32 value)
		{
			byte[] data = BitConverter.GetBytes(value.Swap());
			stream.Write(data, 1, 3);
		}

		public static Int32 ReadS32(this Stream stream)
		{
			byte[] data = new byte[4];
			stream.Read(data, 0, 4);
			return BitConverter.ToInt32(data, 0);
		}

		public static void WriteS32(this Stream stream, Int32 value)
		{
			byte[] data = BitConverter.GetBytes(value);
			stream.Write(data, 0, 4);
		}

		public static Int32 ReadS32BE(this Stream stream)
		{
			byte[] data = new byte[4];
			stream.Read(data, 0, 4);
			return BitConverter.ToInt32(data, 0).Swap();
		}

		public static void WriteS32BE(this Stream stream, Int32 value)
		{
			byte[] data = BitConverter.GetBytes(value.Swap());
			stream.Write(data, 0, 4);
		}

		public static UInt32 ReadU32(this Stream stream)
		{
			byte[] data = new byte[4];
			stream.Read(data, 0, 4);
			return BitConverter.ToUInt32(data, 0);
		}

		public static void WriteU32(this Stream stream, UInt32 value)
		{
			byte[] data = BitConverter.GetBytes(value);
			stream.Write(data, 0, 4);
		}

		public static UInt32 ReadU32BE(this Stream stream)
		{
			byte[] data = new byte[4];
			stream.Read(data, 0, 4);
			return BitConverter.ToUInt32(data, 0).Swap();
		}

		public static void WriteU32BE(this Stream stream, UInt32 value)
		{
			byte[] data = BitConverter.GetBytes(value.Swap());
			stream.Write(data, 0, 4);
		}

		public static Int64 ReadS64(this Stream stream)
		{
			byte[] data = new byte[8];
			stream.Read(data, 0, 8);
			return BitConverter.ToInt64(data, 0);
		}

		public static Int64 ReadS64BE(this Stream stream)
		{
			byte[] data = new byte[8];
			stream.Read(data, 0, 8);
			return BitConverter.ToInt64(data, 0).Swap();
		}

		public static UInt64 ReadU64(this Stream stream)
		{
			byte[] data = new byte[8];
			stream.Read(data, 0, 8);
			return BitConverter.ToUInt64(data, 0);
		}

		public static UInt64 ReadU64BE(this Stream stream)
		{
			byte[] data = new byte[8];
			stream.Read(data, 0, 8);
			return BitConverter.ToUInt64(data, 0).Swap();
		}

		public static Single ReadF32(this Stream stream)
		{
			byte[] data = new byte[4];
			stream.Read(data, 0, 4);
			return BitConverter.ToSingle(data, 0);
		}

		public static void WriteF32(this Stream stream, Single value)
		{
			byte[] data = BitConverter.GetBytes(value);
			stream.Write(data, 0, 4);
		}

		public static Single ReadF32BE(this Stream stream)
		{
			byte[] data = new byte[4];
			stream.Read(data, 0, 4);
			UInt32 value = BitConverter.ToUInt32(data, 0).Swap();
			data = BitConverter.GetBytes(value);
			return BitConverter.ToSingle(data, 0);
		}

		public static void WriteF32BE(this Stream stream, Single value)
		{
			byte[] data = BitConverter.GetBytes(value);
			UInt32 swappedvalue = BitConverter.ToUInt32(data, 0).Swap();
			data = BitConverter.GetBytes(swappedvalue);
			stream.Write(data, 0, 4);
		}

		public static Double ReadF64(this Stream stream)
		{
			byte[] data = new byte[8];
			stream.Read(data, 0, 8);
			return BitConverter.ToDouble(data, 0);
		}

		public static void WriteF64(this Stream stream, Double value)
		{
			byte[] data = BitConverter.GetBytes(value);
			stream.Write(data, 0, 8);
		}

		public static Double ReadF64BE(this Stream stream)
		{
			return BitConverter.Int64BitsToDouble((long)stream.ReadU64BE());
		}

		public static void WriteF64BE(this Stream stream, Double value)
		{
			byte[] data = BitConverter.GetBytes(value);
			UInt64 swappedvalue = BitConverter.ToUInt64(data, 0).Swap();
			data = BitConverter.GetBytes(swappedvalue);
			stream.Write(data, 0, 8);
		}

		public static string ReadASCII(this Stream stream, uint size)
		{
			byte[] data = new byte[size];
			stream.Read(data, 0, data.Length);
			return Encoding.ASCII.GetString(data);
		}

		public static string ReadASCIIZ(this Stream stream)
		{
			int i = 0;
			byte[] data = new byte[64];

			while (true)
			{
				stream.Read(data, i, 1);
				if (data[i] == 0)
				{
					break;
				}

				if (i >= data.Length)
				{
					if (data.Length >= 4096)
					{
						throw new InvalidOperationException();
					}

					Array.Resize(ref data, data.Length + 64);
				}

				i++;
			}

			if (i == 0)
			{
				return "";
			}

			return Encoding.ASCII.GetString(data, 0, i);
		}

		public static void WriteASCII(this Stream stream, string value)
		{
			byte[] data = Encoding.ASCII.GetBytes(value);
			stream.Write(data, 0, data.Length);
		}

		public static void WriteASCIIZ(this Stream stream, string value)
		{
			byte[] data = Encoding.ASCII.GetBytes(value);
			stream.Write(data, 0, data.Length);
			stream.WriteByte(0);
		}

		public static int ReadAligned(this Stream stream, byte[] buffer, int offset, int size, int align)
		{
			if (size == 0)
			{
				return 0;
			}

			int read = stream.Read(buffer, offset, size);
			int skip = size % align;

			// Skip aligned bytes
			if (skip > 0)
			{
				stream.Seek(align - skip, SeekOrigin.Current);
			}

			return read;
		}

		public static void WriteAligned(this Stream stream, byte[] buffer, int offset, int size, int align)
		{
			if (size == 0)
			{
				return;
			}

			stream.Write(buffer, offset, size);
			int skip = size % align;

			// this is a dumbfuck way to do this but it'll work for now
			if (skip > 0)
			{
				byte[] junk = new byte[align - skip];
				stream.Write(junk, 0, align - skip);
			}
		}
	}
}
