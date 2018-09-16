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
                .WithAuthor(Context.Message.Author.Username, avatar)
                .WithDescription(Utilities.GetAlert("EGGPLANT_CONFIRM"))
                .WithFooter(Utilities.GetAlert("INTERACTIVE_CONFIRM_YES_OR_NO"));

            await ReplyAsync("", false, builderConfirm.Build());
            var response = await NextMessageAsync();

            if (response == null)
            {
                await ReplyAsync(Utilities.GetAlert("INTERACTIVE_TIMEOUT"));
                return;
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

            else if (answer == "nie")
            {
                await ReplyAsync(Utilities.GetAlert("EGGPLANT_NO"));
                return;
            }

            else
            {
                await ReplyAndDeleteAsync(Utilities.GetAlert("EGGPLANT_WRONG_ANSWER"), timeout: TimeSpan.FromSeconds(5));
                return;
            }
        }
    }
}