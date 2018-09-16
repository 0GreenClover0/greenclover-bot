using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using System.Collections.Generic;

namespace GreenClover.Music
{
    public class MusicModules : InteractiveBase
    {
        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync([Remainder] string song = "")
        {
            await AudioService.PlayAsync(Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel, song, Context.Channel);
        }

        [Command("search", RunMode = RunMode.Async)]
        public async Task YoutubeAsync([Remainder] string query = "")
        {
            if (query == "")
            {
                await ReplyAsync("Czego mam szukać?");
                return;
            }

            var searchList = AudioService.GetYoutubeAsync(query, Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel);
            string[] videoLinks = new string[10];
            string[] videoTitles = new string[10];
            string[] videoDescs = new string[10];
            string[] videoImages = new string[10];
            List<string> videos = new List<string>();

            int count = 1;
            int i = 0;
            foreach (var searchResult in searchList.Items)
            {
                videos.Add(String.Format("{0}. {1} \n", count, searchResult.Snippet.Title));
                videoLinks[i] = searchResult.Id.VideoId;
                videoTitles[i] = searchResult.Snippet.Title;
                videoDescs[i] = searchResult.Snippet.Description;
                videoImages[i] = searchResult.Snippet.Thumbnails.High.Url;
                count++;
                i++;
            }
            string authorImgUrl = Context.Message.Author.GetAvatarUrl();

            EmbedBuilder builder = new EmbedBuilder();
            builder
                .WithAuthor(Context.Message.Author.Username, authorImgUrl)
                .WithThumbnailUrl(authorImgUrl)
                .WithTitle(Utilities.GetAlert("YOUTUBE_FILMEMBED"))
                .WithDescription(String.Format("{0}", string.Join("\n", videos)))
                .WithColor(Color.Red);

            await ReplyAsync("", false, builder.Build());

            var response = await NextMessageAsync();
            string answer = response.ToString();
            int choose = Utilities.ConvertToInt(answer);

            if (choose == 0)
            {
                await ReplyAsync("Wymagana jest liczba od 1 do 10");
                return;
            }

            if (answer == "cancel" || answer == "anuluj")
            {
                await ReplyAsync("Anulowano");
                return;
            }

            if ((Context.User as IVoiceState).VoiceChannel == null)
            {
                await ReplyAsync("Nie jesteś na żadnym kanale głosowym ćwoku");
                return;
            }

            choose = choose - 1;
            string song = videoLinks[choose];

            if (videoDescs[choose] == null || videoDescs[choose] == "")
            {
                videoDescs[choose] = "Brak opisu";
            }

            EmbedBuilder builderPlay = new EmbedBuilder();
            builderPlay
                .WithAuthor(Context.Message.Author.Username, authorImgUrl)
                .WithThumbnailUrl(videoImages[choose])
                .WithDescription($"Grany utwór:\n[{videoTitles[choose]}](https://www.youtube.com/watch?v={song})")
                .AddField("Opis:", videoDescs[choose])
                .WithColor(Color.DarkRed);

            await ReplyAsync("", false, builderPlay.Build());
            await AudioService.PlayAsync(Context.Guild.Id, (Context.User as IVoiceState).VoiceChannel, $"https://www.youtube.com/watch?v={song}", Context.Channel);
            return;
        }

        [Command("leave")]
        public async Task LeaveAsync()
        {
            await AudioService.LeaveAsync(Context.Guild.Id);
        }

        [Command("stop")]
        public async Task StopAsync()
        {
            await AudioService.StopAsync(Context.Guild.Id);
        }
    }
}