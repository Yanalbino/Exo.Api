using Exo.WebApi.Models;
using Exo.WebApi.DTOs;
using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Exo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjetosController : ControllerBase
    {
        private readonly ProjetoRepository _projetoRepository;
        
        public ProjetosController(ProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ListarAsync()
        {
            return Ok(await _projetoRepository.ListarAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarAsync(ProjetoDTO projetoDTO)
        {
            Projeto projeto = new Projeto
            {
                NomeDoProjeto = projetoDTO.NomeDoProjeto,
                Area = projetoDTO.Area,
                Status = projetoDTO.Status
            };
            
            await _projetoRepository.CadastrarAsync(projeto);
            return StatusCode(201);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorIdAsync(int id)
        {
            Projeto projeto = await _projetoRepository.BuscarPorIdAsync(id);
            if (projeto == null)
            {
                return NotFound();
            }
            return Ok(projeto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarAsync(int id, ProjetoDTO projetoDTO)
        {
            Projeto projeto = new Projeto
            {
                NomeDoProjeto = projetoDTO.NomeDoProjeto,
                Area = projetoDTO.Area,
                Status = projetoDTO.Status
            };
            
            await _projetoRepository.AtualizarAsync(id, projeto);
            return StatusCode(204);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarAsync(int id)
        {
            try
            {
                await _projetoRepository.DeletarAsync(id);
                return StatusCode(204);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
