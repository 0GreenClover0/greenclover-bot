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
        private static DiscordSocketClient _client = new DiscordSocketClient();
        public static LavalinkManager lavalinkManager = new LavalinkManager(_client, new LavalinkManagerConfig()
        {
            RESTHost = "localhost",
            RESTPort = 2333,
            WebSocketHost = "localhost",
            WebSocketPort = 80,
            Authorization = "youshallnotpass",
            TotalShards = 1,
            LogSeverity = LogSeverity.Verbose
        });

        public static async Task PlayAsync(ulong guildId, IVoiceChannel voiceChannel, string song, ISocketMessageChannel channel)
        {
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
                await player.ResumeAsync();
                return;
            }

            LoadTracksResponse response = await lavalinkManager.GetTracksAsync(song);
            LavalinkTrack track = response.Tracks.First();
            await player.PlayAsync(track);
        }

        public static async Task LeaveAsync(ulong guildId)
        {
            await lavalinkManager.LeaveAsync(guildId);
        }

        public static async Task StopAsync(ulong guildId)
        {
            LavalinkPlayer player = lavalinkManager.GetPlayer(guildId);
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