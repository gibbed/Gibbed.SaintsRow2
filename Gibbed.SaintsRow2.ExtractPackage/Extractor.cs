using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Gibbed.SaintsRow2.FileFormats;

namespace Gibbed.SaintsRow2.ExtractPackage
{
	public partial class Extractor : Form
	{
		public Extractor()
		{
			this.InitializeComponent();
		}

		delegate void SetProgressDelegate(long percent);
		private void SetProgress(long percent)
		{
			if (this.progressBar.InvokeRequired)
			{
				SetProgressDelegate callback = new SetProgressDelegate(SetProgress);
				this.Invoke(callback, new object[] { percent });
				return;
			}
			
			this.progressBar.Value = (int)percent;
		}

		delegate void LogDelegate(string message);
		private void Log(string message)
		{
			if (this.logText.InvokeRequired)
			{
				LogDelegate callback = new LogDelegate(Log);
				this.Invoke(callback, new object[] { message });
				return;
			}

			if (this.logText.Text.Length == 0)
			{
				this.logText.AppendText(message);
			}
			else
			{
				this.logText.AppendText(Environment.NewLine + message);
			}
		}

		delegate void EnableButtonsDelegate(bool extract);
		private void EnableButtons(bool extract)
		{
			if (this.extractButton.InvokeRequired || this.cancelButton.InvokeRequired)
			{
				EnableButtonsDelegate callback = new EnableButtonsDelegate(EnableButtons);
				this.Invoke(callback, new object[] { extract });
				return;
			}

			this.extractButton.Enabled = extract ? true : false;
			this.cancelButton.Enabled = extract ? false : true;
		}

		private void OnOpen(object sender, EventArgs e)
		{
			if (this.openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			string path = this.openFileDialog.FileName;
			string save = Path.GetDirectoryName(path);

			save = Path.Combine(Path.Combine(Path.Combine(save, "modding"), "extracted"), Path.GetFileNameWithoutExtension(path));
			Directory.CreateDirectory(save);

			this.savePathDialog.SelectedPath = save;
			if (this.savePathDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			save = this.savePathDialog.SelectedPath;

			PackageFile package = new PackageFile();
			Stream stream = File.OpenRead(path);
			package.Read(stream);
			stream.Close();

			this.progressBar.Minimum = 0;
			this.progressBar.Maximum = package.Entries.Count;
			this.progressBar.Value = 0;

			ExtractThreadInfo info = new ExtractThreadInfo();
			info.Save = save;
			info.Path = path;
			info.Package = package;

			this.ExtractThread = new Thread(new ParameterizedThreadStart(ExtractFiles));
			this.ExtractThread.Start(info);
			this.EnableButtons(false);
		}

		private Thread ExtractThread;
		private class ExtractThreadInfo
		{
			public string Save;
			public string Path;
			public PackageFile Package;
		}

		public void ExtractFiles(object oinfo)
		{
			long succeeded, failed, current;
			ExtractThreadInfo info = (ExtractThreadInfo)oinfo;

			Stream input = File.OpenRead(info.Path);

			succeeded = failed = current = 0;

			this.Log(String.Format("{0} files in package.", info.Package.Entries.Count));

			foreach (PackageEntry entry in info.Package.Entries)
			{
				this.SetProgress(++current);

				input.Seek(entry.Offset, SeekOrigin.Begin);

				string outputName = entry.Name + "." + entry.Extension;
				this.Log(outputName);

				Stream output = File.OpenWrite(Path.Combine(info.Save, outputName));

				if (entry.CompressedSize == -1)
				{
					long left = entry.UncompressedSize;
					byte[] data = new byte[4096];
					while (left > 0)
					{
						int block = (int)(Math.Min(left, 4096));
						input.Read(data, 0, block);
						output.Write(data, 0, block);
						left -= block;
					}
				}
				else
				{
					ICSharpCode.SharpZipLib.Zip.Compression.Inflater inflater =
						new ICSharpCode.SharpZipLib.Zip.Compression.Inflater();

					// compressed using zlib
					long left = entry.CompressedSize;
					byte[] compressedData = new byte[4096];
					byte[] data = new byte[4096];

					while (inflater.TotalOut < entry.UncompressedSize)
					{
						if (inflater.IsNeedingInput == true)
						{
							if (left == 0)
							{
								throw new Exception();
							}

							int block = (int)(Math.Min(left, 4096));
							input.Read(compressedData, 0, block);
							inflater.SetInput(compressedData, 0, block);
							left -= block;
						}

						int inflated = inflater.Inflate(data);
						output.Write(data, 0, inflated);
					}
				}

				output.Close();
				succeeded++;
			}

			input.Close();

			this.Log(String.Format("Done, {0} succeeded, {1} failed, {2} total.", succeeded, failed, info.Package.Entries.Count));
			this.EnableButtons(true);
		}

		private void OnCancel(object sender, EventArgs e)
		{
			if (this.ExtractThread != null)
			{
				this.ExtractThread.Abort();
			}

			this.Close();
		}

		private void OnLoad(object sender, EventArgs e)
		{
			string path = null;
			
			path = (string)Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Steam App 9480", "InstallLocation", null);

			if (path == null)
			{
				path = (string)Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Valve\\Steam", "InstallPath", null);

				if (path != null)
				{
					path = Path.Combine(Path.Combine(Path.Combine(path, "steamapps"), "common"), "saints row 2");
				}
			}

			this.openFileDialog.InitialDirectory = path;
		}
	}
}
