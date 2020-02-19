using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoTask.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GeoTask.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LocationController: ControllerBase
	{
		private readonly IMediator _mediator;

		public LocationController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<ActionResult<IpLocation>> Show([FromQuery] GetIpLocationQuery query)
		{
			if (query.Language == null)
				query.Language = "en"; // by default

			return await _mediator.Send(query);
		}
	}
}
