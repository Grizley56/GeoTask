using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using GeoTask.GeoUpdate.Utils;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace GeoTask.GeoUpdate.Tests
{
	class IPAddressHelperTests
	{
		public static IEnumerable<string> CreateValidIp
		{
			get
			{
				yield return "127.0.0.1";
				yield return "192.168.1.1/27";
				yield return "1.1.1.1/0";
			}
		}

		public static IEnumerable<string> CreateInvalidIp
		{
			get
			{
				yield return "999.999.999.999";
				yield return "some-invalid-string";
				yield return "///";
				yield return "127.0.0.1/33";
				yield return "127.0.0.1/-5";
			}
		}

		[Test]
		[TestCaseSource(nameof(CreateValidIp))]
		public void ParseCIDR_Valid_ReturnsTrue_Test(string ipAddressValue)
		{
			Assert.True(IpAddressHelper.TryParseCIDR(ipAddressValue, out var _, out var __));
		}

		[Test]
		public void ParseCIDR_Valid_ReturnsIp_Test()
		{
			IPAddress ipAddress = IPAddress.Parse("101.101.101.1");
			byte mask = 5;

			IpAddressHelper.TryParseCIDR($"{ipAddress}/{mask}", out var ipParsed, out var maskParsed);

			Assert.AreEqual(ipAddress, ipParsed);
			Assert.AreEqual(maskParsed, mask);
		}

		[Test]
		[TestCaseSource(nameof(CreateInvalidIp))]
		public void ParseCIDR_Invalid_ReturnsFalse_Test(string ipAddressValue)
		{
			Assert.False(IpAddressHelper.TryParseCIDR(ipAddressValue, out var ip, out var mask));
		}


	}
}
