using Discord;
using Discord.WebSocket;
using Onyix.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onyix.Commands
{
	public class Level : ICommand
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
		
		public async Task Execute(Client client, SocketSlashCommand command)
		{
			// Check if a user was specified
			// TODO: Check if a null value case is actually possible here
			IUser target = command.Data.Options.Count switch
			{
				0 => command.User,
				_ => command.Data.Options.ElementAt(0).Value as IUser
			};

			// Check if target user is a bot
			if (target.IsBot)
			{
				EmbedBuilder error = new()
				{
					Title = $"Error",
					Description = "Bots do not have levels",
					Color = Color.Red
				};

				await command.RespondAsync(embed: error.Build());
				return;
			}

			// Get user information
			UserLevel user = client.Database.GetUserLevel(target.Id, (ulong)command.GuildId);
			LevelSettings settings = client.Database.GetLevelSettings((ulong)command.GuildId);

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
	}
}
