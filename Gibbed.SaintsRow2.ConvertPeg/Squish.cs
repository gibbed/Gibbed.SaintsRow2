using System.Runtime.InteropServices;

namespace Gibbed.SaintsRow2.ConvertPeg
{
	public class Squish
	{
		public enum Flags
		{
			DXT1 = (1 << 0),
			DXT3 = (1 << 1),
			DXT5 = (1 << 2),
			ColourClusterFit = (1 << 3),
			ColourRangeFit = (1 << 4),
			ColourMetricPerceptual = (1 << 5),
			ColourMetricUniform = (1 << 6),
			WeightColourByAlpha = (1 << 7),
			ColourIterativeClusterFit = (1 << 8),
		}

		[DllImport("squish.dll", EntryPoint = "DecompressImage")]
		public static extern void Decompress([MarshalAs(UnmanagedType.LPArray)] byte[] rgba, uint width, uint height, [MarshalAs(UnmanagedType.LPArray)] byte[] blocks, int flags);
	}
}
