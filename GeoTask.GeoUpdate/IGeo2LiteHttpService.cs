using System.Threading.Tasks;

namespace GeoTask.GeoUpdate
{
	internal interface IGeo2LiteHttpService
	{
		Task DownloadCsv(string path);
		Task<string> FetchMd5();
	}
}