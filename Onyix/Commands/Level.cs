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
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.DependencyInjection;
using Onyix.Database;
using Onyix.Services;
using System;
using System.Threading.Tasks;

namespace Onyix.Commands;

/// <summary>
/// A command module for displaying the level of a member
/// </summary>
public class Level : ApplicationCommandModule
{
	private readonly DatabaseService database;

	/// <summary>
	/// Create a new instance of the command module
	/// </summary>
	/// <param name="s">The service provider</param>
	public Level(IServiceProvider s)
	{
		database = s.GetRequiredService<DatabaseService>();
	}

	/// <summary>
	/// Reply to an interaction to get a member's level
	/// </summary>
	/// <param name="ctx">The context for this interaction</param>
	/// <param name="target">Get the level for a specific user</param>
	/// <returns>An asynchronous task representing this reply</returns>
	[SlashCommand("level", "Displays your current level in this guild.", true)]
	public async Task Execute(InteractionContext ctx,
		[Option("user", "Get the level for a specific user")] DiscordUser? target = null)
	{
		// Verify user and guild
		if (ctx.Guild is null) return;
		target ??= ctx.Member;

		if (target.IsBot)
		{
			await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
				.WithTitle("Error")
				.WithDescription("Bots cannot participate in the level system.")
				.WithColor(Colors.Red), true);

			return;
		}

		// Get user and settings from database
		LevelSettings? settings = database.FindOne<LevelSettings>(s => s.GuildId == ctx.Guild.Id);
		UserLevel? user = database.FindOne<UserLevel>(s => s.GuildId == ctx.Guild.Id);

		if (settings is null)
		{
			settings = new LevelSettings()
			{
				GuildId = ctx.Guild.Id
			};

			database.Add(settings);
			database.SaveChanges();
		}

		if (user is null)
		{
			user = new UserLevel()
			{
				UserId = ctx.User.Id,
				GuildId = ctx.Guild.Id
			};

			database.Add(user);
			database.SaveChanges();
		}

		// Check if levels are enabled in this guild
		if (!settings.EnableLevels)
		{
			await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
				.WithTitle("Error")
				.WithDescription("Levels are not enabled in this server.")
				.WithColor(Colors.Red));

			return;
		}

		// Reply with embed
		await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
			.WithTitle($"{target.Username}'s Level")
			.WithColor(Colors.Grey)
			.WithThumbnail(target.AvatarUrl)
			.AddField("Level", user.Level.ToString(), true)
			.AddField("Total XP", user.TotalXP.ToString(), true)
			.AddField("Progress to next level", "0", false), true); // Levels.GetLevelProgress(user, settings)
	}
}
