﻿using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Reflection;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace GreenClover
{
    class CommandHandler
    {
        DiscordSocketClient _client;
        CommandService _commands;
        IServiceProvider _services;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _commands = new CommandService();
            _services = new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .BuildServiceProvider();
            
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message == null || message.Author.IsBot) return;

            int argPos = 0;
            if (message.HasStringPrefix(Config.bot.cmdPrefix, ref argPos)
                    || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(_client, message);

                var result = await _commands.ExecuteAsync(context, argPos, _services);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    Console.WriteLine(result.ErrorReason);
            }
        }
    }
}