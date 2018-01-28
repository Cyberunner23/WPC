
using System.Threading;
using OpenQA.Selenium;

namespace WPC.Websites
{
    
    public class Gmail : IWebInteractor
    {

        public override void login(string email, string password)
        {
            Thread.Sleep (2000);
            _driver.Navigate ().GoToUrl ("https://accounts.google.com/signin/v2/identifier");

            IWebElement emailInput = waitForElement (_driver, "input[name=identifier]");
            emailInput.SendKeys (email);
            emailInput.SendKeys (OpenQA.Selenium.Keys.Return);

            IWebElement passwordInput = waitForElement (_driver, "input[name=password]");
            passwordInput.SendKeys (password);
            passwordInput.SendKeys (OpenQA.Selenium.Keys.Return);
        }

        public override void changePassword(string currentPassword, string newPassword)
        {
            Thread.Sleep (2000);

            _driver.Navigate ().GoToUrl ("https://myaccount.google.com/signinoptions/password");

            IWebElement passwordInput = waitForElement (_driver, "input[name=password]");
            passwordInput.SendKeys (currentPassword);
            passwordInput.SendKeys (OpenQA.Selenium.Keys.Return);

            Thread.Sleep (2000);

            IWebElement newPasswordInput = waitForElement (_driver, "input[name=password]");
            newPasswordInput.SendKeys (newPassword);

            Thread.Sleep (2000);

            IWebElement confirmNewPasswordInput = waitForElement (_driver, "input[name=confirmation_password]");
            confirmNewPasswordInput.SendKeys (newPassword);
            confirmNewPasswordInput.SendKeys (OpenQA.Selenium.Keys.Return);
        }
    }
}