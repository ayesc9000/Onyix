using Discord.WebSocket;
using System.Threading.Tasks;

namespace Onyix.Events
{
	public static class OnSlashCommand
	{
		/// <summary>
		/// Fired when one of the client's slash commands is executed
		/// </summary>
		/// <param name="command">Slash command data</param>
		/// <returns>Completed task</returns>
		public static async Task Event(SocketSlashCommand command)
		{
			// Get command executer
			var executer = Program.CommandManager.Executers[command.Data.Name];

			// Invoke executer if it is not null
			if (executer != null)
			{
				await executer.Invoke(command);
			}
		}
	}
}
