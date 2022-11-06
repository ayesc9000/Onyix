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
using System.Threading.Tasks;
using Discord.Commands;
using System;

namespace Onyix
{
	public class Client
	{
		private readonly DiscordSocketClient client;
		private readonly Interactions interactions;
		private readonly Database database;

		/// <summary>
		/// Create a new client
		/// </summary>
		public Client()
		{
			client = new();
			client.Log += Log;
			client.Ready += Ready;
			client.MessageReceived += MessageReceived;
			client.SlashCommandExecuted += SlashCommandExecuted;

			interactions = new();
			database = new();
		}

		/// <summary>
		/// Connect the client to Discord
		/// </summary>
		public async Task Start()
		{
			await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("TOKEN"));
			await client.StartAsync();
			await Task.Delay(-1);
		}

		/// <summary>
		/// Fired when the websocket is ready
		/// </summary>
		private async Task Ready()
		{
			// Push commands
			await interactions.PushCommands(client);
		}

		/// <summary>
		/// Fired when a message is received
		/// </summary>
		/// <param name="param">Message details</param>
		private async Task MessageReceived(SocketMessage param)
		{
			// Check if author is a bot
			if (param.Author.IsBot)
			{
				return;
			}

			// Get socket user message
			if (param is not SocketUserMessage message)
			{
				return;
			}

			// Create context
			var context = new SocketCommandContext(client, message);

			// Check levels
			await Levels.GiveXPAsync(this, context);
		}

		/// <summary>
		/// Fired when a slash command is used
		/// </summary>
		/// <param name="command">Slash command details</param>
		private async Task SlashCommandExecuted(SocketSlashCommand command)
		{
			try
			{
				// Run interaction
				await interactions.ExecuteCommand(this, command);
			}
			catch (Exception e)
			{
				// Reply with error message
				await command.RespondAsync("An internal error occured. Try again later.", ephemeral: true);
				Program.Logs.Error(e, "An unhandled exception occured while running a slash command!");
			}
		}

		/// <summary>
		/// Generates a log message
		/// </summary>
		/// <param name="message">Log details</param>
		private async Task Log(LogMessage message)
		{
			switch (message.Severity)
			{
				case LogSeverity.Verbose:
					Program.Logs.Debug(message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Debug:
					Program.Logs.Debug(message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Info:
					Program.Logs.Info(message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Warning:
					Program.Logs.Warn(message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Error:
					Program.Logs.Error(message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Critical:
					Program.Logs.Fatal(message.Exception, "{0}: {1}", message.Source, message.Message);
					break;
			}
		}

		/// <summary>
		/// Returns the Discord socket client
		/// </summary>
		public DiscordSocketClient DiscordClient
		{
			get => client;
		}

		/// <summary>
		/// Returns the active database
		/// </summary>
		public Database Database
		{
			get => database;
		}
	}
}
