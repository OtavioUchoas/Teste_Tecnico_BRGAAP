using ListagemTarefa.Domain.Entidades;

namespace ListagemTarefa.Application.Dtos;

public record ResultadoPesquisaTarefaDto(IEnumerable<TarefaDto> Tarefas, int Total);
