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
using System.Threading.Tasks;

namespace Onyix.Commands
{
	/// <summary>
	/// A command module for locking and unlocking channels
	/// </summary>
	public class LockCommand : ApplicationCommandModule
	{
		[SlashCommand("lock", "Locks or unlocks a channel.", true)]
		public async Task Execute(InteractionContext ctx,
			[Option("mode", "Whether to lock or unlock the channel")]
			[Choice("Lock", 1)]
			[Choice("Unlock", 2)] long mode)
		{
			// Check if interaction occured in a guild
			if (ctx.Guild is null) return;

			// Lock or unlock the channel
			if (mode is 1)
			{
				// Lock
				await ctx.Channel.AddOverwriteAsync(ctx.Guild.EveryoneRole, deny: DSharpPlus.Permissions.SendMessages);
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Locked")
					.WithDescription("The channel has been locked.")
					.WithColor(Colors.Green), true);
			}
			else
			{
				// Unlock
				// TODO: Restore original permission for this rather than just set it to allow
				await ctx.Channel.AddOverwriteAsync(ctx.Guild.EveryoneRole, allow: DSharpPlus.Permissions.SendMessages);
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Unlocked")
					.WithDescription("The channel has been unlocked.")
					.WithColor(Colors.Green), true);
			}
		}
	}
}
