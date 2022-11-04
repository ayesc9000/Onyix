using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Onyix
{
	public class Interactions
	{
		public Dictionary<string, Func<SocketSlashCommand, Task>> Executers;

		/// <summary>
		/// Create a new command manager
		/// </summary>
		public Interactions()
		{
			Executers = new();
		}

		/// <summary>
		/// Load and build all slash commands
		/// </summary>
		/// <returns>An array of all command properties</returns>
		public ApplicationCommandProperties[] BuildCommands()
		{
			List<ICommand> interfaces = GetCommandInterfaces();
			List<ApplicationCommandProperties> props = new();

			// Build each command
			foreach (ICommand c in interfaces)
			{
				Logger.WriteLog(LogSeverity.Verbose, "CommandManager", $"Discovered command: {c.Name}");

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
				Executers.Add(c.Name, c.Execute);
			}

			// Return application command properties as an array
			return props.ToArray();
		}

		/// <summary>
		/// Get all the interfaces found within the currently executing assembly that
		/// are assignable from ICommand
		/// </summary>
		/// <returns>A list of all found interfaces</returns>
		private static List<ICommand> GetCommandInterfaces()
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
					if (command != null)
					{
						interfaces.Add(command);
					}
				}
			}

			// Return the found interfaces
			return interfaces;
		}
	}
}
