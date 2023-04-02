using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onyix.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LevelSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GuildId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    EnableLevels = table.Column<bool>(type: "INTEGER", nullable: false),
                    EnableLevelUpMessage = table.Column<bool>(type: "INTEGER", nullable: false),
                    Multiplier = table.Column<double>(type: "REAL", nullable: false),
                    XpPerMessage = table.Column<int>(type: "INTEGER", nullable: false),
                    XpPerLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    Cooldown = table.Column<int>(type: "INTEGER", nullable: false),
                    LevelUpTitle = table.Column<string>(type: "TEXT", nullable: false),
                    LevelUpContent = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserKarma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Upvotes = table.Column<long>(type: "INTEGER", nullable: false),
                    Downvotes = table.Column<long>(type: "INTEGER", nullable: false),
                    Awards = table.Column<long>(type: "INTEGER", nullable: false),
                    Posts = table.Column<long>(type: "INTEGER", nullable: false),
                    Removed = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserKarma", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    GuildId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    XP = table.Column<long>(type: "INTEGER", nullable: false),
                    TotalXP = table.Column<long>(type: "INTEGER", nullable: false),
                    Level = table.Column<long>(type: "INTEGER", nullable: false),
                    LastGain = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLevel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LevelSettings_GuildId",
                table: "LevelSettings",
                column: "GuildId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserKarma_UserId",
                table: "UserKarma",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLevel_UserId_GuildId",
                table: "UserLevel",
                columns: new[] { "UserId", "GuildId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LevelSettings");

            migrationBuilder.DropTable(
                name: "UserKarma");

            migrationBuilder.DropTable(
                name: "UserLevel");
        }
    }
}
