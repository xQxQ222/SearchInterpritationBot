using Microsoft.Extensions.Configuration;
using SearchInterpritationBot.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchInterpritationBot.Config
{
    public class Config
    {
        public static SettingsStorage BotSettings { get; private set; } = new SettingsStorage();
        public static void SetProperties(IConfiguration configuration)
        {
            BotSettings = GetSection<SettingsStorage>(configuration, "StorageTgBotSets");
        }
        private static T GetSection<T>(IConfiguration configuration, string sectionName)
        {
            return configuration.GetSection(sectionName).Get<T>()
                ?? throw new InvalidOperationException($"Not found section {nameof(T)} in configuration.");
        }
    }
}