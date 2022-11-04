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
					Description = "Wether to lock or unlock the channel",
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
		
		public async Task Execute(DiscordSocketClient client, SocketSlashCommand command)
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
