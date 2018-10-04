using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using GreenClover.Core;

namespace GreenClover.Modules
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Alias("pomoc")]
        public async Task HelpAsync([Remainder] string msg = "")
        {
            Utilities utilities = new Utilities(Context.Guild);
            string avatar = Context.Client.CurrentUser.GetAvatarUrl() ?? Context.Client.CurrentUser.GetDefaultAvatarUrl();

            if (msg != "")
            {
                // Getting a first word after a command. There is creating an {alertkey}, which is used to get
                // a description of a specific command. HelpAliasesCommands is checking if the user used an alias of the command
                msg = msg.ToLower();
                string[] wholeMsg = msg.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string aliasCommand = CommandUtil.HelpAliasesCommands(wholeMsg);

                if (aliasCommand != null)
                { 
                    wholeMsg[0] = aliasCommand;
                }

                string alertKey = wholeMsg[0].ToUpper();

                // Exception for a commands like "send nudes" because they have more than one word
                // !GlobalVar.allCommandsEng.Contains(wholeMsg[0]) is used when the user types something like
                // --help ping send nudes OR --help send ping nudes etc.
                if (msg.Contains("send") && msg.Contains("nudes") && !GlobalVar.allCommandsEng.Contains(wholeMsg[0]))
                {
                    wholeMsg[0] = "send nudes";
                    alertKey = "SENDNUDES";
                }

                // Getting a description which is store in a json file by using HELP_DESC_{alertKey}
                // It's important to use this way of naming descriptions in the next commands
                // If a variable GlobalVar.allCommandsEng contains a word that the user send,
                // We get description of that command
                // If not, the bot displays a list of commands
                if (GlobalVar.allCommandsEng.Contains(wholeMsg[0]) || wholeMsg[0] == "send nudes")
                {
                    EmbedBuilder builderHelp = new EmbedBuilder();
                    builderHelp
                        .WithAuthor(Utilities.GetFormattedAlert("HELP_SPECIFIC_COMMAND", wholeMsg[0]), avatar)
                        .AddField(Utilities.GetAlert("HELP_TEXT"), "~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~")
                        .AddField($"[*{wholeMsg[0]}]", Utilities.GetAlert($"HELP_DESC_{alertKey}"))
                        .WithColor(new Color(110, 80, 120));

                    await ReplyAsync("", false, builderHelp.Build());
                    return;
                }
            }

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithAuthor(Utilities.GetAlert("BOT_NAME"), avatar)
                .AddField(Utilities.GetAlert("HELP_TEXT"), "~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~ ~")
                .AddField(Utilities.GetAlert("HELP_CAT_COMMANDS"), Utilities.GetAlert("HELP_DESC_COMMANDS"))
                .AddField(Utilities.GetAlert("HELP_CAT_PREFIX"), Utilities.GetAlert("HELP_DEFAULT_PREFIX"))
                .AddField(Utilities.GetAlert("HELP_CAT_FUN"), Utilities.GetAlert("HELP_LIST_FUN"))
                .AddField(Utilities.GetAlert("HELP_CAT_PROFILE"), Utilities.GetAlert("HELP_LIST_PROFILE"))
                .AddField(Utilities.GetAlert("HELP_CAT_AUDIO"), Utilities.GetAlert("HELP_LIST_AUDIO"))
                .AddField(Utilities.GetAlert("HELP_CAT_MOD"), Utilities.GetAlert("HELP_LIST_MOD"))
                .AddField(Utilities.GetAlert("HELP_CAT_GOOGLE"), Utilities.GetAlert("HELP_LIST_GOOGLE"))
                .AddField(Utilities.GetAlert("HELP_CAT_SYSTEM"), Utilities.GetAlert("HELP_LIST_SYSTEM"))
                .WithColor(new Color(90, 50, 165));

            await ReplyAsync("", false, builder.Build());
        }
    }
}