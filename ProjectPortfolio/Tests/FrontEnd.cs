//using System.Text.RegularExpressions;
//using Microsoft.Playwright;
//using Microsoft.Playwright.MSTest;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace ProjectPortfolio.Tests
//{
//    [TestClass]
//    public class FrontEnd : PageTest
//    {
//        private IBrowser _browser;

//        [TestInitialize]
//        public async Task InitializeAsync()
//        {
//            var playwright = await Playwright.Chromium..CreateAsync();
//            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
//            {
//                Headless = true // Defina como "false" para abrir o navegador visível
//            });
//        }

//        [TestMethod]
//        public async Task ExampleTest()
//        {
//            var page = await _browser.NewPageAsync();
//            await page.GotoAsync("https://example.com");
//            Assert.AreEqual("Example Domain", await page.TitleAsync());
//        }

//        [TestCleanup]
//        public async Task CleanupAsync()
//        {
//            await _browser.CloseAsync();
//        }
//    }
//}