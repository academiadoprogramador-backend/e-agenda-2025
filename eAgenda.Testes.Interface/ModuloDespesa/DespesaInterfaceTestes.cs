using eAgenda.Testes.Interface.Compartilhado;
using eAgenda.Testes.Interface.ModuloCategoria;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace eAgenda.Testes.Interface.ModuloDespesa;

[TestClass]
[TestCategory("Testes de Interface de Despesa")]
public class DespesaInterfaceTestes : TestFixture
{
    [TestMethod]
    public void Deve_CadastrarDespesa_ComCategoria_Corretamente()
    {
        // Arranjo
        RegistrarEAutenticarUsuario();

        CategoriaInterfaceTestes.CadastrarCategoriaPadrao();

        webDriverWait!.Until(d => d.Title.Contains("Visualização de Categorias"));
        webDriverWait!.Until(d => d.PageSource.Contains("Mercado"));

        // Ação 
        CadastrarDespesaPadrao();

        // Asserção
        webDriverWait!.Until(d => d.Title.Contains("Visualização de Despesas"));
        webDriverWait!.Until(d => d.PageSource.Contains("Compras do Mês"));
    }

    [TestMethod]
    public void Deve_EditarDespesa_Corretamente()
    {
        // Arranjo
        RegistrarEAutenticarUsuario();
        
        CategoriaInterfaceTestes.CadastrarCategoriaPadrao();

        webDriverWait!.Until(d => d.Title.Contains("Visualização de Categorias"));
        webDriverWait!.Until(d => d.PageSource.Contains("Mercado"));

        CadastrarDespesaPadrao();

        EsperarPorElemento(By.CssSelector("a[data-se=btnEditar]")).Click();

        // Ação
        EsperarPorElemento(By.CssSelector("input[data-se=inputDescricao]"))
            .SendKeys(" Editada");

        EsperarPorElemento(By.CssSelector("button[data-se=btnConfirmar]")).Click();

        // Asserção
        webDriverWait!.Until(d => d.Title.Contains("Visualização de Despesas"));
        webDriverWait!.Until(d => d.PageSource.Contains("Compras do Mês Editada"));
    }

    [TestMethod]
    public void Deve_ExcluirDespesa_Corretamente()
    {
        // Arranjo
        RegistrarEAutenticarUsuario();

        CategoriaInterfaceTestes.CadastrarCategoriaPadrao();

        webDriverWait!.Until(d => d.Title.Contains("Visualização de Categorias"));
        webDriverWait!.Until(d => d.PageSource.Contains("Mercado"));

        CadastrarDespesaPadrao();

        EsperarPorElemento(By.CssSelector("a[data-se=btnExcluir]")).Click();

        // Ação
        EsperarPorElemento(By.CssSelector("button[data-se=btnConfirmar]")).Click();

        // Asserção
        webDriverWait!.Until(d => d.Title.Contains("Visualização de Despesas"));
        webDriverWait!.Until(d => !d.PageSource.Contains("Compras do Mês"));
    }

    public static void CadastrarDespesaPadrao()
    {
        NavegarPara("/despesas/cadastrar");

        EsperarPorElemento(By.CssSelector("input[data-se=inputDescricao]"))
            .SendKeys("Compras do Mês");

        var inputValor = EsperarPorElemento(By.CssSelector("input[data-se=inputValor]"));
        inputValor.Clear();
        inputValor.SendKeys("358,00");

        EsperarPorElemento(By.CssSelector("input[data-se=inputDataOcorrencia]"))
            .SendKeys("20/11/2025");

        var selectFormaPagamento = new SelectElement(
            EsperarPorElemento(By.CssSelector("select[data-se=inputFormaPagamento]"))
        );

        selectFormaPagamento.SelectByText("Crédito");

        webDriverWait?
            .Until(d => d.FindElements(By.CssSelector("input[data-se='inputCategoriasSelecionadas']")))
            .First()
            .Click();

        EsperarPorElemento(By.CssSelector("button[data-se=btnConfirmar]")).Click();
    }
}
