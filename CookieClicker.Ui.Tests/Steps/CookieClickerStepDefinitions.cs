using CookieClicker.Ui.Tests.Pages;
using TechTalk.SpecFlow;
using FluentAssertions;
using FluentAssertions.Execution;
using System;


namespace CookieClicker.Ui.Tests.Steps
{
    [Binding]
    public sealed class CookieClickerStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly CookieClickerHomePage _cookieClickerHomePage;
        private readonly CookieClickerGamePage _cookieClickerGamePage;

        public CookieClickerStepDefinitions(ScenarioContext scenarioContext, CookieClickerHomePage cookieClickerHomePage, CookieClickerGamePage cookieClickerGamePage)
        {
            _scenarioContext = scenarioContext;
            _cookieClickerHomePage = cookieClickerHomePage;
            _cookieClickerGamePage = cookieClickerGamePage;
        }

        [Given(@"I navigate to the Cookie Clicker website")]
        public void GivenINavigateToCookieClickerHomePage()
        {
            _cookieClickerHomePage.NavigateToCookieClickerHomePage();
        }

          [Then(@"the page title should be ""(.*)""")]
        public void ThenThePageTitleShouldBe(string expectedTitle)
        {
            var actualTitle = _cookieClickerHomePage.GetPageTitle(); // Implement the code to retrieve the actual page title
              using (new AssertionScope())
            {
                expectedTitle.Should().Be(actualTitle);
            }
        }

        [Then(@"there should be a `Start!` button")]
        public void ThenThereShouldBeAButton()
        {
            using (new AssertionScope())
            {
                _cookieClickerHomePage.CheckStartButtonIsVisible().Should().Be(true);
            }
        }

        [Then(@"there should be a `New Game` label")]
        public void ThenThereShouldBeANewGameLabel()
        {
            using (new AssertionScope())
            {
                _cookieClickerHomePage.CheckLabelNewGameIsVisible().Should().Be(true);
            }
        }

        [Then(@"there should be a text input field labeled `Your name:`")]
        public void ThenThereShouldBeATextInputFieldLabeledYourName()
        {
            using (new AssertionScope())
            {
                _cookieClickerHomePage.CheckinputNameIsVisible().Should().Be(true);
            }
        }

        [Then(@"there should be a `High Scores` section")]
        public void ThenThereShouldBeAHighScoresSection()
        {
            using (new AssertionScope())
            {
                _cookieClickerHomePage.CheckHighScoresSectionIsVisible().Should().Be(true);
            }
        }

        [Given(@"I am on the Cookie clicker home page")]
        public void GivenIAmOnTheGamePage()
        {
            _cookieClickerHomePage.NavigateToCookieClickerHomePage();
        }

        [When(@"I enter a (.*)")]
        public void WhenIEnterA(string name)
        {
            _cookieClickerHomePage.EnterName(name);
        }

        [When(@"I click the `Start` button")]
        public void WhenIClickTheStartButton()
        {
            _cookieClickerHomePage.ClickStartGameButton();
        }

        [Then(@"the Cookie Clicker game page should load")]
        public void ThenTheCookieClickerGamePageShouldLoad()
        {
            _cookieClickerGamePage.CheckGamePageIsVisible();
        }

        [Then(@"the `Click Cookie!` button should be visible")]
        public void ThenTheClickCookieButtonShouldBeVisible()
        {
            _cookieClickerGamePage.CheckClickCookieButtonIsVisible();
        }

        [Then(@"the `Sell Cookies!` button should be visible")]
        public void ThenTheSellCookiesButtonShouldBeVisible()
        {
            _cookieClickerGamePage.CheckSellCookiesButtonIsVisible();
        }

        [Then(@"the `Buy Factories!` button should be visible")]
        public void ThenTheBuyFactoriesButtonShouldBeVisible()
        {
            _cookieClickerGamePage.CheckBuyFactoriesButtonIsVisible();
        }

    }
}