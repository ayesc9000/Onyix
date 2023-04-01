/* Onyix - An open-source Discord bot
 * Copyright (C) 2023 Liam "AyesC" Hogan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Onyix.Database
{
	/// <summary>
	/// Represents the level system settings for a guild
	/// </summary>
	[Index(nameof(GuildId), IsUnique = true)]
	public class LevelSettings
	{
		public int Id { get; set; }
		public ulong GuildId { get; set; } = 0;
		public bool EnableLevels { get; set; } = false;
		public bool EnableLevelUpMessage { get; set; } = true;
		public double Multiplier { get; set; } = 1;
		public int XpPerMessage { get; set; } = 15;
		public int XpPerLevel { get; set; } = 270;
		public int Cooldown { get; set; } = 60;
		public string LevelUpTitle { get; set; } = "Leveled Up!";
		public string LevelUpContent { get; set; } = "You have leveled up to level $$LVL!";

		[NotMapped]
		public Dictionary<long, ulong> LevelRoles { get; set; } = new();
	}
}
