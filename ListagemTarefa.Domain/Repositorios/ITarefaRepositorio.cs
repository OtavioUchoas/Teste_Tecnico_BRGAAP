using ListagemTarefa.Domain.Entidades;
using ListagemTarefa.Domain.Modelos;

namespace ListagemTarefa.Domain.Repositorios;

public interface ITarefaRepositorio
{
    Task AdicionarAsync(IEnumerable<Tarefa> tarefas);

    Task AtualizarStatusAsync(int tarefaId);

    Task<Tarefa?> ObterPorIdAsync(int tarefaId);

    Task<IEnumerable<Tarefa>> ObterTodosAsync(ParametrosBuscaTarefa parametrosBusca);

    Task<int> ObterQuantidadeTodosASync(ParametrosBuscaTarefa parametrosBusca);

    Task<int> QuantidadeNaoCompletadasDoUsuarioAsync(int userId);
}
