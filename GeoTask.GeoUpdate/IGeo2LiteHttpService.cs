using System.IO;
using System.Threading.Tasks;

namespace GeoTask.GeoUpdate
{
	internal interface IGeo2LiteHttpService
	{
		Task<Stream> DownloadCsv();
		Task<string> FetchMd5();
	}
}