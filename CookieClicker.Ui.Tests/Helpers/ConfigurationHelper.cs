using CookieClicker.Ui.Tests.Helpers.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookieClicker.Ui.Tests.Helpers.Configuration
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public ExternalConnections ExternalConnections { get; }

        public ConfigurationHelper(IConfiguration config)
        {
            ExternalConnections = config.GetSection(nameof(ExternalConnections)).Get<ExternalConnections>();
        }

        public string GetCookieClickerSiteUrl()
        {
            return ExternalConnections.CookieClickerSiteUrl;
        }
    }
}
