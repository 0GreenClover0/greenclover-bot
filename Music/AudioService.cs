using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using SharpLink;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using GreenClover.Core.Accounts;

namespace GreenClover.Music
{
    class AudioService
    {
        private static readonly DiscordSocketClient _client = new DiscordSocketClient();
        public static LavalinkManager lavalinkManager = new LavalinkManager(_client);

        public static async Task PlayAsync(SocketCommandContext context, string song, int choose, YoutubeVideo video = null)
        {
            // Create used objects
            SocketGuild guild = context.Guild;
            SocketUserMessage message = context.Message;
            IVoiceChannel voiceChannel = (context.User as IVoiceState).VoiceChannel;
            ISocketMessageChannel channel = context.Channel;
            Utilities utilities = new Utilities(guild);

            // Checking if voice channel is null (and sending an error message)
            // If not, creating or getting a lavalink player
            // Checking if a given string is empty (if true, and there is a song in queue that is stopped, resuming it
            if (await VoiceChannelNull(channel, voiceChannel, utilities) is true) return;
            LavalinkPlayer player = lavalinkManager.GetPlayer(guild.Id) ?? await lavalinkManager.JoinAsync(voiceChannel);
            var audioQueue = AudioQueues.GetAudioQueue(guild);
            if (await CheckIfSongIsEmpty(channel, utilities, player, audioQueue, song) is true) return;

            LoadTracksResponse response = await lavalinkManager.GetTracksAsync(song);
            LavalinkTrack track = response.Tracks.First();

            // Maximum songs in queue is 50
            if (await CheckIfQueueIsFull(channel, utilities, audioQueue.Queue.Count) is true) return;

            // Adding a track to queue
            audioQueue.Queue = AudioQueues.GetOrCreateGuildQueue(track, audioQueue);
            LavalinkTrack secondTrack = audioQueue.Queue.ElementAtOrDefault(1);

            // A check if a song is first in the queue, or if it's been added
            string songAlert = "PLAY_ADDED_SONG";
            if (secondTrack == null)
            {
                audioQueue.PlayingTrackIndex = 0;
                AudioQueues.SaveQueues();

                await player.PlayAsync(track);
                if (video != null)
                {
                    songAlert = "PLAY_PLAYED_SONG";
                    await SongInfo(channel, message, video, choose, songAlert);
                }
                return;
            }

            // If a user gives a link to a youtube video, we don't need to send song info
            if (choose != -1)
            {
                await SongInfo(channel, message, video, choose, songAlert);
            }
        }

        private static async Task<bool> VoiceChannelNull(ISocketMessageChannel channel, IVoiceChannel voiceChannel, Utilities utilities)
        {
            if (voiceChannel == null)
            {
                await channel.SendMessageAsync(Utilities.GetAlert("PLAY_NULL_CHANNEL"));
                return true;
            }

            return false;
        }

        private static async Task<bool> CheckIfSongIsEmpty(ISocketMessageChannel channel, Utilities utilities, LavalinkPlayer player, AudioQueue audioQueue, string song)
        {
            if (song == "" && player.Playing == true)
            {
                return true;
            }

            else if (song == "" & player.Playing == false)
            {
                if (player.CurrentTrack == null)
                {
                    await channel.SendMessageAsync(Utilities.GetAlert("PLAY_NULL_LINK"));
                    return true;
                }

                if (audioQueue.Queue.FirstOrDefault() != null)
                {
                    await player.ResumeAsync();
                }

                return true;
            }

            return false;
        }

        private static async Task<bool> CheckIfQueueIsFull(ISocketMessageChannel channel, Utilities utilities, int queueCount)
        {
            if (queueCount > 50)
            {
                await channel.SendMessageAsync(Utilities.GetAlert("QUEUE_OVERLOADED"));
                return true;
            }

            return false;
        }

        private static async Task SongInfo(ISocketMessageChannel channel, SocketUserMessage message,YoutubeVideo video, int choose, string playOrAdded)
        {
            string avatar = message.Author.GetAvatarUrl() ?? message.Author.GetDefaultAvatarUrl();

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithAuthor(message.Author.Username, avatar)
                .WithThumbnailUrl(video.image[choose])
                .AddField(Utilities.GetAlert(playOrAdded), $"[{video.title[choose]}](https://www.youtube.com/watch?v={video.link[choose]})")
                .AddField(Utilities.GetAlert("PLAY_VIDEO_DESC"), video.desc[choose])
                .WithColor(Color.DarkRed);

            await channel.SendMessageAsync("", false, builder.Build());
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