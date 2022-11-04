using Discord;
using Discord.Commands;
using Onyix.Models;
using System;
using System.Threading.Tasks;

namespace Onyix
{
	public static class Levels
	{
		public static async Task GiveXPAsync(SocketCommandContext context)
		{
			// Get the user and server data
			UserLevel? user = Database.GetUserLevel(context.Message.Author.Id, context.Guild.Id);
			LevelSettings? settings = Database.GetLevelSettings(context.Guild.Id);

			// Check if we can give XP right now
			if (!(DateTime.Now - user.LastGain >= TimeSpan.FromSeconds(settings.Cooldown)))
			{
				// TODO: Turn this back on layter :D
				//return;
			}
			
			// Give XP
			user.XP += settings.XpPerMessage;
			user.TotalXP += settings.XpPerMessage;
			user.LastGain = DateTime.Now;

			// Check if the user can level up
			if (user.XP >= (settings.XpPerLevel * GetMultiplier(user.Level, settings.Multiplier)))
			{
				user.XP = 0;
				user.Level++;

				// Check if we can send level up message
				if (!settings.EnableLevelUpMessage)
				{
					return;
				}
				
				// Send level up message
				EmbedBuilder embed = new()
				{
					Title = settings.LevelUpTitle,
					Description = settings.LevelUpMessage,
					Color = new Color(0x26C95A)
				}; 

				await context.Message.ReplyAsync("", false, embed.Build());
			}
		}

		public static double GetMultiplier(long level, double multipler)
		{
			return 1.0 + (level * multipler);
		}
	}
}
