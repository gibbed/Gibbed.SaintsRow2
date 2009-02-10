using System;

namespace Gibbed.SaintsRow2.FileFormats
{
	public class FileFormatException : Exception
	{
		public FileFormatException()
			: base()
		{
		}

		public FileFormatException(string message)
			: base(message)
		{
		}

		public FileFormatException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class PackageFileException : FileFormatException
	{
		public PackageFileException()
			: base()
		{
		}

		public PackageFileException(string message)
			: base(message)
		{
		}

		public PackageFileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class NotAPackageFileException : PackageFileException
	{
		public NotAPackageFileException()
			: base()
		{
		}

		public NotAPackageFileException(string message)
			: base(message)
		{
		}

		public NotAPackageFileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class UnsupportedPackageFileVersionException : PackageFileException
	{
		public UnsupportedPackageFileVersionException()
			: base()
		{
		}

		public UnsupportedPackageFileVersionException(string message)
			: base(message)
		{
		}

		public UnsupportedPackageFileVersionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class PegFileException : FileFormatException
	{
		public PegFileException()
			: base()
		{
		}

		public PegFileException(string message)
			: base(message)
		{
		}

		public PegFileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class NotAPegFileException : PegFileException
	{
		public NotAPegFileException()
			: base()
		{
		}

		public NotAPegFileException(string message)
			: base(message)
		{
		}

		public NotAPegFileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class UnsupportedPegFileVersionException : PegFileException
	{
		public UnsupportedPegFileVersionException()
			: base()
		{
		}

		public UnsupportedPegFileVersionException(string message)
			: base(message)
		{
		}

		public UnsupportedPegFileVersionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class VintFileException : FileFormatException
	{
		public VintFileException()
			: base()
		{
		}

		public VintFileException(string message)
			: base(message)
		{
		}

		public VintFileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class NotAVintFileException : PegFileException
	{
		public NotAVintFileException()
			: base()
		{
		}

		public NotAVintFileException(string message)
			: base(message)
		{
		}

		public NotAVintFileException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class UnsupportedVintFileVersionException : PegFileException
	{
		public UnsupportedVintFileVersionException()
			: base()
		{
		}

		public UnsupportedVintFileVersionException(string message)
			: base(message)
		{
		}

		public UnsupportedVintFileVersionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
