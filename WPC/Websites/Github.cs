using System.Threading;

using OpenQA.Selenium;

namespace WPC.Websites
{
    
    public class Github : IWebInteractor
    {

        public override void login(string email, string password)
        {
            Thread.Sleep (2000);
            _driver.Navigate().GoToUrl("https://github.com/login");

            IWebElement emailInput = waitForElement(_driver, "#login_field");
            emailInput.SendKeys(email);

            IWebElement passwordInput = waitForElement(_driver, "#password");
            passwordInput.SendKeys(password);
            passwordInput.SendKeys(Keys.Return);
        }

        public override void changePassword(string currentPassword, string newPassword)
        {
            Thread.Sleep (2000);

            _driver.Navigate().GoToUrl("https://github.com/settings/admin");

            IWebElement passwordInput = waitForElement(_driver, "#user_old_password");
            passwordInput.SendKeys(currentPassword);


            IWebElement newPasswordInput = waitForElement(_driver, "#user_new_password");
            newPasswordInput.SendKeys(newPassword);


            IWebElement confirmNewPasswordInput = waitForElement(_driver, "#user_confirm_new_password");
            confirmNewPasswordInput.SendKeys(newPassword);
            confirmNewPasswordInput.SendKeys(Keys.Return);
        }
    }
}