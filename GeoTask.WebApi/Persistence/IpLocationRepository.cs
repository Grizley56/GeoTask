using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Dapper;
using GeoTask.Application.Queries;
using GeoTask.Application.Repository;
using GeoTask.Common;
using GeoTask.WebApi.CustomSqlParameters;

namespace GeoTask.WebApi.Persistence
{
	public class IpLocationRepository : IIpLocationRepository
	{
		private readonly IDbConnectionFactory _connectionFactory;

		public IpLocationRepository(IDbConnectionFactory connectionFactory)
		{
			_connectionFactory = connectionFactory;
		}

		public async Task<IpLocation> FindByIp(IPAddress ipAddress, string locale)
		{
			//TBD: stored func or not
			var result = await _connectionFactory.CreateConnection().QueryAsync<Ip, Location, IpLocation>(
				$"SELECT network, longitude, latitude, accuracy_radius, time_zone, metro_code, country_name, country_iso_code, " +
				$"continent_name, continent_code, city_name, is_in_european_union  FROM ip " +
				$"LEFT JOIN location ON ip.geoname_id = location.geoname_id " +
				$"WHERE ip.network >> @ip AND location.locale_code = @localeCode", (ip, location) =>
				{
					ip.Network = ipAddress;
					return new IpLocation(ip, location, locale);
				}, new
				{
					ip = new InetParameter(ipAddress),
					localeCode = locale
				}, splitOn: "time_zone");

			return result.FirstOrDefault();
		}
	}

}
