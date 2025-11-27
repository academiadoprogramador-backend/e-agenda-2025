using eAgenda.Infraestrutura.Orm;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace eAgenda.Testes.Interface.Compartilhado;

[TestClass]
public abstract class TestFixture
{
    public TestContext TestContext { get; set; } = null!;

    protected static SeleniumWebApplicationFactory serverFactory;
    protected static WebDriver? webDriver;
    protected static WebDriverWait? webDriverWait;
    protected static AppDbContext? dbContext;
    protected static string enderecoBase = null!;

    [AssemblyInitialize]
    public static void ConfigurarTestFixture(TestContext testContext)
    {
        serverFactory = new SeleniumWebApplicationFactory();

        dbContext = serverFactory.Servicos.GetRequiredService<AppDbContext>();

        enderecoBase = serverFactory.UrlKestrel;

        ChromeOptions chromeOptions = new ChromeOptions();

        // Se estiver no GitHub Actions (CI não está vazia)
        if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("CI")))
        {
            chromeOptions.AddArguments(
                "--headless",              // Sem interface gráfica
                "--no-sandbox",            // Necessário para Docker/CI
                "--disable-dev-shm-usage", // Evita problemas de memória
                "--disable-gpu",           // Desabilita GPU
                "--window-size=1920,1080", // Resolução fixa
                "--lang=pt-BR"             // Configura cultura do navegador fixa
            );
        }
        else
        {
            chromeOptions.AddArgument("--start-fullscreen");
        }

        webDriver = new ChromeDriver(chromeOptions);
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
        if (dbContext is null || webDriver is null) return;

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

        webDriver.Manage().Cookies.DeleteAllCookies();

        webDriverWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
    }

    [TestCleanup]
    public void EncerrarTeste()
    {
        if (TestContext.CurrentTestOutcome is not UnitTestOutcome.Failed)
            return;

        if (webDriver is null)
            return;

        try
        {
            Console.WriteLine("========== [DEBUG] ==========");

            Console.WriteLine($"Teste: {TestContext.TestName}");
            Console.WriteLine($"Resultado: {TestContext.CurrentTestOutcome}");
            Console.WriteLine($"Url da página atual: {webDriver.Url}");
            Console.WriteLine($"Título da página atual: {webDriver.Title}");

            Console.WriteLine("---- PageSource ----");
            Console.WriteLine(webDriver.PageSource);

            Console.WriteLine("========== [FIM DO DEBUG] ==========");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DEBUG] Erro ao coletar evidências: {ex}");
        }
    }

    protected void RegistrarEAutenticarUsuario()
    {
        webDriver?.Navigate().GoToUrl(Path.Combine(enderecoBase, "autenticacao", "registro"));

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputEmail]")))
            .SendKeys("teste@gmail.com");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputSenha]")))
            .SendKeys("SenhaSuperForteTeste@5912");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputConfirmarSenha]")))
            .SendKeys("SenhaSuperForteTeste@5912");

        webDriverWait?
           .Until(d => d.FindElement(By.CssSelector("button[data-se=btnConfirmar]")))
           .Click();

        webDriverWait?
            .Until(d => d.PageSource.Contains("Página Inicial"));

        webDriverWait?
            .Until(d => d.PageSource.Contains("teste@gmail.com"));
    }

    protected static void NavegarPara(string caminhoRelativo)
    {
        var enderecoBaseUri = new Uri(enderecoBase);

        var uri = new Uri(enderecoBaseUri, caminhoRelativo);

        webDriver?.Navigate().GoToUrl(uri);
    }

    protected static IWebElement EsperarPorElemento(By localizador)
    {
        return webDriverWait!.Until(driver =>
        {
            var elemento = driver.FindElement(localizador);

            return elemento.Displayed ? elemento : null;
        });
    }
}
