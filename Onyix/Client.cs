using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using NLog;
using System.Reflection;

namespace Onyix
{
	public class Client
	{
		private readonly DiscordSocketClient client;
		private readonly Dictionary<string, Func<SocketSlashCommand, Task>> executers;

		public Client()
		{
			client = new();
			executers = new();

			client.Log += Log;
		}

		public async Task Start()
		{
			//await client.LoginAsync(TokenType.Bot, "");
			//await client.StartAsync();
			await Task.Delay(-1);
		}

		public ApplicationCommandProperties[] BuildCommands()
		{
			List<ICommand> interfaces = GetCommandInterfaces();
			List<ApplicationCommandProperties> props = new();

			// Build each command
			foreach (ICommand c in interfaces)
			{
				Program.Logs.Debug($"Discovered command: {c.Name}");

				// Create the command builder
				SlashCommandBuilder builder = new()
				{
					Name = c.Name,
					Description = c.Description,
					IsDMEnabled = c.UseInDMs,
					Options = c.Options
				};

				// Add the built command to the properties list and executers dictionary
				props.Add(builder.Build());
				executers.Add(c.Name, c.Execute);
			}

			// Return application command properties as an array
			return props.ToArray();
		}

		private List<ICommand> GetCommandInterfaces()
		{
			// Get executing assembly
			Assembly asm = Assembly.GetExecutingAssembly();
			List<ICommand> interfaces = new();

			// Go through all types
			foreach (Type type in asm.GetTypes())
			{
				// Check if the type is assignable from ICommand
				if (typeof(ICommand).IsAssignableFrom(type) && !type.IsInterface)
				{
					// Create a new instance of the type if it is
					ICommand? command = Activator.CreateInstance(type) as ICommand;

					// Make sure it is not null
					if (command == null) continue;

					interfaces.Add(command);
				}
			}

			// Return the found interfaces
			return interfaces;
		}

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

		public DiscordSocketClient DiscordClient
		{
			get => client;
		}
	}
}
