using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace GeoTask.GeoUpdate
{
	public interface IGeo2LiteExtractor: IDisposable
	{
		void Extract(string zipPath);

		CsvBlockReader BlocksReader { get; }
		CsvLocationReader[] LocationsReader { get; }
	}

	//TODO: change this class
	internal class Geo2LiteExtractor : IGeo2LiteExtractor
	{
		//Ipv4+Ipv6
		public CsvBlockReader BlocksReader { get; private set; }

		//Locales
		public CsvLocationReader[] LocationsReader { get; private set; }

		private string _workingDirectory;

		public Geo2LiteExtractor()
		{

		}

		public void Extract(string zipPath)
		{
			if (_workingDirectory != null)
				return; // already extracted

			_workingDirectory = Path.Combine(Path.GetDirectoryName(zipPath), "geoUpdate_csv");

			if (!Directory.Exists(_workingDirectory))
				Directory.CreateDirectory(_workingDirectory);

			using ZipFile zip = new ZipFile(zipPath);

			foreach (ZipEntry ze in zip)
			{
				if (ze.IsFile)
				{
					using var fs = new FileStream(Path.Combine(_workingDirectory, Path.GetFileName(ze.Name)), FileMode.OpenOrCreate, FileAccess.Write);
					zip.GetInputStream(ze).CopyTo(fs);
				}
			}

			List<string> ipAddressBlocks = new List<string>(2);
			List<CsvLocationReader> locationReaders = new List<CsvLocationReader>(8);

			foreach (var file in Directory.EnumerateFiles(_workingDirectory, "*.csv"))
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
				var split = fileNameWithoutExtension.Split('-').ToArray();

				//Example: GeoLite2-City-Blocks-IPv6
				//Example: GeoLite2-City-Locations-pt-BR


				if (split[2].Equals("Blocks"))
				{
					ipAddressBlocks.Add(file);
					continue;
				}

				if (split[2].Equals("Locations"))
				{
					locationReaders.Add(new CsvLocationReader(file, 
						string.Join("-", split, 3, split.Length - 3)));
				}
			}

			BlocksReader = new CsvBlockReader(ipAddressBlocks.ToArray());
			LocationsReader = locationReaders.ToArray();
		}

		public void Dispose()
		{
			Directory.Delete(_workingDirectory, true);
			_workingDirectory = null;

			BlocksReader = null;
			LocationsReader = null;
		}
	}
}
