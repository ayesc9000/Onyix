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

using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.VoiceNext;
using System.Threading.Tasks;
using Onyix.Queue;

namespace Onyix.Commands
{
	[SlashCommandGroup("music", "Stream music in a voice channel", true)]
	public class Music : ApplicationCommandModule
	{
		[SlashCommand("join", "Connect to your current voice channel", true)]
		public static async Task Join(InteractionContext ctx)
		{
			VoiceNextExtension voice = ctx.Client.GetVoiceNext();

			// Check if Onyix is already connected
			if (voice.GetConnection(ctx.Guild) is not null)
			{
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Error")
					.WithDescription("Onyix is already connected to a voice channel in this guild.")
					.WithColor(Colors.Red), true);

				return;
			}

			// Check if user is in a channel
			if (ctx.Member.VoiceState is null)
			{
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Error")
					.WithDescription("You are currently not in a voice channel.")
					.WithColor(Colors.Red), true);

				return;
			}

			// Connect to the channel
			try
			{
				VoiceNextConnection connection = await voice.ConnectAsync(ctx.Member.VoiceState.Channel);

				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Success")
					.WithDescription("Connected to voice.")
					.WithColor(Colors.Green), true);
			}
			catch
			{
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Error")
					.WithDescription("Failed to connect to voice channel. Please check the permissions for this channel.")
					.WithColor(Colors.Red), true);
			}
		}

		[SlashCommand("leave", "Disconnect from the current voice channel", true)]
		public static async Task Leave(InteractionContext ctx)
		{
			VoiceNextExtension voice = ctx.Client.GetVoiceNext();
			VoiceNextConnection connection = voice.GetConnection(ctx.Guild);

			// Check if Onyix is connected to a channel
			if (connection is null)
			{
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Error")
					.WithDescription("Onyix is not connected to a voice channel.")
					.WithColor(Colors.Red), true);

				return;
			}

			// Disconnect from the channel
			try
			{
				connection.Disconnect();

				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Success")
					.WithDescription("Disconnected from voice.")
					.WithColor(Colors.Green), true);
			}
			catch
			{
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Error")
					.WithDescription("Failed to disconnect from voice channel.")
					.WithColor(Colors.Red), true);
			}
		}

		[SlashCommand("play", "Play an audio file", true)]
		public static async Task Play(InteractionContext ctx,
			[Option("file", "The file to play")] string uri)
		{
			VoiceNextExtension voice = ctx.Client.GetVoiceNext();
			VoiceNextConnection connection = voice.GetConnection(ctx.Guild);

			// Check if Onyix is connected to a channel
			if (connection is null)
			{
				await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
					.WithTitle("Error")
					.WithDescription("Onyix is not connected to a voice channel.")
					.WithColor(Colors.Red), true);

				return;
			}

			// Get queue
			GuildQueue q = QueueManager.GetQueue(ctx.Guild.Id);
			q.AddToQueue(uri);
			q.Play(connection);

			await ctx.CreateResponseAsync(new DiscordEmbedBuilder()
				.WithTitle("Now Playing")
				.WithDescription("Now playing some audio.")
				.WithColor(Colors.Green), true);
		}
	}
}
