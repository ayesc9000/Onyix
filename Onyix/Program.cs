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

using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Onyix;

public partial class Program
{
	/// <summary>
	/// Main entry point
	/// </summary>
	public static Task Main()
	{
		// Get version string
		// TODO: See if this can be simplified
		Assembly? asm = Assembly.GetEntryAssembly();

		if (asm is not null)
		{
			Version? ver = asm.GetName().Version;

			if (ver is not null)
				Version = ver.ToString();
		}

		// Configure NLog
		Logs = LogManager.GetCurrentClassLogger();
		LogManager.Configuration = new XmlLoggingConfiguration(Paths.NLog);

#if DEBUG
		// Load environment variables from user secrets if we are in a debug build
		ConfigurationBuilder builder = new();
		builder.AddUserSecrets<Program>();
		IConfigurationRoot config = builder.Build();

		foreach (IConfigurationSection child in config.GetChildren())
			Environment.SetEnvironmentVariable(child.Key, child.Value);

		Logs.Info("Loaded user secrets into environment variables");
#endif

		// Create bot and database
		Logs.Info("Onyix version " + Version);
		Bot = new();

		// Start bot
		return Bot.Start();
	}

	// TODO: Possible null references on getters below.

	/// <summary>
	/// Get the version string
	/// </summary>
	public static string Version { get; private set; }

	/// <summary>
	/// Get the bot instance
	/// </summary>
	public static Bot Bot { get; private set; }

	/// <summary>
	/// Get the NLog logger
	/// </summary>
	public static Logger Logs { get; private set; }
}
