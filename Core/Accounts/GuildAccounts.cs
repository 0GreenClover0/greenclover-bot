using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

namespace GreenClover.Core.Accounts
{
    class GuildAccounts
    {
        private static List<GuildAccount> guildAccounts;

        private static readonly string guildsFile = "Resources/guildAccounts.json";

        static GuildAccounts()
        {
            if (DataStorage.SaveExists(guildsFile))
            {
                guildAccounts = DataStorage.LoadGuilds(guildsFile).ToList();
            }

            else
            {
                guildAccounts = new List<GuildAccount>();
                SaveGuilds();
            }
        }

        public static void SaveGuilds()
        {
            DataStorage.SaveGuilds(guildAccounts, guildsFile);
        }

        public static GuildAccount GetGuildAccount(SocketGuild guild)
        {
            return GetOrCreateGuildAccount(guild, guild.Id);
        }

        private static GuildAccount GetOrCreateGuildAccount(SocketGuild guild, ulong id)
        {
            var result = from g in guildAccounts
                         where g.ID == id
                         select g;

            var account = result.FirstOrDefault();

            if (account == null)
            {
                account = CreateGuildAccount(id, guild);
                return account;
            }
            return account;
        }

        private static GuildAccount CreateGuildAccount(ulong id, SocketGuild guild)
        {
            Utilities utilities = new Utilities(guild);
            var newAccount = new GuildAccount()
            {
                ID = id,
                Prefix = Config.bot.cmdPrefix,
                ConfigLang = Config.bot.language,
                BotReligion = Utilities.GetAlert("BOT_RELIGION"),
            };

            guildAccounts.Add(newAccount);
            SaveGuilds();
            return newAccount;
        }
    }
}