using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // This is a TODO, i really need to improve creating the queue because it's slow and inefficient af
        public static List<string> CreateListOfSongs(ISocketMessageChannel channel, List<LavalinkTrack> queue, string username, string avatar)
        {
            YoutubeVideo video = new YoutubeVideo();
            int count = 1;
            //int i = 0;

            foreach (var track in queue)
            {
                video.videosList.Add($"{count}. [{track.Title}]({track.Url}) `{track.Length}` \n");
                //video.link[i] = track.Url;
                //video.title[i] = track.Title;
                count++;
                //i++;
            }

            return video.videosList;
        }

        public static List<List<string>> CreateListOfPages(List<string> videoList)
        {
            List<List<string>> pages = new List<List<string>>();

            if (videoList.Count < 11)
            {
                pages.Add(videoList.GetRange(0, videoList.Count));
            }

            else if (videoList.Count > 10 && videoList.Count < 21)
            {
                pages.Add(videoList.GetRange(0, 10));
                pages.Add(videoList.GetRange(10, videoList.Count - 10));
            }

            else if (videoList.Count > 20 && videoList.Count < 31)
            {
                pages.Add(videoList.GetRange(0, 10));
                pages.Add(videoList.GetRange(10, 10));
                pages.Add(videoList.GetRange(20, videoList.Count - 20));
            }

            else if (videoList.Count > 30 && videoList.Count < 41)
            {
                pages.Add(videoList.GetRange(0, 10));
                pages.Add(videoList.GetRange(10, 10));
                pages.Add(videoList.GetRange(20, 10));
                pages.Add(videoList.GetRange(30, videoList.Count - 30));
            }

            else if (videoList.Count > 40)
            {
                pages.Add(videoList.GetRange(0, 10));
                pages.Add(videoList.GetRange(10, 10));
                pages.Add(videoList.GetRange(20, 10));
                pages.Add(videoList.GetRange(30, 10));
                pages.Add(videoList.GetRange(40, videoList.Count - 40));
            }

            return pages;
        }

        public static string[] AssignContentToPages(string[] pages, List<List<string>> pagesContent)
        {
            pages[0] = string.Join("\n", pagesContent[0].ToArray());

            if (pagesContent.Count > 1)
            {
                pages[1] = string.Join("\n", pagesContent[1].ToArray());
            }

            if (pagesContent.Count > 2)
            {
                pages[2] = string.Join("\n", pagesContent[2].ToArray());
            }

            if (pagesContent.Count > 3)
            {
                pages[3] = string.Join("\n", pagesContent[3].ToArray());
            }

            if (pagesContent.Count > 4)
            {
                pages[4] = string.Join("\n", pagesContent[4].ToArray());
            }

            return pages;
        }
    }
}
