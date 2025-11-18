using eAgenda.Testes.Interface.Compartilhado;
using OpenQA.Selenium;

namespace eAgenda.Testes.Interface.ModuloContato;

[TestClass]
[TestCategory("Testes de Interface de Contato")]
public class ContatoInterfaceTestes : TestFixture
{
    [TestMethod]
    public void Deve_CadastrarContato_Corretamente()
    {
        // Arranjo
        RegistrarEAutenticarUsuario();

        webDriver?.Navigate().GoToUrl(Path.Combine(enderecoBase, "contatos", "cadastrar"));

        // Ação
        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputNome]"))).SendKeys("Oscar Lima");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputTelefone]"))).SendKeys("(49) 98888-2222");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputEmail]"))).SendKeys("oscar25lima@hotmail.com");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputEmpresa]"))).SendKeys("TurboAuto");
        
        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputCargo]"))).SendKeys("Mecânico");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("button[data-se=btnConfirmar]"))).Click();

        // Asserção
        webDriverWait?
            .Until(d => d.Title.Contains("Visualização de Contatos"));

        webDriverWait?
            .Until(d => d.PageSource.Contains("Oscar Lima"));
    }

    [TestMethod]
    public void Deve_EditarContato_Corretamente()
    {
        // Arranjo
        RegistrarEAutenticarUsuario();

        webDriver?.Navigate().GoToUrl(Path.Combine(enderecoBase, "contatos", "cadastrar"));

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputNome]"))).SendKeys("Oscar Lima");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputTelefone]"))).SendKeys("(49) 98888-2222");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputEmail]"))).SendKeys("oscar25lima@hotmail.com");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputEmpresa]"))).SendKeys("TurboAuto");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputCargo]"))).SendKeys("Mecânico");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("button[data-se=btnConfirmar]"))).Click();

        // Ação
        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("a[title=Editar]")))
            .Click();

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputNome]"))).Clear();

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputNome]"))).SendKeys("João Alves");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("button[data-se=btnConfirmar]"))).Click();

        // Asserção
        webDriverWait?
            .Until(d => d.Title.Contains("Visualização de Contatos"));

        webDriverWait?
            .Until(d => d.PageSource.Contains("João Alves"));
    }

    [TestMethod]
    public void Deve_ExcluirContato_Corretamente()
    {
        // Arranjo
        RegistrarEAutenticarUsuario();

        webDriver?.Navigate().GoToUrl(Path.Combine(enderecoBase, "contatos", "cadastrar"));

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputNome]"))).SendKeys("Oscar Lima");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputTelefone]"))).SendKeys("(49) 98888-2222");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputEmail]"))).SendKeys("oscar25lima@hotmail.com");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputEmpresa]"))).SendKeys("TurboAuto");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputCargo]"))).SendKeys("Mecânico");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("button[data-se=btnConfirmar]"))).Click();

        // Ação
        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("a[title=Excluir]")))
            .Click();

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("button[data-se=btnConfirmar]"))).Click();

        // Asserção
        webDriverWait?
            .Until(d => d.Title.Contains("Visualização de Contatos"));

        webDriverWait?
            .Until(d => !d.PageSource.Contains("Oscar Lima"));
    }

    [TestMethod]
    public void Deve_DetalhesContato_Corretamente()
    {
        // Arranjo
        RegistrarEAutenticarUsuario();

        webDriver?.Navigate().GoToUrl(Path.Combine(enderecoBase, "contatos", "cadastrar"));

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputNome]"))).SendKeys("Oscar Lima");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputTelefone]"))).SendKeys("(49) 98888-2222");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputEmail]"))).SendKeys("oscar25lima@hotmail.com");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputEmpresa]"))).SendKeys("TurboAuto");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("input[data-se=inputCargo]"))).SendKeys("Mecânico");

        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("button[data-se=btnConfirmar]"))).Click();

        // Ação
        webDriverWait?
            .Until(d => d.FindElement(By.CssSelector("a[title=Detalhes]")))
            .Click();

        // Asserção
        webDriverWait?
            .Until(d => d.Title.Contains("Detalhes do Contato"));

        webDriverWait?
            .Until(d => d.PageSource.Contains("Oscar Lima"));
    }
}
