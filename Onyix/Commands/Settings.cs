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

/*using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Onyix.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyix.Commands
{
	[SlashCommandGroup("settings", "Modify the behaviour of Onyix", false)]
	public class Settings : ApplicationCommandModule
	{
		[SlashCommandGroup("levels", "Modify level system settings", true)]
		public class SettingsLevels : ApplicationCommandModule
		{
			[SlashCommand("enabled", "Enable or disable the level system", true)]
			public static async Task Enabled(InteractionContext ctx,
				[Option("enabled", "Whether the level system should be enabled or not")] bool enabled)
			{
				// Check if interaction occured in a guild
				if (ctx.Guild is null) return;

				// Set value
				Database.StartTransaction();
				LevelSettings settings = Database.GetLevelSettings(ctx.Guild.Id);
				settings.EnableLevels = enabled;

				// Save changes
				Database.SetLevelSettings(settings);
				Database.CommitTransaction();

				// Reply
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Success")
					.WithDescription("The changes were saved successfully.")
					.WithColor(Colors.Green)
					.AddField("Enabled", enabled.ToString(), true));
			}

			[SlashCommand("messageenabled", "Enable or disable the level up message", true)]
			public static async Task MessageEnabled(InteractionContext ctx,
				[Option("enabled", "Whether the level up message should be enabled or not")] bool enabled)
			{
				// Check if interaction occured in a guild
				if (ctx.Guild is null) return;

				// Set value
				Database.StartTransaction();
				LevelSettings settings = Database.GetLevelSettings(ctx.Guild.Id);
				settings.EnableLevelUpMessage = enabled;

				// Save changes
				Database.SetLevelSettings(settings);
				Database.CommitTransaction();

				// Reply
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Success")
					.WithDescription("The changes were saved successfully.")
					.WithColor(Colors.Green)
					.AddField("Enabled", enabled.ToString(), true));
			}

			[SlashCommand("roles", "Show a list of every level's assigned role", true)]
			public static async Task Roles(InteractionContext ctx,
				[Option("page", "The page number of the role list to display")] long page = 1)
			{
				// Check if interaction occured in a guild
				if (ctx.Guild is null) return;

				// Get guild data
				LevelSettings settings = Database.GetLevelSettings(ctx.Guild.Id);

				// Check the amount of items in the list
				if (settings.LevelRoles.Count == 0)
				{
					await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
						.WithTitle("Level roles")
						.WithDescription("There are no items to display.")
						.WithColor(Colors.Grey));

					return;
				}

				// Calculate page system
				const int itemcount = 10;
				int pages = (int)Math.Ceiling(settings.LevelRoles.Count / (double)itemcount);

				if (page > pages)
					page = 1;

				int start = (int)(itemcount * (page - 1));
				int end = (itemcount * page) > settings.LevelRoles.Count
					? settings.LevelRoles.Count
					: (int)(itemcount * page);

				// Generate embed
				DiscordEmbedBuilder embed = new DiscordEmbedBuilder()
					.WithTitle("Level roles")
					.WithDescription($"Page {page}/{pages}")
					.WithColor(Colors.Grey);

				// TODO: Add page system!
				for (int i = start; i < end; i++)
				{
					KeyValuePair<long, ulong> role = settings.LevelRoles.ElementAt(i);
					embed.AddField($"Level {role.Key}", $"<@&{role.Value}>", true);
				}

				// Reply
				await ctx.CreateResponseAsync(embed, true);
			}

			[SlashCommand("assignrole", "Assign a role for a level", true)]
			public static async Task AssignRole(InteractionContext ctx,
				[Option("level", "The level to modify")] long level,
				[Option("role", "The role to assign to this level")] DiscordRole role)
			{
				// Check if interaction occured in a guild
				if (ctx.Guild is null) return;

				// Assign role
				Database.StartTransaction();
				LevelSettings settings = Database.GetLevelSettings(ctx.Guild.Id);
				settings.LevelRoles.Add(level, role.Id);

				// Save changes
				Database.SetLevelSettings(settings);
				Database.CommitTransaction();

				// Reply
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Success")
					.WithDescription("The changes were saved successfully.")
					.WithColor(Colors.Green)
					.AddField("Level", level.ToString(), true)
					.AddField("Role", role.Name, true), true);
			}

			[SlashCommand("removerole", "Remove the assigned role for a level", true)]
			public static async Task RemoveRole(InteractionContext ctx,
				[Option("level", "The level to modify")] long level)
			{
				// Check if interaction occured in a guild
				if (ctx.Guild is null) return;

				// Get guild data
				Database.StartTransaction();
				LevelSettings settings = Database.GetLevelSettings(ctx.Guild.Id);

				// Check if level has assigned role
				if (!settings.LevelRoles.ContainsKey(level))
				{
					Database.CancelTransaction();

					await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
						.WithTitle("Error")
						.WithDescription("The specified level does not have any roles assigned to it.")
						.WithColor(Colors.Red));

					return;
				}

				// Remove role
				settings.LevelRoles.Remove(level);

				// Save changes
				Database.SetLevelSettings(settings);
				Database.CommitTransaction();

				// Reply
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Success")
					.WithDescription("The changes were saved successfully.")
					.WithColor(Colors.Green)
					.AddField("Level", level.ToString()), true);
			}

			[SlashCommand("leveluptitle", "Change the level up message title", true)]
			public static async Task LevelUpTitle(InteractionContext ctx,
				[Option("value", "The new message title")] string title)
			{
				// Check if interaction occured in a guild
				if (ctx.Guild is null) return;

				// Get guild data
				Database.StartTransaction();
				LevelSettings settings = Database.GetLevelSettings(ctx.Guild.Id);
				settings.LevelUpTitle = title;

				// Save changes
				Database.SetLevelSettings(settings);
				Database.CommitTransaction();

				// Reply
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Success")
					.WithDescription("The changes were saved successfully.")
					.WithColor(Colors.Green)
					.AddField("Title", title), true);
			}

			[SlashCommand("levelupcontent", "Change the level up message content", true)]
			public static async Task LevelUpContent(InteractionContext ctx,
				[Option("value", "The new message content")] string desc)
			{
				// Check if interaction occured in a guild
				if (ctx.Guild is null) return;

				// Get guild data
				Database.StartTransaction();
				LevelSettings settings = Database.GetLevelSettings(ctx.Guild.Id);
				settings.LevelUpContent = desc;

				// Save changes
				Database.SetLevelSettings(settings);
				Database.CommitTransaction();

				// Reply
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Success")
					.WithDescription("The changes were saved successfully.")
					.WithColor(Colors.Green)
					.AddField("Content", desc), true);
			}
		}
	}
}
*/