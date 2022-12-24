using System.Collections.Generic;

namespace Onyix.Queue
{
	public static class QueueManager
	{
		private static Dictionary<ulong, GuildQueue> queues = new();

		public static GuildQueue GetQueue(ulong id)
		{
			if (!queues.ContainsKey(id))
				queues[id] = new();

			return queues[id];
		}
	}
}
