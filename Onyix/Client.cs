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
			SocketUserMessage? message = param as SocketUserMessage;

			if (message == null)
			{
				return;
			}

			// Create context
			var context = new SocketCommandContext(client, message);

			// Check levels
			await Levels.GiveXPAsync(context);
		}

		/// <summary>
		/// Fired when a slash command is used
		/// </summary>
		/// <param name="command">Slash command details</param>
		private async Task SlashCommandExecuted(SocketSlashCommand command)
		{
			await interactions.ExecuteCommand(client, command);
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
					Program.Logs.Debug(exception: message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Debug:
					Program.Logs.Debug(exception: message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Info:
					Program.Logs.Info(exception: message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Warning:
					Program.Logs.Warn(exception: message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Error:
					Program.Logs.Error(exception: message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Critical:
					Program.Logs.Fatal(exception: message.Exception, "{0}: {1}", message.Source, message.Message);
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
	}
}
