using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeoTask.Application.Repository;
using GeoTask.Common;
using MediatR;

namespace GeoTask.Application.Queries
{
	public class GetIpLocationQuery: IRequest<IpLocation>
	{
		public string Ip { get; set; }
		public string Language { get; set; }
	}
}
