using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoTask.GeoUpdate.Utils
{
	public static class CommandLine
	{
		public static bool ContainsArgument(string[] args, string arg)
			=> args.Contains(arg, StringComparer.OrdinalIgnoreCase);

		public static bool TryGetArgumentValue(string[] args, string arg, out string value)
		{
			value = null;

			if (string.IsNullOrWhiteSpace(arg))
				throw new ArgumentException(nameof(arg));

			int pos = Array.FindLastIndex(args, i => string.Equals(i, arg, StringComparison.OrdinalIgnoreCase));

			if (pos == -1)
				return false;

			var possibleValuePos = pos + 1;

			if (possibleValuePos >= args.Length || args[possibleValuePos].FirstOrDefault() == '-')
				return false;

			value = args[possibleValuePos];
			return true;
		}
	}
}
