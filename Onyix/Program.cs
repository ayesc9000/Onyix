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

using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Onyix
{
	public partial class Program
	{
		private static readonly Logger logs = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Main entry point
		/// </summary>
		/// <param name="args">Command-line arguments</param>
		public static Task Main(string[] args)
		{
			LogManager.Configuration = new XmlLoggingConfiguration(Paths.NLog);
			Logs.Info("Onyix version " + Version);

#if DEBUG
			// Load environment variables from user secrets if we are in a debug build
			ConfigurationBuilder builder = new();
			builder.AddUserSecrets<Program>();
			IConfigurationRoot config = builder.Build();

			foreach (IConfigurationSection child in config.GetChildren())
			{
				Environment.SetEnvironmentVariable(child.Key, child.Value);
			}

			Logs.Info("Loaded environment varibles from user secrets");
#endif

			return new Client().Start();
		}

		/// <summary>
		/// Return the NLog manager
		/// </summary>
		public static Logger Logs
		{
			get => logs;
		}

		/// <summary>
		/// Return the version of the bot
		/// </summary>
		public static string Version
		{
			get => Assembly.GetEntryAssembly().GetName().Version.ToString();
		}
	}
}
