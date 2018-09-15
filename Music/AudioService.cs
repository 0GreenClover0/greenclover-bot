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

        public async static Task PlayAsync(ulong guildId, IVoiceChannel voiceChannel, string song, ISocketMessageChannel channel)
        {
            if (voiceChannel == null)
            {
                await channel.SendMessageAsync("Nie jesteś na żadnym kanale głosowym ćwoku");
                return;
            }

            LavalinkPlayer player = lavalinkManager.GetPlayer(guildId) ?? await lavalinkManager.JoinAsync(voiceChannel);

            if (song == "" && player.Playing == true)
            {
                await channel.SendMessageAsync("Brak nazwy/linku");
                return;
            }

            else if (song == "" & player.Playing == false)
            {
                await player.ResumeAsync();
                return;
            }

            LoadTracksResponse response = await lavalinkManager.GetTracksAsync(song);
            LavalinkTrack track = response.Tracks.First();
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
            //List<string> channels = new List<string>();
            List<string> playlists = new List<string>();
            List<string> videosLinks = new List<string>();

            int count = 1;
            string videoId = "";
            foreach (var searchResult in searchListResponse.Items)
            {
                videos.Add(String.Format("`{0}`. {1} \n", count, searchResult.Snippet.Title));
                videosLinks.Add(searchResult.Id.VideoId);
                videoId = videoId + searchResult.Id.VideoId + '.';
                count++;

                //switch (searchResult.Id.Kind)
                //{
                    //case "youtube#video":
                        //videos.Add(String.Format("`{0}`. {1} \n", count, searchResult.Snippet.Title));
                        //videosLinks.Add(searchResult.Id.VideoId);
                        //videoId = videoId + searchResult.Id.VideoId + '.';
                        //count++;
                        //break;

                        //case "youtube#channel":
                        //    channels.Add(String.Format("{0}", searchResult.Snippet.Title));
                        //    break;

                        //case "youtube#playlist":
                        //    playlists.Add(String.Format("{0}", searchResult.Snippet.Title));
                        //    break;

                        // Można rónież szukać kanałów i playlisty, jednak trzeba pozmieniać kilka rzeczy oprócz tych caseów wyżej
                        // a konkretnie SearchListRequest.Type = "video"; i moze coś jeszcze(?)
                //}
            }
            return videos;
        }

        public static string[] GetYoutubeLinksAsync(string query, ulong guildId, IVoiceChannel voiceChannel)
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

            string videoId = "";
            foreach (var searchResult in searchListResponse.Items)
            {
                videoId = videoId + searchResult.Id.VideoId + '.';
            }
            string[] options = videoId.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            return options;
        }
    }
}