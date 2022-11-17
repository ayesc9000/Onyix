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
using DSharpPlus.SlashCommands;
using Onyix.Entities;
using System.Threading.Tasks;

namespace Onyix.Commands
{
	[SlashCommandGroup("levelmessage", "See and modify the level up message", true)]
	public class LevelMessage : ApplicationCommandModule
	{
		[SlashCommand("show", "Show the current level up message", true)]
		public async Task Show(InteractionContext ctx)
		{
			// Check if interaction occured in a guild
			if (ctx.Guild is null) return;

			// Get guild data
			LevelSettings settings = Database.GetLevelSettings(ctx.Guild.Id);

			// Reply
			await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
				.WithTitle("Level up message")
				.WithDescription("The level up message currently has the following values:")
				.WithColor(Colors.Gray)
				.AddField("Title", settings.LevelUpTitle)
				.AddField("Description", settings.LevelUpMessage), true);
		}

		[SlashCommand("title", "Change the level up message title", true)]
		public async Task Title(InteractionContext ctx,
			[Option("value", "The new message title")] string title)
		{
			// Check if interaction occured in a guild
			if (ctx.Guild is null) return;

			// Get guild data
			LevelSettings settings = Database.GetLevelSettings(ctx.Guild.Id);
			settings.LevelUpTitle = title;

			// Save changes
			Database.StartTransaction();
			Database.SetLevelSettings(settings);
			Database.CommitTransaction();

			// Reply
			await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
				.WithTitle("Success")
				.WithDescription("The changes were saved successfully.")
				.WithColor(Colors.Green)
				.AddField("New value", title), true);
		}

		[SlashCommand("description", "Change the level up message description", true)]
		public async Task Description(InteractionContext ctx,
			[Option("value", "The new message description")] string desc)
		{
			// Check if interaction occured in a guild
			if (ctx.Guild is null) return;

			// Get guild data
			LevelSettings settings = Database.GetLevelSettings(ctx.Guild.Id);
			settings.LevelUpMessage = desc;

			// Save changes
			Database.StartTransaction();
			Database.SetLevelSettings(settings);
			Database.CommitTransaction();

			// Reply
			await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
				.WithTitle("Success")
				.WithDescription("The changes were saved successfully.")
				.WithColor(Colors.Green)
				.AddField("New value", desc), true);
		}
	}
}
