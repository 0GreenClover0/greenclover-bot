using Discord;
using Discord.WebSocket;
using SharpLink;
using System;
using System.Threading.Tasks;

namespace GreenClover
{
    class Program
    {
        static void Main(string[] args)
            => new Program().RunBotAsync().GetAwaiter().GetResult();

        private static DiscordSocketClient _client = new DiscordSocketClient();
        private CommandHandler _handler;
        private AudioService _audioService;

        public static LavalinkManager _lavalinkManager = new LavalinkManager(_client, new LavalinkManagerConfig()
        {
            RESTHost = "localhost",
            RESTPort = 2333,
            WebSocketHost = "localhost",
            WebSocketPort = 80,
            Authorization = "youshallnotpass",
            TotalShards = 1,
            LogSeverity = LogSeverity.Debug
        });

        public async Task RunBotAsync()
        {
            if (Config.bot.token == "" || Config.bot.token == null) return;

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            _lavalinkManager = new LavalinkManager(_client, new LavalinkManagerConfig()
            {
                RESTHost = "localhost",
                RESTPort = 2333,
                WebSocketHost = "localhost",
                WebSocketPort = 80,
                Authorization = "youshallnotpass",
                TotalShards = 1,
                LogSeverity = LogSeverity.Debug
            });

            _client.Log += Log;
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();

            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            _client.UserJoined += AnnounceUserJoined;
            
            _client.Ready += async () =>
            {
                await _lavalinkManager.StartAsync();
            };
            _lavalinkManager.Log += message =>
            {
                Console.WriteLine(message);
                return Task.CompletedTask;
            };

            await Task.Delay(-1);
        }

        private async Task AnnounceUserJoined(SocketGuildUser user)
        {
            var guild = user.Guild;
            var channel = guild.DefaultChannel;
            string mention = user.Mention;
            await channel.SendMessageAsync(Utilities.GetFormattedAlert("WELCOME", mention));  
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}