using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Console = Colorful.Console;

namespace GreenClover.Modules
{
    public class OwnerCommands : ModuleBase<SocketCommandContext>
    {
        [Command("set game")]
        public async Task SetBotGameAsync([Remainder] string game = "")
        {
            Utilities utilities = new Utilities(Context.Guild);

            if (Context.Message.Author.Id == Config.bot.ownerID)
            {
                await (Context.Client as DiscordSocketClient).SetGameAsync(game);
                await ReplyAsync(Utilities.GetFormattedAlert("OWNER_SETGAME_SUCCESS", game));
                Console.WriteLine($"{DateTime.Now} Game was changed to: {game}", System.Drawing.Color.FloralWhite);
            }

            else
            {
                await ReplyAsync(Utilities.GetAlert("OWNER_SETGAME_FAIL"));
            }
        }
    }
}