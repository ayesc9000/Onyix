using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Onyix
{
	/// <summary>
	/// Represents a slash command
	/// </summary>
	public interface ICommand
	{
		/// <summary>
		/// The name of the command
		/// </summary>
		public string Name
		{
			get;
		}

		/// <summary>
		/// The description of the command
		/// </summary>
		public string Description
		{
			get;
		}

		/// <summary>
		/// Should the command be usable in direct messages
		/// </summary>
		public bool UseInDMs
		{
			get;
		}

		/// <summary>
		/// The options for this command
		/// </summary>
		public List<SlashCommandOptionBuilder>? Options
		{
			get;
		}

		/// <summary>
		/// The executer method that is ran when the command is used
		/// </summary>
		public Task Execute(Client client, SocketSlashCommand command);
	}
}
