using System;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using GreenClover.src;

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

            SocketMessage response = await NextMessageAsync(true, true, timeout: TimeSpan.FromSeconds(120));
            DateTime timeStart = DateTime.Now;

            bool answered = await InteractiveUtil.CheckAnswerAsync(response, Context.Channel);

            if (answered == true)
            {
                return;
            }

            else
            {
                TimeSpan timePassed;
                do
                {
                    // W przypadku braku odpowiedzi w ciągu {30 sekund} bot przestanie jej oczekiwać
                    // i responseSecond przyjmie wartość null
                    var responseSecond = await NextMessageAsync(true, true, timeout: TimeSpan.FromSeconds(30));

                    if (responseSecond == null)
                    {
                        await ReplyAsync("Czas na odpowiedź upłynął)");
                        return;
                    }

                    // Przekształcenie SocketMessage na string
                    string answerSecond = responseSecond.ToString();
                    answerSecond = answerSecond.ToLower();

                    if (answerSecond == "tak")
                    {
                        EmbedBuilder builderYes = new EmbedBuilder();
                        builderYes
                            .WithImageUrl("https://cdn.discordapp.com/attachments/374222963999768578/469830447254339586/EggplantHand_Animated.gif");

                        await ReplyAsync("", false, builderYes.Build());
                        return;
                    }

                    else if (answerSecond == "nie")
                    {
                        await ReplyAsync(Utilities.GetAlert("EGGPLANT_NO"));
                        return;
                    }

                    else
                    {
                        DateTime timeEnd = DateTime.Now;
                        timePassed = timeEnd - timeStart;
                    }

                } while (timePassed.TotalSeconds < 30);

                await ReplyAsync("Czas na odpowiedź upłynął");
            }
        }
    }
}