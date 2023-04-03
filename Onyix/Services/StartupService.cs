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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Threading.Tasks;

namespace Onyix.Services;

/// <summary>
/// A service to manage starting other services
/// </summary>
public class StartupService
{
	private readonly DiscordClient client;
	private readonly DatabaseService database;
	private readonly Logger logger;

	/// <summary>
	/// Create a new instance of the startup service
	/// </summary>
	/// <param name="s"></param>
	public StartupService(IServiceProvider s)
	{
		client = s.GetRequiredService<DiscordClient>();
		database = s.GetRequiredService<DatabaseService>();
		logger = s.GetRequiredService<Logger>();
	}

	/// <summary>
	/// Start the DiscordClient and apply pending database migrations
	/// </summary>
	/// <returns></returns>
	public async Task StartAsync()
	{
		// Apply database migrations
		database.Database.Migrate();
		logger.Debug("Applied database migrations");

		// Connect client
		await client.ConnectAsync();
		logger.Info("Connected!");
	}
}
