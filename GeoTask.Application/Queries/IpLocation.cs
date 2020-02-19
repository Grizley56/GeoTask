using GeoTask.Common;

namespace GeoTask.Application.Queries
{
	public class IpLocation
	{
		public string IpAddress { get; set; }
		public string Longitude { get; set; }
		public string Latitude { get; set; }
		public int? AccuracyRadius { get; set; }

		public string TimeZone { get; set; }
		public int? MetroCode { get; set; }
		public string CountryName { get; set; }
		public string CountryIsoCode { get; set; }
		public string ContinentName { get; set; }
		public string ContinentCode { get; set; }
		public string CityName { get; set; }
		public bool IsInEuropeanUnion { get; set; }

		public string Language { get; set; }

		public IpLocation()
		{
			
		}

		public IpLocation(Ip ip, Location location, string language)
		{
			IpAddress = ip.Network.ToString();
			Longitude = ip.Longitude;
			Latitude = ip.Latitude;
			AccuracyRadius = ip.AccuracyRadius;
			TimeZone = location.TimeZone;
			MetroCode = location.MetroCode;
			CountryName = location.CountryName;
			CountryIsoCode = location.CountryIsoCode;
			ContinentName = location.ContinentName;
			ContinentCode = location.ContinentCode;
			CityName = location.CityName;
			IsInEuropeanUnion = location.IsInEuropeanUnion;
			Language = language;
		}
	}
}