using System;

namespace Gibbed.SaintsRow2.FileFormats
{
	public class PackageFileException : Exception
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

	public class NotAPackageException : PackageFileException
	{
		public NotAPackageException()
			: base()
		{
		}

		public NotAPackageException(string message)
			: base(message)
		{
		}

		public NotAPackageException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class UnsupportedPackageVersionException : PackageFileException
	{
		public UnsupportedPackageVersionException()
			: base()
		{
		}

		public UnsupportedPackageVersionException(string message)
			: base(message)
		{
		}

		public UnsupportedPackageVersionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
	
	public class PegFileException : Exception
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

	public class NotAPegException : PegFileException
	{
		public NotAPegException()
			: base()
		{
		}

		public NotAPegException(string message)
			: base(message)
		{
		}

		public NotAPegException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	public class UnsupportedPegVersionException : PegFileException
	{
		public UnsupportedPegVersionException()
			: base()
		{
		}

		public UnsupportedPegVersionException(string message)
			: base(message)
		{
		}

		public UnsupportedPegVersionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
