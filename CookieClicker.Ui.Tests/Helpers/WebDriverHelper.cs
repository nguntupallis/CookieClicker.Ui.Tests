using CookieClicker.Ui.Tests.Helpers.Interface;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using CookieClicker.Ui.Tests.Constants;
using WebDriverManager.DriverConfigs.Impl;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using Sayaka.Common;

namespace CookieClicker.Ui.Tests.Helpers
{
    public class WebDriverHelper : IWebDriverHelper
    {
        private IWebDriver _webDriver;
        private WebDriverWait _webDriverWait;
        private ChromeOptions _chromeOptions = new();

        private FirefoxOptions _fireFoxOptions = new();
        private EdgeOptions _edgeOptions = new();
        private FirefoxDriverService _firefoxDriverService;
        private Uri _url = new Uri("http://localhost:4444/wd/hub");
        string userAgent = ProviderFakeUserAgent.RandomComputer;

        public WebDriverHelper()
        {
            _chromeOptions.AddArguments("--headless", "--window-size=1920,2000");
            _chromeOptions.AddArgument("--start-maximized");
            _chromeOptions.AddArgument("--no-sandbox");
            _chromeOptions.AddArgument("--verbose");
            _chromeOptions.AddArgument("--disable-gpu");
            _chromeOptions.AddArgument("--disable-extensions");
            _chromeOptions.AddArgument("--disable-dev-shm-usage");
            _chromeOptions.AddArgument("disable-infobars");
            _chromeOptions.AddArgument($"user-agent={userAgent}");
            _edgeOptions.AddArgument($"user-agent={userAgent}");
            _fireFoxOptions.AddArgument($"user-agent={userAgent}");

            var testPipeline = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.TestPipeline);
            if (!testPipeline.Equals(EnvironmentVariableValues.Test, StringComparison.InvariantCultureIgnoreCase))
            {
                _chromeOptions.AddArguments("--headless", "--window-size=1920,1200");

            }

            var testEnvironment = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.TestEnvironment);
            if (testEnvironment == EnvironmentVariableValues.Local)
            {
                _chromeOptions.AcceptInsecureCertificates = true;
                _fireFoxOptions.AcceptInsecureCertificates = true;
                _edgeOptions.AcceptInsecureCertificates = true;
            }
            _fireFoxOptions.AddArgument("--headless");
            _edgeOptions.AddArgument("--headless");
        }

        public IWebDriver GetWebDriver()
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.Test_Browser)))
                {
                    Environment.SetEnvironmentVariable(EnvironmentVariableKeys.Test_Browser, EnvironmentVariableValues.Chrome);
                }
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.WebDriver)))
                {
                    Environment.SetEnvironmentVariable(EnvironmentVariableKeys.WebDriver, EnvironmentVariableValues.Remote);
                }
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WebDriver")))
            {
                if (Environment.GetEnvironmentVariable(EnvironmentVariableKeys.WebDriver).ToLower() == EnvironmentVariableValues.Remote)
                {
                    switch (Environment.GetEnvironmentVariable(EnvironmentVariableKeys.Test_Browser).ToLower())
                    {
                        case EnvironmentVariableValues.Edge:
                            {
                                return new RemoteWebDriver(_url, _edgeOptions);
                            }
                        case EnvironmentVariableValues.Chrome:
                            {
                                return new RemoteWebDriver(_url, _chromeOptions);
                            }
                        case EnvironmentVariableValues.BrowserNotSpecified:
                            {
                                return new RemoteWebDriver(_url, _chromeOptions);
                            }
                        case string browser: throw new NotSupportedException($"{browser} is not a supported browser");
                        default: throw new NotSupportedException("Not Supported Browser: <null>");
                    }
                }
            }
        
            // else
            // {
            //     if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.Test_Browser)))
            //     {
            //         Environment.SetEnvironmentVariable(EnvironmentVariableKeys.Test_Browser, EnvironmentVariableValues.Chrome);
            //     }

            //     string browser = Environment.GetEnvironmentVariable("Test_Browser").ToLower();
            //     switch (Environment.GetEnvironmentVariable(EnvironmentVariableKeys.Test_Browser).ToLower())
            //     {
            //         case EnvironmentVariableValues.Edge:
            //             {
            //                 var edgeConfig = new EdgeConfig();
            //                 var version = edgeConfig.GetMatchingBrowserVersion();
            //                 var envEdgeWebDriver = Environment.GetEnvironmentVariable("EdgeWebDriver");

            //                 if (string.IsNullOrEmpty(envEdgeWebDriver))
            //                 {
            //                     new WebDriverManager.DriverManager().SetUpDriver(edgeConfig, version);
            //                     _webDriver = new EdgeDriver(_edgeOptions);
            //                     _webDriver.Manage().Cookies.DeleteAllCookies();
            //                     return _webDriver;
            //                 }
            //                 else if (File.Exists(Path.Combine(envEdgeWebDriver, "msedgedriver.exe")))
            //                 {
            //                     var driverPath = Path.Combine(envEdgeWebDriver);
            //                     EdgeDriverService defaultService = EdgeDriverService.CreateDefaultService(driverPath);
            //                     defaultService.UseVerboseLogging = true;
            //                     defaultService.HideCommandPromptWindow = true;

            //                     _webDriver = new EdgeDriver(defaultService, _edgeOptions);
            //                     _webDriver.Manage().Cookies.DeleteAllCookies();
            //                     return _webDriver;
            //                 }
            //                 else
            //                     throw new DriverServiceNotFoundException("Driver Not installed <null>");

            //             }
            //         case EnvironmentVariableValues.Firefox:
            //             {
            //                 var firefoxConfig = new FirefoxConfig();
            //                 new WebDriverManager.DriverManager().SetUpDriver(firefoxConfig);
            //                 var envFirefoxWebDriver = Environment.GetEnvironmentVariable("GeckoWebDriver");
            //                 _firefoxDriverService = FirefoxDriverService.CreateDefaultService();

            //                 if (string.IsNullOrEmpty(envFirefoxWebDriver))
            //                 {
            //                     _firefoxDriverService.Host = "::1";
            //                     FirefoxDriver firefoxDriver = new FirefoxDriver(_firefoxDriverService, _fireFoxOptions);
            //                     firefoxDriver.Manage().Window.Maximize();
            //                     firefoxDriver.Manage().Cookies.DeleteAllCookies();
            //                     return firefoxDriver;
            //                 }
            //                 else if (File.Exists(Path.Combine(envFirefoxWebDriver, "geckodriver.exe")))
            //                 {
            //                     var driverPath = Path.Combine(envFirefoxWebDriver);
            //                     _firefoxDriverService = FirefoxDriverService.CreateDefaultService(driverPath);
            //                     _firefoxDriverService.Host = "::1";
            //                     FirefoxDriver firefoxDriver = new FirefoxDriver(_firefoxDriverService, _fireFoxOptions);
            //                     firefoxDriver.Manage().Cookies.DeleteAllCookies();
            //                     return firefoxDriver;
            //                 }
            //                 else
            //                     throw new DriverServiceNotFoundException("Driver not installed : <null>");
            //             }
            //         case EnvironmentVariableValues.Chrome:
            //             {
            //                 var chromeConfig = new ChromeConfig();
            //                 var version = chromeConfig.GetMatchingBrowserVersion();
            //                 var envChromeWebDriver = Environment.GetEnvironmentVariable("ChromeWebDriver");
            //                 if (string.IsNullOrEmpty(envChromeWebDriver))
            //                 {
            //                     new WebDriverManager.DriverManager().SetUpDriver(chromeConfig, version);
            //                     _webDriver = new ChromeDriver(_chromeOptions);
            //                     _webDriver.Manage().Cookies.DeleteAllCookies();
            //                     return _webDriver;
            //                 }
            //                 else if (File.Exists(Path.Combine(envChromeWebDriver, "chromewebdriver.exe")))
            //                 {
            //                     _chromeOptions.AddArguments("--headless", "--window-size=1920,2000");
            //                     var driverPath = Path.Combine(envChromeWebDriver);
            //                     ChromeDriverService defaultService = ChromeDriverService.CreateDefaultService(driverPath);
            //                     defaultService.HideCommandPromptWindow = true;
            //                     _webDriver = new ChromeDriver(defaultService, _chromeOptions);
            //                     _webDriver.Manage().Cookies.DeleteAllCookies();
            //                     return _webDriver;
            //                 }
            //                 else
            //                     throw new DriverServiceNotFoundException("Driver not installed: <null>");

            //             }
            //         case EnvironmentVariableValues.BrowserNotSpecified:
            //             {
            //                 var chromeConfig = new ChromeConfig();
            //                 var version = chromeConfig.GetMatchingBrowserVersion();
            //                 var envChromeWebDriver = Environment.GetEnvironmentVariable("ChromeWebDriver");
            //                 if (string.IsNullOrEmpty(envChromeWebDriver))
            //                 {
            //                     new WebDriverManager.DriverManager().SetUpDriver(chromeConfig, version);
            //                     _webDriver = new ChromeDriver(_chromeOptions);
            //                     _webDriver.Manage().Cookies.DeleteAllCookies();
            //                     return _webDriver;
            //                 }
            //                 else if (File.Exists(Path.Combine(envChromeWebDriver, "chromewebdriver.exe")))
            //                 {
            //                     _chromeOptions.AddArguments("--headless", "--window-size=1920,2000");
            //                     var driverPath = Path.Combine(envChromeWebDriver);
            //                     ChromeDriverService defaultService = ChromeDriverService.CreateDefaultService(driverPath);
            //                     defaultService.HideCommandPromptWindow = true;
            //                     _webDriver = new ChromeDriver(defaultService, _chromeOptions);
            //                     _webDriver.Manage().Cookies.DeleteAllCookies();
            //                     return _webDriver;
            //                 }
            //                 else
            //                     throw new DriverServiceNotFoundException("Driver not installed: <null>");
            //             }
            //         case string browser1: throw new NotSupportedException($"{browser1} is not a supported browser");
            //         default: throw new NotSupportedException("Not Supported Browser: <null>");
            //     }
            // }
            throw new Exception("Web Driver Variable not set");

        }

        public WebDriverWait GetWebDriverWait(IWebDriver webDriver, double waitValue, double pollValue)
        {
            _webDriverWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(waitValue))
            {
                PollingInterval = TimeSpan.FromSeconds(pollValue)
            };

            return _webDriverWait;
        }

        public void WaitForElement(IWebDriver webDriver, By elementBy, int timeout = 10)
        {
            WebDriverWait webDriverWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(timeout));
            webDriverWait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(ElementNotVisibleException));

            webDriverWait.Until(webDriver =>
            {
                IWebElement webElement = webDriver.FindElement(elementBy);
                if (webElement !=null && webElement.Displayed)
                {
                    return webElement;
                }
                return null;
            });
        }
    }
}
