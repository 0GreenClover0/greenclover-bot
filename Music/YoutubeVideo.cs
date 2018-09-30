using System;
using System.Collections.Generic;
using System.Text;

namespace GreenClover.Music
{
    public class YoutubeVideo
    {
        public string[] desc = new string[10];
        public string[] image = new string[10];
        public string[] link = new string[10];
        public string[] title = new string[10];
        public List<string> videosList = new List<string>();

        public void SetInfoVideo(string videoDesc, string videoImageUrl, string videoLink, string videoTitle)
        {
            desc[0] = videoDesc;
            image[0] = videoImageUrl;
            link[0] = videoLink;
            title[0] = videoTitle;
        }

        public void SetInfoMultipleVideos(YoutubeVideo video, Google.Apis.YouTube.v3.Data.SearchListResponse searchList)
        {
            int count = 1;
            int i = 0;
            foreach (var searchResult in searchList.Items)
            {
                video.videosList.Add(String.Format("{0}. {1} \n", count, searchResult.Snippet.Title));
                video.link[i] = searchResult.Id.VideoId;
                video.title[i] = searchResult.Snippet.Title;
                video.desc[i] = searchResult.Snippet.Description;
                video.image[i] = searchResult.Snippet.Thumbnails.High.Url;
                count++;
                i++;
            }
        }
    }
}
