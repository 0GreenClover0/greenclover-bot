using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace GreenClover.Modules
{
    public class Google : ModuleBase<SocketCommandContext>
    {
        [Command("google")]
        [Alias("wygoogluj", "wyszukaj")]
        public async Task GoogleAsync([Remainder] string query = "")
        {
            if (query == "")
            {
                await Context.Channel.SendMessageAsync("Nie podano frazy do wyszukania");
                return;
            }

            await Context.Channel.SendMessageAsync(GoogleService.GetGoogleUrl(query));
            return;
        }

        [Command("image")]
        public async Task GoogleImageAsync([Remainder] string query = "")
        {
            if (query == "")
            {
                await Context.Channel.SendMessageAsync("Nie podano frazy do wyszukania");
                return;
            }

            string googleImage = GoogleService.GetGoogleImage(query);

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithImageUrl(googleImage);

            await Context.Channel.SendMessageAsync("", false, builder.Build());
        }
    }
}
