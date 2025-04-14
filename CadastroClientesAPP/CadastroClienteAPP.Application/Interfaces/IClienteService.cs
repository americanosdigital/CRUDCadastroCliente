using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CadastroClienteAPP.Application.DTOs;

namespace CadastroClienteAPP.Application.Interfaces
{
    public interface IClienteService
    {
        Task<Guid> CriarAsync(ClienteDto clienteDto);
        Task AtualizarClienteAsync(Guid id, ClienteDto clienteDto);
        Task RemoverClienteAsync(Guid id);
        Task<ClienteDto> ObterPorIdAsync(Guid id);
        Task<IEnumerable<ClienteDto>> ObterTodosAsync();
    }
}
