using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GeoTask.GeoUpdate
{
	internal class Geo2LiteHttpService: IGeo2LiteHttpService
	{
		private readonly string _licenseKey;

		public string Host { get; }

		public Geo2LiteHttpService(string licenseKey, string host = "download.maxmind.com")
		{
			_licenseKey = licenseKey;
			Host = host;
		}

		public async Task DownloadCsv(string path)
		{
			using var httpClient = new HttpClient();
			using var file = File.Create(path);
			using var response = await httpClient.GetAsync(BuildAddress("zip"));

			if (response.IsSuccessStatusCode)
				await response.Content.CopyToAsync(file);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
				throw new InvalidLicenseKeyException();

			response.EnsureSuccessStatusCode();
		}

		private string BuildAddress(string suffix)
		{
			return
				$"https://{Host}/app/geoip_download?edition_id=GeoLite2-City-CSV&license_key={_licenseKey}&suffix={suffix}";
		}

		public async Task<string> FetchMd5()
		{
			using var httpClient = new HttpClient();

			var response = await httpClient.GetAsync(BuildAddress("zip.md5"));
			
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadAsStringAsync();

			if (response.StatusCode == HttpStatusCode.Unauthorized)
				throw new InvalidLicenseKeyException();

			response.EnsureSuccessStatusCode();
			return null;
		}
	}
}
