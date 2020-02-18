using System;

namespace GeoTask.GeoUpdate
{
	public class InvalidLicenseKeyException : ApplicationException
	{
		public InvalidLicenseKeyException()
			:this("Invalid license key. Unauthorized")
		{
		}

		public InvalidLicenseKeyException(string message) : base(message)
		{
		}

		public InvalidLicenseKeyException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}