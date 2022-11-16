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

using LiteDB;
using Onyix.Entities;
using System;

namespace Onyix
{
	public static class Database
	{
		private static long transcount = 0;
		private static LiteDatabase? database;
		private static ILiteCollection<LevelSettings>? levelsettings;
		private static ILiteCollection<UserKarma>? userkarma;
		private static ILiteCollection<UserLevel>? userlevel;

		/// <summary>
		/// Start the LiteDB database
		/// </summary>
		public static void Initialize()
		{
			database = new(Paths.Database);

			// Get collections
			levelsettings = database.GetCollection<LevelSettings>("levelsettings");
			userkarma = database.GetCollection<UserKarma>("userkarma");
			userlevel = database.GetCollection<UserLevel>("userlevel");

			// Configure indexes
			StartTransaction();

			levelsettings.DropIndex("*");
			levelsettings.EnsureIndex(x => x.GuildId, true);

			userkarma.DropIndex("*");
			userkarma.EnsureIndex(x => x.UserId, true);

			userlevel.DropIndex("*");
			userlevel.EnsureIndex(x => x.UserId, true);
			userlevel.EnsureIndex(x => x.GuildId, true);

			CommitTransaction();
		}

		/// <summary>
		/// Checks whether the database has been initialized and is online
		/// </summary>
		/// <exception cref="Exception"></exception>
		public static void VerifyOnline()
		{
			if (database is null
				|| levelsettings is null
				|| userkarma is null
				|| userlevel is null)
				throw new Exception("Database is not ready!");
		}

		/// <summary>
		/// Get the level settings for a guild
		/// </summary>
		/// <param name="guild">Guild ID</param>
		/// <returns>Level settings entity</returns>
		public static LevelSettings GetLevelSettings(ulong guild)
		{
			VerifyOnline();
			return levelsettings.FindOne(x => x.GuildId == guild) ?? new(guild);
		}

		/// <summary>
		/// Get the karma for a user
		/// </summary>
		/// <param name="user">User ID</param>
		/// <returns>User karma entity</returns>
		public static UserKarma GetUserKarma(ulong user)
		{
			VerifyOnline();
			return userkarma.FindOne(x => x.UserId == user) ?? new(user);
		}

		/// <summary>
		/// Get the level for a user in a guild
		/// </summary>
		/// <param name="guild">Guild ID</param>
		/// <param name="user">User ID</param>
		/// <returns>User level entity</returns>
		public static UserLevel GetUserLevel(ulong guild, ulong user)
		{
			VerifyOnline();
			return userlevel.FindOne(x => x.GuildId == guild && x.UserId == user) ?? new(user, guild);
		}

		/// <summary>
		/// Get the level settings for a guild
		/// </summary>
		/// <param name="entity">Level Settings entity</param>
		public static void SetLevelSettings(LevelSettings entity)
		{
			VerifyOnline();
			levelsettings.Upsert(entity);
		}

		/// <summary>
		/// Get the karma for a user
		/// </summary>
		/// <param name="entity">User Karma entity</param>
		public static void SetUserKarma(UserKarma entity)
		{
			VerifyOnline();
			userkarma.Upsert(entity);
		}

		/// <summary>
		/// Get the level for a user in a guild
		/// </summary>
		/// <param name="entity">User Level entity</param>
		public static void SetUserLevel(UserLevel entity)
		{
			VerifyOnline();
			userlevel.Upsert(entity);
		}

		/// <summary>
		/// Start a new transaction on the database
		/// </summary>
		/// <remarks>There can only be one active transaction per thread</remarks>
		/// <exception cref="Exception">Failed to start transaction</exception>
		public static void StartTransaction()
		{
			VerifyOnline();

			// Start transaction
			// TODO: Should we throw or return null in this state?
			if (!database.BeginTrans())
				throw new Exception("Failed to start database transaction");

			transcount++;
		}

		/// <summary>
		/// Commit an active database transaction
		/// </summary>
		public static void CommitTransaction()
		{
			VerifyOnline();

			// End transaction
			try
			{
				if (!database.Commit())
					throw new Exception("Commit returned false");

				Program.Logs.Info("Commited transaction {0}", transcount);
			}
			catch (Exception e)
			{
				database.Rollback();
				Program.Logs.Error(e, "Failed to commit database transaction {0}!", transcount);
			}
		}

		/// <summary>
		/// Cancel an active database transaction
		/// </summary>
		public static void CancelTransaction()
		{
			database.Rollback();
			Program.Logs.Info("Cancelled transaction {0}", transcount);
		}
	}
}
