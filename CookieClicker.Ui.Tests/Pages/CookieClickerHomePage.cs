using CookieClicker.Ui.Tests.Helpers.Interface;
using CookieClicker.Ui.Tests.Helpers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CookieClicker.Ui.Tests.Pages
{
    public class CookieClickerHomePage 
    {
        private SeleniumHelper _seleniumHelper;
        private IConfigurationHelper _configHelper;

        private readonly By _cookieClickerPagetTitle = By.XPath("//*[text()='Cookie Clicker!']");
        private readonly By _inputName = By.Name("name");
        private readonly By _btnStart = By.XPath("//button[text()='Start!']");
        private readonly By _lblNewGame = By.XPath("//h2[text()='New Game']");
        private readonly By _lblHighScores = By.XPath("//h2[text()='High Scores']");
        private readonly By _tableElement = By.TagName("table");

        public CookieClickerHomePage(SeleniumHelper seleniumHelper, IConfigurationHelper configHelper)
        {
            _seleniumHelper = seleniumHelper;
            _configHelper = configHelper;
        }


        public void NavigateToCookieClickerHomePage()
        {
            _seleniumHelper.SetPageUrl(_configHelper.GetCookieClickerSiteUrl());
        }

        public bool CheckCookieClickerHomePageIsLoaded()
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

        public bool CheckStartButtonIsVisible()
        {
            return _seleniumHelper.FindElement(_btnStart).Displayed;
        }


        public bool CheckLabelNewGameIsVisible()
        {
            return _seleniumHelper.FindElement(_lblNewGame).Displayed;
        }

        public bool CheckinputNameIsVisible()
        {
            return _seleniumHelper.FindElement(_inputName).Displayed;
        }

        public void ClickStartGameButton()
        {
            _seleniumHelper.ClickElement(_btnStart);
        }

        public void EnterName(string name)
        {
            _seleniumHelper.SendKeys_CatchStaleElementException(_inputName, name);
        }

        public void DeleteAllCookies()
        {
            _seleniumHelper.DeleteAllCookies();
        }

        public bool CheckHighScoresSectionIsVisible()
        {
            var labelVisible = _seleniumHelper.FindElement(_lblHighScores).Displayed;
            var tableVisible = _seleniumHelper.FindElement(_tableElement).Displayed;

            if (labelVisible && tableVisible)
                return true;
            else
                return false;

        }

    }
}
