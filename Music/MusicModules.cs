using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord.Addons.Interactive;

namespace GreenClover.Music
{
    public class MusicModules : InteractiveBase
    {
        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync([Remainder] string song = "")
        {
            await AudioService.PlayAsync(Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel, song, Context.Channel);
        }

        [Command("search", RunMode = RunMode.Async)]
        public async Task YoutubeAsync([Remainder] string query = "")
        {
            if (query == "")
            {
                await ReplyAsync("Czego mam szukać?");
                return;
            }

            var videos = AudioService.GetYoutubeAsync(query, Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel);
            string[] links = AudioService.GetYoutubeLinksAsync(query, Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel);
            string authorImgUrl = Context.Message.Author.GetAvatarUrl();

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithAuthor(Context.Message.Author.Username)
                .WithThumbnailUrl(authorImgUrl)
                .WithTitle(Utilities.GetAlert("YOUTUBE_FILMEMBED"))
                .WithDescription(String.Format("{0}", string.Join("\n", videos)))
                .WithColor(Color.Red);
            await ReplyAsync("", false, builder.Build());

            var response = await NextMessageAsync();
            string answer = response.ToString();
            int choose = Utilities.ConvertToInt(answer, Context.Channel);

            if (choose == 0)
            {
                return;
            }

            else
            {
                choose = choose - 1;
                string song = links[choose];
                await AudioService.PlayAsync(Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel, $"https://www.youtube.com/watch?v={song}", Context.Channel);
            }
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