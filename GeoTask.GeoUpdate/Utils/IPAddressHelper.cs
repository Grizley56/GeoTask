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
			mask = 32;

			var split = value.Split('/');

			if (split.Length > 0 && split.Length <= 2)
			{
				if (!IPAddress.TryParse(split[0], out var ipAddress))
					return false;

				address = ipAddress;

				if (split.Length == 2)
				{
					if (!byte.TryParse(split[1], out var ipMask) || ipMask > 32)
						return false;

					mask = ipMask;
				}
				
				return true;
			}

			return false;
		}
	}
}
