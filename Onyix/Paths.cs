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
