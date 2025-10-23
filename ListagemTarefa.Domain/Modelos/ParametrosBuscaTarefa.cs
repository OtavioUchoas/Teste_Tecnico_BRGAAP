using ListagemTarefa.Domain.Enums;

namespace ListagemTarefa.Domain.Modelos;

public class ParametrosBuscaTarefa
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public TarefaCamposOrdenacao Sort { get; set; } = TarefaCamposOrdenacao.Id;
    public TiposOrdenacao Order { get; set; } = TiposOrdenacao.Asc;

    public int? UserId { get; set; }
    public int? Id { get; set; }
    public string? Title { get; set; }
    public bool? Completed { get; set; }
}
