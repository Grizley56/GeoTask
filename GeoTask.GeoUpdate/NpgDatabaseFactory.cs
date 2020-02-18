using Npgsql;

namespace GeoTask.GeoUpdate
{
	internal class NpgDatabaseFactory: INpgDbFactory
	{
		private readonly string _npgSqlConnectionString;

		public NpgDatabaseFactory(string npgSqlConnectionString)
		{
			_npgSqlConnectionString = npgSqlConnectionString;
		}

		public NpgsqlConnection CreateConnection()
		{
			return new NpgsqlConnection(_npgSqlConnectionString);
		}
	}
}