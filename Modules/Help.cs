using Discord;
using Discord.Commands;
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

            if (msg == "ping")
            {
                EmbedBuilder builderPing = new EmbedBuilder();
                builderPing
                .WithAuthor("Komenda Ping", avatarUrl)
                .AddField(" - Pomoc - ", "~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~")
                .AddField("[*ping]", Utilities.GetAlert("HELP_PINGDESC"))
                .WithColor(new Color(90, 50, 165));

                await Context.Channel.SendMessageAsync("", false, builderPing.Build());
                return;
            }

            EmbedBuilder builder = new EmbedBuilder();

            builder
            .WithAuthor("Paulinka", avatarUrl)
            .AddField(" - Pomoc - ", "~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~")
            .AddField("Komendy", Utilities.GetAlert("HELP_COMMANDS"))
            .AddField("Prefix", Utilities.GetAlert("HELP_PREFIX"))
            .AddField("Zabawa", Utilities.GetAlert("HELP_FUN"))
            .AddField("Muzyka", Utilities.GetAlert("HELP_AUDIO"))
            .AddField("Google", Utilities.GetAlert("HELP_GOOGLE"))
            .AddField("Systemowe", Utilities.GetAlert("HELP_SYSTEM"))
            .WithColor(new Color(90, 50, 165));

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}