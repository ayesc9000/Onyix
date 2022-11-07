# Onyix

Onyix is an open-source general purpose Discord bot, written in C#.

The code is licensed under the GPL v3 license. Please refer to `LICENSE.md` for more information. Contributors should read section 1 of this readme for more information regarding required boilerplate in source code.

---

**Table of contents:**

1. [Source code boilerplate](#1-source-code-boilerplate)

2. [Setting up the development environment](#2-setting-up-the-development-environment)

3. [Docker usage](#3-docker-usage)
   
   3.1 [Debugging within Docker on Windows](#31-debugging-within-docker-on-windows)
   
   3.2 [Debugging within Docker on MacOS or Linux](#32-debugging-within-docker-on-macos-or-linux)
   
   3.3 [Building a release image](#33-building-a-release-image)

4. [Environment variables](#4-environment-variables)

5. [Embed colours](#5-embed-colours)

## 1. Source code boilerplate

All source files in this repository should include boilerplate for the GPL v3 license. A template for this is included below:

```csharp
/* Onyix - An open-source Discord bot
 * Copyright (C) 2022 Liam "AyesC" Hogan
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see http://www.gnu.org/licenses/.
 */
```

You may modify these aspects of the boilerplate:

- The copyright date

- The commenting syntax

Any changes to the boilerplate that do not conform to these requirements will not be accepted.

## 2. Setting up the development environment

Everything should be pretty much ready to go right out of the box after you clone the repository to your machine.

> Note: If you are getting tons of errors upon opening the solution for the first time, then run a full build of the solution first. This often resolves the issue.

When running in a debug build, Onyix will automatically load all of your user secrets as environment variables.

To set user secrets, open a terminal in the same folder as the Onyix project file and use the following command:

> Note: Onyix only uses user-secrets for debug builds. You must specify environment variables manually when running a production build.

```shell
dotnet user-secrets set "NAME" "VALUE"
```

## 3. Docker usage

### 3.1 Debugging within Docker on Windows

Onyix comes preconfigured to support debugging within a Docker container for Windows users.

Make sure Docker Desktop is already running in the background. In Visual Studio, open the Debug Target drop down, and select the Docker option. Begin debugging by either clicking the Start Debugging button, or by pressing F5.

### 3.2 Debugging within Docker on MacOS or Linux

> Important: This feature has not been tested, as I am unable to test it properly myself. It has been included in the hopes that it will be useful to others who may need it. 

Onyix requires some slight configuration before you can begin debugging within a Docker container on non-Windows platforms.

Open the Onyix project file, and locate `DockerfileRunArguments`. Documentation comments have been placed to help guide you through the changes you will need to make in order to allow your user secrets to be properly mounted.

You can now begin debugging by opening a terminal in the same folder as the project file, and using the following command:

```shell
dotnet run --launch-profile Docker
```

### 3.3 Building a release image

**For Visual Studio:**

Make sure that Docker Desktop is already running. Open the Onyix project and locate the `dockerfile` at the root of the project. Right click the file, and select "Build Docker Image"

**For everything else:**

Make sure Docker Desktop is already running, if applicable. Open a terminal in the Onyix project file. Run the following command to begin building the image:

```shell
docker build
```

## 4. Environment variables

| **Variable name** | **Purpose**                                                                       |
| ----------------- | --------------------------------------------------------------------------------- |
| TOKEN             | The Discord account that Onyix should log in to.                                  |
| GUILD             | The ID of the Discord guild to push slash commands to when running a debug build. |

## 5. Embed colours

| Colour name | Hex value | Use case or purpose           |
| ----------- | --------- | ----------------------------- |
| Gray        | `#606266` | General information           |
| Green       | `#32a852` | Success or command completion |
| Yellow      | `#edbc28` | Warning or low-priority error |
| Red         | `#e83c3c` | High priority error           |