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
            if (arg3 == "FINISHED")
            {
                await RemoveAndPlay(arg1, arg2);
            }
            return;
        }

        public static async Task RemoveAndPlay(LavalinkPlayer player, LavalinkTrack track)
        {
            var audioQueue = AudioQueues.GetAudioQueue(player.VoiceChannel.Guild as SocketGuild);

            RemoveTrack(audioQueue, track);
            await player.StopAsync();
            await PlayNextAfterRemove(audioQueue, player);
        }

        private static void RemoveTrack(AudioQueue audioQueue, LavalinkTrack track)
        {
            
            audioQueue.Queue.Remove(audioQueue.Queue[audioQueue.PlayingTrackIndex]);
            AudioQueues.SaveQueues();
            return;
        }

        private static async Task PlayNextAfterRemove(AudioQueue audioQueue, LavalinkPlayer player)
        { 
            if (audioQueue.Queue.ElementAtOrDefault(0) != null)
            {
                audioQueue.PlayingTrackIndex = 0;
                AudioQueues.SaveQueues();
                LavalinkTrack track = audioQueue.Queue.ElementAtOrDefault(0);
                await player.PlayAsync(track);
            }

            await Task.CompletedTask;
        }
    }
}
