using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeoTask.GeoUpdate.Utils
{
	internal static  class TaskHelper
	{
		public static async Task<Task[]> RunWithLimitCount(IEnumerable<Task> items, int maxCount)
		{
			List<Task> allTasks = new List<Task>();
			List<Task> currentTasks = new List<Task>(maxCount);

			foreach (var task in items)
			{
				if (currentTasks.Count == maxCount)
				{
					var completed = await Task.WhenAny(currentTasks).ConfigureAwait(false);
					await completed; // if there is an exception, throw it

					currentTasks.Remove(completed);
					allTasks.Add(completed);
				}

				currentTasks.Add(task);
			}

			await Task.WhenAll(currentTasks).ConfigureAwait(false);
			allTasks.AddRange(currentTasks);
			return allTasks.ToArray();
		}
	}
}
