using System;
using System.Collections.Generic;
using System.IO;
using Discord.WebSocket;
using GreenClover.Core.Accounts;
using Newtonsoft.Json;

namespace GreenClover
{
    class Utilities
    {
        private static Dictionary<string, string> alerts;

        public Utilities(SocketGuild guild)
        {
            string filePath = CheckLanguage(guild);
            string json = File.ReadAllText(filePath);
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            alerts = data.ToObject<Dictionary<string, string>>();
        }

        private static string CheckLanguage(SocketGuild guild)
        {
            var guildAccount = GuildAccounts.GetGuildAccount(guild);
            string filePath = guildAccount.ConfigLang;
            return filePath;
        }

        // Basic functions to get alerts
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

        // Get random line from a text file
        public static string GetRandomLine(string path)
        {
            var lines = File.ReadAllLines(path);
            var r = new Random();
            var randomLineNumber = r.Next(0, lines.Length);
            var line = lines[randomLineNumber];
            return line;
        }

        // Changing user's answer (from 1 to 10) from string to int
        public static int ConvertToInt(string answer)
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
                return 0;
            }
        }
    }
}