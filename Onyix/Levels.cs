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

using Discord;
using Discord.Commands;
using Onyix.Entities;
using System;
using System.Threading.Tasks;

namespace Onyix
{
	public static class Levels
	{
		public static async Task GiveXPAsync(Client client, SocketCommandContext context)
		{
			// Get the user and server data
			UserLevel user = client.Database.GetUserLevel(context.Message.Author.Id, context.Guild.Id);
			LevelSettings settings = client.Database.GetLevelSettings(context.Guild.Id);

			// Check if we can give XP right now
			if (!((DateTime.Now - user.LastGain) >= TimeSpan.FromSeconds(settings.Cooldown)))
			{
				// TODO: Turn this back on layter :D
				//return;
			}
			
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
					// Send level up message
					EmbedBuilder embed = new()
					{
						Title = settings.LevelUpTitle,
						Description = settings.LevelUpMessage,
						Color = new Color(0x26C95A)
					};

					await context.Message.ReplyAsync("", false, embed.Build());
				}
			}

			// Commit data to database
			Program.Logs.Info("Updating levels");
			client.Database.StartTransaction();
			client.Database.SetUserLevel(user);
			client.Database.CommitTransaction();
		}

		public static double GetMultiplier(long level, double multipler)
		{
			return 1.0 + (level * (multipler / 10));
		}
	}
}
