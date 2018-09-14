﻿using System;
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

            if (target.Id == Context.Message.Author.Id)
            {
                key = "HUG_&AUTHORNAME";
            }

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
            return;
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

        [Command("choose")]
        [Alias("wybierz")]
        public async Task ChooseAsync([Remainder]string message = "")
        {
            if (message == "")
            {
                await Context.Channel.SendMessageAsync("Zbyt mały wybór");
            }
            string[] options = message.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            Random r = new Random();
            string selection = options[r.Next(0, options.Length)];

            await Context.Channel.SendMessageAsync(Utilities.GetFormattedAlert("CHOOSE", selection));
            return;
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanAsync(IGuildUser user, [Remainder] string reason = "Nie podano powodu")
        {
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            if (mentionedUser == null)
            {
                await Context.Channel.SendMessageAsync("Nie oznaczono użytkownika do zbanowania");
                return;
            }

            SocketUser target = null;
            target = mentionedUser ?? Context.User;
            var dmChannel = await target.GetOrCreateDMChannelAsync();
            if (target.IsBot == true)
            {
                await user.Guild.AddBanAsync(user, 0, reason);
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
                .WithImageUrl("https://cdn.discordapp.com/attachments/374222963999768578/469830447254339586/EggplantHand_Animated.gif");

            await Context.Channel.SendMessageAsync("", false, builder.Build());
            return;
        }

        [Command("test")]
        public async Task TestAsync()
        {
            await Context.Channel.SendMessageAsync("");
        }
    }
}