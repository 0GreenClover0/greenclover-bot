using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Newtonsoft.Json;

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

        // Podstawowe funkcje dla pliku Jsona do wyciągania wartości
        public static string GetAlert(string key)
        {
            if (alerts.ContainsKey(key))
            {
                return alerts[key];
            }

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

        // Pobieranie losowej linii z pliku tekstowego
        public static string GetRandomLine(string path)
        {
            var lines = File.ReadAllLines(path);
            var r = new Random();
            var randomLineNumber = r.Next(0, lines.Length);
            var line = lines[randomLineNumber];
            return line;
        }

        // Pobieranie zaproszeń danego serwera
        public static async Task GetInvites(SocketCommandContext context)
        {
            var invites = await context.Guild.GetInvitesAsync();

            if (invites.Select(x => x.Url).FirstOrDefault() != null)
            {
                Console.WriteLine(invites);
                Console.WriteLine(invites.Select(x => x.Url).FirstOrDefault());
                return;
            }

            else
            {
                Console.WriteLine($"Na tym serwerze {context.Guild.Id}, {context.Guild.Name} nie ma żadnych zaproszeń");
            }
        }
    }
}