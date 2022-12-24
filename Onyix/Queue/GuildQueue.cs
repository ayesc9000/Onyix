using DSharpPlus.VoiceNext;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Onyix.Queue
{
	public class GuildQueue
	{
		private List<string> queue;
		private QueueState state;
		private CancellationTokenSource ctoken;
		private VoiceNextConnection connection;

		public GuildQueue()
		{
			queue = new();
			state = QueueState.Stopped;
			ctoken = new();
		}

		public void AddToQueue(string uri)
		{
			queue.Add(uri);
		}

		public void ClearQueue()
		{
			if (queue.Count < 1)
				throw new QueueEmptyException();

			queue.Clear();
			state = QueueState.Stopped;
			ctoken.Cancel();
		}

		public bool Play(VoiceNextConnection c)
		{
			connection = c;

			if (state == QueueState.Playing)
				throw new QueueStateException("Already playing");

			if (state == QueueState.Paused)
			{
				state = QueueState.Playing;
				return false;
			}

			if (queue.Count < 1)
				throw new QueueEmptyException();

			state = QueueState.Playing;
			PlayNextItem();

			return true;
		}

		public void Pause()
		{
			if (state == QueueState.Paused)
				throw new QueueStateException("Already paused");

			state = QueueState.Paused;
		}

		public void Skip()
		{
			if (queue.Count < 1)
				throw new QueueEmptyException();

			ctoken.Cancel();
		}

		public void Stop()
		{
			if (state == QueueState.Stopped)
				throw new QueueStateException("Already stopped");

			state = QueueState.Stopped;
			ctoken.Cancel();
		}

		private void PlayNextItem()
		{
			if (state == QueueState.Stopped) return;
			if (queue.Count < 1) return;

			//queue.RemoveAt(0);
			string uri = queue.First();

			ctoken = new();
			PlayURI(uri, ctoken.Token);
		}

		private async Task PlayURI(string uri, CancellationToken token)
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

			Stream stream = ffmpeg.StandardOutput.BaseStream;

			byte[] buffer = new byte[3840];
			int read;

			await connection.SendSpeakingAsync();

			while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
			{
				// Check queue state
				if (state == QueueState.Paused)
					await Task.Delay(50, token);

				if (token.IsCancellationRequested) break;

				// Check if we have read past the end of the stream
				if (read < buffer.Length)
					for (var i = read; i < buffer.Length; i++)
						buffer[i] = 0;

				// Write audio data
				VoiceTransmitSink sink = connection.GetTransmitSink();
				await sink.WriteAsync(buffer, 0, buffer.Length, token);
			}

			await connection.SendSpeakingAsync(false);
		}

		private enum QueueState
		{
			Stopped,
			Paused,
			Playing
		}
	}
}
