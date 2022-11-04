using System.ComponentModel.DataAnnotations;

namespace Onyix.Models
{
	public class LevelSettings
	{
		[Key]
		public ulong GuildId { get; set; }
		public bool EnableLevels { get; set; }
		public bool EnableLevelUpMessage { get; set; }
		public long XpPerMessage { get; set; }
		public long XpPerLevel { get; set; }
		public double Multiplier { get; set; }
		public long Cooldown { get; set; }
		public string? LevelUpTitle { get; set; }
		public string? LevelUpMessage { get; set; }

		public LevelSettings()
		{
			GuildId = 0;
			EnableLevels = false;
			EnableLevelUpMessage = true;
			XpPerMessage = 15;
			XpPerLevel = 270;
			Multiplier = 0.1;
			Cooldown = 60;
			LevelUpTitle = "Leveled Up!";
			LevelUpMessage = "You have leveled up to level $$LVL!";
		}
	}
}
