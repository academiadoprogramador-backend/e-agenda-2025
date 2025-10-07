using eAgenda.Dominio.ModuloCategoria;
using Microsoft.EntityFrameworkCore;

namespace eAgenda.Infraestrutura.Orm.ModuloCategoria;

public class RepositorioCategoriaEmOrm : IRepositorioCategoria
{
    private readonly AppDbContext context;
    private readonly DbSet<Categoria> registros;

    public RepositorioCategoriaEmOrm(AppDbContext context)
    {
        this.context = context;
        registros = context.Categorias;
    }

    public void CadastrarRegistro(Categoria novoRegistro)
    {
        registros.Add(novoRegistro);

        context.SaveChanges();
    }

    public bool EditarRegistro(Guid idRegistro, Categoria registroEditado)
    {
        var registroSelecionado = SelecionarRegistroPorId(idRegistro);

        if (registroSelecionado is null)
            return false;

        registroSelecionado.AtualizarRegistro(registroEditado);

        context.SaveChanges();

        return true;
    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        var registroSelecionado = SelecionarRegistroPorId(idRegistro);

        if (registroSelecionado is null)
            return false;

        registros.Remove(registroSelecionado);

        context.SaveChanges();

        return true;
    }


    public Categoria? SelecionarRegistroPorId(Guid idRegistro)
    {
        return registros.Include(x => x.Despesas).FirstOrDefault(x => x.Id == idRegistro);
    }

    public List<Categoria> SelecionarRegistros()
    {
        return registros.ToList();
    }
}
