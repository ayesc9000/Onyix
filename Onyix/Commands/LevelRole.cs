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

using DSharpPlus.SlashCommands;
using Onyix.Entities;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace Onyix.Commands
{
	[SlashCommandGroup("levelrole", "List and modify the assigned role for each level", true)]
	public class LevelRole : ApplicationCommandModule
	{
		[SlashCommand("list", "Show a list of every level's assigned role", true)]
		public async Task List(InteractionContext ctx)
		{
			// Check if interaction occured in a guild
			if (ctx.Guild is null) return;

			// Get guild data
			LevelSettings settings = Database.GetLevelSettings(ctx.Guild.Id);

			// Reply
			await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("List")
					.WithDescription("This command is not finished. Try again later.")
					.WithColor(Colors.Gray));
		}

		[SlashCommand("assign", "Assign a role for a level", true)]
		public async Task Assign(InteractionContext ctx,
			[Option("level", "The level to modify")] long level,
			[Option("role", "The role to assign to this level")] DiscordRole role)
		{
			// Check if interaction occured in a guild
			if (ctx.Guild is null) return;

			// Get guild data
			LevelSettings settings = Database.GetLevelSettings(ctx.Guild.Id);

			// Reply
			await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Assign")
					.WithDescription("This command is not finished. Try again later.")
					.WithColor(Colors.Gray));
		}

		[SlashCommand("remove", "Remove the assigned role for a level", true)]
		public async Task Remove(InteractionContext ctx,
			[Option("level", "The level to modify")] long level)
		{
			// Check if interaction occured in a guild
			if (ctx.Guild is null) return;

			// Get guild data
			LevelSettings settings = Database.GetLevelSettings(ctx.Guild.Id);

			// Reply
			await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Remove")
					.WithDescription("This command is not finished. Try again later.")
					.WithColor(Colors.Gray));
		}
	}
}
