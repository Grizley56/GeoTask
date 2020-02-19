using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoTask.GeoUpdate
{
	public interface ICsvStorage : IDisposable
	{
		CsvBlockReader BlocksReader { get; }
		CsvLocationReader[] LocationReaders { get; }
	}

	public class CsvMemoryStorage : ICsvStorage
	{
		public CsvBlockReader BlocksReader { get; private set; }
		public CsvLocationReader[] LocationReaders { get; private set; }

		public CsvMemoryStorage(CsvBlockReader blockReader, IEnumerable<CsvLocationReader> locationReaders)
		{
			BlocksReader = blockReader ?? throw new ArgumentNullException(nameof(blockReader));
			LocationReaders = locationReaders?.ToArray() ?? throw new ArgumentNullException(nameof(locationReaders));;
		}

		public void Dispose()
		{
			BlocksReader?.Dispose();

			foreach (var csvLocationReader in LocationReaders)
			{
				csvLocationReader.Dispose();
			}
		}

	}
}