using eAgenda.Dominio.ModuloCategoria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.Orm.ModuloCategoria;

public class MapeadorCategoriaEmOrm : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("TBCategoria");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Titulo)
               .HasMaxLength(100)
               .IsRequired();

        builder.HasMany(x => x.Despesas)
               .WithMany(c => c.Categorias)
               .UsingEntity(x => x.ToTable("TBCategoria_TBDespesa"));
    }
}