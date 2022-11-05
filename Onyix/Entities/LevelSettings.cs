﻿using LiteDB;

namespace Onyix.Entities
{
	public class LevelSettings
	{
		public ObjectId Id { get; set; }
		public ulong GuildId { get; set; }
		public bool EnableLevels { get; set; }
		public bool EnableLevelUpMessage { get; set; }
		public double Multiplier { get; set; }
		public int XpPerMessage { get; set; }
		public int XpPerLevel { get; set; }
		public int Cooldown { get; set; }
		public string LevelUpTitle { get; set; }
		public string LevelUpMessage { get; set; }

		public LevelSettings()
		{
			GuildId = 0;
			EnableLevels = false;
			EnableLevelUpMessage = true;
			XpPerMessage = 15;
			XpPerLevel = 270;
			Multiplier = 1;
			Cooldown = 60;
			LevelUpTitle = "Leveled Up!";
			LevelUpMessage = "You have leveled up to level $$LVL!";
		}
	}
}
