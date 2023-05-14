using BoDi;
using CookieClicker.Ui.Tests.Configuration;
using CookieClicker.Ui.Tests.Helpers.Configuration;
using CookieClicker.Ui.Tests.Pages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.TestFramework;
using CookieClicker.Ui.Tests.Helpers.Interface;
using CookieClicker.Ui.Tests.Helpers;

namespace CookieClicker.Ui.Tests.Hooks
{
    [Binding]
    public class BeforeBindingHooks
    {
        private readonly IObjectContainer _objectContainer;
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;
        private readonly ITestRunContext _testRunContext;
        private readonly IConfigurationGenerator _configurationGenerator;
        private SeleniumHelper _seleniumHelper;
        private IWebDriverHelper _webDriverHelper;
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        public BeforeBindingHooks(IObjectContainer objectContainer, ScenarioContext scenarioContext, FeatureContext featureContext, ITestRunContext testRunContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
            _testRunContext = testRunContext;
            _configurationGenerator = new ConfigurationGenerator(_testRunContext, _scenarioContext, _featureContext, _objectContainer);
        }

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.test.json")
                .AddEnvironmentVariables()
                .Build();
            return config;
        }



        [BeforeScenario]
        public void BeforeScenario()
        {
            _configurationGenerator.CheckAndOverWriteEnvironmentVariables();
            var configurationHelper = _configurationGenerator.BindConfig();
            _configurationGenerator.SetInitialConfiguration(configurationHelper);
            BindHelpers(configurationHelper);
            BindPages(configurationHelper);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _seleniumHelper.CloseDriver();
            _seleniumHelper.QuitDriver();
        }

        public void BindHelpers(ConfigurationHelper configurationHelper)
        {
            _objectContainer.RegisterTypeAs<WebDriverHelper, IWebDriverHelper>().InstancePerContext();
            _webDriverHelper = _objectContainer.Resolve<IWebDriverHelper>();
            _seleniumHelper = new SeleniumHelper(_webDriverHelper);
        }

        public void BindPages(ConfigurationHelper configurationHelper)
        {
            var cookieClickerHomePage = new CookieClickerHomePage(_seleniumHelper, configurationHelper);
            var cookieClickerGamePage = new CookieClickerGamePage(_seleniumHelper, configurationHelper);
            _objectContainer.RegisterInstanceAs(cookieClickerHomePage);
            _objectContainer.RegisterInstanceAs(cookieClickerGamePage);
        }

    }
}
