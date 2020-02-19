using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using FluentValidation.AspNetCore;
using GeoTask.Application.Core;
using GeoTask.Application.Queries;
using GeoTask.Application.Repository;
using GeoTask.WebApi.Persistence;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace GeoTask.WebApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			DefaultTypeMap.MatchNamesWithUnderscores = true;

			services.AddTransient<IIpLocationRepository, IpLocationRepository>();
			services.AddTransient<IDbConnectionFactory, NpgsqlConnectionFactory>(i =>
				new NpgsqlConnectionFactory(Configuration.GetSection("ConnectionStrings")["PrimaryConnection"]));

			services.AddMediatR(typeof(GetIpLocationQueryHandler).GetTypeInfo().Assembly);
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

			services
				.AddMvc(options => { options.Filters.Add(typeof(CustomExceptionFilterAttribute)); })
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
				.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GetIpLocationQueryValidator>())
				.AddJsonOptions(options =>
				{
					options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				});
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();
		}
	}
}
