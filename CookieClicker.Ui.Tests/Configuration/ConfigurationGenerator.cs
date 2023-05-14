using CookieClicker.Ui.Tests.Constants;
using CookieClicker.Ui.Tests.Helpers.Configuration;
using BoDi;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.TestFramework;

namespace CookieClicker.Ui.Tests.Configuration
{
    public interface IConfigurationGenerator
    {
        public ConfigurationHelper BindConfig();
        public void CheckAndOverWriteEnvironmentVariables();
        public void SetInitialConfiguration(ConfigurationHelper configurationHelper);
    }

    [Binding]
    public class ConfigurationGenerator : IConfigurationGenerator
    {
        private readonly ITestRunContext _testRunContext;
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        private readonly IObjectContainer _objectContainer;

        public ConfigurationGenerator(ITestRunContext testRunContext, ScenarioContext scenarioContext, FeatureContext featureContext, IObjectContainer objectContainer)
        {
            _testRunContext = testRunContext;
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
            _objectContainer = objectContainer;
        }

        public void CheckAndOverWriteEnvironmentVariables()
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.TestPipeline)))
            {
                Environment.SetEnvironmentVariable(EnvironmentVariableKeys.TestPipeline, EnvironmentVariableValues.Test);
            }
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.TestEnvironment)))
            {
                Environment.SetEnvironmentVariable(EnvironmentVariableKeys.TestEnvironment, EnvironmentVariableValues.Test);
            }
        }

        public void SetInitialConfiguration(ConfigurationHelper configurationHelper)
        {
            if (!configurationHelper.ExternalConnections.CookieClickerSiteUrl.EndsWith("/"))
            {
                configurationHelper.ExternalConnections.CookieClickerSiteUrl = configurationHelper.ExternalConnections.CookieClickerSiteUrl + "/";
            }
        }

        public ConfigurationHelper BindConfig()
        {
            var environment = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.TestEnvironment);
            string appsettingPath = environment == null ? $"appsettings.json" : $"appsettings.{environment}.json";
            IConfiguration config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<ConfigurationGenerator>()
                .SetBasePath(_testRunContext.GetTestDirectory())
                .AddJsonFile(appsettingPath, optional: true).Build();

            ConfigurationHelper configurationHelper = new ConfigurationHelper(config);


            _objectContainer.RegisterInstanceAs(config);
            _objectContainer.RegisterInstanceAs(configurationHelper);

            return configurationHelper;
        }
    }
}
