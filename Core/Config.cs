﻿using System.IO;
using Newtonsoft.Json;

namespace GreenClover
{
    class Config
    {
        private const string configFolder = "Resources";
        private const string configFile = "config.Json";

        public static BotConfig bot;
        
        static Config()
        {
            if (!Directory.Exists("configFolder"))
                Directory.CreateDirectory(configFolder);

            if (!File.Exists(configFolder + "/" + configFile))
            {
                bot = new BotConfig();
                string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + configFile, json);
            }
            
            else
            {
                string json = File.ReadAllText(configFolder + "/" + configFile);
                bot = JsonConvert.DeserializeObject<BotConfig>(json);
            }
        }
    }

    public struct BotConfig
    {
        public string apiKey;
        public string cmdPrefix;
        public string language;
        public ulong ownerID;
        public string searchEngineId;
        public string token;
    }
}