using Discord;
using Discord.Commands;
using SharpLink;
using System.Linq;
using System.Threading.Tasks;

namespace GreenClover.Music
{
    public class MusicModules : ModuleBase<SocketCommandContext>
    {
        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync([Remainder] string song = "")
        {
            await AudioService.PlayAsync(Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel, song);
        }
    }
}