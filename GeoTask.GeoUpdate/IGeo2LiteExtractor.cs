using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;

namespace GeoTask.GeoUpdate
{
	internal interface IGeo2LiteExtractor
	{
		ICsvStorage Extract(Stream stream);
	}

	internal class Geo2LiteZipExtractor : IGeo2LiteExtractor
	{
		public ICsvStorage Extract(Stream stream)
		{
			List<Stream> ipAddressBlocks = new List<Stream>(2);
			List<CsvLocationReader> locationReaders = new List<CsvLocationReader>(8);

			ZipFile zip = new ZipFile(stream);

			foreach (ZipEntry ze in zip)
			{
				if (ze.IsFile)
				{
					if (!ze.Name.EndsWith(".csv"))
						continue;

					string file = Path.GetFileName(ze.Name);
					Stream inputStream = zip.GetInputStream(ze);

					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
					var split = fileNameWithoutExtension.Split('-').ToArray();

					//Example: GeoLite2-City-Blocks-IPv6
					//Example: GeoLite2-City-Locations-pt-BR

					if (split[2].Equals("Blocks"))
					{
						ipAddressBlocks.Add(inputStream);
						continue;
					}

					if (split[2].Equals("Locations"))
					{
						locationReaders.Add(new CsvLocationReader(inputStream,
							string.Join("-", split, 3, split.Length - 3)));
					}

				}
			}

			return new CsvMemoryStorage( new CsvBlockReader(ipAddressBlocks.ToArray()), locationReaders.ToArray());
		}
	}
}
