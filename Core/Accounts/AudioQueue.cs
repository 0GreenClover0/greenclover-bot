using System.Collections.Generic;
using SharpLink;

namespace GreenClover.Core.Accounts
{
    public class AudioQueue
    {
        public ulong GuildID { get; set; }

        public int PlayingTrackIndex { get; set; }

        public List<LavalinkTrack> Queue { get; set; }
    }
}
