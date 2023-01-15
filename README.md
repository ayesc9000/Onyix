# Onyix

Onyix is an open-source general purpose Discord bot, written in C# using DSharpPlus.

The code is licensed under the GPL v3 license. Please refer to `LICENSE.md` for more information. For contributing guidelines, please read `CONTRIBUTING.md`.

---

## 1. Development quick start

Prerequisites:
 -  Visual Studio 2022 with .NET 7

    -- OR --

    The .NET 7 SDK with your IDE of choice

 -  A MariaDB database for development use

Clone the repository to your local machine, blah blah blah the usual.

Open a terminal from the Onyix project directory and enter your Discord token, database login, and development guild ID into the project user secrets with `dotnet user-secrets set "NAME" "VALUE"`.

You can refer to [Environment variables](#3-environment-variables) for a complete list of all variables and their purpose.

You can debug both on your local machine or from within a Docker container. To debug within a container, select the Docker debug target from within Visual Studio.

For non-VS users, you can use `dotnet run --launch-profile Docker`.

## 2. Deploying container

Prerequisites:
 -  Working development environment ([Development quick start](#1-development-quick-start))
 -  Docker Desktop (If using Visual Studio)
 -  Server with Docker and MariaDB

From the root of the Onyix project in Visual Studio, right-click the Dockerfile, and select `Build Docker Image`.

For non-VS users, you can run `docker build` from within the project directory.

> Note: Visual Studio will fail to build the Docker image if Docker Desktop is not running.

On your server, add a new container with the created image. The container must have a Discord token and database login specified in the environment variables. Your development guild ID should not be included. Refer to [Environment variables](#3-environment-variables) for a complete list of all variables.

## 3. Environment variables

| **Name** | **Optional** | **Purpose**             |
| -------- | ------------ | ----------------------- |
| TOKEN    | No           | Discord account token   |
| GUILD    | Yes          | Debug guild ID          |
| DBSERVER | No           | Database server address |
| DBPORT   | No           | Database server port    |
| DBUSER   | No           | Database username       |
| DBPASS   | No           | Database password       |
| DATABASE | No           | Database name           |
