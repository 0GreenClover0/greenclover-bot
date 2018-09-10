using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.YouTube.v3;
using SharpLink;
using System.Linq;
using System.Threading.Tasks;
using Discord;

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

        public static string GetGoogleUrl(string query)
        {
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
    }
}