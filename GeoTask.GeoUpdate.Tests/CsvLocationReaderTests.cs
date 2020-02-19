using System.IO;
using System.Linq;
using GeoTask.GeoUpdate.Tests.Helpers;
using NUnit.Framework;

namespace GeoTask.GeoUpdate.Tests
{
	public class CsvLocationReaderTests
	{
		private string _csv;

		[SetUp]
		public void Setup()
		{
			_csv =
				"geoname_id,locale_code,continent_code,continent_name,country_iso_code,country_name,subdivision_1_iso_code," +
				"subdivision_1_name,subdivision_2_iso_code,subdivision_2_name,city_name,metro_code,time_zone,is_in_european_union\n" +
				"75337,en,AS,Asia,YE,Yemen,HJ,Ḩajjah,,,Hajjah,,Asia/Aden,0\n" +
				"75427,en,AS,Asia,YE,Yemen,SU,Soqatra,,,Hadibu,,Asia/Aden,0\n" +
				"76184,en,AS,Asia,YE,Yemen,DH,Dhamār,,,Dhamar,,Asia/Aden,0";
		}

		[Test]
		public void EnumerateReader_Test()
		{
			CsvLocationReader reader = new CsvLocationReader(_csv.ToStream(), "en");

			Assert.That(reader, Has.Exactly(3).Items);
		}

		[Test]
		public void EnumerateEmptyStream_DoesNotThrow_Test()
		{
			CsvLocationReader reader = new CsvLocationReader(Stream.Null, "en");

			Assert.DoesNotThrow(() =>
			{
				using (var enumerator = reader.GetEnumerator())
					enumerator.MoveNext();
			});
		}

		[Test]
		public void EnumerateEmptyStream_IsEmpty_Test()
		{
			CsvLocationReader reader = new CsvLocationReader(Stream.Null, "en");

			Assert.IsEmpty(reader);
		}

		[Test]
		public void EnumerateReader_Returns_Valid_Value_Test()
		{
			GeoLocation geoLocation = new GeoLocation()
			{
				GeoNameId = 75337,
				ContinentCode = "AS",
				ContinentName = "Asia",
				CountryIsoCode = "YE",
				CountryName = "Yemen",
				CityName = "Hajjah",
				MetroCode = null,
				TimeZone = "Asia/Aden",
				IsInEuropeanUnion = false
			};

			var csv =
				"geoname_id,continent_code,continent_name,country_iso_code,country_name," +
				"city_name,metro_code,time_zone,is_in_european_union\n" +
				"75337,AS,Asia,YE,Yemen,Hajjah,,Asia/Aden,0\n";

			CsvLocationReader reader = new CsvLocationReader(csv.ToStream(), "en");

			var row = reader.First();

			Assert.Multiple(() =>
			{
				Assert.AreEqual(geoLocation.GeoNameId, row.GeoNameId);
				Assert.AreEqual(geoLocation.CityName, row.CityName);
				Assert.AreEqual(geoLocation.ContinentName, row.ContinentName);
				Assert.AreEqual(geoLocation.ContinentCode, row.ContinentCode);
				Assert.AreEqual(geoLocation.CountryIsoCode, row.CountryIsoCode);
				Assert.AreEqual(geoLocation.MetroCode, row.MetroCode);
				Assert.AreEqual(geoLocation.TimeZone, row.TimeZone);
				Assert.AreEqual(geoLocation.IsInEuropeanUnion, row.IsInEuropeanUnion);
			});
		}


	}
}