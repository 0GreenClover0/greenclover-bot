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

        // podstawowe funkcje dla pliku Jsona do wyciągania wartości

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
            // Ponieważ customSearchService.Cse.List potrzebuje stringa, to do
            string apiKey = Config.bot.apiKey;
            string searchEngineId = Config.bot.searchEngineId;

            var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
            var listRequest = customSearchService.Cse.List(query);
            listRequest.Cx = searchEngineId;

            IList<Result> paging = new List<Result>();

            paging = listRequest.Execute().Items;

            if (paging != null)
            {
                var link = paging[0];
                return $"Tytuł: {link.Title} Link: {link.Link}";
                // Można też zrobić tak jak w funkcji GetYoutube (czyli uzyć foreach i dostać więcej wyników),
                // which will give you more than one result
            }
            else if (paging == null)
                return "Błąd - nie znaleziono wyników";
            else
                return "Nieznany błąd";
        }

        public static List<string> GetYoutube(string query, string second, string third, string fourth, string fifth)
        {
            query = query + " " + second + " " + third + " " + fourth + " " + fifth;
            // ponieważ SearchListRequest.Q potrzebuje stringa...
            // może da się to jakoś obejść? ToDo
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
            foreach (var searchResult in searchListResponse.Items)
            {
                switch (searchResult.Id.Kind)
                {
                    case "youtube#video":
                        videos.Add(String.Format("`{0}`. {1} \n", count, searchResult.Snippet.Title));
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
                }
            }
            return videos;
        }
    }
}