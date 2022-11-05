# Onyix

This is a C# re-write of my Discord bot. The code is licensed under the Apache 2.0 license. Please refer to `LICENSE.md` for more information.

## Setting up the development environment

Everything should be pretty much ready to go right out of the box after you clone the repository to your machine.

> Note: If you are getting tons of errors upon opening the solution for the first time, then run a full build of the solution first. This often resolves the issue.

When running in a debug build, Onyix will automatically load all of your user secrets as environment variables.

To set user secrets, open a terminal in the same folder as the Onyix project file and use the following command:

```shell
dotnet user-secrets set "NAME" "VALUE"
```

> Note: Onyix only uses user-secrets for debug builds. You must specify environment variables manually when running a production build.

## Environment Variables

| **Variable name** | **Purpose**                                                                       |
| ----------------- | --------------------------------------------------------------------------------- |
| TOKEN             | The Discord account that Onyix should log in to.                                  |
| GUILD             | The ID of the Discord guild to push slash commands to when running a debug build. |

## Docker usage

Docker is not yet properly set up. I'll fix it eventually.