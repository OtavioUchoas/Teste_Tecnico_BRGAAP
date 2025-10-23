using ListagemTarefa.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ListagemTarefa.Infraestructure.Data;
public class ListagemTarefaContext : DbContext
{
    public ListagemTarefaContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tarefa>(options =>
        {
            options.HasKey("Id");

            options.Property(t => t.Id)
                .ValueGeneratedNever();
        });
    }

    public DbSet<Tarefa> Tarefas { get; set; }
}
