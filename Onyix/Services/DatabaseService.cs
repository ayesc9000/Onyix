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
using Onyix.Database;
using Onyix.Exceptions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Onyix.Services;

public class DatabaseService : DbContext
{
	public DatabaseService(DbContextOptions options) : base(options) { }

	public T? FindOne<T>(Expression<Func<T, bool>> exp) where T : class
	{
		// Get dbset and query expression
		DbSet<T> set = Set<T>();
		T? ent;

		try
		{
			ent = set.SingleOrDefault(exp);
		}
		catch (InvalidOperationException)
		{
			throw new DatabaseResultException("There is more than one row in this table with the same index value!");
		}

		return ent;
	}

	public DbSet<LevelSettings>? LevelSettings { get; set; }

	public DbSet<UserKarma>? UserKarma { get; set; }

	public DbSet<UserLevel>? UserLevel { get; set; }
}
