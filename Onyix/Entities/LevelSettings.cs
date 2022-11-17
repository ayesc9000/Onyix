/* Onyix - An open-source Discord bot
 * Copyright (C) 2022 Liam "AyesC" Hogan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using LiteDB;
using System.Collections.Generic;

namespace Onyix.Entities
{
	public class LevelSettings
	{
		public ObjectId? Id { get; set; }
		public ulong GuildId { get; set; }
		public bool EnableLevels { get; set; }
		public bool EnableLevelUpMessage { get; set; }
		public double Multiplier { get; set; }
		public int XpPerMessage { get; set; }
		public int XpPerLevel { get; set; }
		public int Cooldown { get; set; }
		public string LevelUpTitle { get; set; }
		public string LevelUpContent { get; set; }
		public Dictionary<long, ulong> LevelRoles { get; set; }

		public LevelSettings()
		{
			Id = null;
			GuildId = 0;
			EnableLevels = false;
			EnableLevelUpMessage = true;
			XpPerMessage = 15;
			XpPerLevel = 270;
			Multiplier = 1;
			Cooldown = 60;
			LevelUpTitle = "Leveled Up!";
			LevelUpContent = "You have leveled up to level $$LVL!";
			LevelRoles = new();
		}

		public LevelSettings(ulong guild)
		{
			Id = null;
			GuildId = guild;
			EnableLevels = false;
			EnableLevelUpMessage = true;
			XpPerMessage = 15;
			XpPerLevel = 270;
			Multiplier = 1;
			Cooldown = 60;
			LevelUpTitle = "Leveled Up!";
			LevelUpContent = "You have leveled up to level $$LVL!";
			LevelRoles = new();
		}
	}
}
