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

using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using DSharpPlus.VoiceNext;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Onyix
{
	public class Bot
	{
		private readonly ulong guild;
		private readonly DiscordClient client;
		private readonly SlashCommandsExtension commands;
		private readonly VoiceNextExtension voicenext;

		/// <summary>
		/// Create a new client
		/// </summary>
		public Bot()
		{
			// Get debug guild
			string? guildstr = Environment.GetEnvironmentVariable("GUILD");
			guild = guildstr is not null ? ulong.Parse(guildstr) : 0;

			// Create client
			client = new(new()
			{
				Token = Environment.GetEnvironmentVariable("TOKEN"),
				TokenType = TokenType.Bot,
				Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMembers,
				// FIXME: ILoggerFactory.AddNLog() is deprecated. Find a workaround.
				LoggerFactory = new LoggerFactory().AddNLog()
			});

			client.Ready += (client, e) =>
			{
				_ = Ready();
				return Task.CompletedTask;
			};

			client.MessageCreated += (client, e) =>
			{
				_ = MessageCreated(e);
				return Task.CompletedTask;
			};

			// Setup slash commands
			commands = client.UseSlashCommands();
			voicenext = client.UseVoiceNext();

			commands.SlashCommandErrored += (cmds, e) =>
			{
				Program.Logs.Error(e.Exception, e.ToString());
				return Task.CompletedTask;
			};

			//client.CurrentApplication.GetAssetsAsync().Wait();
		}

		/// <summary>
		/// Connect the client to Discord
		/// </summary>
		public async Task Start()
		{
			// Initialize
			Database.Initialize();
			RegisterModules();

			// Connect to Discord
			await client.ConnectAsync();
			await Task.Delay(-1);
		}

		/// <summary>
		/// Fired when the websocket is ready
		/// </summary>
		private async Task Ready()
		{
			Program.Logs.Info("Onyix is online");
			return;
		}

		/// <summary>
		/// Fired when a message is received
		/// </summary>
		/// <param name="param">Message details</param>
		private async Task MessageCreated(MessageCreateEventArgs e)
		{
			if (e.Author.IsBot) return;
			await Levels.GiveXPAsync(e);
		}

		/// <summary>
		/// Register all application command modules
		/// </summary>
		private async void RegisterModules()
		{
			// Register new modules
			commands.RegisterCommands(Assembly.GetExecutingAssembly(), guild != 0 ? guild : null);
			Program.Logs.Info("Registered assembly modules");
		}

		/// <summary>
		/// Returns the Discord socket client
		/// </summary>
		public DiscordClient Client => client;
	}
}
