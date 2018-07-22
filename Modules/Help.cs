using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace GreenClover.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("pomoc")]
        public async Task HelpAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder
            .AddField("- GreenClover - pomoc", "~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~")
            .AddField("Komendy", Utilities.GetAlert("HELP_COMMANDS"))
            .AddField("Prefix", Utilities.GetAlert("HELP_PREFIX"))
            .AddField("[*ping]", Utilities.GetAlert("HELP_PINGDESC"))
            .AddField("[*profil]", Utilities.GetAlert("HELP_PROFILEDESC"))
            .AddField("[*przytul]", Utilities.GetAlert("HELP_HUGDESC"))
            .AddField("[*wybierz]", Utilities.GetAlert("HELP_CHOOSEDESC"))
            .AddField("[*gra]", Utilities.GetAlert("HELP_GAMEDESC"))
            .AddField("[*kot]", Utilities.GetAlert("HELP_CATDESC"))
            .AddField("[*pies]", Utilities.GetAlert("HELP_DOGDESC"))
            .AddField("[*google]", Utilities.GetAlert("HELP_GOOGLEDESC"))
            .AddField("[*szukaj]", Utilities.GetAlert("HELP_YOUTUBEDESC"))
            .WithColor(new Color(90, 50, 165));

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}
