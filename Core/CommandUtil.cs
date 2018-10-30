namespace GreenClover.Core
{
    class CommandUtil
    {
        static CommandUtil()
        {

        }

        public static string HelpAliasesCommands(string[] wholeMsg)
        {
            if (wholeMsg[0] == "desc" && !GlobalVar.allCommandsEng.Contains(wholeMsg[0]))
            {
                return "description";
            }

            else
            {
                return null;
            }
        }
    }
}
