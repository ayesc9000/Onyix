# Onyix

Onyix is an open-source general purpose Discord bot, written in C# using DSharpPlus.

Please read `LICENSE.md`, `CODE_OF_CONDUCT.md` and `CONTRIBUTING.md` before contributing or forking for additional information.

---

**Table of contents:**

1. [Quick start](#1-quick-start)

2. [Debugging containers](#2-debugging-containers)

3. [Deploying containers](#3-deploying-containers)

4. [Variables](#4-variables)

## 1. Quick start

Prerequisites:

- If using Visual Studio, must be 2022 or newer
- .NET SDK or .NET Desktop Development workload for Visual Studio
- .NET 7 runtime

Clone and restore the repository (`dotnet restore`).

To set variables, you can either use environment variables or user secrets. To set a user secret, open a terminal in the project directory and use the command `dotnet user-secrets set "NAME" "VALUE"`.

Set the `TOKEN` variable to your Discord bot account token. You can also set the `GUILD` variable to the ID of your testing/debugging guild.

Next, set the `DATABASE` variable to a valid MySql/MariaDB connection string. Make sure that the database you put in the connection string does not already exist. You can also set the `MIGRATIONDATABASE` connection string variable if you plan to create new EF Core migrations. Note that this database will be recreated every time you create a migration, so it should never be used to store information.

Finally, hit F5 (or type `dotnet run`) and you're debugging!

## 2. Debugging containers

Prerequisites:

- Docker (Docker Desktop if using VS)

**Visual Studio:**
Select the debug target drop down, and select the `Docker` profile from the list. You can then either click Debug or press F5 to begin debugging within the container.

**CLI:**
Open a terminal in the Onyix project folder, and type `dotnet run --launch-profile Docker`.

## 3. Deploying containers

Prerequisites:

- Docker (Docker Desktop if using VS)
- Server with Docker and MariaDB

Right-click the Onyix project and select `Build Docker Image`, or type `docker build` in a terminal from within the project directory.

Copy the newly created image onto your server via your preferred means, and create a new container. Add the `TOKEN` and `DATABASE` strings as environment variables. DO NOT include the `GUILD` string in a production deployment.

## 4. Variables

| **Name**          | **Optional** | **Purpose**                           |
| ----------------- | ------------ | ------------------------------------- |
| TOKEN             | No           | Discord account token                 |
| GUILD             | Yes          | Debug guild ID                        |
| DATABASE          | No           | Database connection string            |
| MIGRATIONDATABASE | Yes          | Migrations database connection string |
