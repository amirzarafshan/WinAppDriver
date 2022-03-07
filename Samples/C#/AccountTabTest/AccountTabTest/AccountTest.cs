using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;

namespace AccountTabTest
{
    [TestClass]
    public class AccountTest: AccountTabSession
    { 

        [TestMethod]
        public void SearchOptionsFilterBoxTest()
        {
            // Open search Options combo/filter box and move to the first item in the box
            WindowsElement SearchOptionsCombobox = session.FindElementByAccessibilityId("SearchOptions");
            SearchOptionsCombobox.Click();
            SearchOptionsCombobox.SendKeys(Keys.Up + Keys.Up);

            // Verify first item is 'Account' in the filter box
            WindowsElement accountItem = session.FindElementByName("Account");
            Assert.IsNotNull(accountItem);
            Assert.AreEqual(accountItem.Text, "Account");

            // Verify first item 'Account' in the filter box is selectable
            accountItem.Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var accountItemSelected = SearchOptionsCombobox.Text;
            Assert.IsTrue(accountItemSelected.Contains("Account"));
            Assert.AreEqual(accountItemSelected, "{\"name\":\"Account\",\"id\":1}");


            // Verify second item is 'Location' in the filter box
            SearchOptionsCombobox.Click();
            SearchOptionsCombobox.SendKeys(Keys.Down);
            WindowsElement locationItem = session.FindElementByName("Location");
            Assert.IsNotNull(locationItem);
            Assert.AreEqual(locationItem.Text, "Location");

            // Verify second item 'Location' in the filter box is selectable
            locationItem.Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var locationItemSelected = SearchOptionsCombobox.Text;
            Assert.IsTrue(locationItemSelected.Contains("Location"));
            Assert.AreEqual(locationItemSelected, "{\"name\":\"Location\",\"id\":2}");


            // Verify third item is 'Station' in the filter box
            SearchOptionsCombobox.Click();
            SearchOptionsCombobox.SendKeys(Keys.Down);
            WindowsElement stationItem = session.FindElementByName("Station");
            Assert.IsNotNull(stationItem);
            Assert.AreEqual(stationItem.Text, "Station");

            // Verify third item 'Location' in the filter box is selectable
            stationItem.Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var stationItemSelected = SearchOptionsCombobox.Text;
            Assert.IsTrue(stationItemSelected.Contains("Station"));
            Assert.AreEqual(stationItemSelected, "{\"name\":\"Station\",\"id\":3}");

            // Verify fourth item is 'CRM' in the filter box
            SearchOptionsCombobox.Click();
            SearchOptionsCombobox.SendKeys(Keys.Down);
            WindowsElement crmItem = session.FindElementByName("CRM");
            Assert.IsNotNull(crmItem);
            Assert.AreEqual(crmItem.Text, "CRM");

            // Verify fourth item 'CRM' in the filter box is selectable
            crmItem.Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var crmItemSelected = SearchOptionsCombobox.Text;
            Assert.IsTrue(crmItemSelected.Contains("CRM"));
            Assert.AreEqual(crmItemSelected, "{\"name\":\"CRM\",\"id\":4}");

            // Re-select item 'station' in filterbox as the defult item.
            SearchOptionsCombobox.Click();
            SearchOptionsCombobox.SendKeys(Keys.Up);
            stationItem.Click();
            Assert.IsNotNull(stationItem);
            Assert.IsTrue(stationItemSelected.Contains("Station"));

            SearchOptionsCombobox = null;
        }

        [TestMethod]
        public void FileMenuTest()
        {

            WindowsElement file = session.FindElementByName("File");
            file.Click();

            WindowsElement menu_new = session.FindElementByName("New...");
            menu_new.Click();

            WindowsElement accountMenuItem = session.FindElementByAccessibilityId("AccountMenuItem");
            Assert.IsTrue(accountMenuItem.Enabled);

            WindowsElement stationMenuItem = session.FindElementByAccessibilityId("StationMenuItem");
            Assert.IsFalse(stationMenuItem.Enabled);

            WindowsElement chainMenuItem = session.FindElementByAccessibilityId("ChainMenuItem");
            Assert.IsFalse(chainMenuItem.Enabled);

            WindowsElement subchainMenuItem = session.FindElementByAccessibilityId("SubChainMenuItem");
            Assert.IsFalse(subchainMenuItem.Enabled);

            WindowsElement userMenuItem = session.FindElementByAccessibilityId("UserMenuItem");
            Assert.IsTrue(userMenuItem.Enabled);

            file.Click();

        }

        [TestMethod]
        public void CreateAccountCancelButtonTest()
        {
            WindowsElement file = session.FindElementByName("File");
            file.Click();


            WindowsElement menu_new = session.FindElementByName("New...");
            menu_new.Click();

            // create new account
            WindowsElement accountMenuItem = session.FindElementByAccessibilityId("AccountMenuItem");
            accountMenuItem.Click();
            WindowsElement newAccountForm = session.FindElementByName("New Account");

            //Test 'Cancel' button
            WindowsElement cancelButton = session.FindElementByName("Cancel");
            cancelButton.Click();

            var allWindowHandles = session.WindowHandles;
            Assert.IsTrue(session.CurrentWindowHandle == allWindowHandles[0]);
            Assert.IsFalse(newAccountForm.Displayed);

        }

        [TestMethod]
        public void CreateAccountSaveButtonTest()
        {
            WindowsElement file = session.FindElementByName("File");
            file.Click();

            WindowsElement menu_new = session.FindElementByName("New...");
            menu_new.Click();

            // create new account
            WindowsElement accountMenuItem = session.FindElementByAccessibilityId("AccountMenuItem");
            accountMenuItem.Click();
            WindowsElement newAccountForm = session.FindElementByName("New Account");

            // Test 'Save' button
            WindowsElement saveButton = session.FindElementByName("Save");
            saveButton.Click();

            // Verify 'Name cannot be blank' warning message appears on acreen
            var allWindowHandles = session.WindowHandles;
            session.SwitchTo().Window(allWindowHandles[0]);
            WindowsElement warningMessage = session.FindElementByXPath($"//Text[starts-with(@Name,\"Name cannot be blank\")]");
            Assert.IsNotNull(warningMessage);
            Assert.IsTrue(warningMessage.Displayed);

            // Press OK button on warning message
            WindowsElement confirmMessage = session.FindElementByName("OK");
            confirmMessage.Click();

            // Switch back to the 'new account' window after confirming warning message
            session.SwitchTo().Window(allWindowHandles[1]);

            // Press Cancel on new account window
            WindowsElement cancelButton = session.FindElementByName("Cancel");
            cancelButton.Click();
            
        }


        [TestMethod]
        public void CreateAccountTest()
        {
            WindowsElement file = session.FindElementByName("File");
            file.Click();

            WindowsElement menu_new = session.FindElementByName("New...");
            menu_new.Click();

            // Create new account
            WindowsElement accountMenuItem = session.FindElementByAccessibilityId("AccountMenuItem");
            accountMenuItem.Click();

            // Enter account name
            WindowsElement accountName = session.FindElementByName("New Account");
            accountName.SendKeys("AmirNewAccount");

            // Enter email 
            accountName.SendKeys(Keys.Tab + "test@test.ca");

            // Press save 
            WindowsElement saveButton = session.FindElementByName("Save");
            saveButton.Click();

            var allWindowHandles = session.WindowHandles;

            // Verify account created:
            // 1.no internal error generated
            Assert.AreEqual(allWindowHandles.Count, 2);

            // 2.check whether the account is created in search result
           

        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
           WindowsElement closeAccountWindow = session.FindElementByXPath($"//Button[@Name=\"Close\"]");
           closeAccountWindow.Click();

            // Switch to the main window and close the session
           var allWindowHandles = session.WindowHandles;
           session.SwitchTo().Window(allWindowHandles[0]);

           TearDown();
        }

        [TestInitialize]
        public override void TestInit()
        {
            // Invoke base class test initialization to ensure that the app is in the main page
            base.TestInit();

            try
            {
                if (session.FindElementByXPath($"//Window[starts-with(@Name,\"RX Music Chain Manager\")]").Displayed)
                { 

                    //Go to WPF Accounts Window
                    session.FindElementByName("File").Click();
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    session.FindElementByName("WPF Accounts Window").Click();

                    Thread.Sleep(TimeSpan.FromSeconds(5));

                    var allWindowHandles = session.WindowHandles;
                    session.SwitchTo().Window(allWindowHandles[0]);

                    //Verify CM correctly sitting on 'Stations & Schedules window':
                    //1. CM display "Stations & Schedules window".
                    WindowsElement accountWindow = session.FindElementByXPath($"//Window[starts-with(@Name,\"Stations & Schedules\")]");
                    Assert.IsNotNull(accountWindow);

                    //2. Tab 'Accounts' is focused/selected by default
                    WindowsElement accountTab = session.FindElementByName("Accounts");
                    Assert.IsNotNull(accountTab);

                    //3. From filter combobox, item 'Station' is selected by default.
                    WindowsElement accountSearchCombobox = session.FindElementByAccessibilityId("SearchOptions");
                    Assert.IsNotNull(accountSearchCombobox);

                    var comboboxItems = accountSearchCombobox.Text;
                    Assert.IsTrue(comboboxItems.Contains("Station"));
                    Assert.AreEqual(comboboxItems, "{\"name\":\"Station\",\"id\":3}");
                    accountSearchCombobox = null;
                }
            }
            catch(InvalidOperationException)
            {
                return;
            }
        }
    }
}
