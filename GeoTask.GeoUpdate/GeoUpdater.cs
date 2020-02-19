using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoTask.GeoUpdate
{
	public class GeoUpdater
	{
		private readonly IGeo2LiteHttpService _httpService;
		private readonly IGeoDbWriter _dbWriter;
		private readonly IGeo2LiteExtractor _extractor;
		private readonly IMD5StorageService _md5Storage;
		private readonly Action<string> _logger;

		public GeoUpdater(IGeo2LiteHttpService httpService, IGeoDbWriter dbWriter, IGeo2LiteExtractor extractor, IMD5StorageService md5Storage, Action<string> logger)
		{
			_httpService = httpService;
			_dbWriter = dbWriter;
			_extractor = extractor;
			_md5Storage = md5Storage;
			_logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="forceStart">Update database even if md5 was not changed from last update</param>
		/// <returns></returns>
		public async Task Start(bool forceStart = false)
		{
			_logger?.Invoke("Update started");

			string lastMd5;

			try
			{
				lastMd5 = await _httpService.FetchMd5();
			}
			catch (Exception ex)
			{
				_logger?.Invoke("MD5 fetch failed with exception. Details: " + ex.Message);
				return;
			}

			string currentMd5 = await _md5Storage.FetchMd5();

			if (currentMd5 != null)
			{
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

			Stream stream;

			try
			{
				_logger?.Invoke("Downloading started");

				stream = await _httpService.DownloadCsv();
			}
			catch (Exception ex)
			{
				_logger?.Invoke("Downloading failed with exception. Details: " + ex.Message);
				return;
			}

			_logger?.Invoke("Extracting files");

			using (var storage = _extractor.Extract(stream))
			{
				_logger?.Invoke("Extracting completed");

				try
				{
					await _dbWriter.Write(
						storage.BlocksReader,
						storage.LocationReaders.Select(i => ((IEnumerable<GeoLocation>)i, i.LanguageISO639)),
						_logger);
				}
				catch
				{
					return;
				}
			}

			await _md5Storage.SaveMd5(currentMd5);
		}
	}
}
