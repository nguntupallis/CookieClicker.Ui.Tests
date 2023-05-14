using CookieClicker.Ui.Tests.Helpers.Interface;
using CookieClicker.Ui.Tests.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CookieClicker.Ui.Tests.Pages
{
    public class CookieClickerGamePage 
    {
        private SeleniumHelper _seleniumHelper;
        private IConfigurationHelper _configHelper;

        private readonly By _cookieClickerPagetTitle = By.XPath("//*[text()='Cookie Clicker!']");
        private readonly By _welcomeName = By.XPath("//p[contains(text(), 'Hello')]");
        private readonly By _btnClickCookie = By.Id("click");
        private readonly By _lblCookieCount = By.Id("cookies");
        private readonly By _lblFactoryCount = By.Id("factories");
        private readonly By _lblmoneyCount = By.Id("money");
        private readonly By _btnSellCookies = By.Id("sell-cookies");
        private readonly By _btnBuyFactories = By.Id("buy-factories");
        private readonly By _inputSellCookies = By.Id("cookies-to-sell");

        private readonly By _inputBuyFactories = By.Id("factories-to-buy");

        public CookieClickerGamePage(SeleniumHelper seleniumHelper, IConfigurationHelper configHelper)
        {
            _seleniumHelper = seleniumHelper;
            _configHelper = configHelper;
        }


        public void NavigateToCookieClickerHomePagee()
        {
            _seleniumHelper.SetPageUrl(_configHelper.GetCookieClickerSiteUrl());
        }

        public bool CheckCookieClickerGamePageIsLoaded()
        {
            return _seleniumHelper.GetPageLoadStatus();
        }

        public string GetPageTitle()
        {
            _seleniumHelper.WaitUntilAllElementsPresent(_cookieClickerPagetTitle);
            return _seleniumHelper.FindElement(_cookieClickerPagetTitle).GetAttribute("text");
        }

        public bool CheckPageTitleIsVisible()
        {
            return _seleniumHelper.FindElement(_cookieClickerPagetTitle).Displayed;
        }

        public bool CheckClickCookieButtonIsVisible()
        {
            return _seleniumHelper.FindElement(_btnClickCookie).Displayed;
        }

        public void ClickCookieButton()
        {
            _seleniumHelper.ClickElement(_btnClickCookie);
        }

        public void ClickSellCookiesButton()
        {
            _seleniumHelper.ClickElement(_btnSellCookies);
        }

        public void ClickBuyFactoriesButton()
        {
            _seleniumHelper.ClickElement(_btnBuyFactories);
        }

        public bool CheckSellCookiesButtonIsVisible()
        {
            return _seleniumHelper.FindElement(_btnSellCookies).Displayed;
        }


        public bool CheckBuyFactoriesButtonIsVisible()
        {
            return _seleniumHelper.FindElement(_btnBuyFactories).Displayed;
        }

        public bool CheckLabelCookieCountIsVisible()
        {
            return _seleniumHelper.FindElement(_lblCookieCount).Displayed;
        }

        public bool CheckLabelFactoryCountIsVisible()
        {
            return _seleniumHelper.FindElement(_lblFactoryCount).Displayed;
        }

        public bool CheckLabelMoneyCountIsVisible()
        {
            return _seleniumHelper.FindElement(_lblmoneyCount).Displayed;
        }

        public bool CheckWelcomeNameIsVisible()
        {
            return _seleniumHelper.FindElement(_welcomeName).Displayed;
        }

        public string GetWelcomeName()
        {
            return _seleniumHelper.FindElement(_welcomeName).Text;
        }

        public void DeleteAllCookies()
        {
            _seleniumHelper.DeleteAllCookies();
        }

        public bool CheckGamePageIsVisible()
        {
            var btnClickCookieVisible = _seleniumHelper.FindElement(_btnClickCookie).Displayed;
            var welcomeNameVisible = _seleniumHelper.FindElement(_welcomeName).Displayed;

            if (btnClickCookieVisible && welcomeNameVisible)
                return true;
            else
                return false;

        }

    }
}
