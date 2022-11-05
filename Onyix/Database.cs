using LiteDB;
using Onyix.Entities;
using System;

namespace Onyix
{
	public class Database
	{
		private long transcount;
		private readonly LiteDatabase database;
		private readonly ILiteCollection<LevelSettings> levelsettings;
		private readonly ILiteCollection<UserKarma> userkarma;
		private readonly ILiteCollection<UserLevel> userlevel;

		/// <summary>
		/// Create a new LiteDB instance
		/// </summary>
		public Database()
		{
			transcount = 0;
			database = new(Paths.Database);

			// Get collections
			levelsettings = database.GetCollection<LevelSettings>("levelsettings");
			userkarma = database.GetCollection<UserKarma>("userkarma");
			userlevel = database.GetCollection<UserLevel>("userlevel");

			// Configure indexes
			StartTransaction();
			levelsettings.EnsureIndex(x => x.GuildId, true);
			userkarma.EnsureIndex(x => x.UserId, true);
			userlevel.EnsureIndex(x => x.UserId, true);
			userlevel.EnsureIndex(x => x.GuildId, true);
			CommitTransaction();
		}

		/// <summary>
		/// Get the level settings for a guild
		/// </summary>
		/// <param name="guild">Guild ID</param>
		/// <returns>Level settings entity</returns>
		public LevelSettings GetLevelSettings(ulong guild)
		{
			// Find entity
			LevelSettings entity = levelsettings.FindOne(x => x.GuildId == guild);

			// Create entity if it does not exist
			if (entity == null)
			{
				entity = new()
				{
					GuildId = guild
				};
			}

			return entity;
		}

		/// <summary>
		/// Get the karma for a user
		/// </summary>
		/// <param name="user">User ID</param>
		/// <returns>User karma entity</returns>
		public UserKarma GetUserKarma(ulong user)
		{
			// Find entity
			UserKarma entity = userkarma.FindOne(x => x.UserId == user);

			// Create entity if it does not exist
			if (entity == null)
			{
				entity = new()
				{
					UserId = user
				};
			}

			return entity;
		}

		/// <summary>
		/// Get the level for a user in a guild
		/// </summary>
		/// <param name="guild">Guild ID</param>
		/// <param name="user">User ID</param>
		/// <returns>User level entity</returns>
		public UserLevel GetUserLevel(ulong guild, ulong user)
		{
			// Find entity
			UserLevel entity = userlevel.FindOne(x => x.GuildId == guild && x.UserId == user);

			// Create entity if it does not exist
			if (entity == null)
			{
				entity = new()
				{
					GuildId = guild,
					UserId = user
				};
			}

			return entity;
		}

		/// <summary>
		/// Get the level settings for a guild
		/// </summary>
		/// <param name="entity">Level Settings entity</param>
		public void SetLevelSettings(LevelSettings entity)
		{
			levelsettings.Upsert(entity);
		}

		/// <summary>
		/// Get the karma for a user
		/// </summary>
		/// <param name="entity">User Karma entity</param>
		public void SetUserKarma(UserKarma entity)
		{
			userkarma.Upsert(entity);
		}

		/// <summary>
		/// Get the level for a user in a guild
		/// </summary>
		/// <param name="entity">User Level entity</param>
		public void SetUserLevel(UserLevel entity)
		{
			userlevel.Upsert(entity);
		}

		/// <summary>
		/// Start a new transaction on the database
		/// </summary>
		/// <remarks>There can only be one active transaction per thread</remarks>
		/// <exception cref="Exception">Failed to start transaction</exception>
		public void StartTransaction()
		{
			// Start transaction
			if (!database.BeginTrans())
			{
				// TODO: Not sure whether to throw exception or return null in this state.
				// An exception will do for now. Fix this later.
				throw new Exception("Failed to start database transaction");
			}

			transcount++;
			Program.Logs.Info("Beginning transaction {0}", transcount);
		}

		/// <summary>
		/// Commit an active database transaction
		/// </summary>
		public void CommitTransaction()
		{
			// End transaction
			try
			{
				if (!database.Commit())
				{
					throw new Exception("Commit returned false");
				}

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
		public void CancelTransaction()
		{
			database.Rollback();
			Program.Logs.Info("Cancelled transaction {0}", transcount);
		}
	}
}
