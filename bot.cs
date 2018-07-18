using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace GreenClover
{
    class Program
    {
        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();
        
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        
        public async Task RunBotAsync()
        {
            if (Config.bot.token == "" || Config.bot.token == null) return;

            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .BuildServiceProvider();

            _client.Log += Log;
            _client.UserJoined += AnnounceUserJoined;

            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task AnnounceUserJoined(SocketGuildUser user)
        {
            var guild = user.Guild;
            var channel = guild.DefaultChannel;
            string mention = user.Mention;
            await channel.SendMessageAsync(Utilities.GetFormattedAlert("WELCOME", mention));  
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message is null || message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix(Config.bot.cmdPrefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);

                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    Console.WriteLine(result.ErrorReason);
            }
        }
    }
}
