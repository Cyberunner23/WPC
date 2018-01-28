using System;
using System.IO;
using System.Reflection;
using KeePass.Plugins;
using System.Windows.Forms;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Linq;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;
using KeePassLib.Utility;

namespace WPC
{
    public class WPCExt : Plugin 
    {

        private IPluginHost _host = null;

		private IWebElement waitForElement (IWebDriver driver, string query) {
			Thread.Sleep(1000);
			new WebDriverWait(driver, TimeSpan.FromSeconds(1000))
				.Until(d => d.FindElements(By.CssSelector(query)).Count != 0);
			Thread.Sleep(1000);
			return driver.FindElement(By.CssSelector(query));
		}

		private void logInGmail (IWebDriver driver, string email, string currentPassword) {
			Thread.Sleep(2000);
			driver.Navigate().GoToUrl("https://accounts.google.com/signin/v2/identifier");

			IWebElement emailInput = waitForElement(driver, "input[name=identifier]");
			emailInput.SendKeys(email);
			emailInput.SendKeys(OpenQA.Selenium.Keys.Return);

			IWebElement passwordInput = waitForElement(driver, "input[name=password]");
			passwordInput.SendKeys(currentPassword);
			passwordInput.SendKeys(OpenQA.Selenium.Keys.Return);
		}
		private void changePasswordGmail (IWebDriver driver, string currentPassword, string newPassword) {
			Thread.Sleep(2000);

			driver.Navigate().GoToUrl("https://myaccount.google.com/signinoptions/password");

			IWebElement passwordInput = waitForElement(driver, "input[name=password]");
			passwordInput.SendKeys(currentPassword);
			passwordInput.SendKeys(OpenQA.Selenium.Keys.Return);

			Thread.Sleep(2000);

			IWebElement newPasswordInput = waitForElement(driver, "input[name=password]");
			newPasswordInput.SendKeys(newPassword);

			Thread.Sleep(2000);

			IWebElement confirmNewPasswordInput = waitForElement(driver, "input[name=confirmation_password]");
			confirmNewPasswordInput.SendKeys(newPassword);
			confirmNewPasswordInput.SendKeys(OpenQA.Selenium.Keys.Return);
		}
		private void logInTwitter (IWebDriver driver, string email, string currentPassword) {
			Thread.Sleep(2000);
			driver.Navigate().GoToUrl("https://twitter.com/login");

			IWebElement emailInput = waitForElement(driver, "input.js-username-field.email-input.js-initial-focus");
			emailInput.SendKeys(email);

			Thread.Sleep(2000);

			IWebElement passwordInput = waitForElement(driver, "input.js-password-field");
			passwordInput.SendKeys(currentPassword);
			passwordInput.SendKeys(OpenQA.Selenium.Keys.Return);
		}
		private void changePasswordTwitter (IWebDriver driver, string currentPassword, string newPassword) {
			Thread.Sleep(2000);

			driver.Navigate().GoToUrl("https://twitter.com/settings/password");

			IWebElement passwordInput = waitForElement(driver, "input[name=current_password]");
			passwordInput.SendKeys(currentPassword);

			Thread.Sleep(2000);

			IWebElement newPasswordInput = waitForElement(driver, "input[name=user_password]");
			newPasswordInput.SendKeys(newPassword);

			Thread.Sleep(2000);

			IWebElement confirmNewPasswordInput = waitForElement(driver, "input[name=user_password_confirmation]");
			confirmNewPasswordInput.SendKeys(newPassword);
			confirmNewPasswordInput.SendKeys(OpenQA.Selenium.Keys.Return);
		}
		private void logInFacebook (IWebDriver driver, string email, string currentPassword) {
			Thread.Sleep(2000);
			driver.Navigate().GoToUrl("https://www.facebook.com/");

			IWebElement emailInput = waitForElement(driver, "#email");
			emailInput.SendKeys(email);

			IWebElement passwordInput = waitForElement(driver, "#pass");
			passwordInput.SendKeys(currentPassword);
			passwordInput.SendKeys(OpenQA.Selenium.Keys.Return);
		}

		private void changePasswordFacebook (IWebDriver driver, string currentPassword, string newPassword) {
			Thread.Sleep(2000);

			driver.Navigate().GoToUrl("https://www.facebook.com/settings?tab=security&section=password&view");

			IWebElement passwordInput = waitForElement(driver, "#password_old");
			passwordInput.SendKeys(currentPassword);


			IWebElement newPasswordInput = waitForElement(driver, "#password_new");
			newPasswordInput.SendKeys(newPassword);


			IWebElement confirmNewPasswordInput = waitForElement(driver, "#password_confirm");
			confirmNewPasswordInput.SendKeys(newPassword);
			confirmNewPasswordInput.SendKeys(OpenQA.Selenium.Keys.Return);
		}

		private void logInGithub (IWebDriver driver, string email, string currentPassword) {
			Thread.Sleep(2000);
			driver.Navigate().GoToUrl("https://github.com/login");

			IWebElement emailInput = waitForElement(driver, "#login_field");
			emailInput.SendKeys(email);

			IWebElement passwordInput = waitForElement(driver, "#password");
			passwordInput.SendKeys(currentPassword);
			passwordInput.SendKeys(OpenQA.Selenium.Keys.Return);
		}
		private void changePasswordGithub (IWebDriver driver, string currentPassword, string newPassword) {
			Thread.Sleep(2000);

			driver.Navigate().GoToUrl("https://github.com/settings/admin");

			IWebElement passwordInput = waitForElement(driver, "#user_old_password");
			passwordInput.SendKeys(currentPassword);


			IWebElement newPasswordInput = waitForElement(driver, "#user_new_password");
			newPasswordInput.SendKeys(newPassword);


			IWebElement confirmNewPasswordInput = waitForElement(driver, "#user_confirm_new_password");
			confirmNewPasswordInput.SendKeys(newPassword);
			confirmNewPasswordInput.SendKeys(OpenQA.Selenium.Keys.Return);
		}
		//May fail, maybe path does not exist?
		//Or Chrome out of date?
		//try...catch, and mbox or something
		public IWebDriver createDriver () {
			ChromeOptions options = new ChromeOptions();
			options.AddArguments("--disable-notifications");

			IWebDriver driver = new ChromeDriver(
				Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/chromedriver",
				options
			);
			return driver;
			/*{
				string email = "conuhax3@gmail.com";
				string currentPassword = "concordia-new2";
				string newPassword = "concordia-new1";

				logInGithub(driver, email, currentPassword);
				changePasswordGithub(driver, currentPassword, newPassword);
			}*/
		}
        public byte[] deriveNewPassword (byte[] currentPassword) {
            ProtectedString psCur = new ProtectedString(true, currentPassword);
            PwProfile pwp = PwProfile.DeriveFromPassword(psCur);

            ProtectedString psNew;
            PwGenerator.Generate(
                out psNew,
                pwp,
                null,
                _host.PwGeneratorPool
            );
            byte[] pbNew = psNew.ReadUtf8();

            if (currentPassword.SequenceEqual(pbNew)) {
                //Try again, this shouldn't happen too often
                return deriveNewPassword(currentPassword);
            }

            //Zero fill the currentPassword buffer
            MemUtil.ZeroByteArray(currentPassword);

            //You should zero fill the array after you are done
            //using it. Because the array might still be in memory for a while.
            //We don't want memory sniffers getting it so easily
            //KeePassLib.Utility.MemUtil.ZeroByteArray(pbNew);
            return pbNew;
        }
        public override bool Initialize(IPluginHost host)
        {
            _host = host;
            //Usage
            byte[] current = System.Text.Encoding.UTF8.GetBytes("kjsdhfr232&@#");
            byte[] next = deriveNewPassword(current);
            MessageBox.Show(System.Text.Encoding.UTF8.GetString(next));

            return true;
        }
    }
}
