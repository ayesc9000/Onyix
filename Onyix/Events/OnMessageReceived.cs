using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Onyix.Events
{
	public static class OnMessageReceived
	{
		/// <summary>
		/// Fired when a message is received by the client
		/// </summary>
		/// <returns>Completed task</returns>
		public static async Task Event(SocketMessage param)
		{
			// Check if author is a bot
			if (param.Author.IsBot)
			{
				return;
			}

			// Get socket user message
			var message = param as SocketUserMessage;

			if (message == null)
			{
				return;
			}

			// Create context
			var context = new SocketCommandContext(Program.Client, message);

			// Check levels
			await Levels.GiveXPAsync(context);
		}
	}
}
