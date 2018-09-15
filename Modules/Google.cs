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
                await ReplyAsync("Nie podano frazy do wyszukania");
                return;
            }

            await ReplyAsync(GoogleService.GetGoogleUrl(query));
            return;
        }

        [Command("image")]
        [Alias("zdjęcie", "obraz")]
        public async Task GoogleImageAsync([Remainder] string query = "")
        {
            if (query == "")
            {
                await ReplyAsync("Nie podano frazy do wyszukania");
                return;
            }

            string googleImage = GoogleService.GetGoogleImage(query);

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithImageUrl(googleImage);

            await ReplyAsync("", false, builder.Build());
        }
    }
}