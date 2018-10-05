using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using SharpLink;

namespace GreenClover
{
    class AudioService
    {
        private static readonly DiscordSocketClient _client = new DiscordSocketClient();
        public static LavalinkManager lavalinkManager = new LavalinkManager(_client);

        public static async Task PlayAsync(SocketGuild guild, ulong guildId, IVoiceChannel voiceChannel, string song, ISocketMessageChannel channel)
        {
            Utilities utilities = new Utilities(guild);
            if (voiceChannel == null)
            {
                await channel.SendMessageAsync(Utilities.GetAlert("PLAY_NULL_CHANNEL"));
                return;
            }

            LavalinkPlayer player = lavalinkManager.GetPlayer(guildId) ?? await lavalinkManager.JoinAsync(voiceChannel);

            if (song == "" && player.Playing == true)
            {
                await channel.SendMessageAsync(Utilities.GetAlert("PLAY_NULL_LINK"));
                return;
            }

            else if (song == "" & player.Playing == false)
            {
                if (player.CurrentTrack == null)
                {
                    await channel.SendMessageAsync(Utilities.GetAlert("PLAY_NULL_LINK"));
                    return;
                }
                await player.ResumeAsync();
                return;
            }

            LoadTracksResponse response = await lavalinkManager.GetTracksAsync(song);
            LavalinkTrack track = response.Tracks.First();
            await player.PlayAsync(track);
        }

        public static async Task LeaveAsync(ulong guildId)
        {
            LavalinkPlayer player = lavalinkManager.GetPlayer(guildId);
            if (player == null) return;

            await lavalinkManager.LeaveAsync(guildId);
        }

        public static async Task StopAsync(ulong guildId)
        {
            LavalinkPlayer player = lavalinkManager.GetPlayer(guildId);
            if (player.Playing == false) return;

            await player.PauseAsync();
        }

        public static Google.Apis.YouTube.v3.Data.SearchListResponse GetYoutubeAsync(string query, ulong guildId, IVoiceChannel voiceChannel)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = Config.bot.apiKey,
                ApplicationName = "DiscordBot"
            });
            var SearchListRequest = youtubeService.Search.List("snippet");
            SearchListRequest.Q = query;
            SearchListRequest.Type = "video";
            SearchListRequest.MaxResults = 10;

            var searchListResponse = SearchListRequest.Execute();
            return searchListResponse;
        }
    }
}