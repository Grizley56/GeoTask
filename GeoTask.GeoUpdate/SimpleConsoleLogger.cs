using System;

namespace GeoTask.GeoUpdate
{
	internal class SimpleConsoleLogger
	{
		private readonly object _lock;

		public SimpleConsoleLogger()
		{
			_lock = new object();
		}

		public void Log(string log)
		{
			lock(_lock)
				Console.WriteLine(log);
		}
	}
}