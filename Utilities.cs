using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Google.Apis.Customsearch.v1;
using Google.Apis.Services;
using Google.Apis.Customsearch.v1.Data;

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

        public static string GetGoogleUrl(string text)
        {
            const string apiKey = "AIzaSyAOE5Ilq9ovt9M7epd7QZ78ClPf-MFK97E";
            const string searchEngineId = "005465617827265131652:so9dtxfb1zk";

            var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
            var listRequest = customSearchService.Cse.List(text);
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
    }
}