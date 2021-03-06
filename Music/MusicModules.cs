﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using GreenClover.Core;
using GreenClover.Core.Accounts;

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
            int choose = -1;

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
                choose = 0;
                song = $"https://www.youtube.com/watch?v={video.link[choose]}";

                await AudioService.PlayAsync(Context, song, choose, video);
                return;
            }

            await AudioService.PlayAsync(Context, song, 0);
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

            if (Utilities.GetAlert("answerCancel").Contains(wholeMsg[0]))
            {
                await ReplyAsync(Utilities.GetAlert("PLAY_CANCEL"));
                return;
            }
            if (choose == 0) return;

            choose = choose - 1;
            string song = $"https://www.youtube.com/watch?v={video.link[choose]}";
            await AudioService.PlayAsync(Context, song, choose, video);
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

            queue = AudioQueuesManagment.CreateListOfSongs(Context.Channel, audioQueue.Queue, Context.Message.Author.Username, avatar);
            pagesContent = AudioQueuesManagment.CreateListOfPages(queue);
            string[] pages = new string[5];
            pages = AudioQueuesManagment.AssignContentToPages(pages, pagesContent);

            await PagedReplyAsync(pages);
        }
    }
}