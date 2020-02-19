using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

namespace GeoTask.GeoUpdate
{
	public struct GeoLocation
	{
		public long GeoNameId;
		public string TimeZone;
		public int? MetroCode;
		public string CountryName;
		public string CountryIsoCode;
		public string ContinentName;
		public string ContinentCode;
		public string CityName;
		public bool IsInEuropeanUnion;

	}

	public class CsvLocationReader : IEnumerable<GeoLocation>, IDisposable
	{
		public string LanguageISO639 { get; }

		private readonly Stream _stream;

		public CsvLocationReader(Stream stream, string languageISO639)
		{
			_stream = stream;
			LanguageISO639 = languageISO639;
		}


		public IEnumerator<GeoLocation> GetEnumerator()
		{
			using var reader = new StreamReader(_stream);
			using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

			csv.Read();
			csv.ReadHeader();

			while (csv.Read())
			{
				if (!csv.TryGetField<long>("geoname_id", out var geoNameId))
					continue;

				if (!csv.TryGetField<string>("continent_code", out var continentCode))
					continentCode = null;

				if (!csv.TryGetField<string>("time_zone", out var timeZone))
					timeZone = null;

				if (!csv.TryGetField<string>("country_name", out var countryName))
					countryName = null;

				if (!csv.TryGetField<string>("country_iso_code", out var countryIso))
					countryIso = null;

				if (!csv.TryGetField<string>("continent_name", out var continentName))
					continentName = null;

				if (!csv.TryGetField<string>("city_name", out var cityName))
					cityName = null;

				if (!csv.TryGetField<bool>("is_in_european_union", out var isInEuropeanUnion))
					isInEuropeanUnion = false;

				int? metroCode = null;

				if (csv.TryGetField<int>("metro_code", out var metroCodeValue))
					metroCode = metroCodeValue;

				yield return new GeoLocation()
				{
					GeoNameId = geoNameId,
					CityName = cityName,
					ContinentCode = continentCode,
					ContinentName = continentName,
					CountryIsoCode = continentCode,
					CountryName = countryName,
					IsInEuropeanUnion = isInEuropeanUnion,
					MetroCode = metroCode,
					TimeZone = timeZone
				};
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Dispose()
		{
			_stream?.Dispose();
		}
	}
}