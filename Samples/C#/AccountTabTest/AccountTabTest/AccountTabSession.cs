using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Threading;

namespace AccountTabTest
{
    public class  AccountTabSession
    {
        private const string buildNumber = "22.3.2.1059";
        private const string user = "Amir1";

        private const string mainPath = @"C:\Program Files (x86)\PCM\";
        private const string executable = @"\debug\PCMusic_Chain_Manager.exe";
        private const string cm_path = mainPath + buildNumber + executable;

        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        protected static WindowsDriver<WindowsElement> session;

        public static void Setup(TestContext context)
        {
            // Launch Chain Manager application if it is not yet launched
            if (session == null)
            {
                TearDown();

                // Create a new session to bring up Chain Manager application
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", cm_path);

                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                Assert.IsNotNull(session);
                Assert.IsNotNull(session.SessionId);

                // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
                session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            }

        }

        public static void TearDown()
        {
            // Close the application and delete the session
            if (session != null)
            {
                session.Quit();
                session = null;
            }
        }

        [TestInitialize]
        public virtual void TestInit()
        {
            try
            {
                session.FindElementByAccessibilityId("FrmLogin");

                //Clear username text box first
                session.FindElementByAccessibilityId("txtUserId").Clear();

                //Enter username
                session.FindElementByAccessibilityId("txtUserId").SendKeys(user);

                //Enter password
                session.FindElementByAccessibilityId("txtPassword").SendKeys("amir12345");

                //Login to chian manager
                session.FindElementByAccessibilityId("BtnLogin").Click();
                Thread.Sleep(TimeSpan.FromSeconds(3));

                //Login window closes, it needs to switch to the chain manager main window, as the currently open form
                var allWindowHandles = session.WindowHandles;
                session.SwitchTo().Window(allWindowHandles[0]);


                //Verify the main window with the currently logged in user opens
                WindowsElement chain_managerEntry = session.FindElementByXPath($"//Window[starts-with(@Name,\"RX Music Chain Manager\")]");
                Assert.IsNotNull(chain_managerEntry);
                Assert.IsTrue(chain_managerEntry.Text.Contains("DEV"));
                Assert.IsTrue(chain_managerEntry.Text.Contains(user));

                //Main window with three buttons are displayed
                WindowsElement musicSearch = session.FindElementByAccessibilityId("btnTitles");
                WindowsElement accounts = session.FindElementByAccessibilityId("btnAccounts");
                WindowsElement packages = session.FindElementByAccessibilityId("btnPackages");

                Assert.IsNotNull(musicSearch);
                Assert.IsNotNull(accounts);
                Assert.IsNotNull(packages);

                Assert.IsTrue(musicSearch.Displayed);
                Assert.IsTrue(accounts.Displayed);
                Assert.IsTrue(packages.Displayed);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Login page not launched");
                return;
            }
        }
    }
}
