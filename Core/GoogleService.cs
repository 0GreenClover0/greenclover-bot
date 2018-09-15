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
                return "Błąd - nie znaleziono wyników";
            }

            var link = paging[0];
            return $"Tytuł: {link.Title} Link: {link.Link}";
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
                return "Błąd - nie znaleziono wyników";
            }

            var link = paging[0];
            System.Console.WriteLine(link.Image.ThumbnailLink);
            return link.Image.ThumbnailLink;
            // Można też zrobić tak jak w funkcji GetYoutube (czyli uzyć foreach i dostać więcej wyników)
            // foreach (Result result in paging.Items)
            // {
            //     imageUrls.Add(result.Image.ThumbnailLink);
            // }
            // Coś w tym stylu, nie
        }
    }
}