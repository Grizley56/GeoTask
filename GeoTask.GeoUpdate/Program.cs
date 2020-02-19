using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using GeoTask.GeoUpdate.Utils;

namespace GeoTask.GeoUpdate
{
	internal class Program
	{
		private static async Task Main(string[] args)
		{
			SimpleConsoleLogger logger = new SimpleConsoleLogger();

			bool forceRun = CommandLine.ContainsArgument(args, "-force");

			if (!CommandLine.TryGetArgumentValue(args, "-licenseKey", out var licenseKey))
			{
				logger?.Log("LicenseKey not (-licenseKey) defined.");
				return;
			}

			if (!CommandLine.TryGetArgumentValue(args, "-db", out var connectionString))
			{
				logger?.Log("Connection string (-db) not defined.");
				return;
			}


			GeoUpdater updater = new GeoUpdater(
				new Geo2LiteHttpService(licenseKey),
				new GeoDbWriter(new NpgDatabaseFactory(connectionString.Trim('"'))),
				new Geo2LiteZipExtractor(), 
				new LocalMd5Storage(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
					"GeoUpdateService", "lastDownload.md5")), 
				logger.Log);

			await updater.Start(forceRun);
		}
	}
}
