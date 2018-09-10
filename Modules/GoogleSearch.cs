using Discord.Commands;
using System.Threading.Tasks;

namespace GreenClover.Modules
{
    public class GoogleSearch : ModuleBase<SocketCommandContext>
    {
        [Command("google")]
        public async Task Google1Async([Remainder] string query)
        {
            
            await Context.Channel.SendMessageAsync(Utilities.GetGoogleUrl(query));
            return;
        }

        [Command("wygoogluj")]
        public async Task Google2Async([Remainder] string query)
        {
            await Context.Channel.SendMessageAsync(Utilities.GetGoogleUrl(query));
            return;
        }

        [Command("wyszukaj")]
        public async Task Google3Async([Remainder] string query)
        {
            await Context.Channel.SendMessageAsync(Utilities.GetGoogleUrl(query));
            return;
        }
    }
}
