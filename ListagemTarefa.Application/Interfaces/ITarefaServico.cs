using ListagemTarefa.Domain.Entidades;
using ListagemTarefa.Domain.Modelos;

namespace ListagemTarefa.Application.Interfaces;

public interface ITarefaServico
{
    Task SincronizarDadosAsync();

    Task AtualizarStatusAsync(int tarefaId);

    Task<Tarefa?> ObterPorIdAsync(int tarefaId);

    Task<IEnumerable<Tarefa>> ObterTodosAsync(ParametrosBuscaTarefa parametrosBusca);
}
