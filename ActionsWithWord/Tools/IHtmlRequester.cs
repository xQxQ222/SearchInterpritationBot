﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchInterpritationBot.ActionsWithWord.Tools
{
    public interface IHtmlRequester
    {
        public Task<string> GetPage(string url);
    }
}