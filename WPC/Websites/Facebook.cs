using System.Threading;
using OpenQA.Selenium;

namespace WPC.Websites
{
    public class Facebook : IWebInteractor
    {

        public override void login(string email, string password)
        {
            Thread.Sleep (2000);
            _driver.Navigate ().GoToUrl ("https://www.facebook.com/");

            IWebElement emailInput = waitForElement (_driver, "#email");
            emailInput.SendKeys (email);

            IWebElement passwordInput = waitForElement (_driver, "#pass");
            passwordInput.SendKeys (password);
            passwordInput.SendKeys (OpenQA.Selenium.Keys.Return);
        }

        public override void changePassword(string currentPassword, string newPassword)
        {
            Thread.Sleep (2000);

            _driver.Navigate ().GoToUrl ("https://www.facebook.com/settings?tab=security&section=password&view");

            IWebElement passwordInput = waitForElement (_driver, "#password_old");
            passwordInput.SendKeys (currentPassword);


            IWebElement newPasswordInput = waitForElement (_driver, "#password_new");
            newPasswordInput.SendKeys (newPassword);


            IWebElement confirmNewPasswordInput = waitForElement (_driver, "#password_confirm");
            confirmNewPasswordInput.SendKeys (newPassword);
            confirmNewPasswordInput.SendKeys (OpenQA.Selenium.Keys.Return);
        }
    }
}