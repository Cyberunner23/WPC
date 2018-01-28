using System.Threading;
using OpenQA.Selenium;

namespace WPC.Websites
{
    public class Twitter : IWebInteractor
    {

        public override void login(string email, string password)
        {
            Thread.Sleep (2000);
            _driver.Navigate ().GoToUrl ("https://twitter.com/login");

            IWebElement emailInput = waitForElement (_driver, "input.js-username-field.email-input.js-initial-focus");
            emailInput.SendKeys (email);

            Thread.Sleep (2000);

            IWebElement passwordInput = waitForElement (_driver, "input.js-password-field");
            passwordInput.SendKeys (password);
            passwordInput.SendKeys (OpenQA.Selenium.Keys.Return);
        }

        public override void changePassword(string currentPassword, string newPassword)
        {
            Thread.Sleep (2000);

            _driver.Navigate ().GoToUrl ("https://twitter.com/settings/password");

            IWebElement passwordInput = waitForElement (_driver, "input[name=current_password]");
            passwordInput.SendKeys (currentPassword);

            Thread.Sleep (2000);

            IWebElement newPasswordInput = waitForElement (_driver, "input[name=user_password]");
            newPasswordInput.SendKeys (newPassword);

            Thread.Sleep (2000);

            IWebElement confirmNewPasswordInput = waitForElement (_driver, "input[name=user_password_confirmation]");
            confirmNewPasswordInput.SendKeys (newPassword);
            confirmNewPasswordInput.SendKeys (OpenQA.Selenium.Keys.Return);
        }
    }
}