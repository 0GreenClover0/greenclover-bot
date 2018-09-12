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
            string avatarUrl = Context.Client.CurrentUser.GetAvatarUrl();

            if (msg != "")
            {
                msg = msg.ToLower();
                string[] wholeMsg = msg.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string alertKey = wholeMsg[0].ToUpper();

                // Wyjątek dla send nudes, jedyna komenda składająca się z dwóch wyrazów
                if (msg.Contains("send") && msg.Contains("nudes"))
                {
                    wholeMsg[0] = "send nudes";
                    alertKey = "SENDNUDES";
                }

                if (GlobalVar.allCommandsEng.Contains(wholeMsg[0]))
                {
                    EmbedBuilder builderHelp = new EmbedBuilder();
                    builderHelp
                        .WithAuthor($"Komenda {wholeMsg[0]}", avatarUrl)
                        .AddField(" - Pomoc - ", "~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~")
                        .AddField($"[*{wholeMsg[0]}]", Utilities.GetAlert($"HELP_DESC{alertKey}"))
                        .WithColor(new Color(110, 80, 120));

                    await Context.Channel.SendMessageAsync("", false, builderHelp.Build());
                    return;
                }
            }

            EmbedBuilder builder = new EmbedBuilder();

            builder
                .WithAuthor("Paulinka", avatarUrl)
                .AddField(" - Pomoc - ", "~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~")
                .AddField("Komendy", Utilities.GetAlert("HELP_COMMANDS"))
                .AddField("Prefix", Utilities.GetAlert("HELP_PREFIX"))
                .AddField("Zabawa", Utilities.GetAlert("HELP_LISTFUN"))
                .AddField("Profil", Utilities.GetAlert("HELP_LISTPROFILE"))
                .AddField("Muzyka", Utilities.GetAlert("HELP_LISTAUDIO"))
                .AddField("Google", Utilities.GetAlert("HELP_LISTGOOGLE"))
                .AddField("Systemowe", Utilities.GetAlert("HELP_LISTSYSTEM"))
                .WithColor(new Color(90, 50, 165));

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}