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

using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Onyix.Entities;
using System;
using System.Threading.Tasks;

namespace Onyix
{
	public static class Levels
	{
		public static async Task GiveXPAsync(MessageCreateEventArgs e)
		{
			// Get the user and server data
			UserLevel user = Database.GetUserLevel(e.Author.Id, e.Guild.Id);
			LevelSettings settings = Database.GetLevelSettings(e.Guild.Id);

			// TODO: Debug this
			// Check if we can give XP right now
			if ((DateTime.Now - user.LastGain) >= TimeSpan.FromSeconds(settings.Cooldown) is not true) return;
			
			// Give XP
			user.XP += settings.XpPerMessage;
			user.TotalXP += settings.XpPerMessage;
			user.LastGain = DateTime.Now;

			// Check if the user can level up
			if (user.XP >= (settings.XpPerLevel * GetMultiplier(user.Level, settings.Multiplier)))
			{
				user.XP = 0;
				user.Level++;

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
			Database.StartTransaction();
			Database.SetUserLevel(user);
			Database.CommitTransaction();
		}

		public static double GetMultiplier(long level, double multipler)
		{
			return 1.0 + (level * (multipler / 10));
		}

		public static string GetLevelProgress(UserLevel user, LevelSettings settings)
		{
			long current = user.XP;
			double total = settings.XpPerLevel * Levels.GetMultiplier(user.Level, settings.Multiplier);

			return current + "/" + total;
		}
	}
}
