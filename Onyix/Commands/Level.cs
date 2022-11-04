using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyix.Commands
{
	// Due to database nonsense, this command is broken for now.

	/*public class Level : ICommand
	{
		public string Name
		{
			get => "level";
		}

		public string Description
		{
			get => "Displays your current level in this guild.";
		}

		public List<SlashCommandOptionBuilder>? Options
		{
			get => new()
			{
				new SlashCommandOptionBuilder()
				{
					Name = "user",
					Description = "The user's level to check",
					IsRequired = false,
					Type = ApplicationCommandOptionType.User
				}
			};
		}

		public bool UseInDMs
		{
			get => false;
		}
		
		public async Task Execute(SocketSlashCommand command)
		{
			// Check if a specific user was specified
			IUser target;

			if (command.Data.Options.Count > 0)
			{
				target = command.Data.Options.ElementAt(0).Value as IUser;
			}
			else
			{
				target = command.User;
			}

			// Get user information
			UserLevel? user = Database.GetUserLevel(target.Id, (ulong)command.GuildId);
			LevelSettings? settings = Database.GetLevelSettings((ulong)command.GuildId);

			EmbedBuilder embed = new()
			{
				Title = $"{target.Username}'s Level",
				Description = "",
				Fields = new List<EmbedFieldBuilder>()
				{
					new()
					{
						Name = "Level",
						Value = user.Level,
						IsInline = true
					},
					new()
					{
						Name = "Total XP",
						Value = user.TotalXP,
						IsInline = true
					},
					new()
					{
						Name = "Progress to next level",
						Value = $"{user.XP}/{settings.XpPerLevel * Levels.GetMultiplier(user.Level, settings.Multiplier)}",
						IsInline = false
					}
				},
				ThumbnailUrl = target.GetAvatarUrl(),
				Color = new Color(0x26C95A)
			};

			await command.RespondAsync(embed: embed.Build());
		}
	}*/
}
