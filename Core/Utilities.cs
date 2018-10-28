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
            if (alerts.ContainsKey(key)) return alerts[key];

            return "Error - does not contains key to alert. Please report this in our site.";
        }

        public static string GetFormattedAlert(string key, params object[] parameter)
        {
            if (alerts.ContainsKey(key)) return String.Format(alerts[key], parameter);

            return "Error - does not contains key to alert. Please report this in our site.";
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
    }
}