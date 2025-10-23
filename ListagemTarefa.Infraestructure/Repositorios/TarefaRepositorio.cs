using ListagemTarefa.Domain.Entidades;
using ListagemTarefa.Domain.Enums;
using ListagemTarefa.Domain.Modelos;
using ListagemTarefa.Domain.Repositorios;
using ListagemTarefa.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ListagemTarefa.Infraestructure.Repositorios;
public class TarefaRepositorio : ITarefaRepositorio
{
    private readonly ListagemTarefaContext _context;

    public TarefaRepositorio(ListagemTarefaContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(IEnumerable<Tarefa> tarefas)
    {
        if (_context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
        {
            _context.Tarefas.RemoveRange(_context.Tarefas);
        }
        else
        {
            await _context.Tarefas.ExecuteDeleteAsync();
        }

        _context.Tarefas.AddRange(tarefas);

        await _context.SaveChangesAsync();
    }

    public async Task AtualizarStatusAsync(int tarefaId)
    {
        await _context.Tarefas.Where(x => x.Id == tarefaId).ExecuteUpdateAsync(setter => setter.SetProperty(x => x.Completed, x => !x.Completed));
    }

    public async Task<Tarefa?> ObterPorIdAsync(int tarefaId)
    {
         var tarefa = await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == tarefaId);

         return tarefa;
    }

    public async Task<int> QuantidadeNaoCompletadasDoUsuarioAsync(int userId)
    {
        var tarefasNaoCompletadas = await _context.Tarefas.CountAsync(x => x.UserId == userId && !x.Completed);

        return tarefasNaoCompletadas;
    }

    public async Task<IEnumerable<Tarefa>> ObterTodosAsync(ParametrosBuscaTarefa parametrosBusca)
    {
        var query = _context.Tarefas.AsQueryable();

        if (parametrosBusca.UserId.HasValue)
        {
            query = query.Where(x => x.UserId == parametrosBusca.UserId.Value);
        }

        if (parametrosBusca.Id.HasValue)
        {
            query = query.Where(x => x.Id ==  parametrosBusca.Id.Value);
        }

        if (!string.IsNullOrWhiteSpace(parametrosBusca.Title))
        {
            query = query.Where(x => x.Title.ToLower().Contains(parametrosBusca.Title.ToLower()));
        }

        if (parametrosBusca.Completed.HasValue)
        {
            query = query.Where(x => x.Completed == parametrosBusca.Completed.Value);
        }

        Expression<Func<Tarefa, object>>? ordenacaoExpression = parametrosBusca.Sort switch
        {
            TarefaCamposOrdenacao.UserId => x => x.UserId,
            TarefaCamposOrdenacao.Id => x => x.Id,
            TarefaCamposOrdenacao.Title => x => x.Title,
            TarefaCamposOrdenacao.Completed => x => x.Completed,
            _ => null,
        };

        if (ordenacaoExpression == null)
        {
            ordenacaoExpression = x => x.Id;
        }

        query = parametrosBusca.Order == TiposOrdenacao.Asc
            ? query.OrderBy(ordenacaoExpression)
            : query.OrderByDescending(ordenacaoExpression);

        var tarefas = await query
            .Skip((parametrosBusca.Page - 1) * parametrosBusca.PageSize)
            .Take(parametrosBusca.PageSize)
            .ToListAsync();

        return tarefas;

    }




}
