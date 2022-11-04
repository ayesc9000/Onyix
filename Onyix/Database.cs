using LiteDB;
using Onyix.Entities;
using System;

namespace Onyix
{
	public class Database
	{
		private readonly LiteDatabase database;

		/// <summary>
		/// Create a new LiteDB instance
		/// </summary>
		public Database()
		{
			database = new(Paths.Database);

			// Configure collections
			ILiteCollection<LevelSettings> levelsettings = database.GetCollection<LevelSettings>("levelsettings");
			levelsettings.EnsureIndex(x => x.GuildId);

			ILiteCollection<UserKarma> userkarma = database.GetCollection<UserKarma>("userkarma");
			userkarma.EnsureIndex(x => x.UserId);

			ILiteCollection<UserLevel> userlevel = database.GetCollection<UserLevel>("userlevel");
			userlevel.EnsureIndex(x => x.UserId);
			userlevel.EnsureIndex(x => x.GuildId);
		}

		/// <summary>
		/// Get the level settings for a guild
		/// </summary>
		/// <param name="guild">Guild ID</param>
		/// <returns>Level settings entity</returns>
		public LevelSettings GetLevelSettings(ulong guild)
		{
			// Find entity
			ILiteCollection<LevelSettings> collection = GetCollection<LevelSettings>("levelsettings");
			LevelSettings entity = collection.FindOne(x => x.GuildId == guild);

			// Create entity if it does not exist
			if (entity == null)
			{
				entity = new()
				{
					GuildId = guild
				};

				collection.Insert(entity);
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
			ILiteCollection<UserKarma> collection = GetCollection<UserKarma>("userkarma");
			UserKarma entity = collection.FindOne(x => x.UserId == user);

			// Create entity if it does not exist
			if (entity == null)
			{
				entity = new()
				{
					UserId = user
				};

				collection.Insert(entity);
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
			ILiteCollection<UserLevel> collection = GetCollection<UserLevel>("userlevel");
			UserLevel entity = collection.FindOne(x => x.GuildId == guild && x.UserId == user);

			// Create entity if it does not exist
			if (entity == null)
			{
				entity = new()
				{
					GuildId = guild,
					UserId = user
				};

				collection.Insert(entity);
			}

			return entity;
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

			Program.Logs.Info("Started database transaction");
		}

		/// <summary>
		/// End an active database transaction
		/// </summary>
		public void EndTransaction()
		{
			// End transaction
			try
			{
				if (!database.Commit())
				{
					throw new Exception("Commit returned false");
				}
			}
			catch (Exception e)
			{
				database.Rollback();
				Program.Logs.Error(e, "Failed to commit database transaction!");
			}
		}

		/// <summary>
		/// Get a collection from the database
		/// </summary>
		/// <typeparam name="T">Entity class type</typeparam>
		/// <param name="name">Name of collection</param>
		/// <returns>Found collection</returns>
		/// <exception cref="Exception">Collection does not exist</exception>
		private ILiteCollection<T> GetCollection<T>(string name)
		{
			// Check collection
			if (!database.CollectionExists(name))
			{
				throw new Exception("This collection does not exist");
			}

			return database.GetCollection<T>(name);
		}
	}
}
