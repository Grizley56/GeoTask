using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GeoTask.GeoUpdate.Utils
{
	public static class IpAddressHelper
	{
		public static bool TryParseCIDR(string value, out IPAddress address, out byte mask)
		{
			address = IPAddress.None;
			mask = 0;

			var split = value.Split('/');

			if (split.Length != 2 || !IPAddress.TryParse(split[0], out var ipAddress) ||! byte.TryParse(split[1], out var ipMask))
				return false;

			address = ipAddress;
			mask = ipMask;

			return true;
		}
	}
}
