﻿/* Onyix - An open-source Discord bot
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

using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Onyix.Entities;
using System;
using System.Threading.Tasks;

namespace Onyix
{
	public static class Levels
	{
		public static async Task GiveXPAsync(MessageCreateEventArgs e, Database db)
		{
			// Get guild settings
			LevelSettings? settings = db.FindOne<LevelSettings>(s => s.GuildId == e.Guild.Id);
			settings ??= new LevelSettings(e.Guild.Id);

			// Check if levels are enabled in this server
			if (!settings.EnableLevels) return;

			// Get user level
			UserLevel? user = db.FindOne<UserLevel>(s => s.GuildId == e.Guild.Id);
			user ??= new UserLevel(e.Author.Id, e.Guild.Id);

			// Check if we can give XP right now
			//if ((DateTime.Now - user.LastGain) >= TimeSpan.FromSeconds(settings.Cooldown) is not true) return;

			// Give XP
			user.XP += settings.XpPerMessage;
			user.TotalXP += settings.XpPerMessage;
			user.LastGain = DateTime.Now;

			// Check if the user can level up
			if (user.XP >= (settings.XpPerLevel * GetMultiplier(user.Level, settings.Multiplier)))
			{
				user.XP = 0;
				user.Level++;

				// Assign level roles
				if (settings.LevelRoles.ContainsKey(user.Level))
				{
					DiscordRole role = e.Guild.GetRole(settings.LevelRoles[user.Level]);
					DiscordMember member = await e.Guild.GetMemberAsync(e.Author.Id);

					await member.GrantRoleAsync(role);
				}

				// Check if we can send level up message
				if (settings.EnableLevelUpMessage)
				{
					await e.Message.RespondAsync(new DiscordEmbedBuilder()
						.WithTitle(settings.LevelUpTitle)
						.WithDescription(settings.LevelUpContent)
						.WithColor(Colors.Green));
				}
			}

			// Commit data to database
			// TODO: Double check this
			db.Update(user);
			db.SaveChanges();
		}

		public static double GetMultiplier(long level, double multipler)
		{
			return 1.0 + (level * (multipler / 10));
		}

		public static string GetLevelProgress(UserLevel user, LevelSettings settings)
		{
			long current = user.XP;
			double total = settings.XpPerLevel * GetMultiplier(user.Level, settings.Multiplier);

			return current + "/" + total;
		}
	}
}
