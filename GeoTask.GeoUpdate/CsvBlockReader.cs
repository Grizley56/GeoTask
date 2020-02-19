using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using GeoTask.GeoUpdate.Utils;

namespace GeoTask.GeoUpdate
{
	public struct GeoBlock
	{
		public IPAddress IpAddress;
		public int IpMask;

		public long GeoNameId;
		public string Longitude;
		public string Latitude;
		public int? AccuracyRadius;
	}

	public class CsvBlockReader : IEnumerable<GeoBlock>, IDisposable
	{
		private readonly Stream[] _streams;

		public CsvBlockReader(params Stream[] streams)
		{
			_streams = streams;
		}

		public IEnumerator<GeoBlock> GetEnumerator()
		{
			foreach (var stream in _streams)
			{
				if (stream.Length == 0)
					continue;

				using var reader = new StreamReader(stream);
				using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

				csv.Read();
				csv.ReadHeader();

				while (csv.Read())
				{
					if (!csv.TryGetField<string>("network", out var network))
						continue;

					if (!csv.TryGetField<long>("geoname_id", out var geoNameId))
						continue;

					if (!csv.TryGetField<string>("latitude", out var latitude))
						latitude = null;

					if (!csv.TryGetField<string>("longitude", out var longitude))
						longitude = null;

					int? accuracyRadius = null;

					if (csv.TryGetField<int>("accuracy_radius", out int accuracyRadiusValue))
						accuracyRadius = accuracyRadiusValue;


					if (IpAddressHelper.TryParseCIDR(network, out var ip, out var mask))
						yield return new GeoBlock()
						{
							IpAddress = ip,
							IpMask = mask,
							GeoNameId = geoNameId,
							Latitude = latitude,
							Longitude = longitude,
							AccuracyRadius = accuracyRadius
						};
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Dispose()
		{
			foreach (var stream in _streams)
			{
				stream.Dispose();
			}
		}
	}
}
