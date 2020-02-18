using Npgsql;

namespace GeoTask.GeoUpdate
{
	internal interface INpgDbFactory
	{
		NpgsqlConnection CreateConnection();
	}
}