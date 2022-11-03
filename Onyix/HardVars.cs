namespace Onyix
{
	public static class HardVars
	{
		/*
		 * These are hard-coded variables, stored inside the
		 * program for various reasons.
		 * 
		 * Anything that may need to be changed on the sysadmin's
		 * end or may change overtime should not be stored here.
		 * If you need to add any new variables, add them to the
		 * main config file instead.
		 */

		public const string ConfigPath = "Data/Configuration.config";
		public const string DebugConfigPath = "Data/Debug.config";
		public const string NLogConfigPath = "Data/NLog.config";
		public const string DBPath = "Data/Database.db";
	}
}
