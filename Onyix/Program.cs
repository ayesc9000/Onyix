using NLog;
using NLog.Config;
using System.Reflection;
using System.Threading.Tasks;

namespace Onyix
{
	public partial class Program
	{
		private static readonly Logger logs = LogManager.GetCurrentClassLogger();
#if DEBUG
		private static readonly Configuration config = Configuration.Load(HardVars.DebugConfigPath);
#else
		private static readonly Configuration config = Configuration.Load(HardVars.ConfigPath);
#endif

		public static Task Main(string[] args)
		{
			LogManager.Configuration = new XmlLoggingConfiguration(HardVars.NLogConfigPath);
			Logs.Info("Onyix version " + Version);

			return new Client().Start();
		}

		public static Logger Logs
		{
			get => logs;
		}

		public static Configuration Config
		{
			get => config;
		}

		public static string Version
		{
			get => Assembly.GetEntryAssembly().GetName().Version.ToString();
		}
	}
}
