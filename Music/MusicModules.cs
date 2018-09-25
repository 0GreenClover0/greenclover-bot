using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using System.Collections.Generic;

namespace GreenClover.Music
{
    public class MusicModules : InteractiveBase
    {
        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync([Remainder] string song = "")
        {
            string avatar = Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl();

            if (!song.Contains(".com") && song != "")
            {
                var searchList = AudioService.GetYoutubeAsync(song, Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel);
                Google.Apis.YouTube.v3.Data.SearchResult searchResult = searchList.Items[0];
                string videoLink = searchResult.Id.VideoId;
                string videoTitle = searchResult.Snippet.Title;
                string videoDesc = searchResult.Snippet.Description;
                string videoImage = searchResult.Snippet.Thumbnails.High.Url;

                EmbedBuilder builderPlay = new EmbedBuilder();
                builderPlay
                    .WithAuthor(Context.Message.Author.Username, avatar)
                    .WithThumbnailUrl(videoImage)
                    .WithDescription(Utilities.GetAlert("PLAY_PLAYED_SONG") + $"[{videoTitle}]({Utilities.GetAlert("PLAY_YOUTUBE_LINK")}{videoLink})")
                    .AddField("Opis:", videoDesc)
                    .WithColor(Color.DarkRed);

                await ReplyAsync("", false, builderPlay.Build());
                await AudioService.PlayAsync(Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel, $"{Utilities.GetAlert("PLAY_YOUTUBE_LINK")}{videoLink})", Context.Channel);
                return;
            }

            await AudioService.PlayAsync(Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel, song, Context.Channel);
        }

        [Command("search", RunMode = RunMode.Async)]
        public async Task YoutubeAsync([Remainder] string query = "")
        {
            string avatar = Context.Message.Author.GetAvatarUrl() ?? Context.Message.Author.GetDefaultAvatarUrl();

            if (query == "")
            {
                await ReplyAsync(Utilities.GetAlert("PLAY_NULL_QUERY"));
                return;
            }

            var searchList = AudioService.GetYoutubeAsync(query, Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel);
            string[] videoLinks = new string[10];
            string[] videoTitles = new string[10];
            string[] videoDescs = new string[10];
            string[] videoImages = new string[10];
            List<string> videos = new List<string>();

            int count = 1;
            int i = 0;
            foreach (var searchResult in searchList.Items)
            {
                videos.Add(String.Format("{0}. {1} \n", count, searchResult.Snippet.Title));
                videoLinks[i] = searchResult.Id.VideoId;
                videoTitles[i] = searchResult.Snippet.Title;
                videoDescs[i] = searchResult.Snippet.Description;
                videoImages[i] = searchResult.Snippet.Thumbnails.High.Url;
                count++;
                i++;
            }

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithAuthor(Context.Message.Author.Username, avatar)
                .WithThumbnailUrl("http://i65.tinypic.com/2uqk3yr.png")
                .WithTitle(Utilities.GetAlert("YOUTUBE_FILMEMBED"))
                .WithDescription(String.Format("{0}", string.Join("\n", videos)))
                .WithColor(Color.Red);

            await ReplyAsync("", false, builder.Build());

            var response = await NextMessageAsync();
            string answer = response.ToString();
            int choose = Utilities.ConvertToInt(answer);

            if (choose == 0)
            {
                await ReplyAsync(Utilities.GetAlert("PLAY_WRONG_ANSWER"));
                return;
            }

            if (answer == "cancel" || answer == "anuluj")
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
            string song = videoLinks[choose];

            if (videoDescs[choose] == null || videoDescs[choose] == "")
            {
                videoDescs[choose] = Utilities.GetAlert("PLAY_NULL_DESC");
            }

            EmbedBuilder builderPlay = new EmbedBuilder();
            builderPlay
                .WithAuthor(Context.Message.Author.Username, avatar)
                .WithThumbnailUrl(videoImages[choose])
                .WithDescription(Utilities.GetAlert("PLAY_PLAYED_SONG") + $"[{videoTitles[choose]}]({Utilities.GetAlert("PLAY_YOUTUBE_LINK")}{song})")
                .AddField("Opis:", videoDescs[choose])
                .WithColor(Color.DarkRed);

            await ReplyAsync("", false, builderPlay.Build());
            await AudioService.PlayAsync(Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel, $"{Utilities.GetAlert("PLAY_YOUTUBE_LINK")}{song})", Context.Channel);
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