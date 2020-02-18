using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Remotion.Linq.Clauses;

namespace GeoTask.WebApi
{
	public class Program
	{
		private static readonly IConfiguration Configuration;

		static Program()
		{
			Configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false)
				.AddEnvironmentVariables()
				.Build();
		}

		public static void Main(string[] args)
		{
			CreateWebHostBuilder(args).Build().Run();
		}

		private static void MigrateDatabase()
		{
			var cnx = new NpgsqlConnection(Configuration.GetConnectionString("PrimaryConnection"));
			
			var evolve = new Evolve.Evolve(cnx)
			{
				Locations = new[] { "db/migrations" },
				IsEraseDisabled = true
			};

			evolve.Migrate();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args)
		{
			//Create GeoDb manually
			//Then apply migration
			MigrateDatabase();

			return WebHost.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration((hostingContext, config) =>
			{
				config.AddConfiguration(Configuration);
			})
			.UseStartup<Startup>();
		}
	}
}
