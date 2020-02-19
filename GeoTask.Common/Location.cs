namespace GeoTask.Common
{
	public class Location
	{
		public long GeoNameId { get; set; }
		public string TimeZone { get; set; }
		public int? MetroCode { get; set; }
		public string CountryName { get; set; }
		public string CountryIsoCode { get; set; }
		public string ContinentName { get; set; }
		public string ContinentCode { get; set; }
		public string CityName { get; set; }
		public bool IsInEuropeanUnion { get; set; }
	}
}