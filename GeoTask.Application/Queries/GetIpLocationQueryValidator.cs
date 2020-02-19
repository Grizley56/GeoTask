using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using FluentValidation;

namespace GeoTask.Application.Queries
{
	public class GetIpLocationQueryValidator: AbstractValidator<GetIpLocationQuery>
	{
		public GetIpLocationQueryValidator()
		{
			RuleFor(i => i.Ip).NotNull();
			RuleFor(i => i.Ip).Must(i =>
			{
				return IPAddress.TryParse(i, out var _);
			});
		}
	}
}
