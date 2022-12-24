using DSharpPlus.VoiceNext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyix
{
	public static class AudioDispatcher
	{
		private static readonly Dictionary<ulong, List<string>> queues = new();
		private static readonly Dictionary<ulong, CancellationTokenSource> managers = new();
		private static readonly Dictionary<ulong, CancellationTokenSource> players = new();

		public static void QueueAudio(ulong id, string uri)
		{
			lock (queues[id])
			{
				queues[id].Add(uri);
			}
		}

		public static void ClearQueue(ulong id)
		{
			lock (queues[id])
			{
				queues[id].Clear();
			}
		}

		public static bool QueueIsNotEmpty(ulong id)
		{
			lock (queues[id])
			{
				return queues[id].Count > 0;
			}
		}

		public static void SkipItem(ulong id)
		{
			lock (players)
			{
				players[id].Cancel();
			}
		}

		public static void StopGuildQueue(ulong id)
		{
			// Cancel the manager first so we dont start a new player
			lock (managers)
			{
				managers[id].Cancel();
			}

			lock (players)
			{
				players[id].Cancel();
			}
		}

		public static void StartGuildQueue(ulong id, VoiceNextConnection connection)
		{
			CancellationTokenSource cts = new();
			Task manager = PlayQueue(id, connection, cts.Token);
			
			lock (managers)
			{
				managers.Add(id, cts);
			}

			manager.Start();
		}

		private static async Task PlayQueue(ulong id, VoiceNextConnection connection, CancellationToken token)
		{
			while (queues[id].Count > 0)
			{
				string uri;

				// Get next item from queue
				lock (queues[id])
				{
					uri = queues[id].First();
					queues[id].RemoveAt(0);
				}

				// Create audio stream
				Stream stream = CreateFFmpegStream(uri);

				// Play stream
				CancellationTokenSource cts = new();
				Task player = PlayStream(stream, connection, cts.Token);

				lock (players)
				{
					players.Add(id, cts);
				}

				try
				{
					await connection.SendSpeakingAsync();
					player.Start();
					player.Wait(token);
					await connection.SendSpeakingAsync(false);
				}
				catch (OperationCanceledException)
				{
					break;
				}
			}
		}

		private static Stream CreateFFmpegStream(string uri)
		{
			Process? ffmpeg = Process.Start(new ProcessStartInfo()
			{
				FileName = "ffmpeg",
				Arguments = $@"-i ""{uri}"" -ac 2 -f s16le -ar 48000 pipe:1",
				RedirectStandardOutput = true,
				UseShellExecute = false
			});

			if (ffmpeg is null)
				throw new Exception("FFmpeg could not be started!");

			return ffmpeg.StandardOutput.BaseStream;
		}

		private static async Task PlayStream(Stream stream, VoiceNextConnection connection, CancellationToken token)
		{
			byte[] buffer = new byte[3840];
			int read;

			while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
			{
				// Check if we have read past the end of the stream
				if (read < buffer.Length)
					for (var i = read; i < buffer.Length; i++)
						buffer[i] = 0;

				// Write audio data
				VoiceTransmitSink sink = connection.GetTransmitSink();
				await sink.WriteAsync(buffer, 0, buffer.Length, token);

				if (token.IsCancellationRequested) break;
			}
		}
	}
}
