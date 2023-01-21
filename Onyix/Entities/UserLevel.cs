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
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using Microsoft.EntityFrameworkCore;
using System;

namespace Onyix.Entities
{
	[Index(nameof(UserId), nameof(GuildId), IsUnique = true)]
	public class UserLevel
	{
		public int Id { get; set; }

		public ulong UserId { get; set; }

		public ulong GuildId { get; set; }

		public long XP { get; set; }

		public long TotalXP { get; set; }

		public long Level { get; set; }

		public DateTime LastGain { get; set; }

		public UserLevel(ulong user, ulong guild)
		{
			UserId = user;
			GuildId = guild;
			XP = 0;
			TotalXP = 0;
			Level = 0;
			LastGain = DateTime.MinValue;
		}
	}
}
