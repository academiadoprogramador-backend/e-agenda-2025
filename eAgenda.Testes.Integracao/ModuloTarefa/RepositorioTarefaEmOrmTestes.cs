using eAgenda.Dominio.ModuloTarefa;
using eAgenda.Testes.Integracao.Compartilhado;

namespace eAgenda.Testes.Integracao.ModuloTarefa;

[TestClass]
[TestCategory("Testes de Integração de Tarefa")]
public class RepositorioTarefaEmOrmTestes : TestFixture
{
    [TestMethod]
    public void Deve_CadastrarRegistro_ComSucesso()
    {
        // Arranjo
        Tarefa tarefa = new Tarefa("Lavar o cachorro", PrioridadeTarefa.Alta);

        // Ação
        repositorioTarefa?.CadastrarRegistro(tarefa);

        Tarefa? tarefaSelecionada = repositorioTarefa?.SelecionarRegistroPorId(tarefa.Id);

        // Asserção
        Assert.AreEqual(tarefa, tarefaSelecionada);
    }

    [TestMethod]
    public void Deve_RetornarNulo_Ao_SelecionarRegistroPorId_ComIdErrado()
    {
        // Arranjo
        Tarefa tarefa = new Tarefa("Lavar o cachorro", PrioridadeTarefa.Alta);


        repositorioTarefa?.CadastrarRegistro(tarefa);

        // Ação
        Tarefa? tarefaSelecionada = repositorioTarefa?.SelecionarRegistroPorId(Guid.NewGuid());

        // Asserção
        Assert.AreNotEqual(tarefa, tarefaSelecionada);
    }

    [TestMethod]
    public void Deve_EditarRegistro_ComSucesso()
    {
        // Arranjo
        Tarefa tarefaOriginal = new Tarefa("Lavar o cachorro", PrioridadeTarefa.Alta);

        repositorioTarefa?.CadastrarRegistro(tarefaOriginal);

        Tarefa tarefaEditada = new Tarefa("Lavar o carro", PrioridadeTarefa.Alta);

        // Ação
        bool? registroEditado = repositorioTarefa?.EditarRegistro(tarefaOriginal.Id, tarefaEditada);

        // Asserção
        Tarefa? tarefaSelecionada = repositorioTarefa?.SelecionarRegistroPorId(tarefaOriginal.Id);

        Assert.IsTrue(registroEditado);
        Assert.AreEqual(tarefaOriginal, tarefaSelecionada);
    }

    [TestMethod]
    public void Deve_ExcluirRegistro_ComSucesso()
    {
        // Arranjo
        Tarefa tarefa = new Tarefa("Lavar o cachorro", PrioridadeTarefa.Alta);

        repositorioTarefa?.CadastrarRegistro(tarefa);

        // Ação
        bool? registroExcluido = repositorioTarefa?.ExcluirRegistro(tarefa.Id);

        // Asserção
        Tarefa? tarefaSelecionada = repositorioTarefa?.SelecionarRegistroPorId(tarefa.Id);

        Assert.IsTrue(registroExcluido);
        Assert.IsNull(tarefaSelecionada);
    }
}
