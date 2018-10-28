using System;
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

            audioQueue.Queue = AudioQueues.GetOrCreateGuildQueue(track, audioQueue);
            LavalinkTrack isFirst = audioQueue.Queue.ElementAtOrDefault(1);

            if (isFirst == null)
            {
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

        public static async Task QueueAsync(ISocketMessageChannel channel, List<LavalinkTrack> queue, string username, string avatar)
        {
            YoutubeVideo video = new YoutubeVideo();
            int count = 1;
            int i = 0;

            foreach (var track in queue)
            {
                video.videosList.Add($"{count}. {track.Title} `{track.Length}` \n");
                video.link[i] = track.Url;
                video.title[i] = track.Title;
                count++;
                i++;
            }

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithAuthor(username, avatar)
                .WithTitle(Utilities.GetAlert("YOUTUBE_FILMEMBED_QUEUE"))
                .WithDescription($"{string.Join("\n", video.videosList.Take(10))}")
                .WithColor(Color.Red);

            await channel.SendMessageAsync("", false, builder.Build());
        }
    }
}