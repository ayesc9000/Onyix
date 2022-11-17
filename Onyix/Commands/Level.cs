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
	public class Level : ApplicationCommandModule
	{
		[SlashCommand("level", "Displays your current level in this guild.", true)]
		public static async Task Execute(InteractionContext ctx,
			[Option("user", "Get the level for a specific user")] DiscordUser? target = null)
		{
			// Check if interaction occured in a guild
			if (ctx.Guild is null) return;

			// Check if a specific user was provided
			target ??= ctx.Member;

			// Check if target user is a bot
			if (target.IsBot)
			{
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Error")
					.WithDescription("Bots cannot participate in the level system.")
					.WithColor(Colors.Red), true);

				return;
			}

			// Get user information
			UserLevel user = Database.GetUserLevel(target.Id, ctx.Guild.Id);
			LevelSettings settings = Database.GetLevelSettings(ctx.Guild.Id);

			// Reply with embed
			await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
				.WithTitle($"{target.Username}'s Level")
				.WithColor(Colors.Gray)
				.WithThumbnail(target.AvatarUrl)
				.AddField("Level", user.Level.ToString(), true)
				.AddField("Total XP", user.TotalXP.ToString(), true)
				.AddField("Progress to next level", Levels.GetLevelProgress(user, settings), false), true);
		}
	}
}
