using Microsoft.EntityFrameworkCore;

namespace Onyix.Models
{
	public class DatabaseContext : DbContext
	{
		public DbSet<UserLevel>? UserLevel { get; set; }
		public DbSet<LevelSettings>? LevelSettings { get; set; }
		public DbSet<UserKarma>? UserKarma { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite($"Data Source=\"{Program.DataPath}onyix.db\"");
		}
	}
}
