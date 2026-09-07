using Exo.WebApi.Contexts;
using Exo.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Exo.WebApi.Repositories
{
    public class ProjetoRepository
    {
        private readonly ExoContext _context;
        public ProjetoRepository(ExoContext context)
        {
            _context = context;
        }

        public async Task<List<Projeto>> ListarAsync()
        {
            return await _context.Projetos.ToListAsync();
        }

        public async Task CadastrarAsync(Projeto projeto)
        {
            await _context.Projetos.AddAsync(projeto);
            await _context.SaveChangesAsync();
        }

        public async Task<Projeto> BuscarPorIdAsync(int id)
        {
            return await _context.Projetos.FindAsync(id);
        }

        public async Task AtualizarAsync(int id, Projeto projetoAtualizado)
        {
            Projeto projetoBuscado = await _context.Projetos.FindAsync(id);
            if (projetoBuscado != null)
            {
                projetoBuscado.NomeDoProjeto = projetoAtualizado.NomeDoProjeto;
                projetoBuscado.Area = projetoAtualizado.Area;
                projetoBuscado.Status = projetoAtualizado.Status;
                _context.Projetos.Update(projetoBuscado);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletarAsync(int id)
        {
            Projeto projetoBuscado = await _context.Projetos.FindAsync(id);
            if (projetoBuscado != null)
            {
                _context.Projetos.Remove(projetoBuscado);
                await _context.SaveChangesAsync();
            }
        }
    }
}