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

- .NET SDK (Or .NET Desktop Development workload for VS)
- .NET 7 runtime

Clone and restore the repository (`dotnet restore`).

Set the `TOKEN`, `GUILD` and `DATABASE` variables with user secrets. You can set a user secret via the CLI with `dotnet user-secrets set "NAME" "VALUE"`. You can also set these through environment variables.

Hit F5 (or type `dotnet run`) and you're debugging!

## 2. Debugging containers

Prerequisites:

- Docker (Docker Desktop if using VS)

**Visual Studio:**
Select the debug target dropdown, and select the `Docker` profile from the list. You can then either click Debug or press F5 to begin debugging within the container.

**CLI:**
Open a terminal in the Onyix project folder, and type `dotnet run --launch-profile Docker`.

## 3. Deploying containers

Prerequisites:

- Docker (Docker Desktop if using VS)
- Server with Docker and MariaDB

Right-click the Onyix project and select `Build Docker Image`, or type `docker build` in a terminal from within the project directory.

Copy the newly created image onto your server via your preferred means, and create a new container. Add the `TOKEN` and `DATABASE` strings as environment variables. DO NOT include the `GUILD` string in a production deployment.

## 4. Variables

| **Name** | **Optional** | **Purpose**                |
| -------- | ------------ | -------------------------- |
| TOKEN    | No           | Discord account token      |
| GUILD    | Yes          | Debug guild ID             |
| DATABASE | No           | Database connection string |
