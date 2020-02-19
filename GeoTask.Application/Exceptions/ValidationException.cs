using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation.Results;

namespace GeoTask.Application.Exceptions
{
	public class ValidationException: ApplicationException
	{
		public ValidationException()
			: base("One or more validation failures have occurred.")
		{
			Failures = new Dictionary<string, string[]>();
		}

		public ValidationException(IEnumerable<ValidationFailure> failures)
			: this()
		{
			Failures = failures
				.GroupBy(i => i.PropertyName)
				.ToDictionary(
					i => i.Key, 
					j => j.Select(i => i.ErrorMessage).ToArray());
		}

		public IReadOnlyDictionary<string, string[]> Failures { get; }
	}
}
