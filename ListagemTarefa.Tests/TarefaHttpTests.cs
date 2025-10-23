using ListagemTarefa.Application.Dtos;
using System.Net;
using System.Net.Http.Json;

namespace ListagemTarefa.Tests;

[TestCaseOrderer("Namespace.AlphabeticalOrderer", "AssemblyName")]
public class TarefaHttpTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public TarefaHttpTests(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task A_SincronizarDadosAsync_DeveRetornar200()
    {
        var response = await _httpClient.PostAsync($"/sync", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task B_AtualizarStatusAsync_QuandoUsuarioTem5Incompletas_DeveRetornar400()
    {
        var userId = 4;

        var response = await _httpClient.PutAsync($"/todos/{userId}", null);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task C_ObterPorIdAsync_QuandoInformadoId_DeveRetornarUmaTarefa()
    {
        var id = 1;

        var response = await _httpClient.GetAsync($"/todos/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<TarefaDto>();
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task D_ObterTarefasAsync_DeveRetornarPaginado()
    {
        var url = "/todos?page=1&pageSize=15";

        var response = await _httpClient.GetAsync(url);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<IEnumerable<TarefaDto>>();
        Assert.True(result!.Count() <= 15);
    }
}
