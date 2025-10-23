using ListagemTarefa.Application.Interfaces;
using ListagemTarefa.Domain.Entidades;
using System.Net.Http.Json;
using System.Text.Json;

namespace ListagemTarefa.Application.Servicos;

public class JsonPlaceholderService : IJsonPlaceholderServico
{
    private readonly HttpClient _httpClient;

    public JsonPlaceholderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<Tarefa>> BuscarDadosAsync()
    {
        var response = await _httpClient.GetAsync("todos");

        response.EnsureSuccessStatusCode();

        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var tarefas = await response.Content.ReadFromJsonAsync<IEnumerable<Tarefa>>(jsonOptions);

        return tarefas ?? [];
    }
}
