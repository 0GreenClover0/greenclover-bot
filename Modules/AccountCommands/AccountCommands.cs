using Discord.Commands;
using Discord;
using System.Threading.Tasks;
using GreenClover.Core.Accounts;
using Discord.WebSocket;
using System.Linq;

namespace GreenClover.Modules.AccountCommands
{
    public class AccountCommands : ModuleBase<SocketCommandContext>
    {
        [Command("profile")]
        [Alias("profil")]
        public async Task ProfileAsync([Remainder] string arg = "")
        {
            Utilities utilities = new Utilities(Context.Guild);
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            SocketUser target = mentionedUser ?? Context.User;

            var account = UserAccounts.GetAccount(target);
            string avatar = target.GetAvatarUrl() ?? target.GetDefaultAvatarUrl();

            EmbedBuilder builder = new EmbedBuilder();
            builder
               .WithAuthor(Utilities.GetFormattedAlert("PROFILE_ACCOUNT_NAME", target.Username), avatar)
               .WithThumbnailUrl(avatar)
               .WithDescription(account.Description)
               .AddField(Utilities.GetAlert("PROFILE_ACCOUNT_LEVEL"), $"{account.XP}")
               .WithFooter(Utilities.GetAlert("BOT_NAME_INPROFILE"))
               .WithColor(Color.Magenta);

            await ReplyAsync(" ", false, builder.Build());
        }

        [Command("level")]
        [Alias("poziom", "lvl")]
        public async Task LevelAsync([Remainder] string arg = "")
        {
            Utilities utilities = new Utilities(Context.Guild);

            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            SocketUser target = mentionedUser ?? Context.User;
            var account = UserAccounts.GetAccount(target);
            string avatar = target.GetAvatarUrl() ?? target.GetDefaultAvatarUrl();

            EmbedBuilder builder = new EmbedBuilder();
            builder
               .WithAuthor(target.Username, avatar)
               .WithDescription(Utilities.GetFormattedAlert("LEVEL_LEVEL", target.Username, account.XP));

            await ReplyAsync("", false, builder.Build());
        }

        [Command("description set")]
        [Alias("desc set")]
        public async Task DescriptionSetAsync([Remainder]string desc = "")
        {
            Utilities utilities = new Utilities(Context.Guild);
            var account = UserAccounts.GetAccount(Context.User);
            string avatar = Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl();

            if (desc.Length > 140)
            {
                await ReplyAsync(Utilities.GetFormattedAlert("DESCRIPTION_TOO_MANY_CHARS", desc.Length));
                return;
            }

            account.Description = desc;
            UserAccounts.SaveAccounts();
            
            EmbedBuilder builder = new EmbedBuilder();
            builder
               .WithAuthor(Context.Message.Author.Username, avatar)
               .WithDescription(Utilities.GetFormattedAlert("DESCRIPTION_CHANGED", account.Description))
               .WithColor(Color.Magenta);

            await ReplyAsync("", false, builder.Build());
        }
    }
}