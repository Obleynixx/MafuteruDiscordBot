using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace MyDiscordBot
{
    class Program
    {
        private DiscordSocketClient? _client;
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            if (_client != null)
            {
                _client.Log += Log;
                _client.MessageReceived += MessageReceived;

                var config = LoadConfiguration();
                await _client.LoginAsync(TokenType.Bot, config.BotToken);
                await _client.StartAsync();
            }

            // Prevents the app from closing
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Author.IsBot) return;

            if (message.Content == "!ping")
            {
                await message.Channel.SendMessageAsync("Pong!");
            }
        }

        private Configuration LoadConfiguration()
        {
            using var stream = new StreamReader("config.json");
            var json = stream.ReadToEnd();
            return JsonConvert.DeserializeObject<Configuration>(json);
        }
    }

    public class Configuration
    {
        public string BotToken { get; set; }
    }
}
