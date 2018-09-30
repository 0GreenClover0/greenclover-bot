using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GreenClover.Core.Accounts;

namespace GreenClover.Modules.AccountCommands
{
    public class GuildCommands
    {
        public class AccountCommands : ModuleBase<SocketCommandContext>
        {
            [Command("prefix set")]
            [RequireUserPermission(GuildPermission.Administrator)]
            public async Task SetPrefixAsync([Remainder] string prefix = "")
            {
                string avatar = Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl();

                if (prefix.Length > 12)
                {
                    await ReplyAsync("Prefix nie może być dłuższy niż 12 znaków");
                    return;
                }

                var guildAccount = GuildAccounts.GetGuildAccount(Context.Guild);
                guildAccount.Prefix = prefix;
                GuildAccounts.SaveGuilds();

                EmbedBuilder builder = new EmbedBuilder();
                builder
                    .WithAuthor(Context.Message.Author.Username, avatar)
                    .WithDescription($"Prefix serwera został zmieniony na {prefix}");

                await ReplyAsync("", false, builder.Build());
            }

            [Command("prefix")]
            public async Task ShowPrefixAsync()
            {
                string avatar = Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl();
                var guildAccount = GuildAccounts.GetGuildAccount(Context.Guild);

                EmbedBuilder builder = new EmbedBuilder();
                builder
                    .WithAuthor(Context.Message.Author.Username, avatar)
                    .WithDescription($"Domyślny prefix bota: {Config.bot.cmdPrefix} \nPrefix serwera: {guildAccount.Prefix}\nAby go zmienić użyj komendy **prefix set**");

                await ReplyAsync("", false, builder.Build());
            }
        }
    }
}