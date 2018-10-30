using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace GreenClover.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        // Just because I use it a lot
        // avatar = Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl();

        [Command("hug")]
        [Alias("przytul")]
        public async Task HugAsync([Remainder]string arg = "")
        {
            Utilities utilities = new Utilities(Context.Guild);
            string key = "HUG_&AUTHORNAME_&TARGETID";
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            SocketUser target = mentionedUser ?? Context.User;

            if (target.Id == Context.Message.Author.Id)
            {
                key = "HUG_&AUTHORNAME";
            }

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithDescription(Utilities.GetFormattedAlert(key, Context.User.Username, target.Id))
                .WithImageUrl(Utilities.GetRandomLine("Texts/hugGif.txt"));

            await ReplyAsync("", false, builder.Build());
            return;
        }

        [Command("ping")]
        public async Task PingAsync()
        {
            Utilities utilities = new Utilities(Context.Guild);

            IUserMessage message = null;
            Stopwatch stopwatch = null;
            int heartbeat = Context.Client.Latency;

            stopwatch = Stopwatch.StartNew();
            message = await ReplyAsync(Utilities.GetFormattedAlert("PING_INIT", heartbeat));
            stopwatch.Stop();
            var init = stopwatch.ElapsedMilliseconds;

            await message.ModifyAsync(x => x.Content = Utilities.GetFormattedAlert("PING_RESPONSE", heartbeat, init));
        }

        [Command("send nudes")]
        [Alias("nudes")]
        public async Task NudesAsync()
        {
            Utilities utilities = new Utilities(Context.Guild);

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithDescription(":womens: | **Wyślij sms'a o treści 69 na mój numer aby dostać więcej**")
                .WithImageUrl("http://fakty.dinoanimals.pl/wp-content/uploads/2014/04/Nosacz3.jpg")
                .WithColor(Color.DarkRed);

            await ReplyAsync("", false, builder.Build());
            return;
        }

        [Command("choose")]
        [Alias("wybierz")]
        public async Task ChooseAsync([Remainder]string message = "")
        {
            Utilities utilities = new Utilities(Context.Guild);

            if (message == "")
            {
                await ReplyAsync(Utilities.GetAlert("CHOOSE_NULLMSG"));
                return;
            }

            string[] options = message.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            Random r = new Random();
            string selection = options[r.Next(0, options.Length)];

            await ReplyAsync(Utilities.GetFormattedAlert("CHOOSE", selection));
            return;
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanAsync(SocketGuildUser user, [Remainder] string reason = "Nie podano powodu")
        {
            Utilities utilities = new Utilities(Context.Guild);
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            if (mentionedUser == null)
            {
                await ReplyAsync(Utilities.GetAlert("BAN_NULLMENTION"));
                return;
            }

            SocketUser target = mentionedUser;

            if (target.IsBot == true)
            {
                await user.Guild.AddBanAsync(user, 0, reason);
                await ReplyAsync(Utilities.GetFormattedAlert("BAN", Context.Message.Author.Id));
                return;
            }

            else
            {
                var dmChannel = await target.GetOrCreateDMChannelAsync();
                await dmChannel.SendMessageAsync(Utilities.GetFormattedAlert("BAN_USERMESSAGE",
                    Context.Guild.Name, Context.Message.Author.Username, reason));
                await user.Guild.AddBanAsync(user, 0, reason);
                await ReplyAsync(Utilities.GetFormattedAlert("BAN", target.Id));
                return;
            }
        }

        [Command("version")]
        [Alias("wersja", "ver")]
        public async Task VersionAsync()
        {
            Utilities utilities = new Utilities(Context.Guild);
            string avatar = Context.Client.CurrentUser.GetAvatarUrl() ?? Context.Client.CurrentUser.GetDefaultAvatarUrl();

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithAuthor(Context.Client.CurrentUser.Username, avatar)
                .WithDescription(Utilities.GetFormattedAlert("BOT_VERSION", GlobalVar.botVersion))
                .WithColor(Color.Gold);

            await ReplyAsync("", false, builder.Build());
            return;
        }

        [Command("about")]
        [Alias("informacje", "info")]
        public async Task AboutAsync()
        {
            Utilities utilities = new Utilities(Context.Guild);
            string avatar = Context.Client.CurrentUser.GetAvatarUrl() ?? Context.Client.CurrentUser.GetDefaultAvatarUrl();

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithAuthor(Context.Client.CurrentUser.Username, avatar)
                .WithDescription(Utilities.GetAlert("BOT_AUTHOR_INFO"))
                .WithFooter(Utilities.GetFormattedAlert("BOT_VERSION", GlobalVar.botVersion))
                .WithColor(Color.Gold);

            await ReplyAsync("", false, builder.Build());
            return;
        }

        [Command("game")]
        [Alias("gra")]
        public async Task GameAsync()
        {
            Utilities utilities = new Utilities(Context.Guild);
            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithImageUrl(Utilities.GetRandomLine("Texts/gameGif.txt"));

            await ReplyAsync("", false, builder.Build());
            return;
        }

        [Command("cat")]
        [Alias("kot")]
        public async Task CatAsync()
        {
            Utilities utilities = new Utilities(Context.Guild);
            Random r = new Random();
            int random = r.Next(1, 327);

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithImageUrl($"https://www.catgifpage.com/gifs/{random}.gif");

            await ReplyAsync("", false, builder.Build());
            return;
        }

        [Command("dog")]
        [Alias("pies")]
        public async Task DogAsync()
        {
            Utilities utilities = new Utilities(Context.Guild);
            Random r = new Random();
            int random = r.Next(1, 16);

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithImageUrl($"https://www.what-dog.net/Images/faces2/scroll00{random}.jpg");

            await ReplyAsync("", false, builder.Build());
            return;
        }

        [Command("avatar")]
        [Alias("dp")]
        public async Task AvatarAsync([Remainder] string arg = "")
        {
            Utilities utilities = new Utilities(Context.Guild);

            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            string avatar = target.GetAvatarUrl() ?? target.GetDefaultAvatarUrl();

            await ReplyAsync(Utilities.GetFormattedAlert("AVATAR_GET", target.Username, avatar));
        }

        [Command("test")]
        public async Task TestAsync()
        {
            Utilities utilities = new Utilities(Context.Guild);
            await ReplyAsync("");
        }
    }
}