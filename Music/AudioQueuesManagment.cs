using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using SharpLink;
using GreenClover.Core.Accounts;

namespace GreenClover.Music
{
    class AudioQueuesManagment
    {

        public AudioQueuesManagment()
        {

        }

        public static async Task LavalinkManager_TrackEnd(LavalinkPlayer arg1, LavalinkTrack arg2, string arg3)
        {
            LavalinkPlayer player = arg1;
            await RemoveEndedTrack(player.VoiceChannel.Guild, arg2);
            await PlayNext(player.VoiceChannel.Guild, AudioService.lavalinkManager);
        }

        private static async Task RemoveEndedTrack(IGuild guild, LavalinkTrack track)
        {
            var audioQueue = AudioQueues.GetAudioQueue(guild as SocketGuild);
            audioQueue.Queue.Remove(track);
            AudioQueues.SaveQueues();
            await Task.CompletedTask;
        }

        private static async Task PlayNext(IGuild guild, LavalinkManager lavalinkManager)
        {
            var audioQueue = AudioQueues.GetAudioQueue(guild as SocketGuild);
            LavalinkPlayer player = lavalinkManager.GetPlayer(guild.Id);

            if (audioQueue.Queue.ElementAtOrDefault(0) != null)
            {
                await player.PlayAsync(audioQueue.Queue.ElementAtOrDefault(0));
            }

            await Task.CompletedTask;
        }
    }
}
