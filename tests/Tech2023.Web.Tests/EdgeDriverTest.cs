using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Tech2023.Web.Tests;

[TestClass]
public class EdgeDriverTest
{
    // In order to run the below test(s), 
    // please follow the instructions from http://go.microsoft.com/fwlink/?LinkId=619687
    // to install Microsoft WebDriver.

#pragma warning disable CS8618
    private EdgeDriver _driver;
#pragma warning restore

    [TestInitialize]
    public void EdgeDriverInitialize()
    {
        // Initialize edge driver 
        var options = new EdgeOptions
        {
            PageLoadStrategy = PageLoadStrategy.Normal
        };
        _driver = new EdgeDriver(options);
    }

    [TestMethod]
    public void VerifyPageTitle()
    {
        // Replace with your own test logic
        _driver.Url = "https://www.bing.com";
        Assert.AreEqual("Bing", _driver.Title);
    }

    [TestCleanup]
    public void EdgeDriverCleanup()
    {
        _driver.Quit();
    }
}
