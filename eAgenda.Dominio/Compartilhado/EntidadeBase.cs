using eAgenda.Dominio.ModuloAutenticacao;

namespace eAgenda.Dominio.Compartilhado;

public abstract class EntidadeBase<T>
{
    public Guid Id { get; set; }

    public Guid UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public abstract void AtualizarRegistro(T registroEditado);
}
