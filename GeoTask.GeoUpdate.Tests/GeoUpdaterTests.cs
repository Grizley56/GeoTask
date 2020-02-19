using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using GeoTask.GeoUpdate.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace GeoTask.GeoUpdate.Tests
{
	class GeoUpdaterTests
	{
		[SetUp]
		public void Setup()
		{
			
		}


		[Test]
		public async Task If_MD5_NotChanged_Dont_Download_CSV_Test()
		{
			string md5 = "e7bbb1048c92d3bb9e5f0d5637cb7497";

			Mock<IGeo2LiteHttpService> httpServiceMock = new Mock<IGeo2LiteHttpService>();
			httpServiceMock.Setup(i => i.FetchMd5()).ReturnsAsync(md5);

			Mock<IGeoDbWriter> dbWriterMock = new Mock<IGeoDbWriter>();
			Mock<IGeo2LiteExtractor> geoExtractorMock = new Mock<IGeo2LiteExtractor>();


			GeoUpdater updater = new GeoUpdater(httpServiceMock.Object, dbWriterMock.Object, geoExtractorMock.Object, 
				new InMemoryMd5Storage(md5), null);

			await updater.Start(false);

			httpServiceMock.Verify(i => i.DownloadCsv(), Times.Never);
		}

		[Test]
		public async Task Save_MD5_After_Import_Test()
		{
			string md5 = "e7bbb1048c92d3bb9e5f0d5637cb7497";

			Mock<IGeo2LiteHttpService> httpServiceMock = new Mock<IGeo2LiteHttpService>();
			httpServiceMock.Setup(i => i.FetchMd5()).ReturnsAsync(md5);

			Mock<IGeoDbWriter> dbWriterMock = new Mock<IGeoDbWriter>();
			Mock<IGeo2LiteExtractor> geoExtractorMock = new Mock<IGeo2LiteExtractor>();
			geoExtractorMock.Setup(i => i.Extract(It.IsAny<Stream>())).Returns(() => new CsvMemoryStorage(
				new CsvBlockReader(Stream.Null), new CsvLocationReader[0]));


			var md5Storage = new InMemoryMd5Storage();

			GeoUpdater updater = new GeoUpdater(httpServiceMock.Object, dbWriterMock.Object, geoExtractorMock.Object,
				md5Storage, null);

			await updater.Start();

			Assert.True(md5Storage.SaveMd5Called);
		}


	}
}
