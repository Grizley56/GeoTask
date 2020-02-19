using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoTask.GeoUpdate
{
	public interface ICsvStorage : IDisposable
	{
		CsvBlockReader BlocksReader { get; }
		CsvLocationReader LocationsReader { get; }
	}

	public class CsvMemoryStorage : ICsvStorage
	{
		public CsvBlockReader BlocksReader { get; private set; }
		public CsvLocationReader LocationsReader { get; private set; }

		public CsvMemoryStorage(CsvBlockReader blockReader, CsvLocationReader locationReaders)
		{
			BlocksReader = blockReader ?? throw new ArgumentNullException(nameof(blockReader));
			LocationsReader = locationReaders ?? throw new ArgumentNullException(nameof(locationReaders));;
		}

		public void Dispose()
		{
			BlocksReader?.Dispose();
			LocationsReader?.Dispose();
		}

	}
}