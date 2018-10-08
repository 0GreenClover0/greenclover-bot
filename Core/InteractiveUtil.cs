using Discord;
using Discord.WebSocket;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GreenClover.Core
{
    class InteractiveUtil
    {
        static InteractiveUtil()
        {

        }

        public static async Task<bool> CheckAnswerAsync(SocketGuild guild, SocketMessage response, ISocketMessageChannel channel)
        {
            Utilities utilities = new Utilities(guild);
            if (response == null) return true;

            string answer = response.ToString();
            answer = answer.ToLower();
            string[] wholeMsg = answer.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            wholeMsg[0] = Regex.Replace(wholeMsg[0], "[*]", string.Empty);

            if (GlobalVar.allCommandsEng.Contains(wholeMsg[0])
                || GlobalVar.allCommandsPl.Contains(wholeMsg[0])
                || answer.Contains("send") && answer.Contains("nudes")
                || answer.Contains("set") && answer.Contains("game"))
            {
                return true;
            }

            if (Utilities.GetAlert("answerTrue").Contains(wholeMsg[0]))
            {
                EmbedBuilder builderYes = new EmbedBuilder();
                builderYes
                    .WithImageUrl("https://cdn.discordapp.com/attachments/374222963999768578/469830447254339586/EggplantHand_Animated.gif");

                await channel.SendMessageAsync("", false, builderYes.Build());
                return true;
            }

            else if (Utilities.GetAlert("answerFalse").Contains(wholeMsg[0]))
            {
                await channel.SendMessageAsync(Utilities.GetAlert("EGGPLANT_NO"));
                return true;
            }

            else
                return false;
        }

        // Changing user's answer (from 1 to 10) from string to int
        public static int ConvertToInt(string answer)
        {
            int.TryParse(answer, out int response);
            if (int.TryParse(answer, out response) == false)
            {
                return 0;
            }

            else if (response < 1 || response > 10)
            {
                return 0;
            }

            else
            {
                return response;
            }
        }
    }
}
