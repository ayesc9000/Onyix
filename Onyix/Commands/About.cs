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
using System.Threading.Tasks;

namespace Onyix.Commands;

/// <summary>
/// A command module for displaying information about the bot
/// </summary>
public class AboutCommand : ApplicationCommandModule
{
	/// <summary>
	/// Reply to an interaction with the about command
	/// </summary>
	/// <param name="ctx">The context for this interaction</param>
	/// <returns>An asynchronous task representing this reply</returns>
	[SlashCommand("about", "Displays information about the bot.", true)]
	public static async Task About(InteractionContext ctx)
	{
		await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
			.WithTitle("About Onyix")
			.WithDescription("Onyix is a Discord bot that provides a variety of commands and tools for users and admins.")
			.WithColor(Colors.Grey)
			.WithThumbnail(ctx.Client.CurrentUser.AvatarUrl));
	}
}
