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
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyix.Commands
{
	public class LockCommand : ICommand
	{
		public string Name
		{
			get => "lock";
		}

		public string Description
		{
			get => "Locks or unlocks a channel";
		}

		public List<SlashCommandOptionBuilder>? Options
		{
			get => new()
			{
				new SlashCommandOptionBuilder()
				{
					Name = "mode",
					Description = "Whether to lock or unlock the channel",
					IsRequired = true,
					Type = ApplicationCommandOptionType.Integer,
					Choices = new()
					{
						new()
						{
							Name = "Lock",
							Value = 1
						},
						new()
						{
							Name = "Unlock",
							Value = 2
						}
					}
				}
			};
		}

		public bool UseInDMs
		{
			get => false;
		}
		
		public async Task Execute(Client client, SocketSlashCommand command)
		{
			// Get command parameters
			long mode = (long)command.Data.Options.ElementAt(0).Value;
			SocketTextChannel? channel = command.Channel as SocketTextChannel;

			// Check if channel is null
			if (channel == null)
			{
				return;
			}
			
			// Lock or unlock the channel
			if (mode == 1)
			{
				// Lock
				await channel.AddPermissionOverwriteAsync(channel.Guild.EveryoneRole, new OverwritePermissions(sendMessages: PermValue.Deny));

				EmbedBuilder embed = new()
				{
					Title = "Locked",
					Description = "The channel has been locked.",
					Color = new Color(0x26C95A)
				};

				await command.RespondAsync(embed: embed.Build(), ephemeral: true);
			}
			else if (mode == 2)
			{
				// Unlock
				await channel.AddPermissionOverwriteAsync(channel.Guild.EveryoneRole, new OverwritePermissions(sendMessages: PermValue.Allow));

				EmbedBuilder embed = new()
				{
					Title = "Unlocked",
					Description = "The channel has been unlocked.",
					Color = new Color(0x26C95A)
				};

				await command.RespondAsync(embed: embed.Build(), ephemeral: true);
			}
		}
	}
}
