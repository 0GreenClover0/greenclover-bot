using Discord;
using Discord.Commands;
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
                Utilities utilities = new Utilities(Context.Guild);
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
                Utilities utilities = new Utilities(Context.Guild);
                string avatar = Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl();
                var guildAccount = GuildAccounts.GetGuildAccount(Context.Guild);

                EmbedBuilder builder = new EmbedBuilder();
                builder
                    .WithAuthor(Context.Message.Author.Username, avatar)
                    .WithDescription($"Domyślny prefix bota: {Config.bot.cmdPrefix} \nPrefix serwera: {guildAccount.Prefix}\nAby go zmienić użyj komendy **prefix set**");

                await ReplyAsync("", false, builder.Build());
            }

            [Command("language set")]
            public async Task ChangeLanguage([Remainder] string language = "")
            {
                Utilities utilities = new Utilities(Context.Guild);
                string avatar = Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl();
                var guildAccount = GuildAccounts.GetGuildAccount(Context.Guild);

                if (language == "")
                {
                    EmbedBuilder builderList = new EmbedBuilder();
                    builderList
                        .WithAuthor(Context.Message.Author.Username, avatar)
                        .WithDescription(Utilities.GetAlert("BOT_LANGUAGES_SHOW_LIST") + Config.bot.languageList);

                    await ReplyAsync("", false, builderList.Build());
                    return;
                }

                else if (language == "polish" || language == "polski")
                {
                    guildAccount.ConfigLang = "Texts/pl_PL.json";
                    GuildAccounts.SaveGuilds();

                    EmbedBuilder builderPolish = new EmbedBuilder();
                    builderPolish
                        .WithAuthor(Context.Message.Author.Username, avatar)
                        .WithDescription(Utilities.GetAlert("BOT_LANGUAGE_CHANGED_POLISH"));

                    await ReplyAsync("", false, builderPolish.Build());
                    return;
                }

                else if (language == "english")
                {
                    guildAccount.ConfigLang = "Texts/en_US.json";
                    GuildAccounts.SaveGuilds();

                    EmbedBuilder builderEnglish = new EmbedBuilder();
                    builderEnglish
                        .WithAuthor(Context.Message.Author.Username, avatar)
                        .WithDescription(Utilities.GetAlert("BOT_LANGUAGE_CHANGED_ENGLISH"));

                    await ReplyAsync("", false, builderEnglish.Build());
                    return;
                }

                else
                {
                    EmbedBuilder builderEnglish = new EmbedBuilder();
                    builderEnglish
                        .WithAuthor(Context.Message.Author.Username, avatar)
                        .WithDescription(Utilities.GetAlert("BOT_LANGUAGE_ERROR"));

                    await ReplyAsync("", false, builderEnglish.Build());
                    return;
                }
            }
        }
    }
}