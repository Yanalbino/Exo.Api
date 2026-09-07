using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Exo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuariosController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ListarAsync()
        {
            return Ok(await _usuarioRepository.ListarAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarAsync(Usuario usuario)
        {
            await _usuarioRepository.CadastrarAsync(usuario);
            return StatusCode(201);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorIdAsync(int id)
        {
            Usuario usuario = await _usuarioRepository.BuscarPorIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarAsync(int id)
        {
            try
            {
                await _usuarioRepository.DeletarAsync(id);
                return StatusCode(204);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(Usuario usuarioLogin)
        {
            Usuario usuarioBuscado = await _usuarioRepository.LoginAsync(usuarioLogin.Email, usuarioLogin.Senha);
            
            if (usuarioBuscado == null)
            {
                return Unauthorized("E-mail ou senha inválidos.");
            }

            // TODO: Aqui vamos gerar o Token JWT em breve!
            return Ok(usuarioBuscado);
        }
    }
}

