using Discord.WebSocket;
using SharpLink;
using System.Collections.Generic;
using System.Linq;

namespace GreenClover.Core.Accounts
{
    public static class AudioQueues
    {
        public static List<AudioQueue> audioQueues;

        public static readonly string queuesFile = "Resources/audioQueues.json";

        static AudioQueues()
        {
            if (DataStorage.SaveExists(queuesFile))
            {
                audioQueues = DataStorage.LoadQueues(queuesFile).ToList();
            }

            else
            {
                audioQueues = new List<AudioQueue>();
                SaveQueues();
            }
        }

        public static void SaveQueues()
        {
            DataStorage.SaveQueues(audioQueues, queuesFile);
        }

        public static AudioQueue GetAudioQueue(SocketGuild guild)
        {
            return GetOrCreateAudioQueue(guild.Id);
        }

        private static AudioQueue GetOrCreateAudioQueue(ulong id)
        {
            var result = from q in audioQueues
                         where q.GuildID == id
                         select q;

            var queue = result.FirstOrDefault();

            if (queue == null)
            {
                queue = CreateQueue(id);
                return queue;
            }
            return queue;
        }

        private static AudioQueue CreateQueue(ulong id)
        {
            var newQueue = new AudioQueue()
            {
                GuildID = id,
                PlayingTrackIndex = -1,
                Queue = new List<LavalinkTrack>()
            };

            audioQueues.Add(newQueue);
            SaveQueues();
            return newQueue;
        }

        public static List<LavalinkTrack> GetOrCreateGuildQueue(LavalinkTrack track, AudioQueue audioQueue)
        {
            LavalinkTrack firstTrack = audioQueue.Queue.ElementAtOrDefault(0);

            if (firstTrack == null)
            {
                audioQueue.Queue = CreateGuildQueue(track);
                SaveQueues();
                return audioQueue.Queue;
            }

            else
            {
                audioQueue.Queue = GetAndAddToGuildQueue(track, audioQueue.Queue);
                SaveQueues();
                return audioQueue.Queue;
            }
        }

        private static List<LavalinkTrack> CreateGuildQueue(LavalinkTrack track)
        {
            List<LavalinkTrack> queue = new List<LavalinkTrack>
            {
                track
            };
            return queue;
        }

        private static List<LavalinkTrack> GetAndAddToGuildQueue(LavalinkTrack track, List<LavalinkTrack> queue)
        {
            queue.Add(track);
            return queue;
        }
    }
}