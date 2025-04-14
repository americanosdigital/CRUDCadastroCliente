using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CadastroClienteAPP.Application.DTOs;
using CadastroClienteAPP.Application.Interfaces;
using CadastroClienteAPP.Domain.Entities;
using CadastroClienteAPP.Domain.Interfaces;

namespace CadastroClienteAPP.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repo;

        public ClienteService(IClienteRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ClienteDto>> ObterTodosAsync()
        {
            var clientes = await _repo.GetAllAsync();
            return clientes.Select(MapearParaDto);
        }

        public async Task<ClienteDto> ObterPorIdAsync(Guid id)
        {
            var cliente = await _repo.GetByIdAsync(id);
            return cliente is null ? null : MapearParaDto(cliente);
        }

        public async Task<Guid> CriarAsync(ClienteDto dto)
        {
            if (await _repo.DocumentoExiste(dto.Documento))
                throw new Exception("Documento já cadastrado.");

            if (await _repo.EmailExiste(dto.Email))
                throw new Exception("Email já cadastrado.");

            if (dto.TipoPessoa == "F" && CalcularIdade(dto.DataNascimento) < 18)
                throw new Exception("Pessoa física deve ter pelo menos 18 anos.");

            if (dto.TipoPessoa == "J" && string.IsNullOrEmpty(dto.InscricaoEstadual) && !dto.Isento)
                throw new Exception("Informe a IE ou marque como isento.");

            var cliente = new Cliente
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome,
                Documento = dto.Documento,
                TipoPessoa = dto.TipoPessoa,
                DataNascimento = dto.DataNascimento,
                Telefone = dto.Telefone,
                Email = dto.Email,
                InscricaoEstadual = dto.InscricaoEstadual,
                Isento = dto.Isento,
                Endereco = new Endereco
                {
                    Id = Guid.NewGuid(),
                    Cep = dto.Endereco.Cep,
                    Logradouro = dto.Endereco.Logradouro,
                    Numero = dto.Endereco.Numero,
                    Bairro = dto.Endereco.Bairro,
                    Cidade = dto.Endereco.Cidade,
                    Estado = dto.Endereco.Estado
                }
            };

            await _repo.AddAsync(cliente);
            return cliente.Id;
        }

        public async Task AtualizarClienteAsync(Guid id, ClienteDto dto)
        {
            var cliente = await _repo.GetByIdAsync(id);
            if (cliente == null)
                throw new Exception("Cliente não encontrado.");

            cliente.Nome = dto.Nome;
            cliente.Documento = dto.Documento;
            cliente.TipoPessoa = dto.TipoPessoa;
            cliente.DataNascimento = dto.DataNascimento;
            cliente.Telefone = dto.Telefone;
            cliente.Email = dto.Email;
            cliente.InscricaoEstadual = dto.InscricaoEstadual;
            cliente.Isento = dto.Isento;

            cliente.Endereco = new Endereco
            {
                Id = cliente.Endereco?.Id ?? Guid.NewGuid(),
                Cep = dto.Endereco.Cep,
                Logradouro = dto.Endereco.Logradouro,
                Numero = dto.Endereco.Numero,
                Bairro = dto.Endereco.Bairro,
                Cidade = dto.Endereco.Cidade,
                Estado = dto.Endereco.Estado
            };

            await _repo.UpdateAsync(cliente);
        }

        public async Task RemoverClienteAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }

        private int CalcularIdade(DateTime nascimento)
        {
            var hoje = DateTime.Today;
            var idade = hoje.Year - nascimento.Year;
            if (nascimento.Date > hoje.AddYears(-idade)) idade--;
            return idade;
        }

        private ClienteDto MapearParaDto(Cliente cliente)
        {
            return new ClienteDto
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Documento = cliente.Documento,
                TipoPessoa = cliente.TipoPessoa,
                DataNascimento = cliente.DataNascimento,
                Telefone = cliente.Telefone,
                Email = cliente.Email,
                InscricaoEstadual = cliente.InscricaoEstadual,
                Isento = cliente.Isento,
                Endereco = new EnderecoDto
                {
                    Cep = cliente.Endereco?.Cep,
                    Logradouro = cliente.Endereco?.Logradouro,
                    Numero = cliente.Endereco?.Numero,
                    Bairro = cliente.Endereco?.Bairro,
                    Cidade = cliente.Endereco?.Cidade,
                    Estado = cliente.Endereco?.Estado
                }
            };
        }
    }

}
