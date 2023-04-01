/* Onyix - An open-source Discord bot
 * Copyright (C) 2023 Liam "AyesC" Hogan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog;
using NLog.Extensions.Logging;
using Onyix.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus.SlashCommands.EventArgs;
using Microsoft.EntityFrameworkCore;

namespace Onyix;

/// <summary>
/// Core bot class
/// </summary>
public class Bot
{
	private readonly IConfigurationRoot config;
	private readonly ServiceProvider services;
	private readonly DiscordClient client;
	private readonly Logger logger;
	private readonly SlashCommandsExtension commands;

	/// <summary>
	/// Construct a new bot instance
	/// </summary>
	public Bot()
	{
		// Configuration
		config = new ConfigurationBuilder()
			.SetBasePath(Environment.CurrentDirectory)
			.AddEnvironmentVariables()
			.AddUserSecrets<Program>()
			.Build();

		// Logs
		LogManager.Configuration = new XmlLoggingConfiguration("NLog.config");
		logger = LogManager.GetCurrentClassLogger();

		// Client
		client = new(new()
		{
			Token = config["TOKEN"],
			TokenType = TokenType.Bot,
			Intents = DiscordIntents.AllUnprivileged | DiscordIntents.GuildMembers,
			LoggerFactory = LoggerFactory.Create(builder => builder.AddNLog())
		});

		//client.CurrentApplication.GetAssetsAsync().Wait();
		client.Ready += Ready;
		client.MessageCreated += MessageCreated;

		// Services
		services = new ServiceCollection()
			.AddSingleton(config)
			.AddSingleton(client)
			.AddSingleton(logger)
			.AddDbContext<DatabaseService>(c => c.UseMySql(config["DATABASE"], ServerVersion.AutoDetect(config["DATABASE"])))
			.AddSingleton<LogonService>()
			.AddSingleton<LevelsService>()
			.BuildServiceProvider();

		// Slash commands
		_ = uint.TryParse(config["GUILD"], out uint guild);

		commands = client.UseSlashCommands(new()
		{
			Services = services
		});

		commands.RegisterCommands(Assembly.GetExecutingAssembly(), guild != 0 ? guild : null);
		commands.SlashCommandErrored += SlashCommandErrored;
	}

	/// <summary>
	/// Start the bot
	/// </summary>
	public async Task StartAsync()
	{
		LogonService ls = services.GetRequiredService<LogonService>();
		await ls.StartAsync();
		await Task.Delay(-1);
	}

	/// <summary>
	/// Fired when the websocket is ready
	/// </summary>
	/// <param name="sender">Sender client</param>
	/// <param name="e">Event arguments</param>
	private Task Ready(DiscordClient sender, ReadyEventArgs e)
	{
		logger.Info("Startup complete");
		return Task.CompletedTask;
	}

	/// <summary>
	/// Fired when a message is received
	/// </summary>
	/// <param name="sender">Sender client</param>
	/// <param name="e">Event arguments</param>
	private async Task MessageCreated(DiscordClient sender, MessageCreateEventArgs e)
	{
		if (e.Author.IsBot) return;

		LevelsService ls = services.GetRequiredService<LevelsService>();
		await ls.GiveXPAsync(e);
	}

	/// <summary>
	/// Fired when a slash command encounters an error
	/// </summary>
	/// <param name="cmds">Slash commands extension</param>
	/// <param name="e">Event arguments</param>
	private Task SlashCommandErrored(SlashCommandsExtension cmds, SlashCommandErrorEventArgs e)
	{
		logger.Error(e.Exception, e.ToString());
		return Task.CompletedTask;
	}
}
