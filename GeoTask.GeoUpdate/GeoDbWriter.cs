﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using GeoTask.GeoUpdate.Utils;
using Npgsql;
using NpgsqlTypes;

namespace GeoTask.GeoUpdate
{
	public interface IGeoDbWriter
	{
		Task Write(
			IEnumerable<GeoBlock> blocks,
			IEnumerable<GeoLocation> locations, 
			Action<string> logger);
	}

	internal class GeoDbWriter: IGeoDbWriter
	{
		private readonly INpgDbFactory _npgDbFactory;

		public GeoDbWriter(INpgDbFactory npgDbFactory)
		{
			_npgDbFactory = npgDbFactory;
		}

		public async Task Write(
			IEnumerable<GeoBlock> blocks, 
			IEnumerable<GeoLocation> locations,
			Action<string> logger)
		{
			logger?.Invoke("Import started.");

			Stopwatch sw = Stopwatch.StartNew();

			string[] tableNames = new[] {"ip", "location"};

			using (var connection = _npgDbFactory.CreateConnection())
			{
				await connection.OpenAsync()
					.ConfigureAwait(false);

				foreach (var tableName in tableNames)
				{
					NpgsqlCommand command = CreateTempTableCommand($"__{tableName}", tableName);
					command.Connection = connection;
					await command.ExecuteNonQueryAsync()
						.ConfigureAwait(false);
				}
			}

			var ipTask = Task.Run(() => InsertIpBlocks(blocks, logger));
			var locationsTask = InsertLocations(locations, logger);

			try
			{
				await Task.WhenAll(ipTask, locationsTask);
			}
			catch
			{
				logger?.Invoke("Import failed. Rollback");
				return;
			}

			using (var connection = _npgDbFactory.CreateConnection())
			{
				await connection.OpenAsync()
					.ConfigureAwait(false);

				using var transaction = await connection.BeginTransactionAsync()
					.ConfigureAwait(false);

				foreach (var tableName in tableNames)
				{
					NpgsqlCommand command = SwitchTableCommand($"__{tableName}", tableName);
					command.Transaction = transaction;
					command.Connection = connection;

					await command.ExecuteNonQueryAsync()
						.ConfigureAwait(false);
				}
	
				await transaction.CommitAsync()
					.ConfigureAwait(false);
			}

			logger?.Invoke("Import completed. Total time: " + sw.Elapsed);
		}

		private async Task InsertIpBlocks(IEnumerable<GeoBlock> blocks, Action<string> logger)
		{
			Stopwatch sw = Stopwatch.StartNew();

			using var importConnection = _npgDbFactory.CreateConnection();

			try
			{
				await importConnection.OpenAsync().ConfigureAwait(false);

				using var import = importConnection.BeginBinaryImport(
					"COPY __ip (network, geoname_id, latitude, longitude, accuracy_radius) FROM STDIN (FORMAT BINARY)");

				foreach (var ipBlock in blocks)
				{
					await import.StartRowAsync().ConfigureAwait(false);

					await import.WriteAsync(new ValueTuple<IPAddress, int>(ipBlock.IpAddress, ipBlock.IpMask), NpgsqlDbType.Inet)
						.ConfigureAwait(false);
					await import.WriteAsync(ipBlock.GeoNameId, NpgsqlDbType.Bigint)
						.ConfigureAwait(false);
					await import.WriteAsync(ipBlock.Latitude, NpgsqlDbType.Varchar)
						.ConfigureAwait(false);
					await import.WriteAsync(ipBlock.Longitude, NpgsqlDbType.Varchar)
						.ConfigureAwait(false);
					await import.WriteAsync((object) ipBlock.AccuracyRadius ?? DBNull.Value, NpgsqlDbType.Integer)
						.ConfigureAwait(false);
				}

				await import.CompleteAsync()
					.ConfigureAwait(false);
			}
			catch(Exception ex)
			{
				logger?.Invoke("Ip-address importing failed. Detailed: " + ex.Message);
				throw;
			}

			logger?.Invoke("Ip-addresses imported successfully. Total time: " + sw.Elapsed);
		}

		private async Task InsertLocations(IEnumerable<GeoLocation> locations, Action<string> logger)
		{
			Stopwatch sw = Stopwatch.StartNew();

			try
			{
				using var connection = _npgDbFactory.CreateConnection();

				await connection.OpenAsync()
					.ConfigureAwait(false);

				using NpgsqlBinaryImporter import =
					connection.BeginBinaryImport($"COPY __location " +
					                             $"(geoname_id, time_zone, metro_code, country_name, country_iso_code, " +
					                             $"continent_name, continent_code, city_name, is_in_european_union, locale_code) FROM STDIN (FORMAT BINARY)");

				foreach (var location in locations)
				{
					await import.StartRowAsync()
						.ConfigureAwait(false);

					await import.WriteAsync(location.GeoNameId, NpgsqlDbType.Bigint)
						.ConfigureAwait(false);
					await import.WriteAsync(location.TimeZone, NpgsqlDbType.Varchar)
						.ConfigureAwait(false);
					await import.WriteAsync((object) location.MetroCode ?? DBNull.Value, NpgsqlDbType.Integer)
						.ConfigureAwait(false);
					await import.WriteAsync(location.CountryName, NpgsqlDbType.Varchar)
						.ConfigureAwait(false);
					await import.WriteAsync(location.CountryIsoCode, NpgsqlDbType.Char)
						.ConfigureAwait(false);
					await import.WriteAsync(location.ContinentName, NpgsqlDbType.Varchar)
						.ConfigureAwait(false);
					await import.WriteAsync(location.ContinentCode, NpgsqlDbType.Char)
						.ConfigureAwait(false);
					await import.WriteAsync(location.CityName, NpgsqlDbType.Varchar)
						.ConfigureAwait(false);
					await import.WriteAsync(location.IsInEuropeanUnion, NpgsqlDbType.Boolean)
						.ConfigureAwait(false);
					await import.WriteAsync(location.LocaleCode, NpgsqlDbType.Varchar)
						.ConfigureAwait(false);
				}

				await import.CompleteAsync()
					.ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				logger?.Invoke($"Locations importing failed. Detailed: {ex.Message}");

				throw;
			}

			logger?.Invoke($"Locations imported successfully. Total time: {sw.Elapsed}");
		}

		private NpgsqlCommand CreateTempTableCommand(string tableName, string asTable)
		{
			return new NpgsqlCommand($"CREATE TABLE IF NOT EXISTS \"{tableName}\" " +
			                         $"(LIKE \"{asTable}\" INCLUDING ALL); TRUNCATE TABLE \"{tableName}\"");
		}

		private NpgsqlCommand SwitchTableCommand(string fromTable, string toTable)
		{
			return new NpgsqlCommand(
				$"ALTER TABLE \"{toTable}\" RENAME TO \"{toTable}_old\"; " + 
				$"ALTER TABLE \"{fromTable}\" RENAME TO \"{toTable}\"; DROP TABLE \"{toTable}_old\"");
		}
	}


}
