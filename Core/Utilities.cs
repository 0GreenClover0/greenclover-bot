using System;
using System.Collections.Generic;
using System.IO;
using Discord.WebSocket;
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

        public static int ConvertToInt(string answer, ISocketMessageChannel channel)
        {
            int response = 0;
            if (answer == "1")
            {
                response = 1;
                return response;
            }

            if (answer == "2")
            {
                response = 2;
                return response;
            }

            if (answer == "3")
            {
                response = 3;
                return response;
            }

            if (answer == "4")
            {
                response = 4;
                return response;
            }

            if (answer == "5")
            {
                response = 5;
                return response;
            }

            if (answer == "6")
            {
                response = 6;
                return response;
            }

            if (answer == "7")
            {
                response = 7;
                return response;
            }

            if (answer == "8")
            {
                response = 8;
                return response;
            }

            if (answer == "9")
            {
                response = 9;
                return response;
            }

            if (answer == "10")
            {
                response = 10;
                return response;
            }

            else
            {
                channel.SendMessageAsync("Wymagania jest liczba od 1 do 10");
                return 0;
            }
        }
        // Pobieranie zaproszeń danego serwera
        //public static async Task GetInvites(SocketCommandContext context)
        //{
        //   var invites = await context.Guild.GetInvitesAsync();

        //    if (invites.Select(x => x.Url).FirstOrDefault() != null)
        //    {
        //        Console.WriteLine(invites);
        //        Console.WriteLine(invites.Select(x => x.Url).FirstOrDefault());
        //        return;
        //    }

        //    else
        //    {
        //        Console.WriteLine($"Na tym serwerze {context.Guild.Id}, {context.Guild.Name} nie ma żadnych zaproszeń");
        //    }
        //}
    }
}