using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gibbed.SaintsRow2.ConvertPeg
{
	class Program
	{
		private static Bitmap MakeBitmapFromDXT(uint width, uint height, byte[] buffer, bool keepAlpha)
		{
			Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);

			for (uint i = 0; i < width * height * 4; i += 4)
			{
				// flip red and blue
				byte r = buffer[i + 0];
				buffer[i + 0] = buffer[i + 2];
				buffer[i + 2] = r;
			}

			Rectangle area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			BitmapData data = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
			Marshal.Copy(buffer, 0, data.Scan0, (int)(width * height * 4));
			bitmap.UnlockBits(data);
			return bitmap;
		}

		private static Bitmap MakeBitmapFromR5G6B5(uint width, uint height, byte[] buffer)
		{
			Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format16bppRgb565);
			Rectangle area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			BitmapData data = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
			Marshal.Copy(buffer, 0, data.Scan0, (int)(width * height * 2));
			bitmap.UnlockBits(data);
			return bitmap;
		}

		private static Bitmap MakeBitmapFromA8R8G8B8(uint width, uint height, byte[] buffer)
		{
			Bitmap bitmap = new Bitmap((int)width, (int)height, PixelFormat.Format32bppArgb);
			Rectangle area = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			BitmapData data = bitmap.LockBits(area, ImageLockMode.WriteOnly, bitmap.PixelFormat);
			Marshal.Copy(buffer, 0, data.Scan0, (int)(width * height * 4));
			bitmap.UnlockBits(data);
			return bitmap;
		}

		private static void Convert(string pegName)
		{
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
				int index = 0;
				foreach (FileFormats.PegFrame frame in entry.Frames)
				{
					data.Seek(frame.Offset, SeekOrigin.Begin);
					byte[] compressed = new byte[frame.Size];
					data.Read(compressed, 0, (int)frame.Size);

					FileFormats.PegFormat format = (FileFormats.PegFormat)frame.Format;

					Bitmap bitmap;

					// DXT
					if (format == FileFormats.PegFormat.DXT1 ||
						format == FileFormats.PegFormat.DXT3 ||
						format == FileFormats.PegFormat.DXT5)
					{
						Squish.Flags flags = 0;

						if (format == FileFormats.PegFormat.DXT1)
						{
							flags |= Squish.Flags.DXT1;
						}
						else if (format == FileFormats.PegFormat.DXT3)
						{
							flags |= Squish.Flags.DXT3;
						}
						else if (format == FileFormats.PegFormat.DXT5)
						{
							flags |= Squish.Flags.DXT5;
						}

						byte[] decompressed = new byte[frame.Width * frame.Height * 4];
						Squish.Decompress(decompressed, frame.Width, frame.Height, compressed, (int)flags);
						bitmap = MakeBitmapFromDXT(frame.Width, frame.Height, decompressed, true);
					}
					// R5G6B5
					else if (format == FileFormats.PegFormat.R5G6B5)
					{
						bitmap = MakeBitmapFromR5G6B5(frame.Width, frame.Height, compressed);
					}
					// A8R8G8B8
					else if (format == FileFormats.PegFormat.A8R8G8B8)
					{
						bitmap = MakeBitmapFromA8R8G8B8(frame.Width, frame.Height, compressed);
					}
					else
					{
						throw new Exception("unhandled format " + frame.Format.ToString());
					}

					string prefix = Path.ChangeExtension(pegName, null) + "   ";

					if (entry.Frames.Count == 1)
					{
						bitmap.Save(prefix + Path.ChangeExtension(entry.Name, ".png"), ImageFormat.Png);
					}
					else
					{
						string name = Path.GetFileNameWithoutExtension(entry.Name);
						name += " (frame " + index.ToString() + ")";

						bitmap.Save(prefix + Path.ChangeExtension(name, ".png"), ImageFormat.Png);
					}

					index++;
				}
			}
		}

		public static void Main(string[] args)
		{
			string directory;

			if (args.Length == 0)
			{
				directory = ".";
			}
			else if (args.Length == 1)
			{
				directory = args[1];
			}
			else
			{
				Console.WriteLine("{0} [directory with peg files]", Path.GetFileName(Application.ExecutablePath));
				return;
			}

			foreach (string path in Directory.GetFiles(Path.GetFullPath(directory), "*.peg_pc"))
			{
				Convert(path);
			}
		}
	}
}
