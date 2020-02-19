using System.Data;
using System.Net;
using Dapper;
using Npgsql;
using NpgsqlTypes;

namespace GeoTask.WebApi.CustomSqlParameters
{
	internal class InetParameter : SqlMapper.ICustomQueryParameter
	{
		private readonly IPAddress _value;

		public InetParameter(IPAddress value)
		{
			_value = value;
		}

		public void AddParameter(IDbCommand command, string name)
		{
			command.Parameters.Add(new NpgsqlParameter
			{
				ParameterName = name,
				NpgsqlDbType = NpgsqlDbType.Inet,
				Value = _value
			});
		}
	}
}