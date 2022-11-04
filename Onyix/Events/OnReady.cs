using Discord;
using System.Threading.Tasks;

namespace Onyix.Events
{
	public static class OnReady
	{
		/// <summary>
		/// Fired when the client is ready to begin working with Discord
		/// </summary>
		/// <returns>Completed task</returns>
		public static async Task Event()
		{
			// Load commands
			ApplicationCommandProperties[] props = Program.CommandManager.BuildCommands();
			var guild = Program.Client.GetGuild(Config.Data.Guild);

			// Write commands if guild is not null
			if (guild != null)
			{
				await guild.BulkOverwriteApplicationCommandAsync(props);
			}
		}
	}
}
