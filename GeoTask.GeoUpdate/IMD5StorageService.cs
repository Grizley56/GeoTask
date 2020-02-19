using System.Threading.Tasks;

namespace GeoTask.GeoUpdate
{
	public interface IMD5StorageService
	{
		Task<string> FetchMd5();
		Task SaveMd5(string md5);
	}
}