using eAgenda.Infraestrutura.Orm;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Globalization;

namespace eAgenda.Testes.Interface.Compartilhado;

[TestClass]
public abstract class TestFixture
{
    protected static SeleniumServerFactory? serverFactory;
    protected static AppDbContext? dbContext;
    protected static WebDriver? webDriver;
    protected static WebDriverWait? webDriverWait;

    protected static string enderecoBase = null!;

    [AssemblyInitialize]
    public static void ConfigurarTestFixture(TestContext testContext)
    {
        var cultura = new CultureInfo("pt-BR");
        CultureInfo.DefaultThreadCurrentCulture = cultura;
        CultureInfo.DefaultThreadCurrentUICulture = cultura;

        serverFactory = new SeleniumServerFactory();
        dbContext = serverFactory.Servicos.GetRequiredService<AppDbContext>();
        enderecoBase = serverFactory.UrlKestrel;

        var chromeOptions = new ChromeOptions();

        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI")))
        {
            chromeOptions.AddArguments(
                "--headless",              // Sem interface gráfica
                "--no-sandbox",            // Necessário para Docker/CI
                "--disable-dev-shm-usage", // Evita problemas de memória
                "--disable-gpu",           // Desabilita GPU
                "--window-size=1920,1080"  // Resolução fixa
            );
        }

        webDriver = new ChromeDriver(chromeOptions);
        webDriverWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(15));
    }

    [AssemblyCleanup]
    public static void LimparAmbiente()
    {
        if (webDriver is not null)
        {
            webDriver.Quit();
            webDriver.Dispose();
            webDriver = null;
        }

        if (dbContext is not null)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Dispose();
            dbContext = null;
        }

        if (serverFactory is not null)
        {
            serverFactory?.Dispose();
            serverFactory = null;
        }
    }

    [TestInitialize]
    public void InicializarTeste()
    {
        if (dbContext is not null)
        {
            dbContext.Database.EnsureCreated();

            dbContext.Tarefas.RemoveRange(dbContext.Tarefas);
            dbContext.Despesas.RemoveRange(dbContext.Despesas);
            dbContext.Categorias.RemoveRange(dbContext.Categorias);
            dbContext.Compromissos.RemoveRange(dbContext.Compromissos);
            dbContext.Contatos.RemoveRange(dbContext.Contatos);

            // Tabelas do ASP.NET Identity
            dbContext.UserClaims.RemoveRange(dbContext.UserClaims);
            dbContext.UserTokens.RemoveRange(dbContext.UserTokens);
            dbContext.UserLogins.RemoveRange(dbContext.UserLogins);
            dbContext.UserRoles.RemoveRange(dbContext.UserRoles);
            dbContext.Users.RemoveRange(dbContext.Users);

            dbContext.SaveChanges();
        }

        if (webDriver is not null)
            webDriver.Manage().Cookies.DeleteAllCookies();
    }

    protected void RegistrarEAutenticarUsuario()
    {
        if (webDriver is null || webDriverWait is null)
            throw new InvalidOperationException("WebDriver não inicializado");

        NavegarPara("/autenticacao/registro");

        EsperarPorElemento(By.CssSelector("input[data-se=inputEmail]"))
            .SendKeys("teste@gmail.com");

        EsperarPorElemento(By.CssSelector("input[data-se=inputSenha]"))
            .SendKeys("SenhaSuperForteTeste@5912");

        EsperarPorElemento(By.CssSelector("input[data-se=inputConfirmarSenha]"))
            .SendKeys("SenhaSuperForteTeste@5912");

        EsperarPorElemento(By.CssSelector("button[data-se=btnConfirmar]"))
            .Click();

        webDriverWait.Until(d => d.PageSource.Contains("Página Inicial"));
        webDriverWait.Until(d => d.PageSource.Contains("teste@gmail.com"));
    }

    protected static void NavegarPara(string caminhoRelativo)
    {
        var uri = new Uri(new Uri(enderecoBase), caminhoRelativo);

        webDriver!.Navigate().GoToUrl(uri);
    }

    protected static IWebElement EsperarPorElemento(By localizador)
    {
        try
        {
            return webDriverWait!.Until(driver =>
            {
                var element = driver.FindElement(localizador);
                return element.Displayed ? element : null!;
            });
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine($"[DEBUG] Timeout esperando pelo elemento: {localizador}");
            Console.WriteLine($"[DEBUG] URL atual: {webDriver?.Url}");
            Console.WriteLine("[DEBUG] Título da página: " + webDriver?.Title);

            // Opcional: mostra um pedaço do HTML
            var pageSource = webDriver?.PageSource ?? string.Empty;
            Console.WriteLine("[DEBUG] Page source:");
            Console.WriteLine(pageSource);

            throw;
        }
    }
}
