using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using CadastroClienteAPP.Application.DTOs;
using CadastroClienteAPP.Application.Interfaces;
using CadastroClienteAPP.Domain.Entities;

namespace CadastroClientesAPP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]       
        [ProducesResponseType(typeof(IEnumerable<ClienteDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ListarTodos()        
        {
            try
            {
                var clientes = await _clienteService.ObterTodosAsync();
                return Ok(clientes);
            }
            catch (ApplicationException e)
            {
                return BadRequest(new { mensagem = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensagem = $"Falha ao listar clientes: {e.Message}" });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Criar([FromBody] ClienteDto dto)
        {
            try
            {
                var id = await _clienteService.CriarAsync(dto);
                return CreatedAtAction(nameof(GetPorId), new { id }, id);
            }
            catch (ApplicationException e)
            {
                return BadRequest(new { mensagem = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensagem = $"Falha ao criar cliente: {e.Message}" });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClienteDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPorId(Guid id)
        {
            try
            {
                var cliente = await _clienteService.ObterPorIdAsync(id);
                if (cliente == null) return NotFound();
                return Ok(cliente);
            }
            catch (ApplicationException e)
            {
                return BadRequest(new { mensagem = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensagem = $"Falha ao atualizar cliente: {e.Message}" });
            }
        }       

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] ClienteDto dto)
        {
            try
            {
                await _clienteService.AtualizarClienteAsync(id, dto);
                return NoContent();
            }
            catch (ApplicationException e)
            {
                return BadRequest(new { mensagem = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensagem = $"Falha ao atualizar cliente: {e.Message}" });
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                await _clienteService.RemoverClienteAsync(id);
                return NoContent();
            }
            catch (ApplicationException e)
            {
                return BadRequest(new { mensagem = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { mensagem = $"Falha ao atualizar cliente: {e.Message}" });
            }
        }

    }

}
