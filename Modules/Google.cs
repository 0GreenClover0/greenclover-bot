using System.Threading.Tasks;
using Discord.Commands;

namespace GreenClover.Modules
{
    public class Google : ModuleBase<SocketCommandContext>
    {
        [Command("google")]
        [Alias("wygoogluj", "wyszukaj")]
        public async Task GoogleAsync([Remainder] string query = "")
        {
            if (query == "" || query == " ")
            {
                await Context.Channel.SendMessageAsync("Nie podano frazy do wyszukania");
            }
            await Context.Channel.SendMessageAsync(Utilities.GetGoogleUrl(query));
            return;
        }
    }
}
