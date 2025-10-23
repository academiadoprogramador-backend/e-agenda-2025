using eAgenda.Dominio.ModuloDespesa;
using eAgenda.Dominio.ModuloCategoria;
using eAgenda.Testes.Integracao.Compartilhado;
using FizzWare.NBuilder;

namespace eAgenda.Testes.Integracao.ModuloDespesa;


[TestClass]
[TestCategory("Testes de Integração de Despesa")]
public class RepositorioDespesaEmOrmTestes : TestFixture
{
    [TestMethod]
    public void Deve_CadastrarRegistro_ComSucesso()
    {
        // Arrange - Arranjo
        Despesa despesa = new Despesa(
            "Compras",
            190.33m,
            DateTime.Now,
            FormaPagamento.Credito
        );

        // Act - Ação
        repositorioDespesa?.CadastrarRegistro(despesa);

        // Assert - Asserção
        Despesa? despesaSelecionada = repositorioDespesa?.SelecionarRegistroPorId(despesa.Id);

        Assert.AreEqual(despesa, despesaSelecionada);
    }

    [TestMethod]
    public void Deve_CadastrarRegistro_ComCategoria_ComSucesso()
    {
        // Arrange - Arranjo
        Categoria categoria = Builder<Categoria>
            .CreateNew()
            .Build();

        Despesa despesa = new Despesa(
            "Compras",
            190.33m,
            DateTime.Now,
            FormaPagamento.Credito
        );

        despesa.RegistarCategoria(categoria);

        // Act - Ação
        repositorioDespesa?.CadastrarRegistro(despesa);

        // Assert - Asserção
        Despesa? despesaSelecionado = repositorioDespesa?.SelecionarRegistroPorId(despesa.Id);

        Assert.AreEqual(despesa, despesaSelecionado);
    }

    [TestMethod]
    public void Deve_EditarRegistro_ComSucesso()
    {
        // Arrange - Arranjo
        Categoria categoria = Builder<Categoria>
            .CreateNew()
            .Build();

        Despesa despesa = new Despesa(
            "Compras",
            190.33m,
            DateTime.Now,
            FormaPagamento.Credito
        );

        repositorioDespesa?.CadastrarRegistro(despesa);

        Despesa despesaEditada = new Despesa(
            "Steam",
            40.30m,
            DateTime.Now,
            FormaPagamento.Pix
        );

        // Act - Ação
        bool? registroEditado = repositorioDespesa?.EditarRegistro(despesa.Id, despesaEditada);

        // Assert - Asserção
        Despesa? despesaSelecionado = repositorioDespesa?.SelecionarRegistroPorId(despesa.Id);

        Assert.IsTrue(registroEditado);
        Assert.AreEqual(despesa, despesaSelecionado);
    }

    [TestMethod]
    public void Deve_ExcluirRegistro_ComSucesso()
    {
        // Arrange - Arranjo
        Categoria categoria = Builder<Categoria>
            .CreateNew()
            .Build();

        Despesa despesa = new Despesa(
            "Compras",
            190.33m,
            DateTime.Now,
            FormaPagamento.Credito
        );

        repositorioDespesa?.CadastrarRegistro(despesa);

        // Act - Ação
        bool? registroExcluido = repositorioDespesa?.ExcluirRegistro(despesa.Id);

        // Assert - Asserção
        Despesa? despesaSelecionado = repositorioDespesa?.SelecionarRegistroPorId(despesa.Id);

        Assert.IsTrue(registroExcluido);
        Assert.IsNull(despesaSelecionado);
    }
}
