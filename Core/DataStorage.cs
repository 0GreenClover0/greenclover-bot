using GreenClover.Core.UserAccounts;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace GreenClover.Core
{
    public static class DataStorage
    {
        // Zapisuje wszystkie userAccounts
        public static void SaveUserAccounts(IEnumerable<UserAccount> accounts, string filePath)
        {
            string json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        // Wczytuje userAccounts
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

        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}