using System;

namespace GeoTask.Application.Exceptions
{
	public class NotFoundException : ApplicationException
	{
		public NotFoundException(string name, object key)
			: base($"Entity \"{name}\" ({key}) was not found.")
		{

		}
	}
}