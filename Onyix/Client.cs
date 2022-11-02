using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Onyix
{
	public class Client
	{
		private readonly DiscordSocketClient client;

		public Client()
		{
			client = new();
			client.Log += Log;
		}

		public async Task Start()
		{
			//await client.LoginAsync(TokenType.Bot, "");
			//await client.StartAsync();
			await Task.Delay(-1);
		}

		public async Task Log(LogMessage message)
		{
			switch (message.Severity)
			{
				case LogSeverity.Verbose:
					Program.Logs.Debug(exception: message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Debug:
					Program.Logs.Debug(exception: message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Info:
					Program.Logs.Info(exception: message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Warning:
					Program.Logs.Warn(exception: message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Error:
					Program.Logs.Error(exception: message.Exception, "{0}: {1}", message.Source, message.Message);
					break;

				case LogSeverity.Critical:
					Program.Logs.Fatal(exception: message.Exception, "{0}: {1}", message.Source, message.Message);
					break;
			}
		}
	}
}
