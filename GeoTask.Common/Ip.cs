using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GeoTask.Common
{
	public class Ip
	{
		public IPAddress Network { get; set; }
		public string Longitude { get; set; }
		public string Latitude { get; set; }
		public int? AccuracyRadius { get; set; }
		public long GeoNameId { get; set; }
	}
}
