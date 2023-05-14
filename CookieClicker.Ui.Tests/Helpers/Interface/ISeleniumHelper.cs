using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CookieClicker.Ui.Tests.Helpers.Interface
{
    public interface ISeleniumHelper
    {
        void ClickDateInDatePicker(By nextMonth, By datePicker, string date);
        void ClickElement(By element);
        void CloseDriver();
        string GetElementText(By element);
        bool GetPageLoadStatus();
        void QuitDriver();
        void SelectElementByText(By element, string text);
        void SendKeys(By element, string text);
        void SetPageUrl(string url);
        void WaitUntilDocumentIsReady(int timeoutInSeconds);
        void WaitUntilElementIsClickable(By element);
        void ZoomToElementWithID(string Id);
    }
}
