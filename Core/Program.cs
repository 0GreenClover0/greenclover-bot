using Discord;
using Discord.WebSocket;
using SharpLink;
using System;
using System.Threading.Tasks;
using InstaSharp;

namespace GreenClover
{
    class Program
    {
        static void Main(string[] args)
            => new Program().RunBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private CommandHandler _handler;

        public async Task RunBotAsync()
        {
            if (Config.bot.token == "" || Config.bot.token == null) return;

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });

            AudioService.lavalinkManager = new LavalinkManager(_client, new LavalinkManagerConfig()
            {
                RESTHost = "127.0.0.1",
                RESTPort = 2333,
                WebSocketHost = "127.0.0.1",
                WebSocketPort = 80,
                Authorization = "youshallnotpass",
                TotalShards = 1,
                LogSeverity = LogSeverity.Verbose
            });

            await InitializationClient();
            await InitializationLogs();

            await Task.Delay(-1);
        }

        private async Task InitializationClient()
        {
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
            _client.Ready += async () =>
            {
                await AudioService.lavalinkManager.StartAsync();
            };
        }

        private async Task InitializationLogs()
        {
            _client.Log += Log;
            //_client.UserJoined += AnnounceUserJoined;

            // Lavalink logs
            AudioService.lavalinkManager.Log += message =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(message);
                return Task.CompletedTask;
            };
        }

        /*private async Task AnnounceUserJoined(SocketGuildUser user)
        {
            var guild = user.Guild;
            var channel = guild.DefaultChannel;
            string mention = user.Mention;
            string avatar = user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl();

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithAuthor(user.Username, avatar)
                .WithDescription($"Hello {mention}")
                .WithColor(new Color(65, 140, 230));

            await channel.SendMessageAsync("", false, builder.Build());  
        }*/

        // Client logs
        private async Task Log(LogMessage msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg.Message);
        }
    }
}