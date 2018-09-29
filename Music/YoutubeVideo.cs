using System;
using System.Collections.Generic;
using System.Text;

namespace GreenClover.Music
{
    public class YoutubeVideo
    {
        public string desc;
        public string image;
        public string link;
        public string title;

        public void AssignInformation(string videoDesc, string videoImageUrl, string videoLink, string videoTitle)
        {
            desc = videoDesc;
            image = videoImageUrl;
            link = videoLink;
            title = videoTitle;
        }
    }
}
