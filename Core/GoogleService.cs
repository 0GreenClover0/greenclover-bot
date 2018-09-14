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

            IList<Result> paging = new List<Result>();

            paging = listRequest.Execute().Items;

            if (paging != null)
            {
                var link = paging[0];
                return $"Tytuł: {link.Title} Link: {link.Link}";
                // Można też zrobić tak jak w funkcji GetYoutube (czyli uzyć foreach i dostać więcej wyników),
            }

            else if (paging == null)
            {
                return "Błąd - nie znaleziono wyników";
            }

            else
            {
                return "Nieznany błąd";
            }
        }
    }
}