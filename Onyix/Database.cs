using Discord;
using Microsoft.EntityFrameworkCore;
using Onyix.Models;
using System;
using System.Linq;
using System.Timers;

namespace Onyix
{
	public static class Database
	{
		private static readonly DatabaseContext dbcontext = new();
		private static readonly Timer savetimer = new();

		/// <summary>
		/// Get the active database context
		/// </summary>
		public static DatabaseContext DatabaseContext
		{
			get { return dbcontext; }
		}

		/// <summary>
		/// Ensure the database is ready to begin sending and receiving data
		/// </summary>
		public static void StartDatabase()
		{
			// Create database if it does not exist
			Logger.WriteLog(LogSeverity.Info, "Database", "Starting database...");
			dbcontext.Database.EnsureCreated();

			// Apply migrations
			Logger.WriteLog(LogSeverity.Info, "Database", "Migrating changes...");
			dbcontext.Database.Migrate();

			// Start save timer
			savetimer.Interval = Config.Data.SaveInterval;
			savetimer.Elapsed += TimerElapsed;
			savetimer.Start();

			Logger.WriteLog(LogSeverity.Info, "Database", "Database is ready!");
		}

		/// <summary>
		/// Event handler for when the save interval timer is elapsed
		/// </summary>
		/// <param name="sender">The sender of this event</param>
		/// <param name="e">The parameters of this event</param>
		private static void TimerElapsed(object? sender, ElapsedEventArgs e)
		{
			Save();
		}

		/// <summary>
		/// Save the database
		/// </summary>
		public static void Save()
		{
			if (dbcontext.ChangeTracker.HasChanges())
			{
				dbcontext.SaveChanges();
				Logger.WriteLog(LogSeverity.Verbose, "Database", "Changes have been saved.");
			}
		}

		/// <summary>
		/// Get an item from the LevelSettings table or create it if it doesn't exist
		/// </summary>
		/// <param name="guildId">The Id of the guild</param>
		/// <returns>The found item or a new one if it does not exist</returns>
		public static LevelSettings GetLevelSettings(ulong guildId)
		{
			// Get item
			LevelSettings? result = dbcontext.LevelSettings.Find(guildId);

			// Check if it exists
			if (result == null)
			{			
				// Create it if it does not
				result = new LevelSettings()
				{
					GuildId = guildId
				};

				dbcontext.LevelSettings.Add(result);
			}

			// Return item
			return result;
		}

		/// <summary>
		/// Get an item from the UserKarma table or create it if it doesn't exist
		/// </summary>
		/// <param name="userId">The Id of the user</param>
		/// <param name="guildId">The Id of the guild</param>
		/// <returns>The found item or a new one if it does not exist</returns>
		public static UserKarma GetUserKarma(ulong userId, ulong guildId)
		{
			// Get item
			UserKarma? result = dbcontext.UserKarma
				.Where(x => x.UserId == userId && x.GuildId == guildId)
				.FirstOrDefault();

			
			// Check if it exists
			if (result == null)
			{
				// Create item if it does not exist
				result = new UserKarma()
				{
					UserId = userId,
					GuildId = guildId
				};

				dbcontext.UserKarma.Add(result);
			}

			// Return item
			return result;
		}

		/// <summary>
		/// Get an item from the UserLevel table or create it if it doesn't exist
		/// </summary>
		/// <param name="userId">The Id of the user</param>
		/// <param name="guildId">The Id of the guild</param>
		/// <returns>The found item or a new one if it does not exist</returns>
		public static UserLevel GetUserLevel(ulong userId, ulong guildId)
		{
			// Get item
			UserLevel? result = dbcontext.UserLevel
				.Where(x => x.UserId == userId && x.GuildId == guildId)
				.FirstOrDefault();

			// Check if it exists
			if (result == null)
			{
				// Create it if it does not
				result = new UserLevel()
				{
					UserId = userId,
					GuildId = guildId
				};

				dbcontext.UserLevel.Add(result);
			}

			// Return item
			return result;
		}
	}
}
