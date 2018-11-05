using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

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

        public static UserAccount GetAccount(SocketUser user)
        {
            return GetOrCreateAccount(user.Id);
        }

        private static UserAccount GetOrCreateAccount(ulong id)
        {
            var result = from a in accounts
                         where a.ID == id
                         select a;

            var account = result.FirstOrDefault();

            if (account == null)
            {
                account = CreateUserAccount(id);
                return account;
            }
            return account;
        }

        private static UserAccount CreateUserAccount(ulong id)
        {
            var newAccount = new UserAccount()
            {
                ID = id,
                Description = "Description not set",
                Credits = 10,
                XP = 0,
            };

            accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }
    }
}