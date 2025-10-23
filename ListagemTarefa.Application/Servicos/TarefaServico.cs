using ListagemTarefa.Application.Interfaces;
using ListagemTarefa.Domain.Entidades;
using ListagemTarefa.Domain.Modelos;
using ListagemTarefa.Domain.Repositorios;

namespace ListagemTarefa.Application.Servicos;

public class TarefaServico : ITarefaServico
{
    private readonly IJsonPlaceholderServico _jsonPlaceholderServico;
    private readonly ITarefaRepositorio _tarefaRepositorio;

    public TarefaServico(IJsonPlaceholderServico jsonPlaceholderServico, ITarefaRepositorio tarefaRepositorio)
    {
        _jsonPlaceholderServico = jsonPlaceholderServico;
        _tarefaRepositorio = tarefaRepositorio;
    }

    public async Task SincronizarDadosAsync()
    {
        var tarefas = await _jsonPlaceholderServico.BuscarDadosAsync();

        await _tarefaRepositorio.AdicionarAsync(tarefas);
    }

    public async Task AtualizarStatusAsync(int tarefaId)
    {
        var tarefa = await ObterPorIdAsync(tarefaId)
            ?? throw new KeyNotFoundException($"Tarefa com id {tarefaId} não encontrada.");

        if (tarefa.Completed)
        {
            int quantidadeNaoCompletadasDoUsuario = await _tarefaRepositorio.QuantidadeNaoCompletadasDoUsuarioAsync(tarefa.UserId);

            if (quantidadeNaoCompletadasDoUsuario >= 5)
            {
                throw new Exception("O número de tarefas incompletas já ultrapassa o limite de 5 tarefas");
            }
        }

        await _tarefaRepositorio.AtualizarStatusAsync(tarefaId);
    }

    public async Task<Tarefa?> ObterPorIdAsync(int tarefaId)
    {
        var tarefa = await _tarefaRepositorio.ObterPorIdAsync(tarefaId)
            ?? throw new KeyNotFoundException($"Tarefa com id {tarefaId} não encontrada.");

        return tarefa;
    }

    public async Task<IEnumerable<Tarefa>> ObterTodosAsync(ParametrosBuscaTarefa parametrosBusca)
    {
        var tarefas = await _tarefaRepositorio.ObterTodosAsync(parametrosBusca);

        return tarefas;
    }
}
