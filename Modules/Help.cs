using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace GreenClover.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Alias("pomoc")]
        public async Task HelpAsync([Remainder] string msg = "")
        {
            string avatar = Context.Client.CurrentUser.GetAvatarUrl() ?? Context.Client.CurrentUser.GetDefaultAvatarUrl();

            if (msg != "")
            {
                // Pobieranie pierwszego wyrazu po komendzie. Tworzony jest {alertkey}, który jest potrzebny do pobrania
                // opisu danej komendy
                msg = msg.ToLower();
                string[] wholeMsg = msg.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string alertKey = wholeMsg[0].ToUpper();

                // Wyjątek dla send nudes, jedyna komenda składająca się z dwóch wyrazów
                if (msg.Contains("send") && msg.Contains("nudes") && !GlobalVar.allCommandsEng.Contains(wholeMsg[0]))
                {
                    wholeMsg[0] = "send nudes";
                    alertKey = "SENDNUDES";
                }

                // Pobierany jest jego opis, znajdujący się w pliku json'a pod tą samą nazwą
                // w postaci HELP_DESC_{alertKey}
                // Jeśli wyraz ten znajduje się na liście angielskich komend tworzona jest wiadomość jej opisem
                // Jeśli nie bot wysyła zwykłą listę komend
                if (GlobalVar.allCommandsEng.Contains(wholeMsg[0]) || wholeMsg[0] == "send nudes")
                {
                    EmbedBuilder builderHelp = new EmbedBuilder();
                    builderHelp
                        .WithAuthor(Utilities.GetFormattedAlert("HELP_SPECIFIC_COMMAND", wholeMsg[0]), avatar)
                        .AddField(Utilities.GetAlert("HELP_TEXT"), "~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~")
                        .AddField($"[*{wholeMsg[0]}]", Utilities.GetAlert($"HELP_DESC_{alertKey}"))
                        .WithColor(new Color(110, 80, 120));

                    await ReplyAsync("", false, builderHelp.Build());
                    return;
                }
            }

            EmbedBuilder builder = new EmbedBuilder();

            builder
                .WithAuthor(Utilities.GetAlert("BOT_NAME"), avatar)
                .AddField(Utilities.GetAlert("HELP_TEXT"), "~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~")
                .AddField(Utilities.GetAlert("HELP_CAT_COMMANDS"), Utilities.GetAlert("HELP_DESC_COMMANDS"))
                .AddField(Utilities.GetAlert("HELP_CAT_PREFIX"), Utilities.GetAlert("HELP_DESC_PREFIX"))
                .AddField(Utilities.GetAlert("HELP_CAT_FUN"), Utilities.GetAlert("HELP_LIST_FUN"))
                .AddField(Utilities.GetAlert("HELP_CAT_PROFILE"), Utilities.GetAlert("HELP_LIST_PROFILE"))
                .AddField(Utilities.GetAlert("HELP_CAT_AUDIO"), Utilities.GetAlert("HELP_LIST_AUDIO"))
                .AddField(Utilities.GetAlert("HELP_CAT_MOD"), Utilities.GetAlert("HELP_LIST_MOD"))
                .AddField(Utilities.GetAlert("HELP_CAT_GOOGLE"), Utilities.GetAlert("HELP_LIST_GOOGLE"))
                .AddField(Utilities.GetAlert("HELP_CAT_SYSTEM"), Utilities.GetAlert("HELP_LIST_SYSTEM"))
                .WithColor(new Color(90, 50, 165));

            await ReplyAsync("", false, builder.Build());
        }
    }
}