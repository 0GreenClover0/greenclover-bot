using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using System;
using GreenClover.Core;
using GreenClover.Core.Accounts;
using System.Collections.Generic;
using System.Linq;

namespace GreenClover.Music
{
    public class MusicModules : InteractiveBase
    {
        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync([Remainder] string song = "")
        {
            Utilities utilities = new Utilities(Context.Guild);
            var guildAccount = GuildAccounts.GetGuildAccount(Context.Guild);
            string avatar = Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl();

            if (!song.Contains(".com") && song != "")
            {
                if ((Context.User as IVoiceState).VoiceChannel == null)
                {
                    await ReplyAsync(Utilities.GetAlert("PLAY_NULL_CHANNEL"));
                    return;
                }

                var searchList = AudioService.GetYoutubeAsync(song, Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel);
                var searchResult = searchList.Items[0];
                YoutubeVideo video = new YoutubeVideo();
                video.SetInfoVideo(Context.Guild, searchResult.Snippet.Description, searchResult.Snippet.Thumbnails.High.Url,
                    searchResult.Id.VideoId, searchResult.Snippet.Title);
                int choose = 0;

                await AudioService.PlayAsync(Context.Guild, (Context.User as IVoiceState).VoiceChannel, Context.Channel, $"https://www.youtube.com/watch?v={video.link[0]}");
                await AudioService.SongInfo(Context.Channel, video, Context.Message, video.link[0], choose);
                return;
            }

            await AudioService.PlayAsync(Context.Guild, (Context.User as IVoiceState).VoiceChannel, Context.Channel, song);
        }

        [Command("search", RunMode = RunMode.Async)]
        public async Task YoutubeAsync([Remainder] string query = "")
        {
            Utilities utilities = new Utilities(Context.Guild);
            string avatar = Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl();

            if (query == "")
            {
                await ReplyAsync(Utilities.GetAlert("PLAY_NULL_QUERY"));
                return;
            }

            var searchList = AudioService.GetYoutubeAsync(query, Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel);
            YoutubeVideo video = new YoutubeVideo();
            video.SetMultipleVideosInfo(Context.Guild, video, searchList);

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithAuthor(Context.Message.Author.Username, avatar)
                .WithThumbnailUrl("http://i65.tinypic.com/2uqk3yr.png")
                .WithTitle(Utilities.GetAlert("YOUTUBE_FILMEMBED"))
                .WithDescription($"{string.Join("\n", video.videosList)}")
                .WithColor(Color.Red);

            await ReplyAsync("", false, builder.Build());

            var response = await NextMessageAsync(true, true, timeout: TimeSpan.FromSeconds(30));
            string answer = response.ToString();
            string[] wholeMsg = answer.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int choose = InteractiveUtil.ConvertToInt(answer);

            if (choose == 0)
            {
                await ReplyAsync(Utilities.GetAlert("PLAY_WRONG_ANSWER"));
                return;
            }

            if (Utilities.GetAlert("answerCancel").Contains(wholeMsg[0]))
            {
                await ReplyAsync(Utilities.GetAlert("PLAY_CANCEL"));
                return;
            }

            if ((Context.User as IVoiceState).VoiceChannel == null)
            {
                await ReplyAsync(Utilities.GetAlert("PLAY_NULL_CHANNEL"));
                return;
            }

            choose = choose - 1;

            await AudioService.PlayAsync(Context.Guild, (Context.User as IVoiceState).VoiceChannel, Context.Channel, $"https://www.youtube.com/watch?v={video.link[choose]})");
            await AudioService.SongInfo(Context.Channel, video, Context.Message, video.link[choose], choose);
            return;
        }

        [Command("leave")]
        public async Task LeaveAsync()
        {
            await AudioService.LeaveAsync(Context.Guild);
        }

        [Command("stop")]
        public async Task StopAsync()
        {
            await AudioService.StopAsync(Context.Guild.Id);
        }

        [Command("skip")]
        public async Task SkipAsync()
        {
            var audioQueue = AudioQueues.GetAudioQueue(Context.Guild);
            string skippedTrackTitle = audioQueue.Queue[audioQueue.PlayingTrackIndex].Title;
            await AudioService.SkipAsync(Context.Guild);
            await ReplyAsync($":fast_forward: {skippedTrackTitle} skipped");
        }

        [Command("queue")]
        public async Task QueueAsync()
        {
            var audioQueue = AudioQueues.GetAudioQueue(Context.Guild);
            string avatar = Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl();

            if (audioQueue.Queue.ElementAtOrDefault(0) == null)
            {
                EmbedBuilder builderNull = new EmbedBuilder();
                builderNull
                    .WithAuthor(Context.Message.Author.Username, avatar)
                    .WithDescription("\n Kolejka jest pusta")
                    .WithColor(Color.DarkRed);

                await ReplyAsync("", false, builderNull.Build());
                return;
            }

            List<string> queue = new List<string>();
            List<List<string>> pagesContent = new List<List<string>>(5);

            queue = AudioService.QueueAsync(Context.Channel, audioQueue.Queue, Context.Message.Author.Username, avatar);
            pagesContent = AudioService.QueuePaging(queue);
            string[] pages = new string[5];
            pages = AudioService.QueueAddPages(pages, pagesContent);

            await PagedReplyAsync(pages);
        }
    }
}