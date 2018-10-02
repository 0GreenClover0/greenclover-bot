using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

namespace GreenClover.Core.Accounts
{
    public static class UserAccounts
    {
        private static List<UserAccount> accounts;

        private static readonly string accountsFile = "Resources/accounts.json";

        static UserAccounts()
        {
            if (DataStorage.SaveExists(accountsFile))
            {
                accounts = DataStorage.LoadUserAccounts(accountsFile).ToList();
            }

            else
            {
                accounts = new List<UserAccount>();
                SaveAccounts();
            }
        }

        public static void SaveAccounts()
        {
            DataStorage.SaveUserAccounts(accounts, accountsFile);
        }

        public static UserAccount GetAccount(SocketGuild guild, SocketUser user)
        {
            return GetOrCreateAccount(guild, user.Id);
        }

        private static UserAccount GetOrCreateAccount(SocketGuild guild, ulong id)
        {
            var result = from a in accounts
                         where a.ID == id
                         select a;

            var account = result.FirstOrDefault();

            if (account == null)
            {
                account = CreateUserAccount(guild, id);
                return account;
            }
            return account;
        }

        private static UserAccount CreateUserAccount(SocketGuild guild, ulong id)
        {
            Utilities utilities = new Utilities(guild);
            var newAccount = new UserAccount()
            {
                ID = id,
                Description = Utilities.GetAlert("USER_DEFAULT_DESC"),
                Credits = 10,
                XP = 0,
            };

            accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}