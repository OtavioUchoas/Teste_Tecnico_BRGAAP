using ListagemTarefa.Application.Dtos;
using ListagemTarefa.Application.Interfaces;
using ListagemTarefa.Domain.Entidades;
using ListagemTarefa.Domain.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace ListagemTarefa.API.Controllers
{
  public class TarefaController : ControllerBase
  {
    private readonly ITarefaServico _tarefaServico;

    public TarefaController(ITarefaServico tarefaServico)
    {
      _tarefaServico = tarefaServico;
    }

    [HttpPost]
    [Route("sync")]
    public async Task<IActionResult> SincronizarDadosAsync()
    {
      try
      {
        await _tarefaServico.SincronizarDadosAsync();

        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpPut]
    [Route("todos/{id}")]
    public async Task<IActionResult> AtualizarStatusAsync(int id)
    {
      try
      {
        await _tarefaServico.AtualizarStatusAsync(id);

        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet]
    [Route("todos/{id}")]
    public async Task<ActionResult<TarefaDto>> ObterPorIdAsync(int id)
    {
      try
      {
        var tarefa = await _tarefaServico.ObterPorIdAsync(id);

        if (tarefa != null)
        {
          var tarefaDto = new TarefaDto
          {
            UserId = tarefa.UserId,
            Id = tarefa.Id,
            Title = tarefa.Title,
            Completed = tarefa.Completed,
          };

          return Ok(tarefaDto);
        }
        else
        {
          return Ok(null);
        }
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpGet]
    [Route("todos")]
    public async Task<ActionResult<ResultadoPesquisaTarefaDto>> ObterTodosAsync(
      [FromQuery] ParametrosBuscaTarefa parametrosBusca)
    {
      try
      {
        var tarefas = await _tarefaServico.ObterTodosAsync(parametrosBusca);
        var total = await _tarefaServico.ObterQuantidadeTodosASync(parametrosBusca);

        if (!tarefas.Any()) return Ok(new ResultadoPesquisaTarefaDto([], 0));
        
        var tarefasDtos = tarefas.Select(tarefa => new TarefaDto
        {
          UserId = tarefa.UserId,
          Id = tarefa.Id,
          Title = tarefa.Title,
          Completed = tarefa.Completed,
        }).ToList();

        return Ok(new ResultadoPesquisaTarefaDto(tarefasDtos, total));

      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }
  }
}
