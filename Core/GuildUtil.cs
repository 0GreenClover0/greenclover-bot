using GreenClover.Core.Accounts;

namespace GreenClover.Core
{
    class GuildUtil
    {
        static GuildUtil()
        {

        }

        public static string ChangeLanguage(GuildAccount guildAccount, string language)
        {
            if (language == "polish" || language == "polski")
            {
                guildAccount.ConfigLang = "Texts/pl_PL.json";
                GuildAccounts.SaveGuilds();
                return "polish";
            }

            else if (language == "english")
            {
                guildAccount.ConfigLang = "Texts/en_US.json";
                GuildAccounts.SaveGuilds();
                return "english";
            }

            else
            {
                return null;
            }
        }

        public static string ChangedLanguageAlertKey(string changedLanguage)
        {
            //Gets an alert from string returned by ChangeLanguage method
            switch (changedLanguage)
            {
                case null:
                    return "LANGUAGE_ERROR";
                case "polish":
                    return "LANGUAGE_CHANGED_POLISH";
                case "english":
                    return "LANGUAGE_CHANGED_ENGLISH";
                default:
                    return null;
            }
        }
    }
}
