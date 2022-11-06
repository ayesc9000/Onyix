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
using System;

namespace Onyix.Entities
{
	public class UserLevel
	{
		public ObjectId? Id { get; set; }
		public ulong UserId { get; set; }
		public ulong GuildId { get; set; }
		public long XP { get; set; }
		public long TotalXP { get; set; }
		public long Level { get; set; }
		public DateTime LastGain { get; set; }

		public UserLevel()
		{
			Id = null;
			UserId = 0;
			GuildId = 0;
			XP = 0;
			TotalXP = 0;
			Level = 0;
			LastGain = DateTime.MinValue;
		}
	}
}
