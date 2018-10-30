using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using GreenClover.Core.Accounts;
using SharpLink;

namespace GreenClover.Music
{
    class AudioService
    {
        private static readonly DiscordSocketClient _client = new DiscordSocketClient();
        public static LavalinkManager lavalinkManager = new LavalinkManager(_client);

        public static async Task PlayAsync(SocketGuild guild, IVoiceChannel voiceChannel, ISocketMessageChannel channel, string song)
        {
            Utilities utilities = new Utilities(guild);
            var audioQueue = AudioQueues.GetAudioQueue(guild);

            if (voiceChannel == null)
            {
                await channel.SendMessageAsync(Utilities.GetAlert("PLAY_NULL_CHANNEL"));
                return;
            }

            LavalinkPlayer player = lavalinkManager.GetPlayer(guild.Id) ?? await lavalinkManager.JoinAsync(voiceChannel);

            if (song == "" && player.Playing == true)
            {
                return;
            }

            else if (song == "" & player.Playing == false)
            {
                if (player.CurrentTrack == null)
                {
                    await channel.SendMessageAsync(Utilities.GetAlert("PLAY_NULL_LINK"));
                    return;
                }
                await player.ResumeAsync();
                return;
            }

            LoadTracksResponse response = await lavalinkManager.GetTracksAsync(song);
            LavalinkTrack track = response.Tracks.First();

            if (audioQueue.Queue.Count > 50)
            {
                await channel.SendMessageAsync(Utilities.GetAlert("QUEUE_OVERLOADED"));
                return;
            }

            audioQueue.Queue = AudioQueues.GetOrCreateGuildQueue(track, audioQueue);
            LavalinkTrack isFirst = audioQueue.Queue.ElementAtOrDefault(1);

            if (isFirst == null)
            {
                audioQueue.PlayingTrackIndex = 0;
                AudioQueues.SaveQueues();
                await player.PlayAsync(track);
                return;
            }
        }

        public static async Task LeaveAsync(SocketGuild guild)
        {
            LavalinkPlayer player = lavalinkManager.GetPlayer(guild.Id);
            if (player == null) return;

            await lavalinkManager.LeaveAsync(guild.Id);
            await RemoveAllTracks(guild);
        }

        private static async Task RemoveAllTracks(SocketGuild guild)
        {
            var audioQueue = AudioQueues.GetAudioQueue(guild);
            audioQueue.Queue.Clear();
            AudioQueues.SaveQueues();
            await Task.CompletedTask;
        }

        public static async Task StopAsync(ulong guildId)
        {
            LavalinkPlayer player = lavalinkManager.GetPlayer(guildId);
            if (player.Playing == false) return;

            await player.PauseAsync();
        }

        public static async Task SkipAsync(SocketGuild guild)
        {
            LavalinkPlayer player = lavalinkManager.GetPlayer(guild.Id);
            await AudioQueuesManagment.RemoveAndPlay(player, player.CurrentTrack);
            return;
        }

        public static Google.Apis.YouTube.v3.Data.SearchListResponse GetYoutubeAsync(string query, ulong guildId, IVoiceChannel voiceChannel)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = Config.bot.apiKey,
                ApplicationName = "DiscordBot"
            });
            var SearchListRequest = youtubeService.Search.List("snippet");
            SearchListRequest.Q = query;
            SearchListRequest.Type = "video";
            SearchListRequest.MaxResults = 10;

            var searchListResponse = SearchListRequest.Execute();
            return searchListResponse;
        }

        public static List<string> QueueAsync(ISocketMessageChannel channel, List<LavalinkTrack> queue, string username, string avatar)
        {
            YoutubeVideo video = new YoutubeVideo();
            int count = 1;
            //int i = 0;

            foreach (var track in queue)
            {
                video.videosList.Add($"{count}. [{track.Title}]({track.Url}) `{track.Length}` \n");
                //video.link[i] = track.Url;
                //video.title[i] = track.Title;
                count++;
                //i++;
            }

            return video.videosList;
        }

        public static List<List<string>> QueuePaging(List<string> videoList)
        {
            List<List<string>> pages = new List<List<string>>();

            if (videoList.Count < 11)
            {
                pages.Add(videoList.GetRange(0, videoList.Count));
            }

            else if (videoList.Count > 10 && videoList.Count < 21)
            {
                pages.Add(videoList.GetRange(0, 10));
                pages.Add(videoList.GetRange(10, videoList.Count - 10));
            }

            else if (videoList.Count > 20 && videoList.Count < 31)
            {
                pages.Add(videoList.GetRange(0, 10));
                pages.Add(videoList.GetRange(10, 10));
                pages.Add(videoList.GetRange(20, videoList.Count - 20));
            }

            else if (videoList.Count > 30 && videoList.Count < 41)
            {
                pages.Add(videoList.GetRange(0, 10));
                pages.Add(videoList.GetRange(10, 10));
                pages.Add(videoList.GetRange(20, 10));
                pages.Add(videoList.GetRange(30, videoList.Count - 30));
            }

            else if (videoList.Count > 40)
            {
                pages.Add(videoList.GetRange(0, 10));
                pages.Add(videoList.GetRange(10, 10));
                pages.Add(videoList.GetRange(20, 10));
                pages.Add(videoList.GetRange(30, 10));
                pages.Add(videoList.GetRange(40, videoList.Count - 40));
            }

            return pages;
        }

        public static string[] QueueAddPages(string[] pages, List<List<string>> pagesContent)
        {
            pages[0] = string.Join("\n", pagesContent[0].ToArray());

            if (pagesContent.Count > 1)
            {
                pages[1] = string.Join("\n", pagesContent[1].ToArray());
            }

            if (pagesContent.Count > 2)
            {
                pages[2] = string.Join("\n", pagesContent[2].ToArray());
            }

            if (pagesContent.Count > 3)
            {
                pages[3] = string.Join("\n", pagesContent[3].ToArray());
            }

            if (pagesContent.Count > 4)
            {
                pages[4] = string.Join("\n", pagesContent[4].ToArray());
            }

            return pages;
        }
    }
}