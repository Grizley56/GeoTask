using System.Data.Common;
using Npgsql;

namespace GeoTask.WebApi.Persistence
{
	public interface IDbConnectionFactory
	{
		DbConnection CreateConnection();
	}

	public class NpgsqlConnectionFactory : IDbConnectionFactory
	{
		private readonly string _connectionString;

		public NpgsqlConnectionFactory(string connectionString)
		{
			_connectionString = connectionString;
		}

		public DbConnection CreateConnection()
		{
			return new NpgsqlConnection(_connectionString);
		}
	}
}