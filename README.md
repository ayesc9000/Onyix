# Onyix

This is a C# re-write of my Discord bot. The code is licensed under the Apache 2.0 license. Please refer to `LICENSE.md` for more information.

1. [Setting up the development environment](#1-setting-up-the-development-environment)

2. [Environment variables](#2-environment-variables)

3. [Docker usage](#3-docker-usage)
   
   3.1 [Debugging within Docker on Windows](#31-debugging-within-docker-on-windows)
   
   3.2 [Debugging within Docker on MacOS or Linux](#32-debugging-within-docker-on-macos-or-linux)
   
   3.3 [Building a release image](#33-building-a-release-image)

## 1. Setting up the development environment

Everything should be pretty much ready to go right out of the box after you clone the repository to your machine.

> Note: If you are getting tons of errors upon opening the solution for the first time, then run a full build of the solution first. This often resolves the issue.

When running in a debug build, Onyix will automatically load all of your user secrets as environment variables.

To set user secrets, open a terminal in the same folder as the Onyix project file and use the following command:

```shell
dotnet user-secrets set "NAME" "VALUE"
```

> Note: Onyix only uses user-secrets for debug builds. You must specify environment variables manually when running a production build.

## 2. Environment variables

| **Variable name** | **Purpose**                                                                       |
| ----------------- | --------------------------------------------------------------------------------- |
| TOKEN             | The Discord account that Onyix should log in to.                                  |
| GUILD             | The ID of the Discord guild to push slash commands to when running a debug build. |

## 3. Docker usage

### 3.1 Debugging within Docker on Windows

Onyix comes pre-configured to support debugging within a Docker container for Windows users.

Make sure Docker Desktop is already running in the background. In Visual Studio, open the Debug Target drop down, and select the Docker option. Begin debugging by either clicking the Start Debugging button, or by pressing F5.

---

### 3.2 Debugging within Docker on MacOS or Linux

> Important: This feature has not been tested, as I am unable to test it properly myself. It has been included in the hopes that it will be useful to others who may need it. 

Onyix requires some slight configuration before you can begin debugging within a Docker container on non-Windows platforms.

Open the Onyix project file, and locate `DockerfileRunArguments`. Documentation comments have been placed to help guide you through the changes you will need to make in order to allow your user secrets to be properly mounted.

You can now begin debugging by opening a terminal in the same folder as the project file, and using the following command:

```shell
dotnet run --launch-profile Docker
```

---

### 3.3 Building a release image

**For Visual Studio:**

Make sure that Docker Desktop is already running. Open the Onyix project and locate the `dockerfile` at the root of the project. Right click the file, and select "Build Docker Image"

**For everything else:**

Make sure Docker Desktop is already running, if applicable. Open a terminal in the Onyix project file. Run the following command to begin building the image:

```shell
docker build
```