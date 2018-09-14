using Discord;
using Discord.Commands;
using SharpLink;
using System;
using System.Threading.Tasks;

namespace GreenClover.Music
{
    public class MusicModules : ModuleBase<SocketCommandContext>
    {
        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync([Remainder] string song = "")
        {
            await AudioService.PlayAsync(Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel, song, Context.Channel);
        }

        [Command("search")]
        public async Task YoutubeAsync([Remainder] string query = "")
        {
            if (query == "")
            {
                await Context.Channel.SendMessageAsync("Czego mam szukać?");
                return;
            }

            var videos = AudioService.GetYoutubeAsync(query, Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel);
            string authorImgUrl = Context.Message.Author.GetAvatarUrl();

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithAuthor(Context.Message.Author.Username)
                .WithThumbnailUrl(authorImgUrl)
                .WithTitle(Utilities.GetAlert("YOUTUBE_FILMEMBED"))
                .WithDescription(String.Format("{0}", string.Join("\n", videos)))
                .WithColor(Color.Red);
            await Context.Channel.SendMessageAsync("", false, builder.Build());
            return;
        }

        [Command("leave")]
        public async Task LeaveAsync()
        {
            await AudioService.LeaveAsync(Context.Guild.Id);
        }

        [Command("stop")]
        public async Task StopAsync()
        {
            await AudioService.StopAsync(Context.Guild.Id);
        }
    }
}