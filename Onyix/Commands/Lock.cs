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
/// A command module for locking and unlocking channels
/// </summary>
public class LockCommand : ApplicationCommandModule
{
	/// <summary>
	/// Reply to an interaction to lock the current channel
	/// </summary>
	/// <param name="ctx">The context for this interaction</param>
	/// <returns>An asynchronous task representing this reply</returns>
	[SlashCommand("lock", "Lock the current channel.", true)]
	public static async Task Lock(InteractionContext ctx)
	{
		// Check if interaction occured in a guild
		if (ctx.Guild is null) return;

		// Lock
		await ctx.Channel.AddOverwriteAsync(ctx.Guild.EveryoneRole, deny: DSharpPlus.Permissions.SendMessages);
		await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
			.WithTitle("Locked")
			.WithDescription("The channel has been locked.")
			.WithColor(Colors.Green), true);
	}

	/// <summary>
	/// Reply to an interaction to unlock the current channel
	/// </summary>
	/// <param name="ctx">The context for this interaction</param>
	/// <returns>An asynchronous task representing this reply</returns>
	[SlashCommand("unlock", "Unlock the current channel.", true)]
	public static async Task Unlock(InteractionContext ctx)
	{
		// Check if interaction occured in a guild
		if (ctx.Guild is null) return;

		// Unlock
		// TODO: Restore original permission for this rather than just set it to allow
		await ctx.Channel.AddOverwriteAsync(ctx.Guild.EveryoneRole, allow: DSharpPlus.Permissions.SendMessages);
		await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
			.WithTitle("Unlocked")
			.WithDescription("The channel has been unlocked.")
			.WithColor(Colors.Green), true);
	}
}
