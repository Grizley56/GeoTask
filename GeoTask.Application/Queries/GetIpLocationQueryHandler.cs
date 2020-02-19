using System.Net;
using System.Threading;
using System.Threading.Tasks;
using GeoTask.Application.Exceptions;
using GeoTask.Application.Repository;
using MediatR;

namespace GeoTask.Application.Queries
{
	public class GetIpLocationQueryHandler: IRequestHandler<GetIpLocationQuery, IpLocation>
	{
		private readonly IIpLocationRepository _ipLocationRepository;

		public GetIpLocationQueryHandler(IIpLocationRepository ipLocationRepository)
		{
			_ipLocationRepository = ipLocationRepository;
		}

		public async Task<IpLocation> Handle(GetIpLocationQuery request, CancellationToken cancellationToken)
		{
			var ipLocation = await _ipLocationRepository.FindByIp(IPAddress.Parse(request.Ip), request.Language)
				.ConfigureAwait(false);

			if (ipLocation == null)
				throw new NotFoundException(nameof(request.Ip), request.Ip);

			return ipLocation;
		}
	}
}