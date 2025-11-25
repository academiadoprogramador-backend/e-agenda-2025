using eAgenda.Testes.Interface.Compartilhado;
using OpenQA.Selenium;

namespace eAgenda.Testes.Interface.ModuloCategoria;

[TestClass]
[TestCategory("Testes de Interface de Categoria")]
public class CategoriaInterfaceTestes : TestFixture
{
    [TestMethod]
    public void Deve_CadastrarCategoria_Corretamente()
    {
        // Arranjo
        RegistrarEAutenticarUsuario();

        // Ação
        CadastrarCategoriaPadrao();

        // Asserção
        webDriverWait?.Until(d => d.Title.Contains("Visualização de Categorias"));
        webDriverWait?.Until(d => d.PageSource.Contains("Mercado"));
    }

    [TestMethod]
    public void Deve_EditarCategoria_Corretamente()
    {
        // Arranjo
        RegistrarEAutenticarUsuario();

        CadastrarCategoriaPadrao();

        EsperarPorElemento(By.CssSelector("a[data-se=btnEditar]"))
            .Click();

        // Ação
        EsperarPorElemento(By.CssSelector("input[data-se=inputTitulo]"))
            .SendKeys(" Editado");

        EsperarPorElemento(By.CssSelector("button[data-se=btnConfirmar]"))
            .Click();

        // Asserção
        webDriverWait?.Until(d => d.Title.Contains("Visualização de Categorias"));
        webDriverWait?.Until(d => d.PageSource.Contains("Mercado Editado"));
    }

    [TestMethod]
    public void Deve_ExcluirCategoria_Corretamente()
    {
        // Arranjo
        RegistrarEAutenticarUsuario();

        CadastrarCategoriaPadrao();

        EsperarPorElemento(By.CssSelector("a[data-se=btnExcluir]"))
            .Click();

        // Ação
        EsperarPorElemento(By.CssSelector("button[data-se=btnConfirmar]"))
            .Click();

        // Asserção
        webDriverWait?.Until(d => d.Title.Contains("Visualização de Categorias"));
        webDriverWait?.Until(d => !d.PageSource.Contains("Mercado"));
    }

    public static void CadastrarCategoriaPadrao()
    {
        NavegarPara("/categorias/cadastrar");

        EsperarPorElemento(By.CssSelector("input[data-se=inputTitulo]"))
            .SendKeys("Mercado");

        EsperarPorElemento(By.CssSelector("button[data-se=btnConfirmar]"))
            .Click();
    }
}
