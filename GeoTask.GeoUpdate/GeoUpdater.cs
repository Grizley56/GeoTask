﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GeoTask.GeoUpdate
{
	internal class GeoUpdater
	{
		private readonly IGeo2LiteHttpService _httpService;
		private readonly IGeoDbWriter _dbWriter;
		private readonly IGeo2LiteExtractor _extractor;

		private readonly string _md5Path;

		private readonly Action<string> _logger;

		public GeoUpdater(IGeo2LiteHttpService httpService, IGeoDbWriter dbWriter, IGeo2LiteExtractor extractor, Action<string> logger)
		{
			_httpService = httpService;
			_dbWriter = dbWriter;
			_extractor = extractor;
			_logger = logger;

			_md5Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"GeoUpdateService", "lastDownload.md5");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="forceStart">Update database even if md5 was not changed from last update</param>
		/// <returns></returns>
		public async Task Start(bool forceStart = false)
		{
			_logger?.Invoke("Update started");

			string lastMd5 = await _httpService.FetchMd5();

			if (File.Exists(_md5Path))
			{
				string currentMd5 = await File.ReadAllTextAsync(_md5Path);

				_logger?.Invoke("Current MD5: " + currentMd5);

				_logger?.Invoke("Last MD5: " + lastMd5);

				if (lastMd5.Equals(currentMd5))
				{
					_logger?.Invoke("MD5 was not changed");

					if (!forceStart)
					{
						_logger?.Invoke("Operation completed");
						return;
					}
				}
			}

			var tempPath = Path.Combine(Path.GetTempPath(), "geo2lite.tar.gz");

			try
			{
				_logger?.Invoke("Downloading started");

				await _httpService.DownloadCsv(tempPath);
			}
			catch (Exception ex)
			{
				_logger?.Invoke("Downloading failed with exception. Details: " + ex.Message);
				return;
			}
			
			using (_extractor)
			{
				_logger?.Invoke("Extracting files");

				_extractor.Extract(tempPath);

				_logger?.Invoke("Extracting completed");

				try
				{
					await _dbWriter.Write(
						_extractor.BlocksReader,
						_extractor.LocationsReader.Select(i => ((IEnumerable<GeoLocation>) i, i.LanguageISO639)),
						_logger);
				}
				catch
				{
					return;
				}
			}

			var md5Dir = Path.GetDirectoryName(_md5Path);

			if (!Directory.Exists(md5Dir))
				Directory.CreateDirectory(md5Dir);

			File.WriteAllText(_md5Path, lastMd5, Encoding.UTF8);
		}
	}
}
