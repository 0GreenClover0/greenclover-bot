using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace GreenClover.src
{
    class InteractiveUtil
    {
        static InteractiveUtil()
        {

        }

        public static async Task<bool> CheckAnswerAsync(SocketMessage response, ISocketMessageChannel channel)
        {
            if (response == null)
            {
                return true;
            }

            string answer = response.ToString();
            answer = answer.ToLower();

            if (answer == "tak")
            {
                EmbedBuilder builderYes = new EmbedBuilder();
                builderYes
                    .WithImageUrl("https://cdn.discordapp.com/attachments/374222963999768578/469830447254339586/EggplantHand_Animated.gif");

                await channel.SendMessageAsync("", false, builderYes.Build());
                return true;
            }

            else if (answer == "nie")
            {
                await channel.SendMessageAsync(Utilities.GetAlert("EGGPLANT_NO"));
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
