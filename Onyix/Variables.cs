using System;

namespace Onyix
{
	public static class Variables
	{
		public static string? Token => Environment.GetEnvironmentVariable("TOKEN");
		public static string? Guild => Environment.GetEnvironmentVariable("GUILD");
		public static string? Database => Environment.GetEnvironmentVariable("DATABASE");
	}
}
