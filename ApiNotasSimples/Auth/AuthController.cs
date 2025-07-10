using ApiCadastroClientes.Models.DTO;
using ApiCadastroClientes.Services;
using ApiNotasSimples.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiCadastroClientes.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Cadastro")]
        public async Task<IActionResult> Cadastro([FromBody] UsuarioDTO cadastro)
        {
            if (cadastro == null || !ModelState.IsValid)
                return BadRequest("Dados inválidos.");

            try
            {
                var usuario = await _authService.Cadastrar(cadastro);
                return Ok(new
                {
                    usuario.Id,
                    usuario.Nome,
                    usuario.Email
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao cadastrar: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var usuarioTask = _authService.Logar(login);
            var usuario = await usuarioTask; 

            if (usuario == null)
                return Unauthorized("Email ou senha incorretos.");
                    
            return Ok(new
            {
                usuario.Id,
                usuario.Nome,
                usuario.Email
            });
        }
    }
}