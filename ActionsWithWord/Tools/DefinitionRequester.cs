using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace SearchInterpritationBot.ActionsWithWord.Tools
{
    public class DefinitionRequester:IHtmlRequester
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefinitionRequester(IHttpClientFactory clientFactory)
        {
            this._httpClientFactory = clientFactory;
        }

        public async Task<string> GetPage(string url)
        {
            var client = _httpClientFactory.CreateClient("DefinitionClient");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 YaBrowser/24.4.0.0 Safari/537.36");
            var res = await client.GetStringAsync(url);
            return res;
        }
    }
}
