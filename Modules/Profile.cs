using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace GreenClover.Modules
{
    public class Profile : ModuleBase<SocketCommandContext>
    {
        [Command("profil")]
        public async Task ProfileAsync()
        {
            EmbedBuilder builder = new EmbedBuilder();
            
             builder
            .WithThumbnailUrl($"https://cdn.discordapp.com/avatars/{Context.User.Id}/{Context.User.AvatarId}.png")
            .WithDescription($"Profil użytkownika: **{Context.User.Username}**")
            .AddField("Opis:", $"test")
            .WithFooter("GreenClover Inc. © 2018")
            .WithColor(Color.Magenta);

            await Context.Channel.SendMessageAsync(" ", false, builder.Build());
        }
    }
}