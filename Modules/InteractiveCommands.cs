using System;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;

namespace GreenClover.Modules
{
    public class InteractiveCommands : InteractiveBase
    {
        [Command("eggplant", RunMode = RunMode.Async)]
        public async Task EggplantAsync()
        {
            string avatar = Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl();

            EmbedBuilder builderConfirm = new EmbedBuilder();
            builderConfirm
                .WithAuthor("GreenClover", avatar)
                .WithDescription("Jesteś pewien, że chcesz użyć tej komendy?")
                .WithFooter("Wpisz tak lub nie");

            await ReplyAsync("", false, builderConfirm.Build());
            var response = await NextMessageAsync();

            if (response == null)
            {
                await ReplyAsync("Słucham?");
            }

            string answer = response.ToString();
            answer = answer.ToLower();
            if (answer == "tak")
            {
                EmbedBuilder builderYes = new EmbedBuilder();
                builderYes
                    .WithImageUrl("https://cdn.discordapp.com/attachments/374222963999768578/469830447254339586/EggplantHand_Animated.gif");

                await ReplyAsync("", false, builderYes.Build());
                return;
            }

            if (answer == "nie")
            {
                await ReplyAsync("Dobry wybór");
                return;
            }

            else
            {
                await ReplyAndDeleteAsync("Nie rozumiem", timeout: TimeSpan.FromSeconds(5));
                return;
            }
        }
    }
}