using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using System.Collections.Generic;

namespace GreenClover
{
    public class GoogleService
    {
        public static string GetGoogle(string query, int type)
        {
            IList<Result> paging = new List<Result>();
            paging = GoogleSearch(query, type);
            if (paging == null) return Utilities.GetAlert("GOOGLE_NULL_RESULTS");
            var result = paging[0];

            if (type == 0)
            {
                string imageLink = result.Image.ThumbnailLink;
                if (imageLink == null || imageLink == "") return Utilities.GetAlert("GOOGLE_IMAGE_ERROR");

                return imageLink;
            }

            return Utilities.GetFormattedAlert("GOOGLE_RESULT", result.Title, result.Link);
            // Można też zrobić tak jak w funkcji GetYoutube (czyli uzyć foreach i dostać więcej wyników)
            // foreach (Result result in paging.Items)
            // {
            //     imageUrls.Add(result.Image.ThumbnailLink);
            // }
        }

        private static IList<Result> GoogleSearch(string query, int type)
        {
            // Klucz do wyszukiwarki "Custom Search"
            string apiKey = Config.bot.apiKey;
            string searchEngineId = Config.bot.searchEngineId;

            // Serwis wyszukiwania, ustawienia wyszukiwarki
            var customSearchService = new CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
            var listRequest = customSearchService.Cse.List(query);
            listRequest.Cx = searchEngineId;
            listRequest.Safe = CseResource.ListRequest.SafeEnum.High;
            if (type == 0)
            {
                listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            }

            return listRequest.Execute().Items;
        }
    }
}