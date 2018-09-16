using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using System.Collections.Generic;

namespace GreenClover
{
    public class GoogleService
    {
        public static string GetGoogleUrl(string query)
        {
            string apiKey = Config.bot.apiKey;
            string searchEngineId = Config.bot.searchEngineId;

            var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
            var listRequest = customSearchService.Cse.List(query);
            listRequest.Cx = searchEngineId;
            listRequest.Safe = CseResource.ListRequest.SafeEnum.High;

            IList<Result> paging = new List<Result>();

            paging = listRequest.Execute().Items;

            if (paging == null)
            {
                return Utilities.GetAlert("GOOGLE_NULL_RESULTS");
            }

            var link = paging[0];
            return Utilities.GetFormattedAlert("GOOGLE_IMAGE_RESULT", link.Title, link.Link);
            // Można też zrobić tak jak w funkcji GetYoutube (czyli uzyć foreach i dostać więcej wyników)
        }

        public static string GetGoogleImage(string query)
        {
            string apiKey = Config.bot.apiKey;
            string searchEngineId = Config.bot.searchEngineId;

            var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
            var listRequest = customSearchService.Cse.List(query);
            listRequest.Cx = searchEngineId;
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            listRequest.Safe = CseResource.ListRequest.SafeEnum.High;

            IList<Result> paging = new List<Result>();
            paging = listRequest.Execute().Items;

            if (paging == null)
            {
                return "0";
            }

            var result = paging[0];
            string link = result.Image.ThumbnailLink;

            if (link == null || link == "")
            {
                return Utilities.GetAlert("GOOGLE_IMAGE_ERROR");
            }

            return link;
            // Można też zrobić tak jak w funkcji GetYoutube (czyli uzyć foreach i dostać więcej wyników)
            // foreach (Result result in paging.Items)
            // {
            //     imageUrls.Add(result.Image.ThumbnailLink);
            // }
        }
    }
}