using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using GreenClover.Core;
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
                    await ReplyAsync(Utilities.GetAlert("PREFIX_ERROR_CHARS"));
                    return;
                }

                var guildAccount = GuildAccounts.GetGuildAccount(Context.Guild);
                guildAccount.Prefix = prefix;
                GuildAccounts.SaveGuilds();

                EmbedBuilder builder = new EmbedBuilder();
                builder
                    .WithAuthor(Context.Message.Author.Username, avatar)
                    .WithDescription(Utilities.GetFormattedAlert("PREFIX_CHANGED", prefix));

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
                    .WithDescription(Utilities.GetFormattedAlert("PREFIX_INFO", Config.bot.cmdPrefix, guildAccount.Prefix));

                await ReplyAsync("", false, builder.Build());
            }

            [Command("language set")]
            [RequireUserPermission(GuildPermission.Administrator)]
            public async Task ChangeLanguageAsync([Remainder] string language = "")
            {
                Utilities utilities = new Utilities(Context.Guild);
                string avatar = Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl();
                var guildAccount = GuildAccounts.GetGuildAccount(Context.Guild);
                language.ToLower();

                string changedLanguage = GuildUtil.ChangeLanguage(guildAccount, language);
                string changedLanguageAlertKey = GuildUtil.ChangedLanguageAlertKey(changedLanguage);

                if (changedLanguage == null)
                {
                    EmbedBuilder builderList = new EmbedBuilder();
                    builderList
                        .WithAuthor(Context.Message.Author.Username, avatar)
                        .WithDescription(Utilities.GetAlert("LANGUAGES_SHOW_LIST") + Utilities.GetAlert("LANGUAGES_LIST") + Utilities.GetAlert("LANGUAGE_USING"))
                        .WithColor(new Color(66, 244, 113));

                    await ReplyAsync("", false, builderList.Build());
                    return;
                }

                else
                {
                    EmbedBuilder builder = new EmbedBuilder();
                    builder
                        .WithAuthor(Context.Message.Author.Username, avatar)
                        .WithDescription(Utilities.GetAlert(changedLanguageAlertKey));

                    await ReplyAsync("", false, builder.Build());
                    return;
                }
            }

            [Command("language")]
            public async Task LanguageAsync()
            {
                Utilities utilities = new Utilities(Context.Guild);
                string avatar = Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl();
                var guildAccount = GuildAccounts.GetGuildAccount(Context.Guild);

                EmbedBuilder builderList = new EmbedBuilder();
                builderList
                    .WithAuthor(Context.Message.Author.Username, avatar)
                    .WithDescription(Utilities.GetAlert("LANGUAGES_SHOW_LIST") + Utilities.GetAlert("LANGUAGES_LIST") + Utilities.GetAlert("LANGUAGE_USING"))
                    .WithColor(new Color(66, 244, 113));

                await ReplyAsync("", false, builderList.Build());
                return;
            }
        }
    }
}