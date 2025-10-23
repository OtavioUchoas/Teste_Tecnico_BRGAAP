namespace ListagemTarefa.Application.Dtos;

public class TarefaDto
{
    public int UserId { get; set; }
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }

}
