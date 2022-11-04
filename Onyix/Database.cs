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

			// Make sure collections exist
			_ = database.GetCollection<LevelSettings>("levelsettings");
			_ = database.GetCollection<UserKarma>("userkarma");
			_ = database.GetCollection<UserLevel>("userlevel");
		}

		/// <summary>
		/// Start a new transaction on the database
		/// </summary>
		/// <remarks>There can only be one active transaction per thread</remarks>
		/// <typeparam name="T">Entity class type</typeparam>
		/// <param name="name">Name of collection</param>
		/// <returns>Found collection</returns>
		/// <exception cref="Exception">Missing collection or failed to start transaction</exception>
		public ILiteCollection<T> StartTransaction<T>(string name)
		{
			// Check collection
			if (!database.CollectionExists(name))
			{
				throw new Exception("This collection does not exist");
			}

			// Start transaction
			if (!database.BeginTrans())
			{
				// TODO: Not sure whether to throw exception or return null in this state.
				// An exception will do for now. Fix this later.
				throw new Exception("Failed to start database transaction.");
			}

			Program.Logs.Info("Started database transaction with collection {0}", name);
			return database.GetCollection<T>(name);
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
	}
}
