using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CadastroClienteAPP.Domain.Entities;
using CadastroClienteAPP.Domain.Interfaces;
using CadastroClienteAPP.Infrastructure.Context;

namespace CadastroClienteAPP.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
            => await _context.Clientes.Include(c => c.Endereco).ToListAsync();

        public async Task<Cliente?> GetByIdAsync(Guid id)
            => await _context.Clientes.Include(c => c.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<bool> DocumentoExiste(string documento)
            => await _context.Clientes.AnyAsync(c => c.Documento == documento);

        public async Task<bool> EmailExiste(string email)
            => await _context.Clientes.AnyAsync(c => c.Email == email);

        public async Task AddAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente is null) return;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }

}
