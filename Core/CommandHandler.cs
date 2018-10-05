using System.Threading.Tasks;
using System.Reflection;
using System;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using GreenClover.Core.Accounts;

namespace GreenClover
{
    class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _commands = new CommandService();
            _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(new InteractiveService(client))
            .AddSingleton(_commands)
            .BuildServiceProvider();
            
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message) || message.Author.IsBot) return;
            var context = new SocketCommandContext(_client, message);
            await CheckPrefix(context, message);
        }

        private async Task CheckPrefix(SocketCommandContext context, SocketUserMessage message)
        {
            var guildAccount = GuildAccounts.GetGuildAccount(context.Guild);

            int argPos = 0;
            if (message.HasStringPrefix(Config.bot.cmdPrefix, ref argPos)
                    || message.HasMentionPrefix(_client.CurrentUser, ref argPos)
                    || message.HasStringPrefix(guildAccount.Prefix, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    Console.WriteLine(result.ErrorReason);
            }
        }
    }
}