using LiteDB;
using System;

namespace Onyix.Entities
{
	public class UserLevel
	{
		public ObjectId Id { get; set; }
		public ulong UserId { get; set; }
		public ulong GuildId { get; set; }
		public long XP { get; set; }
		public long TotalXP { get; set; }
		public long Level { get; set; }
		public DateTime LastGain { get; set; }

		public UserLevel()
		{
			UserId = 0;
			GuildId = 0;
			XP = 0;
			TotalXP = 0;
			Level = 0;
			LastGain = DateTime.MinValue;
		}
	}
}
