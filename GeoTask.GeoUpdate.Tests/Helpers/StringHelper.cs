using System.IO;
using System.Text;

namespace GeoTask.GeoUpdate.Tests.Helpers
{
	public static class StringExtensions
	{
		public static Stream ToStream(this string contents)
		{
			byte[] byteArray = Encoding.UTF8.GetBytes(contents); 
			return new MemoryStream(byteArray);
		}
	}
}
