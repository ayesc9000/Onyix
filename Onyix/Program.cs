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
			LogManager.Configuration = new XmlLoggingConfiguration(HardVars.NLogConfigPath);
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
