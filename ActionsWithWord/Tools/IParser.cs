using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchInterpritationBot.ActionsWithWord.Tools
{
    public interface IParser
    {
        public Task<string> Parse(string requestedWord);

        public IHtmlRequester htmlRequester { get; set; }
    }
}
