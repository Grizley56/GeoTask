using System.IO;
using System.Threading.Tasks;

namespace GeoTask.GeoUpdate
{
	public interface IGeo2LiteHttpService
	{
		/// <summary>
		/// Throws an InvalidLicenseKeyException if license key is invalid
		/// </summary>
		Task<Stream> DownloadCsv();

		/// <summary>
		/// Throws an InvalidLicenseKeyException if license key is invalid
		/// </summary>
		Task<string> FetchMd5();
	}
}