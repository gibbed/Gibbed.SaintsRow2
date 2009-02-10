using System;
using System.Collections.Generic;
using System.IO;
using Gibbed.SaintsRow2.Helpers;

namespace Gibbed.SaintsRow2.FileFormats
{
	public class PegFile
	{
		public List<PegEntry> Entries = new List<PegEntry>();

		public UInt32 FileSize;
		public UInt32 Unknown0C;
		public UInt16 Unknown12;
		public UInt32 Unknown16;

		public PegFrame ReadFrame(Stream stream)
		{
			byte[] data = new byte[0x30];
			stream.Read(data, 0, 0x30);
			return (PegFrame)data.BytesToStructure(typeof(PegFrame));
		}

		public void Read(Stream stream)
		{
			this.Entries.Clear();

			if (stream.ReadU32() != 0x564B4547)
			{
				throw new NotAPegFileException("header must be GEKV");
			}

			if (stream.ReadU32() != 10)
			{
				throw new UnsupportedPackageFileVersionException("only version 10 is supported");
			}

			this.FileSize = stream.ReadU32();

			if (stream.Length != this.FileSize)
			{
				throw new PegFileException("size of file does not match size in header");
			}

			this.Unknown0C = stream.ReadU32();
			int entryCount = stream.ReadU16();
			this.Unknown12 = stream.ReadU16();
			int frameCount = stream.ReadU16();
			this.Unknown16 = stream.ReadU16();

			// Read names
			string[] names = new string[entryCount];
			stream.Seek(0x18 + (0x30 * frameCount), SeekOrigin.Begin);
			for (int i = 0; i < entryCount; i++)
			{
				names[i] = stream.ReadASCIIZ();
			}

			int totalFrames = 0;

			// Read entries
			stream.Seek(0x18, SeekOrigin.Begin);
			for (int i = 0; i < entryCount; i++)
			{
				PegEntry entry = new PegEntry();
				entry.Name = names[i];

				PegFrame frame = this.ReadFrame(stream);
				entry.Frames.Add(frame);
				totalFrames++;

				if (frame.Frames == 0)
				{
					throw new Exception("frame count is 0");
				}
				else if (frame.Frames > 1)
				{
					/* The first frame of a peg will say how many frames are in the
					 * image (including itself), all subsequent frames have 1 for the
					 * frame count.
					 */
					for (int j = 1; j < frame.Frames; j++)
					{
						entry.Frames.Add(this.ReadFrame(stream));
						totalFrames++;
					}
				}

				this.Entries.Add(entry);
			}

			if (totalFrames != frameCount)
			{
				throw new Exception("something bad happened");
			}
		}
	}
}
