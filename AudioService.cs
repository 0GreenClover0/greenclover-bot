using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using SharpLink;

namespace GreenClover
{
    class AudioService
    {
        public async static Task PlayAsync(ulong guildId, IVoiceChannel voiceChannel, string song)
        {
            LavalinkPlayer player = Program._lavalinkManager.GetPlayer(guildId) ?? await Program._lavalinkManager.JoinAsync(voiceChannel);
            LoadTracksResponse response = await Program._lavalinkManager.GetTracksAsync(song);
            LavalinkTrack track = response.Tracks.First();
            await player.PlayAsync(track);
        }

        public static List<string> GetYoutube(string query)
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