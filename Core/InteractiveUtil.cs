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
            int response = 0;
            if (answer == "1")
            {
                response = 1;
                return response;
            }

            if (answer == "2")
            {
                response = 2;
                return response;
            }

            if (answer == "3")
            {
                response = 3;
                return response;
            }

            if (answer == "4")
            {
                response = 4;
                return response;
            }

            if (answer == "5")
            {
                response = 5;
                return response;
            }

            if (answer == "6")
            {
                response = 6;
                return response;
            }

            if (answer == "7")
            {
                response = 7;
                return response;
            }

            if (answer == "8")
            {
                response = 8;
                return response;
            }

            if (answer == "9")
            {
                response = 9;
                return response;
            }

            if (answer == "10")
            {
                response = 10;
                return response;
            }

            else
                return 0;
        }
    }
}
