using CookieClicker.Ui.Tests.Helpers.Interface;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace CookieClicker.Ui.Tests.Helpers
{
    public class SeleniumHelper
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;
        private IJavaScriptExecutor _js;
        private IWebDriverHelper _webDriverHelper;
        public SeleniumHelper(IWebDriverHelper webDriverHelper)
        {
            _webDriverHelper = webDriverHelper;
            _driver = _webDriverHelper.GetWebDriver();
            _wait = _webDriverHelper.GetWebDriverWait(_driver, 30, 2);
            _js = (IJavaScriptExecutor)_driver;
        }

        public void WaitUntilDocumentIsReady(int timeoutInSeconds)
        {
            var javaScriptExecutor = _driver as IJavaScriptExecutor;
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));

            try
            {
                Func<IWebDriver, bool> readyCondition = webDriver => (bool)javaScriptExecutor.ExecuteScript("return (document.readyState == 'complete' && jQuery.active == 0)");
                wait.Until(readyCondition);
            }
            catch (InvalidOperationException)
            {
                wait.Until(wd => javaScriptExecutor.ExecuteScript("return document.readyState").ToString() == "complete");
            }
        }

        public void WaitUntilElementIsClickable(By element, IWebElement webElement = null)
        {
            if (element != null)
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(_driver.FindElement(element)));
            }
            if (webElement != null)
            {
                _wait.Until(ExpectedConditions.ElementToBeClickable(webElement));
            }
        }

        public void SetPageUrl(string url)
        {
            _driver.Url = url;
        }

        public void CloseDriver()
        {
            _driver.Close();
        }

        public void QuitDriver()
        {
            _driver.Quit();
        }

        public bool GetPageLoadStatus()
        {
            var javaScriptExecutor = _driver as IJavaScriptExecutor;
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));

            try
            {
                Func<IWebDriver, bool> readyCondition = webDriver => (bool)javaScriptExecutor.ExecuteScript("return (document.readyState == 'complete')");
                wait.Until(readyCondition);
                return true;
            }
            catch (InvalidOperationException)
            {
                wait.Until(wd => javaScriptExecutor.ExecuteScript("return document.readyState").ToString() == "complete");
                return false;
            }
        }

        public void SendKeys(By element, string text)
        {
            _driver.FindElement(element).SendKeys(text);
        }

        public void ClickElement(By element)
        {
            try
            {
                _driver.FindElement(element).Click();
            }
            catch (Exception ex)
            {
                if (ex is ElementClickInterceptedException)
                {
                    WaitUntilElementIsClickable(element);
                    _driver.FindElement(element).Click();
                }
            }
        }

        public void SelectElementByText(By element, string text)
        {
            SelectElement selectElement = new SelectElement(_driver.FindElement(element));            
            selectElement.SelectByText(text);
        }

        public void SelectElementByTextCatchNoElementException(By element, string text, string url)
        {
            try
            {
                SelectElement selectElement = new SelectElement(_driver.FindElement(element));
                selectElement.SelectByText(text);
            }
            catch (Exception ex)
            {
                if (ex is NoSuchElementException)
                {
                    _driver.Url = url;
                }
            }
        }

        public void ZoomToElementWithID(string Id)
        {
            _js.ExecuteScript($"document.getElementById('{Id}').scrollIntoView('true')");
        }

        public string GetElementText(By element)
        {
            string text = _driver.FindElement(element).Text;
            return text;
        }

        public void ClickDateInDatePicker(By nextMonth, By datePicker, string date)
        {
            ClickElement(nextMonth);
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> elementList = _driver.FindElements(datePicker);

            foreach (IWebElement element in elementList) // use foreach iterate each cell.
            {
                string dateElement = element.Text; // get the text of the element.

                if (!dateElement.Equals(" ") && int.Parse(dateElement) < 10)
                {
                    dateElement = "0" + dateElement;
                }

                if (dateElement.Equals(date)) // check if the date matches the input date parameter
                {
                    element.Click(); // if the date matches, select it.
                    break;
                }
            }
        }

        public void ExecuteScript(string script)
        {
            _js.ExecuteScript(script);
        }
        public void ClearElement(By element)
        {
            _driver.FindElement(element).Clear();
        }

        public void PerformAction(By element, string action)
        {
            Actions actions = new(_driver);

            switch (action)
            {
                case "draganddrop":
                    var draganddrop = actions.ClickAndHold(_driver.FindElement(element)).MoveByOffset(10, 0).MoveByOffset(10, 5).Release().Build();
                    draganddrop.Perform();
                    draganddrop.Perform();
                    break;
                case "movetoelement":
                    var canvasElement = _driver.FindElement(element);
                    actions.MoveToElement(canvasElement, 250, 80).KeyDown(Keys.Control)
                        .ContextClick().MoveByOffset(250, 10).ContextClick()
                        .MoveByOffset(100, 270).ContextClick()
                        .MoveByOffset(-200, 110).ContextClick()
                        .MoveByOffset(-10, -300).ContextClick()
                        .MoveByOffset(-140, -90).ContextClick().KeyUp(Keys.Control).Perform();
                    break;
            }
        }

        public void PerformActionOnWebElement(IWebElement element, string action)
        {
            Actions actions = new(_driver);

            switch (action)
            {
                case "draganddrop":
                    var draganddrop = actions.ClickAndHold(element).MoveByOffset(10, 0).MoveByOffset(10, 5).Release().Build();
                    draganddrop.Perform();
                    draganddrop.Perform();
                    break;
                case "movetoelement":
                    var canvasElement = element;
                    try
                    {
                        actions.MoveToElement(canvasElement)
                          .MoveByOffset(canvasElement.Location.X, canvasElement.Location.Y)
                          .MoveToElement(canvasElement)
                          .Click(canvasElement)
                          .Build()
                          .Perform();
                    }
                    catch (Exception ex)
                    {
                        if (ex is MoveTargetOutOfBoundsException)
                        {
                            /// Do Nothing;
                        }
                    }
                    break;
            }
        }

        public string GetElementAttributeValue(By element)
        {
            return _driver.FindElement(element).GetAttribute("value");
        }

        public string GetElementAttribute(By element, string attribute)
        {
            string text = "";

            try
            {
                text = _driver.FindElement(element).GetAttribute(attribute);
            }
            catch (Exception ex)
            {
                if (ex is StaleElementReferenceException || ex is ElementClickInterceptedException)
                {
                    Refresh();
                    text = _driver.FindElement(element).GetAttribute(attribute);

                }
            }
            return text;
        }

        public IWebElement FindElement(By element)
        {
            return _driver.FindElement(element);
        }

        public IList<IWebElement> FindElements(By element)
        {
            return _driver.FindElements(element);
        }

        public void WaitForPageLoad()
        {
            _wait.Until(_driver => _js.ExecuteScript("return document.readyState").Equals("complete"));
        }


        public void WaitUntilElementisInvisible(By element)
        {
            _wait = _webDriverHelper.GetWebDriverWait(_driver, 60, 5);
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(element));
        }

        public void WaitUntilElementIsAvailable(By element)
        {
            _wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(element));
            _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(element));
            _wait.Until(ExpectedConditions.ElementToBeClickable(element));
        }
        public void WaitUntilElementIsAvailableWithoutPresenceOfAllElements(By element)
        {
            _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(element));
            _wait.Until(ExpectedConditions.ElementToBeClickable(element));
        }

        /// <summary>
        /// Common helper function to click elements
        /// </summary>
        /// <param name="element"></param>
        public void Click_CatchStaleElementException(By element)
        {
            Thread.Sleep(500);
            try
            {
                WaitUntilElementIsAvailable(element);
                if (FindElement(element).Displayed)
                {
                    FindElement(element).Click();
                }
            }
            catch (Exception ex)
            {
                if (ex is StaleElementReferenceException)
                {
                    _driver.Navigate().Refresh();
                    WaitUntilElementIsAvailable(element);
                    FindElement(element).Click();
                }
                if (ex is ElementClickInterceptedException)
                {
                    DateTime date = DateTime.Now.AddSeconds(30);

                    while (ex is ElementClickInterceptedException && DateTime.Now < date)
                    {
                        WaitUntilElementIsAvailable(element);
                        try
                        {
                            FindElement(element).Click();
                            return;
                        }
                        catch (ElementClickInterceptedException)
                        {
                            //Keep looping until no more ElementClickInterceptedException or Timeout of 30 seconds is reached
                        }
                        catch (StaleElementReferenceException)
                        {
                            _driver.Navigate().Refresh();
                            _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(element));
                            _wait.Until(ExpectedConditions.ElementToBeClickable(element));

                            FindElement(element).Click();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Common helper function to send keys to elements
        /// </summary>
        /// <param name="element"></param>
        /// <param name="keys"></param>
        public void SendKeys_CatchStaleElementException(By element, string keys)
        {
            Thread.Sleep(500);
            try
            {
                _wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(element));
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(element));
                if (FindElement(element).Displayed)
                {
                    FindElement(element).SendKeys(keys);
                }
            }
            catch (Exception ex)
            {
                if (ex is StaleElementReferenceException)
                {
                    _driver.Navigate().Refresh();
                    _wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(element));
                    _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(element));
                    FindElement(element).SendKeys(keys);
                }
            }
        }

        public void ScrollToBottom()
        {
            long scrollHeight = 0;

            do
            {
                var newScrollHeight = (long)_js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight); return document.body.scrollHeight;");

                if (newScrollHeight == scrollHeight)
                {
                    break;
                }
                else
                {
                    scrollHeight = newScrollHeight;
                }
            } while (true);
        }

        public void ScrollToTop()
        {
            _js.ExecuteScript("window.scrollTo(0, 0)");
        }
        public void ScrollTo(int xPosition = 0, int yPosition = 0)
        {
            var x = String.Format("window.scrollTo({0}, {1})", xPosition, yPosition);
            _js.ExecuteScript(x);
        }

        public void ScrollToView(By element)
        {
            IWebElement scrollElement = _driver.FindElement(element);
            if (scrollElement.Location.Y > 200)
            {
                ScrollTo(0, scrollElement.Location.Y - 100);
            }
            else if (scrollElement.Location.Y == 0 || scrollElement.Location.Y > 0)
            {
                ScrollToBottom();
            }
        }

        public void ScrollToViewElement(IWebElement element)
        {            
            if (element.Location.Y > 200)
            {
                ScrollTo(0, element.Location.Y - 100);
            }
            else if (element.Location.Y == 0 || element.Location.Y > 0)
            {
                ScrollToBottom();
            }
        }


        public void ScrollIntoViewWithElementId(string id)
        {
            string script = $"document.getElementById('{id}').scrollIntoView(true);";
            _js.ExecuteScript(script);
        }

        public void ScrollIntoViewWithElementText(string element, string text, int instance)
        {
            string script = $"$(`{element}:contains('{text}')`)[{instance}].scrollIntoView('true')";
            _js.ExecuteScript(script);
        }

        /// <summary>
        /// Execute Javascript to scroll to the item
        /// </summary>
        /// <param name="elementId"></param>
        public void ScrollInToView(string elementId)
        {
            string script = $"if(document.getElementById('{elementId}')){{console.log('scrolling into view {elementId}!!');document.getElementById('{elementId}').scrollIntoView(true);}}";
            _js.ExecuteScript(script);
            Thread.Sleep(200);//wait for element to scroll
        }

        public void ScrollInToViewWithTagName(string tagName)
        {
            string script = $"if(document.getElementsByTagName('{tagName}')){{console.log('scrolling into view {tagName}!!');document.getElementsByTagName('{tagName}').scrollIntoView(true);}}";
            _js.ExecuteScript(script);
            Thread.Sleep(200);//wait for element to scroll
        }


        public string GetPageUrl()
        {
            return _driver.Url;
        }
        public void WaitUntilAllElementsPresent(By element)
        {
            _wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(element));
        }

        public void WaitUntilElementExists(By element)
        {
            _wait.Until(ExpectedConditions.ElementExists(element));
        }

        public void WaitUntilVisibilityOfElementLocatedBy(By element)
        {
            _wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(element));
            _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(element));
        }

        public void AddCookie(string cookieName, string cookieValue)
        {
            _driver.Manage().Cookies.AddCookie(new Cookie(cookieName, cookieValue));
        }

        public void DeleteAllCookies()
        {
            var cookiesBeforeDeletion = _driver.Manage().Cookies.AllCookies;
            Console.Write(cookiesBeforeDeletion + "mayukhaBefore");
            _driver.Manage().Cookies.DeleteAllCookies();
            _driver.Navigate().Refresh();
            var cookiesAfterDeletion = _driver.Manage().Cookies.AllCookies;

            Console.Write(cookiesAfterDeletion + "mayukhaAfter");
        }

        public string ExecuteScriptGetString(string script)
        {
            return (string)_js.ExecuteScript(script);
        }

        public string GetSelectedElementLabel(By element)
        {
            var selectedElement = _driver.FindElement(element);
            var value = selectedElement.Selected;
            return value.ToString();
        }
        public void SwitchToFrameElement(By element)
        {
            var findelement = _driver.FindElement(element);
            _driver.SwitchTo().Frame(findelement);
        }
        public void SwitchToParentFrame()
        {
            _driver.SwitchTo().ParentFrame();
        }
        public void SwitchToActiveElement()
        {
            _driver.SwitchTo().ActiveElement();
        }

        public T ExecuteScriptWithReturnValue<T>(string script)
        {
            try
            {
                return (T)_js.ExecuteScript(script);
            }
            catch
            {
                //We will just swallow exception and return default value
            }
            return default;
        }

        public bool CheckElementIsPresent(By element)
        {
            bool elementPresent = false;

            try
            {
                elementPresent = _driver.FindElement(element).Displayed;
            }
            catch (Exception ex)
            {
                if (ex is NoSuchElementException || ex is WebDriverTimeoutException)
                {
                    elementPresent = false;
                }
            }

            return elementPresent;
        }

        public string GetElementTextValue(By element)
        {
            return _driver.FindElement(element).GetAttribute("value");
        }

        public string GetElementTextWithElementId(string Id)
        {
            string _script = $"return document.getElementById('{Id}').value;";
            var _text = _js.ExecuteScript(_script);
            return (string)_text;
        }

        public void WaitForElementToBeLoaded(By elementToFind)
        {
            var refreshDriverOnce = FindElements(elementToFind);
            int count = 0;
            while (refreshDriverOnce.Count == 0)
            {
                Thread.Sleep(1000);
                refreshDriverOnce = FindElements(elementToFind);
                count++;
                if (count == 10)
                {
                    break;
                }
            }
        }

        public void Refresh()
        {
            _driver.Navigate().Refresh();
        }
    }
}
