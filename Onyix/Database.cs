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

using Microsoft.EntityFrameworkCore;
using Onyix.Entities;
using System;

namespace Onyix
{
	public class Database : DbContext
	{
		public DbSet<LevelSettings>? LevelSettings { get; set; }
		public DbSet<UserKarma>? UserKarma { get; set; }
		public DbSet<UserLevel>? UserLevel { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			string? connection = Variables.Database;

			if (connection is null)
				throw new Exception("Database connection string missing");

			options.UseMySql(ServerVersion.AutoDetect(connection));
		}
	}
}
