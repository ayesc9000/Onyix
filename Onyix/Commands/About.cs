using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onyix.Commands
{
	public class AboutCommand : ICommand
	{
		public string Name
		{
			get => "about";
		}
		
		public string Description
		{
			get => "Displays information about the bot.";
		}
		
		public List<SlashCommandOptionBuilder>? Options
		{
			get => null;
		}
		
		public bool UseInDMs
		{
			get => true;
		}

		public async Task Execute(DiscordSocketClient client, SocketSlashCommand command)
		{
			EmbedBuilder embed = new()
			{
				Title = "About Onyix",
				Description = "Onyix is a Discord bot that provides a variety of commands and tools for users and admins.",
				ThumbnailUrl = client.CurrentUser.GetAvatarUrl(),
				Color = new Color(0x26C95A)
			};

			await command.RespondAsync(embed: embed.Build());
		}
	}
}
