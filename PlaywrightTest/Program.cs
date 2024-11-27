using Microsoft.Playwright;

namespace PlaywrightTest
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            // Cria uma instância do Playwright
            using var playwright = await Playwright.CreateAsync();

            // Inicia um navegador (por exemplo, Chromium)
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });

            // Cria uma nova página
            var page = await browser.NewPageAsync();

            // Navega para um site
            await page.GotoAsync("https://localhost:5001/" /);

            // Captura e exibe o título da página
            var title = await page.TitleAsync();
            Console.WriteLine($"Título da página: {title}");

            // Fecha o navegador
            await browser.CloseAsync();
        }
    }
}