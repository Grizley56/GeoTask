using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GeoTask.GeoUpdate
{
	public class LocalMd5Storage : IMD5StorageService
	{
		private readonly string _filePath;

		public LocalMd5Storage(string filePath)
		{
			_filePath = filePath;
		}

		public Task<string> FetchMd5()
		{
			if (File.Exists(_filePath))
				return File.ReadAllTextAsync(_filePath);

			return Task.FromResult((string)null);
		}

		public Task SaveMd5(string md5)
		{
			if (md5 == null)
			{
				File.Delete(_filePath);
				return Task.CompletedTask;
			}

			var md5Dir = Path.GetDirectoryName(_filePath);

			if (!Directory.Exists(md5Dir))
				Directory.CreateDirectory(md5Dir);

			return File.WriteAllTextAsync(_filePath, md5, Encoding.UTF8);
		}
	}
}