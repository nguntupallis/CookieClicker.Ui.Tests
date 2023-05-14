using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace CookieClicker.Ui.Tests.Helpers.Interface
{
    public interface IWebDriverHelper
    {
        public IWebDriver GetWebDriver();
        public WebDriverWait GetWebDriverWait(IWebDriver webDriver, double waitValue, double pollValue);
        public void WaitForElement(IWebDriver webDriver, By elementBy, int timeout = 10);
    }
}
