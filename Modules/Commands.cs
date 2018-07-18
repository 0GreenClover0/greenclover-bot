﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;
using NReco.ImageGenerator;
using System.Net;
using Newtonsoft.Json;

namespace GreenClover.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        Random rnd = new Random();

        [Command("przytul")]
        public async Task HugAsync([Remainder]string arg = "")
        {
            string authorName = Context.User.Username;

            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            EmbedBuilder builder = new EmbedBuilder()
            .WithDescription(Utilities.GetFormattedAlert("HUG_&AUTHORNAME_&TARGETID", authorName, target.Id))
            .WithImageUrl(Utilities.GetAlert("HUG_IMAGE1"));

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
        
        [Command("ping")]
        [Summary("Sprawdza czy wszystko jest ok")]
        public async Task PingAsync()
        {
            int latency = Context.Client.Latency;
            await Context.Channel.SendMessageAsync(Utilities.GetFormattedAlert("PING", latency));
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
        }   

        [Command("witaj")]
        public async Task HelloAsync()
        {
            int randChoose = rnd.Next(1, 4);

            if (randChoose == 1)
                await Context.Channel.SendMessageAsync("witaj");

            if (randChoose == 2)
                await Context.Channel.SendMessageAsync("cześć");

            if (randChoose == 3)
                await Context.Channel.SendMessageAsync("siema");
        }

        [Command("shop")]
        public async Task ShopAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithImageUrl("https://cdn.discordapp.com/attachments/412190473042657280/467989946599342080/bxKSjhxFL2U.png");

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }

        [Command("wybierz")]
        public async Task ChooseAsync([Remainder]string message)
        {
            string[] options = message.Split(new char[] {'|'}, StringSplitOptions.RemoveEmptyEntries);

            Random random = new Random();
            string selection = options[random.Next(0, options.Length)];

            await Context.Channel.SendMessageAsync(Utilities.GetFormattedAlert("CHOOSE", selection));
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]  
        public async Task CovertAsync(IGuildUser user, string reason = "Powód nie został podany"    )
        {
            SocketUser target = null;
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
            target = mentionedUser ?? Context.User;

            await user.Guild.AddBanAsync(user, 0, reason);
            await Context.Channel.SendMessageAsync($"Użytkownik <@{target.Id}> został zbanowany.");
        }

        [Command("version")]
        public async Task VersionAsync()
        {
            await Context.Channel.SendMessageAsync(Utilities.GetAlert("VERSION"));
        }
    }
}