using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using CadastroClientesAPP.API.Controllers;
using CadastroClienteAPP.Application.DTOs;
using CadastroClienteAPP.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;

namespace CadastroClienteAPP.Tests.Controllers
{
    public class ClienteControllerTests
    {
        private readonly Mock<IClienteService> _clienteServiceMock;
        private readonly ClienteController _controller;

        // Faker para gerar dados falsos
        private readonly Faker<ClienteDto> _clienteFaker;

        public ClienteControllerTests()
        {
            _clienteServiceMock = new Mock<IClienteService>();
            _controller = new ClienteController(_clienteServiceMock.Object);

            // Inicializando o Faker para gerar dados falsos
            _clienteFaker = new Faker<ClienteDto>()
                .RuleFor(c => c.Id, f => f.Random.Guid())
                .RuleFor(c => c.Nome, f => f.Name.FullName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Telefone, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.Documento, f => f.Random.Long(10000000000, 99999999999).ToString())
                .RuleFor(c => c.DataNascimento, f => f.Date.Past(30))
                .RuleFor(c => c.Endereco, f => new EnderecoDto
                {
                    Cep = f.Address.ZipCode(),
                    Logradouro = f.Address.StreetAddress(),
                    Numero = f.Random.Int(1, 100).ToString(),
                    Bairro = f.Address.County(),
                    Cidade = f.Address.City(),
                    Estado = f.Address.StateAbbr()
                });
        }

        [Fact]
        public async Task ListarTodos_DeveRetornarOkComClientes()
        {
            // Arrange
            var clientes = new List<ClienteDto>
            {
                _clienteFaker.Generate(),
                _clienteFaker.Generate()
            };

            _clienteServiceMock.Setup(x => x.ObterTodosAsync()).ReturnsAsync(clientes);

            // Act
            var resultado = await _controller.ListarTodos();

            // Assert
            var okResult = resultado as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().BeEquivalentTo(clientes);
        }

        [Fact]
        public async Task GetPorId_DeveRetornarOk_QuandoEncontrado()
        {
            var cliente = _clienteFaker.Generate();

            _clienteServiceMock.Setup(x => x.ObterPorIdAsync(cliente.Id)).ReturnsAsync(cliente);

            var resultado = await _controller.GetPorId(cliente.Id);

            var okResult = resultado as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.Value.Should().Be(cliente);
        }

        [Fact]
        public async Task GetPorId_DeveRetornarNotFound_QuandoNaoEncontrado()
        {
            _clienteServiceMock.Setup(x => x.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((ClienteDto)null!);

            var resultado = await _controller.GetPorId(Guid.NewGuid());

            resultado.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Criar_DeveRetornarCreatedAtAction()
        {
            var cliente = _clienteFaker.Generate();
            var novoId = Guid.NewGuid();

            _clienteServiceMock.Setup(x => x.CriarAsync(cliente)).ReturnsAsync(novoId);

            var resultado = await _controller.Criar(cliente);

            var createdResult = resultado as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult!.RouteValues["id"].Should().Be(novoId);
        }

        [Fact]
        public async Task Atualizar_DeveRetornarNoContent()
        {
            var cliente = _clienteFaker.Generate();
            var id = Guid.NewGuid();

            _clienteServiceMock.Setup(x => x.AtualizarClienteAsync(id, cliente)).Returns(Task.CompletedTask);

            var resultado = await _controller.Atualizar(id, cliente);

            resultado.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Deletar_DeveRetornarNoContent()
        {
            var id = Guid.NewGuid();

            _clienteServiceMock.Setup(x => x.RemoverClienteAsync(id)).Returns(Task.CompletedTask);

            var resultado = await _controller.Deletar(id);

            resultado.Should().BeOfType<NoContentResult>();
        }
    }
}
