using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Exo.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(Usuario usuarioLogin)
        {
            Usuario usuarioBuscado = await _usuarioRepository.LoginAsync(usuarioLogin.Email, usuarioLogin.Senha);
            
            if (usuarioBuscado == null)
            {
                return Unauthorized("E-mail ou senha inválidos.");
            }

            var issuer = "exoapi.webapi";
            var audience = "exoapi.webapi";
            // A chave precisa ter no mínimo 32 caracteres (256 bits)
            var key = "exoapichave-autenticacao-1234567";

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, usuarioBuscado.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);

            return Ok(new { token = stringToken });
        }
    }
}
