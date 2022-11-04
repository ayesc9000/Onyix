namespace Onyix
{
	public static class Paths
	{
		/*
		 * These are hard-coded paths, stored inside the program
		 * since they are used before any config data is accessed.
		 * 
		 * You should not store any data in here that could be
		 * otherwise stored in a config file or environment variable.
		 */

		public const string NLog = "Data/NLog.config";
		public const string Database = "Data/Database.db";
	}
}
