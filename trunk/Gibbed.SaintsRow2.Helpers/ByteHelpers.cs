using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Gibbed.SaintsRow2.Helpers
{
	public static class ByteHelpers
	{
		public static object BytesToStructure(this byte[] data, Type type)
		{
			if (data.Length != Marshal.SizeOf(type))
			{
				throw new Exception("structure size is not the same as the data size");
			}

			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			object structure = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), type);
			handle.Free();
			return structure;
		}

		public static string GetASCIIZ(this byte[] data, int offset)
		{
			int i = offset;

			while (i < data.Length)
			{
				if (data[i] == 0)
				{
					break;
				}

				i++;
			}

			if (i == offset)
			{
				return "";
			}

			return Encoding.ASCII.GetString(data, offset, i - offset);
		}

		public static string GetASCIIZ(this byte[] data, uint offset)
		{
			return data.GetASCIIZ((int)offset);
		}
	}
}
