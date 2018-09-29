using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using GreenClover.Core;

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

            DateTime timeStart = DateTime.Now;
            SocketMessage response = await NextMessageAsync(true, true, timeout: TimeSpan.FromSeconds(40));

            bool completed = await InteractiveUtil.CheckAnswerAsync(response, Context.Channel);

            if (completed == true)
            {
                return;
            }

            else
            {
                TimeSpan timePassed;
                do
                {
                    // In case that a bot doesn't receive a response within 30 seconds,
                    // it stops awaiting it and responseSecond takes the value null
                    var responseSecond = await NextMessageAsync(true, true, timeout: TimeSpan.FromSeconds(30));

                    if (responseSecond == null)
                    {
                        await ReplyAsync(Utilities.GetAlert("INTERACTIVE_TIMEOUT"));
                        return;
                    }

                    // SocketMessage to string
                    string answerSecond = responseSecond.ToString();
                    answerSecond = answerSecond.ToLower();

                    string[] wholeMsg = answerSecond.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    wholeMsg[0] = Regex.Replace(wholeMsg[0], "[*]", string.Empty);

                    if (answerSecond.Contains("send") && answerSecond.Contains("nudes")
                        || GlobalVar.allCommandsEng.Contains(wholeMsg[0])
                        || GlobalVar.allCommandsPl.Contains(wholeMsg[0]))
                    {
                        return;
                    }

                    if (answerSecond == Utilities.GetAlert("answerTrueEng")
                        || answerSecond == Utilities.GetAlert("answerTruePl"))
                    {
                        EmbedBuilder builderYes = new EmbedBuilder();
                        builderYes
                            .WithImageUrl("https://cdn.discordapp.com/attachments/374222963999768578/469830447254339586/EggplantHand_Animated.gif");

                        await ReplyAsync("", false, builderYes.Build());
                        return;
                    }

                    else if (answerSecond == Utilities.GetAlert("answerFalseEng")
                        || answerSecond == Utilities.GetAlert("answerFalsePl"))
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

                await ReplyAsync(Utilities.GetAlert("INTERACTIVE_TIMEOUT"));
                return;
            }
        }
    }
}