using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

using OpenQA.Selenium;

namespace WPC
{
	public class Command {
		public string goToUrl { get; set; }
		public string waitForElement { get; set; }
		public int sleep { get; set; }
		public string sendKeys { get; set; }
		public bool sendReturnKey { get; set; }
	}
	public class Site {
		public string name { get; set; }
		public List<Command> logIn { get; set; }
		public List<Command> changePassword { get; set; }
	}

	public class CustomInteraction : IWebInteractor
	{
		private Site site;
		public CustomInteraction (Site site)
		{
			this.site = site;
		}
		private void interpret (List<Command> commands, string email, string currentPassword, string newPassword) {
			IWebElement element = null;

			foreach (Command c in commands) {
				if (c.goToUrl != null && c.goToUrl != "") {
					_driver.Navigate().GoToUrl(c.goToUrl);
				} else if (c.waitForElement != null && c.waitForElement != "") {
					element = waitForElement(_driver, c.waitForElement);
				} else if (c.sleep > 0) {
					Thread.Sleep(c.sleep);
				} else if (c.sendKeys != null && c.sendKeys != "") {
					if (element != null) {
						if (c.sendKeys == "<email>") {
							element.SendKeys(email);
						} else if (c.sendKeys == "<currentPassword>") {
							element.SendKeys(currentPassword);
						} else if (c.sendKeys == "<newPassword>") {
							element.SendKeys(newPassword);
						} else {
							element.SendKeys(c.sendKeys);
						}
					}
				} else if (c.sendReturnKey == true) {
					if (element != null) {
						element.SendKeys(OpenQA.Selenium.Keys.Return);
					}
				}
			}
		}
		public override void login(string email, string password) {
			interpret(site.logIn, email, password, "");
		}
		public override void changePassword(string currentPassword, string newPassword) {
			interpret(site.changePassword, "", currentPassword, newPassword);
		}
	}
}

