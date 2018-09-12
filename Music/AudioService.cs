using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using SharpLink;

namespace GreenClover
{
    class AudioService
    {
        private static DiscordSocketClient _client = new DiscordSocketClient();
        public static LavalinkManager lavalinkManager = new LavalinkManager(_client, new LavalinkManagerConfig()
        {
            RESTHost = "localhost",
            RESTPort = 2333,
            WebSocketHost = "localhost",
            WebSocketPort = 80,
            Authorization = "youshallnotpass",
            TotalShards = 1,
            LogSeverity = LogSeverity.Verbose
        });

        public async static Task PlayAsync(ulong guildId, IVoiceChannel voiceChannel, string song)
        {
            LavalinkPlayer player = lavalinkManager.GetPlayer(guildId) ?? await lavalinkManager.JoinAsync(voiceChannel);
            LoadTracksResponse response = await lavalinkManager.GetTracksAsync(song);
            LavalinkTrack track = response.Tracks.First();
            await player.ResumeAsync();
            await player.PlayAsync(track);
        }

        public async static Task LeaveAsync(ulong guildId)
        {
            await lavalinkManager.LeaveAsync(guildId);
        }

        public async static Task StopAsync(ulong guildId)
        {
            LavalinkPlayer player = lavalinkManager.GetPlayer(guildId);
            await player.PauseAsync();
        }

        public async static Task LoopAsync(ulong guildId)
        {
            LavalinkPlayer player = lavalinkManager.GetPlayer(guildId);
            {
                LavalinkTrack track = player.CurrentTrack;
                await player.PlayAsync(track);
            } 
        }

        public static List<string> GetYoutubeAsync(string query, ulong guildId, IVoiceChannel voiceChannel)
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

            List<string> videos = new List<string>();
            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();

            // w pętli niżej można też użyć searchResult.Id.VideoId aby dostać id każdego filmu

            int count = 1;
            string videoId = "";
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format("`{0}`. {1} \n", count, searchResult.Snippet.Title));
                        videoId = videoId + searchResult.Id.VideoId + '.';
                        count++;
                        break;

                        /*
                    case "youtube#channel":
                        channels.Add(String.Format("{0}", searchResult.Snippet.Title));
                        break;

                    case "youtube#playlist":
                        playlists.Add(String.Format("{0}", searchResult.Snippet.Title));
                        break;
                        */
                        // Można rónież szukać kanałów i playlisty, jednak trzeba pozmieniać kilka rzeczy oprócz tych caseów wyżej
                        // a konkretnie SearchListRequest.Type = "video"; i moze coś jeszcze(?)
                }
            }
            string[] options = videoId.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            string selection = options[1];

            return videos;
        }
    }
}