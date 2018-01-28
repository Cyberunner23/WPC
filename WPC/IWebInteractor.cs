using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Collections.ObjectModel;

using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace WPC
{
    public abstract class IWebInteractor
    {

        protected IWebDriver _driver;

        public IWebInteractor()
        {
            ChromeOptions options = new ChromeOptions ();
            options.AddArguments ("--disable-notifications");

            _driver = new ChromeDriver (
                Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location) + "/chromedriver",
                options
            );
        }
        
        protected IWebElement waitForElement(IWebDriver driver, string query)
        {
            for (int cur = 0; cur < 7; ++cur) {
                Thread.Sleep (1000);
                ReadOnlyCollection<IWebElement> elements = driver.FindElements(query);
                if (elements.Count > 0) {
                    IWebElement result = elements[0];
                    return result;
                }
            }
            throw new Exception("We could not change your password, an element is missing; " + query);
        }

        public abstract void login(string email, string password);

        public abstract void changePassword(string currentPassword, string newPassword);
        
        
        
        
      

        

        

       

        

        //May fail, maybe path does not exist?
        //Or Chrome out of date?
        //try...catch, and mbox or something
        public static IWebDriver createDriver()
        {
            ChromeOptions options = new ChromeOptions ();
            options.AddArguments ("--disable-notifications");

            IWebDriver driver = new ChromeDriver (
                Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location) + "/chromedriver",
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
    }
}
