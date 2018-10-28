using GreenClover.Core.Accounts;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GreenClover.Core
{
    public static class DataStorage
    {
        // Saves accounts
        public static void SaveUserAccounts(IEnumerable<UserAccount> accounts, string filePath)
        {
            string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        // Loads accounts
        public static IEnumerable<UserAccount> LoadUserAccounts(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File with users doesn't exist");
                return null;
            }

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<UserAccount>>(json);
        }

        // Saves guilds
        public static void SaveGuilds(IEnumerable<GuildAccount> guilds, string filePath)
        {
            string json = JsonConvert.SerializeObject(guilds, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        // Loads guilds
        public static IEnumerable<GuildAccount> LoadGuilds(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File with guilds doesn't exist");
                return null;
            }

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<GuildAccount>>(json);
        }

        public static void SaveQueues(IEnumerable<AudioQueue> audioQueues, string filePath)
        {
            string json = JsonConvert.SerializeObject(audioQueues, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static IEnumerable<AudioQueue> LoadQueues(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File with audio queues doesn't exist");
                return null;
            }

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<AudioQueue>>(json);
        }

        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}