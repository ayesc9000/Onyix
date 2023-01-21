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
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */

using System;

namespace Onyix
{
	public static class Env
	{
		/*
		 * These are environment variables that are loaded
		 * at runtime.
		 * 
		 * Any data that needs to be loaded from the command
		 * line should go in here.
		 */

		public static string? Token => Environment.GetEnvironmentVariable("TOKEN");
		public static string? Guild => Environment.GetEnvironmentVariable("GUILD");
		public static string? Database => Environment.GetEnvironmentVariable("DATABASE");
	}
}
