using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GeoTask.Application.Queries;

namespace GeoTask.Application.Repository
{
	public interface IIpLocationRepository
	{
		Task<IpLocation> FindByIp(IPAddress ipAddress, string locale);
	}
}
