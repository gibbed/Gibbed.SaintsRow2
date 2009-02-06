using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace Gibbed.SaintsRow2.ConvertPeg
{
	class Program
	{
		private static Bitmap MakeBitmapFromData(uint width, uint height, byte[] buffer, bool keepAlpha)
		{
			byte[] myBuffer = (byte[])buffer.Clone();
			Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

			for (uint i = 0; i < width * height * 4; i += 4)
			{
				byte v = myBuffer[i + 0];
				myBuffer[i + 0] = buffer[i + 2];
				myBuffer[i + 2] = v;
				if (keepAlpha == false)
				{
					myBuffer[i + 3] = 0xFF;
				}
			}

			Rectangle area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			BitmapData data = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
			Marshal.Copy(myBuffer, 0, data.Scan0, (int)(width * height * 4));
			bitmap.UnlockBits(data);
			return bitmap;
		}

		private static Bitmap MakeBitmapFromR5G6B5(uint width, uint height, byte[] buffer)
		{
			byte[] myBuffer = (byte[])buffer.Clone();
			Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format16bppRgb565);

			Rectangle area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			BitmapData data = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
			Marshal.Copy(myBuffer, 0, data.Scan0, (int)(width * height * 2));
			bitmap.UnlockBits(data);
			return bitmap;
		}

		private static Bitmap MakeBitmapFromA8R8G8B8(uint width, uint height, byte[] buffer)
		{
			byte[] myBuffer = (byte[])buffer.Clone();
			Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);
			Rectangle area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			BitmapData data = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
			Marshal.Copy(myBuffer, 0, data.Scan0, (int)(width * height * 4));
			bitmap.UnlockBits(data);
			return bitmap;
		}

		public static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				return;
			}

			string pegName = args[0];
			if (File.Exists(pegName) == false)
			{
				Console.WriteLine("{0} does not exist.", pegName);
				return;
			}

			string gpegName = Path.ChangeExtension(pegName, ".g_peg_pc");
			if (File.Exists(gpegName) == false)
			{
				Console.WriteLine("{0} does not exist.", gpegName);
				return;
			}

			Stream header = File.OpenRead(pegName);
			Stream data = File.OpenRead(gpegName);

			FileFormats.PegFile peg = new FileFormats.PegFile();
			peg.Read(header);

			foreach (FileFormats.PegEntry entry in peg.Entries)
			{
				data.Seek(entry.Offset, SeekOrigin.Begin);
				byte[] compressed = new byte[entry.Size];
				data.Read(compressed, 0, (int)entry.Size);

				// DXT
				if (entry.Format == FileFormats.PegFormat.DXT1 ||
					entry.Format == FileFormats.PegFormat.DXT3 ||
					entry.Format == FileFormats.PegFormat.DXT5)
				{
					Squish.Flags flags = 0;

					if (entry.Format == FileFormats.PegFormat.DXT1)
					{
						flags |= Squish.Flags.DXT1;
					}
					else if (entry.Format == FileFormats.PegFormat.DXT3)
					{
						flags |= Squish.Flags.DXT3;
					}
					else if (entry.Format == FileFormats.PegFormat.DXT5)
					{
						flags |= Squish.Flags.DXT5;
					}

					byte[] decompressed = new byte[entry.Width * entry.Height * 4];
					Squish.Decompress(decompressed, entry.Width, entry.Height, compressed, (int)flags);
					Bitmap bitmap = MakeBitmapFromData(entry.Width, entry.Height, decompressed, true);
					bitmap.Save(Path.ChangeExtension(entry.Name, ".png"), ImageFormat.Png);
				}
				// R5G6B5
				else if (entry.Format == FileFormats.PegFormat.R5G6B5)
				{
					Bitmap bitmap = MakeBitmapFromR5G6B5(entry.Width, entry.Height, compressed);
					bitmap.Save(Path.ChangeExtension(entry.Name, ".png"), ImageFormat.Png);
				}
				// A8R8G8B8
				else if (entry.Format == FileFormats.PegFormat.A8R8G8B8)
				{
					Bitmap bitmap = MakeBitmapFromA8R8G8B8(entry.Width, entry.Height, compressed);
					bitmap.Save(Path.ChangeExtension(entry.Name, ".png"), ImageFormat.Png);
				}
				else
				{
					throw new Exception("unhandled format " + entry.Format.ToString());
				}
			}
		}
	}
}
