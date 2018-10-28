using Discord.WebSocket;
using System;
using System.Collections.Generic;

namespace GreenClover.Music
{
    public class YoutubeVideo
    {
        public string[] desc = new string[10];
        public string[] image = new string[10];
        public string[] link = new string[10];
        public string[] title = new string[10];
        public List<string> videosList = new List<string>();

        public void SetInfoVideo(SocketGuild guild, string videoDesc, string videoImageUrl, string videoLink, string videoTitle)
        {
            Utilities utilities = new Utilities(guild);
            if (videoDesc == null || videoDesc == "") videoDesc = Utilities.GetAlert("PLAY_NULL_DESC");
            desc[0] = videoDesc;
            image[0] = videoImageUrl;
            link[0] = videoLink;
            title[0] = videoTitle;
        }

        public void SetMultipleVideosInfo(SocketGuild guild, YoutubeVideo video, Google.Apis.YouTube.v3.Data.SearchListResponse searchList)
        {
            int count = 1;
            int i = 0;
            foreach (var searchResult in searchList.Items)
            {
                Utilities utilities = new Utilities(guild);
                video.videosList.Add($"{count}. {searchResult.Snippet.Title} \n");
                video.link[i] = searchResult.Id.VideoId;
                video.title[i] = searchResult.Snippet.Title;
                video.desc[i] = searchResult.Snippet.Description;
                video.image[i] = searchResult.Snippet.Thumbnails.High.Url;
                if (video.desc[i] == null || video.desc[i] == "") video.desc[i] = Utilities.GetAlert("PLAY_NULL_DESC");
                count++;
                i++;
            }
        }
    }
}
