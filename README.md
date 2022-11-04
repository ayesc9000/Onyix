# Onyix

This is a C# re-write of my old Discord bot. The code is licensed under the Apache 2.0 license. Please refer to `LICENSE.txt` for more information.

## Setting up the development environment

Open a shell window in the Onyix project directory and run the following commands:

Set the development token secret:

```shell
dotnet user-secrets set "TOKEN" "Your Bot Token Here"
```

Set the development server ID:

```shell
dotnet user-secrets set "GUILD" "1234567890"
```

**Important: This is for the development environment only. The token is set through the `TOKEN` environment variable in production.**

## To-Do

- [x] Functioning client

- [x] Working slash commands

- [ ] Rewrite database

- [ ] Cleanup