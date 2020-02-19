using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeoTask.GeoUpdate.Tests.Stubs
{
	class InMemoryMd5Storage: IMD5StorageService
	{
		private string _md5;

		public bool SaveMd5Called { get; private set; }

		public InMemoryMd5Storage()
		{
			
		}

		public InMemoryMd5Storage(string md5)
		{
			_md5 = md5;
		}

		public Task<string> FetchMd5()
		{
			return Task.FromResult(_md5);
		}

		public Task SaveMd5(string md5)
		{
			SaveMd5Called = true;

			_md5 = md5;
			return Task.CompletedTask;
		}
	}
}
