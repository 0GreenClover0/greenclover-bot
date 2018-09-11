using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace GreenClover.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        // Czy return jest potrzebne i czy w ogóle coś robi na końcu komendy/funkcji?
        // W sensie dodałem na wszelki wypadek xd bo wcześniej działało bez
        // Prawdopodobnie nie jest potrzebne
        // avatar: Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl()

        [Command("hug")]
        [Alias("przytul")]
        public async Task HugAsync([Remainder]string arg = "")
        {
            string authorName = Context.User.Username;
            string key = "HUG_&AUTHORNAME_&TARGETID";

            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            if(target.Id == Context.Message.Author.Id)
                key = "HUG_&AUTHORNAME";

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithDescription(Utilities.GetFormattedAlert(key, authorName, target.Id))
                .WithImageUrl(Utilities.GetRandomLine("Texts/hugGif.txt"));

            await Context.Channel.SendMessageAsync("", false, builder.Build());
            return;
        }

        [Command("ping")]
        [Summary("Sprawdza czy wszystko jest ok")]
        public async Task PingAsync()
        {
            int latency = Context.Client.Latency;
            await Context.Channel.SendMessageAsync(Utilities.GetFormattedAlert("PING", latency));

            System.Collections.Generic.IReadOnlyCollection<Discord.Rest.RestInviteMetadata> invites = await Context.Guild.GetInvitesAsync();
            if (invites.Select(x => x.Url).FirstOrDefault() != null)
            {
                Console.WriteLine(invites);
                Console.WriteLine(invites.Select(x => x.Url).FirstOrDefault());
                return;
            }
            else
            {
                Console.WriteLine($"Na tym serwerze {Context.Guild.Id}, {Context.Guild.Name} nie ma żadnych zaproszeń");
                return;
            }
        }

        [Command("kim jestem")]
        public async Task WhoAmIAsync()
        {
            if (Context.User.Id == 375300562893275138)
                await Context.Channel.SendMessageAsync($"{Context.User.Username} jest najlepsza");

            else if (Context.User.Id == 259435378878971927)
                await Context.Channel.SendMessageAsync($"{Context.User.Username} to lamus!");

            else if (Context.User.Id == 371332977428398081)
                await Context.Channel.SendMessageAsync($"{Context.User.Username} jest najbogatszy!");
        }

        [Command("send nudes")]
        public async Task NudesAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithDescription(":womens: | **Wyślij sms'a o treści 69 na mój numer aby dostać więcej**")
                .WithImageUrl("http://fakty.dinoanimals.pl/wp-content/uploads/2014/04/Nosacz3.jpg")
                .WithColor(Color.DarkRed);

            await Context.Channel.SendMessageAsync("", false, builder.Build());
            return;
        }

        [Command("shop")]
        public async Task ShopAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithImageUrl("https://cdn.discordapp.com/attachments/412190473042657280/467989946599342080/bxKSjhxFL2U.png");

            await Context.Channel.SendMessageAsync("", false, builder.Build());
            return;
        }

        [Command("choose")]
        [Alias("wybierz")]
        public async Task ChooseAsync([Remainder]string message)
        {
            string[] options = message.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            Random r = new Random();
            string selection = options[r.Next(0, options.Length)];

            await Context.Channel.SendMessageAsync(Utilities.GetFormattedAlert("CHOOSE", selection));
            return;
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanAsync(IGuildUser user, string reason = "Nie podano powodu")
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;
            var dmChannel = await target.GetOrCreateDMChannelAsync();

            if (target.Id == 371332977428398081)
            {
                await dmChannel.SendMessageAsync(Utilities.GetFormattedAlert("BAN_USERMESSAGE",
                    Context.Guild.Name, Context.Message.Author.Username, "Nie banuje się Mistrza."));
                await user.Guild.AddBanAsync(Context.Message.Author, 0, reason);
                await Context.Channel.SendMessageAsync(Utilities.GetFormattedAlert("BAN", Context.Message.Author.Id));
                return;
            }

            else
            {
                await dmChannel.SendMessageAsync(Utilities.GetFormattedAlert("BAN_USERMESSAGE",
                    Context.Guild.Name, Context.Message.Author.Username, reason));
                await user.Guild.AddBanAsync(user, 0, reason);
                await Context.Channel.SendMessageAsync(Utilities.GetFormattedAlert("BAN", target.Id));
                return;
            }
        }

        [Command("version")]
        [Alias("wersja")]
        public async Task VersionAsync()
        {
            await Context.Channel.SendMessageAsync(Utilities.GetFormattedAlert("VERSION"));
            return;
        }

        [Command("game")]
        [Alias("gra")]
        public async Task GameAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithImageUrl(Utilities.GetRandomLine("Texts/gameGif.txt"));

            await Context.Channel.SendMessageAsync("", false, builder.Build());
            return;
        }

        [Command("cat")]
        [Alias("kot")]
        public async Task CatAsync()
        {
            Random r = new Random();
            int random = r.Next(1, 327);

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithImageUrl($"https://www.catgifpage.com/gifs/{random}.gif");

            await Context.Channel.SendMessageAsync("", false, builder.Build());
            return;
        }

        [Command("dog")]
        [Alias("pies")]
        public async Task DogAsync()
        {
            Random r = new Random();
            int random = r.Next(1, 16);

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithImageUrl($"https://www.what-dog.net/Images/faces2/scroll00{random}.jpg");

            await Context.Channel.SendMessageAsync("", false, builder.Build());
            return;
        }

        [Command("eggplant")]
        public async Task EggplantAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithImageUrl(Utilities.GetAlert("EGGPLANTIMG"));

            await Context.Channel.SendMessageAsync("", false, builder.Build());
            return;
        }

        [Command("google")]
        public async Task GoogleAsync([Remainder] string query = null)
        {
            if (query == null)
                await Context.Channel.SendMessageAsync("Słucham");
            await Context.Channel.SendMessageAsync(Utilities.GetGoogleUrl(query));
            return;
        }   

        [Command("test")]
        public async Task TestAsync()
        {
            await Context.Channel.SendMessageAsync("To testowa komenda");
        }
    }
}