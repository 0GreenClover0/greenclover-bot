using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

namespace GreenClover.src.UserAccounts
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

            var checkedAccount = UserAccountCheck(account);
            return checkedAccount;
        }

        private static UserAccount CreateUserAccount(ulong id)
        {
            var newAccount = new UserAccount()
            {
                ID = id,
                Description = "Nie ustanowiono opisu",
                Points = 10,
                XP = 0,
            };

            accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }

        private static UserAccount UserAccountCheck(UserAccount account)
        {
            if (account.Description == null)
            {
                account.Description = "Nie ustawiono opisu.";
            }
            SaveAccounts();
            return account;
        }
    }
}