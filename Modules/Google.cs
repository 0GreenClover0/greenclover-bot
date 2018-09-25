﻿using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace GreenClover.Modules
{
    public class Google : ModuleBase<SocketCommandContext>
    {
        [Command("google")]
        [Alias("wygoogluj", "wyszukaj")]
        public async Task GoogleAsync([Remainder] string query = "")
        {
            int searchType = 1;

            if (query == "")
            {
                await ReplyAsync(Utilities.GetAlert("GOOGLE_NULL_QUERY"));
                return;
            }

            await ReplyAsync(GoogleService.GetGoogle(query, searchType));
            return;
        }

        [Command("image")]
        [Alias("zdjęcie", "obraz")]
        public async Task GoogleImageAsync([Remainder] string query = "")
        {
            int searchType = 0;

            if (query == "")
            {
                await ReplyAsync(Utilities.GetAlert("GOOGLE_NULL_QUERY"));
                return;
            }

            string googleImage = GoogleService.GetGoogle(query, searchType);

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithImageUrl(googleImage);

            await ReplyAsync("", false, builder.Build());
        }
    }
}