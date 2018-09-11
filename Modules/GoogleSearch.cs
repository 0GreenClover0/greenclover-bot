using Discord.Commands;
using System.Threading.Tasks;

namespace GreenClover.Modules
{
    public class GoogleSearch : ModuleBase<SocketCommandContext>
    {
        [Command("google")]
        [Alias("wygoogluj", "wyszukaj")]
        public async Task Google1Async([Remainder] string query)
        { 
            await Context.Channel.SendMessageAsync(Utilities.GetGoogleUrl(query));
            return;
        }
    }
}