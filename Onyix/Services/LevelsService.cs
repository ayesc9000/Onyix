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

using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.DependencyInjection;
using Onyix.Entities;
using System;
using System.Threading.Tasks;

namespace Onyix.Services;

public class LevelsService
{
	private readonly DatabaseService db;

	public LevelsService(IServiceProvider s)
	{
		db = s.GetRequiredService<DatabaseService>();
	}

	public async Task GiveXPAsync(MessageCreateEventArgs e)
	{
		// Get guild settings
		LevelSettings? settings = db.FindOne<LevelSettings>(s => s.GuildId == e.Guild.Id);

		if (settings is null)
		{
			settings = new LevelSettings(e.Guild.Id);
			db.Add(settings);
		}

		// Check if levels are enabled in this guild
		if (!settings.EnableLevels) return;

		// Get user level
		UserLevel? user = db.FindOne<UserLevel>(s => s.GuildId == e.Guild.Id);

		if (user is null)
		{
			user = new UserLevel(e.Author.Id, e.Guild.Id);
			db.Add(user);
		}

		// Check if we can give XP right now
		// TODO: Remember to uncomment before prod
		//if ((DateTime.Now - user.LastGain) >= TimeSpan.FromSeconds(settings.Cooldown) is not true) return;

		// Give XP
		user.XP += settings.XpPerMessage;
		user.TotalXP += settings.XpPerMessage;
		user.LastGain = DateTime.Now;

		// Check if the user can level up
		if (user.XP >= settings.XpPerLevel * GetMultiplier(user.Level, settings.Multiplier))
		{
			user.XP = 0;
			user.Level++;

			// Assign level roles
			if (settings.LevelRoles.TryGetValue(user.Level, out ulong value))
			{
				DiscordRole role = e.Guild.GetRole(value);
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
		db.SaveChanges();
	}

	public static double GetMultiplier(long level, double multipler)
	{
		return 1.0 + level * (multipler / 10);
	}

	public static string GetLevelProgress(UserLevel user, LevelSettings settings)
	{
		long current = user.XP;
		double total = settings.XpPerLevel * GetMultiplier(user.Level, settings.Multiplier);

		return current + "/" + total;
	}
}
