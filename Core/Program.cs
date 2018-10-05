using Discord;
using Discord.WebSocket;
using SharpLink;
using System;
using System.Threading.Tasks;
using Console = Colorful.Console;
using InstaSharp;

namespace GreenClover
{
    class Program : IDisposable
    {
        private DiscordSocketClient _client;
        private CommandHandler _handler;

        static void Main(string[] args)
            => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync()
        {
            if (Config.bot.token == "" || Config.bot.token == null) return;

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Info
            });

            await InitializationClient();
            await InitializationLogs();

            await Task.Delay(-1);
        }

        private async Task InitializationClient()
        {
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

            await LoginAsync();
            await HandlerInitialize();

            _client.Ready += async () =>
            {
                await AudioService.lavalinkManager.StartAsync();
            };
        }

        private async Task LoginAsync()
        {
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
        }

        private async Task HandlerInitialize()
        {
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
        }

        private Task InitializationLogs()
        {
            //_client.UserJoined += AnnounceUserJoined;
            // Client logs and Lavalink logs
            _client.Log += BotLog;
            AudioService.lavalinkManager.Log += message =>
            {
                Console.WriteLine(message, Color.Gold);
                return Task.CompletedTask;
            };

            return Task.CompletedTask;
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
        private Task BotLog(LogMessage msg)
        {
            Console.WriteLine(msg.Message, Color.Green);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // Dispose of the client when we are done with it
            _client.Dispose();
        }
    }
}