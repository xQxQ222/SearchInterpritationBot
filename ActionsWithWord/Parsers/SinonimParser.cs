using AngleSharp.Dom;
using AngleSharp;
using SearchInterpritationBot.ActionsWithWord.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchInterpritationBot.ActionsWithWord.Parsers
{
    public class SinonimParser : IParser
    {
        private string _url = "https://sinonim.org/t/*";
        public SinonimParser(IHttpClientFactory httpClientFactory)
        {
            this.htmlRequester = new DefinitionRequester(httpClientFactory);
        }

        private async Task<IDocument> GetDocument(string pageUrl)
        {
            var pageContent = await htmlRequester.GetPage(pageUrl);
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(pageContent));
            return document;
        }

        public IHtmlRequester htmlRequester { get; set; }

        public async Task<string> Parse(string requestedWord)
        {
            _url = _url.Replace("*", requestedWord);
            var document = await GetDocument(_url);
            var pageContent = document.QuerySelector("ol");
            if (pageContent == null)
                return string.Empty;
            return $"(Источник sinonim.org)\n{requestedWord.ToUpperInvariant()} - \n{pageContent.TextContent}";
        }
    }
}
