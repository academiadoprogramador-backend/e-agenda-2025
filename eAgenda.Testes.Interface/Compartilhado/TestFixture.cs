using eAgenda.Infraestrutura.Orm;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace eAgenda.Testes.Interface.Compartilhado;

[TestClass]
public abstract class TestFixture
{
    protected static string enderecoBase = "https://localhost:9001";

    protected static AppDbContext? dbContext;
    protected static WebDriver? webDriver;
    protected static WebDriverWait? webDriverWait;

    [AssemblyInitialize]
    public static void InicializarTestFixture(TestContext testContext)
    {
        dbContext = AppDbContextFactory.CriarDbContext("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=eAgendaBackendTestDb;Integrated Security=True");

        webDriver = new ChromeDriver();
    }

    [AssemblyCleanup]
    public static void LimparTestFixure()
    {
        if (webDriver is null || dbContext is null) return;

        webDriver.Quit();
        webDriver.Dispose();

        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }


    [TestInitialize]
    public void InicializarTeste()
    {
        if (dbContext is null || webDriver is null) return;

        dbContext.Database.EnsureCreated();

        dbContext.Tarefas.RemoveRange(dbContext.Tarefas);
        dbContext.Tarefas.RemoveRange(dbContext.Tarefas);
        dbContext.Despesas.RemoveRange(dbContext.Despesas);
        dbContext.Categorias.RemoveRange(dbContext.Categorias);
        dbContext.Compromissos.RemoveRange(dbContext.Compromissos);
        dbContext.Contatos.RemoveRange(dbContext.Contatos);
        dbContext.UserClaims.RemoveRange(dbContext.UserClaims);
        dbContext.UserTokens.RemoveRange(dbContext.UserTokens);
        dbContext.UserLogins.RemoveRange(dbContext.UserLogins);
        dbContext.UserRoles.RemoveRange(dbContext.UserRoles);
        dbContext.Users.RemoveRange(dbContext.Users);

        dbContext.SaveChanges();

        webDriver.Manage().Cookies.DeleteAllCookies();

        webDriverWait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(5));
    }
}
