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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Onyix.Services;

namespace Onyix;

/// <summary>
/// This is a context factory for design-time database work. It creates a
/// temporary Sqlite database and is mainly used to create migrations.
/// This code should not be used for any Onyix related functionality.
/// </summary>
public class DbContextFactory : IDesignTimeDbContextFactory<DatabaseService>
{
	/// <summary>
	/// Create a new DatabaseService context with an Sqlite database
	/// </summary>
	/// <param name="args">Command line arguments</param>
	/// <returns>A DatabaseService instance with an Sqlite database</returns>
	public DatabaseService CreateDbContext(string[] args)
	{
		var opts = new DbContextOptionsBuilder<DatabaseService>();
		opts.UseSqlite("migrations.db");

		return new DatabaseService(opts.Options);
	}
}
