using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.YouTube.v3;


namespace GreenClover
{
    class Utilities
    {
        private static Dictionary<string, string> alerts;

        static Utilities()
        {
            string json = File.ReadAllText("Texts/alerts.json");
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            alerts = data.ToObject<Dictionary<string, string>>();
        }

        public static string GetAlert(string key)
        {
            if (alerts.ContainsKey(key)) return alerts[key];
            return "Error - does not contains key to alert";
        }

        public static string GetFormattedAlert(string key, params object[] parameter)
        {
            if (alerts.ContainsKey(key))
            {
                return String.Format(alerts[key], parameter);
            }
            return "Error - does not contains key to alert";
        }

        public static string GetFormattedAlert(string key, object parameter)
        {
            return GetFormattedAlert(key, new object[] { parameter });
        }

        public static string GetRandomLine(string path)
        {
            var lines = File.ReadAllLines(path);
            var r = new Random();
            var randomLineNumber = r.Next(0, lines.Length);
            var line = lines[randomLineNumber];
            return line;
        }

        public static string GetGoogleUrl(string query, string second, string third, string fourth, string fifth)
        {
            query = query + " " + second + " " + third + " " + fourth + " " + fifth;

            string apiKey = Config.bot.apiKey;
            string searchEngineId = Config.bot.searchEngineId;

            var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
            var listRequest = customSearchService.Cse.List(query);
            listRequest.Cx = searchEngineId;

            IList<Result> paging = new List<Result>();

            paging = listRequest.Execute().Items;

            if (paging != null)
            {
                var test = paging[0];
                return $"Tytuł: {test.Title} Link: {test.Link}";
            }
            else if (paging == null)
                return "Błąd - nie znaleziono wyników";
            else
                return "Nieznany błąd";
        }

        public static List<string> GetYoutube(string query, string second, string third, string fourth, string fifth)
        {
            query = query + " " + second + " " + third + " " + fourth + " " + fifth;
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = Config.bot.apiKey,
                ApplicationName = "test"
            });
            var SearchListRequest = youtubeService.Search.List("snippet");
            SearchListRequest.Q = query;
            SearchListRequest.Type = "video";
            SearchListRequest.MaxResults = 10;

            var searchListResponse = SearchListRequest.Execute();

            List<string> videos = new List<string>();
            List<string> channels = new List<string>();
            List<string> playlists = new List<string>();

            // Also you can use searchResult.Id.VideoId to get ID of the video (obvious)

            int count = 1;
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format("`{0}`. {1} \n", count, searchResult.Snippet.Title));
                        count++;
                        break;

                    /*case "youtube#channel":
                        channels.Add(String.Format("{0}", searchResult.Snippet.Title));
                        break;

                    case "youtube#playlist":
                        playlists.Add(String.Format("{0}", searchResult.Snippet.Title));
                        break;
                   */
                }
            }
            return videos;
        }
    }
}