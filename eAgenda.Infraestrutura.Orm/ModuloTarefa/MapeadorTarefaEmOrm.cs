using eAgenda.Dominio.ModuloTarefa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eAgenda.Infraestrutura.Orm.ModuloTarefa;

public class MapeadorTarefaEmOrm : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.ToTable("TBTarefa");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Titulo)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Prioridade)
               .IsRequired();

        builder.Property(x => x.DataCriacao)
               .IsRequired();

        builder.Property(x => x.Concluida)
               .IsRequired();

        builder.Property(x => x.DataConclusao)
               .IsRequired(false);

        builder.HasMany(x => x.Itens)
               .WithOne(i => i.Tarefa)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();
    }
}
public class MapeadorItemTarefaEmOrm : IEntityTypeConfiguration<ItemTarefa>
{
    public void Configure(EntityTypeBuilder<ItemTarefa> builder)
    {
        builder.ToTable("TBItemTarefa");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Titulo)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Concluido)
               .IsRequired();

        builder.HasOne(x => x.Tarefa)
               .WithMany(i => i.Itens)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();
    }
}