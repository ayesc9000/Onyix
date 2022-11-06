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
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Onyix
{
	public class Interactions
	{
		private readonly Dictionary<string, Func<Client, SocketSlashCommand, Task>> executers;

		/// <summary>
		/// Create a new command manager
		/// </summary>
		public Interactions()
		{
			executers = new();
		}

		/// <summary>
		/// Execute a slash command, if it exists
		/// </summary>
		/// <param name="command">Slash command object</param>
		public async Task ExecuteCommand(Client client, SocketSlashCommand command)
		{
			// Get command executer
			var executer = executers[command.Data.Name];

			// Invoke executer if it is not null
			if (executer != null)
			{
				await executer.Invoke(client, command);
			}
		}

		/// <summary>
		/// Push all slash commands to Discord.
		/// <para>When debugging, commands will be pushed to the specified guild.</para>
		/// <para>When releasing, commands will be pushed globally.</para>
		/// </summary>
		/// <param name="client">Discord client</param>
		/// <exception cref="Exception">Invalid or missing guild information</exception>
		public async Task PushCommands(DiscordSocketClient client)
		{
#if DEBUG
			// Parse guild id from environment variable
			ulong guildid;

			try
			{
				string? guildenv = Environment.GetEnvironmentVariable("GUILD");

				if (guildenv is null)
				{
					throw new Exception("No guild ID");
				}

				guildid = ulong.Parse(guildenv);
			}
			catch (Exception e)
			{
				throw new Exception("Invalid format for guild ID", e);
			}

			Program.Logs.Info($"Pushing commands to {guildid}");

			// Get guild and push commands
			SocketGuild guild = client.GetGuild(guildid);

			// Write commands if guild is not null
			if (guild != null)
			{
				ApplicationCommandProperties[] properties = BuildCommands();
				await guild.BulkOverwriteApplicationCommandAsync(properties);
			}
			else
			{
				throw new Exception("Provided guild does not exist");
			}
#else
			Program.Logs.Info("Pushing commands system-wide");
			// TODO: Implement system-wide command push
#endif
		}

		/// <summary>
		/// Builds an array of application command properties for each slash command class
		/// </summary>
		/// <returns>An array of ApplicationCommandProperties</returns>
		private ApplicationCommandProperties[] BuildCommands()
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

		/// <summary>
		/// Use reflection to find all classes implementing ICommand
		/// </summary>
		/// <returns>List of all applicable classes</returns>
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
					if (command == null) continue;

					interfaces.Add(command);
				}
			}

			// Return the found interfaces
			return interfaces;
		}
	}
}
