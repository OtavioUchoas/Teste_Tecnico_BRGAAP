using ListagemTarefa.Domain.Entidades;

namespace ListagemTarefa.Application.Interfaces;

public interface IJsonPlaceholderServico
{
    Task<IEnumerable<Tarefa>> BuscarDadosAsync();
}
